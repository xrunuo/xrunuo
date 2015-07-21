using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Factions;

namespace Server.Engines.Imbuing
{
	public class Unraveling
	{
		private static UnravelInfo[] m_UnravelTable = new UnravelInfo[]
			{
				new UnravelInfo( typeof( MagicalResidue ),		1, 1, 1031697,	0.0,	0 ),
				new UnravelInfo( typeof( MagicalResidue ),		2, 3, 1031697,	0.0,	50 ),
				new UnravelInfo( typeof( MagicalResidue ),		3, 4, 1031697,	0.0,	100 ),
				new UnravelInfo( typeof( MagicalResidue ),		4, 5, 1031697,	0.0,	150 ),
				new UnravelInfo( typeof( EnchantedEssence ),	1, 2, 1031698,	50.1,	200 ),
				new UnravelInfo( typeof( EnchantedEssence ),	2, 3, 1031698,	50.1,	300 ),
				new UnravelInfo( typeof( EnchantedEssence ),	3, 4, 1031698,	50.1,	400 ),
				new UnravelInfo( typeof( RelicFragment ),		1, 2, 1031699,	95.1,	451 )
			};

		public static UnravelInfo[] UnravelTable { get { return m_UnravelTable; } }

		private static double GetMaterialScalar( Item item )
		{
			CraftResource material = CraftResource.None;

			if ( item is BaseWeapon )
				material = ( (BaseWeapon) item ).Resource;
			else if ( item is BaseArmor )
				material = ( (BaseArmor) item ).Resource;
			else if ( item is BaseJewel )
				material = ( (BaseJewel) item ).Resource;

			double scalar = 1.0;

			CraftResourceInfo info = CraftResources.GetInfo( material );

			if ( info != null )
				scalar += info.AttributeInfo.ImbuingUnravelBonus;

			return scalar;
		}

		private static double GetDurabilityPenalty( IDurability item )
		{
			if ( !item.CanLoseDurability )
				return 0.0;

			return 0.02 * ( Math.Max( 0, 50 - item.HitPoints ) );
		}

		public static int GetUnravelWeight( Mobile from, Item item )
		{
			PropCollection props = new PropCollection( item );
			int intensity = props.Intensity;

			if ( props.Count == 0 )
				return 0;

			if ( props.Count > 5 )
				intensity = 5 * ( intensity / props.Count );

			intensity = (int) ( intensity * GetMaterialScalar( item ) );

			if ( from.Race == Race.Gargoyle )
				intensity += 20;

			if ( from.InRange( new Point2D( 797, 3432 ), 3 ) )
				intensity += 10;

			if ( from.Region.IsPartOf( "Queen's Palace" ) )
				intensity += 30;

			double correctionRate = 1.0;

			if ( item is IDurability )
				correctionRate -= GetDurabilityPenalty( (IDurability) item );

			if ( item is IImbuable && ( (IImbuable) item ).TimesImbued > 0 )
				correctionRate -= 0.2;

			intensity = (int) ( intensity * correctionRate );

			return intensity;
		}

		public static bool Unravel( Mobile from, Item toUnravel )
		{
			UnravelInfo info = GetUnravelInfo( from, toUnravel );

			if ( info == null )
			{
				// You cannot magically unravel this item. It appears to possess little or no magic.
				from.SendLocalizedMessage( 1080437 );

				return false;
			}
			else
			{
				double imbuing = from.Skills[SkillName.Imbuing].Value;

				if ( imbuing < info.MinSkill )
				{
					// Your Imbuing skill is not high enough to magically unravel this item.
					from.SendLocalizedMessage( 1080434 );

					return false;
				}
				else
				{
					bool plus = from.CheckSkill( SkillName.Imbuing, info.MinSkill, info.MinSkill + 70.0 );

					toUnravel.Delete();

					Item resource = Activator.CreateInstance( info.Resource ) as Item;
					resource.Amount = plus ? info.MaxResource : info.MinResource;

					from.AddToBackpack( resource );

					from.SendLocalizedMessage( 1080429 ); // You magically unravel the item!
					from.SendLocalizedMessage( 1072223 ); // An item has been placed in your backpack.

					Effects.SendPacket( from, from.Map, new GraphicalEffect( EffectType.FixedFrom, from.Serial, Serial.Zero, 0x375A, from.Location, from.Location, 1, 17, true, false ) );
					from.PlaySound( 0x1EB );

					return true;
				}
			}
		}

		public static bool TryUnravel( Mobile from, Item item, bool warnIfSpecial )
		{
			IImbuable unravel = item as IImbuable;

			if ( unravel == null )
			{
				from.SendLocalizedMessage( 1080425 ); // You cannot magically unravel this item.
				return false;
			}
			else if ( item is IFactionArtifact )
			{
				from.SendLocalizedMessage( 1112408 ); // You cannot magically unravel a faction reward item.
				return false;
			}
			else if ( item is ICollectionItem )
			{
				from.SendLocalizedMessage( 1080425 ); // You cannot magically unravel this item.
				return false;
			}
			else if ( item.LootType == LootType.Blessed )
			{
				from.SendLocalizedMessage( 1080421 ); // You cannot unravel the magic of a blessed item.
				return false;
			}
			else if ( warnIfSpecial && unravel.IsSpecialMaterial )
			{
				from.BeginAction( typeof( Imbuing ) );
				from.SendGump( new ConfirmUnravelGump( item ) );
				return false;
			}
			else
			{
				return Unravel( from, item );
			}
		}

		public static void UnravelContainer( Mobile from, Container c )
		{
			List<Item> unravel = new List<Item>( c.Items );
			int count = 0;

			foreach ( Item item in unravel )
			{
				if ( TryUnravel( from, item, false ) )
					count++;
			}

			// Unraveled: ~1_COUNT~/~2_NUM~ items
			from.SendLocalizedMessage( 1111814, String.Format( "{0}\t{1}", count, unravel.Count ) );
		}

		public static UnravelInfo GetUnravelInfo( Mobile from, Item item )
		{
			int intensity = GetUnravelWeight( from, item );

			if ( intensity > 0 )
			{
				for ( int i = m_UnravelTable.Length - 1; i >= 0; i-- )
				{
					UnravelInfo info = m_UnravelTable[i];

					if ( intensity >= info.MinIntensity )
						return info;
				}
			}

			return null;
		}
	}

	public class UnravelInfo
	{
		private Type m_Resource;
		private int m_MinResource, m_MaxResource;
		private int m_Name;
		private double m_MinSkill;
		private int m_MinIntensity;

		public Type Resource { get { return m_Resource; } }
		public int MinResource { get { return m_MinResource; } }
		public int MaxResource { get { return m_MaxResource; } }
		public int Name { get { return m_Name; } }
		public double MinSkill { get { return m_MinSkill; } }
		public int MinIntensity { get { return m_MinIntensity; } }

		public UnravelInfo( Type resource, int minResource, int maxResource, int name, double minSkill, int minIntensity )
		{
			m_Resource = resource;
			m_MinResource = minResource;
			m_MaxResource = maxResource;
			m_Name = name;
			m_MinSkill = minSkill;
			m_MinIntensity = minIntensity;
		}
	}
}