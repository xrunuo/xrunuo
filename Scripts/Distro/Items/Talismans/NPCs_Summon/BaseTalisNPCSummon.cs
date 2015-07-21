using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.ContextMenus;

namespace Server.Mobiles
{
	public abstract class BaseTalisNPCSummon : BaseCreature
	{
		public BaseTalisNPCSummon()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 3 )
		{
		}

		public override bool BardImmune { get { return true; } }
		public override bool Commandable { get { return false; } }
		public override double DispelDifficulty { get { return 160.0; } }

		private bool m_LastHidden;

		public virtual void StartFollow( Mobile m )
		{
			if ( m == null )
				return;

			ActiveSpeed = 0.1;
			PassiveSpeed = 0.2;

			ControlOrder = OrderType.Follow;
			ControlTarget = m;

			CurrentSpeed = 0.1;
		}

		public override void OnThink()
		{
			base.OnThink();

			Mobile master = ControlMaster;

			if ( master == null )
				return;

			if ( master.Deleted || master.Map != this.Map || !this.InRange( master.Location, 30 ) )
			{
				ControlOrder = OrderType.Stay;
				return;
			}
			else
				if ( ControlOrder != OrderType.Follow && !this.InRange( master, 1 ) )
					StartFollow( master );

			if ( m_LastHidden != master.Hidden )
				Hidden = m_LastHidden = master.Hidden;

			Mobile toAttack = null;

			if ( !Hidden )
			{
				toAttack = master.Combatant;

				if ( toAttack == this )
					toAttack = master;
				else if ( toAttack == null )
					toAttack = this.Combatant;
			}

			if ( Combatant != toAttack )
				Combatant = null;

			if ( toAttack == null )
			{
				if ( ControlTarget != master || ControlOrder != OrderType.Follow )
					StartFollow( master );
			}
			else if ( ( ControlTarget != toAttack || ControlOrder != OrderType.Attack ) && master.InRange( toAttack, 1 ) && this.InRange( master, 1 ) )
			{
				ControlTarget = toAttack;
				ControlOrder = OrderType.Attack;
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive && Controlled && from == ControlMaster && from.InRange( this, 14 ) )
				list.Add( new ReleaseEntry( from, this ) );
		}

		public virtual void BeginRelease( Mobile from )
		{
			if ( !Deleted && Controlled && from == ControlMaster && from.CheckAlive() )
				EndRelease( from );
		}

		public virtual void EndRelease( Mobile from )
		{
			if ( from == null || ( !Deleted && Controlled && from == ControlMaster && from.CheckAlive() ) )
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 2100, 3, 5042, 0 );
				PlaySound( 0x201 );
				Delete();
			}
		}

		public BaseTalisNPCSummon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();

			Delete();
		}

		private class ReleaseEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseTalisNPCSummon m_TalisSummon;

			public ReleaseEntry( Mobile from, BaseTalisNPCSummon talissummon )
				: base( 6118, 14 )
			{
				m_From = from;
				m_TalisSummon = talissummon;
			}

			public override void OnClick()
			{
				if ( !m_TalisSummon.Deleted && m_TalisSummon.Controlled && m_From == m_TalisSummon.ControlMaster && m_From.CheckAlive() )
					m_TalisSummon.BeginRelease( m_From );
			}
		}
	}
}