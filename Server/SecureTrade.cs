using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server
{
	public class SecureTrade
	{
		public SecureTradeInfo From { get; }

		public SecureTradeInfo To { get; }

		public bool Valid { get; private set; }

		public void Cancel()
		{
			if ( !Valid )
				return;

			var list = From.Container.Items;

			for ( var i = list.Count - 1; i >= 0; --i )
			{
				if ( i < list.Count )
				{
					var item = list[i];

					item.OnSecureTrade( From.Mobile, To.Mobile, From.Mobile, false );

					if ( !item.Deleted )
						From.Mobile.AddToBackpack( item );
				}
			}

			list = To.Container.Items;

			for ( var i = list.Count - 1; i >= 0; --i )
			{
				if ( i < list.Count )
				{
					var item = list[i];

					item.OnSecureTrade( To.Mobile, From.Mobile, To.Mobile, false );

					if ( !item.Deleted )
						To.Mobile.AddToBackpack( item );
				}
			}

			Close();
		}

		public void Close()
		{
			if ( !Valid )
				return;

			From.Mobile.Send( new CloseSecureTrade( From.Container ) );
			To.Mobile.Send( new CloseSecureTrade( To.Container ) );

			Valid = false;

			var ns = From.Mobile.NetState;

			ns?.RemoveTrade( this );

			ns = To.Mobile.NetState;

			ns?.RemoveTrade( this );

			From.Container.Delete();
			To.Container.Delete();
		}

		public void Update()
		{
			if ( !Valid )
				return;

			if ( From.Accepted && To.Accepted )
			{
				var list = From.Container.Items;

				var allowed = true;

				for ( var i = list.Count - 1; allowed && i >= 0; --i )
				{
					if ( i < list.Count )
					{
						var item = list[i];

						if ( !item.AllowSecureTrade( From.Mobile, To.Mobile, To.Mobile, true ) )
							allowed = false;
					}
				}

				list = To.Container.Items;

				for ( var i = list.Count - 1; allowed && i >= 0; --i )
				{
					if ( i < list.Count )
					{
						var item = list[i];

						if ( !item.AllowSecureTrade( To.Mobile, From.Mobile, From.Mobile, true ) )
							allowed = false;
					}
				}

				if ( !allowed )
				{
					From.Accepted = false;
					To.Accepted = false;

					From.Mobile.Send( new UpdateSecureTrade( From.Container, From.Accepted, To.Accepted ) );
					To.Mobile.Send( new UpdateSecureTrade( To.Container, To.Accepted, From.Accepted ) );

					return;
				}

				list = From.Container.Items;

				for ( var i = list.Count - 1; i >= 0; --i )
				{
					if ( i < list.Count )
					{
						var item = list[i];

						item.OnSecureTrade( From.Mobile, To.Mobile, To.Mobile, true );

						if ( !item.Deleted )
							To.Mobile.AddToBackpack( item );
					}
				}

				list = To.Container.Items;

				for ( var i = list.Count - 1; i >= 0; --i )
				{
					if ( i < list.Count )
					{
						var item = list[i];

						item.OnSecureTrade( To.Mobile, From.Mobile, From.Mobile, true );

						if ( !item.Deleted )
							From.Mobile.AddToBackpack( item );
					}
				}

				Close();
			}
			else
			{
				From.Mobile.Send( new UpdateSecureTrade( From.Container, From.Accepted, To.Accepted ) );
				To.Mobile.Send( new UpdateSecureTrade( To.Container, To.Accepted, From.Accepted ) );
			}
		}

		public SecureTrade( Mobile from, Mobile to )
		{
			Valid = true;

			From = new SecureTradeInfo( this, from, new SecureTradeContainer( this ) );
			To = new SecureTradeInfo( this, to, new SecureTradeContainer( this ) );

			from.Send( new MobileStatus( from, to ) );
			from.Send( new UpdateSecureTrade( From.Container, false, false ) );
			from.Send( new SecureTradeEquip( To.Container, to ) );
			from.Send( new UpdateSecureTrade( From.Container, false, false ) );
			from.Send( new SecureTradeEquip( From.Container, from ) );
			from.Send( new DisplaySecureTrade( to, From.Container, To.Container, to.Name ) );
			from.Send( new UpdateSecureTrade( From.Container, false, false ) );

			to.Send( new MobileStatus( to, from ) );
			to.Send( new UpdateSecureTrade( To.Container, false, false ) );
			to.Send( new SecureTradeEquip( From.Container, from ) );
			to.Send( new UpdateSecureTrade( To.Container, false, false ) );
			to.Send( new SecureTradeEquip( To.Container, to ) );
			to.Send( new DisplaySecureTrade( from, To.Container, From.Container, from.Name ) );
			to.Send( new UpdateSecureTrade( To.Container, false, false ) );
		}
	}

	public class SecureTradeInfo
	{
		public SecureTradeInfo( SecureTrade owner, Mobile m, SecureTradeContainer c )
		{
			Owner = owner;
			Mobile = m;
			Container = c;

			Mobile.AddItem( Container );
		}

		public SecureTrade Owner { get; }

		public Mobile Mobile { get; }

		public SecureTradeContainer Container { get; }

		public bool Accepted { get; set; }
	}
}
