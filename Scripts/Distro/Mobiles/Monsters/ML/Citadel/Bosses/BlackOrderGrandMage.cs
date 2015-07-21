using System;
using System.Collections;
using Server.Items;
using Server.Engines.MLQuests;

namespace Server.Mobiles
{
	[CorpseName( "a black order mage corpse" )]
	public class BlackOrderGrandMage : BaseCreature
	{
		[Constructable]
		public BlackOrderGrandMage()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Black Order Grand Mage";
			Title = "of the Dragons's Flame Sect";
			Race = Utility.RandomBool() ? Race.Human : Race.Elf;
			Body = Race == Race.Elf ? 605 : 400;
			Hue = Utility.RandomSkinHue();

			Utility.AssignRandomHair( this );

			if ( Utility.RandomBool() )
				Utility.AssignRandomFacialHair( this, HairHue );

			SetStr( 325, 375 );
			SetDex( 290, 310 );
			SetInt( 485, 505 );

			SetHits( 1900, 2100 );

			SetDamage( 16, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 70 );
			SetResistance( ResistanceType.Fire, 55, 75 );
			SetResistance( ResistanceType.Cold, 35, 55 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 75 );

			Fame = 15000;
			Karma = -15000;

			SetSkill( SkillName.MagicResist, 120.0, 130.0 );
			SetSkill( SkillName.Tactics, 115.0, 130.0 );
			SetSkill( SkillName.Wrestling, 95.0, 120.0 );
			SetSkill( SkillName.Anatomy, 105.0, 120.0 );
			SetSkill( SkillName.Magery, 120.0, 130.0 );
			SetSkill( SkillName.EvalInt, 120.0, 130.0 );

			/* Equip */
			Item item = null;

			AddItem( new Waraji() );

			item = new FancyShirt();
			item.Hue = 1309;
			AddItem( item );

			item = new Kasa();
			item.Hue = 1309;
			AddItem( item );

			item = new Hakama();
			item.Hue = 1309;
			AddItem( item );
		}

		protected override void OnAfterDeath( Container c )
		{
			c.DropItem( new DragonFlameBadge() );

			if ( Utility.RandomBool() )
				c.DropItem( new DragonFlameKey() );

			base.OnAfterDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public BlackOrderGrandMage( Serial serial )
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