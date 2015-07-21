using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a crystal sea serpent corpse" )]
	public class CrystalSeaSerpent : BaseCreature
	{
		[Constructable]
		public CrystalSeaSerpent()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify Fight Mode
		{
			Name = "a crystal sea serpent";
			Body = 150;
			BaseSoundID = 447; // TODO: Verify
			Hue = 0x47E; // TODO: Correct

			SetStr( 255, 420 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 100, 145 );
			SetInt( 95, 155 );

			SetHits( 230, 325 );

			SetDamage( 6, 15 ); // TODO: Correct

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 45 );
			SetDamageType( ResistanceType.Energy, 45 );

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 0, 0 );
			SetResistance( ResistanceType.Cold, 70, 75 );
			SetResistance( ResistanceType.Poison, 20, 25 );
			SetResistance( ResistanceType.Energy, 65, 65 );

			SetSkill( SkillName.MagicResist, 60, 75 );
			SetSkill( SkillName.Tactics, 60, 70 );
			SetSkill( SkillName.Wrestling, 60, 70 );

			Fame = 6000; // Deep Sea Serpent
			Karma = -6000; // Deep Sea Serpent

			CanSwim = true;
			CantWalk = true;

			PackSpellweavingScroll();
		}

		public override bool HasBreath { get { return true; } } // ice breath enabled
		public override int BreathEffectHue { get { return 0x47E; } } // TODO: Correct breath
		public override int BreathFireDamage { get { return 0; } }
		public override int BreathColdDamage { get { return 100; } }

		public override int Meat { get { return 1; } }
		public override int Scales { get { return 8; } }
		public override ScaleType ScaleType { get { return ScaleType.Blue; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager ); // Deep Sea Serpent
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);
			
			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new IcyHeart() );
            if (0.2 > Utility.RandomDouble())
                c.DropItem(new CrushedCrystals ());
		}


		public CrystalSeaSerpent( Serial serial )
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
