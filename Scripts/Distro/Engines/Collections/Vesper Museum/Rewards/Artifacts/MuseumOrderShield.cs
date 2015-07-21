using System;
using Server;

namespace Server.Items
{
	public class MuseumOrderShield : OrderShield, ICollectionItem
	{
		public override int LabelNumber { get { return 1073258; } } // Order Shield - Museum of Vesper Replica

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public MuseumOrderShield()
		{
			Hue = 1000;

			Attributes.SpellChanneling = 1;
			Attributes.DefendChance = 15;
			Attributes.AttackChance = 15;
			Attributes.Luck = 80;
		}

		public MuseumOrderShield( Serial serial )
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