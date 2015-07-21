using System;

namespace Server.Items
{
	[FlipableAttribute( 0x3175, 0x2FBE )]
	public class ElvenShirt : BaseShirt
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenShirt()
			: this( 0 )
		{
		}

		[Constructable]
		public ElvenShirt( int hue )
			: base( 0x3175, hue )
		{
			Weight = 2.0;
		}

		public ElvenShirt( Serial serial )
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
