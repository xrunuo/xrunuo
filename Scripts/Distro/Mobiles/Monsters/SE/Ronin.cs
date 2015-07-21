using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ronin corpse" )]
	public class Ronin : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.RidingSwipe;
		}

		[Constructable]
		public Ronin()
			: base( AIType.AI_Samurai, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a ronin";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 125, 175 );
			SetDex( 200, 255 );
			SetInt( 85, 105 );

			SetHits( 350, 420 );

			Fame = 5000;
			Karma = -5000;

			SetDamage( 17, 25 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.MagicResist, 100.0, 110.0 );
			SetSkill( SkillName.Tactics, 85.0, 95.0 );
			SetSkill( SkillName.Wrestling, 85.0, 95.0 );
			SetSkill( SkillName.Fencing, 60.0, 80.0 );
			SetSkill( SkillName.Anatomy, 85.0, 95.0 );

			SetSkill( SkillName.Bushido, 50.0, 120.0 );

			AddItem( new Lajatang() );
			AddItem( new LeatherDo() );
			AddItem( new LightPlateJingasa() );
			AddItem( new LeatherHiroSode() );
			AddItem( new StuddedHaidate() );
			AddItem( new SamuraiTabi() );

			PackItem( new BookOfBushido() );

			Utility.AssignRandomHair( this );
		}

		protected override void OnAfterDeath( Container c )
		{
			c.DropItem( new BookOfBushido() );

			base.OnAfterDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public Ronin( Serial serial )
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
