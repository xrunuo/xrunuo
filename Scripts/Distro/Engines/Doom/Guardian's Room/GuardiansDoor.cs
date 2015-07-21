using System;
using Server;

namespace Server.Items
{
	public class GuardianDoor : BaseDoor
	{
		private InternalItem m_Item;
		private DoorFacing m_Face;

		[Constructable]
		public GuardianDoor( DoorFacing facing )
			: base( 0x675 + ( 2 * (int) facing ), 0x676 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
			Movable = false;
			m_Face = facing;
			m_Item = new InternalItem( m_Face + 1, this );
			Link = m_Item;
		}

		public GuardianDoor( Serial serial )
			: base( serial )
		{
		}

		public override void OnMapChange()
		{
			if ( m_Item != null )
			{
				m_Item.Map = Map;
				m_Item.Location = new Point3D( X, Y - 1, Z );
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Item != null )
				m_Item.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Item );

			writer.Write( (int) m_Face );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;

			m_Face = (DoorFacing) ( reader.ReadInt() );
		}

		private class InternalItem : BaseDoor
		{
			private GuardianDoor m_Item;
			private DoorFacing m_Face;

			public InternalItem( DoorFacing facing, GuardianDoor item )
				: base( 0x675 + ( 2 * (int) facing ), 0x676 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
			{
				Movable = false;
				m_Face = facing;
				m_Item = item;
				Link = m_Item;
			}

			public InternalItem( Serial serial )
				: base( serial )
			{
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Item != null )
				{
					m_Item.Delete();
				}
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version

				writer.Write( m_Item );

				writer.Write( (int) m_Face );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				/*int version = */
				reader.ReadInt();

				m_Item = reader.ReadItem() as GuardianDoor;

				m_Face = (DoorFacing) ( reader.ReadInt() );
			}
		}
	}
}
