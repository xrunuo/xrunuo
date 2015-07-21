using System;
using Server;

namespace Server.Items
{
	public class EpauletteBearingTheChestOfBlackthorn : BaseOuterTorso
	{
		[Constructable]
		public EpauletteBearingTheChestOfBlackthorn()
			: base( 0x9985 )
		{
			Weight = 3.0;
			Hue = 1194;
			Attributes.RegenMana = 2;
			Attributes.DefendChance = 5;
			Attributes.Luck = 140;
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1154548, "#1123325" ); // ~1_TYPE~ bearing the crest of Blackthorn;
		}

		public EpauletteBearingTheChestOfBlackthorn( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}