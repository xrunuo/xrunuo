using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{

	public enum OrganoTypes
	{
		Organo_Maiden = 0,
		Organo_Venas = 1,
		Organo_Pulpo = 2,
		Organo_Calamar = 3,
		Organo_Central = 4
	}

	public class BaseOrgano : Item
	{

		public Item Mybrain;

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


		public BaseOrgano( int itemID, string name, int color, BrainTypes cerebro )
			: base( itemID )
		{
			Name = name;

			m_brain = cerebro;
			Hue = color;
			Stackable = false;
			Movable = false;
			Weight = 1;
		}
		public virtual void CrearVisceras()
		{
		}



		public BaseOrgano( Serial serial )
			: base( serial )
		{
		}

		public void PonerSangre( Item t, int x_, int y_ )
		{
			Container c = ( Parent as Container );
			if ( ( t != null ) && ( c != null ) )
			{
				c.AddItem( t );
				t.Location = new Point3D( X + x_, Y + y_, 0 );
			}
		}

		public override bool IsAccessibleTo( Mobile check )
		{
			return true;
		}

		public virtual void Abrir( Mobile from )
		{
			Abierto = true;
			if ( m_brain != BrainTypes.Brain_None )
			{
				Mybrain.Visible = true;
			}
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (bool) m_abierto );
			writer.Write( (int) m_brain );
			writer.Write( (Item) Mybrain );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
			m_abierto = reader.ReadBool();
			m_brain = (BrainTypes) reader.ReadInt();
			Mybrain = reader.ReadItem();
		}
	}
}
