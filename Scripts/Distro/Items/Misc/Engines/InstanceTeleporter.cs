using System;
using Server;

namespace Server.Items
{
	public class InstanceTeleporter : Teleporter
	{
		private int m_DestInstance;

		[CommandProperty( AccessLevel.GameMaster )]
		public int DestInstance
		{
			get { return m_DestInstance; }
			set { m_DestInstance = value; }
		}

		[Constructable]
		public InstanceTeleporter()
		{
		}

		public InstanceTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( Active )
				m.InstanceID = m_DestInstance;

			return base.OnMoveOver( m );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_DestInstance );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_DestInstance = reader.ReadInt();
		}
	}
}