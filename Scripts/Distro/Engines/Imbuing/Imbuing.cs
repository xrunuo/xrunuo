using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Network;
using Server.Events;

namespace Server.Engines.Imbuing
{
	public enum ImbuingFlag
	{
		None = 0x0000,
		Weapon = 0x0001,
		Ranged = 0x0002,
		Throwing = 0x0004,
		Armor = 0x0008,
		Shield = 0x0010,
		Jewelry = 0x0020,

		AllWeapons = Weapon | Ranged | Throwing,
	}

	public interface IImbuable
	{
		ImbuingFlag ImbuingFlags { get; }
		int TimesImbued { get; set; }
		void OnImbued();
		bool IsSpecialMaterial { get; }
		int MaxIntensity { get; }
		bool CanImbue { get; }
	}

	public class Imbuing
	{
		public static void Initialize()
		{
			EventSink.Instance.Login += new LoginEventHandler( OnLogin );
		}

		private static void OnLogin( LoginEventArgs e )
		{
			if ( !e.Mobile.CanBeginAction( typeof( Imbuing ) ) )
				e.Mobile.EndAction( typeof( Imbuing ) );
		}

		/// <summary>
		/// Do the imbuing stuff.
		/// </summary>
		public static void Do( Mobile from, Item item, BaseAttrInfo propToImbue, BaseAttrInfo propToReplace, int value, int wTotalIntensity, int r1, int r2, int r3, double successChance )
		{
			IImbuable imbuable = item as IImbuable;
			int curValue = propToImbue.GetValue( item );

			if ( curValue == value )
				from.SendLocalizedMessage( 1113473 ); // The item already has that property imbued at that intensity.
			else if ( wTotalIntensity > imbuable.MaxIntensity )
				from.SendLocalizedMessage( 1113364 ); // You can not imbue this property on this item at the selected intensity because it will make the item unstable.
			else if ( !Check( from, item ) )
			{
			}
			else if ( !Resources.ConsumeResources( from, propToImbue, r1, r2, r3 ) )
				from.SendLocalizedMessage( 1079773 ); // You do not have enough resources to imbue this item.
			else
			{
				bool success;

				if ( imbuable.TimesImbued >= 20 )
				{
					from.SendLocalizedMessage( 1113377 ); // Your chance to learn while imbuing this item is diminished, as it has been imbued many times before.
					success = successChance > Utility.RandomDouble();
				}
				else
				{
					success = from.CheckSkill( SkillName.Imbuing, successChance );
				}

				Effects.SendPacket( from, from.Map, new GraphicalEffect( EffectType.FixedFrom, from.Serial, Server.Serial.Zero, 0x375A, from.Location, from.Location, 1, 17, true, false ) );
				Effects.SendTargetParticles( from, 0, 1, 0, 0x1593, EffectLayer.Waist );

				if ( success )
				{
					if ( propToReplace != null )
						propToReplace.SetValue( item, 0 );

					propToImbue.SetValue( item, value );

					if ( item is IArmor )
						( (IArmor) item ).ArmorAttributes.SelfRepair = 0;
					else if ( item is ICloth )
						( (ICloth) item ).ClothingAttributes.SelfRepair = 0;
					else if ( item is IWeapon )
						( (IWeapon) item ).WeaponAttributes.SelfRepair = 0;

					imbuable.OnImbued();
					imbuable.TimesImbued++;

					from.SendLocalizedMessage( 1079775 ); // You successfully imbue the item!
					from.PlaySound( 0x1EB );
				}
				else
				{
					from.SendLocalizedMessage( 1079774 ); // You attempt to imbue the item, but fail.
					from.PlaySound( 0x1E4 );
				}

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
					delegate
					{
						from.SendGump( new ImbuingMainGump() );
					}
				) );

				return;
			}

