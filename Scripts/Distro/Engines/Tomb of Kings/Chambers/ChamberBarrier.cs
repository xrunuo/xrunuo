using System;
using Server;
using Server.Items;

namespace Server.Engines.TombOfKings
{
	public class ChamberBarrier : Item
	{
		private Blocker m_Blocker;

		public bool Active
		{
			get { return Visible; }
			set
			{
				if ( Visible != value )
				{
					if ( Visible = value )
					{
						m_Blocker = new Blocker();
						m_Blocker.MoveToWorld( Location, Map );
					}
					else
					{
						m_Blocker.Delete();
						m_Blocker = null;
					}
				}
			}
		}

		public ChamberBarrier( Point3D loc )
			: base( 0x3979 )
		{
			Light = LightType.Circle150;

			Movable = false;
			MoveToWorld( loc, Map.TerMur );

			m_Blocker = new Blocker();
			m_Blocker.MoveToWorld( loc, Map.TerMur );
		}

		public ChamberBarrier( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) ( m_Blocker != null ) );

			if ( m_Blocker != null )
				writer.Write( (Item) m_Blocker );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( reader.ReadBool() )
			{
				m_Blocker = reader.ReadItem() as Blocker;
				m_Blocker.Delete();
			}

			Delete();
		}
	}
}