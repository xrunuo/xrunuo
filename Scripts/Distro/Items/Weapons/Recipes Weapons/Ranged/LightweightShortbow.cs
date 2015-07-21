using System;
using Server;

namespace Server.Items
{
	public class LightweightShortbow : MagicalShortbow
	{
		public override int LabelNumber { get { return 1073510; } } // Lightweight Shortbow

		[Constructable]
		public LightweightShortbow()
		{
			WeaponAttributes.Balanced = 1;
		}


		public LightweightShortbow( Serial serial )
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