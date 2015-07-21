using System.Collections;
using Server.Engines.Housing.Multis;
using Server.Engines.Housing.Targets;

namespace Server.Engines.Housing.Items
{
	public abstract class HouseDeed : Item
	{
		private int m_MultiID;
		private Point3D m_Offset;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MultiID
		{
			get { return m_MultiID; }
			set { m_MultiID = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}

		public HouseDeed( int id, Point3D offset )
			: base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Newbied;

			m_MultiID = id & TileData.MaxItemValue;
			m_Offset = offset;
		}

		public HouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Offset );

			writer.Write( m_MultiID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Offset = reader.ReadPoint3D();

						goto case 0;
					}
				case 0:
					{
						m_MultiID = reader.ReadInt();

						break;
					}
			}

			if ( Weight == 0.0 )
			{
				Weight = 1.0;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse( from ) )
			{
				from.SendLocalizedMessage( 501271 ); // You already own a house, you may not place another!
			}
			else
			{
				from.SendLocalizedMessage( 1010433 ); /* House placement cancellation could result in a
													   * 60 second delay in the return of your deed.
													   */

				from.Target = new HousePlacementTarget( this );
			}
		}

		public abstract BaseHouse GetHouse( Mobile owner );
		public abstract Rectangle2D[] Area { get; }

		public void OnPlacement( Mobile from, Point3D p )
		{
			if ( Deleted )
			{
				return;
			}

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse( from ) )
			{
				from.SendLocalizedMessage( 501271 ); // You already own a house, you may not place another!
			}
			else
			{
				ArrayList toMove;
				Point3D center = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
				HousePlacementResult res = HousePlacement.Check( from, m_MultiID, center, out toMove );

				switch ( res )
				{
					case HousePlacementResult.Valid:
						{
							BaseHouse house = GetHouse( from );
							house.MoveToWorld( center, from.Map );
							Delete();

							for ( int i = 0; i < toMove.Count; ++i )
							{
								object o = toMove[i];

								if ( o is Mobile )
									( (Mobile) o ).Location = house.BanLocation;
								else if ( o is Item )
									( (Item) o ).Location = house.BanLocation;
							}

							break;
						}
					case HousePlacementResult.BadItem:
					case HousePlacementResult.BadLand:
					case HousePlacementResult.BadStatic:
					case HousePlacementResult.BadRegionHidden:
						{
							from.SendLocalizedMessage( 1043287 ); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
							break;
						}
					case HousePlacementResult.NoSurface:
						{
							from.SendMessage( "The house could not be created here.  Part of the foundation would not be on any surface." );
							break;
						}
					case HousePlacementResult.BadRegion:
						{
							from.SendLocalizedMessage( 501265 ); // Housing cannot be created in this area.
							break;
						}
					case HousePlacementResult.BadRegionTemp:
						{
							from.SendLocalizedMessage( 501270 ); // Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
							break;
						}
				}
			}
		}
	}
}