using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ImprisonedDogStatuette : BaseImprisonedMobile
	{
		public override BaseCreature Summon{ get{ return new ImprisonedDog(); } }
		
		[Constructable]
		public ImprisonedDogStatuette() : base( 0x1F19 )
		{
			Name = "an imprisoned dog";
			Weight = 1.0;
			Hue = 0x485; // TODO check
		}

		public ImprisonedDogStatuette( Serial serial ) : base( serial )
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