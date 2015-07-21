using System;
using Server;

namespace Server.Items
{
	public class BoneMachete : ElvenMachete
	{
		public override int LabelNumber { get { return 1020526; } } // Bone Machete

		public override int InitMaxHits { get { return 6; } }
		public override int InitMinHits { get { return 6; } }

		[Constructable]
		public BoneMachete()
		{
			Resistances.Cold = 1;
			Resistances.Energy = 1;
			Resistances.Fire = 1;
			Resistances.Physical = 1;
			Resistances.Poison = 1;
		}


		public BoneMachete( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}