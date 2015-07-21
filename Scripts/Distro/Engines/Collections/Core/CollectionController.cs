using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Engines.Collections
{
	public abstract class CollectionController : Item
	{
		public static ArrayList WorldCollections = new ArrayList();

		public override int LabelNumber { get { return 1073436; } } // Donation Box

		private Hashtable m_Table = new Hashtable();
		public Hashtable Table { get { return m_Table; } set { m_Table = value; } }

		public abstract IRewardEntry[] Rewards { get; }
		public abstract DonationEntry[] Donations { get; }

		public abstract int PointsPerTier { get; }
		public abstract int MaxTiers { get; }

		private int m_Tier, m_Points;

		[CommandProperty( AccessLevel.Owner )]
		public int Tier
		{
			get { return m_Tier; }
			set
			{
				if ( m_Tier < 0 || m_Tier > MaxTiers )
					return;

				m_Tier = value;
				InvalidateProperties();

				if ( this is BritainLibraryCollection )
				{
					BritainLibraryCollection col = this as BritainLibraryCollection;

					if ( col.Representative != null )
						col.InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.Owner )]
		public int Points
		{
			get { return m_Points; }
			set
			{
				m_Points = value;

				if ( m_Points > PointsPerTier && m_Tier < MaxTiers )
				{
					m_Points -= PointsPerTier;
					m_Tier++;

					OnTierAdvanced();
				}

				if ( m_Points < 0 )
				{
					if ( m_Tier <= 0 )
					{
						m_Tier = 0;
						m_Points = 0;
					}
					else
					{
						m_Tier--;
						m_Points += PointsPerTier;
					}

					OnTierDecreased();
				}

				InvalidateProperties();

				if ( this is BritainLibraryCollection )
				{
					BritainLibraryCollection col = this as BritainLibraryCollection;

					if ( col.Representative != null )
						col.Representative.InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.Owner )]
		public int PointsUntilNextTier
		{
			get
			{
				if ( m_Tier == MaxTiers )
					return 0;

				return PointsPerTier - Points;
			}
		}

		public CollectionController()
			: this( 0x9AA, 1165 )
		{
		}

		public CollectionController( int itemid, int hue )
			: base( itemid )
		{
			Hue = hue;
			WorldCollections.Add( this );
			Movable = false;
		}

		public CollectionController( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072819, m_Tier.ToString() ); // Current Tier: ~1_TIER~
			list.Add( 1072820, m_Points.ToString() ); // Current Points: ~1_POINTS~
			list.Add( 1072821, PointsUntilNextTier.ToString() ); // Points until next tier: ~1_POINTS~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			base.OnDoubleClick( from );

			from.CloseGump( typeof( CollectionDonateGump ) );
			from.CloseGump( typeof( CollectionRewardGump ) );
			from.CloseGump( typeof( CollectionConfirmReward ) );
			from.CloseGump( typeof( CollectionSelectHueGump ) );

			from.SendGump( new CollectionDonateGump( this, from ) );
		}

		public void Award( Mobile from, int points )
		{
			// Chequeamos la tabla de la colección
			if ( !m_Table.Contains( from ) )
				m_Table.Add( from, ( Misc.TestCenter.Enabled ? 5000000 : 0 ) );

			from.SendLocalizedMessage( 1072816 ); // Thank you for your donation!

			m_Table[from] = (int) m_Table[from] + points;
			from.SendLocalizedMessage( 1072817, points.ToString() ); // You have earned ~1_POINTS~ reward points for this donation.

			Points += points;
			from.SendLocalizedMessage( 1072818, points.ToString() ); // The Collection has been awarded ~1_POINTS~ points
		}

		public virtual void OnTierAdvanced()
		{
		}

		public virtual void OnTierDecreased()
		{
		}

		public virtual void OnRewardCreated( Mobile from, Item reward )
		{
		}

		public bool CheckType( DonationEntry entry, Type type )
		{
			if ( entry.Type == typeof( TimberWolf ) && ( type == typeof( DireWolf ) || type == typeof( GreyWolf ) || type == typeof( WhiteWolf ) ) )
				return true;

			if ( entry.Type == typeof( Dragon ) && ( type == typeof( Drake ) || type == typeof( WhiteWyrm ) ) )
				return true;

			if ( entry.Type == typeof( Unicorn ) && ( type == typeof( Kirin ) || type == typeof( Nightmare ) || type == typeof( FireSteed ) || type == typeof( SwampDragon ) || type == typeof( CuSidhe ) ) )
				return true;

			if ( entry.Type == typeof( RuneBeetle ) && ( type == typeof( FireBeetle ) || type == typeof( Beetle ) ) )
				return true;


			return entry.Type == type;
		}

		public static bool CheckTreasureMap( TreasureMap map, double award )
		{
			if ( map.Level == 1 && award == 5 )
				return true;

			if ( map.Level == 2 && award == 8 )
				return true;

			if ( map.Level == 3 && award == 11 )
				return true;

			if ( map.Level == 4 && award == 14 )
				return true;

			if ( map.Level == 5 && award == 17 )
				return true;

			if ( map.Level == 6 && award == 20 )
				return true;

			return false;
		}

		public bool CheckType( object o, out double award )
		{
			foreach ( DonationEntry entry in Donations )
			{
				if ( o is TreasureMap )
				{
					if ( CheckTreasureMap( o as TreasureMap, entry.Award ) )
					{
						award = entry.Award;
						return true;
					}
				}
				else if ( CheckType( entry, o.GetType() ) )
				{
					award = entry.Award;
					return true;
				}
			}

			award = 0;
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( m_Tier );
			writer.Write( m_Points );

			writer.Write( m_Table.Count );

			foreach ( Mobile m in m_Table.Keys )
			{
				writer.Write( m );
				writer.Write( (int) m_Table[m] );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			reader.ReadInt();

			m_Tier = reader.ReadInt();
			m_Points = reader.ReadInt();

			m_Table = new Hashtable();
			int count = reader.ReadInt();

			for ( int i = 0; i < count; i++ )
			{
				Mobile m = reader.ReadMobile();
				int points = reader.ReadInt();

				if ( m != null )
					m_Table.Add( m, points );
			}

			WorldCollections.Add( this );
		}
	}
}