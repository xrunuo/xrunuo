using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public interface IUsesRemaining
	{
		int UsesRemaining { get; set; }
		bool ShowUsesRemaining { get; set; }
	}

	public abstract class BaseHarvestTool : Item, IUsesRemaining, ICraftable
	{
		private Mobile m_Crafter;
		private bool m_Exceptional;
		private int m_UsesRemaining;

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
				return 200;

			return 100;
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		public abstract HarvestSystem HarvestSystem { get; }

		public BaseHarvestTool( int itemID )
			: this( 50, itemID )
		{
		}

		public BaseHarvestTool( int usesRemaining, int itemID )
			: base( itemID )
		{
			m_UsesRemaining = usesRemaining;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Exceptional )
				list.Add( 1060636 ); // exceptional

			// As per OSI, intentionally do not show maker's mark

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) || Parent == from )
				HarvestSystem.BeginHarvesting( from, this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			AddContextMenuEntries( from, this, list, HarvestSystem );
		}

		public static void AddContextMenuEntries( Mobile from, Item item, List<ContextMenuEntry> list, HarvestSystem system )
		{
			if ( system != Mining.System )
				return;

			if ( !item.IsChildOf( from.Backpack ) && item.Parent != from )
				return;

			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			ContextMenuEntry miningEntry = new ContextMenuEntry( pm.ToggleMiningStone ? 6179 : 6178 );
			miningEntry.Color = 0x421F;
			list.Add( miningEntry );

			list.Add( new ToggleMiningStoneEntry( pm, false, 6176 ) );
			list.Add( new ToggleMiningStoneEntry( pm, true, 6177 ) );
		}

		public class ToggleMiningStoneEntry : ContextMenuEntry
		{
			private PlayerMobile m_Mobile;
			private bool m_Value;

			public ToggleMiningStoneEntry( PlayerMobile mobile, bool value, int number )
				: base( number )
			{
				m_Mobile = mobile;
				m_Value = value;

				bool stoneMining = ( mobile.StoneMining && mobile.Skills[SkillName.Mining].Base >= 100.0 );

				if ( mobile.ToggleMiningStone == value || ( value && !stoneMining ) )
					this.Flags |= CMEFlags.Disabled;
			}

			public override void OnClick()
			{
				bool oldValue = m_Mobile.ToggleMiningStone;

				if ( m_Value )
				{
					if ( oldValue )
					{
						m_Mobile.SendLocalizedMessage( 1054023 ); // You are already set to mine both ore and stone!
					}
					else if ( !m_Mobile.StoneMining || m_Mobile.Skills[SkillName.Mining].Base < 100.0 )
					{
						m_Mobile.SendLocalizedMessage( 1054024 ); // You have not learned how to mine stone or you do not have enough skill!
					}
					else
					{
						m_Mobile.ToggleMiningStone = true;
						m_Mobile.SendLocalizedMessage( 1054022 ); // You are now set to mine both ore and stone.
					}
				}
				else
				{
					if ( oldValue )
					{
						m_Mobile.ToggleMiningStone = false;
						m_Mobile.SendLocalizedMessage( 1054020 ); // You are now set to mine only ore.
					}
					else
					{
						m_Mobile.SendLocalizedMessage( 1054021 ); // You are already set to mine only ore!
					}
				}
			}
		}

		public BaseHarvestTool( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (Mobile) m_Crafter );
			writer.Write( (bool) m_Exceptional );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Crafter = reader.ReadMobile();
						m_Exceptional = reader.ReadBool();
						goto case 0;
					}
				case 0:
					{
						m_UsesRemaining = reader.ReadInt();
						break;
					}
			}
		}

		#region ICraftable Members

		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			return exceptional;
		}

		#endregion
	}
}