using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{

	public enum BrainTypes
	{
		Brain_None = 0,
		Brain_Green = 1,
		Brain_Blue = 2,
		Brain_Yellow = 3,
		Brain_Purple = 4
	}


	public class PlagueBrain : Item
	{


		public static Point3D[] BrainSlot = new Point3D[]
	  {
	  	new Point3D(10,10,0), //Este no vale pa na. Es del cerebro none
	  	new Point3D(150,170,0),	  	
	  	new Point3D(240,170,0),	  		  	
	  	new Point3D(150,240,0),	  		  	
	  	new Point3D(240,240,0),	  		  	
	  };

		public static Point3D[] Recpt_Connectors = new Point3D[]
	  {
	  	new Point3D(10,10,0), //Este no vale pa na. Es del cerebro none
	  	new Point3D(170,179,0),	  	
	  	new Point3D(229,179,0),	  		  	
	  	new Point3D(170,229,0),	  		  	
	  	new Point3D(229,229,0),	  		  	
	  };

		public static int[] BrainColors = new int[]
			{  
				0x0001,  //None
				0x0047,  //Verde
				0x0060,  //Azul
				0x0035,  //Amarillo
				0x0010   //Purpura
			};

		Item sangre;
		private Item Bolsa;

		bool m_abierto = false;
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Abierto
		{
			get { return m_abierto; }
			set { m_abierto = value; }
		}


		BrainTypes m_brain;
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public BrainTypes HoldBrain
		{
			get { return m_brain; }
			set { m_brain = value; }
		}


		public PlagueBrain( BrainTypes t )
			: base( 7408 )
		{
			Name = Plague_Texts.Line[6];
			m_brain = t;
			Hue = BrainColors[(int) t];
			Stackable = false;
			Movable = true;
			Weight = 40;
			Visible = false;
		}



		public PlagueBrain( Serial serial )
			: base( serial )
		{
		}


		public virtual void CrearVisceras( int bloodtype, int x_, int y_ )
		{
			Container c = Parent as Container;
			Bolsa = c;
			sangre = new Plague_Blood( bloodtype );
			c.AddItem( sangre );
			sangre.Location = new Point3D( X + x_ - 5, Y + y_, 0 );
			Plague_Blood pb = sangre as Plague_Blood;
			pb.CrearVisceras();
		}


		public void Sangrar( Mobile from )
		{
			Plague_Blood blood_ = sangre as Plague_Blood;
			blood_.Hemorragia( from );
		}

		public override bool DropToMobile( Mobile from, Mobile target, Point3D p )
		{ return false; }

		public override void OnItemLifted( Mobile from, Item item )
		{
			Sangrar( from );
			base.OnItemLifted( from, item );
		}

		public override bool DropToItem( Mobile from, Item target, Point3D p, byte gridloc )
		{
			//Comprobamos si el cachivache está colocado		
			if ( ( m_brain != BrainTypes.Brain_None ) && ( target.Equals( Bolsa ) ) )
			{
				PlagueBeastLord PBL = Bolsa.Parent as PlagueBeastLord;
				Point3D p2 = BrainSlot[(int) m_brain];
				if ( ( ( p2.X ) <= p.X ) && ( p.X <= ( p2.X + 5 ) )
					 && ( ( p2.Y ) <= p.Y ) && ( p.Y <= ( p2.Y + 5 ) ) )
				{
					PBL.ColocarCerebro( (int) m_brain - 1, true );
					Movable = false;
					from.LocalOverheadMessage( MessageType.Regular, 0x66B, false, Plague_Texts.Line[7] );
				}
				else { PBL.ColocarCerebro( (int) m_brain - 1, false ); }
			}
			return ( ( target.Equals( Bolsa ) ) && p.X != -1 && p.Y != -1 && base.DropToItem( from, target, p, gridloc ) );
		}

		public override bool DropToWorld( Mobile from, Point3D p )
		{ return false; }

		public override int GetLiftSound( Mobile from )
		{ return 682; }


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (bool) m_abierto );
			writer.Write( (int) m_brain );
			writer.Write( (Item) sangre );
			writer.Write( (Item) Bolsa );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			m_abierto = reader.ReadBool();
			m_brain = (BrainTypes) reader.ReadInt();
			sangre = reader.ReadItem();
			Bolsa = reader.ReadItem();
		}
	}
}
