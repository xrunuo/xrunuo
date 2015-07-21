using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage wolf's corpse" )]
	public class ClanScratchSavageWolf : BaseCreature
	{
		[Constructable]
		public ClanScratchSavageWolf()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Scratch Savage Wolf";
			Body = 23;
			BaseSoundID = 0xE5;

			Hue = 1255;

			SetStr( 165, 190 );
			SetDex( 217, 240 );
			SetInt( 64, 81 );

			SetHits( 65, 73 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 10, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 28.6, 43.7 );
			SetSkill( SkillName.Tactics, 30.3, 48.6 );
			SetSkill( SkillName.Wrestling, 32.4, 49.3 );

			Fame = 1000;
			Karma = -1000;
		}

		protected override void OnAfterDeath( Items.Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new ReflectiveWolfEye() );
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 7; } }

		public ClanScratchSavageWolf( Serial serial )
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
