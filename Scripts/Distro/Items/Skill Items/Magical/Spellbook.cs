using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.Craft;
using Server.Engines.Housing.Multis;
using Server.Engines.Imbuing;
using Server.Network;
using Server.Scripts.Commands;
using Server.Spells;
using Server.Targeting;
using Server.Events;

namespace Server.Items
{
	public enum SpellbookType
	{
		Invalid = -1,
		Regular,
		Necromancer,
		Paladin,
		Ninja,
		Samurai,
		Arcanist,
		Mystic,
		Bard
	}

	public class Spellbook : Item, ICraftable, ISlayer, IMagicalItem, ISkillBonuses, IImbuable
	{
		public static void Initialize()
		{
			EventSink.Instance.OpenSpellbookRequest += new OpenSpellbookRequestEventHandler( EventSink_OpenSpellbookRequest );
			EventSink.Instance.CastSpellRequest += new CastSpellRequestEventHandler( EventSink_CastSpellRequest );

			CommandSystem.Register( "AllSpells", AccessLevel.GameMaster, new CommandEventHandler( AllSpells_OnCommand ) );
		}

		[Usage( "AllSpells" )]
		[Description( "Completely fills a targeted spellbook with scrolls." )]
		private static void AllSpells_OnCommand( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( AllSpells_OnTarget ) );
			e.Mobile.SendMessage( "Target the spellbook to fill." );
		}

		private static void AllSpells_OnTarget( Mobile from, object obj )
		{
			if ( obj is Spellbook )
			{
				Spellbook book = (Spellbook) obj;

				if ( book.BookCount == 64 )
					book.Content = ulong.MaxValue;
				else
					book.Content = ( 1ul << book.BookCount ) - 1;

				from.SendMessage( "The spellbook has been filled." );

				CommandLogging.WriteLine( from, "{0} {1} filling spellbook {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( book ) );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( AllSpells_OnTarget ) );
				from.SendMessage( "That is not a spellbook. Try again." );
			}
		}

		private static void EventSink_OpenSpellbookRequest( OpenSpellbookRequestEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !DesignContext.Check( from ) )
				return; // They are customizing

			SpellbookType type;

			switch ( e.Type )
			{
				default:
				case 1: type = SpellbookType.Regular; break;
				case 2: type = SpellbookType.Necromancer; break;
				case 3: type = SpellbookType.Paladin; break;
				case 4: type = SpellbookType.Ninja; break;
				case 5: type = SpellbookType.Samurai; break;
				case 6: type = SpellbookType.Arcanist; break;
				case 7: type = SpellbookType.Mystic; break;
			}

			Spellbook book = Spellbook.Find( from, -1, type );

			if ( book != null )
				book.DisplayTo( from );
		}

		private static void EventSink_CastSpellRequest( CastSpellRequestEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !DesignContext.Check( from ) )
				return; // They are customizing

			Spellbook book = e.Spellbook as Spellbook;
			int spellID = e.SpellID;

			if ( book == null || !book.HasSpell( spellID ) )
				book = Find( from, spellID );

