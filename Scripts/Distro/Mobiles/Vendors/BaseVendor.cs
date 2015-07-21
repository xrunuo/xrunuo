using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Accounting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.BulkOrders;
using Server.Regions;
using Server.Factions;
using Server.Events;

namespace Server.Mobiles
{
	public enum VendorShoeType
	{
		None,
		Shoes,
		Boots,
		Sandals,
		ThighBoots,
		SamuraiTabi,
		NinjaTabi
	}

	public abstract class BaseVendor : BaseCreature, IVendor
	{
		public static void Initialize()
		{
			EventSink.Instance.WorldBeforeSave += new WorldBeforeSaveEventHandler( EventSink_WorldBeforeSave );
		}

		private static void EventSink_WorldBeforeSave()
		{
			foreach ( Mobile m in World.Instance.Mobiles )
			{
				BaseVendor vendor = m as BaseVendor;

				if ( vendor == null )
					continue;

				List<Item> items = vendor.BuyPack.Items;

				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;

					Item item = items[i];

					if ( ( item.LastMoved + InventoryDecayTime ) <= DateTime.Now )
						item.Delete();
				}
			}
		}

		private static Dictionary<Type, int> m_VendorPrices = new Dictionary<Type, int>();

		public static int GetVendorPrice( Type type )
		{
			if ( m_VendorPrices.ContainsKey( type ) )
				return m_VendorPrices[type];

			return 10;
		}

		public static void SetVendorPrice( Type type, int price )
		{
			if ( type == null )
				return;

			m_VendorPrices[type] = price;
		}

		private const int MaxSell = 500;

		protected abstract ArrayList SBInfos { get; }

		private ArrayList m_ArmorBuyInfo = new ArrayList();
		private ArrayList m_ArmorSellInfo = new ArrayList();

		private DateTime m_LastRestock;

		public override bool CanTeach { get { return true; } }

		public override bool PlayerRangeSensitive { get { return true; } }

		public virtual bool IsActiveVendor { get { return true; } }
		public virtual bool IsActiveBuyer { get { return IsActiveVendor; } } // response to vendor SELL
		public virtual bool IsActiveSeller { get { return IsActiveVendor; } } // repsonse to vendor BUY

		public virtual NpcGuild NpcGuild { get { return NpcGuild.None; } }

		public virtual bool IsInvulnerable { get { return true; } }

		public virtual Race DefaultRace { get { return Race.DefaultRace; } }

		public override bool ShowFameTitle { get { return false; } }

		public override bool CheckLOSOnUse { get { return false; } }

		#region Bulk Orders
		public virtual bool IsValidBulkOrder( Item item )
		{
			return false;
		}

		public virtual Item CreateBulkOrder( Mobile from, bool fromContextMenu )
		{
			return null;
		}

		public virtual bool SupportsBulkOrders( Mobile from )
		{
			return false;
		}

		public virtual TimeSpan GetNextBulkOrder( Mobile from )
		{
			return TimeSpan.Zero;
		}

		public virtual void ClearNextBulkOrder( Mobile from )
		{
		}
		#endregion

		#region Faction
		public virtual int GetPriceScalar()
		{
			Town town = Town.FromRegion( this.Region );

			if ( town != null )
				return ( 100 + town.Tax );

			return 100;
		}

		public void UpdateBuyInfo()
		{
			int priceScalar = GetPriceScalar();

			IBuyItemInfo[] buyinfo = (IBuyItemInfo[]) m_ArmorBuyInfo.ToArray( typeof( IBuyItemInfo ) );

			if ( buyinfo != null )
			{
				foreach ( IBuyItemInfo info in buyinfo )
					info.PriceScalar = priceScalar;
			}
		}
		#endregion

		private class BulkOrderInfoEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseVendor m_Vendor;

			public BulkOrderInfoEntry( Mobile from, BaseVendor vendor )
				: base( 6152, 6 )
			{
				m_From = from;
				m_Vendor = vendor;
			}

