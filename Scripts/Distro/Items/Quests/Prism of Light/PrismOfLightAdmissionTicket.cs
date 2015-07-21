using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class PrismOfLightAdmissionTicket : TransientItem
	{
		private static readonly Point3D m_Dest = new Point3D( 3785, 1107, 20 );

		public override int LabelNumber { get { return 1074340; } } // Prism of Light Admission Ticket

		[Constructable]
		public PrismOfLightAdmissionTicket()
			: base( 0x14EF, TimeSpan.FromSeconds( 43200 ) )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public PrismOfLightAdmissionTicket( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1074841 ); // Double click to transport out of the Prism of Light dungeon
			list.Add( 1075269 ); // Destroyed when dropped
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( ( m.Map == Map.Felucca || m.Map == Map.Trammel ) && m.X > 6435 && m.Y > 9 && m.X < 6631 && m.Y < 244 ) // Prism of Light
			{
				BaseCreature.TeleportPets( m, m_Dest, m.Map );
				m.MoveToWorld( m_Dest, m.Map );
			}
			else
				m.SendLocalizedMessage( 1074840 ); // This ticket can only be used while you are in the Prism of Light dungeon.
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			Delete();
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