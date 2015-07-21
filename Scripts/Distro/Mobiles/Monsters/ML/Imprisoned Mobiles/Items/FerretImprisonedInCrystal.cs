using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class FerretImprisonedInCrystal : BaseImprisonedMobile
	{
		public override BaseCreature Summon{ get{ return new SpecialFerret(); } }
		
		[Constructable]
		public FerretImprisonedInCrystal() : base( 0x1F19 )
		{
			Name = "a ferret imprisoned in a crystal";
			Weight = 1.0;
		}

		public FerretImprisonedInCrystal( Serial serial ) : base( serial )
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

