using System;
using Server;

namespace Server.Items
{
	public class DragonNunchaku : Nunchaku
	{
		public override int LabelNumber { get { return 1070914; } } // Dragon Nunchaku

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DragonNunchaku()
		{
			WeaponAttributes.SelfRepair = 3;
			WeaponAttributes.HitFireball = 50;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 40;
			Resistances.Fire = 5;
		}

		public DragonNunchaku( Serial serial )
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
