using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class EggBomb : SmokeBomb
	{
		public override int LabelNumber { get { return 1030249; } } // egg bomb

		[Constructable]
		public EggBomb()
		{
			Weight = 1.0;

			Stackable = true;
		}

		public EggBomb( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( !Stackable )
				Stackable = true;
		}
	}
}
