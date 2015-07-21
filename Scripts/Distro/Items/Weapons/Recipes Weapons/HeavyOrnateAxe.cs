using System;
using Server;

namespace Server.Items
{
	public class HeavyOrnateAxe : OrnateAxe
	{
		public override int LabelNumber { get { return 1073548; } } // Heavy Ornate Axe

		[Constructable]
		public HeavyOrnateAxe()
		{
			Attributes.WeaponDamage = 8;
		}


		public HeavyOrnateAxe( Serial serial )
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