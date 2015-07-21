using System;
using System.Collections;
using Server.Items;
using Server.Engines.MLQuests;

namespace Server.Mobiles
{
	[CorpseName( "a black order thief corpse" )]
	public class BlackOrderMasterThief : BaseCreature
	{
		[Constructable]
		public BlackOrderMasterThief()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Black Order Master Thief";
			Title = "of the Tiger's Claw Sect";
			Race = Utility.RandomBool() ? Race.Human : Race.Elf;
			Hue = Race.RandomSkinHue();

			Utility.AssignRandomHair( this );

			if ( Utility.RandomBool() )
				Utility.AssignRandomHair( this, HairHue );

			SetStr( 525, 375 );
			SetDex( 290, 310 );
			SetInt( 285, 305 );

			SetHits( 1900, 2100 );

			SetDamage( 16, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 70 );
			SetResistance( ResistanceType.Fire, 55, 75 );
			SetResistance( ResistanceType.Cold, 35, 55 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 75 );

			Fame = 10000;
			Karma = -10000;

			SetSkill( SkillName.MagicResist, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 125.0, 140.0 );
			SetSkill( SkillName.Wrestling, 110.0, 130.0 );
			SetSkill( SkillName.Anatomy, 105.0, 120.0 );
			SetSkill( SkillName.Swords, 115.0, 125.0 );
			SetSkill( SkillName.Parry, 115.0, 125.0 );

			/* Equip */
			Item item = null;

			item = new Wakizashi();
			item.Hue = 1309;
			AddItem( item );

			item = new LeatherNinjaPants();
			item.Hue = 1309;
			AddItem( item );			

			item = new FancyShirt();
			item.Hue = 1309;
			AddItem( item );

			item = new StuddedGloves();
			item.Hue = 105;
			AddItem( item );

			item = new JinBaori();
			item.Hue = 105;
			AddItem( item );

			item = new LightPlateJingasa();
			item.Hue = 1309;
			AddItem( item );
		}

		protected override void OnAfterDeath( Container c )
		{
			c.DropItem( new TigerClawBadge() );

			if ( Utility.RandomBool() )
				c.DropItem( new TigerClawKey() );

			base.OnAfterDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public BlackOrderMasterThief( Serial serial )
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