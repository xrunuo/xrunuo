using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Housing.Gumps;
using Server.Engines.Housing.Multis;
using Server.Gumps;
using Server.Multis;

namespace Server.Engines.Housing.Items
{
	[TypeAlias( "Server.Multis.HouseSign" )]
	public class HouseSign : Item
	{
		private static readonly string UnownedName = Core.Config.ServerName;

		private BaseHouse m_Owner;
		private Mobile m_OrgOwner;

		public HouseSign( BaseHouse owner )
			: base( 0xBD2 )
		{
			m_Owner = owner;
			m_OrgOwner = m_Owner.Owner;
			Name = "a house sign";
			Movable = false;
		}

		public HouseSign( Serial serial )
			: base( serial )
		{
		}

		public BaseHouse Owner { get { return m_Owner; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RestrictDecay
		{
			get { return ( m_Owner != null && m_Owner.RestrictDecay ); }
			set
			{
				if ( m_Owner != null )
				{
					m_Owner.RestrictDecay = value;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile OriginalOwner { get { return m_OrgOwner; } }

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Owner != null && !m_Owner.Deleted )
			{
				m_Owner.Delete();
			}
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1061638 ); // A House Sign
		}

		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1061639, Name == "a house sign" ? "nothing" : Utility.FixHtml( Name ) ); // Name: ~1_NAME~
			list.Add( 1061640, ( m_Owner == null || m_Owner.Owner == null ) ? UnownedName : m_Owner.Owner.Name ); // Owner: ~1_OWNER~

			if ( m_Owner != null )
			{
				list.Add( m_Owner.Public ? 1061641 : 1061642 ); // This House is Open to the Public : This is a Private Home

				DecayLevel level = m_Owner.DecayLevel;

				if ( level == DecayLevel.DemolitionPending )
				{
					list.Add( 1062497 ); // Demolition Pending
				}
				else if ( level != DecayLevel.Ageless )
				{
					if ( level == DecayLevel.Collapsed )
						level = DecayLevel.IDOC;

					list.Add( 1043009 + (int) level ); // This structure is ...
				}
			}
		}

		public void ShowSign( Mobile m )
		{
			if ( m_Owner != null )
			{
				if ( m_Owner.IsFriend( m ) && m.AccessLevel < AccessLevel.GameMaster )
				{
					m_Owner.RefreshDecay();
					m.SendLocalizedMessage( 501293 ); // Welcome back to the house, friend!
				}

				m.SendGump( new HouseGump( HouseGumpPage.Information, m, m_Owner ) );
			}
		}

		// House sign can be used from inside the house even with no LOS to it.
		public override bool CheckLOSOnUse { get { return false; } }

		public override void OnDoubleClick( Mobile m )
		{
			if ( m_Owner == null )
				return;

			ShowSign( m );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive && Owner != null )
			{
				if ( Owner.AreThereAvailableVendorsFor( from ) )
					list.Add( new VendorsEntry( this ) );

				if ( Owner.VendorInventories.Count > 0 )
					list.Add( new ReclaimVendorInventoryEntry( this ) );
			}
		}

		private class VendorsEntry : ContextMenuEntry
		{
			private HouseSign m_Sign;

			public VendorsEntry( HouseSign sign )
				: base( 6211 )
			{
				m_Sign = sign;
			}

			public override void OnClick()
			{
				Mobile from = this.Owner.From;

				if ( !from.CheckAlive() || m_Sign.Deleted || m_Sign.Owner == null || !m_Sign.Owner.AreThereAvailableVendorsFor( from ) )
				{
					return;
				}

				if ( from.Map != m_Sign.Map || !from.InRange( m_Sign, 5 ) )
				{
					from.SendLocalizedMessage( 1062429 ); // You must be within five paces of the house sign to use this option.
				}
				else
				{
					from.SendGump( new HouseGump( HouseGumpPage.Vendors, from, m_Sign.Owner ) );
				}
			}
		}

		private class ReclaimVendorInventoryEntry : ContextMenuEntry
		{
			private HouseSign m_Sign;

			public ReclaimVendorInventoryEntry( HouseSign sign )
				: base( 6213 )
			{
				m_Sign = sign;
			}

			public override void OnClick()
			{
				Mobile from = this.Owner.From;

				if ( m_Sign.Deleted || m_Sign.Owner == null || m_Sign.Owner.VendorInventories.Count == 0 || !from.CheckAlive() )
				{
					return;
				}

				if ( from.Map != m_Sign.Map || !from.InRange( m_Sign, 5 ) )
				{
					from.SendLocalizedMessage( 1062429 ); // You must be within five paces of the house sign to use this option.
				}
				else
				{
					from.CloseGump( typeof( VendorInventoryGump ) );
					from.SendGump( new VendorInventoryGump( m_Sign.Owner, from ) );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Owner );
			writer.Write( m_OrgOwner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Owner = reader.ReadItem() as BaseHouse;
			m_OrgOwner = reader.ReadMobile();
		}
	}
}