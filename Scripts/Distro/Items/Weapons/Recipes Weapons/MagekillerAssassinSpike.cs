using System;
using Server;

namespace Server.Items
{
	public class MagekillerAssassinSpike : AssassinSpike
	{
		public override int LabelNumber { get { return 1073519; } } // Magekiller Assassin Spike

		[Constructable]
		public MagekillerAssassinSpike()
		{
			WeaponAttributes.HitLeechMana = 16;
		}


		public MagekillerAssassinSpike( Serial serial )
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