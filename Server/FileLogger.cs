//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Text;

namespace Server
{
	public class FileLogger : TextWriter, IDisposable
	{
		private string m_FileName;
		private bool m_NewLine;
		public const string DateFormat = "[MMMM dd hh:mm:ss.f tt]: ";

		public string FileName { get { return m_FileName; } }

		public FileLogger( string file )
			: this( file, false )
		{
		}

		public FileLogger( string file, bool append )
		{
			m_FileName = file;

			using ( StreamWriter writer = new StreamWriter( new FileStream( m_FileName, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read ) ) )
				writer.WriteLine( ">>>Logging started on {0}.", DateTime.UtcNow.ToString( "f" ) ); //f = Tuesday, April 10, 2001 3:51 PM 

			m_NewLine = true;
		}

		public override void Write( char ch )
		{
			using ( StreamWriter writer = new StreamWriter( new FileStream( m_FileName, FileMode.Append, FileAccess.Write, FileShare.Read ) ) )
			{
				if ( m_NewLine )
				{
					writer.Write( DateTime.UtcNow.ToString( DateFormat ) );
					m_NewLine = false;
				}

				writer.Write( ch );
			}
		}

		public override void Write( string str )
		{
			using ( StreamWriter writer = new StreamWriter( new FileStream( m_FileName, FileMode.Append, FileAccess.Write, FileShare.Read ) ) )
			{
				if ( m_NewLine )
				{
					writer.Write( DateTime.UtcNow.ToString( DateFormat ) );
					m_NewLine = false;
				}

				writer.Write( str );
			}
		}

		public override void WriteLine( string line )
		{
			using ( StreamWriter writer = new StreamWriter( new FileStream( m_FileName, FileMode.Append, FileAccess.Write, FileShare.Read ) ) )
			{
				if ( m_NewLine )
					writer.Write( DateTime.UtcNow.ToString( DateFormat ) );

				writer.WriteLine( line );
				m_NewLine = true;
			}
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}
}
