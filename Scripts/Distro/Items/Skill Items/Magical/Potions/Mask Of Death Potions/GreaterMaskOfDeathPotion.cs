using System;
using Server;

namespace Server.Items
{
	public class GreaterMaskOfDeathPotion : BaseMaskOfDeathPotion
	{
		public override TimeSpan Duration { get { return TimeSpan.FromSeconds( 60.0 ); } }

		public override int LabelNumber { get { return 1072103; } } // a Greater Mask of Death potion

		[Constructable]
		public GreaterMaskOfDeathPotion()
			: base( PotionEffect.MaskOfDeathGreater )
		{
		}

		public GreaterMaskOfDeathPotion( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}