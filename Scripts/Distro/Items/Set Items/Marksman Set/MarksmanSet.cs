using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class MarksmanSet
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

			return m.FindItemOnLayer( Layer.Cloak ) is Feathernock
				&& m.FindItemOnLayer( Layer.TwoHanded ) is Swiftflight;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.Cloak ) as Feathernock );
			ApplyBonus( m.FindItemOnLayer( Layer.TwoHanded ) as Swiftflight );

			List<AttributeMod> mods = new List<AttributeMod>();

			mods.Add( new AttributeMod( MagicalAttribute.WeaponSpeed, 30 ) );
			mods.Add( new AttributeMod( MagicalAttribute.AttackChance, 15 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			// +8 dex (total)
			m.AddStatMod( new StatMod( StatType.Dex, "MarksmanSetStr", 8, TimeSpan.Zero ) );

			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
		}

		public static void RemoveBonus( Mobile m )
		{
			Feathernock f;
			Swiftflight s;

			if ( ( f = m.FindItemOnLayer( Layer.Cloak ) as Feathernock ) != null )
				RemoveBonus( f );
			if ( ( s = m.FindItemOnLayer( Layer.TwoHanded ) as Swiftflight ) != null )
				RemoveBonus( s );

			if ( m_Bonus.ContainsKey( m ) )
			{
				List<AttributeMod> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			m.RemoveStatMod( "MarksmanSetStr" );
		}

		public static void ApplyBonus( Feathernock f )
		{
			f.Hue = 1428;
		}

		public static void ApplyBonus( Swiftflight s )
		{
			s.WeaponAttributes.SelfRepair = 3;
			s.Hue = 1428;
		}

		public static void RemoveBonus( Feathernock f )
		{
			f.Hue = 0;
		}

		public static void RemoveBonus( Swiftflight s )
		{
			s.WeaponAttributes.SelfRepair = 0;
			s.Hue = 0;
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1073491, "2" ); // Part of a Weapon/Armor Set (~1_val~ pieces)

			if ( item.Hue == 1428 )
			{
				list.Add( 1073492 ); // Full Weapon/Armor Set Present
				list.Add( 1073490, "15" ); // hit chance increase ~1_val~% (total)
				list.Add( 1072503, "8" ); // dexterity bonus ~1_val~ (total)
				list.Add( 1074323, "30" ); // swing speed increase ~1_val~% (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 1428 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1060450, 3.ToString() ); // self repair ~1_val~
				list.Add( 1073490, "15" ); // hit chance increase ~1_val~% (total)
				list.Add( 1072503, "8" ); // dexterity bonus ~1_val~ (total)
				list.Add( 1074323, "30" ); // swing speed increase ~1_val~% (total)
			}
		}
	}
}