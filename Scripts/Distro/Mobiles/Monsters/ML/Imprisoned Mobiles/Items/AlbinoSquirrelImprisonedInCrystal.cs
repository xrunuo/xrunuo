using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AlbinoSquirrelImprisonedInCrystal : BaseImprisonedMobile
	{
		public override int LabelNumber{ get{ return 1075004; } } // An Albino Squirrel Imprisoned in a Crystal
		public override BaseCreature Summon{ get{ return new AlbinoSquirrel(); } }
		
		[Constructable]
		public AlbinoSquirrelImprisonedInCrystal() : base( 0x1F1C )
		{
			Weight = 1.0;
			Hue = 0x482;
		}

		public AlbinoSquirrelImprisonedInCrystal( Serial serial ) : base( serial )
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