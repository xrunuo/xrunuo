using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2FBA, 0x3174 )]
	public class FemaleElvenRobe : BaseOuterTorso
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public FemaleElvenRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public FemaleElvenRobe( int hue )
			: base( 0x2FBA, hue )
		{
			Weight = 2.0;
		}

		public FemaleElvenRobe( Serial serial )
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