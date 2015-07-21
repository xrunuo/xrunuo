using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2FB9, 0x3174 )]
	public class ElvenRobe : BaseOuterTorso
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public ElvenRobe( int hue )
			: base( 0x2FB9, hue )
		{
			Weight = 2.0;
		}

		public ElvenRobe( Serial serial )
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
