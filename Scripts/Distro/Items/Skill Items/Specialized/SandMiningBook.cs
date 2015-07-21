using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class SandMiningBook : Item
	{
		[Constructable]
		public SandMiningBook()
			: base( 0xFF4 )
		{
			Name = "Find Glass-Quality Sand";
			Weight = 1.0;
		}

		public SandMiningBook( Serial serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( pm == null || from.Skills[SkillName.Mining].Base < 100.0 )
				pm.SendLocalizedMessage( 1080041 ); // Only a Grandmaster Miner can learn from this book.
			else if ( pm.SandMining )
				pm.SendLocalizedMessage( 1080066 ); // You have already learned this information.
			else
			{
				pm.SandMining = true;
				pm.SendLocalizedMessage( 1111701 ); // You have learned how to mine fine sand.  Target sand areas when mining to look for fine sand.
				Delete();
			}
		}
	}
}
