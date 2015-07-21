using System;

namespace Server.Items
{
	[FlipableAttribute( 0x3176, 0x3177 )]
	public class ElvenShirtDark : BaseShirt
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenShirtDark()
			: base( 0x3176 )
		{
			Weight = 5.0;
		}

		public ElvenShirtDark( Serial serial )
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