using System;
using Server;

namespace Server.Items
{
	public class UnderworldSecretDoor : Item
	{
		public override int LabelNumber { get { return 1020233; } } // secret door

		private int m_ClosedId, m_MediumId;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ClosedId
		{
			get { return m_ClosedId; }
			set { m_ClosedId = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MediumId
		{
			get { return m_MediumId; }
			set { m_MediumId = value; }
		}

		[Constructable]
		public UnderworldSecretDoor( int closedId, int mediumId )
			: base( closedId )
		{
			Movable = false;

			m_ClosedId = closedId;
			m_MediumId = mediumId;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this, 1 ) )
				return;

			if ( ItemID == ClosedId )
			{
				Effects.PlaySound( Location, Map, 0x21D );
				ItemID = m_MediumId;

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( Open ) );
			}
		}

		protected void Open()
		{
			Effects.PlaySound( Location, Map, 0x21D );
			ItemID = 1; // no draw

			Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback( BeginClose ) );
		}

		protected void BeginClose()
		{
			Effects.PlaySound( Location, Map, 0x21D );
			ItemID = m_MediumId;

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( Close ) );
		}

		protected void Close()
		{
			Effects.PlaySound( Location, Map, 0x21D );
			ItemID = m_ClosedId;
		}

		public UnderworldSecretDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_ClosedId );
			writer.Write( (int) m_MediumId );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_ClosedId = reader.ReadInt();
			m_MediumId = reader.ReadInt();

			// make sure we don't get stuck at opened state before deserialize
			ItemID = m_ClosedId;
		}
	}
}