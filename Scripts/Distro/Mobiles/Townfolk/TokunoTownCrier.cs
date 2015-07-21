using System;
using System.Text;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
	public class TokunoTownCrier : TownCrier
	{
		[Constructable]
		public TokunoTownCrier()
		{
			AddItem( new LeatherNinjaPants() );
			AddItem( new LightPlateJingasa() );
			AddItem( new LeatherNinjaBelt() );
			AddItem( new HakamaShita() );
			AddItem( new SamuraiTabi() );
		}

		public TokunoTownCrier( Serial serial )
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