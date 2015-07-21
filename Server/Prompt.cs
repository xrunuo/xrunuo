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
using Server;
using Server.Gumps;

namespace Server.Prompts
{
	public abstract class Prompt
	{
		private IEntity m_Sender;
		private String m_MessageArgs;

		public IEntity Sender
		{
			get { return m_Sender; }
		}

		public String MessageArgs
		{
			get { return m_MessageArgs; }
		}

		public virtual int MessageCliloc
		{
			get { return 1042971; } // ~1_NOTHING~
		}

		public virtual int MessageHue
		{
			get { return 0; }
		}

		public virtual int TypeId
		{
			get { return GetType().FullName.GetHashCode(); }
		}

		public Prompt()
			: this( null )
		{
		}

		public Prompt( IEntity sender )
			: this( sender, String.Empty )
		{
		}

		public Prompt( IEntity sender, String args )
		{
			m_Sender = sender;
			m_MessageArgs = args;
		}

		public virtual void OnCancel( Mobile from )
		{
		}

		public virtual void OnResponse( Mobile from, string text )
		{
		}
	}
}