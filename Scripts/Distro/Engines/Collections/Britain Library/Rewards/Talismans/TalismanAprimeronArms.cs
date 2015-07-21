using System;
using Server;

namespace Server.Items
{
	public class TalismanAprimeronArms : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1074887; } } // Library Talisman - A Primer on Arms Damage Removal

		[Constructable]
		public TalismanAprimeronArms()
			: base( 0x2F59 )
		{
			Weight = 1.0;
			Attributes.BonusStr = 1;
			Attributes.RegenHits = 2;
			Attributes.WeaponDamage = 20;
			TalismanType = TalismanType.DamageRemoval;
			Charges = -1;
		}

		public TalismanAprimeronArms( Serial serial )
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
