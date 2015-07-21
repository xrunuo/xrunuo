using System;
using Server;

namespace Server.Mobiles
{
	public class AbyssGuardian : Mobile
	{
		public AbyssGuardian()
		{
			Name = "Guardian";

			Body = 0x2F3;
			Hue = 0xB8F;

			Hits = HitsMax;

			Blessed = true;
			Frozen = true;
		}

		public AbyssGuardian( Serial serial )
			: base( serial )
		{
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
