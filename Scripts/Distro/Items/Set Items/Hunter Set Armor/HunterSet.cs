using System;
using System.Collections;

namespace Server.Items
{
	public class HunterSet
	{
		private static Hashtable m_Table = new Hashtable();

		public static bool FullSet( Mobile m )
		{
			return m.FindItemOnLayer( Layer.Gloves ) is HunterGloves
				&& m.FindItemOnLayer( Layer.Pants ) is HunterLeggings
				&& m.FindItemOnLayer( Layer.Arms ) is HunterSleeves
				&& m.FindItemOnLayer( Layer.InnerTorso ) is HunterTunic;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.Gloves ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Pants ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Arms ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor );

			// +10 dex (total)
			m.AddStatMod( new StatMod( StatType.Dex, "HunterSetDex", 10, TimeSpan.Zero ) );

			// +40 stealth (total)
			SkillMod skillmod = new DefaultSkillMod( SkillName.Stealth, true, 40.0 );
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

			if ( ( armor = m.FindItemOnLayer( Layer.Gloves ) as BaseArmor ) is HunterGloves )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Pants ) as BaseArmor ) is HunterLeggings )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Arms ) as BaseArmor ) is HunterSleeves )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor ) is HunterTunic )
				RemoveBonus( armor );

			m.RemoveStatMod( "HunterSetDex" );

			SkillMod skillmod = m_Table[m] as SkillMod;

			if ( skillmod != null )
			{
				m.RemoveSkillMod( skillmod );
				m_Table.Remove( m );
			}
		}

		public static void ApplyBonus( BaseArmor ba )
		{
			ba.Resistances.Physical = 5;
			ba.Resistances.Fire = 4;
			ba.Resistances.Cold = 3;
			ba.Resistances.Poison = 4;
			ba.Resistances.Energy = 4;
			ba.ArmorAttributes.SelfRepair = 3;
			ba.Hue = 1155;
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
			list.Add( 1072376, "4" ); // Part Of An Armor Set (6 Pieces)

			if ( item.Hue == 1155 )
			{
				list.Add( 1072377 ); // Full Armor Set Present
				list.Add( 1072502, "stealth\t40" ); // ~1_skill~ ~2_val~ (total)
				list.Add( 1072503, "10" ); // dexterity bonus ~1_val~ (total)
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 1155 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1072382, 5.ToString() ); // physical resist +~1_val~%
				list.Add( 1072383, 4.ToString() ); // fire resist +~1_val~%
				list.Add( 1072384, 3.ToString() ); // cold resist +~1_val~%
				list.Add( 1072385, 4.ToString() ); // poison resist +~1_val~%
				list.Add( 1072386, 4.ToString() ); // energy resist +~1_val~%
				list.Add( 1060450, 3.ToString() ); // self repair ~1_val~
				list.Add( 1072502, "stealth\t40" ); // ~1_skill~ ~2_val~ (total)
				list.Add( 1072503, "10" ); // dexterity bonus ~1_val~ (total)
			}
		}
	}
}