using System;
using Server;

namespace Server.Items
{
	public class RighteousAnger : ElvenMachete
	{
		public override int LabelNumber { get { return 1075049; } } // Righteous Anger

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public RighteousAnger()
		{
			Hue = 644;
			Attributes.WeaponSpeed = 35;
			Attributes.WeaponDamage = 40;
			Attributes.AttackChance = 15;
			Attributes.DefendChance = 5;
		}

		public RighteousAnger( Serial serial )
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