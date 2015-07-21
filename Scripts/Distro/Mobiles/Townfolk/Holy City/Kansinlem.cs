using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Kansinlem : BaseCreature
	{
		[Constructable]
		public Kansinlem()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Name = "Kansinlem";
			Title = "the Cook";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86DF;
			HairItemID = 0x425B;
			HairHue = 0x323;

			AddItem( new GargishClothChest( 0x661 ) );
			AddItem( new GargishClothKilt( 0x220 ) );
		}

		public override bool CanTeach { get { return false; } }

		public Kansinlem( Serial serial )
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