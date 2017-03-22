using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class PetBondingGate : Item
	{
		[Constructable]
		public PetBondingGate()
			: base( 0xF6C )
		{
			Light = LightType.Circle300;

			Hue = 0x314;

			Name = "Pet Bonding Gate";

			Movable = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			m.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Player )
			{
				return;
			}

			from.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );
		}

		public PetBondingGate( Serial serial )
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