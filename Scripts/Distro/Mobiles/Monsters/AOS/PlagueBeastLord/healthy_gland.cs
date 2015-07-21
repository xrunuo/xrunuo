//These are the healthy glands. Needed sometimes to resolve plague beast's puzzle.
//Added as loot on plague beasts.


using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Plague_gland : Item
	{
		[Constructable]
		public Plague_gland()
			: base( 7407 )
		{
			Name = Plague_Texts.Line[29];
			Movable = true;
			Visible = true;
			Hue = 1174;
		}

		public Plague_gland( Serial serial )
			: base( serial )
		{
		}

		public override bool DropToItem( Mobile from, Item target, Point3D p, byte gridloc )
		{
			if ( target is PlagueBackpack )
			{
				PlagueBeastLord PBL = target.RootParent as PlagueBeastLord;
				if ( PBL != null )
				{
					PBL.Place_Gland( p, this, from );
				}
			}
			return base.DropToItem( from, target, p, gridloc );
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
