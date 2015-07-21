using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a giant rat corpse" )]
	public class ClanRibbonPlagueRat : BaseCreature
	{
		[Constructable]
		public ClanRibbonPlagueRat()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Clan Ribbon plague rat";
			Body = 215;
			BaseSoundID = 0x188;

			Hue = 151;

			SetStr( 35, 62 );
			SetDex( 46, 59 );
			SetInt( 18, 28 );

			SetHits( 41, 85 );
			SetMana( 0 );

			SetDamage( 4, 8 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.MagicResist, 25.4, 29.5 );
			SetSkill( SkillName.Tactics, 32.8, 37.1 );
			SetSkill( SkillName.Wrestling, 35, 41.6 );

			Fame = 300;
			Karma = -300;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 6; } }

		public ClanRibbonPlagueRat( Serial serial )
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
