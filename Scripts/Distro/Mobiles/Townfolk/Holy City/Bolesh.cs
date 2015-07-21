using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Bolesh : BaseCreature
	{
		[Constructable]
		public Bolesh()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Name = "Bolesh";
			Title = "the Warrior";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86DE;
			HairItemID = 0x425E;
			HairHue = 0x321;

			AddItem( new GargishClothKilt( 0x8FD ) );
		}

		public override bool CanTeach { get { return false; } }

		public Bolesh( Serial serial )
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