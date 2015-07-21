using System;
using Server;

namespace Server.Items
{
	public class FrozenLongbow : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1073507; } } // Frozen Longbow

		[Constructable]
		public FrozenLongbow()
		{
			Attributes.WeaponSpeed = -5;
			Attributes.WeaponDamage = 10;
		}


		public FrozenLongbow( Serial serial )
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