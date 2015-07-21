using System;
using Server;
using Server.Engines.Housing;
using Server.Network;
using Server.Targeting;
using Server.Engines.Plants;
using System.Collections;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Engines.Quests.Matriarch;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Events;

namespace Server.Items
{
	public enum BeverageType
	{
		Ale,
		Cider,
		Liquor,
		Milk,
		Wine,
		Water,
		Champagne,
		Gluhwein,
		Eggnog
	}

	public interface IHasQuantity
	{
		int Quantity { get; set; }
	}

	public interface IWaterSource : IHasQuantity
	{
	}

	// TODO: Flipable attributes

	[TypeAlias( "Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine" )]
	public class BeverageBottle : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1042959; } } // a bottle of Ale
		public override int MaxQuantity { get { return 5; } }
		public override bool Fillable { get { return false; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
			{
				switch ( Content )
				{
					case BeverageType.Ale:
						return 0x99F;
					case BeverageType.Cider:
						return 0x99F;
					case BeverageType.Liquor:
						return 0x99B;
					case BeverageType.Milk:
						return 0x99B;
					case BeverageType.Wine:
						return 0x9C7;
					case BeverageType.Water:
						return 0x99B;
				}
			}

			return 0;
		}

		[Constructable]
		public BeverageBottle( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public BeverageBottle( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						if ( CheckType( "BottleAle" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Ale;
						}
						else if ( CheckType( "BottleLiquor" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Liquor;
						}

						else if ( CheckType( "BottleWine" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Wine;
						}
						else
						{
							throw new Exception( this.GetType().FullName );
						}

						break;
					}
			}
		}
	}

	public class Jug : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1042965; } } // a jug of Ale
		public override int MaxQuantity { get { return 10; } }
		public override bool Fillable { get { return false; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
			{
				return 0x9C8;
			}

			return 0;
		}

		[Constructable]
		public Jug( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public Jug( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class CeramicMug : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1042982; } } // a ceramic mug of Ale
		public override int MaxQuantity { get { return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID >= 0x995 && ItemID <= 0x999 )
			{
				return ItemID;
			}
			else if ( ItemID == 0x9CA )
			{
				return ItemID;
			}

			return 0x995;
		}

		[Constructable]
		public CeramicMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public CeramicMug( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public CeramicMug( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class PewterMug : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1042994; } } // a pewter mug with Ale
		public override int MaxQuantity { get { return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID >= 0xFFF && ItemID <= 0x1002 )
			{
				return ItemID;
			}

			return 0xFFF;
		}

		[Constructable]
		public PewterMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public PewterMug( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public PewterMug( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class Goblet : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1043000; } } // a goblet of Ale
		public override int MaxQuantity { get { return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID == 0x99A || ItemID == 0x9B3 || ItemID == 0x9BF || ItemID == 0x9CB )
			{
				return ItemID;
			}

			return 0x99A;
		}

		[Constructable]
		public Goblet()
		{
			Weight = 1.0;
		}

		[Constructable]
		public Goblet( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public Goblet( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Items.MugAle", "Server.Items.GlassCider", "Server.Items.GlassLiquor", "Server.Items.GlassMilk", "Server.Items.GlassWine", "Server.Items.GlassWater" )]
	public class GlassMug : BaseBeverage
	{
		public override int EmptyLabelNumber { get { return 1022456; } } // mug
		public override int BaseLabelNumber { get { return 1042976; } } // a mug of Ale
		public override int MaxQuantity { get { return 5; } }

		public override int ComputeItemID()
		{
			if ( IsEmpty )
			{
				return ( ItemID >= 0x1F81 && ItemID <= 0x1F84 ? ItemID : 0x1F81 );
			}

			switch ( Content )
			{
				case BeverageType.Ale:
					return ( ItemID == 0x9EF ? 0x9EF : 0x9EE );
				case BeverageType.Cider:
					return ( ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D );
				case BeverageType.Liquor:
					return ( ItemID >= 0x1F85 && ItemID <= 0x1F88 ? ItemID : 0x1F85 );
				case BeverageType.Milk:
					return ( ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89 );
				case BeverageType.Wine:
					return ( ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D );
				case BeverageType.Water:
					return ( ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91 );
			}

			return 0;
		}

		[Constructable]
		public GlassMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public GlassMug( BeverageType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public GlassMug( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						if ( CheckType( "MugAle" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Ale;
						}
						else if ( CheckType( "GlassCider" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Cider;
						}
						else if ( CheckType( "GlassLiquor" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Liquor;
						}
						else if ( CheckType( "GlassMilk" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Milk;
						}
						else if ( CheckType( "GlassWine" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Wine;
						}
						else if ( CheckType( "GlassWater" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Water;
						}
						else
						{
							throw new Exception( this.GetType().FullName );
						}

						break;
					}
			}
		}
	}

	[TypeAlias( "Server.Items.PitcherAle", "Server.Items.PitcherCider", "Server.Items.PitcherLiquor", "Server.Items.PitcherMilk", "Server.Items.PitcherWine", "Server.Items.PitcherWater", "Server.Items.GlassPitcher" )]
	public class Pitcher : BaseBeverage
	{
		public override int BaseLabelNumber { get { return 1048128; } } // a Pitcher of Ale
		public override int MaxQuantity { get { return 5; } }

		public override int ComputeItemID()
		{
			if ( IsEmpty )
			{
				if ( ItemID == 0x9A7 || ItemID == 0xFF7 )
				{
					return ItemID;
				}

				return 0xFF6;
			}

			switch ( Content )
			{
				case BeverageType.Ale:
					{
						if ( ItemID == 0x1F96 )
						{
							return ItemID;
						}

						return 0x1F95;
					}
				case BeverageType.Cider:
					{
						if ( ItemID == 0x1F98 )
						{
							return ItemID;
						}

						return 0x1F97;
					}
				case BeverageType.Liquor:
					{
						if ( ItemID == 0x1F9A )
						{
							return ItemID;
						}

						return 0x1F99;
					}

				case BeverageType.Milk:
					{
						if ( ItemID == 0x9AD )
						{
							return ItemID;
						}

						return 0x9F0;
					}
				case BeverageType.Wine:
					{
						if ( ItemID == 0x1F9C )
						{
							return ItemID;
						}

						return 0x1F9B;
					}
				case BeverageType.Water:
					{
						if ( ItemID == 0xFF8 || ItemID == 0xFF9 || ItemID == 0x1F9E )
						{
							return ItemID;
						}

						return 0x1F9D;
					}
			}

			return 0;
		}

		[Constructable]
		public Pitcher()
		{
			Weight = 2.0;
		}

		[Constructable]
		public Pitcher( BeverageType type )
			: base( type )
		{
			Weight = 2.0;
		}

		public Pitcher( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			if ( CheckType( "PitcherWater" ) || CheckType( "GlassPitcher" ) )
			{
				base.Deserialize( reader, false );
			}
			else
			{
				base.Deserialize( reader, true );
			}

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						if ( CheckType( "PitcherAle" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Ale;
						}
						else if ( CheckType( "PitcherCider" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Cider;
						}
						else if ( CheckType( "PitcherLiquor" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Liquor;
						}
						else if ( CheckType( "PitcherMilk" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Milk;
						}
						else if ( CheckType( "PitcherWine" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Wine;
						}
						else if ( CheckType( "PitcherWater" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageType.Water;
						}
						else if ( CheckType( "GlassPitcher" ) )
						{
							Quantity = 0;
							Content = BeverageType.Water;
						}
						else
						{
							throw new Exception( this.GetType().FullName );
						}

						break;
					}
			}
		}
	}

	public abstract class BaseBeverage : Item, IHasQuantity
	{
		private BeverageType m_Content;
		private int m_Quantity;
		private Mobile m_Poisoner;
		private Poison m_Poison;

		public override int LabelNumber
		{
			get
			{
				int num = BaseLabelNumber;

				if ( IsEmpty || num == 0 )
				{
					return EmptyLabelNumber;
				}

				return BaseLabelNumber + (int) m_Content;
			}
		}

		public virtual bool ShowQuantity { get { return ( MaxQuantity > 1 ); } }
		public virtual bool Fillable { get { return true; } }
		public virtual bool Pourable { get { return true; } }

		public virtual int EmptyLabelNumber { get { return base.LabelNumber; } }
		public virtual int BaseLabelNumber { get { return 0; } }

		public abstract int MaxQuantity { get; }
		public abstract int ComputeItemID();

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsEmpty { get { return ( m_Quantity <= 0 ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ContainsAlchohol { get { return ( !IsEmpty && m_Content != BeverageType.Milk && m_Content != BeverageType.Water ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsFull { get { return ( m_Quantity >= MaxQuantity ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison { get { return m_Poison; } set { m_Poison = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner { get { return m_Poisoner; } set { m_Poisoner = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public BeverageType Content
		{
			get { return m_Content; }
			set
			{
				m_Content = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if ( itemID > 0 )
				{
					ItemID = itemID;
				}
				else
				{
					Delete();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get { return m_Quantity; }
			set
			{
				if ( value < 0 )
				{
					value = 0;
				}
				else if ( value > MaxQuantity )
				{
					value = MaxQuantity;
				}

				m_Quantity = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if ( itemID > 0 )
				{
					ItemID = itemID;
				}
				else
				{
					Delete();
				}
			}
		}

		public virtual int GetQuantityDescription()
		{
			int perc = ( m_Quantity * 100 ) / MaxQuantity;

			if ( perc <= 0 )
			{
				return 1042975; // It's empty.
			}
			else if ( perc <= 33 )
			{
				return 1042974; // It's nearly empty.
			}
			else if ( perc <= 66 )
			{
				return 1042973; // It's half full.
			}
			else
			{
				return 1042972; // It's full.
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ShowQuantity )
			{
				list.Add( GetQuantityDescription() );
			}
		}

		public virtual bool ValidateUse( Mobile from, bool message )
		{
			if ( Deleted )
			{
				return false;
			}

			if ( !Movable && !Fillable )
			{
				IHouse house = HousingHelper.FindHouseAt( this );

				if ( house == null || !house.IsLockedDown( this ) )
				{
					if ( message )
					{
						from.SendLocalizedMessage( 502946, "", 0x59 ); // That belongs to someone else.
					}

					return false;
				}
			}

			if ( from.Map != Map || !from.InRange( GetWorldLocation(), 2 ) || !from.InLOS( this ) )
			{
				if ( message )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				}

				return false;
			}

			return true;
		}

		public virtual void Fill_OnTarget( Mobile from, object targ )
		{
			if ( !IsEmpty || !Fillable || !ValidateUse( from, false ) )
				return;

			if ( targ is BaseBeverage )
			{
				BaseBeverage bev = (BaseBeverage) targ;

				if ( bev.IsEmpty || !bev.ValidateUse( from, true ) )
					return;

				this.Content = bev.Content;
				this.Poison = bev.Poison;
				this.Poisoner = bev.Poisoner;

				if ( bev.Quantity > this.MaxQuantity )
				{
					this.Quantity = this.MaxQuantity;
					bev.Quantity -= this.MaxQuantity;
				}
				else
				{
					this.Quantity += bev.Quantity;
					bev.Quantity = 0;
				}
			}
			else if ( targ is StaticTarget && IsWaterSource( ( (StaticTarget) targ ).ItemID ) )
			{
				this.Content = BeverageType.Water;
				this.Poison = null;
				this.Poisoner = null;

				this.Quantity = this.MaxQuantity;

				from.SendLocalizedMessage( 1010089 ); // You fill the container with water.
			}
			else if ( targ is Item )
			{
				Item item = (Item) targ;
				IWaterSource src = item as IWaterSource;

				if ( src == null && item is AddonComponent )
					src = ( (AddonComponent) item ).Addon as IWaterSource;

				if ( src == null || src.Quantity <= 0 )
					return;

				if ( from.Map != item.Map || !from.InRange( item.GetWorldLocation(), 2 ) || !from.InLOS( item ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
					return;
				}

				this.Content = BeverageType.Water;
				this.Poison = null;
				this.Poisoner = null;

				if ( src.Quantity > this.MaxQuantity )
				{
					this.Quantity = this.MaxQuantity;
					src.Quantity -= this.MaxQuantity;
				}
				else
				{
					this.Quantity += src.Quantity;
					src.Quantity = 0;
				}

				from.SendLocalizedMessage( 1010089 ); // You fill the container with water.
			}
			else if ( targ is Cow )
			{
				Cow cow = (Cow) targ;

				if ( cow.TryMilk( from ) )
				{
					Content = BeverageType.Milk;
					Quantity = MaxQuantity;
					from.SendLocalizedMessage( 1080197 ); // You fill the container with milk.
				}
			}
			else if ( targ is LandTarget )
			{
				int tileID = ( (LandTarget) targ ).TileID;

				PlayerMobile player = from as PlayerMobile;

				if ( player != null )
				{
					QuestSystem qs = player.Quest;

					if ( qs is WitchApprenticeQuest )
					{
						FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;

						if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.SwampWater )
						{
							bool contains = false;

							for ( int i = 0; !contains && i < m_SwampTiles.Length; i += 2 )
							{
								contains = ( tileID >= m_SwampTiles[i] && tileID <= m_SwampTiles[i + 1] );
							}

							if ( contains )
							{
								Delete();

								player.SendLocalizedMessage( 1055035 ); // You dip the container into the disgusting swamp water, collecting enough for the Hag's vile stew.
								obj.Complete();
							}
						}
					}
				}
			}
		}

		private static int[] m_SwampTiles = new int[]
			{
				0x9C4, 0x9EB,
				0x3D65, 0x3D65,
				0x3DC0, 0x3DD9,
				0x3DDB, 0x3DDC,
				0x3DDE, 0x3EF0,
				0x3FF6, 0x3FF6,
				0x3FFC, 0x3FFE,
		};

		private static bool IsWaterSource( int id )
		{
			for ( int i = 0; i < m_WaterSources.Length; ++i )
			{
				if ( id == m_WaterSources[i] )
					return true;
			}

			return false;
		}
		private static int[] m_WaterSources = new int[]
			{
				0xB41,	0xB42,	0xB43,	0xB44,
				0xE23,	0xE7B,	0xFFA,	0x154D,
				0x1552, 0x1553, 0x1554, 0x1559,
				0x1796, 0x1797, 0x1798,	0x1799,
				0x179A,	0x179B, 0x179C,	0x179D,
				0x179E, 0x179F, 0x17A0,	0x17A1,
				0x17A2,	0x17A3,	0x17A4,	0x17A5,
				0x17A6,	0x17A7,	0x17A8,	0x17A9,
				0x17AA,	0x17AB,	0x17AC,	0x17AD,
				0x17AE,	0x17AF,	0x17B0,	0x17B1,
				0x17B2, 0x2004,	0x346E, 0x346F,
				0x3470, 0x3471, 0x3472, 0x3473,
				0x3474, 0x3475, 0x3476, 0x3477,
				0x3478, 0x3479, 0x347A, 0x347B,
				0x347C, 0x347D, 0x347E, 0x347F,
				0x3480, 0x3481, 0x3482, 0x3483,
				0x3484, 0x3485, 0x3490, 0x3491,
				0x3492, 0x3493, 0x3494, 0x3495,
				0x3496, 0x3497, 0x3498, 0x3499,
				0x349A, 0x349B, 0x349C, 0x349D,
				0x349E, 0x349F, 0x34A0, 0x34A1,
				0x34A2, 0x34A3, 0x34A4, 0x34A5,
				0x34A6, 0x34A7, 0x34A8, 0x34A9,
				0x34AA, 0x34AB, 0x34B5, 0x34B6,
				0x34B7, 0x34B8, 0x34B9, 0x34BA,
				0x34BB, 0x34BC, 0x34BD, 0x34BE,
				0x34BF, 0x34C0, 0x34C1, 0x34C2,
				0x34C3, 0x34C4, 0x34C5, 0x34C6,
				0x34C7, 0x34C8, 0x34C9, 0x34CA,
				0x34CB, 0x34CC, 0x34CD, 0x34CE,
				0x34CF, 0x34D0, 0x34D1, 0x34D2,
				0x34D3, 0x34D4, 0x34D5, 0x34ED,
				0x34EE, 0x34EF, 0x34F0, 0x34F0,
				0x34F0, 0x34F0, 0x34F0, 0x34F0,
				0x34F0, 0x34F0, 0x34F0, 0x34F0,
				0x34F0, 0x34F0, 0x34F0, 0x34F0,
				0x34F0, 0x34FF, 0x3500, 0x3501,
				0x3502, 0x3503, 0x3504, 0x3505,
				0x3506, 0x3507, 0x3508, 0x3509,
				0x350A, 0x350B, 0x350C, 0x350D,
				0x350E, 0x350F, 0x3510, 0x3511,
				0x3512, 0x3513, 0x3514, 0x3515,
				0x3516, 0x3517, 0x3518, 0x3519,
				0x351A, 0x351B, 0x351C, 0x351D,
				0x351E, 0x351F, 0x3520, 0x3521,
				0x3522, 0x3523, 0x3524, 0x3525,
				0x3526, 0x3527, 0x3528, 0x3529,
				0x352A, 0x352B, 0x352C, 0x352D,
				0x352E, 0x352F, 0x3530, 
		};

		#region Effects of achohol
		private static Hashtable m_Table = new Hashtable();

		public static void Initialize()
		{
			EventSink.Instance.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			CheckHeaveTimer( e.Mobile );
		}

		public static void CheckHeaveTimer( Mobile from )
		{
			if ( from.BAC > 0 && from.Map != Map.Internal && !from.Deleted )
			{
				Timer t = (Timer) m_Table[from];

				if ( t == null )
				{
					if ( from.BAC > 60 )
					{
						from.BAC = 60;
					}

					t = new HeaveTimer( from );
					t.Start();

					m_Table[from] = t;
				}
			}
			else
			{
				Timer t = (Timer) m_Table[from];

				if ( t != null )
				{
					t.Stop();
					m_Table.Remove( from );

					from.SendLocalizedMessage( 500850 ); // You feel sober.
				}
			}
		}

		private class HeaveTimer : Timer
		{
			private Mobile m_Drunk;

			public HeaveTimer( Mobile drunk )
				: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Drunk = drunk;

			}

			protected override void OnTick()
			{
				if ( m_Drunk.Deleted || m_Drunk.Map == Map.Internal )
				{
					Stop();
					m_Table.Remove( m_Drunk );
				}
				else if ( m_Drunk.Alive )
				{
					if ( m_Drunk.BAC > 60 )
					{
						m_Drunk.BAC = 60;
					}

					// chance to get sober
					if ( 10 > Utility.Random( 100 ) )
					{
						--m_Drunk.BAC;
					}

					// lose some stats
					m_Drunk.Stam -= 1;
					m_Drunk.Mana -= 1;

					if ( Utility.Random( 1, 4 ) == 1 )
					{
						if ( !m_Drunk.Mounted )
						{
							// turn in a random direction
							m_Drunk.Direction = (Direction) Utility.Random( 8 );

							// heave
							m_Drunk.Animate( 32, 5, 1, true, false, 0 );
						}

						// *hic*
						m_Drunk.PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, 500849 );
					}

					if ( m_Drunk.BAC <= 0 )
					{
						Stop();
						m_Table.Remove( m_Drunk );

						if ( BottleOfPungentBrew.UnderEffect( m_Drunk ) )
							m_Drunk.SendLocalizedMessage( 1095141 ); // You feel sober, but you sure do stink!
						else
							m_Drunk.SendLocalizedMessage( 500850 ); // You feel sober.
					}
				}
			}
		}
		#endregion

		public virtual void Pour_OnTarget( Mobile from, object targ )
		{
			if ( IsEmpty || !Pourable || !ValidateUse( from, false ) )
			{
				return;
			}

			if ( targ is BaseBeverage )
			{
				BaseBeverage bev = (BaseBeverage) targ;

				if ( !bev.ValidateUse( from, true ) )
				{
					return;
				}

				if ( bev.IsFull && bev.Content == this.Content )
				{
					from.SendLocalizedMessage( 500848 ); // Couldn't pour it there.  It was already full.
				}
				else if ( !bev.IsEmpty )
				{
					from.SendLocalizedMessage( 500846 ); // Can't pour it there.
				}
				else
				{
					bev.Content = this.Content;
					bev.Poison = this.Poison;
					bev.Poisoner = this.Poisoner;

					if ( this.Quantity > bev.MaxQuantity )
					{
						bev.Quantity = bev.MaxQuantity;
						this.Quantity -= bev.MaxQuantity;
					}
					else
					{
						bev.Quantity += this.Quantity;
						this.Quantity = 0;
					}

					from.PlaySound( 0x4E );
				}
			}
			else if ( from == targ )
			{
				if ( from.Thirst < 20 )
				{
					from.Thirst += 1;
				}

				if ( ContainsAlchohol )
				{
					int bac = 0;

					switch ( this.Content )
					{
						case BeverageType.Ale:
							bac = 1;
							break;
						case BeverageType.Wine:
							bac = 2;
							break;
						case BeverageType.Cider:
							bac = 3;
							break;
						case BeverageType.Liquor:
							bac = 4;
							break;
						case BeverageType.Champagne:
							bac = 5;
							break;
						case BeverageType.Gluhwein:
							bac = 6;
							break;
						case BeverageType.Eggnog:
							bac = 7;
							break;
					}

					from.BAC += bac;

					if ( from.BAC > 60 )
					{
						from.BAC = 60;
					}

					CheckHeaveTimer( from );
				}

				from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );

				if ( m_Poison != null )
				{
					from.ApplyPoison( m_Poisoner, m_Poison );
				}

				--Quantity;
			}
			else if ( targ is PlantItem )
			{
				( (PlantItem) targ ).Pour( from, this );
			}
			else if ( targ is AddonComponent && ( ( (AddonComponent) targ ).Addon is WaterVatEast || ( (AddonComponent) targ ).Addon is WaterVatSouth ) && this.Content == BeverageType.Water )
			{
				PlayerMobile player = from as PlayerMobile;

				if ( player != null )
				{
					SolenMatriarchQuest qs = player.Quest as SolenMatriarchQuest;

					if ( qs != null )
					{
						QuestObjective obj = qs.FindObjective( typeof( GatherWaterObjective ) );

						if ( obj != null && !obj.Completed )
						{
							BaseAddon vat = ( (AddonComponent) targ ).Addon;

							if ( vat.X > 5784 && vat.X < 5814 && vat.Y > 1903 && vat.Y < 1934 && ( ( qs.RedSolen && vat.Map == Map.Trammel ) || ( !qs.RedSolen && vat.Map == Map.Felucca ) ) )
							{
								if ( obj.CurProgress + Quantity > obj.MaxProgress )
								{
									int delta = obj.MaxProgress - obj.CurProgress;

									Quantity -= delta;
									obj.CurProgress = obj.MaxProgress;
								}
								else
								{
									obj.CurProgress += Quantity;
									Quantity = 0;
								}
							}
						}
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 500846 ); // Can't pour it there.
			}
		}

		#region FireBomb
		public virtual object FindParent( Mobile from )
		{
			Mobile m = this.HeldBy;

			if ( m != null && m.Holding == this )
			{
				return m;
			}

			object obj = this.RootParent;

			if ( obj != null )
			{
				return obj;
			}

			if ( Map == Map.Internal )
			{
				return from;
			}

			return this;
		}

		private Timer m_Timer;

		private void Detonate_OnTick( object state )
		{
			if ( Deleted )
			{
				return;
			}

			object[] states = (object[]) state;
			Mobile from = (Mobile) states[0];
			int timer = (int) states[1];

			object parent = FindParent( from );

			if ( timer == 0 )
			{
				Point3D loc;
				Map map;
				bool firefield = false;

				if ( parent is Item )
				{
					Item item = (Item) parent;

					loc = item.GetWorldLocation();
					map = item.Map;
					firefield = true;
				}
				else if ( parent is Mobile )
				{
					Mobile m = (Mobile) parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode( from, firefield, loc, map );
			}
			else
			{
				if ( parent is Item )
				{
					( (Item) parent ).PublicOverheadMessage( MessageType.Regular, 0x3B2, false, timer.ToString() );
				}
				else if ( parent is Mobile )
				{
					( (Mobile) parent ).Send( new AsciiMessage( Serial.MinusOne, 0x99B, MessageType.Regular, 0x3B2, 3, "", timer.ToString() ) );
				}

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted )
			{
				return;
			}

			object[] states = (object[]) state;
			//Mobile from = (Mobile) states[0];
			IPoint3D p = (IPoint3D) states[1];
			Map map = (Map) states[2];

			Point3D loc = new Point3D( p );

			MoveToWorld( loc, map );
		}

		private class ThrowTarget : Target
		{
			private BaseBeverage m_Bomb;

			public BaseBeverage Bomb { get { return m_Bomb; } }

			public ThrowTarget( BaseBeverage bomb )
				: base( 12, true, TargetFlags.None )
			{
				m_Bomb = bomb;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bomb.Deleted || m_Bomb.Map == Map.Internal )
				{
					return;
				}

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
				{
					return;
				}

				Map map = from.Map;

				if ( map == null )
				{
					return;
				}

				SpellHelper.GetSurfaceTop( ref p );

				from.RevealingAction();

				IEntity to;

				if ( p is Mobile )
				{
					to = (Mobile) p;
				}
				else
				{
					to = new DummyEntity( Serial.Zero, new Point3D( p ), map );
				}

				Effects.SendMovingEffect( from, to, m_Bomb.ItemID, 7, 0, false, false, m_Bomb.Hue, 0 );

				m_Bomb.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Bomb.Reposition_OnTick ), new object[] { from, p, map } );
			}
		}

		public void Explode( Mobile from, bool firefield, Point3D loc, Map map )
		{
			if ( Deleted )
			{
				return;
			}

			Delete();

			if ( map == null )
			{
				return;
			}

			if ( firefield )
			{
				Effects.SendPacket( from, map, new HuedEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x376A, loc, loc, 9, 10, true, false, 0, 0 ) );

				new FireFieldSpell.InternalItem( 0x37C3, loc, from, map, TimeSpan.FromSeconds( 10.0 ), 0 );
				new FireFieldSpell.InternalItem( 0x398C, loc, from, map, TimeSpan.FromSeconds( 10.0 ), 0 );
			}
			else
			{
				from.SendLocalizedMessage( 1060583 ); // The firebomb explodes in your hand!

				AOS.Damage( from, null, Utility.RandomMinMax( 8, 12 ), 0, 100, 0, 0, 0 );
			}
		}
		#endregion

		public override void OnDoubleClick( Mobile from )
		{
			if ( Hue == 0x489 ) // firebomb
			{
				ThrowTarget targ = from.Target as ThrowTarget;

				if ( targ != null && targ.Bomb == this )
				{
					return;
				}

				from.RevealingAction();

				from.Target = new ThrowTarget( this );

				if ( m_Timer == null )
				{
					from.SendLocalizedMessage( 1060582 ); // You light the firebomb.  Throw it now!
					m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), TimeSpan.FromSeconds( 1.0 ), 4, new TimerStateCallback( Detonate_OnTick ), new object[] { from, 3 } );
				}
				else
				{
					from.SendLocalizedMessage( 1060581 ); // You've already lit it!  Better throw it now!
				}

				return;
			}

			if ( IsEmpty )
			{
				if ( !Fillable || !ValidateUse( from, true ) )
				{
					return;
				}

				from.BeginTarget( -1, true, TargetFlags.None, new TargetCallback( Fill_OnTarget ) );
				SendLocalizedMessageTo( from, 500837 ); // Fill from what?
			}
			else if ( Pourable && ValidateUse( from, true ) )
			{
				from.BeginTarget( -1, true, TargetFlags.None, new TargetCallback( Pour_OnTarget ) );
				from.SendLocalizedMessage( 1010086 ); // What do you want to use this on?
			}
		}

		public static bool ConsumeTotal( Container pack, BeverageType content, int quantity )
		{
			return ConsumeTotal( pack, typeof( BaseBeverage ), content, quantity );
		}

		public static bool ConsumeTotal( Container pack, Type itemType, BeverageType content, int quantity )
		{
			Item[] items = pack.FindItemsByType( itemType );

			// First pass, compute total
			int total = 0;

			for ( int i = 0; i < items.Length; ++i )
			{
				BaseBeverage bev = items[i] as BaseBeverage;

				if ( bev != null && bev.Content == content && !bev.IsEmpty )
				{
					total += bev.Quantity;
				}
			}

			if ( total >= quantity )
			{
				// We've enough, so consume it

				int need = quantity;

				for ( int i = 0; i < items.Length; ++i )
				{
					BaseBeverage bev = items[i] as BaseBeverage;

					if ( bev == null || bev.Content != content || bev.IsEmpty )
					{
						continue;
					}

					int theirQuantity = bev.Quantity;

					if ( theirQuantity < need )
					{
						bev.Quantity = 0;
						need -= theirQuantity;
					}
					else
					{
						bev.Quantity -= need;
						return true;
					}
				}
			}

			return false;
		}

		public BaseBeverage()
		{
			ItemID = ComputeItemID();
		}

		public BaseBeverage( BeverageType type )
		{
			m_Content = type;
			m_Quantity = MaxQuantity;
			ItemID = ComputeItemID();
		}

		public BaseBeverage( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (Mobile) m_Poisoner );

			Poison.Serialize( m_Poison, writer );
			writer.Write( (int) m_Content );
			writer.Write( (int) m_Quantity );
		}

		protected bool CheckType( string name )
		{
			return ( this.GetType().FullName == String.Format( "Server.Items.{0}", name ) );
		}

		public override void Deserialize( GenericReader reader )
		{
			Deserialize( reader, true );
		}

		public void Deserialize( GenericReader reader, bool read )
		{
			base.Deserialize( reader );

			if ( !read )
			{
				return;
			}

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Poisoner = reader.ReadMobile();
						goto case 0;
					}
				case 0:
					{
						m_Poison = Poison.Deserialize( reader );
						m_Content = (BeverageType) reader.ReadInt();
						m_Quantity = reader.ReadInt();
						break;
					}
			}
		}
	}
}
