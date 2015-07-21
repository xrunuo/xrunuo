using System;
using Server;

namespace Server.Items
{
	public class HonorableSwords : Item
	{
		private string m_Names;

		public string Names { get { return m_Names; } set { m_Names = value; } }

		[Constructable]
		public HonorableSwords()
			: base( 0x2853 )
		{
			Weight = 15.0;
			m_Names = NameList.RandomName( "honorable swords names" );
		}

		public HonorableSwords( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1070936, m_Names ); // Honorable Swords of ~1_name~
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