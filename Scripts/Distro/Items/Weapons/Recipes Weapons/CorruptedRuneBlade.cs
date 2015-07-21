using System;
using Server;

namespace Server.Items
{
	public class CorruptedRuneBlade : RuneBlade
	{
		public override int LabelNumber { get { return 1073540; } } // Corrupted Rune Blade
		[Constructable]
		public CorruptedRuneBlade()
		{
			Resistances.Physical = -5;
			Resistances.Poison = 12;
		}


		public CorruptedRuneBlade( Serial serial )
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