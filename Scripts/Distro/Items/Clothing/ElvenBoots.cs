using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2FC4, 0x317A )]
	public class ElvenBoots : BaseShoes
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenBoots()
			: this( 0 )
		{
		}

		[Constructable]
		public ElvenBoots( int hue )
			: base( 0x2FC4, hue )
		{
			Weight = 2.0;
		}

		public ElvenBoots( Serial serial )
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