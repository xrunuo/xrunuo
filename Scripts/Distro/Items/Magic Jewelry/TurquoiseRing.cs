using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class TurquoiseRing : GoldRing
	{
		public override int LabelNumber { get { return 1073460; } } // Turquoise Ring
		[Constructable]
		public TurquoiseRing()
		{
			Weight = 1.0;
			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 0, 4 ), 1, 90 );

			if ( Utility.Random( 100 ) < 10 )
				Attributes.WeaponSpeed = 5;
			else
				Attributes.WeaponDamage += 15;
		}

		public TurquoiseRing( Serial serial )
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