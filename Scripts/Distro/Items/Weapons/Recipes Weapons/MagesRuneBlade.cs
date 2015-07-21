using System;
using Server;

namespace Server.Items
{
	public class MagesRuneBlade : RuneBlade
	{
		public override int LabelNumber { get { return 1073538; } } // Mage's Rune Blade
		[Constructable]
		public MagesRuneBlade()
		{
			Attributes.CastSpeed = 1;
		}


		public MagesRuneBlade( Serial serial )
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