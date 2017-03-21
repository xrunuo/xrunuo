using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Spells.Mysticism
{
	public class SpellTriggerSpell : MysticismSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Spell Trigger", "In Vas Ort Ex",
			-1,
			9002,
			Reagent.Garlic,
			Reagent.MandrakeRoot,
			Reagent.SpidersSilk,
			Reagent.DragonsBlood
		);

		public override int RequiredMana
		{
			get { return 14; }
		}

		public override double RequiredSkill
		{
			get { return 45.0; }
		}

		public override TimeSpan CastDelayBase
		{
			get { return TimeSpan.FromSeconds( 5.0 ); }
		}

		public SpellTriggerSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( Caster.HasGump( typeof( SpellTriggerGump ) ) )
				Caster.CloseGump( typeof( SpellTriggerGump ) );

			Caster.SendGump( new SpellTriggerGump( this, Caster ) );
		}

		private class SpellTriggerGump : Gump
		{
			private Spell m_Spell;
			private int m_Skill;

			public SpellTriggerGump( Spell spell, Mobile m )
				: base( 60, 36 )
			{
				m_Spell = spell;

				Closable = true;
				Disposable = false;
				Dragable = false;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 520, 404, 0x13BE );

				AddImageTiled( 10, 10, 500, 20, 0xA40 );
				AddImageTiled( 10, 40, 500, 324, 0xA40 );
				AddImageTiled( 10, 374, 500, 20, 0xA40 );
				AddAlphaRegion( 10, 10, 500, 384 );

				AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

				AddHtmlLocalized( 14, 12, 500, 20, 1080151, 0x7FFF, false, false ); // <center>Spell Trigger Selection Menu</center>

				AddPage( 1 );

				m_Skill = (int) ( GetBaseSkill( m ) + GetBoostSkill( m ) );
				int idx = 0;

				for ( int i = 0; i < m_Definitions.Length; i++ )
				{
					SpellTriggerDef entry = m_Definitions[i];

					if ( m_Skill >= ( entry.Rank * 40 ) )
					{
						idx++;

						if ( idx == 11 )
						{
							AddButton( 400, 374, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );
							AddHtmlLocalized( 440, 376, 60, 20, 1043353, 0x7FFF, false, false ); // Next

							AddPage( 2 );

							AddButton( 300, 374, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );
							AddHtmlLocalized( 340, 376, 60, 20, 1011393, 0x7FFF, false, false ); // Back

							idx = 1;
						}

						if ( ( idx % 2 ) != 0 )
						{
							AddButtonTileArt( 14, 44 + ( 64 * ( idx - 1 ) / 2 ), 0x918, 0x919, GumpButtonType.Reply, 0, 100 + i,
								entry.ItemId, 0, 15, 20 );
							AddTooltip( entry.Tooltip );
							AddHtmlLocalized( 98, 44 + ( 64 * ( idx - 1 ) / 2 ), 170, 60, entry.Cliloc, 0x7FFF, false, false );
						}
						else
						{
							AddButtonTileArt( 264, 44 + ( 64 * ( idx - 2 ) / 2 ), 0x918, 0x919, GumpButtonType.Reply, 0, 100 + i,
								entry.ItemId, 0, 15, 20 );
							AddTooltip( entry.Tooltip );
							AddHtmlLocalized( 348, 44 + ( 64 * ( idx - 2 ) / 2 ), 170, 60, entry.Cliloc, 0x7FFF, false, false );
						}
					}
					else
					{
						break;
					}
				}
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				var from = sender.Mobile;

				if ( from.Backpack != null && info.ButtonID >= 100 && info.ButtonID <= 110 && m_Spell.CheckSequence() )
				{
					foreach ( var stone in from.Backpack.FindItemsByType<SpellStone>() )
						stone.Delete();

					var entry = m_Definitions[info.ButtonID - 100];

					if ( m_Skill >= ( entry.Rank * 40 ) )
					{
						from.PlaySound( 0x659 );
						from.PlaceInBackpack( new SpellStone( entry ) );

						from.SendLocalizedMessage( 1080165 ); // A Spell Stone appears in your backpack
					}
				}

				m_Spell.FinishSequence();
			}
		}

		private static SpellTriggerDef[] m_Definitions =
		{
			new SpellTriggerDef( 677, "Nether Bolt", 1, 1031678, 1095193, 0x2D9E ),
			new SpellTriggerDef( 678, "Healing Stone", 1, 1031679, 1095194, 0x2D9F ),
			new SpellTriggerDef( 679, "Purge Magic", 2, 1031680, 1095195, 0x2DA0 ),
			new SpellTriggerDef( 680, "Enchant", 2, 1031681, 1095196, 0x2DA1 ),
			new SpellTriggerDef( 681, "Sleep", 3, 1031682, 1095197, 0x2DA2 ),
			new SpellTriggerDef( 682, "Eagle Strike", 3, 1031683, 1095198, 0x2DA3 ),
			new SpellTriggerDef( 683, "Animated Weapon", 4, 1031684, 1095199, 0x2DA4 ),
			new SpellTriggerDef( 684, "Stone Form", 4, 1031685, 1095200, 0x2DA5 ),
			new SpellTriggerDef( 686, "Mass Sleep", 5, 1031687, 1095202, 0x2DA7 ),
			new SpellTriggerDef( 687, "Cleansing Winds", 6, 1031688, 1095203, 0x2DA8 ),
			new SpellTriggerDef( 688, "Bombard", 6, 1031689, 1095204, 0x2DA9 )
		};

		public static SpellTriggerDef[] Definitions
		{
			get { return m_Definitions; }
		}
	}

	public class SpellTriggerDef
	{
		public int SpellId { get; }
		public string Name { get; }
		public int Rank { get; }
		public int Cliloc { get; }
		public int Tooltip { get; }
		public int ItemId { get; }

		public SpellTriggerDef( int spellId, string name, int rank, int cliloc, int tooltip, int itemId )
		{
			SpellId = spellId;
			Name = name;
			Rank = rank;
			Cliloc = cliloc;
			Tooltip = tooltip;
			ItemId = itemId;
		}
	}

	public class SpellStone : SpellScroll
	{
		private SpellTriggerDef m_SpellDef;

		[Constructable]
		public SpellStone( SpellTriggerDef spellDef )
			: base( spellDef.SpellId, 0x4079, 1 )
		{
			m_SpellDef = spellDef;

			LootType = LootType.Blessed;
		}

		public override bool NonTransferable
		{
			get { return true; }
		}

		public override void HandleInvalidTransfer( Mobile from )
		{
			Delete();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
		}

		private static Dictionary<Mobile, DateTime> m_CooldownTable = new Dictionary<Mobile, DateTime>();

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_CooldownTable.ContainsKey( from ) )
			{
				var next = m_CooldownTable[from];
				var seconds = (int) ( next - DateTime.Now ).TotalSeconds + 1;

				// You must wait ~1_seconds~ seconds before you can use this item.
				from.SendLocalizedMessage( 1079263, seconds.ToString() );

				return;
			}

			base.OnDoubleClick( from );
		}

		public void Use( Mobile from )
		{
			m_CooldownTable[from] = DateTime.Now + TimeSpan.FromSeconds( 300.0 );
			Timer.DelayCall( TimeSpan.FromSeconds( 300.0 ), () => { m_CooldownTable.Remove( from ); } );

			Delete();
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1080166, m_SpellDef.Name ); // Use: ~1_spellName~
		}

		public SpellStone( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 );

			writer.Write( m_SpellDef.SpellId );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					int spellId = reader.ReadInt();

					for ( int i = 0; i < SpellTriggerSpell.Definitions.Length; i++ )
					{
						SpellTriggerDef def = SpellTriggerSpell.Definitions[i];

						if ( def.SpellId == spellId )
						{
							m_SpellDef = def;
							break;
						}
					}

					if ( m_SpellDef == null )
						Delete();

					break;
				}
				case 0:
				{
					Delete();
					break;
				}
			}
		}
	}
}
