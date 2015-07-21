using System;

namespace Server.Items
{
	public class GargishHalfApron : BaseWaist
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishHalfApron()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishHalfApron( int hue )
			: base( 0x50D8, hue )
		{
			Weight = 2.0;
		}

		public GargishHalfApron( Serial serial )
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
