using System;
using Server;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
	public abstract class BaseTool : Item, IUsesRemaining, ICraftable
	{
		private int m_UsesRemaining;
		private bool m_Exceptional;
		private Mobile m_Crafter;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get { return m_Crafter; }
			set { m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Exceptional
		{
			get { return m_Exceptional; }
			set { UnscaleUses(); m_Exceptional = value; InvalidateProperties(); ScaleUses(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public void ScaleUses()
		{
			m_UsesRemaining = ( m_UsesRemaining * GetUsesScalar() ) / 100;
			InvalidateProperties();
		}

		public void UnscaleUses()
		{
			m_UsesRemaining = ( m_UsesRemaining * 100 ) / GetUsesScalar();
		}

		public int GetUsesScalar()
		{
			if ( m_Exceptional )
				return 300;

			return 100;
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		public abstract CraftSystem CraftSystem { get; }

		public BaseTool( int itemID )
			: this( Utility.RandomMinMax( 25, 75 ), itemID )
		{
		}

		public BaseTool( int uses, int itemID )
			: base( itemID )
		{
			m_UsesRemaining = uses;
		}

		public BaseTool( Serial serial )
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

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public static bool CheckAccessible( Item tool, Mobile m )
		{
			return ( tool.IsChildOf( m ) || tool.Parent == m );
		}

		public static bool CheckTool( Item tool, Mobile m )
		{
			Item check = m.FindItemOnLayer( Layer.OneHanded );

			if ( check is BaseTool && check != tool && !( check is AncientSmithyHammer ) )
				return false;

			check = m.FindItemOnLayer( Layer.TwoHanded );

			if ( check is BaseTool && check != tool && !( check is AncientSmithyHammer ) )
				return false;

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) || Parent == from )
			{
				CraftSystem system = this.CraftSystem;

				from.SendGump( new CraftGump( from, system, this, null ) );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( (bool) m_Exceptional );
			writer.Write( (Mobile) m_Crafter );

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

			return exceptional;
		}
	}
}