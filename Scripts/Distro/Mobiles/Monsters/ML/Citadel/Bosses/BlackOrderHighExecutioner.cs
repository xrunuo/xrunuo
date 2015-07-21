using System;
using System.Collections;
using Server.Items;
using Server.Engines.MLQuests;

namespace Server.Mobiles
{
	[CorpseName( "a black order high executioner corpse" )]
	public class BlackOrderHighExecutioner : BaseCreature
	{
		[Constructable]
		public BlackOrderHighExecutioner()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Black Order High Executioner";
			Title = "of the Serpent's Fang Sect";
			Race = Utility.RandomBool() ? Race.Human : Race.Elf;
			Body = Race == Race.Elf ? 605 : 400;
			Hue = Utility.RandomSkinHue();

			Utility.AssignRandomHair( this );

			if ( Utility.RandomBool() )
				Utility.AssignRandomFacialHair( this, HairHue );

			SetStr( 325, 375 );
			SetDex( 390, 510 );
			SetInt( 285, 305 );

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

			SetSkill( SkillName.MagicResist, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 125.0, 140.0 );
			SetSkill( SkillName.Wrestling, 95.0, 120.0 );
			SetSkill( SkillName.Anatomy, 115.0, 130.0 );
			SetSkill( SkillName.Fencing, 115.0, 125.0 );

			/* Equip */
			Item item = null;

			item = new Sai();
			item.Hue = 1309;
			AddItem( item );

			item = new LeatherNinjaPants();
			item.Hue = 1309;
			AddItem( item );			

			item = new FancyShirt();
			item.Hue = 1309;
			AddItem( item );

			item = new StuddedGloves();
			item.Hue = 42;
			AddItem( item );

			item = new JinBaori();
			item.Hue = 42;
			AddItem( item );

			item = new LightPlateJingasa();
			item.Hue = 1309;
			AddItem( item );

			item = new ThighBoots();
			item.Hue = 1309;
			AddItem( item );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( from == null )
				return;

			from.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
			from.PlaySound( 0x1F1 );

			AOS.Damage( from, amount, 0, 0, 0, 0, 100 );
		}

		protected override void OnAfterDeath( Container c )
		{
			c.DropItem( new SerpentFangBadge() );

			if ( Utility.RandomBool() )
				c.DropItem( new SerpentFangKey() );

			/*if ( 0.2 > Utility.RandomDouble() )
			{
				switch ( Utility.RandomMinMax( 1, 8 ) )
				{
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8: c.DropItem( new AssassinationContractForGorrow() ); break;
				}
			}*/

			base.OnAfterDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public BlackOrderHighExecutioner( Serial serial )
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