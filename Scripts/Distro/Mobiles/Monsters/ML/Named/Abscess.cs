using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Abscess corpse" )]
	public class Abscess : BaseCreature
	{
		[Constructable]
		public Abscess()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify
		{
			Name = "Abscess";
			Body = 265;
			BaseSoundID = 219; // TODO: Verify
			Hue = 2101; // TODO: Verify

			SetStr( 855, 860 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 130, 135 );
			SetInt( 120, 125 );

			SetHits( 7455, 7455 );

			SetDamage( 26, 31 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 10 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 75, 80 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 40, 45 );
			SetResistance( ResistanceType.Energy, 35, 35 );

			SetSkill( SkillName.Anatomy, 90, 95 );
			SetSkill( SkillName.MagicResist, 100, 105 );
			SetSkill( SkillName.Tactics, 125, 130 );
			SetSkill( SkillName.Wrestling, 130, 135 );

			Fame = 10000; // TODO: Correct
			Karma = -10000; // TODO: Correct

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );		
			
			c.DropItem( new AbscessTail() );

            if (5000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int BreathEffectSound { get { return 0x56D; } }// TODO: Correct breath
		public override int TreasureMapLevel { get { return 5; } }			

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich ); // ArcticOgreLord loot
		}

		public Abscess( Serial serial )
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