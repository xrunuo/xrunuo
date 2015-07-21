using System;

namespace Server.Items
{
	[FlipableAttribute( 0x4002, 0x4003 )]
	public class GargishFancyRobe : BaseOuterTorso
	{
		public override int LabelNumber { get { return 1095258; } } // gargish fancy robe

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishFancyRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishFancyRobe( int hue )
			: base( 0x4002, hue )
		{
			Weight = 3.0;
		}

		public GargishFancyRobe( Serial serial )
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
