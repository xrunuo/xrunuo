using System;
using Server;

namespace Server.Items
{
	public class Plague_deco : Item
	{

		public Plague_deco( int itemID, string name, int color, bool movil, bool visible )
			: base( itemID )
		{
			Name = name;
			Movable = movil;
			Visible = visible;
			Hue = color;
		}
		public Plague_deco( Serial serial )
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
