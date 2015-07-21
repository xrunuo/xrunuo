using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class LuckJewelrySet
	{
		private static Dictionary<Mobile, List<AttributeMod>> m_Bonus = new Dictionary<Mobile, List<AttributeMod>>();

		private static void ApplyMods( Mobile m, List<AttributeMod> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
				m.AddAttributeMod( mods[i] );
		}

		private static void RemoveMods( Mobile m, List<AttributeMod> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
				m.RemoveAttributeMod( mods[i] );
		}

		public static bool FullSet( Mobile m )
		{
			if ( m == null )
				return false;

			return m.FindItemOnLayer( Layer.Bracelet ) is NovoBleue
				&& m.FindItemOnLayer( Layer.Ring ) is EtoileBleue;
		}

		public static void ApplyBonus( Mobile m )
		{
			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );

			List<AttributeMod> mods = new List<AttributeMod>();

			mods.Add( new AttributeMod( MagicalAttribute.Luck, 100 ) );
			mods.Add( new AttributeMod( MagicalAttribute.RegenMana, 2 ) );
			mods.Add( new AttributeMod( MagicalAttribute.RegenHits, 2 ) );
			mods.Add( new AttributeMod( MagicalAttribute.CastSpeed, 1 ) );
			mods.Add( new AttributeMod( MagicalAttribute.CastRecovery, 4 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			m.FindItemOnLayer( Layer.Bracelet ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Ring ).InvalidateProperties();
		}

		public static void RemoveBonus( Mobile m )
		{
			if ( m_Bonus.ContainsKey( m ) )
			{
				List<AttributeMod> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			Item item;

			if ( ( item = m.FindItemOnLayer( Layer.Ring ) ) is EtoileBleue )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Bracelet ) ) is NovoBleue )
				item.InvalidateProperties();
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1080240, "2" ); // Part of a Jewelry Set (~1_val~ pieces)

			Mobile parent = item.Parent as Mobile;

			if ( parent != null && m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1080241 ); // Full Jewelry Set Present
				list.Add( 1073489, "400" ); // Luck ~1_val~ (total)
				list.Add( 1080243, "3" ); // faster casting ~1_val~ (total)
				list.Add( 1080242, "6" ); // faster cast recovery ~1_val~ (total)
				list.Add( 1080244, "2" ); // hit point regeneration ~1_val~ (total)
				list.Add( 1080245, "2" ); // mana regeneration ~1_val~ (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			Mobile parent = item.Parent as Mobile;

			if ( parent == null || !m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1080246, "100" ); // luck +~1_val~
				list.Add( 1080247, "1" ); // faster casting +~1_val~
				list.Add( 1080248, "4" ); // faster cast recovery +~1_val~
				list.Add( 1080249, "2" ); // hit point regeneration +~1_val~
				list.Add( 1080250, "2" ); // mana regeneration +~1_val~
			}
		}
	}
}