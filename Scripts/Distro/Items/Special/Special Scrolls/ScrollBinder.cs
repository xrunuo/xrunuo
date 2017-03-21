using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public enum ScrollType
	{
		None,
		PowerScroll,
		ScrollOfTranscendence,
		StatsScroll
	}

	public class ScrollBinder : Item
	{
		public override int LabelNumber { get { return 1113135; } } // Scroll Binder

		private ScrollType m_ScrollType;
		private SkillName m_Skill;

		private int m_Value, m_Required, m_Total;

		[CommandProperty( AccessLevel.GameMaster )]
		public ScrollType ScrollType
		{
			get { return m_ScrollType; }
			set
			{
				m_ScrollType = value;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillName Skill
		{
			get { return m_Skill; }
			set
			{
				m_Skill = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Value
		{
			get { return m_Value; }
			set
			{
				m_Value = value;

				if ( m_Value > m_Required )
					m_Value = m_Required;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Required
		{
			get { return m_Required; }
			set
			{
				m_Required = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Total
		{
			get { return m_Total; }
			set
			{
				m_Total = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public ScrollBinder()
			: base( 0x14EF )
		{
			Hue = 0x664;
			Weight = 1.0;

			LootType = LootType.Cursed;
		}

		public ScrollBinder( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( OnTarget ) );

			if ( m_ScrollType == ScrollType.None )
				from.SendLocalizedMessage( 1113141 ); // Target the scroll you wish to bind.
			else
				from.SendLocalizedMessage( 1113137 + (int) m_ScrollType );
		}

		private bool VerifyScroll( Mobile from, Item scroll )
		{
			PowerScroll ps = scroll as PowerScroll;
			StatCapScroll ss = scroll as StatCapScroll;
			ScrollOfTranscendence sot = scroll as ScrollOfTranscendence;

			if ( ps == null && ss == null && sot == null )
			{
				// You may only bind powerscrolls, stats scrolls or scrolls of transcendence.
				from.SendLocalizedMessage( 1113142 );

				return false;
			}
			else if ( ( ps != null && ps.Value >= 120.0 ) || ( ss != null && ss.Value >= 250 ) || ( sot != null && sot.Value >= 5.0 ) )
			{
				// This scroll is already the highest of its type and cannot be bound.
				from.SendLocalizedMessage( 1113144 );

				return false;
			}

			return true;
		}

		private void Initialize( Mobile from, Item item )
		{
			if ( m_ScrollType != ScrollType.None )
				return; // sanity, should never happen

			if ( item is PowerScroll )
			{
				PowerScroll ps = item as PowerScroll;

				m_ScrollType = ScrollType.PowerScroll;
				m_Skill = ps.Skill;

				switch ( (int) ps.Value )
				{
					default:
					case 105: m_Value = 5; m_Required = 8; break;
					case 110: m_Value = 10; m_Required = 12; break;
					case 115: m_Value = 15; m_Required = 10; break;
				}

				OnTargetPowerScroll( from, item );
			}
			else if ( item is StatCapScroll )
			{
				StatCapScroll ss = item as StatCapScroll;

				m_ScrollType = ScrollType.StatsScroll;

				switch ( ss.Value )
				{
					case 230: m_Value = 5; m_Required = 6; break;
					case 235: m_Value = 10; m_Required = 8; break;
					case 240: m_Value = 15; m_Required = 8; break;
					case 245: m_Value = 20; m_Required = 5; break;
				}

				OnTargetStatsScroll( from, item );
			}
			else if ( item is ScrollOfTranscendence )
			{
				ScrollOfTranscendence sot = item as ScrollOfTranscendence;

				m_ScrollType = ScrollType.ScrollOfTranscendence;
				m_Skill = sot.Skill;
				m_Required = 20;

				OnTargetSoT( from, item );
			}
		}

		public void CheckCompleted( Mobile from )
		{
			if ( m_ScrollType == ScrollType.ScrollOfTranscendence )
			{
				if ( Total > 20 && Required == 20 )
					Required = 50;
			}

			if ( m_Total >= m_Required )
			{
				Item upgrade = Upgrade();

				if ( upgrade != null )
				{
					if ( Parent != null && Parent is Container )
					{
						Container cont = Parent as Container;

						cont.DropItem( upgrade );
						upgrade.Location = Location;
					}
					else
						upgrade.MoveToWorld( Location );

					Delete();

					// You've completed your binding and received an upgraded version of your scroll!
					from.SendLocalizedMessage( 1113145 );
				}
			}
		}

		private Item Upgrade()
		{
			switch ( m_ScrollType )
			{
				default: return null;
				case ScrollType.PowerScroll: return new PowerScroll( m_Skill, m_Value + 105.0 );
				case ScrollType.StatsScroll: return new StatCapScroll( m_Value + 230 );
				case ScrollType.ScrollOfTranscendence: return new ScrollOfTranscendence( m_Skill, m_Required / 10.0 );
			}
		}

		private void OnTarget( Mobile from, object o )
		{
			Item item = o as Item;

			if ( !IsChildOf( from.Backpack ) || item == null || !item.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( item is ScrollBinder )
			{
				if ( m_ScrollType == ScrollType.None )
					from.SendLocalizedMessage( 1113143 ); // This scroll does not match the type currently being bound.
				else
					OnTargetScrollBinder( from, item as ScrollBinder );
			}
			else
			{
				switch ( m_ScrollType )
				{
					default:
					case ScrollType.None: OnTargetGeneric( from, item ); break;
					case ScrollType.PowerScroll: OnTargetPowerScroll( from, item ); break;
					case ScrollType.ScrollOfTranscendence: OnTargetSoT( from, item ); break;
					case ScrollType.StatsScroll: OnTargetStatsScroll( from, item ); break;
				}
			}
		}

		private void OnTargetGeneric( Mobile from, Item item )
		{
			if ( VerifyScroll( from, item ) )
				Initialize( from, item );
		}

		private void OnTargetPowerScroll( Mobile from, Item item )
		{
			PowerScroll ps = item as PowerScroll;

			if ( ps == null || m_Skill != ps.Skill || (int) ( ps.Value - 100.0 ) != m_Value )
			{
				// This scroll does not match the type currently being bound.
				from.SendLocalizedMessage( 1113143 );
			}
			else
			{
				item.Delete();
				Total++;

				CheckCompleted( from );
			}
		}

		private void OnTargetSoT( Mobile from, Item item )
		{
			ScrollOfTranscendence sot = item as ScrollOfTranscendence;

			if ( sot == null || sot.Skill != m_Skill )
			{
				// This scroll does not match the type currently being bound.
				from.SendLocalizedMessage( 1113143 );
			}
			else if ( ( sot.Value * 10 ) + m_Total > 50 )
			{
				from.CloseGump( typeof( BindSotWarningGump ) );
				from.SendGump( new BindSotWarningGump( this, sot ) );
			}
			else
			{
				item.Delete();
				Total += (int) ( sot.Value * 10 );

				CheckCompleted( from );
			}
		}

		private void OnTargetStatsScroll( Mobile from, Item item )
		{
			StatCapScroll ss = item as StatCapScroll;

			if ( ss == null || ( ss.Value - 225 ) != m_Value )
			{
				// This scroll does not match the type currently being bound.
				from.SendLocalizedMessage( 1113143 );
			}
			else
			{
				item.Delete();
				Total++;

				CheckCompleted( from );
			}
		}

		private void OnTargetScrollBinder( Mobile from, ScrollBinder scroll )
		{
			if ( scroll == this )
				return;

			if ( m_ScrollType != scroll.ScrollType || m_Skill != scroll.Skill || m_Value != scroll.Value )
			{
				// This scroll does not match the type currently being bound.
				from.SendLocalizedMessage( 1113143 );
			}
			else
			{
				Total += scroll.Total;

				CheckCompleted( from );

				int rest = m_Total - m_Required;

				if ( rest > 0 )
					scroll.Total = rest;
				else
					scroll.Delete();
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			switch ( m_ScrollType )
			{
				case ScrollType.PowerScroll:
					{
						string skillName = SkillInfo.Table[(int) m_Skill].Name;

						// ~1_bonus~ ~2_type~: ~3_given~/~4_needed~
						list.Add( 1113149, String.Format( "{0}\t{1}\t{2}\t{3}", m_Value + 100, skillName, m_Total, m_Required ) );

						break;
					}
				case ScrollType.StatsScroll:
					{
						// ~1_bonus~ ~2_type~: ~3_given~/~4_needed~
						list.Add( 1113149, String.Format( "+{0}\tStats\t{1}\t{2}", m_Value, m_Total, m_Required ) );

						break;
					}
				case ScrollType.ScrollOfTranscendence:
					{
						double val = m_Total / 10.0;
						string skillName = SkillInfo.Table[(int) m_Skill].Name;

						int number;

						if ( m_Required == 20 )
							number = 1113148; // ~1_type~ transcendence: ~2_given~/2.0
						else
							number = 1113620; // ~1_type~ transcendence: ~2_given~/5.0

						list.Add( number, String.Format( "{0}\t{1}", skillName, val.ToString() ) );

						break;
					}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_ScrollType );
			writer.Write( (int) m_Skill );

			writer.Write( m_Value );
			writer.Write( m_Required );
			writer.Write( m_Total );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version > 0 )
			{
				m_ScrollType = (ScrollType) reader.ReadInt();
				m_Skill = (SkillName) reader.ReadInt();

				m_Value = reader.ReadInt();
				m_Required = reader.ReadInt();
				m_Total = reader.ReadInt();
			}
		}

		public class BindSotWarningGump : Gump
		{
			public override int TypeID { get { return 0x236C; } }

			private ScrollBinder m_Binder;
			private ScrollOfTranscendence m_ToBound;

			public BindSotWarningGump( ScrollBinder binder, ScrollOfTranscendence toBound )
				: base( 340, 340 )
			{
				m_Binder = binder;
				m_ToBound = toBound;

				AddPage( 0 );

				AddBackground( 0, 0, 291, 99, 0x13BE );
				AddImageTiled( 5, 6, 280, 20, 0xA40 );

				AddHtmlLocalized( 9, 8, 280, 20, 1113146, 0x7FFF, false, false ); // Binding Scrolls of Transcendence
				AddImageTiled( 5, 31, 280, 40, 0xA40 );

				AddHtmlLocalized( 9, 35, 272, 40, 1113147, 0x7FFF, false, false ); // Binding this SoT will exceed the cap of 5, some points will be lost. Proceed?

				AddButton( 215, 73, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 250, 75, 65, 20, 1006044, 0x7FFF, false, false ); // OK

				AddButton( 5, 73, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 75, 100, 20, 1060051, 0x7FFF, false, false ); // CANCEL
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( info.ButtonID == 1 && !m_Binder.Deleted && !m_ToBound.Deleted && m_ToBound.IsChildOf( sender.Mobile.Backpack ) )
				{
					m_Binder.Total += (int) ( m_ToBound.Value * 10 );
					m_ToBound.Delete();

					m_Binder.CheckCompleted( sender.Mobile );
				}
			}
		}
	}
}