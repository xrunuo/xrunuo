using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class LesserMedusaScales : Item, IScales
	{
		public override int LabelNumber { get { return 1112626; } } // Medusa scales

		public GorgonQuality Quality { get { return GorgonQuality.Exceptional; } }

		[Constructable]
		public LesserMedusaScales()
			: this( 1 )
		{
		}

		[Constructable]
		public LesserMedusaScales( int amount )
			: base( 0x26B4 )
		{
			Stackable = true;
			Amount = amount;
			Weight = 1.0;
			Hue = 0x8B0;
		}

		public LesserMedusaScales( Serial serial )
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

	public class GreaterMedusaScales : Item, IScales
	{
		public override int LabelNumber { get { return 1112626; } } // Medusa scales

		public GorgonQuality Quality { get { return GorgonQuality.Invulnerable; } }

		[Constructable]
		public GreaterMedusaScales()
			: this( 1 )
		{
		}

		[Constructable]
		public GreaterMedusaScales( int amount )
			: base( 0x26B4 )
		{
			Stackable = true;
			Amount = amount;
			Weight = 1.0;
			Hue = 1266;
		}

		public GreaterMedusaScales( Serial serial )
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
