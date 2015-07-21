using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseKeyFragment : TransientItem
	{
		public BaseKeyFragment( int hue )
			: base( 0x2002, TimeSpan.FromHours( 3.0 ) )
		{
			LootType = LootType.Blessed;
			Weight = 2.0;
			Hue = hue;
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			// The key fragment shatters as soon as it touches the ground!
			from.SendLocalizedMessage( 1111651 );

			Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Backpack != null && IsChildOf( from.Backpack ) )
			{
				List<BaseKeyFragment> keys = from.Backpack.FindItemsByType<BaseKeyFragment>();

				if ( keys.Count < 3 )
				{
					// You are still missing ~1_KEY~ key fragments.
					from.SendLocalizedMessage( 1114319, ( 3 - keys.Count ).ToString() );
				}
				else
				{
					foreach ( BaseKeyFragment key in keys )
						key.Delete();

					from.PlaySound( 0x506 );

					// You swiftly join the three fragments and reassemble the Tripartite Key.
					from.SendLocalizedMessage( 1111650 );

					for ( int i = 0; i < 2; i++ )
					{
						from.PlaySound( 0xFA );
						from.PlaySound( 0x5BC );
						from.PlaySound( 0x5C7 );

						Effects.SendLocationEffect( from, from.Map, 0xF6C, 30, 16, 0x47E, 4 );

						for ( int j = 0; j < 5; j++ )
						{
							Point3D loc = new Point3D( from.X, from.Y, 10 + from.Z + ( j * 20 ) );

							Effects.SendLocationEffect( loc, from.Map, 0x1AA1, 17, 16, 0x481, 4 );
							Effects.SendLocationEffect( loc, from.Map, 0x1A9F, 10, 16, 0x481, 4 );
							Effects.SendLocationEffect( loc, from.Map, 0x1A8, 25, 16, 0x47E, 4 );
						}
					}

					// You have proven thyself worthy, and may now enter the Abyss.
					from.LocalOverheadMessage( MessageType.Regular, 0x59, 1113708 );

					// The key vanishes
					from.SendLocalizedMessage( 1113709 );

					if ( from is PlayerMobile )
						( (PlayerMobile) from ).SacredQuest = true;
				}
			}
		}

		public BaseKeyFragment( Serial serial )
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