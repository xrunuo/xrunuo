using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class JuggernautSet
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
			return m.FindItemOnLayer( Layer.OneHanded ) is Evocaricus
				&& m.FindItemOnLayer( Layer.TwoHanded ) is MalekisHonor;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon );
			ApplyBonus( m.FindItemOnLayer( Layer.TwoHanded ) as BaseShield );

			List<AttributeMod> mods = new List<AttributeMod>();

			mods.Add( new AttributeMod( MagicalAttribute.WeaponSpeed, 35 ) );
			mods.Add( new AttributeMod( MagicalAttribute.DefendChance, 10 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			// +10 str (total)
			m.AddStatMod( new StatMod( StatType.Str, "JuggernautSetStr", 10, TimeSpan.Zero ) );

			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
		}

		public static void RemoveBonus( Mobile m )
		{
			BaseArmor armor;
			BaseWeapon weapon;

			if ( ( armor = m.FindItemOnLayer( Layer.TwoHanded ) as BaseArmor ) is MalekisHonor )
				RemoveBonus( armor );
			if ( ( weapon = m.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon ) is Evocaricus )
				RemoveBonus( weapon );

			if ( m_Bonus.ContainsKey( m ) )
			{
				List<AttributeMod> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			m.RemoveStatMod( "JuggernautSetStr" );
		}

		public static void ApplyBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 3;
			ba.Hue = 1901;
		}

		public static void ApplyBonus( BaseWeapon bw )
		{
			bw.WeaponAttributes.SelfRepair = 3;
			bw.Hue = 1901;
		}

		public static void RemoveBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 0;
			ba.Hue = 0;
		}

		public static void RemoveBonus( BaseWeapon bw )
		{
			bw.WeaponAttributes.SelfRepair = 0;
			bw.Hue = 0;
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1073491, "2" ); // Part of a Weapon/Armor Set (~1_val~ pieces)

			if ( item.Hue == 1901 )
			{
				list.Add( 1073492 ); // Full Weapon/Armor Set Present
				list.Add( 1073493, "10" ); // defense chance increase ~1_val~% (total)
				list.Add( 1072514, "10" ); // Strength Bonus 10 (total)
				list.Add( 1074323, "35" ); // swing speed increase ~1_val~% (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 1901 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1060450, 3.ToString() ); // self repair ~1_val~
				list.Add( 1073493, "10" ); // defense chance increase ~1_val~% (total)
				list.Add( 1072514, "10" ); // Strength Bonus 10 (total)
				list.Add( 1074323, "35" ); // swing speed increase ~1_val~% (total)
			}
		}
	}
}