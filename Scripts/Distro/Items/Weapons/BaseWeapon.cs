using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Factions;
using Server.Engines.Craft;
using Server.Engines.Imbuing;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.Necromancy;
using Server.Spells.Bushido;
using Server.Spells.Ninjitsu;
using Server.Spells.Spellweaving;
using Server.Spells.Mysticism;

namespace Server.Items
{
	public interface ISlayer
	{
		SlayerName Slayer { get; set; }
		SlayerName Slayer2 { get; set; }
	}

	public abstract class BaseWeapon : Item, IWeapon, IFactionItem, ICraftable, ISlayer, IDurability, IImbuable, IGorgonCharges, IArtifactRarity
	{
		private string m_EngravedText;

		[CommandProperty( AccessLevel.GameMaster )]
		public string EngravedText
		{
			get { return m_EngravedText; }
			set { m_EngravedText = value; InvalidateProperties(); }
		}

		#region Factions
		private FactionItem m_FactionState;

		public FactionItem FactionItemState
		{
			get { return m_FactionState; }
			set
			{
				m_FactionState = value;

				if ( m_FactionState == null )
					Hue = CraftResources.GetHue( Resource );

				LootType = ( m_FactionState == null ? LootType.Regular : LootType.Blessed );
			}
		}
		#endregion

		// Instance values. These values are unique to each weapon.

		private bool m_Exceptional;
		private Mobile m_Crafter;
		private Poison m_Poison;
		private int m_PoisonCharges;
		private int m_Hits;
		private int m_MaxHits;
		private SlayerName m_Slayer, m_Slayer2;
		private TalisSlayerName m_Slayer3;
		private SkillMod m_SkillMod, m_MageMod;
		private CraftResource m_Resource;
		private bool m_PlayerConstructed;

		private bool m_Immolating; // Is this weapon immolating via Immolating Weapon arcanist spell? Temporary; not serialized.
		private bool m_Cursed; // Is this weapon cursed via Curse Weapon necromancer spell? Temporary; not serialized.

		private bool m_Altered;

		private int m_TimesImbued;

		private MagicalAttributes m_MagicalAttributes;
		private WeaponAttributes m_WeaponAttributes;
		private SkillBonuses m_SkillBonuses;
		private ElementAttributes m_Resistances;
		private AbsorptionAttributes m_AbsorptionAttributes;

		public virtual WeaponAbility PrimaryAbility { get { return null; } }
		public virtual WeaponAbility SecondaryAbility { get { return null; } }

		public virtual SkillName AbilitySkill { get { return SkillName.Alchemy; } }

		// Default values intended to be overriden in single weapon scripts

		public virtual WeaponAnimation Animation { get { return WeaponAnimation.Slash1H; } }
		public virtual WeaponType Type { get { return WeaponType.Slashing; } }
		public virtual SkillName Skill { get { return SkillName.Swords; } }

		public virtual int MaxRange { get { return 1; } }

		public virtual int HitSound { get { return 0; } }
		public virtual int MissSound { get { return 0; } }

		public virtual int MinDamage { get { return 0; } }
		public virtual int MaxDamage { get { return 0; } }

		public virtual int Speed { get { return 0; } }

		public virtual int StrengthReq { get { return 0; } }
		public virtual int DexterityReq { get { return 0; } }
		public virtual int IntelligenceReq { get { return 0; } }

		public virtual bool CanLoseDurability { get { return m_Hits >= 0 && m_MaxHits > 0; } }

		public virtual int InitMinHits { get { return 0; } }
		public virtual int InitMaxHits { get { return 0; } }

		public virtual bool CanFortify { get { return m_TimesImbued == 0; } }

		public virtual bool Brittle { get { return false; } }
		public virtual bool CannotBeRepaired { get { return false; } }

