using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Frederic : MondainQuester
	{
		public override Type[] Quests { get { return null; } }

		[Constructable]
		public Frederic()
			: base( "The Ghost of Frederic Smithson" )
		{
		}

		public Frederic( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Body = 0x1A;
			Hue = 0x455;
			CantWalk = true;
			Frozen = true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}