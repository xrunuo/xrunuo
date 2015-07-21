using System;
using Server;


namespace Server.Items
{
	[FlipableAttribute( 0x3159, 0x3158 )]
	public class MountedDreadHorn : Item
	{

		[Constructable]
		public MountedDreadHorn()
			: base( 0x3159 )
		{
			Weight = 0.0;
		}

		public MountedDreadHorn( Serial serial )
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

