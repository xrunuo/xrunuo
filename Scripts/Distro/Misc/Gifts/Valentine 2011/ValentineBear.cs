using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
	public class ValentineBear : Item
	{
		private string m_OwnerName;
		private string m_Line1, m_Line2, m_Line3;

		private DateTime m_EditEnd;

		[CommandProperty( AccessLevel.GameMaster )]
		public string OwnerName
		{
			get { return m_OwnerName; }
			set
			{
				m_OwnerName = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Line1
		{
			get { return m_Line1 == null ? "" : m_Line1; }
			set { m_Line1 = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Line2
		{
			get { return m_Line2 == null ? "" : m_Line2; }
			set { m_Line2 = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Line3
		{
			get { return m_Line3 == null ? "" : m_Line3; }
			set { m_Line3 = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime EditEnd
		{
			get { return m_EditEnd; }
			set { m_EditEnd = value; }
		}

		[Constructable]
		public ValentineBear( Mobile owner )
			: base( Utility.Random( 0x48E0, 4 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			m_OwnerName = owner.Name;
			m_EditEnd = DateTime.MaxValue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1116249 ); // That must be in your backpack for you to use it.
			}
			else if ( m_EditEnd > DateTime.UtcNow )
			{
				from.CloseGump( typeof( ValentineBearGump ) );
				from.SendGump( new ValentineBearGump( this ) );
			}
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1150295, m_OwnerName ); // ~1_NAME~'s St. Valentine Bear
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !String.IsNullOrEmpty( m_Line1 ) )
				list.Add( 1150301, m_Line1 ); // [ ~1_LINE0~ ]

			if ( !String.IsNullOrEmpty( m_Line2 ) )
				list.Add( 1150302, m_Line2 ); // [ ~1_LINE1~ ]

			if ( !String.IsNullOrEmpty( m_Line3 ) )
				list.Add( 1150303, m_Line3 ); // [ ~1_LINE2~ ]
		}

		public ValentineBear( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_OwnerName );

			writer.Write( m_Line1 );
			writer.Write( m_Line2 );
			writer.Write( m_Line3 );

			writer.Write( m_EditEnd );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_OwnerName = reader.ReadString();

			m_Line1 = reader.ReadString();
			m_Line2 = reader.ReadString();
			m_Line3 = reader.ReadString();

			m_EditEnd = reader.ReadDateTime();
		}
	}
}