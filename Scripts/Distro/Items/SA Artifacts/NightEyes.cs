using System;
using Server;

namespace Server.Items
{
	public class NightEyes : ElvenGlasses
	{
		public override int LabelNumber { get { return 1114785; } } // Night Eyes

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public NightEyes()
		{
			Hue = 1233;

			Attributes.NightSight = 1;
			Attributes.DefendChance = 10;
			Attributes.CastRecovery = 3;

			Resistances.Physical = 8;
			Resistances.Fire = 6;
			Resistances.Cold = 7;
			Resistances.Poison = 7;
			Resistances.Energy = 7;
		}

		public NightEyes( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 8;
				Resistances.Fire = 6;
				Resistances.Cold = 7;
				Resistances.Poison = 7;
				Resistances.Energy = 7;
			}
		}
	}
}
