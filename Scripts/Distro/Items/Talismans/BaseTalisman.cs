using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells.Necromancy;
using Server.Spells.Spellweaving;
using Server.Spells.Mysticism;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Engines.Imbuing;
using Server.Engines.BuffIcons;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public enum CraftList
	{
		None,
		Alchemy,
		Blacksmith,
		Carpentry,
		Cartography,
		Cooking,
		Fletching,
		Inscribe,
		Tailoring,
		Tinkering
	}

	public class BaseTalisman : Item, IMagicalItem, IImbuable, ISlayer, IWearableDurability
	{
		private static String[] m_RemovalSummonTitles = new String[]
		{
			"Talisman of Curse Removal",
			"Talisman of Damage Removal",
			"Talisman of Ward Removal",
			"Talisman of Wildfire Removal",
			"Talisman of Clean Bandage Summoning",
			"Talisman of Board Summoning",
			"Talisman of Ingot Summoning",
			"Talisman of Random Summoning",
			"Talisman of Ant Lion Summoning",
			"Talisman of Cow Summoning",
			"Talisman of Lava Serpent Summoning",
			"Talisman of Orc Brute Summoning",
			"Talisman of Frost Spider Summoning",
			"Talisman of Panther Summoning",
			"Talisman of Doppleganger Summoning",
			"Talisman of Great Hart Summoning",
			"Talisman of Bull Frog Summoning",
			"Talisman of Artic Ogre Lord Summoning",
			"Talisman of Bogling Summoning",
			"Talisman of Bake Kitsune Summoning",
			"Talisman of Sheep Summoning",
			"Talisman of Skeletal Knight Summoning",
			"Talisman of Wailing Banshee Summoning",
			"Talisman of Chiken Summoning",
			"Talisman of Vorpal Bunny Summoning",
			null, // Mana phase
		};

		private bool m_Ownable = false;
		private Mobile m_Owner;

		private BaseCreature m_SummonedCreature = null;

		private ChargeTimeLeftTimer m_ChargeTimer;

		private bool m_isequiped = false;
		private int m_ChargeTimeLeft = 0;
		private int m_ChargeTimeLeft2 = 0;
		private DateTime m_ChargeTimeLeft3;

		private TalismanType m_TalismanType = 0;

		private int m_Charges = 49;

		private SlayerName m_Slayer, m_Slayer2;
		private TalisSlayerName m_TalisSlayer;

		private NPC_Name m_ProtectionEntry;
		private int m_ProtectionValue = 0;

		private NPC_Name m_KillersEntry;
		private int m_KillersValue = 0;

		private CraftList m_CraftBonusRegular = 0;
		private int m_CraftBonusRegularValue = 0;

		private CraftList m_CraftBonusExcep = 0;
		private int m_CraftBonusExcepValue = 0;

		private MagicalAttributes m_MagicalAttributes;
		private ElementAttributes m_AosResistances;
		private SkillBonuses m_SkillBonuses;

		private int m_MaxHitPoints;
		private int m_HitPoints;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get { return m_Owner; }
			set
			{
				m_Owner = value;

				if ( m_Owner != null )
					m_Ownable = true;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Ownable
		{
			get { return m_Ownable; }
			set
			{
				m_Ownable = value;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ChargeTimeLeftTimer ChargeTimer
		{
			get { return m_ChargeTimer; }
			set { m_ChargeTimer = value; }
		}

		public int ChargeTimeLeft
		{
			get { return m_ChargeTimeLeft; }
			set
			{
				m_ChargeTimeLeft = value;
				m_ChargeTimeLeft2 = value;
				InvalidateProperties();
			}
		}

		public int ChargeTimeLeft2
		{
			get { return m_ChargeTimeLeft2; }
			set
			{
				m_ChargeTimeLeft2 = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TalismanType TalismanType
		{
			get { return m_TalismanType; }
			set
			{
				m_TalismanType = value;

				if ( (int) m_TalismanType > 0 && m_Charges > -1 )
					this.Name = m_RemovalSummonTitles[(int) m_TalismanType - 1];
				else
					this.Name = null;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get { return m_Charges; }
			set
			{
				m_Charges = value;

				if ( (int) m_TalismanType > 0 && m_Charges > -1 )
					this.Name = m_RemovalSummonTitles[(int) m_TalismanType - 1];
				else
					this.Name = null;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer
		{
			get { return m_Slayer; }
			set
			{
				m_Slayer = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer2
		{
			get { return m_Slayer2; }
			set
			{
				m_Slayer2 = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TalisSlayerName TalisSlayer
		{
			get { return m_TalisSlayer; }
			set
			{
				m_TalisSlayer = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public NPC_Name ProtectionTalis
		{
			get { return m_ProtectionEntry; }
			set
			{
				m_ProtectionEntry = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ProtectionValue
		{
			get { return m_ProtectionValue; }
			set
			{
				m_ProtectionValue = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public NPC_Name KillersTalis
		{
			get { return m_KillersEntry; }
			set
			{
				m_KillersEntry = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int KillersValue
		{
			get { return m_KillersValue; }
			set
			{
				m_KillersValue = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftList CraftBonusRegular
		{
			get { return m_CraftBonusRegular; }
			set
			{
				m_CraftBonusRegular = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CraftBonusRegularValue
		{
			get { return m_CraftBonusRegularValue; }
			set
			{
				m_CraftBonusRegularValue = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftList CraftBonusExcep
		{
			get { return m_CraftBonusExcep; }
			set
			{
				m_CraftBonusExcep = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CraftBonusExcepValue
		{
			get { return m_CraftBonusExcepValue; }
			set
			{
				m_CraftBonusExcepValue = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MagicalAttributes Attributes
		{
			get { return m_MagicalAttributes; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ElementAttributes Resistances
		{
			get { return m_AosResistances; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillBonuses SkillBonuses
		{
			get { return m_SkillBonuses; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get { return m_MaxHitPoints; }
			set { m_MaxHitPoints = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitPoints
		{
			get
			{
				return m_HitPoints;
			}
			set
			{
				if ( value != m_HitPoints && MaxHitPoints > 0 )
				{
					m_HitPoints = value;

					if ( m_HitPoints < 0 )
						Delete();
					else if ( m_HitPoints > MaxHitPoints )
						m_HitPoints = MaxHitPoints;

					InvalidateProperties();
				}
			}
		}

		public override bool WearableByGargoyles { get { return true; } }

		public override int PhysicalResistance { get { return m_AosResistances.Physical; } }
		public override int FireResistance { get { return m_AosResistances.Fire; } }
		public override int ColdResistance { get { return m_AosResistances.Cold; } }
		public override int PoisonResistance { get { return m_AosResistances.Poison; } }
		public override int EnergyResistance { get { return m_AosResistances.Energy; } }

		public virtual bool CanLoseDurability { get { return m_HitPoints >= 0 && m_MaxHitPoints > 0; } }

		public virtual int InitMinHits { get { return 0; } }
		public virtual int InitMaxHits { get { return 0; } }

		public virtual bool Brittle { get { return false; } }
		public virtual bool CannotBeRepaired { get { return false; } }

		public override int LabelNumber { get { return 1024246; } } // Talisman

		public BaseTalisman( int ItemID )
			: base( ItemID )
		{
			Layer = Layer.Talisman;

			m_MagicalAttributes = new MagicalAttributes( this );
			m_AosResistances = new ElementAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );

			// 1% chance to spawn with the "Owned by no one" property.
			m_Ownable = ( 0.01 > Utility.RandomDouble() );

			m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );
		}

		public BaseTalisman( Serial serial )
			: base( serial )
		{
		}

		public virtual int OnHit( int damageTaken )
		{
			if ( 25 > Utility.Random( 100 ) ) // 25% chance to lower durability
			{
				int wear = Utility.Random( 2 );

				if ( wear > 0 && m_MaxHitPoints > 0 )
				{
					if ( m_HitPoints >= wear )
					{
						HitPoints -= wear;
						wear = 0;
					}
					else
					{
						wear -= HitPoints;
						HitPoints = 0;
					}

					if ( wear > 0 )
					{
						if ( m_MaxHitPoints > wear )
						{
							MaxHitPoints -= wear;

							if ( Parent is Mobile )
							{
								( (Mobile) Parent ).LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061121 ); // Your equipment is severely damaged.
								( (Mobile) Parent ).PlaySound( 0x38E );
							}
						}
						else
						{
							Delete();
						}
					}
				}
			}

			return damageTaken;
		}

		public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if ( m_Ownable && m_Owner != null )
				return false;

			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}

		public override bool OnEquip( Mobile from )
		{
			if ( m_Ownable )
			{
				if ( m_Owner == null )
				{
					m_Owner = from;
					LootType = LootType.Blessed;
					InvalidateProperties();
				}
				else if ( m_Owner != from )
				{
					from.SendLocalizedMessage( 500985 ); // You can't use that, it belongs to someone else.
					return false;
				}
			}

			return base.OnEquip( from );
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m_TalismanType == TalismanType.None )
				return;

			if ( this.Parent != m )
			{
				m.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
			}
			else if ( m_ChargeTimeLeft > 0 )
			{
				int tmptime = ( m_ChargeTimeLeft2 - ( DateTime.Now.Second - m_ChargeTimeLeft3.Second ) + 1 );

				if ( tmptime > ( m_ChargeTimeLeft2 + 1 ) )
					tmptime -= 60;
				if ( tmptime > m_ChargeTimeLeft2 )
					tmptime = m_ChargeTimeLeft2;

				m.SendLocalizedMessage( 1074882, tmptime.ToString() ); // You must wait ~1_val~ seconds for this to recharge.
			}
			else if ( m_Charges == 0 )
			{
				m.SendLocalizedMessage( 501250 ); // This magic item is out of charges.
			}
			else
			{

				if ( (int) m_TalismanType <= 4 )
				{
					//Removal
					m.Target = new InternalTarget( this );
				}
				//Mana Phase
				else if ( m_TalismanType == TalismanType.ManaPhase )
				{
					ManaPhase.OnUse( m, this );
				}
				else if ( (int) m_TalismanType >= 9 )
				{
					//Summon creature
					BaseCreature bc = (BaseCreature) Activator.CreateInstance( SummonEntry.GetNPC( m_TalismanType ) );
					if ( BaseCreature.Summon( bc, m, m.Location, -1, TimeSpan.FromMinutes( 10.0 ) ) )
					{
						bc.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );
						bc.PlaySound( bc.GetIdleSound() );
						m_SummonedCreature = bc;
						m_Charges--;
						InvalidateProperties();
						ChargeTimeLeft = 1800;
						m_ChargeTimer = new ChargeTimeLeftTimer( this );
						m_ChargeTimer.Start();
						m_ChargeTimeLeft3 = DateTime.Now;
					}
				}
				else if ( m_TalismanType == TalismanType.SummonRandom )
				{
					//Summon Random
					TalismanType tmpbc = SummonEntry.GetRandom();
					BaseCreature bc = (BaseCreature) Activator.CreateInstance( SummonEntry.GetNPC( tmpbc ) );
					if ( BaseCreature.Summon( bc, m, m.Location, -1, TimeSpan.FromMinutes( 10.0 ) ) )
					{
						bc.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );
						bc.PlaySound( bc.GetIdleSound() );
						m_SummonedCreature = bc;
						m_Charges--;
						InvalidateProperties();
						ChargeTimeLeft = 1800;
						m_ChargeTimer = new ChargeTimeLeftTimer( this );
						m_ChargeTimer.Start();
						m_ChargeTimeLeft3 = DateTime.Now;
					}
				}
				else
				{
					Item summonitem;
					int message;

					// Summon item
					if ( m_TalismanType == TalismanType.SummonBandage )
					{
						summonitem = new Bandage( 10 );
						summonitem.ItemID = 0x0EE9;
						summonitem.Hue = 0xA3;
						message = 1075002; // You have been given some clean bandages.
					}
					else if ( m_TalismanType == TalismanType.SummonBoard )
					{
						summonitem = new Board( 10 );
						summonitem.Hue = 0xA3;
						message = 1075000; // You have been given some wooden boards.
					}
					else// if ( m_TalismanType == TalismanType.SummonIngot )
					{
						summonitem = new IronIngot( 10 );
						summonitem.Hue = 0xA3;
						message = 1075001; // You have been given some ingots.
					}

					m.AddToBackpack( summonitem );
					m.SendLocalizedMessage( message );

					m_Charges--;
					InvalidateProperties();
					ChargeTimeLeft = 60;

					m_ChargeTimer = new ChargeTimeLeftTimer( this );
					m_ChargeTimer.Start();
					m_ChargeTimeLeft3 = DateTime.Now;
				}
			}
		}

		public void Target( BaseTalisman talis, Mobile owner, Mobile m )
		{
			if ( this.Parent == owner )
			{
				//Curse Removal
				if ( talis.m_TalismanType == TalismanType.CurseRemoval )
				{
					m.PlaySound( 0xF6 );
					m.PlaySound( 0x1F7 );
					m.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );

					StatMod mod;

					mod = m.GetStatMod( "[Magic] Str Malus" );
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod( "[Magic] Str Malus" );

					mod = m.GetStatMod( "[Magic] Dex Malus" );
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod( "[Magic] Dex Malus" );

					mod = m.GetStatMod( "[Magic] Int Malus" );
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod( "[Magic] Int Malus" );

					m.Paralyzed = false;

					EvilOmenSpell.CheckEffect( m );
					StrangleSpell.RemoveCurse( m );
					CorpseSkinSpell.RemoveCurse( m );
					CurseSpell.RemoveEffect( m );

					BuffInfo.RemoveBuff( m, BuffIcon.Clumsy );
					BuffInfo.RemoveBuff( m, BuffIcon.FeebleMind );
					BuffInfo.RemoveBuff( m, BuffIcon.Weaken );
					BuffInfo.RemoveBuff( m, BuffIcon.MassCurse );
					BuffInfo.RemoveBuff( m, BuffIcon.Curse );
					BuffInfo.RemoveBuff( m, BuffIcon.EvilOmen );
					StrangleSpell.RemoveCurse( m );
					CorpseSkinSpell.RemoveCurse( m );

					if ( owner != m )
						owner.SendLocalizedMessage( 1072409 ); // Your targets curses have been lifted
					m.SendLocalizedMessage( 1072408 ); // Any curses on you have been lifted

				}

				//Damage Removal
				if ( talis.m_TalismanType == TalismanType.DamageRemoval )
				{
					Effects.SendLocationEffect( m.Location, m.Map, 0x3728, 8 );
					Effects.PlaySound( m, m.Map, 0x201 );
					BleedAttack.EndBleed( m, false );
					MortalStrike.EndWound( m );
					m.CurePoison( m );

					BuffInfo.RemoveBuff( m, BuffIcon.Bleed );
					BuffInfo.RemoveBuff( m, BuffIcon.MortalStrike );

					if ( owner != m )
						owner.SendLocalizedMessage( 1072406 ); // Your Targets lasting damage effects have been removed!

					m.SendLocalizedMessage( 1072405 ); // Your lasting damage effects have been removed!
				}

				//Ward Removal
				if ( talis.m_TalismanType == TalismanType.WardRemoval )
				{
					Effects.SendLocationEffect( m.Location, m.Map, 0x3728, 8 );
					Effects.PlaySound( m, m.Map, 0x201 );

					ProtectionSpell.RemoveWard( m );
					ReactiveArmorSpell.RemoveWard( m );
					MagicReflectSpell.RemoveWard( m );
					TransformationSpell.RemoveContext( m, true );
					ReaperFormSpell.RemoveEffects( m );
					if ( StoneFormSpell.UnderEffect( m ) )
						StoneFormSpell.RemoveEffects( m );

					if ( owner != m )
						owner.SendLocalizedMessage( 1072403 ); // Your target's wards have been removed!

					m.SendLocalizedMessage( 1072402 ); // Your wards have been removed!
				}

				//Wildfire Removal
				if ( talis.m_TalismanType == TalismanType.WildfireRemoval )
					owner.SendLocalizedMessage( 1042753, "Wildfire Removal" ); // ~1_SOMETHING~ has been temporarily disabled.

				//CARGE TIMER
				ChargeTimeLeft = 1200;
				m_ChargeTimer = new ChargeTimeLeftTimer( this );
				m_ChargeTimer.Start();
				m_ChargeTimeLeft3 = DateTime.Now;

			}
			else if ( m_TalismanType != 0 )
				m.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
		}

		public override void OnAdded( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				//-------- Charged Time Left -----------

				m_isequiped = true;
				InvalidateProperties();

				//CARGE TIMER
				if ( m_ChargeTimeLeft > 0 )
				{
					ChargeTimeLeft2 = m_ChargeTimeLeft;
					m_ChargeTimer = new ChargeTimeLeftTimer( this );
					m_ChargeTimer.Start();
					m_ChargeTimeLeft3 = DateTime.Now;
				}

				//------ FIN Charged Time Left ---------

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
			}
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				if ( m_SummonedCreature != null && m_SummonedCreature.CheckAlive() )
				{
					Effects.SendLocationParticles( EffectItem.Create( m_SummonedCreature.Location, m_SummonedCreature.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 2100, 3, 5042, 0 );
					m_SummonedCreature.PlaySound( 0x201 );
					m_SummonedCreature.Delete();
				}

				//-------- Charged Time Left -----------

				m_isequiped = false;
				InvalidateProperties();
				if ( m_ChargeTimer != null )
					m_ChargeTimer.Stop();

				//------ FIN Charged Time Left ---------

				Mobile from = (Mobile) parent;

				m_SkillBonuses.Remove();

				string modName = this.Serial.ToString();

				from.RemoveStatMod( modName + "Str" );
				from.RemoveStatMod( modName + "Dex" );
				from.RemoveStatMod( modName + "Int" );

				from.CheckStatTimers();
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			#region Props
			base.GetProperties( list );
			if ( m_Slayer != SlayerName.None )
				list.Add( SlayerGroup.GetEntryByName( m_Slayer ).Title );

			if ( m_Slayer2 != SlayerName.None )
				list.Add( SlayerGroup.GetEntryByName( m_Slayer2 ).Title );

			if ( m_TalisSlayer != TalisSlayerName.None )
				list.Add( TalisSlayerEntry.GetSlayerTitle( m_TalisSlayer ) );

			m_SkillBonuses.GetProperties( list );

			if ( m_TalismanType != TalismanType.None && m_isequiped && m_ChargeTimeLeft == 0 )
				list.Add( 1074883 ); // Fully Charged
			else if ( m_TalismanType != TalismanType.None && m_isequiped && m_ChargeTimeLeft >= 1 )
				list.Add( 1074884, m_ChargeTimeLeft2.ToString() ); // Charge time left: ~1_val~

			if ( m_KillersEntry != NPC_Name.None && m_KillersValue > 0 )
				list.Add( 1072388, ProtectionKillerEntry.GetProtectionKillerTitle( m_KillersEntry ) + "\t" + m_KillersValue ); // ~1_NAME~ Killer: +~2_val~%

			if ( m_ProtectionEntry != NPC_Name.None && m_ProtectionValue > 0 )
				list.Add( 1072387, ProtectionKillerEntry.GetProtectionKillerTitle( m_ProtectionEntry ) + "\t" + m_ProtectionValue ); // ~1_NAME~ Protection: +~2_val~%

			if ( m_CraftBonusExcep > 0 && m_CraftBonusExcepValue > 0 )
				list.Add( 1072395, Enum.GetName( typeof( CraftList ), m_CraftBonusExcep ) + "\t" + m_CraftBonusExcepValue ); // ~1_NAME~ Exceptional Bonus: ~2_val~%

			if ( m_CraftBonusRegular > 0 && m_CraftBonusRegularValue > 0 )
				list.Add( 1072394, Enum.GetName( typeof( CraftList ), m_CraftBonusRegular ) + "\t" + m_CraftBonusRegularValue ); // ~1_NAME~ Bonus: ~2_val~%			

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

			if ( m_Ownable )
				list.Add( 1072304, m_Owner != null ? m_Owner.Name : "no one" ); // Owned by ~1_name~

			base.AddResistanceProperties( list );

			if ( m_Charges >= 0 && m_TalismanType != TalismanType.None )
				list.Add( 1060741, m_Charges.ToString() ); // charges: ~1_val~

			if ( m_TalismanType == TalismanType.ManaPhase )
				list.Add( 1116158 ); // Mana Phase

			if ( Brittle )
				list.Add( 1116209 ); // Brittle

			if ( CanLoseDurability )
				list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~

			#endregion
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 4 ); // version

			// Version 4
			writer.WriteEncodedInt( (int) m_MaxHitPoints );
			writer.WriteEncodedInt( (int) m_HitPoints );

			// Version 3
			writer.Write( (int) m_Slayer );
			writer.Write( (int) m_Slayer2 );

			// Version 2
			writer.Write( (bool) m_Ownable );
			writer.Write( (Mobile) m_Owner );
			writer.Write( (int) m_CraftBonusRegular );
			writer.Write( (int) m_CraftBonusRegularValue );
			writer.Write( (int) m_CraftBonusExcep );
			writer.Write( (int) m_CraftBonusExcepValue );
			writer.Write( (int) m_ProtectionEntry );
			writer.Write( (int) m_ProtectionValue );
			writer.Write( (int) m_KillersEntry );
			writer.Write( (int) m_KillersValue );
			writer.Write( (int) m_TalisSlayer );
			writer.Write( (int) m_TalismanType );
			writer.Write( (int) m_Charges );

			m_MagicalAttributes.Serialize( writer );
			m_AosResistances.Serialize( writer );
			m_SkillBonuses.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 4:
					{
						m_MaxHitPoints = reader.ReadEncodedInt();
						m_HitPoints = reader.ReadEncodedInt();

						goto case 3;
					}
				case 3:
					{
						m_Slayer = (SlayerName) reader.ReadInt();
						m_Slayer2 = (SlayerName) reader.ReadInt();

						goto case 2;
					}
				case 2:
					{
						m_Ownable = (bool) reader.ReadBool();
						m_Owner = (Mobile) reader.ReadMobile();
						m_CraftBonusRegular = (CraftList) reader.ReadInt();
						m_CraftBonusRegularValue = reader.ReadInt();
						m_CraftBonusExcep = (CraftList) reader.ReadInt();
						m_CraftBonusExcepValue = reader.ReadInt();
						m_ProtectionEntry = (NPC_Name) reader.ReadInt();
						m_ProtectionValue = reader.ReadInt();
						m_KillersEntry = (NPC_Name) reader.ReadInt();
						m_KillersValue = reader.ReadInt();
						m_TalisSlayer = (TalisSlayerName) reader.ReadInt();
						m_TalismanType = (TalismanType) reader.ReadInt();
						m_Charges = reader.ReadInt();

						goto case 1;
					}
				case 1:
					{
						m_MagicalAttributes = new MagicalAttributes( this, reader );
						m_AosResistances = new ElementAttributes( this, reader );
						m_SkillBonuses = new SkillBonuses( this, reader );

						if ( Parent is Mobile )
						{
							//-------- Charged Time Left -----------

							m_isequiped = true;
							InvalidateProperties();

							//------ FIN Charged Time Left ---------
							m_SkillBonuses.AddTo( (Mobile) Parent );
						}

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

						break;
					}
				case 0:
					{
						if ( Parent is Mobile )
						{
							//-------- Charged Time Left -----------

							m_isequiped = true;
							InvalidateProperties();

							//------ FIN Charged Time Left ---------
							m_SkillBonuses.AddTo( (Mobile) Parent );
						}
						m_MagicalAttributes = new MagicalAttributes( this );
						m_AosResistances = new ElementAttributes( this );
						m_SkillBonuses = new SkillBonuses( this );

						break;
					}
			}

			if ( version < 2 )
			{
				m_Ownable = (bool) reader.ReadBool();
				m_Owner = (Mobile) reader.ReadMobile();
				m_CraftBonusRegular = (CraftList) reader.ReadInt();
				m_CraftBonusRegularValue = reader.ReadInt();
				m_CraftBonusExcep = (CraftList) reader.ReadInt();
				m_CraftBonusExcepValue = reader.ReadInt();
				m_ProtectionEntry = (NPC_Name) reader.ReadInt();
				m_ProtectionValue = reader.ReadInt();
				m_KillersEntry = (NPC_Name) reader.ReadInt();
				m_KillersValue = reader.ReadInt();
				m_TalisSlayer = (TalisSlayerName) reader.ReadInt();
				m_TalismanType = (TalismanType) reader.ReadInt();
				m_Charges = reader.ReadInt();
			}

		}
		public static bool IsTalismanEquiped( Mobile from )
		{
			BaseTalisman talisman = from.FindItemOnLayer( Layer.Talisman ) as BaseTalisman;
			if ( talisman != null )
				return true;
			return false;
		}

		public static BaseTalisman GetTalisman( Mobile from )
		{
			BaseTalisman talisman = from.FindItemOnLayer( Layer.Talisman ) as BaseTalisman;
			if ( talisman != null )
				return talisman;
			return null;
		}

		private class InternalTarget : Target
		{
			private BaseTalisman m_talisman;

			public InternalTarget( BaseTalisman talisman )
				: base( 50, false, TargetFlags.Beneficial )
			{
				m_talisman = talisman;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					if ( from.InRange( m_talisman.GetWorldLocation(), 50 ) )
					{
						if ( m_talisman.m_Charges >= 0 )
							m_talisman.m_Charges--;
						m_talisman.InvalidateProperties();

						m_talisman.Target( m_talisman, from, (Mobile) targeted );
					}
					else
					{
						from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
					}
				}
				else
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
				}
			}
		}
		//---- TIMER
		public class ChargeTimeLeftTimer : Timer
		{
			private BaseTalisman m_talism;

			public ChargeTimeLeftTimer( BaseTalisman talism )
				: base( TimeSpan.FromSeconds( 10.0 ), TimeSpan.FromSeconds( 10.0 ), 200 )
			{
				m_talism = talism;
			}

			protected override void OnTick()
			{
				m_talism.ChargeTimeLeft2 -= 10;
				m_talism.m_ChargeTimeLeft3 = DateTime.Now;
				if ( m_talism.ChargeTimeLeft2 <= 0 )
				{
					m_talism.ChargeTimeLeft = 0;
					Stop();
				}
			}
		}
		//---- END TIMER

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
