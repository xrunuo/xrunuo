using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2FC3, 0x3179 )]
	public class ElvenPants : BasePants
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenPants()
			: this( 0 )
		{
		}

		[Constructable]
		public ElvenPants( int hue )
			: base( 0x2FC3, hue )
		{
			Weight = 2.0;
		}

		public ElvenPants( Serial serial )
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
