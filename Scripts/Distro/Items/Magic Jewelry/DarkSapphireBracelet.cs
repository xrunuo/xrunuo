using System;
using Server;

namespace Server.Items
{
	public class DarkSapphireBracelet : GoldBracelet
	{
		public override int LabelNumber { get { return 1073455; } } // Dark Sapphire Bracelet
		[Constructable]
		public DarkSapphireBracelet()
		{
			Weight = 1.0;
			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 0, 4 ), 1, 80 );

			if ( Utility.Random( 100 ) < 10 )
			{
				Attributes.RegenMana += 2;
			}
			else
			{
				Resistances.Cold += 10;
				if ( Resistances.Cold > 15 ) Resistances.Cold = 15;
			}
		}

		public DarkSapphireBracelet( Serial serial )
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