using System;
using Server;
using Server.Engines.Plants;
using Server.Engines.Craft;
using Server.Targeting;

namespace Server.Items
{
	public class PlantClippings : Item
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
		public PlantClippings()
			: this( PlantHue.Plain )
		{
		}

		[Constructable]
		public PlantClippings( PlantHue plantHue )
			: this( 1, plantHue )
		{
		}

		[Constructable]
		public PlantClippings( int amount, PlantHue plantHue )
			: base( 0x4022 )
		{
			PlantHue = plantHue;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public PlantClippings( Serial serial )
			: base( serial )
		{
		}

		public override void OnAfterDuped( Item newItem )
		{
			PlantClippings newClippings = newItem as PlantClippings;

			if ( newClippings != null )
				newClippings.PlantHue = PlantHue;
		}

		public override LocalizedText GetNameProperty()
		{
			PlantHueInfo info = PlantHueInfo.GetInfo( m_PlantHue );

			if ( Amount != 1 )
			{
				return new LocalizedText( info.IsBright()
						? 1113272 // ~1_AMOUNT~ bright ~2_COLOR~ plant clippings
						: 1113274 // ~1_AMOUNT~ ~2_COLOR~ plant clippings
					, String.Format( "{0}\t#{1}", Amount, info.Name ) );
			}
			else
			{
				return new LocalizedText( info.IsBright()
						? 1112121 // bright ~1_COLOR~ plant clippings
						: 1112122 // ~1_COLOR~ plant clippings
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