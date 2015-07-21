using System;
using Server;

namespace Server.Items
{
	public class BlueDiamondRing : GoldRing
	{
		public override int LabelNumber { get { return 1073458; } } // Blue Diamond Ring

		[Constructable]
		public BlueDiamondRing()
		{

			BaseRunicTool.ApplyAttributesTo( this, Utility.RandomMinMax( 1, 4 ), 1, 100 );
			Attributes.LowerRegCost = 0;

			switch ( Utility.Random( 4 ) )
			{
				case 0: Attributes.LowerRegCost += 10;
					break;
				case 1:
					Attributes.CastSpeed += 1;
					if ( Attributes.CastSpeed > 1 ) Attributes.CastSpeed = 1;
					break;
				case 2:
					Attributes.CastRecovery += 2;
					if ( Attributes.CastRecovery > 3 ) Attributes.CastRecovery = 3;
					break;
				case 3:
					Attributes.SpellDamage += 5;
					if ( Attributes.SpellDamage > 12 ) Attributes.SpellDamage = 12;
					break;

			}
		}

		public BlueDiamondRing( Serial serial )
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