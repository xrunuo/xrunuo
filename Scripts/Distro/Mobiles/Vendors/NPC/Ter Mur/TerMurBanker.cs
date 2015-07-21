using System;
using Server;

namespace Server.Mobiles
{
	public class TerMurBanker : Banker
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurBanker()
		{
			Title = "the Banker";
		}

		public TerMurBanker( Serial serial )
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