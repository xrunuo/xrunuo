using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.Housing;
using Server.Engines.Housing.Multis;
using Server.Gumps;
using Server.Network;
using Server.Multis;
using Server.Engines.Craft;

namespace Server.Items
{
	public class Runebook : Item, ISecurable, ICraftable
	{
		private List<RunebookEntry> m_Entries;
		private string m_Description;
		private int m_CurCharges, m_MaxCharges;
		private int m_DefaultIndex;
		private SecureLevel m_Level;
		private bool m_Exceptional;
		private Mobile m_Crafter;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Exceptional
		{
			get { return m_Exceptional; }
			set { m_Exceptional = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get { return m_Crafter; }
			set { m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurCharges
		{
			get
			{
				return m_CurCharges;
			}
			set
			{
				m_CurCharges = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxCharges
		{
			get
			{
				return m_MaxCharges;
			}
			set
			{
				m_MaxCharges = value;
			}
		}

		public override int LabelNumber { get { return 1041267; } } // runebook

		public override bool CanInsure { get { return false; } }

		[Constructable]
		public Runebook( int maxCharges )
			: base( 0x22C5 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 0x461;

			Layer = Layer.OneHanded;

			m_Entries = new List<RunebookEntry>();

			m_MaxCharges = maxCharges;

			m_DefaultIndex = -1;

			m_Level = SecureLevel.CoOwners;
		}

		[Constructable]
		public Runebook()
			: this( 12 )
		{
		}

		public List<RunebookEntry> Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public void AddEntry( RunebookEntry entry )
		{
			m_Entries.Add( entry );
		}

		public RunebookEntry Default
		{
			get
			{
				if ( m_DefaultIndex >= 0 && m_DefaultIndex < m_Entries.Count )
					return m_Entries[m_DefaultIndex];

				return null;
			}
			set
			{
				if ( value == null )
					m_DefaultIndex = -1;
				else
					m_DefaultIndex = m_Entries.IndexOf( value );
			}
		}

		public Runebook( Serial serial )
			: base( serial )
		{
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			return true;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			SetSecureLevelEntry.AddTo( from, this, list );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 4 );

			writer.Write( (bool) m_Exceptional );
			writer.Write( (Mobile) m_Crafter );

			writer.Write( (int) m_Level );

			writer.Write( m_Entries.Count );

			for ( int i = 0; i < m_Entries.Count; ++i )
				m_Entries[i].Serialize( writer );

			writer.Write( m_Description );
			writer.Write( m_CurCharges );
			writer.Write( m_MaxCharges );
			writer.Write( m_DefaultIndex );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 4:
					{
						m_Exceptional = reader.ReadBool();
						m_Crafter = reader.ReadMobile();
						goto case 3;
					}
				case 3:
					{
						m_Level = (SecureLevel) reader.ReadInt();

						int count = reader.ReadInt();

						m_Entries = new List<RunebookEntry>( count );

						for ( int i = 0; i < count; ++i )
							m_Entries.Add( new RunebookEntry( reader ) );

						m_Description = reader.ReadString();
						m_CurCharges = reader.ReadInt();
						m_MaxCharges = reader.ReadInt();
						m_DefaultIndex = reader.ReadInt();

						break;
					}
			}
		}

		public void DropRune( Mobile from, RunebookEntry e, int index )
		{
			if ( m_DefaultIndex == index )
				m_DefaultIndex = -1;

			m_Entries.RemoveAt( index );

			RecallRune rune = new RecallRune();

			rune.Target = e.Location;
			rune.TargetMap = e.Map;
			rune.Description = e.Description;
			rune.House = e.House;
			rune.Marked = true;
			rune.Hue = e.Hue;

			from.AddToBackpack( rune );

			from.SendLocalizedMessage( 502421 ); // You have removed the rune.
		}

		public bool IsOpen( Mobile toCheck )
		{
			GameClient ns = toCheck.Client;

			if ( ns == null )
				return false;

			foreach ( Gump gump in ns.Gumps )
			{
				if ( gump is RunebookGump )
				{
					RunebookGump rbgump = (RunebookGump) gump;

					if ( rbgump.Book == this )
						return true;
				}
			}

			return false;
		}

		public override bool DisplayLootType { get { return true; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Exceptional )
				list.Add( 1063341 ); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Description != null && m_Description.Length > 0 )
				list.Add( m_Description );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Server.Mobiles.PlayerMobile pm = from as Server.Mobiles.PlayerMobile;

			if ( from.InRange( GetWorldLocation(), 1 ) )
			{
				pm.CloseGump( typeof( RunebookGump ) );
				from.SendGump( new RunebookGump( from, this ) );
			}
		}

		public override void OnAfterDuped( Item newItem )
		{
			// TODO: Dupe entries
		}

		public bool CheckAccess( Mobile m )
		{
			if ( !IsLockedDown || m.AccessLevel >= AccessLevel.GameMaster )
				return true;

			IHouse house = HousingHelper.FindHouseAt( this );

			if ( house != null && ( house.Public ? house.IsBanned( m ) : !house.HasAccess( m ) ) )
				return false;

			return ( house != null && house.HasSecureAccess( m, m_Level ) );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is DarkKnightRune )
			{
				from.SendLocalizedMessage( 1078842 ); // That rune can't be added to a Runebook
			}
			else if ( dropped is RecallRune )
			{
				if ( !CheckAccess( from ) )
				{
					from.SendLocalizedMessage( 502413 ); // That cannot be done while the book is locked down.
				}
				else if ( IsOpen( from ) )
				{
					from.SendLocalizedMessage( 1005571 ); // You cannot place objects in the book while viewing the contents.
				}
				else if ( m_Entries.Count < 16 )
				{
					RecallRune rune = (RecallRune) dropped;

					if ( rune.Marked && rune.TargetMap != null )
					{
						m_Entries.Add( new RunebookEntry( rune.Target, rune.TargetMap, rune.Description, rune.House, rune.Hue ) );

						dropped.Delete();

						from.Send( GenericPackets.PlaySound( 0x42, GetWorldLocation() ) );

						string desc = rune.Description;

						if ( desc == null || ( desc = desc.Trim() ).Length == 0 )
							desc = "(indescript)";

						from.SendAsciiMessage( desc );

						return true;
					}
					else
					{
						from.SendLocalizedMessage( 502409 ); // This rune does not have a marked location.
					}
				}
				else
				{
					from.SendLocalizedMessage( 502401 ); // This runebook is full.
				}
			}
			else if ( dropped is RecallScroll )
			{
				if ( m_CurCharges < m_MaxCharges )
				{
					from.Send( GenericPackets.PlaySound( 0x249, GetWorldLocation() ) );

					int amount = dropped.Amount;

					if ( amount > ( m_MaxCharges - m_CurCharges ) )
					{
						dropped.Consume( m_MaxCharges - m_CurCharges );
						m_CurCharges = m_MaxCharges;
					}
					else
					{
						m_CurCharges += amount;
						dropped.Delete();

						return true;
					}
				}
				else
				{
					from.SendLocalizedMessage( 502410 ); // This book already has the maximum amount of charges.
				}
			}

			return false;
		}

		#region ICraftable Members
		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			int charges = 5 + (int) ( from.Skills[SkillName.Inscribe].Value / 30 );

			if ( exceptional )
				charges += 2;

			if ( charges > 10 )
				charges = 10;

			MaxCharges = charges * 2;

			if ( makersMark )
				Crafter = from;

			Exceptional = exceptional;

			return exceptional;
		}
		#endregion
	}

