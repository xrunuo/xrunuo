using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
	public class NecromancerSet
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

		private static Hashtable m_Table = new Hashtable();

		public static bool FullSet( Mobile m )
		{
			return m.FindItemOnLayer( Layer.Gloves ) is DeathsEssenceGloves
				&& m.FindItemOnLayer( Layer.Pants ) is DeathsEssenceLeggings
				&& m.FindItemOnLayer( Layer.Arms ) is DeathsEssenceSleeves
				&& m.FindItemOnLayer( Layer.Helm ) is DeathsEssenceHelm
				&& m.FindItemOnLayer( Layer.InnerTorso ) is DeathsEssenceTunic;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.Gloves ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Pants ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Arms ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Helm ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor );

			List<AttributeMod> mods = new List<AttributeMod>();

			mods.Add( new AttributeMod( MagicalAttribute.LowerManaCost, 10 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			// +10 necromancy (total)
			SkillMod skillmod = new DefaultSkillMod( SkillName.Necromancy, true, 10.0 );
			skillmod.ObeyCap = true;
			m_Table.Add( m, skillmod );
			m.AddSkillMod( skillmod );

			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
		}

		public static void RemoveBonus( Mobile m )
		{
			BaseArmor armor;

			if ( ( armor = m.FindItemOnLayer( Layer.Gloves ) as BaseArmor ) is DeathsEssenceGloves )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Pants ) as BaseArmor ) is DeathsEssenceLeggings )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Arms ) as BaseArmor ) is DeathsEssenceSleeves )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Helm ) as BaseArmor ) is DeathsEssenceHelm )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor ) is DeathsEssenceTunic )
				RemoveBonus( armor );

			if ( m_Bonus.ContainsKey( m ) )
			{
				List<AttributeMod> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			SkillMod skillmod = m_Table[m] as SkillMod;

			if ( skillmod != null )
			{
				m.RemoveSkillMod( skillmod );
				m_Table.Remove( m );
			}
		}

		public static void ApplyBonus( BaseArmor ba )
		{
			ba.Resistances.Physical = 4;
			ba.Resistances.Fire = 5;
			ba.Resistances.Cold = 3;
			ba.Resistances.Poison = 4;
			ba.Resistances.Energy = 4;
			ba.ArmorAttributes.SelfRepair = 3;
			ba.Hue = 1109;
		}

		public static void RemoveBonus( BaseArmor ba )
		{
			ba.Resistances.Physical = 0;
			ba.Resistances.Fire = 0;
			ba.Resistances.Cold = 0;
			ba.Resistances.Poison = 0;
			ba.Resistances.Energy = 0;
			ba.ArmorAttributes.SelfRepair = 0;
			ba.Hue = 0;
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1072376, "5" ); // Part Of An Armor Set (6 Pieces)

			if ( item.Hue == 1109 )
			{
				list.Add( 1072377 ); // Full Armor Set Present
				list.Add( 1072502, "necromancy\t10" ); // ~1_skill~ ~2_val~ (total)
				list.Add( 1073488, "10" ); // lower mana cost ~1_val~% (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 1109 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1072382, 4.ToString() ); // physical resist +~1_val~%
				list.Add( 1072383, 5.ToString() ); // fire resist +~1_val~%
				list.Add( 1072384, 3.ToString() ); // cold resist +~1_val~%
				list.Add( 1072385, 4.ToString() ); // poison resist +~1_val~%
				list.Add( 1072386, 4.ToString() ); // energy resist +~1_val~%
				list.Add( 1060450, 3.ToString() ); // self repair ~1_val~
				list.Add( 1072502, "necromancy\t10" ); // ~1_skill~ ~2_val~ (total)
				list.Add( 1073488, "10" ); // lower mana cost ~1_val~% (total)
			}
		}
	}
}