using System;
using Server.Network;

namespace Server.Items
{
	public class SecureTradeContainer : Container
	{
		public SecureTrade Trade { get; }

		public SecureTradeContainer( SecureTrade trade )
			: base( 0x1E5E )
		{
			Trade = trade;

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
			if ( Trade != null )
			{
				Trade.From.Accepted = false;
				Trade.To.Accepted = false;
				Trade.Update();
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