using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoArchitect : Architect
	{
		[Constructable]
		public TokunoArchitect()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			if ( Female )
			{
				AddItem( new Doublet() );
			}
			else
			{
				AddItem( new FancyShirt() );
			}

			AddItem( new Kamishimo() );
		}

		public TokunoArchitect( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}