			if ( book != null && book.HasSpell( spellID ) )
			{
				SpecialMove move = SpellRegistry.GetSpecialMove( spellID );

				if ( move != null )
				{
					SpecialMove.SetCurrentMove( from, move );
				}
				else
				{
					Spell spell = SpellRegistry.NewSpell( spellID, from, null );

					if ( spell != null )
					{
						spell.MacroTarget = e.Target;
						spell.Cast();
					}
					else
						from.SendLocalizedMessage( 502345 ); // This spell has been temporarily disabled.			
				}
			}
			else
			{
				from.SendLocalizedMessage( 500015 ); // You do not have that spell!
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public static SpellbookType GetTypeForSpell( int spellID )
		{
			if ( spellID >= 0 && spellID < 64 )
				return SpellbookType.Regular;
			else if ( spellID >= 100 && spellID < 117 )
				return SpellbookType.Necromancer;
			else if ( spellID >= 200 && spellID < 210 )
				return SpellbookType.Paladin;
			else if ( spellID >= 400 && spellID < 406 )
				return SpellbookType.Samurai;
			else if ( spellID >= 500 && spellID < 508 )
				return SpellbookType.Ninja;
			else if ( spellID >= 600 && spellID < 616 )
				return SpellbookType.Arcanist;
			else if ( spellID >= 677 && spellID < 693 )
				return SpellbookType.Mystic;
			else if ( spellID >= 700 && spellID < 706 )
				return SpellbookType.Bard;

			return SpellbookType.Invalid;
		}

		public static Spellbook FindRegular( Mobile from )
		{
			return Find( from, -1, SpellbookType.Regular );
		}

		public static Spellbook FindNecromancer( Mobile from )
		{
			return Find( from, -1, SpellbookType.Necromancer );
		}

		public static Spellbook FindPaladin( Mobile from )
		{
			return Find( from, -1, SpellbookType.Paladin );
		}

		public static Spellbook FindSamurai( Mobile from )
		{
			return Find( from, -1, SpellbookType.Samurai );
		}

		public static Spellbook FindNinja( Mobile from )
		{
			return Find( from, -1, SpellbookType.Ninja );
		}

		public static Spellbook FindArcanist( Mobile from )
		{
			return Find( from, -1, SpellbookType.Arcanist );
		}

		public static Spellbook FindMystic( Mobile from )
		{
			return Find( from, -1, SpellbookType.Mystic );
		}

		public static Spellbook FindBard( Mobile from )
		{
			return Find( from, -1, SpellbookType.Bard );
		}

		public static Spellbook Find( Mobile from, int spellID )
		{
			return Find( from, spellID, GetTypeForSpell( spellID ) );
		}

		public static Spellbook Find( Mobile from, int spellID, SpellbookType type )
		{
			if ( from == null )
				return null;

			ArrayList list = (ArrayList) m_Table[from];

			if ( from.Deleted )
			{
				m_Table.Remove( from );
				return null;
			}

			bool searchAgain = false;

			if ( list == null )
				m_Table[from] = list = FindAllSpellbooks( from );
			else
				searchAgain = true;

			Spellbook book = FindSpellbookInList( list, from, spellID, type );

			if ( book == null && searchAgain )
			{
				m_Table[from] = list = FindAllSpellbooks( from );

				book = FindSpellbookInList( list, from, spellID, type );
			}

			return book;
		}

		public static Spellbook FindSpellbookInList( ArrayList list, Mobile from, int spellID, SpellbookType type )
		{
			Container pack = from.Backpack;

			for ( int i = list.Count - 1; i >= 0; --i )
			{
				if ( i >= list.Count )
					continue;

				Spellbook book = (Spellbook) list[i];

				if ( !book.Deleted && ( book.Parent == from || ( pack != null && book.Parent == pack ) ) && ValidateSpellbook( book, spellID, type ) )
					return book;

				list.Remove( i );
			}

			return null;
		}

		public static ArrayList FindAllSpellbooks( Mobile from )
		{
			ArrayList list = new ArrayList();

			Item item = from.FindItemOnLayer( Layer.OneHanded );

			if ( item is Spellbook )
				list.Add( item );

			Container pack = from.Backpack;

			if ( pack == null )
				return list;

			for ( int i = 0; i < pack.Items.Count; ++i )
			{
				item = (Item) pack.Items[i];

				if ( item is Spellbook )
					list.Add( item );
			}

			return list;
		}

		public static Spellbook FindEquippedSpellbook( Mobile from )
		{
			return ( from.FindItemOnLayer( Layer.OneHanded ) as Spellbook );
		}

		public static bool ValidateSpellbook( Spellbook book, int spellID, SpellbookType type )
		{
			return ( book.SpellbookType == type && ( spellID == -1 || book.HasSpell( spellID ) ) );
		}

		private MagicalAttributes m_MagicalAttributes;
		private SkillBonuses m_SkillBonuses;

		private string m_EngraveString;

		[CommandProperty( AccessLevel.GameMaster )]
		public MagicalAttributes Attributes
		{
			get { return m_MagicalAttributes; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillBonuses SkillBonuses
		{
			get { return m_SkillBonuses; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string EngraveString
		{
			get { return m_EngraveString; }
			set { m_EngraveString = value; InvalidateProperties(); }
		}

		public virtual SpellbookType SpellbookType { get { return SpellbookType.Regular; } }
		public virtual int BookOffset { get { return 0; } }
		public virtual int BookCount { get { return 64; } }

		private ulong m_Content;
		private int m_Count;

		public override bool WearableByGargoyles { get { return true; } }

		public override bool CanInsure { get { return true; } }

		public override void OnAfterDuped( Item newItem )
		{
			// TODO: Copy Attributes
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			return true;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is SpellScroll && dropped.Amount == 1 && !( dropped is Spells.Mysticism.SpellStone ) )
			{
				SpellScroll scroll = (SpellScroll) dropped;

				SpellbookType type = GetTypeForSpell( scroll.SpellID );

				if ( type != this.SpellbookType )
				{
					return false;
				}
				else if ( HasSpell( scroll.SpellID ) )
				{
					from.SendLocalizedMessage( 500179 ); // That spell is already present in that spellbook.
					return false;
				}
				else
				{
					int val = scroll.SpellID - BookOffset;

					if ( val >= 0 && val < BookCount )
					{
						m_Content |= (ulong) 1 << val;
						++m_Count;

						InvalidateProperties();

						scroll.Delete();

						from.Send( GenericPackets.PlaySound( 0x249, GetWorldLocation() ) );
						return true;
					}

					return false;
				}
			}
			else
			{
				return false;
			}
		}

		private Mobile m_Crafter;
		private bool m_Exceptional;

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

		[CommandProperty( AccessLevel.GameMaster )]
		public ulong Content
		{
			get
			{
				return m_Content;
			}
			set
			{
				if ( m_Content != value )
				{
					m_Content = value;

					m_Count = 0;

					while ( value > 0 )
					{
						m_Count += (int) ( value & 0x1 );
						value >>= 1;
					}

					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpellCount
		{
			get
			{
				return m_Count;
			}
		}

		[Constructable]
		public Spellbook()
			: this( (ulong) 0 )
		{
		}

		[Constructable]
		public Spellbook( ulong content )
			: this( content, 0xEFA )
		{
		}

		public Spellbook( ulong content, int itemID )
			: base( itemID )
		{
			m_MagicalAttributes = new MagicalAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );

			Weight = 0.0;
			Layer = Layer.OneHanded;
			LootType = LootType.Blessed;

			Content = content;
		}

		public override bool CanEquip( Mobile from )
		{
			if ( !from.CanBeginAction( typeof( BaseWeapon ) ) )
				return false;
			else
				return base.CanEquip( from );
		}

		public override void OnAdded( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				m_SkillBonuses.AddTo( from );

				int strBonus = m_MagicalAttributes.BonusStr;
				int dexBonus = m_MagicalAttributes.BonusDex;
				int intBonus = m_MagicalAttributes.BonusInt;

				if ( strBonus != 0 || dexBonus != 0 || intBonus != 0 )
				{
					string modName = this.Serial.ToString();

					if ( strBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

					if ( dexBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

					if ( intBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
				}

				from.CheckStatTimers();

				from.Delta( MobileDelta.Armor );
			}
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				m_SkillBonuses.Remove();

				string modName = this.Serial.ToString();

				from.RemoveStatMod( modName + "Str" );
				from.RemoveStatMod( modName + "Dex" );
				from.RemoveStatMod( modName + "Int" );

				from.CheckStatTimers();

				from.Delta( MobileDelta.Armor );
			}
		}

		public bool HasSpell( int spellID )
		{
			spellID -= BookOffset;

			return ( spellID >= 0 && spellID < BookCount && ( m_Content & ( (ulong) 1 << spellID ) ) != 0 );
		}

		public Spellbook( Serial serial )
			: base( serial )
		{
		}

		public void DisplayTo( Mobile to )
		{
			// The client must know about the spellbook or it will crash!

			if ( Parent == null )
			{
				to.Send( this.WorldPacket );
			}
			else if ( Parent is Item )
			{
				to.Send( new ContainerContentUpdate( this ) );
			}
			else if ( Parent is Mobile )
			{
				// What will happen if the client doesn't know about our parent?
				to.Send( new EquipUpdate( this ) );
			}

			to.Send( new DisplaySpellbook( this ) );
			to.Send( new SpellbookContent( this, ItemID, BookOffset + 1, m_Content ) );
		}

		public override bool DisplayLootType { get { return true; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Exceptional )
				list.Add( 1063341 ); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_EngraveString != null && m_EngraveString != "" )
				list.Add( 1072305, m_EngraveString ); // Engraved: ~1_INSCRIPTION~

			m_SkillBonuses.GetProperties( list );

			if ( m_Slayer != SlayerName.None )
			{
				SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer );
				if ( entry != null )
					list.Add( entry.Title );
			}

			if ( m_Slayer2 != SlayerName.None )
			{
				SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer2 );
				if ( entry != null )
					list.Add( entry.Title );
			}

			int prop;

			if ( ( prop = m_MagicalAttributes.WeaponDamage ) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( ( prop = m_MagicalAttributes.DefendChance ) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( ( prop = m_MagicalAttributes.BonusDex ) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( ( prop = m_MagicalAttributes.EnhancePotions ) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( ( prop = m_MagicalAttributes.CastRecovery ) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( ( prop = m_MagicalAttributes.CastSpeed ) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( ( prop = m_MagicalAttributes.AttackChance ) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( ( prop = m_MagicalAttributes.BonusHits ) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( ( prop = m_MagicalAttributes.BonusInt ) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( ( prop = m_MagicalAttributes.LowerManaCost ) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( ( prop = m_MagicalAttributes.LowerRegCost ) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( ( prop = m_MagicalAttributes.Luck ) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( ( prop = m_MagicalAttributes.BonusMana ) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( ( prop = m_MagicalAttributes.RegenMana ) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( ( prop = m_MagicalAttributes.NightSight ) != 0 )
				list.Add( 1060441 ); // night sight

			if ( ( prop = m_MagicalAttributes.ReflectPhysical ) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( ( prop = m_MagicalAttributes.RegenStam ) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( ( prop = m_MagicalAttributes.RegenHits ) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( ( prop = m_MagicalAttributes.SpellChanneling ) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( ( prop = m_MagicalAttributes.SpellDamage ) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( ( prop = m_MagicalAttributes.BonusStam ) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( ( prop = m_MagicalAttributes.BonusStr ) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( ( prop = m_MagicalAttributes.WeaponSpeed ) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

			if ( ( prop = m_MagicalAttributes.IncreasedKarmaLoss ) != 0 )
				list.Add( 1075210, prop.ToString() ); // increased karma loss ~1_val~%

			if ( ( prop = m_MagicalAttributes.CastingFocus ) != 0 )
				list.Add( 1113696, prop.ToString() ); // Casting Focus ~1_val~%

			list.Add( 1042886, m_Count.ToString() ); // ~1_NUMBERS_OF_SPELLS~ Spells
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			if ( Parent == from || ( pack != null && Parent == pack ) )
				DisplayTo( from );
			else
				from.SendLocalizedMessage( 500207 ); // The spellbook must be in your backpack (and not in a container within) to open.
		}

		private SlayerName m_Slayer;
		private SlayerName m_Slayer2;
		// Currently though there are no dual slayer spellbooks, OSI has a habit of putting dual slayer stuff in later

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer
		{
			get { return m_Slayer; }
			set { m_Slayer = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer2
		{
			get { return m_Slayer2; }
			set { m_Slayer2 = value; InvalidateProperties(); }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 7 ); // version

			writer.Write( (bool) m_Exceptional );
			writer.Write( (Mobile) m_Crafter );

			writer.WriteEncodedInt( (int) m_Slayer2 );

			writer.Write( (string) m_EngraveString );

			writer.WriteEncodedInt( (int) m_Slayer );

			m_MagicalAttributes.Serialize( writer );
			m_SkillBonuses.Serialize( writer );

			writer.Write( m_Content );
			writer.Write( m_Count );
		}

		public void FixAttributes()
		{
			if ( m_MagicalAttributes.LowerRegCost > 16 )
				m_MagicalAttributes.LowerRegCost = 16;

			if ( m_MagicalAttributes.LowerManaCost > 6 )
				m_MagicalAttributes.LowerManaCost = 6;

			if ( m_MagicalAttributes.RegenMana > 1 )
				m_MagicalAttributes.RegenMana = 1;

			if ( m_MagicalAttributes.BonusInt > 6 )
				m_MagicalAttributes.BonusInt = 6;

			if ( m_MagicalAttributes.BonusMana > 6 )
				m_MagicalAttributes.BonusMana = 6;

			if ( m_MagicalAttributes.CastSpeed > 1 )
				m_MagicalAttributes.CastSpeed = 1;

			if ( m_MagicalAttributes.CastRecovery > 2 )
				m_MagicalAttributes.CastRecovery = 2;

			if ( m_MagicalAttributes.SpellDamage > 9 )
				m_MagicalAttributes.SpellDamage = 9;

			if ( m_SkillBonuses.Skill_1_Value > 12 )
				m_SkillBonuses.Skill_1_Value = 12;

			if ( m_SkillBonuses.Skill_2_Value > 12 )
				m_SkillBonuses.Skill_2_Value = 12;

			if ( m_SkillBonuses.Skill_3_Value > 12 )
				m_SkillBonuses.Skill_3_Value = 12;

			if ( m_SkillBonuses.Skill_4_Value > 12 )
				m_SkillBonuses.Skill_4_Value = 12;

			if ( m_SkillBonuses.Skill_5_Value > 12 )
				m_SkillBonuses.Skill_5_Value = 12;
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( !( this is TomeOfEnlightenment || this is TomeOfLostKnowledge || this is FactionTomeOfLostKnowledge ) )
				LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 7:
					{
						m_Exceptional = reader.ReadBool();
						m_Crafter = reader.ReadMobile();

						goto case 6;
					}
				case 6:
				case 5:
					{
						m_Slayer2 = (SlayerName) reader.ReadEncodedInt();
						goto case 4;
					}
				case 4:
					{
						m_EngraveString = reader.ReadString();
						goto case 3;
					}
				case 3:
				case 2:
					{
						m_Slayer = (SlayerName) reader.ReadEncodedInt();

						goto case 1;
					}
				case 1:
					{
						m_MagicalAttributes = new MagicalAttributes( this, reader );
						m_SkillBonuses = new SkillBonuses( this, reader );

						if ( this is ScrappersCompendium )
						{
							Attributes.SpellDamage = 25;
							Attributes.LowerManaCost = 10;
						}
						else if ( this is ClaininsSpellbook )
						{
							Attributes.LowerRegCost = 15;
							Attributes.RegenMana = 3;
						}
						else if ( this is TomeOfEnlightenment )
						{
							Attributes.BonusInt = 5;
							Attributes.SpellDamage = 10;
							Attributes.CastSpeed = 1;
						}
						else if ( this is TomeOfLostKnowledge )
						{
							SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 );
							Attributes.BonusInt = 8;
							Attributes.SpellDamage = 15;
							Attributes.LowerManaCost = 15;
						}
						else if ( this is FactionTomeOfLostKnowledge )
						{
							SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 );
							Attributes.BonusInt = 8;
							Attributes.SpellDamage = 15;
							Attributes.LowerManaCost = 15;
							Attributes.RegenMana = 3;
						}
						else if ( this is ConjurersGrimoire )
						{
							SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 );
							Attributes.BonusInt = 8;
							Attributes.SpellDamage = 15;
							Attributes.LowerManaCost = 10;
						}
						else if ( this is FallenMysticsSpellbook )
						{
							Attributes.SpellDamage = 10;
						}
						else
						{
							FixAttributes();
						}

						goto case 0;
					}
				case 0:
					{
						m_Content = reader.ReadULong();
						m_Count = reader.ReadInt();

						break;
					}
			}

			if ( m_MagicalAttributes == null )
				m_MagicalAttributes = new MagicalAttributes( this );

			if ( m_SkillBonuses == null )
				m_SkillBonuses = new SkillBonuses( this );

			if ( Parent is Mobile )
				m_SkillBonuses.AddTo( (Mobile) Parent );

			int strBonus = m_MagicalAttributes.BonusStr;
			int dexBonus = m_MagicalAttributes.BonusDex;
			int intBonus = m_MagicalAttributes.BonusInt;

			if ( Parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 ) )
			{
				Mobile m = (Mobile) Parent;

				string modName = Serial.ToString();

				if ( strBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

				if ( dexBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

				if ( intBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}

			if ( Parent is Mobile )
				( (Mobile) Parent ).CheckStatTimers();
		}

		private static int[] m_GrandPropertyCounts = new int[]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// 0 properties : 10/20 : 50%
				1, 1, 1, 1, 1, 1,				// 1 property   :  6/20 : 30%
				2, 2, 2,						// 2 properties :  3/20 : 15%
				3								// 3 properties :  1/20 :  5%
			};

		private static int[] m_MasterPropertyCounts = new int[]
			{
				0, 0, 0, 0, 0, 0,				// 0 properties : 6/10 : 60%
				1, 1, 1,						// 1 property   : 3/10 : 30%
				2								// 2 properties : 1/10 : 10%
			};

		private static int[] m_AdeptPropertyCounts = new int[]
			{
				0, 0, 0,						// 0 properties : 3/4 : 75%
				1								// 1 property   : 1/4 : 25%
			};

		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			int magery = from.Skills.Magery.BaseFixedPoint;

			if ( magery >= 800 )
			{
				int[] propertyCounts;
				int minIntensity;
				int maxIntensity;

				if ( magery >= 1000 )
				{
					propertyCounts = m_GrandPropertyCounts;
					minIntensity = 55;
					maxIntensity = 75;
				}
				else if ( magery >= 900 )
				{
					propertyCounts = m_MasterPropertyCounts;
					minIntensity = 25;
					maxIntensity = 45;
				}
				else
				{
					propertyCounts = m_AdeptPropertyCounts;
					minIntensity = 0;
					maxIntensity = 15;
				}

				int propertyCount = propertyCounts[Utility.Random( propertyCounts.Length )];

				BaseRunicTool.ApplyAttributesTo( this, true, 0, propertyCount, minIntensity, maxIntensity );

				SlayerName slayer = SlayerName.None;

				if ( 0.05 > Utility.RandomDouble() )
					slayer = BaseRunicTool.GetRandomSlayer();

				m_Slayer = slayer;

				if ( this is ScrappersCompendium )
				{
					this.Attributes.SpellDamage = 25;

					if ( this.Attributes.CastRecovery > 3 )
						this.Attributes.CastRecovery = 3;

					this.Attributes.CastSpeed = 1;

					if ( this.Attributes.LowerManaCost < 10 )
						this.Attributes.LowerManaCost = 10;
				}
			}

			if ( makersMark )
				Crafter = from;

			Exceptional = exceptional;

			return exceptional;
		}

		#region IImbuable members

		public virtual bool CanImbue { get { return false; } }

		public int TimesImbued { get { return 0; } set { } }

		public virtual ImbuingFlag ImbuingFlags { get { return ImbuingFlag.None; } }
		public bool IsSpecialMaterial { get { return false; } }
		public virtual int MaxIntensity { get { return 500; } }

		public void OnImbued()
		{
		}

		#endregion
	}
}