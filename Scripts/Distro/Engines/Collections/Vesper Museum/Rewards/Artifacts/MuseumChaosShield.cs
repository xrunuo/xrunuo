using System;
using Server;

namespace Server.Items
{
	public class MuseumChaosShield : ChaosShield, ICollectionItem
	{
		public override int LabelNumber { get { return 1073259; } } // Chaos Shield - Museum of Vesper Replica

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public MuseumChaosShield()
		{
			Hue = 250;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 2;
			Attributes.CastRecovery = 2;
			ArmorAttributes.SelfRepair = 1;
		}

		public MuseumChaosShield( Serial serial )
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