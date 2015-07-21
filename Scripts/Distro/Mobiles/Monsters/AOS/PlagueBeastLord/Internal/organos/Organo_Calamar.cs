using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Organo_Calamar : BaseOrgano
	{
		public Item Suborgano;
		public Item Agujero; //Simula el agujero cuando cortamos el organo
		public Item Glandula;
		public Item Gland_Recpt;

		public Item Patas1;
		public Item Patas2;
		public Item Patas3;

		public bool Gland_Colocada;


		public Organo_Calamar( BrainTypes cerebro )
			: base( 4962, Plague_Texts.Line[15], 0x26B, cerebro )
		{
			Gland_Colocada = false;
		}

		public Organo_Calamar( Serial serial )
			: base( serial )
		{
		}

		public override void CrearVisceras()
		{
			Gland_Recpt = new Plague_deco( 4650, Plague_Texts.Line[24], 602, false, false );
			PonerSangre( Gland_Recpt, -2, 8 );
			Glandula = new Plague_deco( 7407, Plague_Texts.Line[16], 1271, true, false );
			PonerSangre( Glandula, -3, 15 );

			Suborgano = new Plague_deco( 4960, Plague_Texts.Line[17], 1271, false, true );
			PonerSangre( Suborgano, 45, 15 );

			Patas1 = new Plague_deco( 6939, Plague_Texts.Line[26], 1271, false, true );
			PonerSangre( Patas1, 24, 12 );
			Patas2 = new Plague_deco( 6939, Plague_Texts.Line[26], 1271, false, true );
			PonerSangre( Patas2, 27, 8 );
			Patas3 = new Plague_deco( 6939, Plague_Texts.Line[26], 1271, false, true );
			PonerSangre( Patas3, 30, 4 );


			Agujero = new Plague_deco( 7027/* 4650*/ , Plague_Texts.Line[8], 1, false, false );
			PonerSangre( Agujero, 39, 20 );
			Mybrain = new PlagueBrain( HoldBrain );
			PonerSangre( Mybrain, 37, 22 );
			PlagueBrain Cere = Mybrain as PlagueBrain;
			Cere.CrearVisceras( 4652, -4, -5 );

		}

		public void CambiaGland( Mobile from, Item glandula )
		{

			if ( Glandula.RootParent.Equals( RootParent ) )
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[18] );
			if ( Gland_Colocada )
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[19] );

			if ( ( !Gland_Colocada ) && ( !Glandula.RootParent.Equals( RootParent ) ) ) //Si la glandula mala se sacó del bicho...
			{
				Suborgano.Hue = Hue;
				Patas1.Hue = Hue;
				Patas2.Hue = Hue;
				Patas3.Hue = Hue;
				Gland_Colocada = true;
				glandula.Movable = false;
				Agujero.Visible = true;
				base.Abrir( from );
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[20] );
				//Si no hay cerebro, la herida empieza a sangrar directamente
				if ( HoldBrain == BrainTypes.Brain_None )
				{
					PlagueBrain pb = Mybrain as PlagueBrain;
					pb.Sangrar( from );
				}
			}

		}

		public override void Abrir( Mobile from )
		{
			PlagueBeastLord m = RootParent as PlagueBeastLord;
			if ( m == null ) return;

			from.Direction = from.GetDirectionTo( m );
			if ( Abierto )
			{
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[21] );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[22] );
				from.PlaySound( 0x248 );
				from.PlaySound( 0x2AC );
				Gland_Recpt.Visible = true;
				Glandula.Visible = true;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (Item) Agujero );
			writer.Write( (Item) Glandula );
			writer.Write( (Item) Gland_Recpt );
			writer.Write( (Item) Suborgano );
			writer.Write( (Item) Patas1 );
			writer.Write( (Item) Patas2 );
			writer.Write( (Item) Patas3 );

			writer.Write( (bool) Gland_Colocada );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			Agujero = reader.ReadItem();
			Glandula = reader.ReadItem();
			Gland_Recpt = reader.ReadItem();
			Suborgano = reader.ReadItem();
			Patas1 = reader.ReadItem();
			Patas2 = reader.ReadItem();
			Patas3 = reader.ReadItem();
			Gland_Colocada = reader.ReadBool();
		}
	}
}
