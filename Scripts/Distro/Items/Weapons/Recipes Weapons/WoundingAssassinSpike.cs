using System;
using Server;

namespace Server.Items
{
	public class WoundingAssassinSpike : AssassinSpike
	{
		public override int LabelNumber { get { return 1073520; } } // Wounding Assassin Spike

		[Constructable]
		public WoundingAssassinSpike()
		{
			WeaponAttributes.HitHarm = 15;
		}


		public WoundingAssassinSpike( Serial serial )
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