using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Plague_Blood : Item
	{
		Item Venda;

		bool m_taponado = false;
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Taponado
		{
			get { return m_taponado; }
			set { m_taponado = value; }
		}

		public Plague_Blood()
			: this( 0x1645 )
		{
		}

		public Plague_Blood( int itemID )
			: base( itemID )
		{
			Name = Plague_Texts.Line[8];
			Movable = false;
			Visible = false;
		}

		public Plague_Blood( Serial serial )
			: base( serial )
		{
		}

		public virtual void CrearVisceras()
		{
			Container c = ( Parent as Container );

			Venda = new Plague_deco( 6420, Plague_Texts.Line[9], 0, false, false );
			c.AddItem( Venda );
			Venda.Location = new Point3D( X + 9, Y, 0 );
		}

		public virtual void Hemorragia( Mobile from )
		{
			if ( !Visible )
			{
				new InternalTimer( this ).Start();
				Visible = true;
				//Se abre la herida;
				//b.Heridas += 1;
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[10] );
			}
		}

		public virtual void Taponar( Mobile from )
		{
			if ( !m_taponado )
			{
				Venda.Visible = true;
				m_taponado = true;
				//b.Heridas -= 1;	         	
				LabelTo( from, Plague_Texts.Line[11] );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (bool) m_taponado );
			writer.Write( (Item) Venda );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			m_taponado = reader.ReadBool();
			Venda = reader.ReadItem();
			if ( ( Visible ) && ( !m_taponado ) )
			{ new InternalTimer( this ).Start(); }
		}

		private class InternalTimer : Timer
		{
			private Plague_Blood myblood;
			private PlagueBeastLord PBL;
			private int timepassed;

			public InternalTimer( Plague_Blood item_ )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				myblood = item_;
				timepassed = 0;
				PBL = myblood.RootParent as PlagueBeastLord;
			}

			protected override void OnTick() //Each second
			{

				if ( ( myblood == null ) || ( !myblood.Visible ) || ( myblood.m_taponado )
					  || ( PBL == null ) || ( !PBL.Alive ) )
				{
					Stop();
					return;
				}
				switch ( timepassed++ )
				{
					case 1: myblood.ItemID = 4654;
						myblood.Location = new Point3D( myblood.X + 5, myblood.Y, 0 );
						break;
					case 2: myblood.ItemID = 4653; break;
					case 3: myblood.ItemID = 4650; break;
					case 4: PBL.Kill(); break;
				}
			}
		}


	}


}
