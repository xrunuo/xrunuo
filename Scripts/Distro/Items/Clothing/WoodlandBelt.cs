using System;

namespace Server.Items
{
	public class WoodlandBelt : BaseWaist
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public WoodlandBelt( int hue )
			: base( 0x315F )
		{
			Hue = hue;
			Weight = 2.0;
		}

		[Constructable]
		public WoodlandBelt()
			: this( 0 )
		{
		}

		public WoodlandBelt( Serial serial )
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

