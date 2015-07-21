using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurMage : Mage
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurMage()
		{
			Title = "the Mage";
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Spellbook() );
		}

		public TerMurMage( Serial serial )
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