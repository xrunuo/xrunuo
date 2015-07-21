using System;
using Server;

namespace Server.Items
{
	public class RubyMace : DiamondMace
	{
		public override int LabelNumber { get { return 1073529; } } // Ruby Mace

		[Constructable]
		public RubyMace()
		{
			Attributes.WeaponDamage = 5;
		}


		public RubyMace( Serial serial )
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