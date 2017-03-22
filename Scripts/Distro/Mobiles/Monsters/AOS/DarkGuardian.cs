using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a guardian's corpse" )]
	public class DarkGuardian : BaseCreature
	{
		[Constructable]
		public DarkGuardian()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dark guardian";

			Body = 0x18;

			Hue = 0x44E;

			BaseSoundID = 0x19D;

			Tamable = false;

			SetStr( 126, 150 );
			SetDex( 101, 120 );
			SetInt( 201, 235 );

			SetHits( 153, 179 );
			SetStam( 101, 120 );
			SetMana( 201, 235 );

			SetDamage( 43, 48 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 20, 45 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 45 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 40.1, 50.0 );
			SetSkill( SkillName.Magery, 50.1, 60.0 );
			SetSkill( SkillName.Meditation, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 50.1, 70.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );

			Fame = 23000;
			Karma = -23000;

			PackGold( 400, 500 );

			for ( int i = 0; i < 2; i++ )
			{
				PackScroll( 7, 8 );
				PackNecroScroll( 1 + Utility.Random( 14 ) );
				PackNecroReg( 15, 25 );
			}

			PackMagicItems( 3, 5 );
			PackItem( new TreasureMap( Utility.Random( 1, 3 ) ) );
			PackItem( new DaemonBone( 30 ) );
		}

		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool BleedImmune { get { return true; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public DarkGuardian( Serial serial )
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
