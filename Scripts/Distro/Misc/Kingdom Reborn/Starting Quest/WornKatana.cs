using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class WornKatana : Katana
	{
		public override int LabelNumber { get { return 1077958; } } // Worn Katana

		public override int InitMinHits { get { return 15; } }
		public override int InitMaxHits { get { return 15; } }

		[Constructable]
		public WornKatana()
		{
			Hue = 2420;
			WeaponAttributes.LowerStatReq = 60;
		}

		public override bool OnDragLift( Mobile from )
		{
            if ( base.OnDragLift( from ) )
			{
				if ( from is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile) from;

					pm.CheckKRStartingQuestStep( 11 );
				}

				return true;
			}

			return false;
		}

		public override bool OnEquip( Mobile from )
		{
			if ( base.OnEquip( from ) )
			{
				if ( from is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile) from;

					pm.CheckKRStartingQuestStep( 13 );
				}

				return true;
			}

			return false;
		}

		public WornKatana( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}