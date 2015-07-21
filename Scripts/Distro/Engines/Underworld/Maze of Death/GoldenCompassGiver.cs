using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class GoldenCompassGiver : Item
	{
		public override int LabelNumber { get { return 1113578; } } // a golden compass

		[Constructable]
		public GoldenCompassGiver()
			: base( 0x1CB )
		{
			Weight = 0.0;
			Movable = false;
			Hue = 0x499;
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null )
			{
				if ( pm.Map != this.Map || !pm.InRange( GetWorldLocation(), 2 ) )
					pm.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				else if ( pm.Backpack.FindItemByType<GoldenCompass>() != null )
					pm.SendLocalizedMessage( 501885 ); // You already own one of those!
				else
				{
					GoldenCompass compass = new GoldenCompass();

					if ( pm.PlaceInBackpack( compass ) )
					{
						pm.SendLocalizedMessage( 1072223 ); // An item has been placed in your backpack.
						compass.SendTimeRemainingMessage( pm );
					}
					else
						compass.Delete();
				}
			}
		}

		public GoldenCompassGiver( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}