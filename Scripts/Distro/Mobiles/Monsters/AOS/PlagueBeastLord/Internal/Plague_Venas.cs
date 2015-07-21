using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class Plague_Venas : Item
	{
		Item m_Organo;
		bool Cortada = false;

		public Plague_Venas( Item m, int color, int flip_ )
			: base( 6939 + flip_ )
		{
			Name = Plague_Texts.Line[26];
			Movable = false;
			Visible = true;
			Hue = color;
			m_Organo = m;
		}
		public Plague_Venas( Serial serial )
			: base( serial )
		{
		}

		public void CutVein( Mobile from )
		{
			if ( !Cortada )
			{
				Cortada = true;
				ItemID = 7946;
				Organo_Venas Padre = m_Organo as Organo_Venas;
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[27] );
				Padre.VenaCortada( from, Hue );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (Item) m_Organo );
			writer.Write( (bool) Cortada );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			m_Organo = reader.ReadItem();
			Cortada = reader.ReadBool();
		}

	}

}
