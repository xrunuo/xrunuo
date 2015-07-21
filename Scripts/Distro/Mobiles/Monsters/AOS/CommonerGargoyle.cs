using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	public class CommonerGargoyle : BaseCreature
	{
		[Constructable]
		public CommonerGargoyle()
			: base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
		{
			SetStr( 10, 30 );
			SetDex( 10, 30 );
			SetInt( 10, 30 );

			Fame = 50;
			Karma = 50;
			Body = 0x2F6;
			Hue = RandomBrightHue() | 0x8000;
			Name = NameList.RandomName( "gargoyle vendor" );
			Blessed = true;
		}

		public virtual int RandomBrightHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return Utility.RandomList( 0x62, 0x71 );

			return Utility.RandomList( 0x03, 0x0D, 0x13, 0x1C, 0x21, 0x30, 0x37, 0x3A, 0x44, 0x59 );
		}

		public CommonerGargoyle( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}
