using System;
using Server.Multis;
using System.Collections.Generic;
using Server.Gumps;

namespace Server.Items
{
	public class EnhancedBandage : Bandage
	{
		public override int HealingBonus { get { return 10; } }

		[Constructable]
		public EnhancedBandage()
			: this( 1 )
		{
		}

		[Constructable]
		public EnhancedBandage( int amount )
			: base( amount )
		{
			Hue = 0x8A5;
		}

		public EnhancedBandage( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1075216 ); // these bandages have been enhanced
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}