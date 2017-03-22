using System;
using Server;
using Server.Engines.Loyalty;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a green goblin alchemist corpse" )]
	public class EnslavedGoblinAlchemist : GreenGoblinAlchemist
	{
		[Constructable]
		public EnslavedGoblinAlchemist()
		{
			Name = "enslaved goblin alchemist";
			Body = 723;

			SetStr( 268, 301 );
			SetDex( 61, 68 );
			SetInt( 102, 123 );

			SetHits( 169, 190 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 126.6, 128.1 );
			SetSkill( SkillName.Tactics, 82.9, 86.4 );
			SetSkill( SkillName.Wrestling, 102.3, 103.8 );

			Fame = 15000;
			Karma = -15000;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 8; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public EnslavedGoblinAlchemist( Serial serial )
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
