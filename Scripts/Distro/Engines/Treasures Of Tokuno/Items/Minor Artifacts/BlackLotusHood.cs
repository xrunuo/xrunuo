using System;
using Server;

namespace Server.Items
{
	public class BlackLotusHood : LeatherNinjaHood
	{
		public override int LabelNumber { get { return 1070919; } } // Black Lotus Hood

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BlackLotusHood()
		{
			ItemID = 0x278F;
			ArmorAttributes.SelfRepair = 5;
			Attributes.AttackChance = 6;
			Attributes.LowerManaCost = 6;

			Resistances.Physical = -2;
			Resistances.Fire = 8;
			Resistances.Cold = 12;
			Resistances.Poison = 8;
			Resistances.Energy = 7;
		}

		public BlackLotusHood( Serial serial )
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
				Resistances.Physical = -2;
				Resistances.Fire = 8;
				Resistances.Cold = 12;
				Resistances.Poison = 8;
				Resistances.Energy = 7;
			}
		}
	}
}