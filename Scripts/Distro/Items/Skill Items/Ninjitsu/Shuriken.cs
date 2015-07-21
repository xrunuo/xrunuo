using System;
using Server;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x27AC, 0x27F7 )]
	public class Shuriken : Item, IUsesRemaining, ICraftable
	{
		private int m_UsesRemaining;
		private Poison m_Poison;
		private int m_PoisonCharges;
		private bool m_Exceptional;
		private Mobile m_Crafter;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonCharges
		{
			get { return m_PoisonCharges; }
			set { m_PoisonCharges = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Exceptional
		{
			get { return m_Exceptional; }
			set { m_Exceptional = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get { return m_Crafter; }
			set { m_Crafter = value; InvalidateProperties(); }
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		[Constructable]
		public Shuriken()
			: this( 1 )
		{
		}

		public Shuriken( int uses )
			: base( 0x27AC )
		{
			Weight = 1.0;

			m_UsesRemaining = uses;
		}

		public Shuriken( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Exceptional )
				list.Add( 1060636 ); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Poison != null && m_PoisonCharges > 0 )
				list.Add( 1062412 + m_Poison.Level, m_PoisonCharges.ToString() );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( (bool) m_Exceptional );
			writer.Write( (Mobile) m_Crafter );

			Poison.Serialize( m_Poison, writer );
			writer.Write( m_PoisonCharges );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
					{
						m_Exceptional = reader.ReadBool();
						m_Crafter = reader.ReadMobile();

						goto case 2;
					}
				case 2:
					{
						m_Poison = Poison.Deserialize( reader );
						m_PoisonCharges = reader.ReadInt();

						m_UsesRemaining = reader.ReadInt();

						break;
					}
			}
		}

		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			if ( exceptional )
				UsesRemaining *= 2;

			return exceptional;
		}
	}
}