using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a forgotten servant corpse" )]
	public class ForgottenServant : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public ForgottenServant()
			: base( AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Female = Utility.RandomBool();
			Race = Race.Elf;
			Hue = Race.RandomSkinHue();

			Name = NameList.RandomName( Female ? "female" : "male" );
			Title = "the Forgotten Servant";
			Utility.AssignRandomHair( this );

			AddItem( new Shoes( 0x748 ) );
			AddItem( new ElvenPants( 0x75D ) );
			AddItem( new ElvenShirt() );
			AddItem( new BodySash( 0x65C ) );

			SetStr( 610, 660 );
			SetDex( 90, 110 );
			SetInt( 270, 320 );

			SetHits( 600, 700 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80 );
			SetResistance( ResistanceType.Fire, 80 );
			SetResistance( ResistanceType.Cold, 50 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.EvalInt, 100.0, 110.0 );
			SetSkill( SkillName.Magery, 100.0, 110.0 );
			SetSkill( SkillName.Meditation, 100.0, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0, 120.0 );
			SetSkill( SkillName.Wrestling, 90.0, 90.0 );
			SetSkill( SkillName.Mysticism, 100.0, 110.0 );
			SetSkill( SkillName.Focus, 100.0, 110.0 );

			Fame = 24000;
			Karma = -24000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.HighScrolls, 2 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 8; } }
		public override int TreasureMapLevel { get { return 5; } }		

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.15 > Utility.RandomDouble() )
				c.DropItem( new DraconicOrb() );
		}

		public ForgottenServant( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
