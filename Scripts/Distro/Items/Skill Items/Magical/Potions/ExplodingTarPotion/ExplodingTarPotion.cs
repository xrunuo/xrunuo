using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace Server.Items
{
	public class ExplodingTarPotion : BaseExplodingTarPotion
	{
		public override int LabelNumber { get { return 1095147; } } // Exploding Tar Potion

		[Constructable]
		public ExplodingTarPotion()
			: base( PotionEffect.Explosion )
		{
		}

		public ExplodingTarPotion( Serial serial )
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
