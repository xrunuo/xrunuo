using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DaemonBone : BaseReagent, ICommodity
	{
		[Constructable]
		public DaemonBone()
			: this( 1 )
		{
		}

		[Constructable]
		public DaemonBone( int amount )
			: base( 0xF80, amount )
		{
			Weight = 0.1;
		}

		public DaemonBone( Serial serial )
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