using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Plants;

namespace Server.Items
{
	public class DryReeds : Item
	{
		private PlantHue m_PlantHue;

		[CommandProperty( AccessLevel.GameMaster )]
		public PlantHue PlantHue
		{
			get { return m_PlantHue; }
			set
			{
				m_PlantHue = value;
				Hue = PlantHueInfo.GetInfo( m_PlantHue ).Hue;
				InvalidateProperties();
			}
		}

		[Constructable]
		public DryReeds()
			: this( PlantHue.Plain )
		{
		}

		[Constructable]
		public DryReeds( PlantHue plantHue )
			: this( 1, plantHue )
		{
		}

		[Constructable]
		public DryReeds( int amount, PlantHue plantHue )
			: base( 0x1BD5 )
		{
			PlantHue = plantHue;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public DryReeds( Serial serial )
			: base( serial )
		{
		}

		public override void OnAfterDuped( Item newItem )
		{
			DryReeds newReeds = newItem as DryReeds;

			if ( newReeds != null )
				newReeds.PlantHue = PlantHue;
		}

		public static int GetTotalReeds( Container cont, PlantHue hue )
		{
			return GetTotalReeds( GetReeds( cont, hue ), hue );
		}

		public static int GetTotalReeds( List<DryReeds> reeds, PlantHue hue )
		{
			int total = 0;

			for ( int i = 0; i < reeds.Count; i++ )
				total += reeds[i].Amount;

			return total;
		}

		public static List<DryReeds> GetReeds( Container cont, PlantHue hue )
		{
			return GetReeds( cont.FindItemsByType<DryReeds>(), hue );
		}

		public static List<DryReeds> GetReeds( List<DryReeds> reeds, PlantHue hue )
		{
			List<DryReeds> validReeds = new List<DryReeds>();

			for ( int i = 0; i < reeds.Count; i++ )
			{
				DryReeds reed = reeds[i];

				if ( reed.PlantHue == hue )
					validReeds.Add( reed );
			}

			return validReeds;
		}

		public static bool ConsumeReeds( Container cont, PlantHue hue, int amount )
		{
			List<DryReeds> reeds = GetReeds( cont, hue );

			if ( GetTotalReeds( reeds, hue ) >= amount )
			{
				for ( int i = 0; i < reeds.Count; i++ )
				{
					DryReeds reed = reeds[i];

					if ( amount >= reed.Amount )
					{
						amount -= reed.Amount;
						reed.Delete();
					}
					else
					{
						reed.Amount -= amount;
						break;
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public override LocalizedText GetNameProperty()
		{
			PlantHueInfo info = PlantHueInfo.GetInfo( m_PlantHue );

			if ( Amount != 1 )
			{
				return new LocalizedText( info.IsBright()
						? 1113273 // ~1_AMOUNT~ bright ~2_COLOR~ reeds
						: 1113275 // ~1_AMOUNT~ ~2_COLOR~ reeds
					, String.Format( "{0}\t#{1}", Amount, info.Name ) );
			}
			else
			{
				return new LocalizedText( info.IsBright()
						? 1112288 // bright ~1_COLOR~ reeds
						: 1112289 // ~1_COLOR~ reeds
					, String.Format( "#{0}", info.Name ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_PlantHue );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_PlantHue = (PlantHue) reader.ReadInt();
		}
	}
}