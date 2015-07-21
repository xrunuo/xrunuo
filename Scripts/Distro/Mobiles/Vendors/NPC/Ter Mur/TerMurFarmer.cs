using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class TerMurFarmer : Farmer
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurFarmer()
		{
			Title = "the Farmer";
		}

		public TerMurFarmer( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}