using System;
using System.Collections;
using Server;
using Server.Items;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Mobiles
{
	public abstract class BaseBardCreature : BaseCreature
	{
		private DateTime m_NextAbilityTime;

		public virtual int SuccessSound { get { return 0x5B8; } }
		public virtual int FailureSound { get { return 0x5B7; } }

		public virtual bool UsesPeacemaking { get { return true; } }
		public virtual bool UsesDiscordance { get { return true; } }
		public virtual bool UsesProvocation { get { return true; } }

		public BaseBardCreature( AIType ai, FightMode mode, int irp, int irf, double das, double dps )
			: base( ai, mode, irp, irf, das, dps )
		{
		}

		public BaseBardCreature( Serial serial )
			: base( serial )
		{
		}

		public override void OnThink()
		{
			if ( m_NextAbilityTime < DateTime.UtcNow )
			{
				if ( Combatant != null && !BardPacified )
				{
					if ( UsesDiscordance && IsDiscordable( Combatant ) )
						DoDiscordance();
					else if ( UsesPeacemaking && IsCalmable( Combatant ) )
						DoPeacemaking();
					else if ( UsesProvocation )
						DoProvocation();

					m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( 5.0 );
				}
			}

			base.OnThink();
		}

		public bool IsTargeteable( Mobile m )
		{
			return CanBeHarmful( m, false ) && m.Alive && m.Map == this.Map && this.InLOS( m );
		}

		#region Provocation
		public bool IsProvokable( BaseCreature bc )
		{
			return IsTargeteable( bc ) && !bc.BardImmune && !bc.BardPacified && !bc.IsDeadBondedPet && bc.InRange( this, BaseInstrument.GetBardRange( this, SkillName.Provocation ) );
		}

		public void DoProvocation()
		{
			DebugSay( "I Provoke!" );

			BaseCreature target = null;

			foreach ( Mobile m in this.GetMobilesInRange( BaseInstrument.GetBardRange( this, SkillName.Provocation ) ) )
			{
				if ( m is BaseCreature )
				{
					if ( m != Combatant && m != this && IsProvokable( m as BaseCreature ) )
					{
						target = m as BaseCreature;
						break;
					}
				}
			}

			if ( target != null )
			{
				double diff = ( ( BaseInstrument.GetBaseDifficulty( target ) + BaseInstrument.GetBaseDifficulty( Combatant ) ) * 0.5 ) - 5.0;
				double music = this.Skills[SkillName.Musicianship].Value;

				if ( music > 100.0 )
					diff -= ( music - 100.0 ) * 0.5;

				if ( !this.CheckTargetSkill( SkillName.Provocation, Combatant, diff - 25.0, diff + 25.0 ) )
				{
					this.PlaySound( FailureSound );
				}
				else
				{
					this.PlaySound( SuccessSound );
					target.Provoke( this, Combatant, true );
				}
			}
			else
			{
				DebugSay( "Nothing to provoke." );
			}
		}
		#endregion

		#region Discordance
		public bool IsDiscordable( Mobile m )
		{
			return IsTargeteable( m ) && !Discordance.IsDiscorded( m ) && m.InRange( this, BaseInstrument.GetBardRange( this, SkillName.Discordance ) );
		}

		public void DoDiscordance()
		{
			DebugSay( "I Discord!" );

			TimeSpan len = TimeSpan.FromSeconds( this.Skills[SkillName.Discordance].Value * 2 );

			double diff = BaseInstrument.GetBaseDifficulty( Combatant ) - 10.0;
			double music = this.Skills[SkillName.Musicianship].Value;

			if ( music > 100.0 )
				diff -= ( music - 100.0 ) * 0.5;

			if ( this.CheckTargetSkill( SkillName.Discordance, Combatant, diff - 25.0, diff + 25.0 ) )
			{
				DebugSay( "Discord success!" );

				ArrayList mods = new ArrayList();
				int effect;
				double scalar;

				double discord = this.Skills[SkillName.Discordance].Value;

				if ( discord > 100.0 )
					effect = -20 + (int) ( ( discord - 100.0 ) / -2.5 );
				else
					effect = (int) ( discord / -5.0 );

				if ( BaseInstrument.GetBaseDifficulty( Combatant ) >= 160.0 )
					effect /= 2;

				scalar = effect * 0.01;

				mods.Add( new ResistanceMod( ResistanceType.Physical, effect ) );
				mods.Add( new ResistanceMod( ResistanceType.Fire, effect ) );
				mods.Add( new ResistanceMod( ResistanceType.Cold, effect ) );
				mods.Add( new ResistanceMod( ResistanceType.Poison, effect ) );
				mods.Add( new ResistanceMod( ResistanceType.Energy, effect ) );

				for ( int i = 0; i < Combatant.Skills.Length; ++i )
				{
					if ( Combatant.Skills[i].Value > 0 )
						mods.Add( new DefaultSkillMod( (SkillName) i, true, Combatant.Skills[i].Value * scalar ) );
				}

				Discordance.DiscordanceInfo info = new Discordance.DiscordanceInfo( this, Combatant, len, Math.Abs( effect ), mods );
				info.m_Timer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 1.25 ), new TimerStateCallback( ProcessDiscordance ), info );

				Discordance.m_Table[Combatant] = info;

				this.PlaySound( SuccessSound );
			}
			else
			{
				DebugSay( "Discord failure :(" );
				this.PlaySound( FailureSound );
			}
		}

		private static void ProcessDiscordance( object state )
		{
			Discordance.DiscordanceInfo info = (Discordance.DiscordanceInfo) state;
			Mobile from = info.m_From;
			Mobile targ = info.m_Creature;

			if ( DateTime.UtcNow >= info.m_EndTime || targ.Deleted || from.Map != targ.Map || targ.GetDistanceToSqrt( from ) > 16 )
			{
				if ( info.m_Timer != null )
					info.m_Timer.Stop();

				info.Clear();
				Discordance.m_Table.Remove( targ );
			}
			else
			{
				targ.FixedEffect( 0x376A, 1, 32 );
			}
		}
		#endregion

		#region Peacemaking
		public bool IsCalmable( Mobile m )
		{
			bool inrange = m.InRange( this, BaseInstrument.GetBardRange( this, SkillName.Peacemaking ) );

			if ( m is BaseCreature )
			{
				BaseCreature bc = m as BaseCreature;
				return IsTargeteable( bc ) && !bc.BardImmune && !bc.BardPacified && inrange;
			}
			else
			{
				return !IsCalmed( m ) && IsTargeteable( m ) && inrange;
			}
		}

		private static ArrayList m_List = new ArrayList();

		public static bool IsCalmed( Mobile m )
		{
			return m_List.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_List.Remove( m );
		}

		public void DoPeacemaking()
		{
			DebugSay( "I Peace!" );

			double diff = BaseInstrument.GetBaseDifficulty( Combatant ) - 10.0;
			double music = this.Skills[SkillName.Musicianship].Value;

			if ( music > 100.0 )
				diff -= ( music - 100.0 ) * 0.5;

			if ( !this.CheckTargetSkill( SkillName.Peacemaking, Combatant, diff - 25.0, diff + 25.0 ) )
			{
				DebugSay( "Peace failure :(" );
				this.PlaySound( FailureSound );
			}
			else
			{
				DebugSay( "Peace success!" );
				this.PlaySound( SuccessSound );

				double seconds = 100 - ( diff / 1.5 );
				Utility.FixMinMax( ref seconds, 10, 120 );

				Combatant.Combatant = null;
				Combatant.Warmode = false;

				if ( Combatant is BaseCreature )
				{
					DebugSay( "I have peaced a creature!" );

					BaseCreature bc = (BaseCreature) Combatant;

					bc.Pacify( this, DateTime.UtcNow + TimeSpan.FromSeconds( seconds ) );
				}
				else
				{
					DebugSay( "I have paced a player!" );
					// TODO: es correcto este mensaje?
					Combatant.SendLocalizedMessage( 500616 ); // You hear lovely music, and forget to continue battling!

					m_List.Add( Combatant );

					Timer.DelayCall( TimeSpan.FromSeconds( seconds ), new TimerStateCallback( Expire_Callback ), Combatant );
				}
			}
		}
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}