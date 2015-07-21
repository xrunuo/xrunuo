using System;
using Server;

namespace Server.Items
{
	public class LegsOfStability : PlateSuneate
	{
		public override int LabelNumber { get { return 1070923; } } // Legs of Stability

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public LegsOfStability()
		{
			ArmorAttributes.SelfRepair = 3;
			Attributes.BonusStam = 5;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 15;
			Resistances.Poison = 15;
		}

		public LegsOfStability( Serial serial )
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
				Resistances.Physical = 15;
				Resistances.Poison = 15;
			}
		}
	}
}
