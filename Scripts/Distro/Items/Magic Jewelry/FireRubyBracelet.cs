using System;
using Server;

namespace Server.Items
{
	public class FireRubyBracelet : GoldBracelet
	{
		public override int LabelNumber { get { return 1073454; } } // Fire Ruby Bracelet

		[Constructable]
		public FireRubyBracelet()
		{
			Weight = 1.0;
			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 0, 4 ), 1, 80 );

			if ( Utility.Random( 100 ) < 10 )
			{
				Attributes.RegenHits += 2;
			}
			else
			{
				Resistances.Fire += 10;
				if ( Resistances.Fire > 15 ) Resistances.Fire = 15;
			}
		}

		public FireRubyBracelet( Serial serial )
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