using System;
using Server;

namespace Server.Items
{
	public class IronwoodCrown : RavenHelm
	{
		public override int LabelNumber { get { return 1072924; } } // Ironwood Crown

		[Constructable]
		public IronwoodCrown()
		{
			Hue = 0x1;

			ArmorAttributes.SelfRepair = 3;

			Attributes.BonusStr = 5;
			Attributes.BonusDex = 5;
			Attributes.BonusInt = 5;

			Resistances.Physical = 5;
			Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
			Resistances.Energy = 5;
		}

		public IronwoodCrown( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			if ( version < 1 )
			{
				Resistances.Physical = 5;
				Resistances.Fire = 5;
				Resistances.Cold = 5;
				Resistances.Poison = 5;
				Resistances.Energy = 5;
			}
		}
	}
}