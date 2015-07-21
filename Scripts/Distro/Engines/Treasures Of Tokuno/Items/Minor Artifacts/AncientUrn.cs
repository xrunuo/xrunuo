using System;
using Server;

namespace Server.Items
{
	public class AncientUrn : Item
	{
		private string m_Names;

		public string Names { get { return m_Names; } set { m_Names = value; } }

		[Constructable]
		public AncientUrn()
			: base( 0x241D )
		{
			m_Names = NameList.RandomName( "ancient urn names" );

			Weight = 1.0;
		}

		public AncientUrn( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1070935, m_Names ); // Ancient Urn of ~1_name~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (string) m_Names );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Names = reader.ReadString();

						goto case 0;
					}
				case 0:
					{
						break;
					}
			}

			if ( version < 1 )
			{
				m_Names = NameList.RandomName( "tokuno treasures names" );
			}
		}
	}
}