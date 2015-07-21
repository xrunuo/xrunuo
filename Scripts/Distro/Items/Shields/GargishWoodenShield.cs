using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x4200, 0x4207 )]
	public class GargishWoodenShield : BaseShield
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int BasePhysicalResistance { get { return 0; } }
		public override int BaseFireResistance { get { return 0; } }
		public override int BaseColdResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 1; } }

		public override int InitMinHits { get { return 20; } }
		public override int InitMaxHits { get { return 25; } }

		public override int StrengthReq { get { return 20; } }

		[Constructable]
		public GargishWoodenShield()
			: base( 0x4200 )
		{
			Weight = 5.0;
		}

		public GargishWoodenShield( Serial serial )
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