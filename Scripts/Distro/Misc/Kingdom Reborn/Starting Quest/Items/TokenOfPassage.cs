using System;
using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	public class TokenOfPassage : Item
	{
		public override int LabelNumber { get { return 1078383; } } // Token of Passage

		[Constructable]
		public TokenOfPassage()
			: base( 0xF8B )
		{
			Weight = 1.0;
		}

		public TokenOfPassage( Serial serial )
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
			
			/*int version = */reader.ReadInt();
		}
	}
}