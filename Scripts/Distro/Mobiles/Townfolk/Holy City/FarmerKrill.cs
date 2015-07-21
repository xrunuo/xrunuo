using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class FarmerKrill : BaseCreature
	{
		[Constructable]
		public FarmerKrill()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Name = "Farmer Krill";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86DE;
			HairItemID = 0x4259;
			HairHue = 0x31E;

			AddItem( new GargishClothKilt( 0x516 ) );
			AddItem( new GargishClothArms( 0x531 ) );
		}

		public override bool CanTeach { get { return false; } }

		public FarmerKrill( Serial serial )
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