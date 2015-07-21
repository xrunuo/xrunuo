using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Organo_Pulpo : BaseOrgano
	{
		public Item Agujero; //Simula el agujero cuando cortamos el organo

		public Organo_Pulpo( BrainTypes cerebro )
			: base( 6002, Plague_Texts.Line[15], 0x66B, cerebro )
		{
		}

		public Organo_Pulpo( Serial serial )
			: base( serial )
		{
		}

		public override void CrearVisceras()
		{
			//Container c= (Parent as Container);

			PonerSangre( new Plague_deco( 7857, Plague_Texts.Line[23], Hue, false, true ), -25, 32 );
			PonerSangre( new Plague_deco( 7858, Plague_Texts.Line[23], Hue, false, true ), 20, 32 );

			Agujero = new Plague_deco( 4650, Plague_Texts.Line[8], 1, false, false );
			PonerSangre( Agujero, 1, 13 );

			Mybrain = new PlagueBrain( HoldBrain );
			PonerSangre( Mybrain, 0, 20 );
			PlagueBrain Cere = Mybrain as PlagueBrain;

			Cere.CrearVisceras( 4652, -4, -5 );
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
				Agujero.Visible = true;

				base.Abrir( from );
				//Si no hay cerebro, la herida empieza a sangrar directamente
				if ( HoldBrain == BrainTypes.Brain_None )
				{
					PlagueBrain pb = Mybrain as PlagueBrain;
					pb.Sangrar( from );
				}
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
