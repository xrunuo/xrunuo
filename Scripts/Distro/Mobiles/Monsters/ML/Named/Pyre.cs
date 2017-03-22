using System;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "Pyres corpse" )]
	public class Pyre : BaseCreature
	{
		[Constructable]
		public Pyre()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Pyre";
			Body = 5;
			BaseSoundID = 0x2EE;
			Hue = 1161;

			SetStr( 600, 650 );
			SetDex( 350, 550 );
			SetInt( 669, 818 );

			SetHits( 1750, 2000 );

			SetDamage( 30 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 50 );

			SetResistance( ResistanceType.Physical, 65, 65 );
			SetResistance( ResistanceType.Fire, 70, 75 );
			SetResistance( ResistanceType.Poison, 30, 45 );
			SetResistance( ResistanceType.Energy, 50, 55 );

			SetSkill( SkillName.Magery, 120, 130 );
			SetSkill( SkillName.EvalInt, 100, 120 );
			SetSkill( SkillName.Meditation, 125, 130 );
			SetSkill( SkillName.MagicResist, 140, 160 );
			SetSkill( SkillName.Tactics, 110, 120 );
			SetSkill( SkillName.Wrestling, 120, 140 );
			SetSkill( SkillName.Poisoning, 120, 130 );

			Fame = 10000;
			Karma = -10000;

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new ParagonChest( this.Name, 6 ) );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public override WeaponAbility GetWeaponAbility()
		{
			if ( Utility.RandomBool() )
				return WeaponAbility.BleedAttack;
			else
				return WeaponAbility.ParalyzingBlow;
		}

		protected override void OnAfterDeath( Container c )
		{
			if ( 2000 > Utility.Random( 100000 ) )
				c.DropItem( SetItemsHelper.GetRandomSetItem() );

			base.OnAfterDeath( c );
		}

		/*public override void OnThink()
		{
			if ( DateTime.UtcNow > m_Delay )
			{
				foreach ( Mobile m in Map.GetMobilesInRange( Location, 3 ) )
				{
					if ( !m.Alive || m.AccessLevel > AccessLevel.Player || (m is BaseCreature && ( !((BaseCreature)m).Controlled || ((BaseCreature)m).IsDeadPet )) )
						continue;
					if ( m is PlayerMobile )
					{
						AOS.Damage( m, this, 5, 0, 100, 0, 0, 0 );
						NetState state = ((PlayerMobile)m).NetState;
						Packet p = new MessageLocalizedAffix( Serial, Body, MessageType.Regular, 0x3B2, 3, 1008112, Name, AffixType.Prepend, Name, "" );
						state.Send( p );
					}
					else if ( m.Hits > 5 )
						AOS.Damage( m, this, 5, 0, 100, 0, 0, 0 );
					else
						Timer.DelayCall( TimeSpan.FromSeconds( 0.1 ), new TimerStateCallback( this.DelayedDamage ), m );
					
					m.RevealingAction();
				}

				m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds( 4 );
			}
			base.OnThink();
		}*/

		/*private void DelayedDamage( object state )
		{
			AOS.Damage( (Mobile) state, null, 5, 0, 100, 0, 0, 0 );
		}*/

		public override int Meat { get { return 1; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 36; } }
		public override bool HasAura { get { return true; } }
		public override TimeSpan AuraInterval { get { return TimeSpan.FromSeconds( 10.0 ); } }
		public override int AuraFireDamage { get { return 100; } }
		public override int AuraBaseDamage { get { return 15; } }
		public override int TreasureMapLevel { get { return 5; } }

		public Pyre( Serial serial )
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
		}
	}
}
