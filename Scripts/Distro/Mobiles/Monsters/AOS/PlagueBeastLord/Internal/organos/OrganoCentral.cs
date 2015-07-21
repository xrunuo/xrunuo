using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class OrganoCentral : BaseOrgano
	{
		public Item Agujero; //Simula el agujero cuando cortamos el organo

		public OrganoCentral()
			: base( 4538, Plague_Texts.Line[12], 0x26, BrainTypes.Brain_None )
		{
		}

		public OrganoCentral( Serial serial )
			: base( serial )
		{
		}

		public override void CrearVisceras()
		{
			Agujero = new Plague_deco( 4650, Plague_Texts.Line[8], 1, false, false );
			PonerSangre( Agujero, 42, 21 );

			Mybrain = new Plague_Core();
			PonerSangre( Mybrain, 42, 28 );
		}

		public override void Abrir( Mobile from )
		{
			if ( RootParent.Equals( from ) )
			{
				Agujero.Visible = true;
				Mybrain.Visible = true;
				from.Say( false, Plague_Texts.Line[14] );
			}
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (Item) Agujero );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			Agujero = reader.ReadItem();
		}
	}
}
