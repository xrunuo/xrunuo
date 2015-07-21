using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Ortanord's corpse" )]
	public class Ortanord : BaseCreature
	{
		public static readonly int MaxAmount = 200;

		private static int m_Amount;

		public static void Spawn( Point3D loc, Map map )
		{
			if ( MaxAmount > m_Amount )
			{
				Mobile m = new Ortanord();
				m.MoveToWorld( loc, map );
			}
		}

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public Ortanord()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Ortanord";
			Body = 165;
			BaseSoundID = 466;

			Hue = 2071;

			SetStr( 50 );
			SetDex( 50 );
			SetInt( 50 );

			SetHits( 100 );
			SetMana( 1000 );

			SetDamage( 5, 8 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Physical, 90 );
			SetResistance( ResistanceType.Fire, 90 );
			SetResistance( ResistanceType.Cold, 90 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 90 );

			SetSkill( SkillName.Anatomy, 15.1, 20.0 );
			SetSkill( SkillName.MagicResist, 100.1, 110.0 );
			SetSkill( SkillName.Tactics, 15.1, 20.0 );
			SetSkill( SkillName.Wrestling, 15.1, 20.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.Meditation, 20.0 );

			Fame = 400;
			Karma = -400;

			Team = 1;

			m_Amount++;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			m_Amount--;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public Ortanord( Serial serial )
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

			m_Amount++;
		}
	}
}
