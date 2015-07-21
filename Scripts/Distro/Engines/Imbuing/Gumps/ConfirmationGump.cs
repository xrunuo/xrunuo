using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Imbuing
{
	public class ConfirmationGump : Gump
	{
		private const int Green = 0x41;
		private const int Yellow = 0x36;
		private const int DarkYellow = 0x2E;
		private const int Red = 0x26;

		// The item we are going to imbue with m_Property.
		private Item m_ItemToImbue;
		private IImbuable m_Imbuable;

		// The properties this item possess and their intensities.
		private PropCollection m_ItemProperties;

		// The property we want to imbue in m_Item.
		private BaseAttrInfo m_PropToImbue;
		private int m_ValueToImbue;

		// The property that m_PropToImbue replaces if we imbue it.
		private BaseAttrInfo m_PropToReplace;
		private int m_ValueToReplace;

		// Info about the intensity level we are imbuing.
		private int m_IntensityStep;
		private int m_TotalIntensitySteps;
		private int m_WTotalIntensity;

		// The amount of resources we need to imbue the property.
		private int m_Resources1, m_Resources2, m_Resources3;

		// Success Chance
		private double m_SuccessChance;

		public ConfirmationGump( Mobile from, Item itemToImbue, PropCollection itemProperties, BaseAttrInfo propToImbue, BaseAttrInfo propToReplace, int intensityStep, int totalSteps )
			: base( 50, 50 )
		{
			// Var declarations
			int intensity;			// intensity of the prop we are imbuing
			int wIntensity;			// weighted intensity of the prop we are imbuing
			int totalIntensity;		// total intensity of all props, weighted
			int wTotalIntensity;	// total intensity of all props, weighted
			int totalProperties;	// total number of properties the item will possess when imbued
			string newValueString;	// the new value of the prop we are imbuing

			// Save the parameters.
			m_ItemToImbue = itemToImbue;
			m_Imbuable = m_ItemToImbue as IImbuable;
			m_ItemProperties = itemProperties;
			m_PropToImbue = propToImbue;
			m_PropToReplace = propToReplace;
			m_IntensityStep = intensityStep;
			m_TotalIntensitySteps = totalSteps;

			// Make sure we don't perverse the intensities.
			Utility.FixMinMax( ref m_IntensityStep, 1, m_TotalIntensitySteps );

			// Gets info about the prop that we are going to imbue.
			intensity = (int) ( 100 * ( (double) m_IntensityStep / m_TotalIntensitySteps ) );
			wIntensity = (int) ( intensity * m_PropToImbue.Weight );
			totalIntensity = m_ItemProperties.Intensity + intensity;
			wTotalIntensity = m_ItemProperties.WeightedIntensity + wIntensity;
			totalProperties = m_ItemProperties.Count;

			m_ValueToImbue = ComputeValue( m_IntensityStep, m_TotalIntensitySteps, m_PropToImbue.MinIntensity, m_PropToImbue.MaxIntensity );
			newValueString = FormatValue( m_PropToImbue.Modify( m_ItemToImbue, m_ValueToImbue ), m_PropToImbue );

			// **********************
			// Gump Structure
			// **********************
			AddPage( 0 );

			AddBackground( 0, 0, 520, 440, 0x13BE );
			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 245, 140, 0xA40 );
			AddImageTiled( 265, 40, 245, 140, 0xA40 );
			AddImageTiled( 10, 190, 245, 140, 0xA40 );
			AddImageTiled( 265, 190, 245, 140, 0xA40 );
			AddImageTiled( 10, 340, 500, 60, 0xA40 );
			AddImageTiled( 10, 410, 500, 20, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 420 );

			AddHtmlLocalized( 10, 12, 500, 20, 1079717, 0x7FFF, false, false ); // <CENTER>IMBUING CONFIRMATION</CENTER>

			// **********************
			// Property Info
			// **********************

			AddHtmlLocalized( 50, 50, 250, 20, 1114269, 0x7FFF, false, false ); // PROPERTY INFORMATION

			AddHtmlLocalized( 25, 80, 390, 20, 1114270, 0x7FFF, false, false ); // Property:
			AddHtmlLocalized( 95, 80, 150, 20, m_PropToImbue.Name, 0x7FFF, false, false );

			AddHtmlLocalized( 25, 100, 390, 20, 1114271, 0x7FFF, false, false ); // Replaces:
			if ( m_PropToReplace != null )
			{
				m_ValueToReplace = m_PropToReplace.GetValue( m_ItemToImbue );

				AddHtmlLocalized( 95, 100, 150, 20, m_PropToReplace.Name, 0x7FFF, false, false );

				// The intensity of the property replaced may not count in the total.
				int replaceIntensity = Imbuing.ComputeIntensity( m_ItemToImbue, m_PropToReplace );
				totalIntensity -= replaceIntensity;
				wTotalIntensity -= (int) ( m_PropToReplace.Weight * replaceIntensity );
			}
			else
			{
				// Since we are not replacing any prop, it means we are adding one ^^u
				totalProperties++;
			}

			m_WTotalIntensity = wTotalIntensity;

			AddHtmlLocalized( 25, 120, 200, 20, 1114272, 0x7FFF, false, false ); // Weight:
			AddLabel( 95, 120, 0x481, String.Format( "{0:0.0}x", m_PropToImbue.Weight ) );

			AddHtmlLocalized( 25, 140, 200, 20, 1114273, 0x7FFF, false, false ); // Intensity:
			AddLabel( 95, 140, 0x481, String.Format( "{0}%", wIntensity ) );

			AddHtmlLocalized( 280, 55, 205, 115, m_PropToImbue.Description, 0x7FFF, false, false );

			// **********************
			// Material Info
			// **********************
			AddHtmlLocalized( 100, 200, 80, 20, 1044055, 0x7FFF, false, false ); // <CENTER>MATERIALS</CENTER>

			m_Resources1 = Math.Max( 1, (int) ( intensity / 20 ) );
			m_Resources2 = Math.Max( 1, (int) ( intensity / 10 ) );
			m_Resources3 = Math.Max( 0, ( intensity - 90 ) );

			AddHtmlLocalized( 40, 220, 390, 20, (int) m_PropToImbue.PrimaryResource, 0x7FFF, false, false );
			AddLabel( 210, 220, 0x481, m_Resources1.ToString() );

			AddHtmlLocalized( 40, 240, 390, 20, (int) m_PropToImbue.SecondaryResource, 0x7FFF, false, false );
			AddLabel( 210, 240, 0x481, m_Resources2.ToString() );

			if ( m_Resources3 > 0 )
			{
				AddHtmlLocalized( 40, 260, 390, 20, (int) m_PropToImbue.FullResource, 0x7FFF, false, false );
				AddLabel( 210, 260, 0x481, m_Resources3.ToString() );
			}

			// **********************
			// Results
			// **********************
			AddHtmlLocalized( 350, 200, 65, 20, 1113650, 0x7FFF, false, false ); // RESULTS

			AddHtmlLocalized( 280, 220, 140, 20, 1113645, 0x7FFF, false, false ); // Properties:
			AddFractionLabel( 430, 220, totalProperties, 5 );

			AddHtmlLocalized( 280, 240, 260, 20, 1113646, 0x7FFF, false, false ); // Total Property Weight:
			AddFractionLabel( 430, 240, wTotalIntensity, m_Imbuable.MaxIntensity );

			AddHtmlLocalized( 280, 260, 200, 20, 1113647, 0x7FFF, false, false ); // Times Imbued:
			AddFractionLabel( 430, 260, m_Imbuable.TimesImbued, 20 );

			// **********************
			// Choose Intensity
			// **********************
			if ( m_PropToImbue.MinIntensity != m_PropToImbue.MaxIntensity )
			{
				AddHtmlLocalized( 235, 350, 200, 20, 1062300, 0x7FFF, false, false ); // New Value:
				AddLabel( 256, 370, 0x481, newValueString );

				AddButton( 179, 372, 0x1464, 0x1464, 310, GumpButtonType.Reply, 0 );
				AddButton( 187, 372, 0x1466, 0x1466, 310, GumpButtonType.Reply, 0 );
				AddLabel( 181, 370, 0x0, "<" );
				AddLabel( 185, 370, 0x0, "<" );
				AddLabel( 189, 370, 0x0, "<" );
				AddButton( 199, 372, 0x1464, 0x1464, 314, GumpButtonType.Reply, 0 );
				AddButton( 207, 372, 0x1466, 0x1466, 314, GumpButtonType.Reply, 0 );
				AddLabel( 202, 370, 0x0, "<" );
				AddLabel( 207, 370, 0x0, "<" );
				AddButton( 221, 372, 0x1464, 0x1464, 311, GumpButtonType.Reply, 0 );
				AddButton( 229, 372, 0x1466, 0x1466, 311, GumpButtonType.Reply, 0 );
				AddLabel( 224, 370, 0x0, "<" );
				AddButton( 280, 372, 0x1464, 0x1464, 312, GumpButtonType.Reply, 0 );
				AddButton( 288, 372, 0x1466, 0x1466, 312, GumpButtonType.Reply, 0 );
				AddLabel( 286, 370, 0x0, ">" );
				AddButton( 300, 372, 0x1464, 0x1464, 315, GumpButtonType.Reply, 0 );
				AddButton( 308, 372, 0x1466, 0x1466, 315, GumpButtonType.Reply, 0 );
				AddLabel( 304, 370, 0x0, ">" );
				AddLabel( 308, 370, 0x0, ">" );
				AddButton( 320, 372, 0x1464, 0x1464, 313, GumpButtonType.Reply, 0 );
				AddButton( 328, 372, 0x1466, 0x1466, 313, GumpButtonType.Reply, 0 );
				AddLabel( 322, 370, 0x0, ">" );
				AddLabel( 326, 370, 0x0, ">" );
				AddLabel( 330, 370, 0x0, ">" );
			}

			// **********************
			// Self Repair Warning
			// **********************

			bool selfRepair = ( m_ItemToImbue is IArmor && ( (IArmor) m_ItemToImbue ).ArmorAttributes.SelfRepair != 0 )
						   || ( m_ItemToImbue is ICloth && ( (ICloth) m_ItemToImbue ).ClothingAttributes.SelfRepair != 0 )
						   || ( m_ItemToImbue is IWeapon && ( (IWeapon) m_ItemToImbue ).WeaponAttributes.SelfRepair != 0 );

			if ( selfRepair )
				AddHtmlLocalized( 20, 330, 390, 20, 1080417, 0x7FFF, false, false ); // WARNING! Imbuing will remove Self Repair from this item.

			// **********************
			// Success Chance
			// **********************
			m_SuccessChance = Imbuing.GetSuccessChance( from, m_ItemToImbue, totalIntensity );

			AddHtmlLocalized( 300, 300, 250, 20, 1044057, 0x7FFF, false, false ); // Success Chance:
			AddLabel( 420, 300, GetSuccessChanceHue(), String.Format( "{0:0.0}%", Math.Max( 0, ( m_SuccessChance * 100 ) ) ) );

			// **********************
			// Are we ready to rock?
			// **********************
			AddButton( 15, 410, 0xFA5, 0xFA6, 301, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 412, 100, 20, 1114268, 0x7FFF, false, false ); // Back

			AddButton( 390, 410, 0xFA5, 0xFA6, 302, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 425, 412, 100, 20, 1114267, 0x7FFF, false, false ); // Imbue Item
		}

		private int GetSuccessChanceHue()
		{
			if ( m_SuccessChance >= 0.8 )
				return Green;
			else if ( m_SuccessChance >= 0.5 )
				return Yellow;
			else if ( m_SuccessChance >= 0.1 )
				return DarkYellow;
			else
				return Red;
		}

		private static string FormatValue( int value, BaseAttrInfo prop )
		{
			string format;

			switch ( prop.Display )
			{
				default:
				case DisplayValue.Value: format = "{0}"; break;
				case DisplayValue.PlusValue: format = "+{0}"; break;
				case DisplayValue.MinusValue: format = "-{0}"; break;
				case DisplayValue.ValuePercentage: format = "{0}%"; break;
			}

			return string.Format( format, value.ToString() );
		}

		private static int ComputeValue( int steps, int maxSteps, double minIntensity, double maxIntensity )
		{
			if ( maxSteps == 1 )
				return 1;

			double intensity = (double) ( steps - 1 ) / ( maxSteps - 1 );

			return (int) ( minIntensity + intensity * ( maxIntensity - minIntensity ) );
		}

		private static int GetColor( int value, int toCompare )
		{
			if ( value < toCompare )
				return Green;
			else if ( value == toCompare )
				return Yellow;
			else // value > toCompare
				return Red;
		}

		private void AddFractionLabel( int x, int y, int value, int max )
		{
			AddLabel( x, y, GetColor( value, max ), String.Format( "{0}/{1}", value, max ) );
		}

		public void ChangeIntensity( Mobile from, int newIntensity )
		{
			m_IntensityStep = newIntensity;

			Utility.FixMinMax( ref m_IntensityStep, 1, m_TotalIntensitySteps );

			int intensity = (int) ( 100 * ( (double) m_IntensityStep / m_TotalIntensitySteps ) );

			m_ValueToImbue = ComputeValue( m_IntensityStep, m_TotalIntensitySteps, m_PropToImbue.MinIntensity, m_PropToImbue.MaxIntensity );

			m_Resources1 = Math.Max( 1, (int) ( intensity / 20 ) );
			m_Resources2 = Math.Max( 1, (int) ( intensity / 10 ) );
			m_Resources3 = Math.Max( 0, ( intensity - 90 ) );

			int totalIntensity = m_ItemProperties.Intensity + intensity;

			if ( m_PropToReplace != null )
			{
				int replaceIntensity = Imbuing.ComputeIntensity( m_ItemToImbue, m_PropToReplace );
				totalIntensity -= replaceIntensity;
			}

			m_SuccessChance = Imbuing.GetSuccessChance( from, m_ItemToImbue, totalIntensity );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			switch ( info.ButtonID )
			{
				default: // Close
					{
						from.EndAction( typeof( Imbuing ) );
						break;
					}
				case 301: // No, let me make another selection.
					{
						from.SendGump( new SelectPropGump( m_ItemToImbue ) );
						break;
					}
				case 302: // Yes, imbue this item.
					{
						Imbuing.Do( from, m_ItemToImbue, m_PropToImbue, m_PropToReplace, m_ValueToImbue, m_WTotalIntensity, m_Resources1, m_Resources2, m_Resources3, m_SuccessChance );
						ImbuingContext.AddContext( from, m_ItemToImbue, m_PropToImbue, m_IntensityStep );
						break;
					}
				case 310: // Min Intensity
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, 1, m_TotalIntensitySteps ) );
						break;
					}
				case 311: // Low Intensity 1
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, m_IntensityStep - 1, m_TotalIntensitySteps ) );
						break;
					}
				case 312: // Raise Intensity 1
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, m_IntensityStep + 1, m_TotalIntensitySteps ) );
						break;
					}
				case 313: // Max Intensity
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, m_TotalIntensitySteps, m_TotalIntensitySteps ) );
						break;
					}
				case 314: // Low Intensity 10
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, m_IntensityStep - 10, m_TotalIntensitySteps ) );
						break;
					}
				case 315: // Raise Intensity 10
					{
						from.SendGump( new ConfirmationGump( from, m_ItemToImbue, m_ItemProperties, m_PropToImbue, m_PropToReplace, m_IntensityStep + 10, m_TotalIntensitySteps ) );
						break;
					}
			}
		}
	}
}