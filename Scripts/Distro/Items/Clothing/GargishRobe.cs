using System;

namespace Server.Items
{
	[FlipableAttribute( 0x4000, 0x4001 )]
	public class GargishRobe : BaseOuterTorso
	{
		public override int LabelNumber { get { return 1095256; } } // gargish robe

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishRobe( int hue )
			: base( 0x4000, hue )
		{
			Weight = 3.0;
		}

		public GargishRobe( Serial serial )
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
