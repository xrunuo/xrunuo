using System;
using Server;

namespace Server.Items
{
	public class BloodwoodSpirit : BaseTalisman
	{
		public override int LabelNumber { get { return 1075034; } } // Bloodwood Spirit

		[Constructable]
		public BloodwoodSpirit()
			: base( 0x2F5A )
		{
			Weight = 1.0;
			Hue = 0x27;
			SkillBonuses.SetValues( 0, SkillName.SpiritSpeak, 10.0 );
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 5.0 );
			ProtectionTalis = ProtectionKillerEntry.GetRandom();
			ProtectionValue = 1 + Utility.Random( 59 );
			TalismanType = TalismanType.DamageRemoval;
			Charges = -1;
		}

		public BloodwoodSpirit( Serial serial )
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
