using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class KnightsSet
	{
		private static Dictionary<Mobile, List<object>> m_Bonus = new Dictionary<Mobile, List<object>>();

		private static void ApplyMods( Mobile m, List<object> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
			{
				object mod = mods[i];

				if ( mod is AttributeMod )
					m.AddAttributeMod( (AttributeMod) mod );
				else if ( mod is ResistanceMod )
					m.AddResistanceMod( (ResistanceMod) mod );
			}
		}

		private static void RemoveMods( Mobile m, List<object> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
			{
				object mod = mods[i];

				if ( mod is AttributeMod )
					m.RemoveAttributeMod( (AttributeMod) mod );
				else if ( mod is ResistanceMod )
					m.RemoveResistanceMod( (ResistanceMod) mod );
			}
		}

		public static bool FullSet( Mobile m )
		{
			if ( m == null )
				return false;

			Item chest = m.FindItemOnLayer( Layer.InnerTorso );
			Item pants = m.FindItemOnLayer( Layer.Pants );
			Item arms = m.FindItemOnLayer( Layer.Arms );
			Item gloves = m.FindItemOnLayer( Layer.Gloves );
			Item head = m.FindItemOnLayer( Layer.Helm );
			Item gorget = m.FindItemOnLayer( Layer.Neck );

			return ( chest is KnightsFemaleArmor || chest is KnightsBreastplate )
				&& ( pants is KnightsLegs )
				&& ( arms is KnightsArms )
				&& ( gloves is KnightsGloves )
				&& ( head is KnightsPlateHelm || head is KnightsNorseHelm || head is KnightsCloseHelm || head is KnightsBascinet )
				&& ( gorget is KnightsGorget );
		}

		public static void ApplyBonus( Mobile m )
		{
			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );

			List<object> mods = new List<object>();

			mods.Add( new AttributeMod( MagicalAttribute.BonusHits, 6 ) );
			mods.Add( new AttributeMod( MagicalAttribute.RegenHits, 2 ) );
			mods.Add( new AttributeMod( MagicalAttribute.RegenMana, 2 ) );
			mods.Add( new AttributeMod( MagicalAttribute.AttackChance, 10 ) );
			mods.Add( new AttributeMod( MagicalAttribute.DefendChance, 10 ) );
			mods.Add( new ResistanceMod( ResistanceType.Physical, 28 ) );
			mods.Add( new ResistanceMod( ResistanceType.Fire, 28 ) );
			mods.Add( new ResistanceMod( ResistanceType.Cold, 28 ) );
			mods.Add( new ResistanceMod( ResistanceType.Poison, 28 ) );
			mods.Add( new ResistanceMod( ResistanceType.Energy, 28 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			m.FindItemOnLayer( Layer.InnerTorso ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Pants ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Arms ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Gloves ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Helm ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Neck ).InvalidateProperties();
		}

		public static void RemoveBonus( Mobile m )
		{
			if ( m_Bonus.ContainsKey( m ) )
			{
				List<object> mods = m_Bonus[m];

				RemoveMods( m, mods );
				m_Bonus.Remove( m );
			}

			Item item;

			if ( ( item = m.FindItemOnLayer( Layer.InnerTorso ) ) != null )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Pants ) ) != null )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Arms ) ) != null )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Gloves ) ) != null )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Helm ) ) != null )
				item.InvalidateProperties();
			if ( ( item = m.FindItemOnLayer( Layer.Neck ) ) != null )
				item.InvalidateProperties();
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1072376, "6" ); // Part Of An Armor Set (6 Pieces)

			Mobile parent = item.Parent as Mobile;

			if ( parent != null && m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1072377 ); // Full Armor Set Present
				list.Add( 1080360, "12" ); // hit point increase ~1_val~ (total)
				list.Add( 1080244, "2" ); // hit point regeneration ~1_val~ (total)
				list.Add( 1080245, "2" ); // mana regeneration ~1_val~ (total)
				list.Add( 1073490, "10" ); // hit chance increase ~1_val~% (total)
				list.Add( 1073493, "10" ); // defense chance increase ~1_val~% (total)
				list.Add( 1080361, "70" ); // physical resist ~1_val~% (total)
				list.Add( 1080362, "70" ); // fire resist ~1_val~% (total)
				list.Add( 1080363, "70" ); // cold resist ~1_val~% (total)
				list.Add( 1080364, "70" ); // poison resist ~1_val~% (total)
				list.Add( 1080365, "70" ); // energy resist ~1_val~% (total)				
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			Mobile parent = item.Parent as Mobile;

			if ( parent == null || !m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1080358, "6" ); // hit point increase +~1_val~ 
				list.Add( 1060444, "2" ); // hit point regeneration ~1_val~
				list.Add( 1060440, "2" ); // mana regeneration ~1_val~
				list.Add( 1060415, "10" ); // hit chance increase ~1_val~%
				list.Add( 1060408, "10" ); // defense chance increase ~1_val~%
				list.Add( 1072382, "28" ); // physical resist +~1_val~%
				list.Add( 1072383, "28" ); // fire resist +~1_val~%
				list.Add( 1072384, "28" ); // cold resist +~1_val~%
				list.Add( 1072385, "28" ); // poison resist +~1_val~%
				list.Add( 1072386, "28" ); // energy resist +~1_val~%	
			}
		}
	}
}