	public class RunebookEntry
	{
		private Point3D m_Location;
		private Map m_Map;
		private string m_Description;
		private IHouse m_House;
		private int m_Hue;

		public Point3D Location
		{
			get { return m_Location; }
		}

		public Map Map
		{
			get { return m_Map; }
		}

		public string Description
		{
			get { return m_Description; }
		}

		public IHouse House
		{
			get { return m_House; }
		}

		public int Hue
		{
			get { return m_Hue; }
		}

		public RunebookEntry( Point3D loc, Map map )
			: this( loc, map, string.Empty, null, 0 )
		{
		}

		public RunebookEntry( Point3D loc, Map map, string desc, IHouse house, int hue )
		{
			m_Location = loc;
			m_Map = map;
			m_Description = desc;
			m_House = house;
			m_Hue = hue;
		}

		public RunebookEntry( GenericReader reader )
		{
			int version = reader.ReadByte();

			switch ( version )
			{
				case 2:
					{
						m_Hue = reader.ReadInt();
						goto case 1;
					}
				case 1:
					{
						m_House = reader.ReadItem() as BaseHouse;
						goto case 0;
					}
				case 0:
					{
						m_Location = reader.ReadPoint3D();
						m_Map = reader.ReadMap();
						m_Description = reader.ReadString();

						break;
					}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (byte) 2 ); // version

			writer.Write( m_Hue );

			writer.Write( (Item) m_House );

			writer.Write( m_Location );
			writer.Write( m_Map );
			writer.Write( m_Description );
		}
	}
}