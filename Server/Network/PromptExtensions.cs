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
using Server.Prompts;
using Server.Gumps;

namespace Server.Network
{
	public static class PromptExtensions
	{
		public static void SendTo( this Prompt prompt, Mobile m )
		{
			if ( m.NetState != null && m.NetState.Version.IsEnhanced )
			{
				m.Send( new PromptGumpStub( prompt, m ).GetPacket() );
			}
			else
			{
				if ( prompt.MessageCliloc != 1042971 || prompt.MessageArgs != String.Empty )
					m.SendLocalizedMessage( prompt.MessageCliloc, prompt.MessageArgs, prompt.MessageHue );

				m.Send( new UnicodePrompt( prompt, m ) );
			}
		}
	}

	public class PromptGumpStub : Gump
	{
		public override int TypeID { get { return 0x2AE; } }

		public PromptGumpStub( Prompt prompt, Mobile to )
			: base( 0, 0 )
		{
			Serial senderSerial = prompt.Sender != null ? prompt.Sender.Serial : to.Serial;

			Serial = senderSerial;

			Intern( "TEXTENTRY" );
			Intern( senderSerial.Value.ToString() );
			Intern( to.Serial.Value.ToString() );
			Intern( prompt.TypeId.ToString() );
			Intern( prompt.MessageCliloc.ToString() ); // TODO: Is there a way to include args here?
			Intern( "1" ); // 0 = Ascii response, 1 = Unicode Response

			AddBackground( 50, 50, 540, 350, 0xA28 );

			AddPage( 0 );

			AddHtmlLocalized( 264, 80, 200, 24, 1062524, false, false );
			AddHtmlLocalized( 120, 108, 420, 48, 1062638, false, false );
			AddBackground( 100, 148, 440, 200, 0xDAC );
			AddTextEntryIntern( 120, 168, 400, 200, 0x0, 44, 0 );
			AddButton( 175, 355, 0x81A, 0x81B, 1, GumpButtonType.Reply, 0 );
			AddButton( 405, 355, 0x819, 0x818, 0, GumpButtonType.Reply, 0 );
		}

		public Packet GetPacket()
		{
			return Compile();
		}
	}
}
