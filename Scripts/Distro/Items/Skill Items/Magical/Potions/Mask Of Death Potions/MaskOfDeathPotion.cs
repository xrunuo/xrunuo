using System;
using Server;

namespace Server.Items
{
	public class MaskOfDeathPotion : BaseMaskOfDeathPotion
	{
		public override TimeSpan Duration { get { return TimeSpan.FromSeconds( 45.0 ); } }

		public override int LabelNumber { get { return 1072100; } } // a Mask of Death potion

		[Constructable]
		public MaskOfDeathPotion()
			: base( PotionEffect.MaskOfDeath )
		{
		}

		public MaskOfDeathPotion( Serial serial )
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