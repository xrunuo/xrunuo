using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;

namespace Server.Items
{
	public class StoneMemorial : Item
	{
		public override int LabelNumber { get { return 1095186; } } // stone memorial

		[Constructable]
		public StoneMemorial()
			: base( 0x117F )
		{
			Movable = false;
		}

		public StoneMemorial( Serial serial )
			: base( serial )
		{
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( Parent == null && this.InRange( m, 1 ) && !this.InRange( oldLocation, 1 ) )
				Ankhs.Resurrect( m, this );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			Ankhs.GetContextMenuEntries( from, this, list );
		}

		public override void OnDoubleClickDead( Mobile m )
		{
			Ankhs.Resurrect( m, this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}
}