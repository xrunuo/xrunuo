using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Protector : BaseCreature
	{
		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public Protector()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify Fight Mode
		{
			Name = "a Protector";
			Body = 401;
			Hue = 0x4001;
			Female = true;

			SetStr( 100, 150 ); // TODO: Correct all
			SetDex( 150, 200 );
			SetInt( 100, 150 );

			SetHits( 400, 500 );

			SetDamage( 8, 11 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 110, 120 );
			SetSkill( SkillName.Tactics, 70, 80 );
			SetSkill( SkillName.Wrestling, 90, 110 );

			Fame = 2000;
			Karma = -2000;

			HoodedShroudOfShadows hooded = new HoodedShroudOfShadows();
			hooded.Hue = Utility.RandomBool() ? 0 : 1;
			AddItem( hooded );

			PackSpellweavingScroll();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new ProtectorsEssence() );
		}


		public Protector( Serial serial )
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
		}
	}
}
