using System;
using Server;
using Server.Engines.BuffIcons;

namespace Server.Network
{
	public class AddBuffPacket : Packet
	{
		public AddBuffPacket( Mobile m, BuffInfo info )
			: this( m, info.Id, info.TitleCliloc, info.SecondaryCliloc, info.Args, ( info.TimeStart != DateTime.MinValue ) ? ( ( info.TimeStart + info.TimeLength ) - DateTime.Now ) : TimeSpan.Zero )
		{
		}

		public AddBuffPacket( Mobile mob, BuffIcon iconID, int titleCliloc, int secondaryCliloc, TextDefinition args, TimeSpan length )
			: base( 0xDF )
		{
			bool hasArgs = ( args != null );

			this.EnsureCapacity( ( hasArgs ? ( 48 + args.ToString().Length * 2 ) : 44 ) );
			m_Stream.Write( (int) mob.Serial );


			m_Stream.Write( (short) iconID ); // ID
			m_Stream.Write( (short) 0x1 ); // Type 0 for removal. 1 for add 2 for Data

			m_Stream.Fill( 4 );

			m_Stream.Write( (short) iconID ); // ID
			m_Stream.Write( (short) 0x01 ); // Type 0 for removal. 1 for add 2 for Data

			m_Stream.Fill( 4 );

			if ( length < TimeSpan.Zero )
				length = TimeSpan.Zero;

			m_Stream.Write( (short) length.TotalSeconds ); // Time in seconds

			m_Stream.Fill( 3 );
			m_Stream.Write( (int) titleCliloc );
			m_Stream.Write( (int) secondaryCliloc );

			if ( !hasArgs )
			{
				m_Stream.Fill( 10 );
			}
			else
			{
				m_Stream.Fill( 4 );
				m_Stream.Write( (short) 0x1 ); // Unknown -> Possibly something saying 'hey, I have more data!'?
				m_Stream.Fill( 2 );

				m_Stream.WriteLittleUniNull( String.Format( "\t{0}", args.ToString() ) );

				m_Stream.Write( (short) 0x1 ); // Even more Unknown -> Possibly something saying 'hey, I have more data!'?
				m_Stream.Fill( 2 );
			}
		}
	}
}
