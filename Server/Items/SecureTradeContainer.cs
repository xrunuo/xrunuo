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
using Server.Network;

namespace Server.Items
{
	public class SecureTradeContainer : Container
	{
		private SecureTrade m_Trade;

		public SecureTrade Trade
		{
			get
			{
				return m_Trade;
			}
		}

		public SecureTradeContainer( SecureTrade trade )
			: base( 0x1E5E )
		{
			m_Trade = trade;

			Movable = false;
		}

		public SecureTradeContainer( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			Mobile to;

			if ( this.Trade.From.Container != this )
				to = this.Trade.From.Mobile;
			else
				to = this.Trade.To.Mobile;

			return m.CheckTrade( to, item, this, message, checkItems, plusItems, plusWeight );
		}

		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			reject = LRReason.CannotLift;
			return false;
		}

		public override bool IsAccessibleTo( Mobile check )
		{
			if ( !IsChildOf( check ) )
				return false;

			return base.IsAccessibleTo( check );
		}

		public override void OnItemAdded( Item item )
		{
			ClearChecks();
		}

		public override void OnItemRemoved( Item item )
		{
			ClearChecks();
		}

		public override void OnSubItemAdded( Item item )
		{
			ClearChecks();
		}

		public override void OnSubItemRemoved( Item item )
		{
			ClearChecks();
		}

		public void ClearChecks()
		{
			if ( m_Trade != null )
			{
				m_Trade.From.Accepted = false;
				m_Trade.To.Accepted = false;
				m_Trade.Update();
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
	}
}