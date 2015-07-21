using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class AbyssReaver : Cyclone
	{
		public override int LabelNumber { get { return 1112694; } } // Abyss Reaver

		[Constructable]
		public AbyssReaver()
		{
			Slayer = SlayerName.Demon;
			SkillBonuses.SetValues( 0, SkillName.Throwing, Utility.RandomMinMax( 5, 10 ) );
			Attributes.WeaponDamage = (short) Utility.RandomMinMax( 25, 35 );
		}

		public AbyssReaver( Serial serial )
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
