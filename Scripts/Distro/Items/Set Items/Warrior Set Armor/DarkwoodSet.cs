using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class DarkwoodSet
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

		public static bool IsPart( BaseArmor ba )
		{
			return ba is DarkwoodGorget
				|| ba is DarkwoodCrown
				|| ba is DarkwoodLeggings
				|| ba is DarkwoodPauldrons
				|| ba is DarkwoodGauntlets
				|| ba is DarkwoodChest;
		}
		public static bool FullSet( Mobile m )
		{
			return m.FindItemOnLayer( Layer.Neck ) is DarkwoodGorget
				&& m.FindItemOnLayer( Layer.Helm ) is DarkwoodCrown
				&& m.FindItemOnLayer( Layer.Pants ) is DarkwoodLeggings
				&& m.FindItemOnLayer( Layer.Arms ) is DarkwoodPauldrons
				&& m.FindItemOnLayer( Layer.Gloves ) is DarkwoodGauntlets
				&& m.FindItemOnLayer( Layer.InnerTorso ) is DarkwoodChest;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.Neck ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Helm ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Pants ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Arms ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Gloves ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor );

			List<AttributeMod> mods = new List<AttributeMod>();

			mods.Add( new AttributeMod( AosAttribute.ReflectPhysical, 25 ) );
			mods.Add( new AttributeMod( AosAttribute.Luck, 100 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			m.AddStatMod( new StatMod( StatType.Str, "DarkwoodSetStr", 10, TimeSpan.Zero ) );

			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
		}

		public static void RemoveBonus( Mobile m )
		{
			BaseArmor armor;

			if ( ( armor = m.FindItemOnLayer( Layer.Neck ) as BaseArmor ) is DarkwoodGorget )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Helm ) as BaseArmor ) is DarkwoodCrown )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Pants ) as BaseArmor ) is DarkwoodLeggings )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Arms ) as BaseArmor ) is DarkwoodPauldrons )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Gloves ) as BaseArmor ) is DarkwoodGauntlets )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor ) is DarkwoodChest )
				RemoveBonus( armor );

			if ( m_Bonus.ContainsKey( m ) )
			{
				List<AttributeMod> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			m.RemoveStatMod( "DarkwoodSetStr" );
		}

		public static void ApplyBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 3;
			ba.Resistances.Physical = 2;
			ba.Resistances.Fire = 5;
			ba.Resistances.Cold = 5;
			ba.Resistances.Poison = 3;
			ba.Resistances.Energy = 5;
			ba.Hue = 1172;
		}

		public static void RemoveBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 0;
			ba.Resistances.Physical = 0;
			ba.Resistances.Fire = 0;
			ba.Resistances.Cold = 0;
			ba.Resistances.Poison = 0;
			ba.Resistances.Energy = 0;
			ba.Hue = 1109;
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1072376, "6" ); // Part Of An Armor Set (6 Pieces)

			if ( item.Hue == 1172 )
			{
				list.Add( 1072377 ); // Full Armor Set Present
				list.Add( 1072513, "25" ); // Reflect Physical Damage 25% (total)
				list.Add( 1072514, "10" ); // Strength Bonus 10 (total)
				list.Add( 1073489, "100" ); // Luck 100 (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 1172 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1072382, 2.ToString() ); // physical resist +~1_val~%
				list.Add( 1072383, 5.ToString() ); // fire resist +~1_val~%
				list.Add( 1072384, 5.ToString() ); // cold resist +~1_val~%
				list.Add( 1072385, 3.ToString() ); // poison resist +~1_val~%
				list.Add( 1072386, 5.ToString() ); // energy resist +~1_val~%
				list.Add( 1060450, 3.ToString() ); // self repair ~1_val~
				list.Add( 1072513, "25" ); // Reflect Physical Damage 25% (total)
				list.Add( 1072514, "10" ); // Strength Bonus 10 (total)
				list.Add( 1073489, "100" ); // Luck 100 (total)
			}
		}
	}
}
