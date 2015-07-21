using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class OrcishBow : Bow
	{
		public override int StrengthReq { get { return 80; } }
		public override int DexterityReq { get { return 80; } }

		[Constructable]
		public OrcishBow()
		{
			Hue = 0x497;

			Attributes.WeaponDamage = 25;

			WeaponAttributes.DurabilityBonus = 70;

			Name = "an orcish bow";
		}

		public OrcishBow( Serial serial )
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