			public override void OnClick()
			{
				Account acc = m_From.Account as Account;

				if ( acc == null || acc.Trial )
				{
					m_From.SendLocalizedMessage( 1111859 ); // Trial accounts cannot turn in a bulk order.
				}
				else if ( m_Vendor.SupportsBulkOrders( m_From ) )
				{
					TimeSpan ts = m_Vendor.GetNextBulkOrder( m_From );

					int totalSeconds = (int) ts.TotalSeconds;
					int totalMinutes = ( totalSeconds + 59 ) / 60;

					bool CanGetOrder = false;

					if ( totalMinutes == 0 )
						CanGetOrder = true;

					if ( CanGetOrder )
					{
						m_Vendor.SayTo( m_From, 1049038 ); // You can get an order now.

						Item bulkOrder = m_Vendor.CreateBulkOrder( m_From, true );

						if ( bulkOrder is LargeBOD )
							m_From.SendGump( new LargeBODAcceptGump( m_From, (LargeBOD) bulkOrder ) );
						else if ( bulkOrder is SmallBOD )
							m_From.SendGump( new SmallBODAcceptGump( m_From, (SmallBOD) bulkOrder ) );
					}
					else
					{
						int oldSpeechHue = m_Vendor.SpeechHue;
						m_Vendor.SpeechHue = 0x3B2;

						m_Vendor.SayTo( m_From, 1072058, totalMinutes.ToString() ); // An offer may be available in about ~1_minutes~ minutes.

						m_Vendor.SpeechHue = oldSpeechHue;
					}
				}
			}
		}

		public BaseVendor( string title )
			: base( AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2 )
		{
			LoadSBInfo();

			this.Title = title;

			InitBody();
			InitOutfit();

			Container pack;
			//these packs MUST exist, or the client will crash when the packets are sent
			pack = new Backpack();
			pack.Layer = Layer.ShopBuy;
			pack.Movable = false;
			pack.Visible = false;
			AddItem( pack );

			pack = new Backpack();
			pack.Layer = Layer.ShopResale;
			pack.Movable = false;
			pack.Visible = false;
			AddItem( pack );

			m_LastRestock = DateTime.Now;
		}

		public BaseVendor( Serial serial )
			: base( serial )
		{
		}

		public DateTime LastRestock { get { return m_LastRestock; } set { m_LastRestock = value; } }

		public virtual TimeSpan RestockDelay { get { return TimeSpan.FromHours( 1 ); } }

		public Container BuyPack
		{
			get
			{
				Container pack = FindItemOnLayer( Layer.ShopBuy ) as Container;

				if ( pack == null )
				{
					pack = new Backpack();
					pack.Layer = Layer.ShopBuy;
					pack.Visible = false;
					AddItem( pack );
				}

				return pack;
			}
		}

		public abstract void InitSBInfo();

		protected void LoadSBInfo()
		{
			m_LastRestock = DateTime.Now;

			SBInfos.Clear();

			InitSBInfo();

			m_ArmorBuyInfo.Clear();
			m_ArmorSellInfo.Clear();

			for ( int i = 0; i < SBInfos.Count; i++ )
			{
				SBInfo sbInfo = (SBInfo) SBInfos[i];
				m_ArmorBuyInfo.AddRange( sbInfo.BuyInfo );
				m_ArmorSellInfo.Add( sbInfo.SellInfo );
			}
		}

		public virtual bool GetGender()
		{
			return Utility.RandomBool();
		}

		/// <summary>
		/// Inits the vendor body. After invoke this method, vendor may have Stats, Gender, Race, Hue and Name initialized.
		/// </summary>
		public virtual void InitBody()
		{
			InitStats( 100, 100, 25 );

			SpeechHue = Utility.RandomDyedHue();
			Female = GetGender();

			Race = DefaultRace; // this inits body ID also

			Hue = Race.RandomSkinHue();

			InitHair();
			InitName();
		}

		protected virtual void InitHair()
		{
			int hairHue = GetHairHue();

			Utility.AssignRandomHair( this, hairHue );
			Utility.AssignRandomFacialHair( this, hairHue );
		}

		protected virtual void InitName()
		{
			// TODO (SA): Gargish names for Ter Mur vendors

			if ( Female )
				Name = NameList.RandomName( "female" );
			else
				Name = NameList.RandomName( "male" );
		}

		public virtual int GetRandomHue()
		{
			switch ( Utility.Random( 5 ) )
			{
				default:
				case 0:
					return Utility.RandomBlueHue();
				case 1:
					return Utility.RandomGreenHue();
				case 2:
					return Utility.RandomRedHue();
				case 3:
					return Utility.RandomYellowHue();
				case 4:
					return Utility.RandomNeutralHue();
			}
		}

		public virtual int GetShoeHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return 0;

			return Utility.RandomNeutralHue();
		}

		public virtual VendorShoeType ShoeType { get { return VendorShoeType.Shoes; } }

		public virtual int RandomBrightHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return Utility.RandomList( 0x62, 0x71 );

			return Utility.RandomList( 0x03, 0x0D, 0x13, 0x1C, 0x21, 0x30, 0x37, 0x3A, 0x44, 0x59 );
		}

		public virtual void CheckMorph()
		{
			if ( CheckGargoyle() )
				return;

			CheckNecromancer();
		}

		public virtual bool CheckGargoyle()
		{
			Map map = this.Map;

			if ( map != Map.Ilshenar )
				return false;

			if ( Region.Name != "Gargoyle City" )
				return false;

			if ( Body != 0x2F6 || ( Hue & 0x8000 ) == 0 )
				TurnToGargoyle();

			return true;
		}

		public virtual bool CheckNecromancer()
		{
			Map map = this.Map;

			if ( map != Map.Malas )
				return false;

			if ( Region.Name != "Umbra" )
				return false;

			if ( Hue != 0x83E8 )
				TurnToNecromancer();

			return true;
		}

		public override void OnAfterSpawn()
		{
			CheckMorph();
		}

		protected override void OnMapChange( Map oldMap )
		{
			base.OnMapChange( oldMap );

			CheckMorph();
		}

		public virtual int GetRandomNecromancerHue()
		{
			switch ( Utility.Random( 20 ) )
			{
				case 0:
					return 0;
				case 1:
					return 0x4E9;
				default:
					return Utility.RandomList( 0x485, 0x497 );
			}
		}

		public virtual void TurnToNecromancer()
		{
			foreach ( var item in this.GetEquippedItems() )
			{
				if ( item is BaseClothing || item is BaseWeapon || item is BaseArmor || item is BaseTool )
					item.Hue = GetRandomNecromancerHue();
			}

			HairHue = 0;
			FacialHairHue = 0;

			Hue = 0x83E8;
		}

		public virtual void TurnToGargoyle()
		{
			this.GetEquippedItems().OfType<BaseClothing>().Each( item => item.Delete() );

			HairItemID = 0;
			FacialHairItemID = 0;

			Body = 0x2F6;
			Hue = RandomBrightHue() | 0x8000;
			Name = NameList.RandomName( "gargoyle vendor" );

			CapitalizeTitle();
		}

		public virtual void CapitalizeTitle()
		{
			string title = this.Title;

			if ( title == null )
				return;

			string[] split = title.Split( ' ' );

			for ( int i = 0; i < split.Length; ++i )
			{
				if ( Insensitive.Equals( split[i], "the" ) )
					continue;

				if ( split[i].Length > 1 )
					split[i] = Char.ToUpper( split[i][0] ) + split[i].Substring( 1 );
				else if ( split[i].Length > 0 )
					split[i] = Char.ToUpper( split[i][0] ).ToString();
			}

			this.Title = String.Join( " ", split );
		}

		public virtual int GetHairHue()
		{
			return Utility.RandomHairHue();
		}

		public virtual void InitOutfit()
		{
			if ( Race == Race.Human )
			{
				#region Shirt
				switch ( Utility.Random( 3 ) )
				{
					case 0:
						AddItem( new FancyShirt( GetRandomHue() ) );
						break;
					case 1:
						AddItem( new Doublet( GetRandomHue() ) );
						break;
					case 2:
						AddItem( new Shirt( GetRandomHue() ) );
						break;
				}
				#endregion

				#region Shoes
				switch ( ShoeType )
				{
					case VendorShoeType.Shoes:
						AddItem( new Shoes( GetShoeHue() ) );
						break;
					case VendorShoeType.Boots:
						AddItem( new Boots( GetShoeHue() ) );
						break;
					case VendorShoeType.Sandals:
						AddItem( new Sandals( GetShoeHue() ) );
						break;
					case VendorShoeType.ThighBoots:
						AddItem( new ThighBoots( GetShoeHue() ) );
						break;
					case VendorShoeType.SamuraiTabi:
						AddItem( new SamuraiTabi( GetShoeHue() ) );
						break;
					case VendorShoeType.NinjaTabi:
						AddItem( new NinjaTabi( GetShoeHue() ) );
						break;
				}
				#endregion

				#region Pants
				if ( Female )
				{
					switch ( Utility.Random( 6 ) )
					{
						case 0:
							AddItem( new ShortPants( GetRandomHue() ) );
							break;
						case 1:
						case 2:
							AddItem( new Kilt( GetRandomHue() ) );
							break;
						case 3:
						case 4:
						case 5:
							AddItem( new Skirt( GetRandomHue() ) );
							break;
					}
				}
				else
				{
					switch ( Utility.Random( 2 ) )
					{
						case 0:
							AddItem( new LongPants( GetRandomHue() ) );
							break;
						case 1:
							AddItem( new ShortPants( GetRandomHue() ) );
							break;
					}
				}
				#endregion
			}
			// TODO: Elves outfit
			else if ( Race == Race.Gargoyle )
			{
				AddItem( new GargishClothLeggings( Utility.RandomNeutralHue() ) );
				AddItem( new GargishClothChest( GetRandomHue() ) );
				AddItem( new GargishClothArms( Utility.RandomNeutralHue() ) );
				AddItem( new GargishClothKilt( Utility.RandomNeutralHue() ) );
			}

			#region Gold
			PackGold( 100, 200 );
			#endregion
		}

		public virtual void Restock()
		{
			m_LastRestock = DateTime.Now;

			IBuyItemInfo[] buyInfo = this.GetBuyInfo();

			foreach ( IBuyItemInfo bii in buyInfo )
				bii.OnRestock();
		}

		public ArrayList Sort( ArrayList list )
		{
			ArrayList list_sort = new ArrayList();

			for ( int i = list.Count - 1; i >= 0; --i )
			{
				if ( i >= list.Count )
					continue;

				list_sort.Add( list[i] );
			}

			return list_sort;
		}

		private static TimeSpan InventoryDecayTime = TimeSpan.FromHours( 1.0 );

		public virtual void VendorBuy( Mobile from )
		{
			if ( !IsActiveSeller )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				Say( 501522 ); // I shall not treat with scum like thee!
				return;
			}

			if ( DateTime.Now - m_LastRestock > RestockDelay )
				Restock();

			UpdateBuyInfo();

			int count = 0;
			ArrayList list;
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();
			IShopSellInfo[] sellInfo = this.GetSellInfo();

			list = new ArrayList( buyInfo.Length );
			Container cont = this.BuyPack;

			ArrayList opls = new ArrayList();

			for ( int idx = 0; idx < buyInfo.Length; idx++ )
			{
				IBuyItemInfo buyItem = (IBuyItemInfo) buyInfo[idx];

				if ( buyItem.Amount <= 0 || list.Count >= 250 )
					continue;

				// NOTE: Only GBI supported; if you use another implementation of IBuyItemInfo, this will crash
				GenericBuyInfo gbi = (GenericBuyInfo) buyItem;
				IEntity disp = gbi.GetDisplayObject() as IEntity;

				list.Add( new BuyItemState( buyItem.Name, cont.Serial, disp == null ? (Serial) 0x7FC0FFEE : disp.Serial, buyItem.Price, buyItem.Amount, buyItem.ItemID, buyItem.Hue ) );
				count++;

				if ( disp is Item )
					opls.Add( ( disp as Item ).PropertyList );
				else if ( disp is Mobile )
					opls.Add( ( disp as Mobile ).PropertyList );
			}

			List<Item> playerItems = cont.Items;

			for ( int i = playerItems.Count - 1; i >= 0; --i )
			{
				if ( i >= playerItems.Count )
					continue;

				Item item = playerItems[i];

				if ( ( item.LastMoved + InventoryDecayTime ) <= DateTime.Now )
					item.Delete();
			}

			for ( int i = 0; i < playerItems.Count; ++i )
			{
				Item item = (Item) playerItems[i];

				int price = 0;
				string name = null;

				foreach ( IShopSellInfo ssi in sellInfo )
				{
					if ( ssi.IsSellable( item ) )
					{
						price = ssi.GetBuyPriceFor( item );
						name = ssi.GetNameFor( item );
						break;
					}
				}

				if ( name != null && list.Count < 250 )
				{
					list.Add( new BuyItemState( name, cont.Serial, item.Serial, price, item.Amount, item.ItemID, item.Hue ) );
					count++;

					opls.Add( item.PropertyList );
				}
			}

			//one (not all) of the packets uses a byte to describe number of items in the list.  Osi = dumb.
			//if ( list.Count > 255 )
			//	Console.WriteLine( "Vendor Warning: Vendor {0} has more than 255 buy items, may cause client errors!", this );

			if ( list.Count > 0 )
			{
				list.Sort( new BuyItemStateComparer() );

				list = Sort( list );

				SendPacksTo( from );

				from.Send( new VendorBuyContent( list ) );
				from.Send( new VendorBuyList( this, list ) );
				from.Send( new DisplayBuyList( this ) );
				from.Send( new MobileStatus( from ) ); //make sure their gold amount is sent

				for ( int i = 0; i < opls.Count; ++i )
					from.Send( opls[i] as Packet );

				SayTo( from, 500186 ); // Greetings.  Have a look around.
			}
		}

		public virtual void SendPacksTo( Mobile from )
		{
			Item pack = FindItemOnLayer( Layer.ShopBuy );

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Layer = Layer.ShopBuy;
				pack.Movable = false;
				pack.Visible = false;
				AddItem( pack );
			}

			from.Send( new EquipUpdate( pack ) );

			pack = FindItemOnLayer( Layer.ShopSell );

			if ( pack != null )
				from.Send( new EquipUpdate( pack ) );

			pack = FindItemOnLayer( Layer.ShopResale );

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Layer = Layer.ShopResale;
				pack.Movable = false;
				pack.Visible = false;
				AddItem( pack );
			}

			from.Send( new EquipUpdate( pack ) );
		}

		public virtual void VendorSell( Mobile from )
		{
			if ( !IsActiveBuyer )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				Say( 501522 ); // I shall not treat with scum like thee!
				return;
			}

			Container pack = from.Backpack;

			if ( pack != null )
			{
				IShopSellInfo[] info = GetSellInfo();

				Hashtable table = new Hashtable();

				foreach ( IShopSellInfo ssi in info )
				{
					Item[] items = pack.FindItemsByType( ssi.Types );

					foreach ( Item item in items )
					{
						if ( item is Container && ( (Container) item ).Items.Count != 0 )
							continue;

						if ( item.IsStandardLoot() && item.Movable && ssi.IsSellable( item ) )
							table[item] = new SellItemState( item, GetSellPriceFor( ssi, item ), ssi.GetNameFor( item ) );
					}
				}

				if ( table.Count > 0 )
				{
					SendPacksTo( from );

					from.Send( new VendorSellList( this, table ) );
				}
				else
				{
					Say( 1071140 ); // You have nothing I would be interested in.
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is SmallBOD || dropped is LargeBOD )
			{
				Account acc = from.Account as Account;

				if ( acc == null || acc.Trial )
				{
					SayTo( from, 1111859 ); // Trial accounts cannot turn in a bulk order.
					return false;
				}
				else if ( !IsValidBulkOrder( dropped ) || !SupportsBulkOrders( from ) )
				{
					SayTo( from, 1045130 ); // That order is for some other shopkeeper.
					return false;
				}
				else if ( ( dropped is SmallBOD && !( (SmallBOD) dropped ).Complete ) || ( dropped is LargeBOD && !( (LargeBOD) dropped ).Complete ) )
				{
					SayTo( from, 1045131 ); // You have not completed the order yet.
					return false;
				}

				Item reward;
				int gold, fame;

				if ( dropped is SmallBOD )
					( (SmallBOD) dropped ).GetRewards( out reward, out gold, out fame );
				else
					( (LargeBOD) dropped ).GetRewards( out reward, out gold, out fame );

				from.SendSound( 0x3D );

				if ( reward != null )
					from.AddToBackpack( reward );

				Banker.DepositUpTo( from, gold );
				from.SendLocalizedMessage( 1060397, gold.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.

				SayTo( from, 1045132 ); // Thank you so much!  Here is a reward for your effort.

				Titles.AwardFame( from, fame, true );

				ClearNextBulkOrder( from );

				dropped.Delete();

				return true;
			}

			return base.OnDragDrop( from, dropped );
		}

		private GenericBuyInfo LookupDisplayObject( object obj )
		{
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();

			for ( int i = 0; i < buyInfo.Length; ++i )
			{
				GenericBuyInfo gbi = buyInfo[i] as GenericBuyInfo;

				if ( gbi.GetDisplayObject() == obj )
					return gbi;
			}

			return null;
		}

		private void ProcessSinglePurchase( BuyItemResponse buy, IBuyItemInfo bii, ArrayList validBuy, ref int controlSlots, ref bool fullPurchase, ref int totalCost )
		{
			int amount = buy.Amount;

			if ( amount > bii.Amount )
				amount = bii.Amount;

			if ( amount <= 0 )
				return;

			int slots = bii.ControlSlots * amount;

			if ( controlSlots >= slots )
			{
				controlSlots -= slots;
			}
			else
			{
				fullPurchase = false;
				return;
			}

			totalCost += bii.Price * amount;
			validBuy.Add( buy );
		}

		private void ProcessValidPurchase( int amount, IBuyItemInfo bii, Mobile buyer, Container cont )
		{
			if ( amount > bii.Amount )
				amount = bii.Amount;

			if ( amount < 1 )
				return;

			bii.Amount -= amount;

			object o = bii.GetObject();

			if ( o is Item )
			{
				Item item = (Item) o;

				if ( item.Stackable )
				{
					item.Amount = amount;

					if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
						item.MoveToWorld( buyer.Location, buyer.Map );
				}
				else
				{
					item.Amount = 1;

					if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
						item.MoveToWorld( buyer.Location, buyer.Map );

					for ( int i = 1; i < amount; i++ )
					{
						item = bii.GetObject() as Item;

						if ( item != null )
						{
							item.Amount = 1;

							if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
								item.MoveToWorld( buyer.Location, buyer.Map );
						}
					}
				}
			}
			else if ( o is Mobile )
			{
				Mobile m = (Mobile) o;

				m.Direction = (Direction) Utility.Random( 8 );
				m.MoveToWorld( buyer.Location, buyer.Map );
				m.PlaySound( m.GetIdleSound() );

				if ( m is BaseCreature )
					( (BaseCreature) m ).SetControlMaster( buyer );

				for ( int i = 1; i < amount; ++i )
				{
					m = bii.GetObject() as Mobile;

					if ( m != null )
					{
						m.Direction = (Direction) Utility.Random( 8 );
						m.MoveToWorld( buyer.Location, buyer.Map );

						if ( m is BaseCreature )
							( (BaseCreature) m ).SetControlMaster( buyer );
					}
				}
			}
		}

		public virtual bool OnBuyItems( Mobile buyer, List<BuyItemResponse> list )
		{
			if ( !IsActiveSeller )
				return false;

			if ( !buyer.CheckAlive() )
				return false;

			if ( !CheckVendorAccess( buyer ) )
			{
				Say( 501522 ); // I shall not treat with scum like thee!
				return false;
			}

			UpdateBuyInfo();

			//IBuyItemInfo[] buyInfo = this.GetBuyInfo();
			IShopSellInfo[] info = GetSellInfo();
			int totalCost = 0;
			ArrayList validBuy = new ArrayList( list.Count );
			Container cont;
			bool bought = false;
			bool fromBank = false;
			bool fullPurchase = true;
			int controlSlots = buyer.FollowersMax - buyer.Followers;

			foreach ( BuyItemResponse buy in list )
			{
				Serial ser = buy.Serial;
				int amount = buy.Amount;

				if ( ser.IsItem )
				{
					Item item = World.Instance.FindItem( ser );

					if ( item == null )
						continue;

					GenericBuyInfo gbi = LookupDisplayObject( item );

					if ( gbi != null )
					{
						ProcessSinglePurchase( buy, gbi, validBuy, ref controlSlots, ref fullPurchase, ref totalCost );
					}
					else if ( item.RootParent == this )
					{
						if ( amount > item.Amount )
							amount = item.Amount;

						if ( amount <= 0 )
							continue;

						foreach ( IShopSellInfo ssi in info )
						{
							if ( ssi.IsSellable( item ) )
							{
								if ( ssi.IsResellable( item ) )
								{
									totalCost += ssi.GetBuyPriceFor( item ) * amount;
									validBuy.Add( buy );
									break;
								}
							}
						}
					}
				}
				else if ( ser.IsMobile )
				{
					Mobile mob = World.Instance.FindMobile( ser );

					if ( mob == null )
						continue;

					GenericBuyInfo gbi = LookupDisplayObject( mob );

					if ( gbi != null )
					{
						ProcessSinglePurchase( buy, gbi, validBuy, ref controlSlots, ref fullPurchase, ref totalCost );
					}
				}
			} //foreach

			if ( fullPurchase && validBuy.Count == 0 )
				SayTo( buyer, 500190 ); // Thou hast bought nothing!
			else if ( validBuy.Count == 0 )
				SayTo( buyer, 500187 ); // Your order cannot be fulfilled, please try again.

			if ( validBuy.Count == 0 )
				return false;

			bought = ( buyer.AccessLevel >= AccessLevel.GameMaster );

			cont = buyer.Backpack;
			if ( !bought && cont != null )
			{
				if ( cont.ConsumeTotal( typeof( Gold ), totalCost ) )
					bought = true;
				else if ( totalCost < 2000 )
					SayTo( buyer, 500192 ); // Begging thy pardon, but thou casnt afford that.
			}

			if ( !bought && totalCost >= 2000 )
			{
				cont = buyer.BankBox;

				if ( cont != null && cont.ConsumeTotal( typeof( Gold ), totalCost ) )
				{
					bought = true;
					fromBank = true;
				}
				else
				{
					SayTo( buyer, 500191 ); // Begging thy pardon, but thy bank account lacks these funds.
				}
			}

			if ( !bought )
				return false;
			else
				buyer.PlaySound( 0x32 );

			cont = buyer.Backpack;

			if ( cont == null )
				cont = buyer.BankBox;

			foreach ( BuyItemResponse buy in validBuy )
			{
				Serial ser = buy.Serial;
				int amount = buy.Amount;

				if ( amount < 1 )
					continue;

				if ( ser.IsItem )
				{
					Item item = World.Instance.FindItem( ser );

					if ( item == null )
						continue;

					GenericBuyInfo gbi = LookupDisplayObject( item );

					if ( gbi != null )
					{
						ProcessValidPurchase( amount, gbi, buyer, cont );
					}
					else
					{
						if ( amount > item.Amount )
							amount = item.Amount;

						foreach ( IShopSellInfo ssi in info )
						{
							if ( ssi.IsSellable( item ) )
							{
								if ( ssi.IsResellable( item ) )
								{
									Item buyItem;

									if ( amount >= item.Amount )
									{
										buyItem = item;
									}
									else
									{
										buyItem = Mobile.LiftItemDupe( item, item.Amount - amount );

										if ( buyItem == null )
											buyItem = item;
									}

									if ( cont == null || !cont.TryDropItem( buyer, buyItem, false ) )
										buyItem.MoveToWorld( buyer.Location, buyer.Map );

									break;
								}
							}
						}
					}
				}
				else if ( ser.IsMobile )
				{
					Mobile mob = World.Instance.FindMobile( ser );

					if ( mob == null )
						continue;

					GenericBuyInfo gbi = LookupDisplayObject( mob );

					if ( gbi != null )
						ProcessValidPurchase( amount, gbi, buyer, cont );
				}
			} //foreach

			if ( fullPurchase )
			{
				if ( buyer.AccessLevel >= AccessLevel.GameMaster )
					SayTo( buyer, true, "I would not presume to charge thee anything.  Here are the goods you requested." );
				else if ( fromBank )
					SayTo( buyer, true, "The total of thy purchase is {0} gold, which has been withdrawn from your bank account.  My thanks for the patronage.", totalCost );
				else
					SayTo( buyer, true, "The total of thy purchase is {0} gold.  My thanks for the patronage.", totalCost );
			}
			else
			{
				if ( buyer.AccessLevel >= AccessLevel.GameMaster )
					SayTo( buyer, true, "I would not presume to charge thee anything.  Unfortunately, I could not sell you all the goods you requested." );
				else if ( fromBank )
					SayTo( buyer, true, "The total of thy purchase is {0} gold, which has been withdrawn from your bank account.  My thanks for the patronage.  Unfortunately, I could not sell you all the goods you requested.", totalCost );
				else
					SayTo( buyer, true, "The total of thy purchase is {0} gold.  My thanks for the patronage.  Unfortunately, I could not sell you all the goods you requested.", totalCost );
			}

			return true;
		}

		public virtual bool CheckVendorAccess( Mobile from )
		{
			GuardedRegion reg = this.Region as GuardedRegion;

			if ( reg != null && !reg.CheckVendorAccess( this, from ) )
				return false;

			if ( this.Region != from.Region )
			{
				reg = from.Region as GuardedRegion;

				if ( reg != null && !reg.CheckVendorAccess( this, from ) )
					return false;
			}

			return true;
		}

		private int GetSellPriceFor( IShopSellInfo ssi, Item item )
		{
			int price = ssi.GetSellPriceFor( item );

			Item[] items = this.BuyPack.FindItemsByType( item.GetType() );
			int amount = 0;

			if ( item.Stackable )
				amount = items.Length;
			else
			{
				for ( int i = 0; i < items.Length; i++ )
				{
					amount += items[i].Amount;
				}
			}

			double scalar = 1.0 - ( amount / 250.0 );

			return Math.Max( 1, (int) ( price * scalar ) );
		}

		public virtual bool OnSellItems( Mobile seller, List<SellItemResponse> list )
		{
			if ( !IsActiveBuyer )
				return false;

			if ( !seller.CheckAlive() )
				return false;

			if ( !CheckVendorAccess( seller ) )
			{
				Say( 501522 ); // I shall not treat with scum like thee!
				return false;
			}

			seller.PlaySound( 0x32 );

			IShopSellInfo[] info = this.GetSellInfo();
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();
			int giveGold = 0;
			int sold = 0;
			Container cont;

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 )
					continue;

				foreach ( IShopSellInfo ssi in info )
				{
					if ( ssi.IsSellable( resp.Item ) )
					{
						sold++;
						break;
					}
				}
			}

			if ( sold > MaxSell )
			{
				SayTo( seller, true, "You may only sell {0} items at a time!", MaxSell );
				return false;
			}
			else if ( sold == 0 )
			{
				return true;
			}

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 )
					continue;

				foreach ( IShopSellInfo ssi in info )
				{
					if ( ssi.IsSellable( resp.Item ) )
					{
						int amount = resp.Amount;

						if ( amount > resp.Item.Amount )
							amount = resp.Item.Amount;

						if ( ssi.IsResellable( resp.Item ) )
						{
							bool found = false;

							foreach ( IBuyItemInfo bii in buyInfo )
							{
								if ( bii.Restock( resp.Item, amount ) )
								{
									resp.Item.Consume( amount );
									found = true;

									break;
								}
							}

							if ( !found )
							{
								cont = this.BuyPack;

								if ( amount < resp.Item.Amount )
								{
									Item item = Mobile.LiftItemDupe( resp.Item, resp.Item.Amount - amount );

									if ( item != null )
									{
										item.SetLastMoved();
										cont.DropItem( item );
									}
									else
									{
										resp.Item.SetLastMoved();
										cont.DropItem( resp.Item );
									}
								}
								else
								{
									resp.Item.SetLastMoved();
									cont.DropItem( resp.Item );
								}
							}
						}
						else
						{
							if ( amount < resp.Item.Amount )
								resp.Item.Amount -= amount;
							else
								resp.Item.Delete();
						}

						giveGold += GetSellPriceFor( ssi, resp.Item ) * amount;
						break;
					}
				}
			}

			if ( giveGold > 0 )
			{
				while ( giveGold > 60000 )
				{
					seller.AddToBackpack( new Gold( 60000 ) );
					giveGold -= 60000;
				}

				seller.AddToBackpack( new Gold( giveGold ) );

				seller.PlaySound( 0x0037 ); // Gold dropping sound

				if ( SupportsBulkOrders( seller ) )
				{
					Item bulkOrder = CreateBulkOrder( seller, false );

					if ( bulkOrder is LargeBOD )
						seller.SendGump( new LargeBODAcceptGump( seller, (LargeBOD) bulkOrder ) );
					else if ( bulkOrder is SmallBOD )
						seller.SendGump( new SmallBODAcceptGump( seller, (SmallBOD) bulkOrder ) );
				}
			}

			//no cliloc for this?
			//SayTo( seller, true, "Thank you! I bought {0} item{1}. Here is your {2}gp.", Sold, (Sold > 1 ? "s" : ""), GiveGold );

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			ArrayList sbInfos = this.SBInfos;

			for ( int i = 0; sbInfos != null && i < sbInfos.Count; ++i )
			{
				SBInfo sbInfo = (SBInfo) sbInfos[i];
				ArrayList buyInfo = sbInfo.BuyInfo;

				for ( int j = 0; buyInfo != null && j < buyInfo.Count; ++j )
				{
					GenericBuyInfo gbi = (GenericBuyInfo) buyInfo[j];

					int maxAmount = gbi.MaxAmount;
					int doubled = 0;

					switch ( maxAmount )
					{
						case 200:
							doubled = 1;
							break;
						case 400:
							doubled = 2;
							break;
						case 600:
							doubled = 3;
							break;
						case 800:
							doubled = 4;
							break;
						case 999:
							doubled = 5;
							break;
					}

					if ( doubled > 0 )
					{
						writer.WriteEncodedInt( 1 + ( ( j * sbInfos.Count ) + i ) );
						writer.WriteEncodedInt( doubled );
					}
				}
			}

			writer.WriteEncodedInt( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			LoadSBInfo();

			ArrayList sbInfos = this.SBInfos;

			switch ( version )
			{
				case 1:
					{
						int index;

						while ( ( index = reader.ReadEncodedInt() ) > 0 )
						{
							int doubled = reader.ReadEncodedInt();

							if ( sbInfos != null )
							{
								index -= 1;
								int sbInfoIndex = index % sbInfos.Count;
								int buyInfoIndex = index / sbInfos.Count;

								if ( sbInfoIndex >= 0 && sbInfoIndex < sbInfos.Count )
								{
									SBInfo sbInfo = (SBInfo) sbInfos[sbInfoIndex];
									ArrayList buyInfo = sbInfo.BuyInfo;

									if ( buyInfo != null && buyInfoIndex >= 0 && buyInfoIndex < buyInfo.Count )
									{
										GenericBuyInfo gbi = (GenericBuyInfo) buyInfo[buyInfoIndex];

										int amount = 100;

										switch ( doubled )
										{
											case 1:
												amount = 200;
												break;
											case 2:
												amount = 400;
												break;
											case 3:
												amount = 600;
												break;
											case 4:
												amount = 800;
												break;
											case 5:
												amount = 999;
												break;
										}

										gbi.Amount = gbi.MaxAmount = amount;
									}
								}
							}
						}

						break;
					}
			}

			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( CheckMorph ) );
		}

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive && IsActiveVendor )
			{
				if ( IsActiveSeller )
					list.Add( new VendorBuyEntry( from, this ) );

				if ( IsActiveBuyer )
					list.Add( new VendorSellEntry( from, this ) );

				if ( SupportsBulkOrders( from ) )
					list.Add( new BulkOrderInfoEntry( from, this ) );
			}

			base.AddCustomContextEntries( from, list );
		}

		public virtual IShopSellInfo[] GetSellInfo()
		{
			return (IShopSellInfo[]) m_ArmorSellInfo.ToArray( typeof( IShopSellInfo ) );
		}

		public virtual IBuyItemInfo[] GetBuyInfo()
		{
			return (IBuyItemInfo[]) m_ArmorBuyInfo.ToArray( typeof( IBuyItemInfo ) );
		}

		public override bool CanBeDamaged()
		{
			return !IsInvulnerable;
		}
	}
}

