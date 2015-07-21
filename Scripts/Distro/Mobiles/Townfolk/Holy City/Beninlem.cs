using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Beninlem : BaseCreature
	{
		[Constructable]
		public Beninlem()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Name = "Beninlem";
			Title = "the Goodscrafter";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86DF;
			HairItemID = 0x425F;
			HairHue = 0x31F;

			AddItem( new GargishClothChest( 0x727 ) );
			AddItem( new GargishClothKilt( 0x72A ) );
		}

		public override bool CanTeach { get { return false; } }

		public Beninlem( Serial serial )
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