			from.EndAction( typeof( Imbuing ) );
		}

		/// <summary>
		/// Gets the mobile's success chance of imbuing the given item and intensity.
		/// </summary>
		public static double GetSuccessChance( Mobile from, Item item, int intensity )
		{
			double baseChance, ourChance, top, x, y;

			bool playerConstructed = ( item is BaseWeapon && ( (BaseWeapon) item ).PlayerConstructed ) || ( item is BaseArmor && ( (BaseArmor) item ).PlayerConstructed );
			bool exceptional = ( item is BaseWeapon && ( (BaseWeapon) item ).Exceptional ) || ( item is BaseArmor && ( (BaseArmor) item ).Exceptional ) || ( item is BaseClothing && ( (BaseClothing) item ).Exceptional );

			baseChance = 0.5;

			if ( playerConstructed )
				baseChance += 0.3;

			if ( exceptional )
				baseChance += 0.3;

			top = baseChance + 2.4;

			x = (double) intensity / 500;
			y = Curve( x );

			ourChance = baseChance;
			ourChance += 2 * from.Skills[SkillName.Imbuing].Value / 100.0;
			ourChance -= y * top;

			if ( from != null && from.Region.IsPartOf( "Queen's Palace" ) )
			{
				ourChance += .05;
			}
			else if ( from != null && from.Region.IsPartOf( "RoyalCity's Forge" ) )
			{
				ourChance += .02;
			}

			if ( from.Race == Race.Gargoyle )
				ourChance *= 1.1;

			return ourChance;
		}

		public static double Curve( double d )
		{
			// TODO (SA): Find the correct formula.
			return 0.25 * ( Math.Sqrt( -16.0 * Math.Pow( d, 2 ) + 40.0 * d + 1.0 ) - 1.0 );
		}

		/// <summary>
		/// Performs the pre-imbue checks for the given Mobile and Item.
		/// </summary>
		public static bool Check( Mobile from, Item item )
		{
			IImbuable imbuable = item as IImbuable;
			BaseWeapon weapon = item as BaseWeapon;

			/*
			 * Additional checks that OSI performs but aren't needed on X-RunUO:
			 * - Focus Attack effect: 1080444 "You cannot imbue an item that is under the effects of the ninjitsu focus attack ability."
			 */

			if ( !Soulforge.CheckProximity( from, 2 ) )
				from.SendLocalizedMessage( 1079787 ); // You must be near a soulforge to imbue an item.
			else if ( !Soulforge.CheckQueen( from ) )
				from.SendLocalizedMessage( 1113736 ); // You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
			else if ( item == null || !item.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1079575 ); // The item must be in your backpack to imbue it.
			else if ( imbuable == null || !imbuable.CanImbue || item is ISetItem || item is ICollectionItem )
				from.SendLocalizedMessage( 1079576 ); // You cannot imbue this item.
			else if ( item.LootType == LootType.Blessed )
				from.SendLocalizedMessage( 1080438 ); // You cannot imbue a blessed item.
			else if ( weapon != null && weapon.Enchanted )
				from.SendLocalizedMessage( 1080130 ); // You cannot imbue an item that is currently enchanted.
			else
				return true;

			return false;
		}

		/// <summary>
		/// Returns a list of properties that can be imbued to the given item.
		/// </summary>
		public static List<BaseAttrInfo> GetValidProperties( Item item )
		{
			List<BaseAttrInfo> props = new List<BaseAttrInfo>();
			IImbuable imbuable = item as IImbuable;

			if ( imbuable != null )
			{
				foreach ( BaseAttrInfo prop in m_Properties )
				{
					if ( prop.CanHold( item ) && ValidateFlags( imbuable.ImbuingFlags, prop.Flags ) && prop.Validate( item ) )
						props.Add( prop );
				}
			}

			return props;
		}

		/// <summary>
		/// Returns the number of intensity steps that the given prop can take.
		/// </summary>
		public static int GetIntensitySteps( BaseAttrInfo prop )
		{
			return Math.Max( 1, 1 + (int) ( ( prop.MaxIntensity - prop.MinIntensity ) / prop.IntensityInterval ) );
		}

		/// <summary>
		/// Returns the intensity of the given prop on the given item.
		/// </summary>
		public static int ComputeIntensity( Item item, BaseAttrInfo prop )
		{
			return ComputeIntensity( item, prop, true );
		}

		/// <summary>
		/// Returns the intensity of the given prop on the given item.
		/// </summary>
		/// <param name="checkHold">true if we want to ensure CanHold( item ) is checked, false if we have checked it before.</param>
		public static int ComputeIntensity( Item item, BaseAttrInfo prop, bool checkHold )
		{
			if ( checkHold && !prop.CanHold( item ) )
				return 0;

			int propVal = prop.GetValue( item );

			int total = GetIntensitySteps( prop );
			int relative = Math.Max( 0, 1 + (int) ( ( propVal - prop.MinIntensity ) / prop.IntensityInterval ) );

			int intensity = (int) ( 100 * ( (double) relative / total ) );

			return intensity;
		}

		/// <summary>
		/// Returns the item property from props that newProp replaces, null if it does not replace any.
		/// </summary>
		public static BaseAttrInfo GetReplaced( BaseAttrInfo newProp, PropCollection props )
		{
			foreach ( PropCollection.PropEntry entry in props.Properties )
			{
				BaseAttrInfo attr = entry.Property;

				if ( newProp.Replaces( attr ) )
					return attr;
			}

			return null;
		}

		/// <summary>
		/// The table with all the Imbuable properties.
		/// </summary>
		public static BaseAttrInfo[] Properties { get { return m_Properties; } }

		private static BaseAttrInfo[] m_Properties = new BaseAttrInfo[]
			{
				// Magical Properties
				new DamageIncreaseJewel(),	new LuckRanged(),
				new DamageIncrease(),		new Luck(),
				new DefenseChanceIncrease(),new DefenseChanceIncreaseRanged(),
				new HitChanceIncrease(),	new HitChanceIncreaseRanged(),
				new FasterCasting(),		new SwingSpeedIncrease(),
				new SpellChanneling(),		new HitPointIncrease(),
				new StaminaIncrease(),		new ManaIncrease(),
				new LowerManaCost(),		new ReflectPhysicalDamage(),
				new NightSight(),			new ManaRegeneration(),
				new StaminaRegeneration(),	new HitPointRegeneration(),
				new StrengthBonus(),		new DexterityBonus(),
				new IntelligenceBonus(),	new FasterCastRecovery(),
				new EnhancePotions(),		new LowerReagentCost(),
				new SpellDamageIncrease(),

				// Armor Properties
				new LowerRequirements(),	new MageArmor(),

				// Weapon Properties
				new UseBestWeaponSkill(),	new HitDispel(),
				new HitPhysicalArea(),		new HitFireArea(),
				new HitColdArea(),			new HitPoisonArea(),
				new HitEnergyArea(),		new HitHarm(),
				new HitFireball(),			new HitMagicArrow(),
				new HitLightning(),			new HitLowerAttack(),
				new HitLowerDefense(),		new HitLifeLeech(),
				new HitStaminaLeech(),		new HitManaLeech(),
				new MageWeapon(),

				// Ranged Weapon Properties
				new Velocity(),				new Balanced(),

				// Resistances
				new PhysicalResist(),		new FireResist(),
				new ColdResist(),			new PoisonResist(),
				new EnergyResist(),

				// Slayers
				new AirElementalSlayer(),	new ArachnidSlayer(),
				new DemonSlayer(),			new BloodElementalSlayer(),
				new EarthElementalSlayer(),	new DragonSlayer(),
				new FireElementalSlayer(),	new ElementalSlayer(),
				new LizardmanSlayer(),		new GargoyleSlayer(),
				new OphidianSlayer(),		new OgreSlayer(),
				new PoisonElementalSlayer(),new OrcSlayer(),
				new ReptileSlayer(),		new RepondSlayer(),
				new SnakeSlayer(),			new ScorpionSlayer(),
				new SpiderSlayer(),			new SnowElementalSlayer(),
				new TrollSlayer(),			new TerathanSlayer(),
				new WaterElementalSlayer(),	new UndeadSlayer(),

				// Skills
				new SkillBonusInfo( SkillName.Fencing,		1112012, 1 ),
				new SkillBonusInfo( SkillName.Macing,		1112013, 1 ),
				new SkillBonusInfo( SkillName.Magery,		1112014, 1 ),
				new SkillBonusInfo( SkillName.Musicianship,	1112015, 1 ),
				new SkillBonusInfo( SkillName.Swords,		1112016, 1 ),
				new SkillBonusInfo( SkillName.AnimalTaming,	1112017, 2 ),
				new SkillBonusInfo( SkillName.Provocation,	1112018, 2 ),
				new SkillBonusInfo( SkillName.SpiritSpeak,	1112019, 2 ),
				new SkillBonusInfo( SkillName.Tactics,		1112020, 2 ),
				new SkillBonusInfo( SkillName.Wrestling,	1112021, 2 ),
				new SkillBonusInfo( SkillName.AnimalLore,	1112022, 3 ),
				new SkillBonusInfo( SkillName.Discordance,	1112023, 3 ),
				new SkillBonusInfo( SkillName.Focus,		1112024, 3 ),
				new SkillBonusInfo( SkillName.Meditation,	1112025, 3 ),
				new SkillBonusInfo( SkillName.Parry,		1112026, 3 ),
				new SkillBonusInfo( SkillName.Stealth,		1112027, 3 ),
				new SkillBonusInfo( SkillName.Anatomy,		1112028, 4 ),
				new SkillBonusInfo( SkillName.Bushido,		1112029, 4 ),
				new SkillBonusInfo( SkillName.EvalInt,		1112030, 4 ),
				new SkillBonusInfo( SkillName.Necromancy,	1112031, 4 ),
				new SkillBonusInfo( SkillName.Stealing,		1112032, 4 ),
				new SkillBonusInfo( SkillName.Veterinary,	1112033, 4 ),
				new SkillBonusInfo( SkillName.Archery,		1112034, 5 ),
				new SkillBonusInfo( SkillName.Chivalry,		1112035, 5 ),
				new SkillBonusInfo( SkillName.Healing,		1112036, 5 ),
				new SkillBonusInfo( SkillName.Ninjitsu,		1112037, 5 ),
				new SkillBonusInfo( SkillName.Peacemaking,	1112038, 5 ),
				new SkillBonusInfo( SkillName.MagicResist,	1112039, 5 ),

				// Properties not included in the imbuing system, but
				// needed to compute the magical intensity of an item
				new OtherMagicalAttrInfo( MagicalAttribute.LowerAmmoCost, 1, 20 ),
				new OtherMagicalAttrInfo( MagicalAttribute.IncreasedKarmaLoss, 1, 10 ),
				new OtherMagicalAttrInfo( MagicalAttribute.CastingFocus, 1, 5 ),

				new OtherWeaponAttrInfo( WeaponAttribute.HitFatigue, 1, 15 ),
				new OtherWeaponAttrInfo( WeaponAttribute.HitManaDrain, 1, 15 ),
				new OtherWeaponAttrInfo( WeaponAttribute.HitCurse, 1, 15 ),
				new OtherWeaponAttrInfo( WeaponAttribute.SplinteringWeapon, 1, 1 ),
				new OtherWeaponAttrInfo( WeaponAttribute.BattleLust, 1, 1 ),
				new OtherWeaponAttrInfo( WeaponAttribute.BloodDrinker, 1, 1 ),
				new HitLowerDefendGlasses(),

				new OtherArmorAttrInfo( ArmorAttribute.ReactiveParalyze, 1, 1 ),
				new OtherArmorAttrInfo( ArmorAttribute.SoulCharge, 1, 20 ),

				new AbsorptionAttrInfo( AbsorptionAttribute.KineticEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.FireEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.ColdEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.PoisonEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.EnergyEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.DamageEater, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.KineticResonance, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.FireResonance, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.ColdResonance, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.PoisonResonance, 1, 15 ),
				new AbsorptionAttrInfo( AbsorptionAttribute.EnergyResonance, 1, 15 )
			};

		public static Type[] HitSpellTypes { get { return m_HitSpellTypes; } }
		public static Type[] HitAreaTypes { get { return m_HitAreaTypes; } }

		private static Type[] m_HitSpellTypes = new Type[]
			{
				typeof( HitMagicArrow ),	typeof( HitFireball ),
				typeof( HitDispel ),		typeof( HitLightning ),
				typeof( HitHarm )
			};

		private static Type[] m_HitAreaTypes = new Type[]
			{
				typeof( HitPhysicalArea ),	typeof( HitFireArea ),
				typeof( HitColdArea ),		typeof( HitPoisonArea ),
				typeof( HitEnergyArea )
			};

		public static bool ValidateFlags( ImbuingFlag toValidate, ImbuingFlag mask )
		{
			return ( toValidate & mask ) != 0;
		}
	}

	public class PropCollection
	{
		private List<PropEntry> m_Properties = new List<PropEntry>();

		private int m_Intensity;
		private int m_WeightedIntensity;

		public List<PropEntry> Properties { get { return m_Properties; } }
		public int Count { get { return m_Properties.Count; } }
		public int Intensity { get { return m_Intensity; } }
		public int WeightedIntensity { get { return m_WeightedIntensity; } }

		public PropCollection( Item item )
			: this( item, false )
		{
		}

		public PropCollection( Item item, bool validateFlags )
		{
			if ( item is IImbuable )
			{
				IImbuable imbuable = item as IImbuable;

				foreach ( BaseAttrInfo prop in Imbuing.Properties )
				{
					if ( prop.CanHold( item ) && ( !validateFlags || Imbuing.ValidateFlags( imbuable.ImbuingFlags, prop.Flags ) ) && prop.Validate( item ) )
					{
						int intensity = Imbuing.ComputeIntensity( item, prop, false );

						if ( intensity != 0 )
						{
							PropEntry entry = new PropEntry( prop, intensity );

							m_Properties.Add( entry );

							m_Intensity += entry.Intensity;
							m_WeightedIntensity += entry.WeightedIntensity;
						}
					}
				}
			}
		}

		public class PropEntry
		{
			private BaseAttrInfo m_Property;
			private int m_Intensity;

			public BaseAttrInfo Property { get { return m_Property; } }
			public double Weight { get { return m_Property.Weight; } }
			public int Intensity { get { return m_Intensity; } }
			public int WeightedIntensity { get { return (int) ( Intensity * Weight ); } }

			public PropEntry( BaseAttrInfo prop, int intensity )
			{
				m_Property = prop;
				m_Intensity = intensity;
			}
		}
	}

	public class ImbuingContext
	{
		private static Dictionary<Mobile, ImbuingContext> m_ContextTable = new Dictionary<Mobile, ImbuingContext>();

		public static ImbuingContext GetContext( Mobile m )
		{
			if ( !m_ContextTable.ContainsKey( m ) )
				return null;

			return m_ContextTable[m];
		}

		public static Item GetLastItem( Mobile m )
		{
			if ( !m_ContextTable.ContainsKey( m ) )
				return null;

			return m_ContextTable[m].Item;
		}

		public static BaseAttrInfo GetLastProperty( Mobile m )
		{
			if ( !m_ContextTable.ContainsKey( m ) )
				return null;

			return m_ContextTable[m].Property;
		}

		public static void AddContext( Mobile m, Item item, BaseAttrInfo prop, int intensity )
		{
			if ( m_ContextTable.ContainsKey( m ) )
				m_ContextTable.Remove( m );

			m_ContextTable[m] = new ImbuingContext( item, prop, intensity );
		}

		private Item m_Item;
		private BaseAttrInfo m_Property;
		private int m_Intensity;

		public Item Item { get { return m_Item; } }
		public BaseAttrInfo Property { get { return m_Property; } }
		public int Intensity { get { return m_Intensity; } }

		public ImbuingContext( Item item, BaseAttrInfo property, int intensity )
		{
			m_Item = item;
			m_Property = property;
			m_Intensity = intensity;
		}
	}
}