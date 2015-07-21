using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public enum ImbuingResource
	{
		// Primary
		MagicalResidue = 1031697,
		EnchantedEssence = 1031698,
		RelicFragment = 1031699,

		// Secondary
		Emerald = 1023856,
		Diamond = 1023878,
		Amethyst = 1023862,
		Citrine = 1023861,
		Tourmaline = 1023864,
		Amber = 1023877,
		Ruby = 1023859,
		Sapphire = 1023857,
		StarSapphire = 1023855,

		// Full old
		LuminescentFungi = 1032689,
		FireRuby = 1032695,
		BlueDiamond = 1032696,
		Turquoise = 1032691,
		WhitePearl = 1032694,
		ParasiticPlant = 1032688,

		// Full new
		EssenceOfBalance = 1113324,
		EssenceOfAchievement,
		EssenceOfPassion,
		EssenceOfPrecision,
		EssenceOfDirection,
		SpiderCarapace,
		DaemonClaw,
		VialOfVitriol,
		FeyWings,
		VileTentacles,
		VoidCore,
		GoblinBlood,
		LavaSerpentCrust,
		UndyingFlesh,
		EssenceOfDiligence,
		EssenceOfFeeling,
		EssenceOfControl,
		EssenceOfSingularity,
		EssenceOfOrder,
		EssenceOfPersistence,
		CrystallineBlackrock,
		SeedOfRenewal,
		ElvenFletching,
		CrystalShards,
		Lodestone,
		DelicateScales,
		AbyssalCloth,
		CrushedGlass,
		ArcanicRuneStone,
		PoweredIron,
		VoidOrb,
		BouraPelt,
		ChagaMushroom,
		SilverSnakeSkin,
		FaeryDust,
		SlithTongue,
		RaptorTeeth,
		BottleOfIchor,
		ReflectiveWolfEye,
	}

	public class Resources
	{
		public static void Initialize()
		{
			foreach ( ImbuingResource resource in Enum.GetValues( typeof( ImbuingResource ) ) )
			{
				Type type = ScriptCompiler.FindTypeByFullName( String.Format( "Server.Items.{0}", resource.ToString() ) );

				if ( type != null )
					m_ResourceTable.Add( resource, type );
			}
		}

		public static Dictionary<ImbuingResource, Type> m_ResourceTable = new Dictionary<ImbuingResource, Type>();

		public static Type GetType( ImbuingResource resource )
		{
			if ( m_ResourceTable.ContainsKey( resource ) )
				return m_ResourceTable[resource];
			else
				return null;
		}

		/// <summary>
		/// Returns if the mobile has the needed ingredientes. If true, it actually consumes them.
		/// </summary>
		public static bool ConsumeResources( Mobile from, BaseAttrInfo prop, int first, int second, int third )
		{
			if ( from.Backpack == null )
				return false;

			// Var declaration
			Dictionary<Type, int> types = new Dictionary<Type, int>();
			List<Item> resources = new List<Item>();
			bool hasResources = false;

			hasResources = ( first == 0 || AddNeededResource( types, prop.PrimaryResource, first ) )
						&& ( second == 0 || AddNeededResource( types, prop.SecondaryResource, second ) )
						&& ( third == 0 || AddNeededResource( types, prop.FullResource, third ) );

			if ( hasResources )
			{
				// Have we all the resources needed?
				foreach ( KeyValuePair<Type, int> kvp in types )
				{
					hasResources = hasResources && from.Backpack.GetAmount( kvp.Key ) >= kvp.Value;

					if ( !hasResources )
						break;
				}

				if ( hasResources )
				{
					// Yeah! we do: actually consume them.
					foreach ( KeyValuePair<Type, int> kvp in types )
						from.Backpack.ConsumeUpTo( kvp.Key, kvp.Value );
				}
			}

			return hasResources;
		}

		private static bool AddNeededResource( Dictionary<Type, int> types, ImbuingResource resource, int amount )
		{
			Type type = GetType( resource );

			if ( type != null )
			{
				types.Add( type, amount );
				return true;
			}
			else
				return false;
		}
	}
}