		public override int PhysicalResistance { get { return m_Resistances.Physical; } }
		public override int FireResistance { get { return m_Resistances.Fire; } }
		public override int ColdResistance { get { return m_Resistances.Cold; } }
		public override int PoisonResistance { get { return m_Resistances.Poison; } }
		public override int EnergyResistance { get { return m_Resistances.Energy; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public MagicalAttributes Attributes
		{
			get { return m_MagicalAttributes; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponAttributes WeaponAttributes
		{
			get { return m_WeaponAttributes; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillBonuses SkillBonuses
		{
			get { return m_SkillBonuses; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ElementAttributes Resistances
		{
			get { return m_Resistances; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AbsorptionAttributes AbsorptionAttributes
		{
			get { return m_AbsorptionAttributes; }
			set
			{
			}
		}

		private EnchantContext m_EnchantContext;

		public EnchantContext EnchantContext
		{
			get { return m_EnchantContext; }
			set
			{
				m_EnchantContext = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Enchanted { get { return m_EnchantContext != null; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Immolating
		{
			get { return m_Immolating; }
			set
			{
				m_Immolating = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Cursed { get { return m_Cursed; } set { m_Cursed = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitPoints
		{
			get { return m_Hits; }
			set
			{
				if ( m_Hits != value )
				{
					if ( value > m_MaxHits )
						value = m_MaxHits;

					m_Hits = value;

					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get { return m_MaxHits; }
			set
			{
				if ( m_MaxHits != value )
				{
					m_MaxHits = value;

					if ( m_Hits > m_MaxHits )
						m_Hits = m_MaxHits;

					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonCharges
		{
			get { return m_PoisonCharges; }
			set
			{
				m_PoisonCharges = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set
			{
				m_Poison = value;
				InvalidateProperties();
			}
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
		public TalisSlayerName Slayer3
		{
			get { return m_Slayer3; }
			set
			{
				m_Slayer3 = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PlayerConstructed
		{
			get { return m_PlayerConstructed; }
			set { m_PlayerConstructed = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				if ( m_Resource != value )
				{
					UnscaleDurability();

					m_Resource = value;

					Hue = CraftResources.GetHue( m_Resource );

					ApplyResourceAttributes( m_Resource );

					ScaleDurability();

					InvalidateProperties();
				}
			}
		}

		public override void OnAfterDuped( Item newItem )
		{
			// TODO: Copy Attributes
		}

		public void UnscaleDurability()
		{
			int scale = 100 + WeaponAttributes.DurabilityBonus;

			m_Hits = ( ( m_Hits * 100 ) + ( scale - 1 ) ) / scale;
			m_MaxHits = ( ( m_MaxHits * 100 ) + ( scale - 1 ) ) / scale;
			InvalidateProperties();
		}

		public void ScaleDurability()
		{
			int scale = 100 + WeaponAttributes.DurabilityBonus;

			m_Hits = Math.Min( 255, ( ( m_Hits * scale ) + 99 ) / 100 );
			m_MaxHits = Math.Min( 255, ( ( m_MaxHits * scale ) + 99 ) / 100 );
			InvalidateProperties();
		}

		public int GetLowerStatReq()
		{
			int v = WeaponAttributes.LowerStatReq;

			CraftResourceInfo info = CraftResources.GetInfo( m_Resource );

			if ( info != null )
			{
				CraftAttributeInfo attrInfo = info.AttributeInfo;

				if ( attrInfo != null )
					v += attrInfo.WeaponLowerRequirements;
			}

			if ( v > 100 )
				v = 100;

			return v;
		}

		public static void BlockEquip( Mobile m, TimeSpan duration )
		{
			if ( m.BeginAction( typeof( BaseWeapon ) ) )
			{
				new ResetEquipTimer( m, duration ).Start();
			}
		}

		private class ResetEquipTimer : Timer
		{
			private Mobile m_Mobile;

			public ResetEquipTimer( Mobile m, TimeSpan duration )
				: base( duration )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.EndAction( typeof( BaseWeapon ) );
			}
		}

		public override bool CheckConflictingLayer( Mobile m, Item item, Layer layer )
		{
			if ( base.CheckConflictingLayer( m, item, layer ) )
			{
				return true;
			}

			if ( this.Layer == Layer.TwoHanded && layer == Layer.OneHanded )
			{
				return true;
			}
			else if ( this.Layer == Layer.OneHanded && layer == Layer.TwoHanded && !( item is BaseShield ) && !( item is BaseEquipableLight ) )
			{
				return true;
			}

			return false;
		}

		public override bool CanEquip( Mobile from )
		{
			if ( from.Str < AOS.Scale( StrengthReq, 100 - GetLowerStatReq() ) )
			{
				from.SendLocalizedMessage( 500213 ); // You are not strong enough to equip that.
				return false;
			}
			else if ( !from.CanBeginAction( typeof( BaseWeapon ) ) )
			{
				return false;
			}
			else
			{
				return base.CanEquip( from );
			}
		}

		public virtual bool UseSkillMod { get { return false; } }

		public override bool OnEquip( Mobile from )
		{
			if ( !base.OnEquip( from ) )
				return false;

			int strBonus = Attributes.BonusStr;
			int dexBonus = Attributes.BonusDex;
			int intBonus = Attributes.BonusInt;

			if ( ( strBonus != 0 || dexBonus != 0 || intBonus != 0 ) )
			{
				Mobile m = from;

				string modName = this.Serial.ToString();

				if ( strBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

				if ( dexBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

				if ( intBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}

			if ( m_WeaponAttributes.MageWeapon != 0 && m_WeaponAttributes.MageWeapon != 30 )
			{
				if ( m_MageMod != null )
					m_MageMod.Remove();

				m_MageMod = new DefaultSkillMod( SkillName.Magery, true, -30 + m_WeaponAttributes.MageWeapon );
				from.AddSkillMod( m_MageMod );
			}

			return true;
		}

		public override void OnAfterEquip( Mobile from )
		{
			base.OnAfterEquip( from );

			from.NextCombatTime = DateTime.Now + GetDelay( from );
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				m_SkillBonuses.AddTo( from );

				from.CheckStatTimers();
				from.Delta( MobileDelta.WeaponDamage );
			}
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile m = (Mobile) parent;
				BaseWeapon weapon = m.Weapon as BaseWeapon;

				if ( m_Immolating )
					ImmolatingWeaponSpell.EndImmolating( this, false );

				if ( Enchanted )
					EnchantSpell.RemoveEnchantContext( this );

				if ( ConsecrateWeaponSpell.UnderEffect( m ) )
					ConsecrateWeaponSpell.RemoveEffect( m );

				if ( m is PlayerMobile )
					( (PlayerMobile) m ).LastEquipedWeapon = this;

				string modName = this.Serial.ToString();

				m.RemoveStatMod( modName + "Str" );
				m.RemoveStatMod( modName + "Dex" );
				m.RemoveStatMod( modName + "Int" );

				if ( weapon != null )
					m.NextCombatTime = DateTime.Now + weapon.GetDelay( m );

				if ( UseSkillMod && m_SkillMod != null )
				{
					m_SkillMod.Remove();
					m_SkillMod = null;
				}

				if ( m_MageMod != null )
				{
					m_MageMod.Remove();
					m_MageMod = null;
				}

				m_SkillBonuses.Remove();

				m.CheckStatTimers();

				m.Delta( MobileDelta.WeaponDamage );
			}
		}

		public virtual SkillName GetUsedSkill( Mobile m, bool checkSkillAttrs )
		{
			SkillName sk;

			if ( checkSkillAttrs && m_WeaponAttributes.UseBestSkill != 0 )
			{
				double swrd = m.Skills[SkillName.Swords].Value;
				double fenc = m.Skills[SkillName.Fencing].Value;
				double mcng = m.Skills[SkillName.Macing].Value;
				double val;

				sk = SkillName.Swords;
				val = swrd;

				if ( fenc > val )
				{
					sk = SkillName.Fencing;
					val = fenc;
				}
				if ( mcng > val )
				{
					sk = SkillName.Macing;
					val = mcng;
				}
			}
			else if ( m_WeaponAttributes.MageWeapon != 0 )
			{
				if ( m.Skills[SkillName.Magery].Value > m.Skills[Skill].Value )
				{
					sk = SkillName.Magery;
				}
				else
				{
					sk = Skill;
				}
			}
			else
			{
				sk = Skill;

				if ( sk != SkillName.Wrestling && !m.IsPlayer && !m.Body.IsHuman && m.Skills[SkillName.Wrestling].Value > m.Skills[sk].Value )
				{
					sk = SkillName.Wrestling;
				}
			}

			return sk;
		}

		public virtual double GetAttackSkillValue( Mobile attacker, Mobile defender )
		{
			return attacker.Skills[GetUsedSkill( attacker, true )].Value;
		}

		public virtual double GetDefendSkillValue( Mobile attacker, Mobile defender )
		{
			return defender.Skills[GetUsedSkill( defender, true )].Value;
		}

		public static double GetHitChance( double atkValue, double defValue, int atkBonus, int defBonus )
		{
			if ( atkValue <= -20.0 )
				atkValue = -19.9;

			if ( defValue <= -20.0 )
				defValue = -19.9;

			double ourValue = ( atkValue + 20.0 ) * ( 100 + atkBonus );
			double theirValue = ( defValue + 20.0 ) * ( 100 + defBonus );

			double chance = ourValue / ( theirValue * 2.0 );

			if ( chance < 0.02 )
				chance = 0.02;

			if ( chance > 1.0 )
				chance = 1.0;

			return chance;
		}

		public static int GetAttackChance( Mobile m )
		{
			int bonus = m.GetMagicalAttribute( MagicalAttribute.AttackChance );

			if ( m.BodyMod == 0xF6 || m.BodyMod == 0x19 )
				bonus += (int) ( m.Skills[SkillName.Ninjitsu].Value * 0.1 ); // TODO: verify

			if ( Spells.Chivalry.DivineFurySpell.UnderEffect( m ) )
				bonus += 10; // attacker gets 10% bonus when they're under divine fury

			if ( HitLower.IsUnderAttackEffect( m ) )
				bonus -= 25; // Under Hit Lower Attack effect -> 25% malus

			if ( bonus > 45 )
				bonus = 45;

			if ( m.Race == Race.Gargoyle )
				bonus += 5; // Gargoyles receive a 5% bonus to hit that stacks with HCI and skill bonus.

			return bonus;
		}

		public static int GetDefendChance( Mobile m )
		{
			int bonus = m.GetMagicalAttribute( MagicalAttribute.DefendChance );

			if ( Spells.Chivalry.DivineFurySpell.UnderEffect( m ) )
				bonus -= 20; // defender loses 20% bonus when they're under divine fury

			if ( HitLower.IsUnderDefenseEffect( m ) )
				bonus -= 25; // Under Hit Lower Defense effect -> 25% malus

			if ( ForceArrow.IsUnderForceArrowEffect( m ) )
				bonus -= 25; // TODO: Comprobar

			int surpriseMalus = 0;

			if ( SurpriseAttack.GetMalus( m, ref surpriseMalus ) )
				bonus -= surpriseMalus;

			if ( Block.UnderEffect( m ) )
			{
				int chb = Utility.Random( 10, 15 );

				bonus += (int) ( ( chb / 100.0 ) * bonus );
			}

			int discordanceEffect = 0;

			// Defender loses -0/-28% if under the effect of Discordance.
			if ( SkillHandlers.Discordance.GetEffect( m, ref discordanceEffect ) )
				bonus -= discordanceEffect;

			if ( bonus > 45 )
				bonus = 45;

			return bonus;
		}

		public virtual bool CheckHit( Mobile attacker, Mobile defender )
		{
			BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
			BaseWeapon defWeapon = defender.Weapon as BaseWeapon;

			Skill atkSkill = attacker.Skills[atkWeapon.Skill];

			double atkValue = atkWeapon.GetAttackSkillValue( attacker, defender );
			double defValue = defWeapon.GetDefendSkillValue( attacker, defender );

			double chance = GetHitChance( atkValue, defValue, GetAttackChance( attacker ), GetDefendChance( defender ) );

			WeaponAbility ability = WeaponAbility.GetCurrentAbility( attacker );

			if ( ability != null )
				chance *= ability.GetAccuracyScalar( attacker, defender );

			SpecialMove move = SpecialMove.GetCurrentMove( attacker );

			if ( move != null )
				chance *= move.GetAccuracyScalar( attacker );

			if ( atkWeapon is BaseThrowing && ( (BaseThrowing) atkWeapon ).Debug )
			{
				attacker.SendMessage( "Your weapon's range is {0} of {1}", atkWeapon.GetMaxRange( attacker, defender ), atkWeapon.MaxRange );
				attacker.SendMessage( "Your distance to the target is {0}", (int) attacker.GetDistanceToSqrt( defender ) );
				attacker.SendMessage( "Your base hit chance is {0:0.0}", chance );
			}

			chance *= GetAccuracyScalar( attacker, defender, atkWeapon.GetMaxRange( attacker, defender ) );

			BaseThrowing.ApplyShieldPenalties( attacker, defender, ref chance );

			if ( atkWeapon is BaseThrowing && ( (BaseThrowing) atkWeapon ).Debug )
				attacker.SendMessage( "Your final hit chance is {0:0.0}", chance );

			return attacker.CheckSkill( atkSkill.SkillName, chance );
		}

		public virtual int GetMaxRange( Mobile attacker, Mobile defender )
		{
			return MaxRange;
		}

		public virtual double GetAccuracyScalar( Mobile attacker, Mobile defender, int maxRange )
		{
			return 1.0;
		}

		public virtual double GetDamageScalar( Mobile attacker, Mobile defender, int maxRange )
		{
			return 1.0;
		}

		public int GetSpeed()
		{
			return this.Speed * 100 / ( 100 + Attributes.WeaponSpeed );
		}

		public virtual TimeSpan GetDelay( Mobile m )
		{
			int baseSwingSpeedInTicks = this.Speed;
			int staminaSpeedBonusInTicks = m.Stam / 30;
			int modifiedSwingSpeedInTicks = baseSwingSpeedInTicks - staminaSpeedBonusInTicks;

			int ssiprop = m.GetMagicalAttribute( MagicalAttribute.WeaponSpeed );

			if ( StoneFormSpell.UnderEffect( m ) )
				ssiprop -= 10;

			int finalSwingSpeedInTicks = modifiedSwingSpeedInTicks * 100 / ( 100 + ssiprop );

			// With this speed level we will hit always at cap despite the mods
			if ( finalSwingSpeedInTicks <= 0 )
				return TimeSpan.FromSeconds( 1.25 );

			if ( DualWield.UnderEffect( m ) )
				finalSwingSpeedInTicks /= 2;

			if ( Spells.Chivalry.DivineFurySpell.UnderEffect( m ) )
				finalSwingSpeedInTicks -= 1;

			// Bonus granted by successful use of Honorable Execution. (-0.75 at 120 bushido)
			finalSwingSpeedInTicks -= HonorableExecution.GetSwingBonus( m ) / 10;

			if ( ReaperFormSpell.UnderEffect( m ) )
				finalSwingSpeedInTicks -= 1;

			if ( EssenceOfWindSpell.IsDebuffed( m ) )
				finalSwingSpeedInTicks += EssenceOfWindSpell.GetSSIMalus( m );

			double delayInSeconds = finalSwingSpeedInTicks * 0.25;

			// Maximum swing rate capped at one swing per 1.25 second 
			if ( delayInSeconds < 1.25 )
				delayInSeconds = 1.25;

			return TimeSpan.FromSeconds( delayInSeconds );
		}

		public virtual void OnBeforeSwing( Mobile attacker, Mobile defender )
		{
			WeaponAbility a = WeaponAbility.GetCurrentAbility( attacker );

			if ( a != null && !a.OnBeforeSwing( attacker, defender ) )
				WeaponAbility.ClearCurrentAbility( attacker );

			SpecialMove move = SpecialMove.GetCurrentMove( attacker );

			if ( move != null && !move.OnBeforeSwing( attacker, defender ) )
				SpecialMove.ClearCurrentMove( attacker );
		}

		public virtual bool CanSwing( Mobile m )
		{
			bool canSwing = true;

			canSwing = ( !m.Paralyzed && !m.Frozen );

			if ( canSwing )
			{
				Spell sp = m.Spell as Spell;

				canSwing = ( sp == null || !sp.IsCasting || !sp.BlocksMovement );
			}

			if ( canSwing )
				canSwing = !BaseBardCreature.IsCalmed( m );

			return canSwing;
		}

		public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			return OnSwing( attacker, defender, 1.0 );
		}

		public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender, double damageBonus )
		{
			if ( CanSwing( attacker ) && attacker.HarmfulCheck( defender ) )
			{
				attacker.DisruptiveAction();

				if ( attacker.Client != null )
					attacker.Send( new Swing( 0, attacker, defender ) );

				if ( attacker is BaseCreature )
				{
					BaseCreature bc = (BaseCreature) attacker;
					WeaponAbility ab = bc.GetWeaponAbility();

					// Hiryu dismount special move can no longer be used
					// to separate a Chaos Dragoon Elite from his mount
					if ( ( bc is Hiryu || bc is LesserHiryu ) && defender is ChaosDragoonElite )
						ab = null;

					if ( ab != null )
					{
						if ( bc.WeaponAbilityChance > Utility.RandomDouble() )
							WeaponAbility.SetCurrentAbility( bc, ab );
						else
							WeaponAbility.ClearCurrentAbility( bc );
					}
				}

				if ( CheckHit( attacker, defender ) )
					OnHit( attacker, defender, damageBonus );
				else
					OnMiss( attacker, defender );
			}

			return GetDelay( attacker );
		}

		public virtual int GetHitAttackSound( Mobile attacker, Mobile defender )
		{
			int sound = attacker.GetAttackSound();

			if ( sound == -1 )
				sound = HitSound;

			return sound;
		}

		public virtual int GetHitDefendSound( Mobile attacker, Mobile defender )
		{
			return defender.GetHurtSound();
		}

		public virtual int GetMissAttackSound( Mobile attacker, Mobile defender )
		{
			if ( attacker.GetAttackSound() == -1 )
			{
				return MissSound;
			}
			else
			{
				return -1;
			}
		}

		public virtual int GetMissDefendSound( Mobile attacker, Mobile defender )
		{
			return -1;
		}

		public static bool CheckParry( Mobile defender )
		{
			if ( defender == null )
				return false;

			BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;

			double parry = defender.Skills[SkillName.Parry].Value;
			double bushidoNonRacial = defender.Skills[SkillName.Bushido].NonRacialValue;
			double bushido = defender.Skills[SkillName.Bushido].Value;

			if ( shield != null )
			{
				double chance = ( parry - bushidoNonRacial ) / 400.0; // As per OSI, no negitive effect from the Racial stuffs, ie, 120 parry and '0' bushido with humans

				// Parry over 100 grants a 5% bonus.
				if ( parry >= 100.0 )
					chance += 0.05;

				// Evasion grants a variable bonus post ML. 50% prior.
				if ( Evasion.IsEvading( defender ) )
					chance *= Evasion.GetParryScalar( defender );

				// Low dexterity lowers the chance.
				if ( defender.Dex < 80 )
					chance = chance * ( 20 + defender.Dex ) / 100;

				return defender.CheckSkill( SkillName.Parry, chance );
			}
			else if ( !( defender.Weapon is Fists ) && !( defender.Weapon is BaseRanged ) )
			{
				BaseWeapon weapon = defender.Weapon as BaseWeapon;

				double divisor = ( weapon.Layer == Layer.OneHanded ) ? 48000.0 : 41140.0;

				double newChance = ( parry * bushido ) / divisor;
				double legacyChance = parry / 800.0;

				// Parry or Bushido over 100 grant a 5% bonus.
				if ( parry >= 100.0 )
				{
					newChance += 0.05;
					legacyChance += 0.05;
				}
				else if ( bushido >= 100.0 )
				{
					newChance += 0.05;
				}

				// Evasion grants a variable bonus post ML. 50% prior.
				if ( Evasion.IsEvading( defender ) )
					newChance *= Evasion.GetParryScalar( defender );

				// Low dexterity lowers the chance.
				if ( defender.Dex < 80 )
					newChance = newChance * ( 20 + defender.Dex ) / 100;

				return defender.CheckSkill( SkillName.Parry, Math.Max( newChance, legacyChance ) );
			}

			return false;
		}

		public virtual int AbsorbDamageAOS( Mobile attacker, Mobile defender, int damage )
		{
			bool blocked = false;

			if ( defender.IsPlayer || defender.Body.IsHuman )
			{
				blocked = CheckParry( defender );

				if ( blocked )
				{
					defender.FixedEffect( 0x37B9, 10, 16 );
					damage = 0;

					// Successful block removes the Honorable Execution penalty.
					HonorableExecution.RemovePenalty( defender );

					if ( CounterAttack.IsCountering( defender ) )
					{
						BaseWeapon weapon = defender.Weapon as BaseWeapon;

						if ( weapon != null && defender.GetDistanceToSqrt( attacker ) <= 1 )
						{
							weapon.OnSwing( defender, attacker );

							CounterAttack.StopCountering( defender );
						}
					}

					if ( Confidence.IsConfident( defender ) )
					{
						defender.SendLocalizedMessage( 1063117 ); // Your confidence reassures you as you successfully block your opponent's blow.

						double bushido = defender.Skills[SkillName.Bushido].Value;

						defender.Hits += Utility.Random( 1, (int) ( bushido / 12 ) );
						defender.Stam += Utility.Random( 1, (int) ( bushido / 5 ) );
					}

					BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;

					if ( shield != null )
					{
						if ( shield.ArmorAttributes.SelfRepair > Utility.Random( 10 ) )
						{
							shield.HitPoints += 2;
						}
						else
						{
							int wear = Utility.Random( 2 );

							if ( wear > 0 && shield.MaxHitPoints > 0 )
							{
								if ( shield.HitPoints >= wear )
								{
									shield.HitPoints -= wear;
									wear = 0;
								}
								else
								{
									wear -= shield.HitPoints;
									shield.HitPoints = 0;
								}

								if ( wear > 0 )
								{
									if ( shield.MaxHitPoints > wear )
									{
										shield.MaxHitPoints -= wear;

										if ( shield.Parent is Mobile )
											( (Mobile) shield.Parent ).LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061121 ); // Your equipment is severely damaged.
									}
									else
									{
										shield.Delete();
									}
								}
							}
						}

						if ( shield.ArmorAttributes[ArmorAttribute.ReactiveParalyze] != 0 && 0.3 > Utility.RandomDouble() )
						{
							if ( !attacker.Frozen && !attacker.Paralyzed )
							{
								// Taken from Paralyze Spell

								int secs = (int) ( ( defender.Skills[SkillName.EvalInt].Value - attacker.Skills[SkillName.MagicResist].Value ) / 10.0 );

								if ( !attacker.IsPlayer )
									secs *= 3;

								if ( secs < 0 )
									secs = 0;

								attacker.Paralyze( TimeSpan.FromSeconds( secs ) );

								attacker.PlaySound( 0x204 );
								attacker.FixedEffect( 0x376A, 6, 1 );
							}
						}
					}
				}

				if ( DefenseMastery.UnderEffect( defender ) )
				{
					AOS.Damage( attacker, defender, Utility.Random( 15, 25 ), 0, 0, 0, 0, 100 ); // TODO: verify
					attacker.BoltEffect( 0 );
				}
			}

			return damage;
		}

		public virtual int AbsorbDamage( Mobile attacker, Mobile defender, int damage )
		{
			return AbsorbDamageAOS( attacker, defender, damage );
		}

		public virtual int GetPackInstinctBonus( Mobile attacker, Mobile defender )
		{
			if ( attacker.IsPlayer || defender.IsPlayer )
				return 0;

			BaseCreature bc = attacker as BaseCreature;

			if ( bc == null || bc.PackInstinct == PackInstinct.None || ( !bc.Controlled && !bc.Summoned ) )
				return 0;

			Mobile master = bc.ControlMaster;

			if ( master == null )
				master = bc.SummonMaster;

			if ( master == null )
				return 0;

			int inPack = 1;

			foreach ( Mobile m in defender.GetMobilesInRange( 1 ) )
			{
				if ( m != attacker && m is BaseCreature )
				{
					BaseCreature tc = (BaseCreature) m;

					if ( ( tc.PackInstinct & bc.PackInstinct ) == 0 || ( !tc.Controlled && !tc.Summoned ) )
						continue;

					Mobile theirMaster = tc.ControlMaster;

					if ( theirMaster == null )
						theirMaster = tc.SummonMaster;

					if ( master == theirMaster && tc.Combatant == defender )
						++inPack;
				}
			}

			if ( inPack >= 5 )
				return 100;
			else if ( inPack >= 4 )
				return 75;
			else if ( inPack >= 3 )
				return 50;
			else if ( inPack >= 2 )
				return 25;

			return 0;
		}

		public void OnHit( Mobile attacker, Mobile defender )
		{
			OnHit( attacker, defender, 1.0 );
		}

		public virtual void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			if ( MirrorImage.HasClone( defender ) && ( defender.Skills.Ninjitsu.Value / 150.0 ) > Utility.RandomDouble() )
			{
				Clone bc;

				foreach ( Mobile m in defender.GetMobilesInRange( 4 ) )
				{
					bc = m as Clone;

					if ( bc != null && bc.Summoned && bc.SummonMaster == defender )
					{
						attacker.SendLocalizedMessage( 1063141 ); // Your attack has been diverted to a nearby mirror image of your target!
						defender.SendLocalizedMessage( 1063140 ); // You manage to divert the attack onto one of your nearby mirror images.

						/*
						 * TODO: What happens if the Clone parries a blow?
						 * And what about if the attacker is using Honorable Execution
						 * and kills it?
						 */

						defender = m;
						break;
					}
				}
			}

			PlaySwingAnimation( attacker );
			PlayHurtAnimation( defender );

			attacker.PlaySound( GetHitAttackSound( attacker, defender ) );
			defender.PlaySound( GetHitDefendSound( attacker, defender ) );

			int damage = ComputeDamage( attacker, defender );

			#region Damage Multipliers
			/*
			 * The following damage bonuses multiply damage by a factor.
			 * Capped at x3 (+200%).
			 */
			int percentageBonus = 0;

			// ****** Special Moves and Weapon Abilities *******
			WeaponAbility a = WeaponAbility.GetCurrentAbility( attacker );

			if ( a != null )
				percentageBonus += (int) ( a.GetDamageScalar( attacker, defender ) * 100 ) - 100;

			SpecialMove move = SpecialMove.GetCurrentMove( attacker );

			if ( move != null )
				percentageBonus += (int) ( move.GetDamageScalar( attacker, defender ) * 100 ) - 100;

			// **************************************************

			percentageBonus += (int) ( damageBonus * 100 ) - 100;

			// **************** Slayer weapons ******************
			if ( defender is DemonKnight )
			{
				if ( Slayer != SlayerName.None )
					defender.FixedEffect( 0x37B9, 10, 5 );

				if ( Slayer == SlayerName.Demon || Slayer2 == SlayerName.Demon )
					percentageBonus += 100;

				if ( Slayer == SlayerName.Demon && ( Slayer2 == SlayerName.Elemental || Slayer2 == SlayerName.Undead || Slayer2 == SlayerName.PoisonElemental || Slayer2 == SlayerName.BloodElemental || Slayer2 == SlayerName.EarthElemental || Slayer2 == SlayerName.WaterElemental ) )
					percentageBonus += 100;

				if ( Slayer2 == SlayerName.Demon && ( Slayer == SlayerName.Elemental || Slayer == SlayerName.Undead || Slayer == SlayerName.PoisonElemental || Slayer == SlayerName.BloodElemental || Slayer == SlayerName.EarthElemental || Slayer == SlayerName.WaterElemental ) )
					percentageBonus += 100;
			}
			else
			{
				BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;

				SlayerEntry atkSlayer1 = SlayerGroup.GetEntryByName( atkWeapon.Slayer );
				SlayerEntry atkSlayer2 = SlayerGroup.GetEntryByName( atkWeapon.Slayer2 );

				List<SlayerEntry> slayers = new List<SlayerEntry>();

				if ( atkWeapon.Slayer != SlayerName.None )
					slayers.Add( atkSlayer1 );
				if ( atkWeapon.Slayer2 != SlayerName.None )
					slayers.Add( atkSlayer2 );

				BaseTalisman talisman = attacker.Talisman as BaseTalisman;

				if ( talisman != null )
				{
					SlayerEntry atkSlayer3 = SlayerGroup.GetEntryByName( talisman.Slayer );
					SlayerEntry atkSlayer4 = SlayerGroup.GetEntryByName( talisman.Slayer2 );

					if ( talisman.Slayer != SlayerName.None )
						slayers.Add( atkSlayer3 );
					if ( talisman.Slayer != SlayerName.None )
						slayers.Add( atkSlayer4 );
				}

				CheckSlayerResult cs = CheckSlayerResult.None;

				foreach ( SlayerEntry slayer in slayers.Distinct() )
				{
					if ( CheckSlayer( slayer, defender ) )
					{
						cs = SlayerGroup.IsSuperSlayer( slayer.Name ) ? CheckSlayerResult.GreaterSlayer : CheckSlayerResult.LesserSlayer;
						if ( cs == CheckSlayerResult.LesserSlayer )
							break;
					}
				}

				#region Opposite Slayer
				if ( defender.GetSlayerEntries().Any( e => e.Group.OppositionSuperSlays( attacker ) ) )
					cs = CheckSlayerResult.Opposition;
				#endregion

				if ( cs != CheckSlayerResult.None )
				{
					if ( cs != CheckSlayerResult.Opposition )
						defender.FixedEffect( 0x37B9, 10, 5 );

					percentageBonus += ( cs == CheckSlayerResult.LesserSlayer ) ? 200 : 100;
				}
			}

			if ( TalisSlayerEntry.IsSlayer( m_Slayer3, defender ) )
				percentageBonus += 100;
			// **************************************************

			if ( defender is BaseCreature )
			{
				BaseTalisman talis = BaseTalisman.GetTalisman( attacker );

				// ********* Talisman Kill Property *******
				if ( talis != null && talis.KillersTalis != NPC_Name.None && talis.KillersValue > 0 )
				{
					if ( ProtectionKillerEntry.IsProtectionKiller( talis.KillersTalis, defender ) )
						percentageBonus += talis.KillersValue;
				}
				// *****************************************

				// ************ Talisman Slayer ************
				if ( talis != null && talis.TalisSlayer != TalisSlayerName.None )
				{
					if ( TalisSlayerEntry.IsSlayer( talis.TalisSlayer, defender ) )
						percentageBonus += 100;
				}
				// ******************************************
			}

			if ( !attacker.IsPlayer )
			{
				var context = EnemyOfOneSpell.GetContext( defender );

				if ( context != null )
				{
					if ( context.TargetType != null && context.TargetType != attacker.GetType() )
						percentageBonus += 100;
				}
			}
			else if ( !defender.IsPlayer )
			{
				var context = EnemyOfOneSpell.GetContext( attacker );

				if ( context != null )
				{
					context.OnHit( defender );

					if ( context.TargetType == defender.GetType() )
					{
						defender.FixedEffect( 0x37B9, 10, 5, 1160, 0 );
						percentageBonus += context.DamageScalar;
					}
				}
			}

			ConsecrateContext consecrate = ConsecrateWeaponSpell.GetContext( attacker );

			if ( consecrate != null )
				percentageBonus += consecrate.BonusDamage; // +0~15%

			int packInstinctBonus = GetPackInstinctBonus( attacker, defender );

			if ( packInstinctBonus != 0 )
				percentageBonus += packInstinctBonus;

			TransformContext transform = TransformationSpell.GetContext( defender );

			if ( m_Slayer == SlayerName.Undead && transform != null && transform.Type != typeof( HorrificBeastSpell ) )
			{
				// Every necromancer transformation other than horrific beast take an additional 25% damage
				percentageBonus += 25;
			}

			if ( m_Slayer2 == SlayerName.Undead && transform != null && transform.Type != typeof( HorrificBeastSpell ) )
			{
				// Every necromancer transformation other than horrific beast take an additional 25% damage
				percentageBonus += 25;
			}

			if ( attacker is PlayerMobile )
			{
				PlayerMobile pmAttacker = (PlayerMobile) attacker;

				// Battle Lust: Damage bonus is 15% per opponent, with a cap of 45% in PvP and 90% in PvE
				percentageBonus += pmAttacker.GetBattleLust( defender );

				if ( !( defender is PlayerMobile ) )
				{
					if ( pmAttacker.HonorActive && pmAttacker.InRange( defender, 1 ) )
						percentageBonus += 25;

					if ( pmAttacker.SentHonorContext != null && pmAttacker.SentHonorContext.Target == defender )
						percentageBonus += pmAttacker.SentHonorContext.PerfectionDamageBonus;
				}
			}

			percentageBonus = Math.Min( percentageBonus, 200 );

			damage = AOS.Scale( damage, 100 + percentageBonus );

			if ( RunedSashOfWarding.IsUnderWardingEffect( defender, WardingEffect.WeaponDamage ) )
				damage = (int) ( damage * 0.9 );

			damage = (int) ( damage * GetDamageScalar( attacker, defender, GetMaxRange( attacker, defender ) ) );

			#endregion

			if ( attacker is BaseCreature )
				( (BaseCreature) attacker ).AlterMeleeDamageTo( defender, ref damage );

			if ( defender is BaseCreature )
				( (BaseCreature) defender ).AlterMeleeDamageFrom( attacker, ref damage );

			damage = AbsorbDamage( attacker, defender, damage );

			if ( damage == 0 ) // parried
			{
				if ( a != null && a.Validate( attacker ) /*&& a.CheckMana( attacker, true )*/ ) // Parried special moves have no mana cost 
				{
					a = null;
					WeaponAbility.ClearCurrentAbility( attacker );

					attacker.SendLocalizedMessage( 1061140 ); // Your attack was parried!
				}
				return;
			}

			#region Immolating Weapon
			if ( m_Immolating )
			{
				int d = ImmolatingWeaponSpell.GetImmolatingDamage( this );

				// As per Osi, hits slightly vary in damage counts.
				d = Utility.RandomMinMax( d - 2, d + 2 );
				AOS.Damage( defender, attacker, d, 0, 100, 0, 0, 0 );
			}
			#endregion

			AddBlood( attacker, defender, damage );

			int phys, fire, cold, pois, nrgy, chao;

			GetDamageTypes( attacker, out phys, out fire, out cold, out pois, out nrgy, out chao );

			#region Quivers
			Item cloak = attacker.FindItemOnLayer( Layer.Cloak );
			Item twohanded = attacker.FindItemOnLayer( Layer.TwoHanded );

			if ( cloak is BaseQuiver && twohanded is BaseRanged )
			{
				IQuiver quiver = cloak as IQuiver;

				int dmod = quiver.DamageModifier;

				if ( dmod > 0 )
				{
					int suma = quiver.FireDamage + quiver.PoisonDamage + quiver.ColdDamage + quiver.EnergyDamage + quiver.ChaosDamage;
					int quiverphys = quiver.PhysicalDamage;

					if ( suma < 100 )
						quiverphys = 100 - suma;

					phys -= (int) ( phys * dmod ) / 100;
					fire -= (int) ( fire * dmod ) / 100;
					cold -= (int) ( cold * dmod ) / 100;
					pois -= (int) ( pois * dmod ) / 100;
					nrgy -= (int) ( nrgy * dmod ) / 100;
					chao -= (int) ( chao * dmod ) / 100;

					phys += quiverphys * dmod / 100;
					fire += quiver.FireDamage * dmod / 100;
					cold += quiver.ColdDamage * dmod / 100;
					pois += quiver.PoisonDamage * dmod / 100;
					nrgy += quiver.EnergyDamage * dmod / 100;
					chao += quiver.ChaosDamage * dmod / 100;
				}
			}
			#endregion

			ConsecrateWeaponSpell.AlterDamageTypes( attacker, defender, ref phys, ref fire, ref cold, ref pois, ref nrgy, ref chao );

			int damageGiven = damage;

			if ( a != null && !a.OnBeforeDamage( attacker, defender ) )
			{
				WeaponAbility.ClearCurrentAbility( attacker );
				a = null;
			}

			if ( move != null && !move.OnBeforeDamage( attacker, defender ) )
			{
				SpecialMove.ClearCurrentMove( attacker );
				move = null;
			}

			if ( m_WeaponAttributes.Velocity > Utility.RandomMinMax( 1, 100 ) )
			{
				int damageVelocity = 3 * (int) attacker.GetDistanceToSqrt( defender );

				if ( damageVelocity > 30 )
					damageVelocity = 30;

				AOS.Damage( defender, attacker, damageVelocity, 100, 0, 0, 0, 0 );

				attacker.SendLocalizedMessage( 1072794 ); // Your arrow hits it's mark with velocity!
				defender.SendLocalizedMessage( 1072795 ); // You have been hit by an arrow with velocty!
			}

			if ( a != null )
				a.AlterDamageType( attacker, defender, ref phys, ref fire, ref cold, ref pois, ref nrgy );

			AOS.ArmorIgnore = ( a is ArmorIgnore || ( move != null && move.IgnoreArmor( attacker ) ) );
			AOS.ArmorPierce = ( a is ArmorPierce );

			damageGiven = AOS.Damage( true, defender, attacker, damage, phys, fire, cold, pois, nrgy, chao );

			AOS.ArmorIgnore = false;
			AOS.ArmorPierce = false;

			transform = TransformationSpell.GetContext( attacker );

			if ( transform != null && transform.Type == typeof( VampiricEmbraceSpell ) )
				attacker.Hits += (int) ( damageGiven * 0.2 );

			double propertyBonus = ( move == null ) ? 1.0 : move.GetPropertyBonus( attacker );

			#region Hit Leech
			int lifeLeech = 0;
			int stamLeech = 0;
			int manaLeech = 0;

			int lifeLeechProp = (int) ( m_WeaponAttributes.HitLeechHits * propertyBonus );
			int manaLeechProp = (int) ( m_WeaponAttributes.HitLeechMana * propertyBonus );

			lifeLeechProp = (int) ( ( lifeLeechProp / 2 ) * this.GetSpeed() * 0.25 );
			manaLeechProp = (int) ( ( manaLeechProp / 2 ) * this.GetSpeed() * 0.25 );

			if ( this is BaseRanged )
			{
				lifeLeechProp /= 2;
				manaLeechProp /= 2;
			}

			if ( lifeLeechProp > 100 )
				lifeLeechProp = 100;

			if ( manaLeechProp > 100 )
				manaLeechProp = 100;

			// Leech from 0% to HitLeechHits% of 30% of damage as hit points
			lifeLeech = AOS.Scale( 30, Utility.RandomMinMax( 0, lifeLeechProp ) );

			if ( m_Cursed )
			{
				// Additional 50% life leech for cursed weapons (necro spell)
				lifeLeech += 50;
			}

			if ( ( m_WeaponAttributes.HitLeechStam * propertyBonus ) > Utility.Random( 100 ) )
			{
				// HitLeechStam% chance to leech 100% of damage as stamina
				stamLeech += 100;
			}

			// Leech from 0% to HitLeechMana% of 40% of damage as mana
			manaLeech = AOS.Scale( 40, Utility.RandomMinMax( 0, manaLeechProp ) );

			if ( manaLeech != 0 )
			{
				attacker.Mana += AOS.Scale( damageGiven, manaLeech );
				Effects.SendPacket( defender, defender.Map, new TargetParticleEffect( defender, 0x3728, 1, 12, 0x786, 0x0, 0x2352, 255, 0x0 ) );
			}

			if ( lifeLeech != 0 )
			{
				attacker.Hits += AOS.Scale( damageGiven, lifeLeech );
				Effects.SendPacket( defender, defender.Map, new TargetParticleEffect( defender, 0x3728, 1, 12, 0x7AB, 0x0, 0x2352, 255, 0x0 ) );
			}

			if ( stamLeech != 0 )
				attacker.Stam += AOS.Scale( damageGiven, stamLeech );

			if ( lifeLeech != 0 || stamLeech != 0 || manaLeech != 0 )
				attacker.PlaySound( 0x44D );
			#endregion

			bool acidBlood = defender is Slime || defender is ToxicElemental || defender is UnderworldSlime || defender is AcidSlug;

			if ( m_MaxHits > 0 && ( ( MaxRange <= 1 && acidBlood ) || Utility.Random( 25 ) == 0 ) ) // Stratics says 50% chance, seems more like 4%..
			{
				if ( MaxRange <= 1 && acidBlood )
					attacker.LocalOverheadMessage( MessageType.Regular, 0x3B2, 500263 ); // *Splashing acid blood scars your weapon!*

				if ( m_WeaponAttributes.SelfRepair > Utility.Random( 10 ) )
				{
					HitPoints += 2;
				}
				else
				{
					if ( m_Hits > 0 )
					{
						--HitPoints;
					}
					else if ( m_MaxHits > 1 )
					{
						--MaxHitPoints;

						if ( Parent is Mobile )
							( (Mobile) Parent ).LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061121 ); // Your equipment is severely damaged.
					}
					else
					{
						Delete();
					}
				}
			}

			if ( attacker is VampireBatFamiliar )
			{
				BaseCreature bc = (BaseCreature) attacker;
				Mobile caster = bc.ControlMaster;

				if ( caster == null )
					caster = bc.SummonMaster;

				if ( caster != null && caster.Map == bc.Map && caster.InRange( bc, 2 ) )
					caster.Hits += damage;
				else
					bc.Hits += damage;
			}

			#region Hit Area Damage
			int physChance = (int) ( m_WeaponAttributes.HitPhysicalArea * propertyBonus );
			int fireChance = (int) ( m_WeaponAttributes.HitFireArea * propertyBonus );
			int coldChance = (int) ( m_WeaponAttributes.HitColdArea * propertyBonus );
			int poisChance = (int) ( m_WeaponAttributes.HitPoisonArea * propertyBonus );
			int nrgyChance = (int) ( m_WeaponAttributes.HitEnergyArea * propertyBonus );

			if ( physChance != 0 && physChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x10E, 50, 100, 0, 0, 0, 0 );

			if ( fireChance != 0 && fireChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x11D, 1160, 0, 100, 0, 0, 0 );

			if ( coldChance != 0 && coldChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x0FC, 2100, 0, 0, 100, 0, 0 );

			if ( poisChance != 0 && poisChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x205, 1166, 0, 0, 0, 100, 0 );

			if ( nrgyChance != 0 && nrgyChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x1F1, 120, 0, 0, 0, 0, 100 );
			#endregion

			#region Hit Spell
			int maChance = (int) ( m_WeaponAttributes.HitMagicArrow * propertyBonus );
			int harmChance = (int) ( m_WeaponAttributes.HitHarm * propertyBonus );
			int fireballChance = (int) ( m_WeaponAttributes.HitFireball * propertyBonus );
			int lightningChance = (int) ( m_WeaponAttributes.HitLightning * propertyBonus );
			int dispelChance = (int) ( m_WeaponAttributes.HitDispel * propertyBonus );

			if ( maChance != 0 && maChance > Utility.Random( 100 ) )
				DoMagicArrow( attacker, defender );

			if ( harmChance != 0 && harmChance > Utility.Random( 100 ) )
				DoHarm( attacker, defender );

			if ( fireballChance != 0 && fireballChance > Utility.Random( 100 ) )
				DoFireball( attacker, defender );

			if ( lightningChance != 0 && lightningChance > Utility.Random( 100 ) )
				DoLightning( attacker, defender );

			if ( dispelChance != 0 && dispelChance > Utility.Random( 100 ) )
				DoDispel( attacker, defender );
			#endregion

			int curseChance = m_WeaponAttributes.HitCurse;

			if ( curseChance != 0 && curseChance > Utility.Random( 100 ) )
				DoCurse( attacker, defender );

			#region Hit Lower Attack/Defend
			int laChance = (int) ( m_WeaponAttributes.HitLowerAttack * propertyBonus );
			int ldChance = (int) ( m_WeaponAttributes.HitLowerDefend * propertyBonus );

			ElvenGlasses eg = attacker.FindItemOnLayer( Layer.Helm ) as ElvenGlasses;
			if ( eg != null )
				ldChance += eg.HitLowerDefend;

			if ( laChance != 0 && laChance > Utility.Random( 100 ) )
				DoLowerAttack( attacker, defender );

			if ( ldChance != 0 && ldChance > Utility.Random( 100 ) )
				DoLowerDefense( attacker, defender );
			#endregion

			if ( attacker is BaseCreature )
				( (BaseCreature) attacker ).OnGaveMeleeAttack( defender );

			if ( defender is BaseCreature )
				( (BaseCreature) defender ).OnGotMeleeAttack( attacker );

			if ( a != null )
				a.OnHit( attacker, defender, damage );

			if ( move != null )
				move.OnHit( attacker, defender, damage );

			if ( defender is IHonorTarget && ( (IHonorTarget) defender ).ReceivedHonorContext != null )
				( (IHonorTarget) defender ).ReceivedHonorContext.OnTargetHit( attacker );

			if ( !( this is BaseRanged ) )
			{
				if ( AnimalForm.UnderTransformation( attacker, typeof( GiantSerpent ) ) )
					defender.ApplyPoison( attacker, Poison.Lesser );

				if ( AnimalForm.UnderTransformation( defender, typeof( BullFrog ) ) )
					attacker.ApplyPoison( defender, Poison.Regular );
			}
		}

		public virtual double GetAosDamage( Mobile attacker, int min, int random, double div )
		{
			double scale = 1.0;

			scale += attacker.Skills[SkillName.Inscribe].Value * 0.001;

			if ( attacker.IsPlayer )
			{
				scale += attacker.Int * 0.001;
				scale += SpellHelper.GetSpellDamage( attacker, false ) * 0.01;
			}

			int baseDamage = min;

			double damage = Utility.RandomMinMax( baseDamage, baseDamage + random );

			return damage * scale;
		}

		public virtual void DoMagicArrow( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
			{
				return;
			}

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 10, 4, 10.0 );

			attacker.MovingParticles( defender, 0x36E4, 5, 0, false, true, 3006, 4006, 0 );
			attacker.PlaySound( 0x1E5 );

			SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, damage, 0, 100, 0, 0, 0 );
		}

		public virtual void DoHarm( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
			{
				return;
			}

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 17, 5, 6.5 );

			if ( !defender.InRange( attacker, 2 ) )
			{
				// 1/4 damage at > 2 tile range
				damage *= 0.25;
			}
			else if ( !defender.InRange( attacker, 1 ) )
			{
				// 1/2 damage at 2 tile range
				damage *= 0.50;
			}

			defender.FixedParticles( 0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist );
			defender.PlaySound( 0x0FC );

			SpellHelper.Damage( TimeSpan.Zero, defender, attacker, damage, 0, 0, 100, 0, 0 );
		}

		public virtual void DoFireball( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
			{
				return;
			}

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 19, 5, 5.5 );

			attacker.MovingParticles( defender, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160 );
			attacker.PlaySound( 0x15E );

			SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, damage, 0, 100, 0, 0, 0 );
		}

		public virtual void DoLightning( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
			{
				return;
			}

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 23, 5, 5.0 );

			defender.BoltEffect( 0 );

			SpellHelper.Damage( TimeSpan.Zero, defender, attacker, damage, 0, 0, 0, 0, 100 );
		}

		public virtual void DoDispel( Mobile attacker, Mobile defender )
		{
			bool dispellable = false;

			if ( defender is BaseCreature )
				dispellable = ( (BaseCreature) defender ).Summoned && !( (BaseCreature) defender ).IsAnimatedDead;

			if ( !dispellable )
				return;

			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			Spells.MagerySpell sp = new Spells.Sixth.DispelSpell( attacker, null );

			if ( sp.CheckResisted( defender ) )
			{
				defender.FixedEffect( 0x3779, 10, 20 );
			}
			else
			{
				Effects.SendLocationParticles( EffectItem.Create( defender.Location, defender.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
				Effects.PlaySound( defender, defender.Map, 0x201 );

				defender.Delete();
			}
		}

		public virtual void DoCurse( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			Spells.Fourth.CurseSpell sp = new Spells.Fourth.CurseSpell( attacker, null );

			sp.DoCurse( defender );

			attacker.SendLocalizedMessage( 1113717 ); // You have hit your target with a curse effect.
			defender.SendLocalizedMessage( 1113718 ); // You have been hit with a curse effect.
		}

		public virtual void DoLowerAttack( Mobile from, Mobile defender )
		{
			HitLower.ApplyAttack( defender );

			defender.PlaySound( 0x28E );
			Effects.SendTargetEffect( defender, 0x37BE, 1, 4, 0xA, 3 );
		}

		public virtual void DoLowerDefense( Mobile from, Mobile defender )
		{
			HitLower.ApplyDefense( defender );

			defender.PlaySound( 0x28E );
			Effects.SendTargetEffect( defender, 0x37BE, 1, 4, 0x23, 3 );
		}

		public virtual void DoAreaAttack( Mobile from, Mobile defender, int sound, int hue, int phys, int fire, int cold, int pois, int nrgy )
		{
			Map map = from.Map;

			if ( map == null )
				return;

			ArrayList list = new ArrayList();

			foreach ( Mobile m in from.GetMobilesInRange( 10 ) )
			{
				if ( from != m && defender != m && SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false ) && from.InLOS( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
				return;

			Effects.PlaySound( from.Location, map, sound );

			// TODO: What is the damage calculation?

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = (Mobile) list[i];

				double scalar = ( 11 - from.GetDistanceToSqrt( m ) ) / 10;

				if ( scalar > 1.0 )
					scalar = 1.0;
				else if ( scalar < 0.0 )
					continue;

				from.DoHarmful( m, true );
				m.FixedEffect( 0x3779, 1, 15, hue, 0 );
				AOS.Damage( m, from, (int) ( GetBaseDamage( from ) * scalar ), phys, fire, cold, pois, nrgy );
			}
		}

		public virtual bool CheckSlayer( SlayerEntry atkSlayer, Mobile defender )
		{
			return atkSlayer != null && atkSlayer.Slays( defender );
		}

		public virtual void AddBlood( Mobile attacker, Mobile defender, int damage )
		{
			if ( damage > 0 )
			{
				new Blood().MoveToWorld( defender.Location, defender.Map );

				if ( Utility.RandomBool() )
				{
					new Blood().MoveToWorld( new Point3D( defender.X + Utility.RandomMinMax( -1, 1 ), defender.Y + Utility.RandomMinMax( -1, 1 ), defender.Z ), defender.Map );
				}
			}

			/* if ( damage <= 2 )
				return;

			Direction d = defender.GetDirectionTo( attacker );

			int maxCount = damage / 15;

			if ( maxCount < 1 )
				maxCount = 1;
			else if ( maxCount > 4 )
				maxCount = 4;

			for( int i = 0; i < Utility.Random( 1, maxCount ); ++i )
			{
				int x = defender.X;
				int y = defender.Y;

				switch( d )
				{
					case Direction.North:
						x += Utility.Random( -1, 3 );
						y += Utility.Random( 2 );
						break;
					case Direction.East:
						y += Utility.Random( -1, 3 );
						x += Utility.Random( -1, 2 );
						break;
					case Direction.West:
						y += Utility.Random( -1, 3 );
						x += Utility.Random( 2 );
						break;
					case Direction.South:
						x += Utility.Random( -1, 3 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Up:
						x += Utility.Random( 2 );
						y += Utility.Random( 2 );
						break;
					case Direction.Down:
						x += Utility.Random( -1, 2 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Left:
						x += Utility.Random( 2 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Right:
						x += Utility.Random( -1, 2 );
						y += Utility.Random( 2 );
						break;
				}

				new Blood().MoveToWorld( new Point3D( x, y, defender.Z ), defender.Map );
			}*/
		}

		public virtual void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			if ( wielder is BaseCreature )
			{
				BaseCreature bc = (BaseCreature) wielder;

				phys = bc.PhysicalDamage;
				fire = bc.FireDamage;
				cold = bc.ColdDamage;
				pois = bc.PoisonDamage;
				nrgy = bc.EnergyDamage;
			}
			else
			{
				fire = WeaponAttributes.DamageFirePercent;
				cold = WeaponAttributes.DamageColdPercent;
				pois = WeaponAttributes.DamagePoisonPercent;
				nrgy = WeaponAttributes.DamageEnergyPercent;
				phys = 100 - ( fire + cold + pois + nrgy );
			}
		}

		public virtual void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chao )
		{
			chao = 0;
			GetDamageTypes( wielder, out phys, out fire, out cold, out pois, out nrgy );
		}

		public virtual void OnMiss( Mobile attacker, Mobile defender )
		{
			PlaySwingAnimation( attacker );
			attacker.PlaySound( GetMissAttackSound( attacker, defender ) );
			defender.PlaySound( GetMissDefendSound( attacker, defender ) );

			WeaponAbility ability = WeaponAbility.GetCurrentAbility( attacker );

			if ( ability != null )
				ability.OnMiss( attacker, defender );

			SpecialMove move = SpecialMove.GetCurrentMove( attacker );

			if ( move != null )
				move.OnMiss( attacker, defender );

			if ( defender is IHonorTarget && ( (IHonorTarget) defender ).ReceivedHonorContext != null )
				( (IHonorTarget) defender ).ReceivedHonorContext.OnTargetMissed( attacker );
		}

		public virtual void GetBaseDamageRange( Mobile attacker, out int min, out int max )
		{
			if ( attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature) attacker;

				if ( c.DamageMin >= 0 )
				{
					min = c.DamageMin;
					max = c.DamageMax;
					return;
				}

				if ( this is Fists && !attacker.Body.IsHuman )
				{
					min = attacker.Str / 28;
					max = attacker.Str / 28;
					return;
				}
			}

			min = MinDamage;
			max = MaxDamage;
		}

		public virtual double GetBaseDamage( Mobile attacker )
		{
			int min, max;

			GetBaseDamageRange( attacker, out min, out max );

			return Utility.RandomMinMax( min, max );
		}

		public virtual double GetBonus( double value, double scalar, double threshold, double offset )
		{
			double bonus = value * scalar;

			if ( value >= threshold )
			{
				bonus += offset;
			}

			return bonus / 100;
		}

		public virtual int GetCastSpeedBonus()
		{
			int bonus = 0;

			if ( Attributes.SpellChanneling != 0 )
				bonus -= 1;

			return bonus;
		}

		public virtual int GetAttributeBonus( MagicalAttribute attr )
		{
			int value = 0;

			switch ( attr )
			{
				case MagicalAttribute.CastSpeed:
					value += GetCastSpeedBonus();
					break;
			}

			return value;
		}

		public virtual void GetStatusDamage( Mobile from, out int min, out int max )
		{
			int baseMin, baseMax;

			GetBaseDamageRange( from, out baseMin, out baseMax );

			min = (int) ScaleDamage( from, baseMin, false );
			max = (int) ScaleDamage( from, baseMax, false );

			if ( min < 1 )
				min = 1;

			if ( max < 1 )
				max = 1;
		}

		public virtual double ScaleDamage( Mobile attacker, double damage, bool checkSkills )
		{
			if ( checkSkills )
			{
				attacker.CheckSkill( SkillName.Tactics, 0.0, attacker.Skills[SkillName.Tactics].Cap ); // Passively check tactics for gain
				attacker.CheckSkill( SkillName.Anatomy, 0.0, attacker.Skills[SkillName.Anatomy].Cap ); // Passively check Anatomy for gain

				if ( Type == WeaponType.Axe )
					attacker.CheckSkill( SkillName.Lumberjacking, 0.0, 100.0 ); // Passively check Lumberjacking for gain
			}

			#region Physical bonuses
			/*
			 * These are the bonuses given by the physical characteristics of the mobile.
			 * No caps apply.
			 */
			double strengthBonus = GetBonus( attacker.Str, 0.300, 100.0, 5.00 );
			double anatomyBonus = GetBonus( attacker.Skills[SkillName.Anatomy].Value, 0.500, 100.0, 5.00 );
			double tacticsBonus = GetBonus( attacker.Skills[SkillName.Tactics].Value, 0.625, 100.0, 6.25 );
			double lumberBonus = GetBonus( attacker.Skills[SkillName.Lumberjacking].Value, 0.200, 100.0, 0.05 > Utility.RandomDouble() ? 100.00 : 10.0 );

			if ( Type != WeaponType.Axe )
				lumberBonus = 0.0;
			#endregion

			#region Modifiers
			/*
			 * The following are damage modifiers whose effect shows on the status bar.
			 * Capped at 100% total.
			 */
			int damageBonus = attacker.GetMagicalAttribute( MagicalAttribute.WeaponDamage );

			// Stone Form gives a +0% to +20% bonus to damage.
			if ( StoneFormSpell.UnderEffect( attacker ) )
				damageBonus += StoneFormSpell.GetDIBonus( attacker );

			// Horrific Beast transformation gives a +25% bonus to damage.
			if ( TransformationSpell.UnderTransformation( attacker, typeof( HorrificBeastSpell ) ) )
				damageBonus += 25;

			// Divine Fury gives a +10% bonus to damage.
			if ( Spells.Chivalry.DivineFurySpell.UnderEffect( attacker ) )
				damageBonus += 10;

			// TODO: Defense Mastery bonus should go here.

			int discordanceEffect = 0;

			// Discordance gives a -2%/-48% malus to damage.
			if ( SkillHandlers.Discordance.GetEffect( attacker, ref discordanceEffect ) )
				damageBonus -= discordanceEffect * 2;

			// Grapes of Wrath gives a +35% bonus to damage.
			if ( GrapesOfWrath.IsUnderInfluence( attacker ) )
				damageBonus += 35;

			if ( attacker is PlayerMobile && ( (PlayerMobile) attacker ).Berserk )
				damageBonus += Math.Min( 60, 15 * (int) ( ( (float) ( attacker.HitsMax - attacker.Hits ) / attacker.HitsMax ) * 5.0 ) );

			if ( damageBonus > 100 )
				damageBonus = 100;
			#endregion

			double totalBonus = strengthBonus + anatomyBonus + tacticsBonus + lumberBonus + ( (double) damageBonus / 100 );

			return damage + (int) ( damage * totalBonus );
		}

		public virtual int ScaleDamageByDurability( int damage )
		{
			int scale = 100;

			if ( m_MaxHits > 0 && m_Hits < m_MaxHits )
				scale = 50 + ( ( 50 * m_Hits ) / m_MaxHits );

			return AOS.Scale( damage, scale );
		}

		public virtual int ComputeDamage( Mobile attacker, Mobile defender )
		{
			return (int) ScaleDamage( attacker, GetBaseDamage( attacker ), true );
		}

		public virtual void PlayHurtAnimation( Mobile from )
		{
			if ( from.Mounted )
				return;

			from.Animate( 4 );
		}

		public virtual void PlaySwingAnimation( Mobile from )
		{
			from.Animate( 0, (int) Animation );
		}

		private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
		{
			if ( setIf )
				flags |= toSet;
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( ( flags & toGet ) != 0 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 17 ); // version

			writer.Write( (int) m_GorgonCharges );

			if ( m_GorgonCharges > 0 )
				writer.Write( (int) m_GorgonQuality );

			if ( m_EnchantContext != null )
			{
				writer.Write( (bool) true );
				m_EnchantContext.Serialize( writer );
			}
			else
				writer.Write( (bool) false );

			writer.Write( (int) m_TimesImbued );

			writer.Write( (int) m_Slayer3 );

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.Exceptional, m_Exceptional != false );
			SetSaveFlag( ref flags, SaveFlag.Hits, m_Hits != 0 );
			SetSaveFlag( ref flags, SaveFlag.MaxHits, m_MaxHits != 0 );
			SetSaveFlag( ref flags, SaveFlag.Slayer, m_Slayer != SlayerName.None );
			SetSaveFlag( ref flags, SaveFlag.Poison, m_Poison != null );
			SetSaveFlag( ref flags, SaveFlag.Crafter, m_Crafter != null );
			SetSaveFlag( ref flags, SaveFlag.PoisonCharges, m_PoisonCharges != 0 );
			SetSaveFlag( ref flags, SaveFlag.Resource, m_Resource != CraftResource.Iron );
			SetSaveFlag( ref flags, SaveFlag.xAttributes, !m_MagicalAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.xWeaponAttributes, !m_WeaponAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed );
			SetSaveFlag( ref flags, SaveFlag.SkillBonuses, !m_SkillBonuses.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Resistances, !m_Resistances.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.AbsorptionAttributes, !m_AbsorptionAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Slayer2, m_Slayer2 != SlayerName.None );
			SetSaveFlag( ref flags, SaveFlag.EngravedText, !String.IsNullOrEmpty( m_EngravedText ) );
			SetSaveFlag( ref flags, SaveFlag.Altered, m_Altered );

			writer.Write( (long) flags );

			if ( GetSaveFlag( flags, SaveFlag.Hits ) )
				writer.Write( (int) m_Hits );

			if ( GetSaveFlag( flags, SaveFlag.MaxHits ) )
				writer.Write( (int) m_MaxHits );

			if ( GetSaveFlag( flags, SaveFlag.Slayer ) )
				writer.Write( (int) m_Slayer );

			if ( GetSaveFlag( flags, SaveFlag.Poison ) )
				Poison.Serialize( m_Poison, writer );

			if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
				writer.Write( (Mobile) m_Crafter );

			if ( GetSaveFlag( flags, SaveFlag.PoisonCharges ) )
				writer.Write( (int) m_PoisonCharges );

			if ( GetSaveFlag( flags, SaveFlag.Resource ) )
				writer.Write( (int) m_Resource );

			if ( GetSaveFlag( flags, SaveFlag.xAttributes ) )
				m_MagicalAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.xWeaponAttributes ) )
				m_WeaponAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
				m_SkillBonuses.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
				m_Resistances.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
				m_AbsorptionAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Slayer2 ) )
				writer.Write( (int) m_Slayer2 );

			if ( GetSaveFlag( flags, SaveFlag.EngravedText ) )
				writer.Write( (string) m_EngravedText );
		}

		[Flags]
		private enum SaveFlag : long
		{
			None = 0x00000000,
			//Unused = 0x00000001,
			//Unused = 0x00000002,
			//Unused = 0x00000004,
			Exceptional = 0x00000008,
			Hits = 0x00000010,
			MaxHits = 0x00000020,
			Slayer = 0x00000040,
			Poison = 0x00000080,
			PoisonCharges = 0x00000100,
			Crafter = 0x00000200,
			//Unused = 0x00000400,
			//Unused = 0x00000800,
			//Unused = 0x00001000,
			//Unused = 0x00002000,
			//Unused = 0x00004000,
			//Unused = 0x00008000,
			//Unused = 0x00010000,
			//Unused = 0x00020000,
			//Unused = 0x00040000,
			//Unused = 0x00080000,
			//Unused = 0x00100000,
			//Unused = 0x00200000,
			//Unused = 0x00400000,
			Resource = 0x00800000,
			xAttributes = 0x01000000,
			xWeaponAttributes = 0x02000000,
			PlayerConstructed = 0x04000000,
			SkillBonuses = 0x08000000,
			Slayer2 = 0x10000000,
			//Unused = 0x20000000,
			EngravedText = 0x40000000,
			Resistances = 0x80000000,
			Altered = 0x100000000,
			AbsorptionAttributes = 0x200000000
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 17:
				case 16:
					{
						m_GorgonCharges = reader.ReadInt();

						if ( m_GorgonCharges > 0 )
							m_GorgonQuality = (GorgonQuality) reader.ReadInt();

						if ( reader.ReadBool() )
							EnchantContext = new EnchantContext( reader, this );

						m_TimesImbued = reader.ReadInt();

						m_Slayer3 = (TalisSlayerName) reader.ReadInt();

						SaveFlag flags = (SaveFlag) reader.ReadLong();

						if ( GetSaveFlag( flags, SaveFlag.Exceptional ) )
							m_Exceptional = true;

						if ( GetSaveFlag( flags, SaveFlag.Hits ) )
							m_Hits = reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.MaxHits ) )
							m_MaxHits = reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.Slayer ) )
							m_Slayer = (SlayerName) reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.Poison ) )
							m_Poison = Poison.Deserialize( reader );

						if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
							m_Crafter = reader.ReadMobile();

						if ( GetSaveFlag( flags, SaveFlag.PoisonCharges ) )
							m_PoisonCharges = reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.Resource ) )
							m_Resource = (CraftResource) reader.ReadInt();
						else
							m_Resource = CraftResource.Iron;

						if ( GetSaveFlag( flags, SaveFlag.xAttributes ) )
							m_MagicalAttributes = new MagicalAttributes( this, reader );
						else
							m_MagicalAttributes = new MagicalAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.xWeaponAttributes ) )
							m_WeaponAttributes = new WeaponAttributes( this, reader );
						else
							m_WeaponAttributes = new WeaponAttributes( this );

						if ( version < 7 && m_WeaponAttributes.MageWeapon != 0 )
							m_WeaponAttributes.MageWeapon = (short) ( 30 - m_WeaponAttributes.MageWeapon );

						if ( m_WeaponAttributes.MageWeapon != 0 && m_WeaponAttributes.MageWeapon != 30 && Parent is Mobile )
						{
							m_MageMod = new DefaultSkillMod( SkillName.Magery, true, -30 + m_WeaponAttributes.MageWeapon );
							( (Mobile) Parent ).AddSkillMod( m_MageMod );
						}

						if ( GetSaveFlag( flags, SaveFlag.PlayerConstructed ) )
							m_PlayerConstructed = true;

						if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
							m_SkillBonuses = new SkillBonuses( this, reader );
						else
							m_SkillBonuses = new SkillBonuses( this );

						if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
							m_Resistances = new ElementAttributes( this, reader );
						else
							m_Resistances = new ElementAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
							m_AbsorptionAttributes = new AbsorptionAttributes( this, reader );
						else
							m_AbsorptionAttributes = new AbsorptionAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.Slayer2 ) )
							m_Slayer2 = (SlayerName) reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.EngravedText ) )
							m_EngravedText = reader.ReadString();

