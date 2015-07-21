using System;
using Server;

namespace Server.Items
{
	public class EssenceOfBattle : GoldRing
	{
		public override int LabelNumber { get { return 1072935; } } // Essence of Battle
		[Constructable]
		public EssenceOfBattle()
		{
			Hue = 0x4E9;
			Attributes.BonusDex = 7;
			Attributes.BonusStr = 7;
			Attributes.WeaponDamage = 30;
		}

		public EssenceOfBattle( Serial serial )
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

			/*int version = */
			reader.ReadInt();
			Hue = 0x4E9;
		}
	}
}