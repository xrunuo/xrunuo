using System;
using Server;

namespace Server.Items
{
	public class LocalizedTeleporter : Teleporter
	{
		private int m_LabelNumber;

		public override int LabelNumber { get { return m_LabelNumber; } }

		public override bool ForceShowProperties { get { return true; } }

		public override bool ShowProperties { get { return false; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Number
		{
			get { return m_LabelNumber; }
			set
			{
				m_LabelNumber = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public LocalizedTeleporter( Point3D pointDest, Map mapDest, int labelNumber )
			: base( pointDest, mapDest )
		{
			m_LabelNumber = labelNumber;

			ItemID = 0x1822;

			Visible = true;
		}

		public LocalizedTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( m_LabelNumber );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_LabelNumber = reader.ReadInt();
						break;
					}
			}
		}
	}
}