						if ( GetSaveFlag( flags, SaveFlag.Altered ) )
							m_Altered = true;

						break;
					}
			}

			if ( Parent is Mobile )
				m_SkillBonuses.AddTo( (Mobile) Parent );

			int strBonus = Attributes.BonusStr;
			int dexBonus = Attributes.BonusDex;
			int intBonus = Attributes.BonusInt;

			if ( this.Parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 ) )
			{
				Mobile m = (Mobile) this.Parent;

				string modName = this.Serial.ToString();

				if ( strBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

				if ( dexBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

				if ( intBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}

			if ( Parent is Mobile )
				( (Mobile) Parent ).CheckStatTimers();

			if ( m_Hits <= 0 && m_MaxHits <= 0 )
				m_Hits = m_MaxHits = Utility.RandomMinMax( InitMinHits, InitMaxHits );

			if ( MaxHitPoints > 255 )
				MaxHitPoints = 255;
		}

		public BaseWeapon( int itemID )
			: base( itemID )
		{
			Layer = (Layer) ItemData.Quality;

			m_Hits = m_MaxHits = Utility.RandomMinMax( InitMinHits, InitMaxHits );

			m_Resource = CraftResource.Iron;

			m_MagicalAttributes = new MagicalAttributes( this );
			m_WeaponAttributes = new WeaponAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );
			m_Resistances = new ElementAttributes( this );
			m_AbsorptionAttributes = new AbsorptionAttributes( this );
		}

		public BaseWeapon( Serial serial )
			: base( serial )
		{
		}

		private string GetNameString()
		{
			string name = this.Name;

			if ( name == null )
				name = String.Format( "#{0}", LabelNumber );

			return name;
		}

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public override int Hue
		{
			get { return base.Hue; }
			set
			{
				base.Hue = value;
				InvalidateProperties();
			}
		}

		public int GetElementalDamageHue()
		{
			int phys, fire, cold, pois, nrgy;
			GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy );

			int currentMax = 50;
			int hue = 0;

			if ( pois >= currentMax )
			{
				hue = 1267 + ( pois - 50 ) / 10;
				currentMax = pois;
			}

			if ( fire >= currentMax )
			{
				hue = 1255 + ( fire - 50 ) / 10;
				currentMax = fire;
			}

			if ( nrgy >= currentMax )
			{
				hue = 1273 + ( nrgy - 50 ) / 10;
				currentMax = nrgy;
			}

			if ( cold >= currentMax )
			{
				hue = 1261 + ( cold - 50 ) / 10;
				currentMax = cold;
			}

			return hue;
		}

		public override LocalizedText GetNameProperty()
		{
			int oreType;

			switch ( m_Resource )
			{
				case CraftResource.DullCopper:
					oreType = 1053108;
					break; // dull copper
				case CraftResource.ShadowIron:
					oreType = 1053107;
					break; // shadow iron
				case CraftResource.Copper:
					oreType = 1053106;
					break; // copper
				case CraftResource.Bronze:
					oreType = 1053105;
					break; // bronze
				case CraftResource.Gold:
					oreType = 1053104;
					break; // golden
				case CraftResource.Agapite:
					oreType = 1053103;
					break; // agapite
				case CraftResource.Verite:
					oreType = 1053102;
					break; // verite
				case CraftResource.Valorite:
					oreType = 1053101;
					break; // valorite
				case CraftResource.SpinedLeather:
					oreType = 1061118;
					break; // spined
				case CraftResource.HornedLeather:
					oreType = 1061117;
					break; // horned
				case CraftResource.BarbedLeather:
					oreType = 1061116;
					break; // barbed
				case CraftResource.RedScales:
					oreType = 1060814;
					break; // red
				case CraftResource.YellowScales:
					oreType = 1060818;
					break; // yellow
				case CraftResource.BlackScales:
					oreType = 1060820;
					break; // black
				case CraftResource.GreenScales:
					oreType = 1060819;
					break; // green
				case CraftResource.WhiteScales:
					oreType = 1060821;
					break; // white
				case CraftResource.BlueScales:
					oreType = 1060815;
					break; // blue
				default:
					oreType = 0;
					break;
			}

			if ( oreType != 0 )
				return new LocalizedText( 1053099, "#{0}\t{1}", oreType, GetNameString() ); // ~1_oretype~ ~2_armortype~
			else if ( Name == null )
				return new LocalizedText( LabelNumber );
			else
				return new LocalizedText( Name );
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( base.AllowEquipedCast( from ) )
				return true;

			return ( Attributes.SpellChanneling != 0 );
		}

		public virtual int ArtifactRarity { get { return 0; } }

		public virtual void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
		}

		public virtual void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
		}

		public virtual void AddRangeProperty( ObjectPropertyList list )
		{
			if ( MaxRange > 1 )
				list.Add( 1061169, MaxRange.ToString() ); // range ~1_val~
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !string.IsNullOrEmpty( m_EngravedText ) )
				list.Add( 1062613, m_EngravedText );

			if ( m_Exceptional )
				list.Add( 1060636 ); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_TimesImbued > 0 )
				list.Add( 1080418 ); // (Imbued)

			if ( m_Altered )
				list.Add( 1111880 ); // Altered

			#region Factions
			if ( m_FactionState != null )
				list.Add( 1041350 ); // faction item
			#endregion

			if ( m_GorgonCharges > 0 )
				list.Add( 1112590, m_GorgonCharges.ToString() ); // Gorgon Lens Charges: ~1_val~

			GetSetArmorPropertiesFirst( list );

			if ( m_SkillBonuses != null )
				m_SkillBonuses.GetProperties( list );

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~

			if ( this is IUsesRemaining && ( (IUsesRemaining) this ).ShowUsesRemaining )
				list.Add( 1060584, ( (IUsesRemaining) this ).UsesRemaining.ToString() ); // uses remaining: ~1_val~

			if ( m_Poison != null && m_PoisonCharges > 0 )
			{
				if ( m_Poison.Name == "Darkglow" )
					list.Add( 1072853, m_PoisonCharges.ToString() ); // darkglow poison charges: ~1_val~
				else if ( m_Poison.Name == "Parasitic" )
					list.Add( 1072852, m_PoisonCharges.ToString() ); // parasitic poison charges: ~1_val~
				else
					list.Add( 1062412 + m_Poison.Level, m_PoisonCharges.ToString() );
			}

			if ( m_Slayer != SlayerName.None )
				list.Add( SlayerGroup.GetEntryByName( m_Slayer ).Title );

			if ( m_Slayer2 != SlayerName.None )
				list.Add( SlayerGroup.GetEntryByName( m_Slayer2 ).Title );

			if ( m_Slayer3 != TalisSlayerName.None )
				list.Add( TalisSlayerEntry.GetSlayerTitle( m_Slayer3 ) );

			int woodType;

			if ( Hue == 0 )
				woodType = 0;
			else
			{
				switch ( m_Resource )
				{
					case CraftResource.Oak:
						woodType = 1072533;
						break; // Oak
					case CraftResource.Ash:
						woodType = 1072534;
						break; // Ash
					case CraftResource.Yew:
						woodType = 1072535;
						break; // Yew
					case CraftResource.Heartwood:
						woodType = 1072536;
						break; // Heartwood
					case CraftResource.Bloodwood:
						woodType = 1072538;
						break; // Bloodwood
					case CraftResource.Frostwood:
						woodType = 1072539;
						break; // Frostwood
					default:
						woodType = 0;
						break;
				}
			}

			if ( woodType != 0 )
				list.Add( woodType );

			if ( m_Immolating )
				list.Add( 1111917 ); // Immolated

			if ( Enchanted )
				list.Add( 1080125 ); // Enchanted

			int prop;

			if ( ( prop = WeaponAttributes.BloodDrinker ) != 0 )
				list.Add( 1113591 ); // Blood Drinker

			if ( ( prop = WeaponAttributes.BattleLust ) != 0 )
				list.Add( 1113710 ); // Battle Lust

			if ( ( prop = WeaponAttributes.Balanced ) != 0 )
				list.Add( 1072792 ); // Balanced

			if ( ( prop = WeaponAttributes.Velocity ) != 0 )
				list.Add( 1072793, prop.ToString() ); // Velocity ~1_val~%

			if ( ( prop = Attributes.LowerAmmoCost ) != 0 )
				list.Add( 1075208, prop.ToString() ); // Lower Ammo Cost ~1_Percentage~%

			base.AddResistanceProperties( list );

			if ( ( prop = WeaponAttributes.UseBestSkill ) != 0 )
				list.Add( 1060400 ); // use best weapon skill

			if ( ( prop = Attributes.WeaponDamage ) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( ( prop = Attributes.DefendChance ) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( ( prop = Attributes.BonusDex ) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( ( prop = Attributes.EnhancePotions ) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( ( prop = Attributes.CastRecovery ) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( ( prop = ( GetCastSpeedBonus() + Attributes.CastSpeed ) ) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( ( prop = Attributes.AttackChance ) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			double propertyBonus = 1.0;
			Mobile wielder = this.Parent as Mobile;

			if ( wielder != null )
			{
				SpecialMove move = SpecialMove.GetCurrentMove( wielder );

				if ( move != null )
					propertyBonus = move.GetPropertyBonus( wielder );
			}

			if ( ( prop = (int) ( WeaponAttributes.HitColdArea * propertyBonus ) ) != 0 )
				list.Add( 1060416, prop.ToString() ); // hit cold area ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitDispel * propertyBonus ) ) != 0 )
				list.Add( 1060417, prop.ToString() ); // hit dispel ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitEnergyArea * propertyBonus ) ) != 0 )
				list.Add( 1060418, prop.ToString() ); // hit energy area ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitFireArea * propertyBonus ) ) != 0 )
				list.Add( 1060419, prop.ToString() ); // hit fire area ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitFireball * propertyBonus ) ) != 0 )
				list.Add( 1060420, prop.ToString() ); // hit fireball ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitHarm * propertyBonus ) ) != 0 )
				list.Add( 1060421, prop.ToString() ); // hit harm ~1_val~%

			if ( ( prop = WeaponAttributes.HitCurse ) != 0 )
				list.Add( 1113712, prop.ToString() ); // Hit Curse ~1_val~%

			if ( ( prop = WeaponAttributes.HitFatigue ) != 0 )
				list.Add( 1113700, prop.ToString() ); // Hit Fatigue ~1_val~%

			if ( ( prop = WeaponAttributes.HitManaDrain ) != 0 )
				list.Add( 1113699, prop.ToString() ); // Hit Mana Drain ~1_val~%

			if ( ( prop = WeaponAttributes.SplinteringWeapon ) != 0 )
				list.Add( 1112857, prop.ToString() ); // splintering weapon ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitLeechHits * propertyBonus ) ) != 0 )
			{
				prop = (int) ( ( prop / 2 ) * this.GetSpeed() * 0.25 );

				if ( prop > 100 )
					prop = 100;

				if ( this is BaseRanged )
					prop /= 2;

				list.Add( 1060422, prop.ToString() ); // hit life leech ~1_val~%
			}

			if ( ( prop = (int) ( WeaponAttributes.HitLightning * propertyBonus ) ) != 0 )
				list.Add( 1060423, prop.ToString() ); // hit lightning ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitLowerAttack * propertyBonus ) ) != 0 )
				list.Add( 1060424, prop.ToString() ); // hit lower attack ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitLowerDefend * propertyBonus ) ) != 0 )
				list.Add( 1060425, prop.ToString() ); // hit lower defense ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitMagicArrow * propertyBonus ) ) != 0 )
				list.Add( 1060426, prop.ToString() ); // hit magic arrow ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitLeechMana * propertyBonus ) ) != 0 )
			{
				prop = (int) ( ( prop / 2 ) * this.GetSpeed() * 0.25 );

				if ( prop > 100 )
					prop = 100;

				if ( this is BaseRanged )
					prop /= 2;

				list.Add( 1060427, prop.ToString() ); // hit mana leech ~1_val~%
			}

			if ( ( prop = (int) ( WeaponAttributes.HitPhysicalArea * propertyBonus ) ) != 0 )
				list.Add( 1060428, prop.ToString() ); // hit physical area ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitPoisonArea * propertyBonus ) ) != 0 )
				list.Add( 1060429, prop.ToString() ); // hit poison area ~1_val~%

			if ( ( prop = (int) ( WeaponAttributes.HitLeechStam * propertyBonus ) ) != 0 )
				list.Add( 1060430, prop.ToString() ); // hit stamina leech ~1_val~%

			if ( ( prop = Attributes.BonusHits ) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( ( prop = Attributes.BonusInt ) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( ( prop = Attributes.LowerManaCost ) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( ( prop = Attributes.LowerRegCost ) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( ( prop = GetLowerStatReq() ) != 0 )
				list.Add( 1060435, prop.ToString() ); // lower requirements ~1_val~%

			if ( ( prop = Attributes.Luck ) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( ( prop = WeaponAttributes.MageWeapon ) != 0 )
				list.Add( 1060438, ( 30 - prop ).ToString() ); // mage weapon -~1_val~ skill

			if ( ( prop = Attributes.BonusMana ) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( ( prop = Attributes.RegenMana ) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( ( prop = Attributes.NightSight ) != 0 )
				list.Add( 1060441 ); // night sight

			if ( ( prop = Attributes.ReflectPhysical ) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( ( prop = Attributes.RegenStam ) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( ( prop = Attributes.RegenHits ) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( ( prop = WeaponAttributes.SelfRepair ) != 0 )
				list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

			if ( ( prop = Attributes.SpellChanneling ) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( ( prop = Attributes.SpellDamage ) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( ( prop = Attributes.BonusStam ) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( ( prop = Attributes.BonusStr ) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( ( prop = Attributes.IncreasedKarmaLoss ) != 0 )
				list.Add( 1075210, prop.ToString() ); // Increased Karma Loss ~1_val~%

			if ( ( prop = Attributes.CastingFocus ) != 0 )
				list.Add( 1113696, prop.ToString() ); // Casting Focus ~1_val~%

			if ( ( prop = Attributes.WeaponSpeed ) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

			if ( ( prop = AbsorptionAttributes.KineticEater ) != 0 )
				list.Add( 1113597, prop.ToString() ); // Kinetic Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.FireEater ) != 0 )
				list.Add( 1113593, prop.ToString() ); // Fire Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.ColdEater ) != 0 )
				list.Add( 1113594, prop.ToString() ); // Cold Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.PoisonEater ) != 0 )
				list.Add( 1113595, prop.ToString() ); // Poison Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.EnergyEater ) != 0 )
				list.Add( 1113596, prop.ToString() ); // Energy Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.DamageEater ) != 0 )
				list.Add( 1113598, prop.ToString() ); // Damage Eater ~1_Val~%

			if ( ( prop = AbsorptionAttributes.KineticResonance ) != 0 )
				list.Add( 1113695, prop.ToString() ); // Kinetic Resonance ~1_val~%

			if ( ( prop = AbsorptionAttributes.FireResonance ) != 0 )
				list.Add( 1113691, prop.ToString() ); // Fire Resonance ~1_val~%

			if ( ( prop = AbsorptionAttributes.ColdResonance ) != 0 )
				list.Add( 1113692, prop.ToString() ); // Cold Resonance ~1_val~%

			if ( ( prop = AbsorptionAttributes.PoisonResonance ) != 0 )
				list.Add( 1113693, prop.ToString() ); // Poison Resonance ~1_val~%

			if ( ( prop = AbsorptionAttributes.EnergyResonance ) != 0 )
				list.Add( 1113694, prop.ToString() ); // Energy Resonance ~1_val~%

			AddDamageTypeProperty( list );

			list.Add( 1061168, "{0}\t{1}", MinDamage.ToString(), MaxDamage.ToString() ); // weapon damage ~1_val~ - ~2_val~

			string speed = ( Speed * 0.25 ).ToString() + "s";
			list.Add( 1061167, speed ); // weapon speed ~1_val~

			AddRangeProperty( list );

			int strReq = AOS.Scale( StrengthReq, 100 - GetLowerStatReq() );

			if ( strReq > 0 )
				list.Add( 1061170, strReq.ToString() ); // strength requirement ~1_val~

			if ( Layer == Layer.TwoHanded )
				list.Add( 1061171 ); // two-handed weapon
			else
				list.Add( 1061824 ); // one-handed weapon

			AddSkillRequiredProperty( list );

			if ( CanLoseDurability )
				list.Add( 1060639, "{0}\t{1}", m_Hits, m_MaxHits ); // durability ~1_val~ / ~2_val~

			GetSetArmorPropertiesSecond( list );
		}

		protected virtual void AddDamageTypeProperty( ObjectPropertyList list )
		{
			int phys, fire, cold, pois, nrgy, chao;

			GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy, out chao );

			if ( phys != 0 )
				list.Add( 1060403, phys.ToString() ); // physical damage ~1_val~%

			if ( fire != 0 )
				list.Add( 1060405, fire.ToString() ); // fire damage ~1_val~%

			if ( cold != 0 )
				list.Add( 1060404, cold.ToString() ); // cold damage ~1_val~%

			if ( pois != 0 )
				list.Add( 1060406, pois.ToString() ); // poison damage ~1_val~%

			if ( nrgy != 0 )
				list.Add( 1060407, nrgy.ToString() ); // energy damage ~1_val~%

			if ( chao != 0 )
				list.Add( 1072846, chao.ToString() ); // chaos damage ~1_val~%
		}

		protected virtual void AddSkillRequiredProperty( ObjectPropertyList list )
		{
			if ( m_WeaponAttributes.UseBestSkill == 0 )
			{
				switch ( Skill )
				{
					case SkillName.Swords:
						list.Add( 1061172 ); // skill required: swordsmanship
						break;
					case SkillName.Macing:
						list.Add( 1061173 ); // skill required: mace fighting
						break;
					case SkillName.Fencing:
						list.Add( 1061174 ); // skill required: fencing
						break;
					case SkillName.Archery:
						list.Add( 1061175 ); // skill required: archery
						break;
					case SkillName.Throwing:
						list.Add( 1112075 ); // skill required: throwing
						break;
				}
			}
		}

		private static BaseWeapon m_Fists; // This value holds the default--fist--weapon

		public static BaseWeapon Fists
		{
			get { return m_Fists; }
			set { m_Fists = value; }
		}

		#region ICraftable Members
		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			PlayerConstructed = true;

			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			Resource = CraftResources.GetFromType( resourceType );

			CraftContext context = craftSystem.GetContext( from );

			if ( context != null && context.DoNotColor )
				Hue = 0;

			if ( exceptional )
			{
				Attributes.WeaponDamage += 35;
				WeaponAttributes.DurabilityBonus += 20;
			}

			if ( tool is BaseRunicTool )
			{
				if ( !CraftableArtifacts.IsCraftableArtifact( this ) )
					( (BaseRunicTool) tool ).ApplyAttributesTo( this );
			}

			if ( exceptional )
			{
				Attributes.WeaponDamage += (int) ( from.Skills.ArmsLore.Value / 20 ); // up to 55 di
				from.CheckSkill( SkillName.ArmsLore, 0, 100 );
			}

			return exceptional;
		}

		private void ApplyResourceAttributes( CraftResource resource )
		{
			CraftResourceInfo info = CraftResources.GetInfo( resource );

			if ( info != null )
				info.AttributeInfo.ApplyAttributesTo( this );

			switch ( resource )
			{
				case CraftResource.Heartwood:
					{
						AddHeartwoodBonus();
						break;
					}
				case CraftResource.Bloodwood:
					{
						if ( WeaponAttributes.HitLeechHits < 16 )
							WeaponAttributes.HitLeechHits = 16;

						break;
					}
			}
		}

		private void AddHeartwoodBonus()
		{
			switch ( Utility.RandomMinMax( 1, 6 ) )
			{
				case 1:
					Attributes.WeaponSpeed += 10;
					break;
				case 2:
					Attributes.WeaponDamage += 10;
					break;
				case 3:
					Attributes.AttackChance += 5;
					break;
				case 4:
					Attributes.Luck += 40;
					break;
				case 5:
					WeaponAttributes.HitLeechHits += 10;
					break;
				case 6:
					WeaponAttributes.LowerStatReq += 10;
					break;
			}
		}
		#endregion

		#region IImbuable Members
		[CommandProperty( AccessLevel.GameMaster )]
		public int TimesImbued
		{
			get { return m_TimesImbued; }
			set
			{
				m_TimesImbued = value;
				InvalidateProperties();
			}
		}

		public virtual ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Weapon; } }
		public bool IsSpecialMaterial { get { return !CraftResources.IsStandard( m_Resource ); } }
		public virtual int MaxIntensity { get { return ( Exceptional ? 500 : 450 ); } }
		public virtual bool CanImbue { get { return ArtifactRarity == 0; } }

		public void OnImbued()
		{
		}
		#endregion

		#region IGorgonCharges Members
		private int m_GorgonCharges;
		private GorgonQuality m_GorgonQuality;

		[CommandProperty( AccessLevel.GameMaster )]
		public int GorgonCharges { get { return m_GorgonCharges; } set { m_GorgonCharges = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public GorgonQuality GorgonQuality { get { return m_GorgonQuality; } set { m_GorgonQuality = value; } }
		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Altered
		{
			get { return m_Altered; }
			set
			{
				m_Altered = value;
				InvalidateProperties();
			}
		}

		public void AlterFrom( BaseWeapon orig )
		{
			m_Altered = true;

			Resource = orig.Resource;

			m_MagicalAttributes = orig.Attributes;
			m_Resistances = orig.Resistances;
			m_SkillBonuses = orig.SkillBonuses;
			m_WeaponAttributes = orig.WeaponAttributes;

			Slayer = orig.Slayer;
			Slayer2 = orig.Slayer2;
			Slayer3 = orig.Slayer3;
			EngravedText = orig.EngravedText;
			Poison = orig.Poison;
			PoisonCharges = orig.PoisonCharges;
			TimesImbued = orig.TimesImbued;
			Crafter = orig.Crafter;
			Exceptional = orig.Exceptional;

			MaxHitPoints = orig.MaxHitPoints;
			HitPoints = orig.HitPoints;
		}
	}
}
