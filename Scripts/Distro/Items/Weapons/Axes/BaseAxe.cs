using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	public abstract class BaseAxe : BaseMeleeWeapon //, IUsesRemaining
	{
		public override int HitSound { get { return 0x232; } }
		public override int MissSound { get { return 0x23A; } }

		public override SkillName Skill { get { return SkillName.Swords; } }
		public override WeaponType Type { get { return WeaponType.Axe; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Slash2H; } }

		public virtual HarvestSystem HarvestSystem { get { return Lumberjacking.System; } }

		private int m_UsesRemaining;
		private bool m_ShowUsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowUsesRemaining
		{
			get { return m_ShowUsesRemaining; }
			set
			{
				m_ShowUsesRemaining = value;
				InvalidateProperties();
			}
		}

		public BaseAxe( int itemID )
			: base( itemID )
		{
			m_UsesRemaining = 150;
		}

		public BaseAxe( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( HarvestSystem == null )
				return;

			if ( IsChildOf( from.Backpack ) || Parent == from )
				HarvestSystem.BeginHarvesting( from, this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( HarvestSystem != null )
				BaseHarvestTool.AddContextMenuEntries( from, this, list, HarvestSystem );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (bool) m_ShowUsesRemaining );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						m_ShowUsesRemaining = reader.ReadBool();
						goto case 1;
					}
				case 1:
					{
						m_UsesRemaining = reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						if ( m_UsesRemaining < 1 )
							m_UsesRemaining = 150;

						break;
					}
			}
		}
	}
}
