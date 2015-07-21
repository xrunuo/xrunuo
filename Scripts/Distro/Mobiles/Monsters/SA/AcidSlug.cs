using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an acid slug corpse" )]
	public class AcidSlug : BaseCreature
	{
		[Constructable]
		public AcidSlug()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "an acid slug";
			Body = 51;
			BaseSoundID = 456;

			Hue = Utility.Random( 242, 4 );

			SetStr( 207, 300 );
			SetDex( 80 );
			SetInt( 16, 20 );

			SetHits( 330, 369 );

			SetDamage( 21, 28 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10, 15 );
			SetResistance( ResistanceType.Cold, 10, 15 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 25.0 );
			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.Wrestling, 80.0 );

			Fame = 1000;
			Karma = -1000;

			if ( 0.33 > Utility.RandomDouble() )
				PackItem( new VialOfVitriol() );
			else if ( 0.33 > Utility.RandomDouble() )
				PackItem( new AcidSac() );
			
			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new CongealedSlugAcid() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
			if ( !base.CheckMovement( d, out newZ ) )
				return false;

			if ( newZ > Location.Z )
				return false;

			return true;
		}

		public AcidSlug( Serial serial )
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
