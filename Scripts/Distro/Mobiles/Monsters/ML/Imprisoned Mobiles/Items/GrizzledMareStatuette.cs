using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class GrizzledMareStatuette : BaseImprisonedMobile
	{
		public override int LabelNumber{ get{ return 1074475; } } // Grizzled Mare Statuette
		public override BaseCreature Summon{ get { return new GrizzledMare(); } }
	
		[Constructable]
		public GrizzledMareStatuette() : base( 0x2617 )
		{
			Weight = 1.0;
		}

		public GrizzledMareStatuette( Serial serial ) : base( serial )
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