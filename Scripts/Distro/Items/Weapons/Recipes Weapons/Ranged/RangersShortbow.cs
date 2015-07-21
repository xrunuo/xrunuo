using System;
using Server;

namespace Server.Items
{
	public class RangersShortbow : MagicalShortbow
	{
		public override int LabelNumber { get { return 1073509; } } // Ranger's Shortbow


		[Constructable]
		public RangersShortbow()
		{
			Attributes.WeaponSpeed = 5;
		}


		public RangersShortbow( Serial serial )
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