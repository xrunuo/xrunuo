using System;
using System.IO;
using System.Text;
using Server;
using Server.Network;

namespace Server.Items
{
	public class BookPageInfo
	{
		private string[] m_Lines;

		public string[] Lines { get { return m_Lines; } set { m_Lines = value; } }

		public BookPageInfo()
		{
			m_Lines = new string[0];
		}

		public BookPageInfo( GenericReader reader )
		{
			int length = reader.ReadInt();

			m_Lines = new string[length];

			for ( int i = 0; i < m_Lines.Length; ++i )
			{
				m_Lines[i] = String.Intern( reader.ReadString() );
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( m_Lines.Length );

			for ( int i = 0; i < m_Lines.Length; ++i )
			{
				writer.Write( m_Lines[i] );
			}
		}
	}

	public class BaseBook : Item
	{
		private string m_Title;
		private string m_Author;
		private BookPageInfo[] m_Pages;
		private bool m_Writable;

		[CommandProperty( AccessLevel.GameMaster )]
		public string Title
		{
			get { return m_Title; }
			set
			{
				m_Title = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Author
		{
			get { return m_Author; }
			set
			{
				m_Author = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Writable { get { return m_Writable; } set { m_Writable = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PagesCount { get { return m_Pages.Length; } }

		public BookPageInfo[] Pages { get { return m_Pages; } }

		public virtual double BookWeight { get { return 1.0; } }

		[Constructable]
		public BaseBook( int itemID )
			: this( itemID, 20, true )
		{
		}

		[Constructable]
		public BaseBook( int itemID, int pageCount, bool writable )
			: this( itemID, null, null, pageCount, writable )
		{
		}

		[Constructable]
		public BaseBook( int itemID, string title, string author, int pageCount, bool writable )
			: base( itemID )
		{
			m_Title = title;
			m_Author = author;
			m_Pages = new BookPageInfo[pageCount];
			m_Writable = writable;

			for ( int i = 0; i < m_Pages.Length; ++i )
			{
				m_Pages[i] = new BookPageInfo();
			}

			Weight = BookWeight;
		}

		public BaseBook( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Title );
			writer.Write( m_Author );
			writer.Write( m_Writable );

			writer.Write( m_Pages.Length );

			for ( int i = 0; i < m_Pages.Length; ++i )
			{
				m_Pages[i].Serialize( writer );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Title = reader.ReadString();
						m_Author = reader.ReadString();
						m_Writable = reader.ReadBool();

						m_Pages = new BookPageInfo[reader.ReadInt()];

						for ( int i = 0; i < m_Pages.Length; ++i )
						{
							m_Pages[i] = new BookPageInfo( reader );
						}

						break;
					}
			}

			Weight = BookWeight;
		}

		public override LocalizedText GetNameProperty()
		{
			if ( m_Title != null && m_Title.Length > 0 )
			{
				return new LocalizedText( m_Title );
			}
			else
			{
				return base.GetNameProperty();
			}
		}

		/*public override void GetProperties( IObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Title != null && m_Title.Length > 0 )
				list.Add( 1060658, "Title\t{0}", m_Title ); // ~1_val~: ~2_val~

			if ( m_Author != null && m_Author.Length > 0 )
				list.Add( 1060659, "Author\t{0}", m_Author ); // ~1_val~: ~2_val~

			if ( m_Pages != null && m_Pages.Length > 0 )
				list.Add( 1060660, "Pages\t{0}", m_Pages.Length ); // ~1_val~: ~2_val~
		}*/

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Title == null && m_Author == null && m_Writable == true )
			{
				Title = "a book";
				Author = from.Name;
			}

			from.Send( new BookHeader( from, this ) );
			from.Send( new BookPageDetails( this ) );
		}

		public static void Initialize()
		{
			PacketHandlers.Instance.Register( 0xD4, 0, true, new OnPacketReceive( HeaderChange ) );
			PacketHandlers.Instance.Register( 0x66, 0, true, new OnPacketReceive( ContentChange ) );
			PacketHandlers.Instance.Register( 0x93, 99, true, new OnPacketReceive( OldHeaderChange ) );
		}

		public static void OldHeaderChange( GameClient state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			BaseBook book = World.Instance.FindItem( pvSrc.ReadInt32() ) as BaseBook;

			if ( book == null || !book.Writable || !from.InRange( book.GetWorldLocation(), 1 ) )
			{
				return;
			}

			pvSrc.Seek( 4, SeekOrigin.Current ); // Skip flags and page count

			string title = pvSrc.ReadStringSafe( 60 );
			string author = pvSrc.ReadStringSafe( 30 );

			book.Title = Utility.FixHtml( title );
			book.Author = Utility.FixHtml( author );
		}

		public static void HeaderChange( GameClient state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			BaseBook book = World.Instance.FindItem( pvSrc.ReadInt32() ) as BaseBook;

			if ( book == null || !book.Writable || !from.InRange( book.GetWorldLocation(), 1 ) )
			{
				return;
			}

			pvSrc.Seek( 4, SeekOrigin.Current ); // Skip flags and page count

			int titleLength = pvSrc.ReadUInt16();

			if ( titleLength > 60 )
			{
				return;
			}

			string title = pvSrc.ReadUTF8StringSafe( titleLength );

			int authorLength = pvSrc.ReadUInt16();

			if ( authorLength > 30 )
			{
				return;
			}

			string author = pvSrc.ReadUTF8StringSafe( authorLength );

			book.Title = Utility.FixHtml( title );
			book.Author = Utility.FixHtml( author );
		}

		public static void ContentChange( GameClient state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			BaseBook book = World.Instance.FindItem( pvSrc.ReadInt32() ) as BaseBook;

			if ( book == null || !book.Writable || !from.InRange( book.GetWorldLocation(), 1 ) )
			{
				return;
			}

			int pageCount = pvSrc.ReadUInt16();

			if ( pageCount > book.PagesCount )
			{
				return;
			}

			for ( int i = 0; i < pageCount; ++i )
			{
				int index = pvSrc.ReadUInt16();

				if ( index >= 1 && index <= book.PagesCount )
				{
					--index;

					int lineCount = pvSrc.ReadUInt16();

					if ( lineCount <= 8 )
					{
						string[] lines = new string[lineCount];

						for ( int j = 0; j < lineCount; ++j )
						{
							if ( ( lines[j] = pvSrc.ReadUTF8StringSafe() ).Length >= 80 )
							{
								return;
							}
						}

						book.Pages[index].Lines = lines;
					}
					else
					{
						return;
					}
				}
				else
				{
					return;
				}
			}
		}
	}

	public sealed class BookPageDetails : Packet
	{
		public BookPageDetails( BaseBook book )
			: base( 0x66 )
		{
			EnsureCapacity( 256 );

			m_Stream.Write( (int) book.Serial );
			m_Stream.Write( (ushort) book.PagesCount );

			for ( int i = 0; i < book.PagesCount; ++i )
			{
				BookPageInfo page = book.Pages[i];

				m_Stream.Write( (ushort) ( i + 1 ) );
				m_Stream.Write( (ushort) page.Lines.Length );

				for ( int j = 0; j < page.Lines.Length; ++j )
				{
					byte[] buffer = Utility.UTF8.GetBytes( page.Lines[j] );

					m_Stream.Write( buffer, 0, buffer.Length );
					m_Stream.Write( (byte) 0 );
				}
			}
		}
	}

	public sealed class BookHeader : Packet
	{
		public BookHeader( Mobile from, BaseBook book )
			: base( 0xD4 )
		{
			string title = book.Title == null ? "" : book.Title;
			string author = book.Author == null ? "" : book.Author;

			byte[] titleBuffer = Utility.UTF8.GetBytes( title );
			byte[] authorBuffer = Utility.UTF8.GetBytes( author );

			EnsureCapacity( 15 + titleBuffer.Length + authorBuffer.Length );

			m_Stream.Write( (int) book.Serial );
			m_Stream.Write( (bool) true );
			m_Stream.Write( (bool) book.Writable && from.InRange( book.GetWorldLocation(), 1 ) );
			m_Stream.Write( (ushort) book.PagesCount );

			m_Stream.Write( (ushort) ( titleBuffer.Length + 1 ) );
			m_Stream.Write( titleBuffer, 0, titleBuffer.Length );
			m_Stream.Write( (byte) 0 ); // terminate

			m_Stream.Write( (ushort) ( authorBuffer.Length + 1 ) );
			m_Stream.Write( authorBuffer, 0, authorBuffer.Length );
			m_Stream.Write( (byte) 0 ); // terminate
		}
	}
}
