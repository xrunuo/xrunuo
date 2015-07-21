using System;
using Server;

namespace Server.Items
{
	public class MagekillerLeafblade : Leafblade
	{
		public override int LabelNumber { get { return 1073523; } } // Magekiller Leafblade

		[Constructable]
		public MagekillerLeafblade()
		{
			WeaponAttributes.HitLeechMana = 16;
		}


		public MagekillerLeafblade( Serial serial )
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