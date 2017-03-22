using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Lurg's corpse" )]
	public class Lurg : BaseCreature
	{
		public override int GetDeathSound() { return 0x59D; }
		public override int GetAttackSound() { return 0x59E; }
		public override int GetIdleSound() { return 0x59F; }
		public override int GetAngerSound() { return 0x5A0; }
		public override int GetHurtSound() { return 0x5A1; }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public Lurg()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lurg";
			Body = 267;
			Hue = 1109;

			SetStr( 600, 650 );
			SetDex( 150, 200 );
			SetInt( 85, 105 );

			SetHits( 3000, 3250 );

			SetDamage( 16, 19 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 55 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 55, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Anatomy, 95, 130 );
			SetSkill( SkillName.MagicResist, 70, 90 );
			SetSkill( SkillName.Tactics, 105, 125 );
			SetSkill( SkillName.Wrestling, 105, 135 );

			Fame = 5000;
			Karma = 5000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosUltraRich, 3 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

            if (3000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		// TODO: Self heal with bandages

		public override int TreasureMapLevel { get { return 3; } }
		public override bool AllureImmune{ get { return true; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public Lurg( Serial serial )
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
