using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Mysticism
{
	public class HealingStoneSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Healing Stone", "Kal In Mani",
				-1,
				9002,
				Reagent.Bone,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SpidersSilk
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 5.0 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 4; } }

		public HealingStoneSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				/* Conjures a Healing Stone that will instantly heal the
				 * Caster when used. The amount of damage healed by the
				 * Healing Stone is determined by the Caster's Imbuing
				 * and Mysticism skills.
				 */

				if ( m_CastCooldownList.Contains( Caster ) )
				{
					Caster.SendLocalizedMessage( 1115268 ); // You must wait a few seconds before you can summon a new healing stone.
				}
				else
				{
					var stone = FindHealingStone( Caster );

					stone?.Delete();

					var healingPoints = Math.Max( 1, (int) ( 1.25 * ( GetBaseSkill( Caster ) + GetBoostSkill( Caster ) ) ) );

					stone = new HealingStone( healingPoints );

					if ( Caster.PlaceInBackpack( stone ) )
					{
						Effects.SendPacket( Caster, Caster.Map, new GraphicalEffect( EffectType.FixedFrom, Caster.Serial, Serial.Zero, 0x3779, Caster.Location, Caster.Location, 10, 20, true, false ) );
						Effects.SendPacket( Caster, Caster.Map, new TargetParticleEffect( Caster, 0, 1, 0, 0, 0, 0x1593, 3, 0 ) );

						Caster.PlaySound( 0x650 );

						Caster.SendLocalizedMessage( 1080115 ); // A Healing Stone appears in your backpack.

						m_CastCooldownList.Add( Caster );
						Timer.DelayCall( CastCooldown, new TimerCallback( delegate { m_CastCooldownList.Remove( Caster ); } ) );
					}
					else
						stone.Delete();
				}
			}

			FinishSequence();
		}

		private static HealingStone FindHealingStone( Mobile from )
		{
			if ( from == null || from.Backpack == null )
				return null;

			if ( from.Holding is HealingStone )
				return from.Holding as HealingStone;

			return from.Backpack.FindItemByType( typeof( HealingStone ) ) as HealingStone;
		}

		public static readonly TimeSpan CastCooldown = TimeSpan.FromSeconds( 10.0 );

		private static List<Mobile> m_CastCooldownList = new List<Mobile>();

		public class HealingStone : Item
		{
			public static readonly int Cooldown = 2;
			public static readonly int Malus = 15;

			private static Dictionary<Mobile, HealingStoneContext> m_MalusTable = new Dictionary<Mobile, HealingStoneContext>();

			private int m_HealingPoints;

			[CommandProperty( AccessLevel.GameMaster )]
			public int HealingPoints
			{
				get { return m_HealingPoints; }
				set
				{
					m_HealingPoints = value;
					InvalidateProperties();

					if ( m_HealingPoints < 1 )
						Delete();
				}
			}

			public HealingStone( int healingPoints )
				: base( 0x4078 )
			{
				Weight = 1.0;
				LootType = LootType.Blessed;

				m_HealingPoints = healingPoints;
			}

			public HealingStone( Serial serial )
				: base( serial )
			{
			}

			public override void OnDoubleClick( Mobile from )
			{
				if ( !IsChildOf( from.Backpack ) )
					return;

				if ( !from.Alive )
					return;

				HealingStoneContext context = null;

				if ( m_MalusTable.ContainsKey( from ) )
					context = m_MalusTable[from];

				if ( context != null && context.UnderCooldown )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x22, 1095172 ); // You must wait a few seconds before using another Healing Stone.
				}
				else if ( from.Hits == from.HitsMax )
				{
					from.SendLocalizedMessage( 1049547, "", 0x59 ); // You are already at full health.
				}
				else if ( !BasePotion.HasFreeHand( from ) && !BasePotion.HasBalancedWeapon( from ) )
				{
					from.SendLocalizedMessage( 1080116 ); // You must have a free hand to use a Healing Stone.
				}
				else if ( MortalStrike.IsWounded( from ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x22, 1005000 ); // You cannot heal yourself in your current state.
				}
				else if ( from.Poisoned )
				{
					// TODO (SA): Healing Stone should now heal poison
				}
				else
				{
					int amountToHeal = Utility.RandomMinMax( 1, 6 ) + (int) ( ( GetBaseSkill( from ) + GetBoostSkill( from ) ) / 8.0 );

					if ( context != null )
						amountToHeal = (int) ( amountToHeal * context.Scale );

					amountToHeal = Math.Min( amountToHeal, HealingPoints );

					// TODO (SA): Arcane Empowerment should grant a bonus.

					from.Heal( amountToHeal );
					from.PlaySound( 0x202 );

					HealingPoints -= amountToHeal;

					if ( context != null )
					{
						context.Reset();
					}
					else
					{
						var t = Timer.DelayCall( TimeSpan.FromSeconds( Malus ), new TimerCallback(
							delegate
							{
								m_MalusTable.Remove( from );
							} ) );
						m_MalusTable.Add( from, new HealingStoneContext( from, t ) );
					}
				}
			}

			private class HealingStoneContext
			{
				private Mobile m_Owner;
				private DateTime m_LastHeal;
				private Timer m_Timer;

				public int Seconds
				{
					get { return (int) ( DateTime.Now - m_LastHeal ).TotalSeconds; }
				}

				public bool UnderCooldown
				{
					get { return Seconds <= Cooldown; }
				}

				public double Scale
				{
					get { return (double) Seconds / Malus; }
				}

				public void Reset()
				{
					m_LastHeal = DateTime.Now;

					m_Timer?.Stop();

					m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( Malus ), new TimerCallback(
						delegate
						{
							m_MalusTable.Remove( m_Owner );
						} ) );
				}

				public HealingStoneContext( Mobile owner, Timer t )
				{
					m_Owner = owner;
					m_LastHeal = DateTime.Now;
					m_Timer = t;
				}
			}

			public override void GetProperties( ObjectPropertyList list )
			{
				base.GetProperties( list );

				list.Add( 1115274, m_HealingPoints.ToString() ); // Healing Points: ~1_Val~
			}

			public override bool NonTransferable { get { return true; } }

			public override void HandleInvalidTransfer( Mobile from )
			{
				Delete();
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( (int) m_HealingPoints );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				if ( version >= 1 )
					m_HealingPoints = reader.ReadInt();
			}
		}
	}
}
