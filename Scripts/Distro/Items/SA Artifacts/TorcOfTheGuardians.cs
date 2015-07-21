using System;
using Server;

namespace Server.Items
{
	public class TorcOfTheGuardians : GoldNecklace
	{
		public override int LabelNumber { get { return 1113721; } } // Torc of the Guardians

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public TorcOfTheGuardians()
		{
			Hue = 1021;

			Attributes.BonusStr = 5;
			Attributes.BonusDex = 5;
			Attributes.BonusInt = 5;
			Attributes.RegenStam = 2;
			Attributes.RegenMana = 2;
			Attributes.LowerManaCost = 5;
			Resistances.Physical = 5;
			Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
			Resistances.Energy = 5;
		}

		public TorcOfTheGuardians( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
