using System;
using Server;

namespace Server.Items
{
	public class EcruCitrineRing : GoldRing
	{
		public override int LabelNumber { get { return 1073457; } } // Ecru Citrine Ring

		[Constructable]
		public EcruCitrineRing()
		{
			Weight = 1.0;
			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 1, 4 ), 0, 100 );

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					Attributes.EnhancePotions = 5;
					break;
				case 1:
					Attributes.EnhancePotions = (short) 50;
					break;
				case 2:
					Attributes.BonusStr += 5;
					if ( Attributes.BonusStr > 8 ) Attributes.BonusStr = 8;
					break;

			}

		}

		public EcruCitrineRing( Serial serial )
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