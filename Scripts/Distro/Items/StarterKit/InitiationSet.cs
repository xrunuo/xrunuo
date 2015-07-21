using System;
using System.Collections.Generic;
using Server.Accounting;

namespace Server.Items
{
	public class InitiationSet
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
			Item gorget = m.FindItemOnLayer( Layer.Neck );
			Item head = m.FindItemOnLayer( Layer.Helm );
			if ( m.Race == Race.Gargoyle )
				head = m.FindItemOnLayer( Layer.Earrings );

			return ( chest is InitiationTunic || chest is GargishInitiationChest )
				&& ( pants is InitiationLeggings || pants is GargishInitiationLeggings )
				&& ( arms is InitiationSleeves || arms is GargishInitiationArms )
				&& ( gloves is InitiationGloves || gloves is GargishInitiationKilt )
				&& ( head is InitiationCap || head is GargishInitiationEarrings )
				&& ( gorget is InitiationGorget || gorget is GargishInitiationNecklace );
		}

		public static void ApplyBonus( Mobile m )
		{
			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );

			List<object> mods = new List<object>();

			mods.Add( new ResistanceMod( ResistanceType.Physical, 12 ) );
			mods.Add( new ResistanceMod( ResistanceType.Fire, 30 ) );
			mods.Add( new ResistanceMod( ResistanceType.Cold, 30 ) );
			mods.Add( new ResistanceMod( ResistanceType.Poison, 18 ) );
			mods.Add( new ResistanceMod( ResistanceType.Energy, 30 ) );

			ApplyMods( m, mods );

			m_Bonus[m] = mods;

			m.FindItemOnLayer( Layer.InnerTorso ).Hue = 48;
			m.FindItemOnLayer( Layer.Pants ).Hue = 48;
			m.FindItemOnLayer( Layer.Arms ).Hue = 48;
			m.FindItemOnLayer( Layer.Gloves ).Hue = 48;
			m.FindItemOnLayer( Layer.Neck ).Hue = 48;

			m.FindItemOnLayer( Layer.InnerTorso ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Pants ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Arms ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Gloves ).InvalidateProperties();
			m.FindItemOnLayer( Layer.Neck ).InvalidateProperties();


			if ( m.Race == Race.Gargoyle )
			{
				m.FindItemOnLayer( Layer.Earrings ).Hue = 48;
				m.FindItemOnLayer( Layer.Earrings ).InvalidateProperties();
			}
			else
			{
				m.FindItemOnLayer( Layer.Helm ).Hue = 48;
				m.FindItemOnLayer( Layer.Helm ).InvalidateProperties();
			}

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
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Pants ) ) != null )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Arms ) ) != null )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Gloves ) ) != null )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Helm ) ) != null && m.Race != Race.Gargoyle )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Neck ) ) != null )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
			if ( ( item = m.FindItemOnLayer( Layer.Earrings ) ) != null && m.Race == Race.Gargoyle )
			{
				item.InvalidateProperties();
				item.Hue = 2101;
			}
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1072376, "6" ); // Part Of An Armor Set (6 Pieces)

			Mobile parent = item.Parent as Mobile;

			if ( parent != null && m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1072377 ); // Full Armor Set Present

				list.Add( 1080361, "54" ); // physical resist ~1_val~% (total)
				list.Add( 1080362, "54" ); // fire resist ~1_val~% (total)
				list.Add( 1080363, "54" ); // cold resist ~1_val~% (total)
				list.Add( 1080364, "54" ); // poison resist ~1_val~% (total)
				list.Add( 1080365, "54" ); // energy resist ~1_val~% (total)				
			}
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			Mobile parent = item.Parent as Mobile;

			if ( parent == null || !m_Bonus.ContainsKey( parent ) )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:

				list.Add( 1072382, "2" ); // physical resist +~1_val~%
				list.Add( 1072383, "5" ); // fire resist +~1_val~%
				list.Add( 1072384, "5" ); // cold resist +~1_val~%
				list.Add( 1072385, "3" ); // poison resist +~1_val~%
				list.Add( 1072386, "5" ); // energy resist +~1_val~%	
			}
		}
	}
}