namespace Server.ContextMenus
{
	public class VendorBuyEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;

		public VendorBuyEntry( Mobile from, BaseVendor vendor )
			: base( 6103, 8 )
		{
			m_Vendor = vendor;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.VendorBuy( this.Owner.From );
		}
	}

	public class VendorSellEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;

		public VendorSellEntry( Mobile from, BaseVendor vendor )
			: base( 6104, 8 )
		{
			m_Vendor = vendor;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.VendorSell( this.Owner.From );
		}
	}
}

namespace Server
{
	public interface IShopSellInfo
	{
		// get display name for an item
		string GetNameFor( Item item );

		// get price for an item which the player is selling
		int GetSellPriceFor( Item item );

		// get price for an item which the player is buying
		int GetBuyPriceFor( Item item );

		// can we sell this item to this vendor?
		bool IsSellable( Item item );

		// What do we sell?
		Type[] Types { get; }

		// does the vendor resell this item?
		bool IsResellable( Item item );
	}

	public interface IBuyItemInfo
	{
		// get a new instance of an object (we just bought it)
		object GetObject();

		int ControlSlots { get; }

		int PriceScalar { get; set; }

		// display price of the item
		int Price { get; }

		// display name of the item
		string Name { get; }

		// display hue
		int Hue { get; }

		// display id
		int ItemID { get; }

		// amount in stock
		int Amount { get; set; }

		// max amount in stock
		int MaxAmount { get; }

		// Attempt to restock with item, (return true if restock sucessful)
		bool Restock( Item item, int amount );

		// called when its time for the whole shop to restock
		void OnRestock();
	}
}
