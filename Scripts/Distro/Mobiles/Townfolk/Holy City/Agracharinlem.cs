using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Agracharinlem : BaseCreature
	{
		[Constructable]
		public Agracharinlem()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Name = "Agracharinlem";
			Title = "the Weaponsmith";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86E8;
			HairItemID = 0x4259;
			HairHue = 0x31D;

			AddItem( new GargishLeatherKilt() );
			AddItem( new GargishLeatherChest() );
			AddItem( new GargishLeatherArms() );
		}

		public override bool CanTeach { get { return false; } }

		public Agracharinlem( Serial serial )
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