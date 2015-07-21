using System;
using Server;

namespace Server.Items
{
	public class TalismanTalkingtoWisps : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1074888; } } // Library Talisman - Talking to Wisps Ward Removal

		[Constructable]
		public TalismanTalkingtoWisps()
			: base( 0x2F5B )
		{
			Weight = 1.0;
			TalismanType = TalismanType.WardRemoval;
			SkillBonuses.SetValues( 0, SkillName.SpiritSpeak, 3.0 );
			SkillBonuses.SetValues( 1, SkillName.EvalInt, 5.0 );
			Charges = -1;
		}

		public TalismanTalkingtoWisps( Serial serial )
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
