using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Organo_Venas : BaseOrgano
	{
		public static int[] VenasColors = new int[]
			{  
				0x0492,  
				0x00E9,  
				0x0107, 
				0x0392, 
				0x0487,
				0x048A,
				0x0494
			};
		public static Point3D[] Pos_Venas = new Point3D[]
			{  
			 new Point3D( 30,20,0),
			 new Point3D( 30,30,0),			 			 			 
 			 new Point3D(  5,40,0),			 			 			 			 							 
 			 new Point3D(-10,40,0)
			};


		public Item Agujero; //Simula el agujero cuando cortamos el organo
		public int m_venascortadas = 0;

		public Organo_Venas( BrainTypes cerebro )
			: base( 13357, Plague_Texts.Line[15], VenasColors[Utility.RandomMinMax( 0, 6 )], cerebro )
		{
		}

		public Organo_Venas( Serial serial )
			: base( serial )
		{
		}

		public override void CrearVisceras()
		{
			int Venas_ = Utility.RandomMinMax( 0, 6 );
			for ( int i = 0; i < 4; i++ )
			{
				PonerSangre( new Plague_Venas( this, VenasColors[( Venas_ + i ) % 4], i / 2 ), Pos_Venas[i].X, Pos_Venas[i].Y );
			}


			Agujero = new Plague_deco( 4650, Plague_Texts.Line[8], 1, false, false );
			PonerSangre( Agujero, 1, 13 );

			Mybrain = new PlagueBrain( HoldBrain );
			PonerSangre( Mybrain, 0, 20 );
			PlagueBrain Cere = Mybrain as PlagueBrain;

			Cere.CrearVisceras( 4652, -4, -5 );
		}



		public void VenaCortada( Mobile from, int color )
		{
			from.PlaySound( 0x248 );
			if ( color != Hue )
			{
				m_venascortadas++;
				if ( m_venascortadas == 3 )
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
						from.PlaySound( 0x2AC );

						Agujero.Visible = true;
						Abierto = true;
						if ( HoldBrain != BrainTypes.Brain_None )
						{ Mybrain.Visible = true; }
						//Si no hay cerebro o no se cortaron las venas,
						//la herida empieza a sangrar directamente
						if ( HoldBrain == BrainTypes.Brain_None )
						{
							PlagueBrain pb = Mybrain as PlagueBrain;
							pb.Sangrar( from );
						}
					}

				}
			}
			else
			{
				m_venascortadas = 0;
				PlagueBeastLord PBL = RootParent as PlagueBeastLord;
				PBL.Kill();
			}
		}

		public override void Abrir( Mobile from )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_venascortadas );
			writer.Write( (Item) Agujero );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			m_venascortadas = reader.ReadInt();
			Agujero = reader.ReadItem();
		}
	}
}
