using System;
using Server;
using Server.Engines.Housing;
using Server.Network;
using Server.Regions;
using Server.Multis;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
	public enum DecorateCommand
	{
		None,
		Turn,
		Up,
		Down
	}

	public class InteriorDecorator : Item
	{
		private DecorateCommand m_Command;

		[CommandProperty( AccessLevel.GameMaster )]
		public DecorateCommand Command
		{
			get { return m_Command; }
			set
			{
				m_Command = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public InteriorDecorator()
			: base( 0xFC1 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			m_Command = DecorateCommand.Turn;
		}

		public override int LabelNumber { get { return 1041280; } } // an interior decorator

		public InteriorDecorator( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Command != DecorateCommand.None )
			{
				list.Add( 1018322 + (int) m_Command ); // Turn/Up/Down
			}
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( !CheckUse( this, from ) )
			{
				return;
			}

			from.SendGump( new InternalGump( this ) );
			from.Target = new InternalTarget( this );
		}

		public static bool InHouse( Mobile from )
		{
			IHouse house = HousingHelper.FindHouseAt( from );

			return ( house != null && house.IsCoOwner( from ) );
		}

		public static bool CheckUse( InteriorDecorator tool, Mobile from )
		{
			if ( !InHouse( from ) )
				from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
			else
				return true;

			return false;
		}

		private class InternalGump : Gump
		{
			public override int TypeID { get { return 0x28C; } }

			private InteriorDecorator m_Decorator;

			public InternalGump( InteriorDecorator decorator )
				: base( 150, 50 )
			{
				m_Decorator = decorator;

				AddBackground( 0, 0, 170, 200, 2600 );

				AddPage( 0 );

				if ( m_Decorator.Command == DecorateCommand.Turn )
					AddButton( 40, 36, 0x869, 0x86A, 1, GumpButtonType.Reply, 0 );
				else
					AddButton( 40, 36, 0x868, 0x869, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 41, 100, 20, 1018323, 0x0, false, false ); // Turn

				if ( m_Decorator.Command == DecorateCommand.Up )
					AddButton( 40, 86, 0x869, 0x86A, 2, GumpButtonType.Reply, 0 );
				else
					AddButton( 40, 86, 0x868, 0x869, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 91, 100, 20, 1018324, 0x0, false, false ); // Up

				if ( m_Decorator.Command == DecorateCommand.Down )
					AddButton( 40, 136, 0x869, 0x86A, 3, GumpButtonType.Reply, 0 );
				else
					AddButton( 40, 136, 0x868, 0x869, 3, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 141, 100, 20, 1018325, 0x0, false, false ); // Down
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				DecorateCommand command = DecorateCommand.None;

				switch ( info.ButtonID )
				{
					case 1:
						command = DecorateCommand.Turn;
						break;
					case 2:
						command = DecorateCommand.Up;
						break;
					case 3:
						command = DecorateCommand.Down;
						break;
					default:
						sender.Mobile.Target = null;
						return;
				}

				if ( command != DecorateCommand.None )
				{
					m_Decorator.Command = command;
					sender.Mobile.Target = new InternalTarget( m_Decorator );
					sender.Mobile.SendGump( new InternalGump( m_Decorator ) );
				}
			}
		}

		private class InternalTarget : Target
		{
			private InteriorDecorator m_Decorator;

			public InternalTarget( InteriorDecorator decorator )
				: base( -1, false, TargetFlags.None )
			{
				CheckLOS = false;

				m_Decorator = decorator;
			}

			//protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			//{
			//	from.CloseGump( typeof( InternalGump ) );
			//}

			protected override void OnTargetNotAccessible( Mobile from, object targeted )
			{
				OnTarget( from, targeted );
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted == m_Decorator )
				{
					m_Decorator.Command = DecorateCommand.None;
					from.SendGump( new InternalGump( m_Decorator ) );
				}
				else if ( targeted is Item && InteriorDecorator.CheckUse( m_Decorator, from ) )
				{
					var house = HousingHelper.FindHouseAt( from );
					Item item = (Item) targeted;

					if ( house == null || !house.IsCoOwner( from ) )
					{
						from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
					}
					else if ( item.Parent != null || !house.IsInside( item ) )
					{
						from.SendLocalizedMessage( 1042270 ); // That is not in your house.
					}
					else if ( !house.IsLockedDown( item ) && !house.IsSecure( item ) )
					{
						from.SendLocalizedMessage( 1042271 ); // That is not locked down.
					}
					else if ( item is VendorRentalContract )
					{
						from.SendLocalizedMessage( 1062491 ); // You cannot use the house decorator on that object.
					}
					else if ( item.TotalWeight + item.PileWeight > 100 )
					{
						from.SendLocalizedMessage( 1042272 ); // That is too heavy.
					}
					else
					{
						switch ( m_Decorator.Command )
						{
							case DecorateCommand.Up:
								Up( item, from );
								break;
							case DecorateCommand.Down:
								Down( item, from );
								break;
							case DecorateCommand.Turn:
								Turn( item, from );
								break;
						}
					}
				}
			}

			private static void Turn( Item item, Mobile from )
			{
				FlipableAttribute[] attributes = (FlipableAttribute[]) item.GetType().GetCustomAttributes( typeof( FlipableAttribute ), false );

				Item addon = null;

				if ( attributes.Length > 0 )
					attributes[0].Flip( item );

				#region Heritage Items
				else if ( item is AddonComponent )
				{
					addon = ( (AddonComponent) item ).Addon;
				}
				else if ( item is AddonContainerComponent )
				{
					addon = ( (AddonContainerComponent) item ).Addon;
				}
				else if ( item is BaseAddonContainer )
				{
					addon = (BaseAddonContainer) item;
				}
				#endregion

				else
					from.SendLocalizedMessage( 1042273 ); // You cannot turn that.


			}

			private static void Up( Item item, Mobile from )
			{
				int floorZ = GetFloorZ( item );

				if ( floorZ > int.MinValue && item.Z < ( floorZ + 15 ) ) // Confirmed : no height checks here
				{
					item.Location = new Point3D( item.Location, item.Z + 1 );
				}
				else
				{
					from.SendLocalizedMessage( 1042274 ); // You cannot raise it up any higher.
				}
			}

			private static void Down( Item item, Mobile from )
			{
				int floorZ = GetFloorZ( item );

				if ( floorZ > int.MinValue && item.Z > GetFloorZ( item ) )
				{
					item.Location = new Point3D( item.Location, item.Z - 1 );
				}
				else
				{
					from.SendLocalizedMessage( 1042275 ); // You cannot lower it down any further.
				}
			}

			private static int GetFloorZ( Item item )
			{
				Map map = item.Map;

				if ( map == null )
				{
					return int.MinValue;
				}

				Tile[] tiles = map.Tiles.GetStaticTiles( item.X, item.Y, true );

				int z = int.MinValue;

				for ( int i = 0; i < tiles.Length; ++i )
				{
					Tile tile = tiles[i];
					ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

					int top = tile.Z; // Confirmed : no height checks here

					if ( id.Surface && !id.Impassable && top > z && top <= item.Z )
					{
						z = top;
					}
				}

				return z;
			}
		}
	}
}