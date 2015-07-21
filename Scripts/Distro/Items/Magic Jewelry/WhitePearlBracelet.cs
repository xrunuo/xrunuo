using System;
using Server;

namespace Server.Items
{
	public class WhitePearlBracelet : SilverBracelet
	{
		public override int LabelNumber { get { return 1073456; } } // White Pearl Bracelet

		[Constructable]
		public WhitePearlBracelet()
		{
			Weight = 1.0;

			Attributes.NightSight = 1;
			int num_attributes = Utility.RandomMinMax( 2, 5 );
			BaseRunicTool.ApplyAttributesTo( this, num_attributes, 1, 100 );

			if ( Utility.Random( 100 ) < 50 && num_attributes < 5 )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0:
						Attributes.CastSpeed += 1;
						if ( Attributes.CastSpeed > 1 ) Attributes.CastSpeed = 1;
						break;
					case 1:
						Attributes.CastRecovery += 2;
						if ( Attributes.CastRecovery > 3 ) Attributes.CastRecovery = 3;
						break;
					case 2:
						Attributes.LowerRegCost += 10;
						if ( Attributes.LowerRegCost > 20 ) Attributes.LowerRegCost = 20;
						break;
				}
			}
		}


		public WhitePearlBracelet( Serial serial )
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