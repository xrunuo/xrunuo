using System;
using Server;

namespace Server.Items
{
	public class RuneBeetleCarapace : PlateDo
	{
		public override int LabelNumber { get { return 1070968; } } // Rune Beetle Carapace

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public RuneBeetleCarapace()
		{
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 3;
			Attributes.LowerManaCost = 15;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;

			Resistances.Cold = 12;
			Resistances.Energy = 12;
		}

		public RuneBeetleCarapace( Serial serial )
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

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Cold = 12;
				Resistances.Energy = 12;
			}
		}
	}
}
