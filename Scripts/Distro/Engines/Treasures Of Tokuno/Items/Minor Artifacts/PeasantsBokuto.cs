using System;
using Server;

namespace Server.Items
{
	public class PeasantsBokuto : Bokuto
	{
		public override int LabelNumber { get { return 1070912; } } // Peasant's Bokuto

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public PeasantsBokuto()
		{
			WeaponAttributes.HitLowerDefend = 30;
			Slayer = SlayerName.Snake;
			WeaponAttributes.SelfRepair = 3;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 35;
		}

		public PeasantsBokuto( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}
