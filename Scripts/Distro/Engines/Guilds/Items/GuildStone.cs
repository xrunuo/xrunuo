
namespace Server.Engines.Guilds.Items
{
	[TypeAlias( "Server.Items.Guildstone" )]
	public class Guildstone : Item
	{
		private Guild m_Guild;

		public Guild Guild { get { return m_Guild; } }

		public override int LabelNumber { get { return 1041429; } } // a Guildstone

		public Guildstone( Guild g )
			: base( 0xED4 )
		{
			m_Guild = g;
			Movable = false;
		}

		public Guildstone( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Guild );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Guild = reader.ReadGuild() as Guild;

						goto case 0;
					}
				case 0:
					{
						break;
					}
			}

			if ( m_Guild == null )
			{
				this.Delete();
			}
		}

		public override void OnAfterDelete()
		{
			if ( m_Guild != null && !m_Guild.Disbanded )
			{
				m_Guild.Disband();
			}
		}
	}
}