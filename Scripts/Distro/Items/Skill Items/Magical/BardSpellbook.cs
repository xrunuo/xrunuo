using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Bard;

namespace Server.Items
{
	public class BardSpellbook : Spellbook
	{
		public override SpellbookType SpellbookType { get { return SpellbookType.Bard; } }

		public override int BookOffset { get { return 700; } }
		public override int BookCount { get { return 6; } }

		[Constructable]
		public BardSpellbook()
			: this( (ulong) 0x3F )
		{
		}

		[Constructable]
		public BardSpellbook( ulong content )
			: base( content, 0x225A )
		{
			Layer = Layer.OneHanded;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			PlayerMobile pm = RootParent as PlayerMobile;

			if ( pm != null && pm.BardMastery != null )
			{
				list.Add( pm.BardMastery.Cliloc );

				if ( pm.BardMastery == BardMastery.Discordance )
					list.Add( GetElementalDamageCliloc( pm.BardElementDamage ) );
			}
		}

		private static int GetElementalDamageCliloc( ResistanceType element )
		{
			return 1151800 + (int) element;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( !IsChildOf( from.Backpack ) )
				return;

			PlayerMobile pm = from as PlayerMobile;

			if ( pm.BardMastery == BardMastery.Discordance )
			{
				foreach ( var element in ElementalDamages )
				{
					if ( element != pm.BardElementDamage )
						list.Add( new SwitchDamageEntry( pm, this, element ) );
				}
			}

			if ( DateTime.UtcNow > pm.NextBardMasterySwitch )
			{
				ContextMenuEntry titleEntry = new ContextMenuEntry( 1151948 ); // Switch Mastery
				titleEntry.Enabled = false;
				list.Add( titleEntry );

				foreach ( var mastery in BardMastery.AllMasteries )
				{
					if ( mastery != pm.BardMastery )
						list.Add( new SwitchMasteryEntry( pm, this, mastery ) );
				}
			}
		}

		private class SwitchMasteryEntry : ContextMenuEntry
		{
			private PlayerMobile m_Owner;
			private BardSpellbook m_Book;
			private BardMastery m_Mastery;

			public SwitchMasteryEntry( PlayerMobile owner, BardSpellbook book, BardMastery mastery )
				: base( mastery.Cliloc )
			{
				m_Owner = owner;
				m_Book = book;
				m_Mastery = mastery;
			}

			public override void OnClick()
			{
				if ( !CanSwitch( m_Owner, m_Mastery ) )
				{
					// You do not have the required skill levels to switch to this mastery.
					m_Owner.SendLocalizedMessage( 1151950 );

					return;
				}

				m_Owner.NextBardMasterySwitch = DateTime.UtcNow + TimeSpan.FromMinutes( 10.0 );
				m_Owner.BardMastery = m_Mastery;

				// You have changed to ~1_val~
				m_Owner.SendLocalizedMessage( 1151949, string.Format( "#{0}", m_Mastery.Cliloc ) );

				m_Book.InvalidateProperties();
			}

			private static bool CanSwitch( PlayerMobile pm, BardMastery mastery )
			{
				if ( !pm.HasLearnedBardMastery( mastery ) )
					return false;

				if ( pm.Skills[mastery.Skill].Value < 90.0 )
					return false;

				return true;
			}
		}

		private static readonly ResistanceType[] ElementalDamages = new ResistanceType[]
			{
				ResistanceType.Physical,
				ResistanceType.Fire,
				ResistanceType.Cold,
				ResistanceType.Poison,
				ResistanceType.Energy,
			};

		private class SwitchDamageEntry : ContextMenuEntry
		{
			private PlayerMobile m_Owner;
			private BardSpellbook m_Book;
			private ResistanceType m_Element;

			public SwitchDamageEntry( PlayerMobile owner, BardSpellbook book, ResistanceType element )
				: base( GetElementalDamageCliloc( element ) )
			{
				m_Owner = owner;
				m_Book = book;
				m_Element = element;
			}

			public override void OnClick()
			{
				m_Owner.BardElementDamage = m_Element;
				m_Book.InvalidateProperties();
			}
		}

		public BardSpellbook( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
