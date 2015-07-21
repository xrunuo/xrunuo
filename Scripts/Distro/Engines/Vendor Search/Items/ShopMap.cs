using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Fourth;

namespace Server.Items
{
	public class ShopMap : MapItem
	{
		public static readonly int TeleportCost = 1000;
		public static readonly int DeleteDelayMinutes = 30;

		private Point3D m_DestLocation;
		private Map m_DestMap;
		private string m_VendorName, m_ShopName;
		private Point3D m_PreviousLocation;
		private Map m_PreviousMap;
		private Container m_Container;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D DestLocation { get { return m_DestLocation; } set { m_DestLocation = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Map DestMap { get { return m_DestMap; } set { m_DestMap = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string VendorName { get { return m_VendorName; } set { m_VendorName = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string ShopName { get { return m_ShopName; } set { m_ShopName = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D PreviousLocation { get { return m_PreviousLocation; } set { m_PreviousLocation = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Map PreviousMap { get { return m_PreviousMap; } set { m_PreviousMap = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Used { get { return m_PreviousMap != null; } }

		[Constructable]
		public ShopMap( PlayerVendor vendor, Container container )
		{
			m_DestLocation = vendor.House.BanLocation;
			m_DestMap = vendor.Map;
			m_VendorName = vendor.Name;
			m_ShopName = vendor.ShopName;
			m_Container = container;

			Hue = RecallRune.CalculateHue( m_DestMap );

			const int width = 400;
			const int height = 400;

			SetDisplay( vendor.X - ( width / 2 ), vendor.Y - ( height / 2 ), vendor.X + ( width / 2 ), vendor.Y + ( height / 2 ), width, height );
			AddWorldPin( vendor.X, vendor.Y );

			Timer.DelayCall( TimeSpan.FromMinutes( DeleteDelayMinutes ), Delete );
		}

		public override Map DisplayMap { get { return m_DestMap; } }

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1154559, string.Format( "{0}\t{1}", m_VendorName, m_ShopName ) ); // Map to Vendor ~1_Name~: ~2_Shop~
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1154639, string.Format( "{0}\t#{1}", GetLocationFormatted( m_DestLocation, m_DestMap ), m_DestMap.GetNameCliloc() ) ); // Vendor Located at ~1_loc~ (~2_facet~)
			list.Add( 1075269 ); // Destroyed when dropped
		}

		public static string GetLocationFormatted( Point3D loc, Map map )
		{
			int xLong = 0, yLat = 0;
			int xMins = 0, yMins = 0;
			bool xEast = false, ySouth = false;

			if ( Sextant.Format( loc, map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
				return string.Format( "{0}° {1}'{2}, {3}o {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
			else
				return "an unknown location";
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			from.SendLocalizedMessage( 500424 ); // You destroyed the item.
			Delete();
		}

		public void TeleportToVendor( Mobile from )
		{
			if ( Used )
				return;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else
			{
				Spell spell = new VendorTeleportSpell( from, this );
				spell.Cast();
			}
		}

		public void TeleportToPreviousLocation( Mobile from )
		{
			if ( !Used )
				return;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else
			{
				Spell spell = new ReturnFromVendorSpell( from, this );
				spell.Cast();
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			list.Add( new OpenMapEntry( from, this ) );
			list.Add( new TeleportEntry( from, this ) );

			if ( m_Container != null )
				list.Add( new OpenContainerEntry( from, m_Container ) );
		}

		private class OpenMapEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private ShopMap m_Map;

			public OpenMapEntry( Mobile mobile, ShopMap map )
				: base( 6150, 1 ) // Open Map
			{
				m_Mobile = mobile;
				m_Map = map;
			}

			public override void OnClick()
			{
				m_Map.OnDoubleClick( m_Mobile );
			}
		}

		private class TeleportEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private ShopMap m_Map;

			public TeleportEntry( Mobile mobile, ShopMap map )
				: base( map.Used ? 1154636 : 1154558, 1 )
			{
				m_Mobile = mobile;
				m_Map = map;
			}

			public override void OnClick()
			{
				m_Mobile.CloseGump( typeof( VendorTeleportGump ) );
				m_Mobile.SendGump( new VendorTeleportGump( m_Map ) );
			}
		}

		private class OpenContainerEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Container m_Container;

			public OpenContainerEntry( Mobile mobile, Container container )
				: base( 1154699, 1 )
			{
				m_Mobile = mobile;
				m_Container = container;

				Enabled = IsAccessible();
			}

			private bool IsAccessible()
			{
				if ( !m_Container.IsAccessibleTo( m_Mobile ) )
					return false;

				if ( !m_Mobile.InRange( m_Container.GetWorldLocation(), 18 ) )
					return false;

				return true;
			}

			public override void OnClick()
			{
				RecurseOpen( m_Container, m_Mobile );
			}

			private static void RecurseOpen( Container c, Mobile from )
			{
				Container parent = c.Parent as Container;

				if ( parent != null )
					RecurseOpen( parent, from );

				c.DisplayTo( from );
			}
		}

		public ShopMap( Serial serial )
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

			Delete();
		}
	}

	public abstract class BaseVendorTeleportSpell : RecallSpell
	{
		protected ShopMap m_Map;

		public BaseVendorTeleportSpell( Mobile caster, ShopMap map )
			: base( caster, null )
		{
			m_Map = map;
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = max = -1;
		}

		public override int GetMana()
		{
			return 0;
		}

		public bool IsMapInBackpack()
		{
			return m_Map.IsChildOf( Caster.Backpack );
		}

		public override bool ConsumeReagents()
		{
			return true;
		}
	}

	public class VendorTeleportSpell : BaseVendorTeleportSpell
	{
		public VendorTeleportSpell( Mobile caster, ShopMap shopMap )
			: base( caster, shopMap )
		{
		}

		private bool CanAfford()
		{
			if ( Banker.GetBalance( Caster ) < ShopMap.TeleportCost )
			{
				Caster.SendLocalizedMessage( 1154672 ); // You cannot afford to teleport to the vendor.
				return false;
			}

			return true;
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( !CanAfford() )
				return false;

			if ( !IsMapInBackpack() )
				return false;

			return true;
		}

		public override void OnCast()
		{
			if ( !CanAfford() )
				return;

			if ( !IsMapInBackpack() )
				return;

			var previousLocation = Caster.Location;
			var previousMap = Caster.Map;

			if ( Effect( m_Map.DestLocation, m_Map.DestMap, true ) )
			{
				Banker.Withdraw( Caster, ShopMap.TeleportCost );
				Caster.SendLocalizedMessage( 1060398, ShopMap.TeleportCost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.

				m_Map.PreviousLocation = previousLocation;
				m_Map.PreviousMap = previousMap;

				Caster.SendLocalizedMessage( 1070905 ); // Strong magics have redirected you to a safer location!
			}
		}
	}

	public class ReturnFromVendorSpell : BaseVendorTeleportSpell
	{
		public ReturnFromVendorSpell( Mobile caster, ShopMap map )
			: base( caster, map )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( !IsMapInBackpack() )
				return false;

			return true;
		}

		public override void OnCast()
		{
			if ( !IsMapInBackpack() )
				return;

			if ( Effect( m_Map.PreviousLocation, m_Map.PreviousMap, true ) )
			{
				m_Map.Delete();
			}
		}
	}
}
