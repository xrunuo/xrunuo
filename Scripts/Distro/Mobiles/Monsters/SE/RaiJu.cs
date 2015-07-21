using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a rai ju corpse" )]
	public class RaiJu : BaseCreature
	{
		[Constructable]
		public RaiJu()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rai ju";
			Body = 199;

			Hue = 1153;

			BaseSoundID = 263;

			SetStr( 155, 225 );
			SetDex( 85, 135 );
			SetInt( 175, 180 );

			SetHits( 200, 280 );

			SetDamage( 12, 18 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Fire, 60 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 45, 65 );
			SetResistance( ResistanceType.Fire, 70, 85 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.Wrestling, 85.1, 95 );
			SetSkill( SkillName.Tactics, 55.1, 65 );
			SetSkill( SkillName.MagicResist, 110.1, 120 );
			SetSkill( SkillName.Anatomy, 25, 35 );

			Fame = 4500;
			Karma = -4500;

			Tamable = false;

			PackGold( 550, 650 );
			PackMagicItems( 1, 5 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 6 );
		}

		public RaiJu( Serial serial )
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