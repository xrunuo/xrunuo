using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class PerfectEmeraldRing : GoldRing
	{
		public override int LabelNumber { get { return 1073459; } } // Perfect Emerald Ring

		[Constructable]
		public PerfectEmeraldRing()
		{
			Weight = 1.0;
			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 1, 4 ), 1, 80 );
			Resistances.Poison = 0;
			Attributes.LowerManaCost = 0;

			if ( Utility.Random( 100 ) < 10 )
			{
				Attributes.LowerManaCost += 5;
			}
			else
			{
				Resistances.Poison += 10;
			}
		}

		public PerfectEmeraldRing( Serial serial )
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