using System;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a grubber corpse" )]
	public class Grubber : BaseCreature
	{
		private Timer m_RunTimer;

		[Constructable]
		public Grubber()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a grubber";
			Body = 270;

			SetStr( 15 );
			SetDex( 2000 );
			SetInt( 1000 );

			SetHits( 200 );
			SetStam( 500 );
			SetMana( 0 );

			SetDamage( 1, 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 500;
			Karma = -500;

			new DeleteTimer( this ).Start();
		}

		public override void OnCombatantChange()
		{
			if ( Combatant != null && m_RunTimer == null )
			{
				m_RunTimer = new RunTimer( this );
				m_RunTimer.Start();
			}

			base.OnCombatantChange();
		}

		public override void OnDelete()
		{
			if ( m_RunTimer != null )
			{
				m_RunTimer.Stop();
				m_RunTimer = null;
			}

			base.OnDelete();
		}

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 194;
		}

		private class RunTimer : Timer
		{
			private Grubber m_Grubber;
			private int m_Count = 0;

			public RunTimer( Grubber grubber )
				: base( TimeSpan.FromSeconds( 0.15 ), TimeSpan.FromSeconds( 0.15 ) )
			{
				m_Grubber = grubber;
			}

			protected override void OnTick()
			{
				m_Grubber.BeginFlee( TimeSpan.FromSeconds( 5.0 ) );

				if ( ( m_Count++ & 0x3 ) == 0 )
				{
					m_Grubber.Direction = (Direction) ( Utility.Random( 8 ) | 0x80 );
				}

				m_Grubber.Move( m_Grubber.Direction );
			}
		}

		private class DeleteTimer : Timer
		{
			private Grubber m_Grubber;

			public DeleteTimer( Grubber grubber )
				: base( TimeSpan.FromMinutes( 5.0 ) )
			{
				m_Grubber = grubber;
			}

			protected override void OnTick()
			{
				if ( m_Grubber.Deleted )
					Stop();

				m_Grubber.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*The grubber begins to fade into the shadows*" );

				Timer.DelayCall( TimeSpan.FromSeconds( 3.0 ),
					() =>
					{
						m_Grubber.Delete();
					} );
			}
		}

		public Grubber( Serial serial )
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

			/*int version = */
			reader.ReadInt();

			Delete();
		}
	}
}