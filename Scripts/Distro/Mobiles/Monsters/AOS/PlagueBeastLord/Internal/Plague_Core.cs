using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Plague_Core : Item
	{
		[Constructable]
		public Plague_Core()
			: base( 7408 )
		{
			Name = Plague_Texts.Line[13];
			Movable = false;
			Visible = false;
			Hue = 0x480;
		}
		public Plague_Core( Serial serial )
			: base( serial )
		{
		}

		public void Cortar( Mobile from )
		{
			if ( !Movable )
			{
				Movable = true;
				PlagueBeastLord PBL = RootParent as PlagueBeastLord;
				( (Container) Parent ).RemoveItem( this );
				( from.Backpack ).DropItem( this );
				from.PlaySound( 0x248 );
				from.SendMessage( Plague_Texts.Line[32] );
				PBL.Kill();
			}
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
