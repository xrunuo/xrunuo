using System;
using System.Collections;

namespace Server.Items
{
	public enum CraftResource
	{
		None = 0,
		Iron = 1,
		DullCopper,
		ShadowIron,
		Copper,
		Bronze,
		Gold,
		Agapite,
		Verite,
		Valorite,

		RegularLeather = 101,
		SpinedLeather,
		HornedLeather,
		BarbedLeather,

		RedScales = 201,
		YellowScales,
		BlackScales,
		GreenScales,
		WhiteScales,
		BlueScales,

		Wood,
		Oak,
		Ash,
		Yew,
		Heartwood,
		Bloodwood,
		Frostwood
	}

	public enum CraftResourceType
	{
		None,
		Metal,
		Leather,
		Scales,
		Wood
	}

	public class CraftAttributeInfo
	{
		public int WeaponFireDamage { get; set; }
		public int WeaponColdDamage { get; set; }
		public int WeaponPoisonDamage { get; set; }
		public int WeaponEnergyDamage { get; set; }
		public int WeaponDurability { get; set; }
		public int WeaponLuck { get; set; }
		public int WeaponLowerRequirements { get; set; }
		public int WeaponDamage { get; set; }
		public int WeaponAttackChance { get; set; }
		public int WeaponRegenHits { get; set; }
		public int WeaponLifeLeech { get; set; }
		public int WeaponSpeed { get; set; }

		public int ArmorPhysicalResist { get; set; }
		public int ArmorFireResist { get; set; }
		public int ArmorColdResist { get; set; }
		public int ArmorPoisonResist { get; set; }
		public int ArmorEnergyResist { get; set; }
		public int ArmorDurability { get; set; }
		public int ArmorLuck { get; set; }
		public int ArmorLowerRequirements { get; set; }
		public int ArmorRegenHits { get; set; }

		public int ShieldPhysicalResist { get; set; }
		public int ShieldFireResist { get; set; }
		public int ShieldColdResist { get; set; }
		public int ShieldPoisonResist { get; set; }
		public int ShieldEnergyResist { get; set; }
		public int ShieldReflectDamage { get; set; }
		public int ShieldSpellChanneling { get; set; }
		public int ShieldFasterCasting { get; set; }

		public int RunicMinAttributes { get; set; }
		public int RunicMaxAttributes { get; set; }
		public int RunicMinIntensity { get; set; }
		public int RunicMaxIntensity { get; set; }

		public double ImbuingUnravelBonus { get; set; }

		public CraftAttributeInfo()
		{
		}

		public static readonly CraftAttributeInfo Blank;
		public static readonly CraftAttributeInfo DullCopper, ShadowIron, Copper, Bronze, Golden, Agapite, Verite, Valorite;
		public static readonly CraftAttributeInfo Spined, Horned, Barbed;
		public static readonly CraftAttributeInfo RedScales, YellowScales, BlackScales, GreenScales, WhiteScales, BlueScales;
		public static readonly CraftAttributeInfo Oak, Ash, Yew, Heartwood, Bloodwood, Frostwood;

		static CraftAttributeInfo()
		{
			Blank = new CraftAttributeInfo();

			CraftAttributeInfo dullCopper = DullCopper = new CraftAttributeInfo();

			dullCopper.ArmorPhysicalResist = 6;
			dullCopper.ShieldPhysicalResist = 6;
			dullCopper.ArmorDurability = 50;
			dullCopper.ArmorLowerRequirements = 20;
			dullCopper.WeaponDurability = 100;
			dullCopper.WeaponLowerRequirements = 50;
			dullCopper.RunicMinAttributes = 1;
			dullCopper.RunicMaxAttributes = 2;
			dullCopper.RunicMinIntensity = 40;
			dullCopper.RunicMaxIntensity = 100;
			dullCopper.ImbuingUnravelBonus = 0.02;

			CraftAttributeInfo shadowIron = ShadowIron = new CraftAttributeInfo();

			shadowIron.ArmorPhysicalResist = 2;
			shadowIron.ArmorFireResist = 1;
			shadowIron.ArmorEnergyResist = 5;
			shadowIron.ShieldPhysicalResist = 2;
			shadowIron.ShieldFireResist = 1;
			shadowIron.ShieldEnergyResist = 5;
			shadowIron.ArmorDurability = 100;
			shadowIron.WeaponColdDamage = 20;
			shadowIron.WeaponDurability = 50;
			shadowIron.RunicMinAttributes = 2;
			shadowIron.RunicMaxAttributes = 2;
			shadowIron.RunicMinIntensity = 45;
			shadowIron.RunicMaxIntensity = 100;
			shadowIron.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo copper = Copper = new CraftAttributeInfo();

			copper.ArmorPhysicalResist = 1;
			copper.ArmorFireResist = 1;
			copper.ArmorPoisonResist = 5;
			copper.ArmorEnergyResist = 2;
			copper.ShieldPhysicalResist = 1;
			copper.ShieldFireResist = 1;
			copper.ShieldPoisonResist = 5;
			copper.ShieldEnergyResist = 2;
			copper.WeaponPoisonDamage = 10;
			copper.WeaponEnergyDamage = 20;
			copper.RunicMinAttributes = 2;
			copper.RunicMaxAttributes = 3;
			copper.RunicMinIntensity = 50;
			copper.RunicMaxIntensity = 100;
			copper.ImbuingUnravelBonus = 0.04;

			CraftAttributeInfo bronze = Bronze = new CraftAttributeInfo();

			bronze.ArmorPhysicalResist = 3;
			bronze.ArmorColdResist = 5;
			bronze.ArmorPoisonResist = 1;
			bronze.ArmorEnergyResist = 1;
			bronze.ShieldPhysicalResist = 3;
			bronze.ShieldColdResist = 5;
			bronze.ShieldPoisonResist = 1;
			bronze.ShieldEnergyResist = 1;
			bronze.WeaponFireDamage = 40;
			bronze.RunicMinAttributes = 3;
			bronze.RunicMaxAttributes = 3;
			bronze.RunicMinIntensity = 55;
			bronze.RunicMaxIntensity = 100;
			bronze.ImbuingUnravelBonus = 0.05;

			CraftAttributeInfo golden = Golden = new CraftAttributeInfo();

			golden.ArmorPhysicalResist = 1;
			golden.ArmorFireResist = 1;
			golden.ArmorColdResist = 2;
			golden.ArmorEnergyResist = 2;
			golden.ShieldPhysicalResist = 1;
			golden.ShieldFireResist = 1;
			golden.ShieldColdResist = 2;
			golden.ShieldEnergyResist = 2;
			golden.ArmorLuck = 40;
			golden.ArmorLowerRequirements = 30;
			golden.WeaponLuck = 40;
			golden.WeaponLowerRequirements = 50;
			golden.RunicMinAttributes = 3;
			golden.RunicMaxAttributes = 4;
			golden.RunicMinIntensity = 60;
			golden.RunicMaxIntensity = 100;
			golden.ImbuingUnravelBonus = 0.07;

			CraftAttributeInfo agapite = Agapite = new CraftAttributeInfo();

			agapite.ArmorPhysicalResist = 2;
			agapite.ArmorFireResist = 3;
			agapite.ArmorColdResist = 2;
			agapite.ArmorPoisonResist = 2;
			agapite.ArmorEnergyResist = 2;
			agapite.ShieldPhysicalResist = 2;
			agapite.ShieldFireResist = 3;
			agapite.ShieldColdResist = 2;
			agapite.ShieldPoisonResist = 2;
			agapite.ShieldEnergyResist = 2;
			agapite.WeaponColdDamage = 30;
			agapite.WeaponEnergyDamage = 20;
			agapite.RunicMinAttributes = 4;
			agapite.RunicMaxAttributes = 4;
			agapite.RunicMinIntensity = 65;
			agapite.RunicMaxIntensity = 100;
			agapite.ImbuingUnravelBonus = 0.09;

			CraftAttributeInfo verite = Verite = new CraftAttributeInfo();

			verite.ArmorPhysicalResist = 3;
			verite.ArmorFireResist = 3;
			verite.ArmorColdResist = 2;
			verite.ArmorPoisonResist = 3;
			verite.ArmorEnergyResist = 1;
			verite.ShieldPhysicalResist = 3;
			verite.ShieldFireResist = 3;
			verite.ShieldColdResist = 2;
			verite.ShieldPoisonResist = 3;
			verite.ShieldEnergyResist = 1;
			verite.WeaponPoisonDamage = 40;
			verite.WeaponEnergyDamage = 20;
			verite.RunicMinAttributes = 4;
			verite.RunicMaxAttributes = 5;
			verite.RunicMinIntensity = 70;
			verite.RunicMaxIntensity = 100;
			verite.ImbuingUnravelBonus = 0.12;

			CraftAttributeInfo valorite = Valorite = new CraftAttributeInfo();

			valorite.ArmorPhysicalResist = 4;
			valorite.ArmorColdResist = 3;
			valorite.ArmorPoisonResist = 3;
			valorite.ArmorEnergyResist = 3;
			valorite.ShieldPhysicalResist = 4;
			valorite.ShieldColdResist = 3;
			valorite.ShieldPoisonResist = 3;
			valorite.ShieldEnergyResist = 3;
			valorite.ArmorDurability = 50;
			valorite.WeaponFireDamage = 10;
			valorite.WeaponColdDamage = 20;
			valorite.WeaponPoisonDamage = 10;
			valorite.WeaponEnergyDamage = 20;
			valorite.RunicMinAttributes = 5;
			valorite.RunicMaxAttributes = 5;
			valorite.RunicMinIntensity = 85;
			valorite.RunicMaxIntensity = 100;
			valorite.ImbuingUnravelBonus = 0.20;

			CraftAttributeInfo spined = Spined = new CraftAttributeInfo();

			spined.ArmorPhysicalResist = 5;
			spined.ArmorLuck = 40;
			spined.RunicMinAttributes = 1;
			spined.RunicMaxAttributes = 3;
			spined.RunicMinIntensity = 40;
			spined.RunicMaxIntensity = 100;
			spined.ImbuingUnravelBonus = 0.01;

			CraftAttributeInfo horned = Horned = new CraftAttributeInfo();

			horned.ArmorPhysicalResist = 2;
			horned.ArmorFireResist = 3;
			horned.ArmorColdResist = 2;
			horned.ArmorPoisonResist = 2;
			horned.ArmorEnergyResist = 2;
			horned.RunicMinAttributes = 3;
			horned.RunicMaxAttributes = 4;
			horned.RunicMinIntensity = 45;
			horned.RunicMaxIntensity = 100;
			horned.ImbuingUnravelBonus = 0.02;

			CraftAttributeInfo barbed = Barbed = new CraftAttributeInfo();

			barbed.ArmorPhysicalResist = 2;
			barbed.ArmorFireResist = 1;
			barbed.ArmorColdResist = 2;
			barbed.ArmorPoisonResist = 3;
			barbed.ArmorEnergyResist = 4;
			barbed.RunicMinAttributes = 4;
			barbed.RunicMaxAttributes = 5;
			barbed.RunicMinIntensity = 50;
			barbed.RunicMaxIntensity = 100;
			barbed.ImbuingUnravelBonus = 0.04;

			CraftAttributeInfo red = RedScales = new CraftAttributeInfo();

			red.ArmorFireResist = 10;
			red.ArmorColdResist = -3;
			red.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo yellow = YellowScales = new CraftAttributeInfo();

			yellow.ArmorPhysicalResist = -3;
			yellow.ArmorLuck = 20;
			yellow.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo black = BlackScales = new CraftAttributeInfo();

			black.ArmorPhysicalResist = 10;
			black.ArmorEnergyResist = -3;
			black.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo green = GreenScales = new CraftAttributeInfo();

			green.ArmorFireResist = -3;
			green.ArmorPoisonResist = 10;
			green.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo white = WhiteScales = new CraftAttributeInfo();

			white.ArmorPhysicalResist = -3;
			white.ArmorColdResist = 10;
			white.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo blue = BlueScales = new CraftAttributeInfo();

			blue.ArmorPoisonResist = -3;
			blue.ArmorEnergyResist = 10;
			blue.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo oak = Oak = new CraftAttributeInfo();

			oak.WeaponDamage = 5;
			oak.WeaponLuck = 40;
			oak.ArmorLuck = 40;
			oak.ArmorDurability = 50;
			oak.ArmorPhysicalResist = 3;
			oak.ArmorFireResist = 3;
			oak.ArmorPoisonResist = 2;
			oak.ArmorEnergyResist = 3;
			oak.ShieldPhysicalResist = 1;
			oak.ShieldFireResist = 1;
			oak.ShieldColdResist = 1;
			oak.ShieldPoisonResist = 1;
			oak.ShieldEnergyResist = 1;
			oak.RunicMinIntensity = 20;
			oak.RunicMaxIntensity = 35;
			oak.RunicMinAttributes = 1;
			oak.RunicMaxAttributes = 2;
			oak.ImbuingUnravelBonus = 0.01;

			CraftAttributeInfo ash = Ash = new CraftAttributeInfo();

			ash.WeaponSpeed = 10;
			ash.WeaponLowerRequirements = 20;
			ash.ArmorEnergyResist = 3;
			ash.ArmorLowerRequirements = 20;
			ash.ArmorPhysicalResist = 2;
			ash.ArmorPoisonResist = 1;
			ash.ArmorEnergyResist = 6;
			ash.ArmorColdResist = 4;
			ash.ShieldEnergyResist = 3;
			ash.RunicMinIntensity = 30;
			ash.RunicMaxIntensity = 50;
			ash.RunicMinAttributes = 2;
			ash.RunicMaxAttributes = 3;
			ash.ImbuingUnravelBonus = 0.03;

			CraftAttributeInfo yew = Yew = new CraftAttributeInfo();

			yew.WeaponDamage = 10;
			yew.WeaponAttackChance = 5;
			yew.ArmorRegenHits = 1;
			yew.ArmorPhysicalResist = 6;
			yew.ArmorFireResist = 3;
			yew.ArmorColdResist = 3;
			yew.ArmorEnergyResist = 3;
			yew.ShieldPhysicalResist = 3;
			yew.RunicMinIntensity = 40;
			yew.RunicMaxIntensity = 70;
			yew.RunicMinAttributes = 3;
			yew.RunicMaxAttributes = 4;
			yew.ImbuingUnravelBonus = 0.07;

			CraftAttributeInfo heartwood = Heartwood = new CraftAttributeInfo();

			heartwood.ArmorPhysicalResist = 2;
			heartwood.ArmorFireResist = 3;
			heartwood.ArmorColdResist = 2;
			heartwood.ArmorPoisonResist = 7;
			heartwood.ArmorEnergyResist = 2;
			heartwood.RunicMinIntensity = 50;
			heartwood.RunicMaxIntensity = 100;
			heartwood.RunicMinAttributes = 4;
			heartwood.RunicMaxAttributes = 4;
			heartwood.ImbuingUnravelBonus = 0.10;
			// Valores de Propiedades para weapons y shield Heartwood son
			// aleatorios por lo que son añadidos manualmente al craftear el item.

			CraftAttributeInfo bloodwood = Bloodwood = new CraftAttributeInfo();

			//bloodwood.ArmorLuck = 40; // This is added manually when crafted cause only applies to shields
			bloodwood.ArmorRegenHits = 2;
			bloodwood.WeaponRegenHits = 2;
			bloodwood.ShieldFireResist = 3;
			bloodwood.ArmorPhysicalResist = 3;
			bloodwood.ArmorFireResist = 8;
			bloodwood.ArmorColdResist = 1;
			bloodwood.ArmorPoisonResist = 3;
			bloodwood.ArmorEnergyResist = 3;
			bloodwood.ImbuingUnravelBonus = 0.15;
			// Hit Life Leech 16% es añadido al craftear el arma ya que si
			// esta ya tiene mas de 16% la propiedad no se agrega

			CraftAttributeInfo frostwood = Frostwood = new CraftAttributeInfo();

			frostwood.WeaponColdDamage = 40;
			frostwood.WeaponDamage = 12;
			frostwood.ShieldColdResist = 3;
			frostwood.ShieldFasterCasting = -1;
			frostwood.ShieldSpellChanneling = 1;
			frostwood.ArmorPhysicalResist = 2;
			frostwood.ArmorFireResist = 1;
			frostwood.ArmorColdResist = 8;
			frostwood.ArmorPoisonResist = 3;
			frostwood.ArmorEnergyResist = 4;
			frostwood.ImbuingUnravelBonus = 0.20;
		}

		public void ApplyAttributesTo( BaseWeapon weapon, bool fromLegacyVersion = false )
		{
			weapon.Attributes.Luck += WeaponLuck;
			weapon.Attributes.WeaponDamage += WeaponDamage;
			weapon.Attributes.AttackChance += WeaponAttackChance;
			weapon.Attributes.RegenHits += WeaponRegenHits;
			weapon.Attributes.WeaponSpeed += WeaponSpeed;
			weapon.WeaponAttributes.DurabilityBonus += WeaponDurability;
			weapon.WeaponAttributes.HitLeechHits += WeaponLifeLeech;

			int DamagePhysicalPercent = 100 - ( weapon.WeaponAttributes.DamageFirePercent + weapon.WeaponAttributes.DamageColdPercent + weapon.WeaponAttributes.DamagePoisonPercent + weapon.WeaponAttributes.DamageEnergyPercent );

			if ( DamagePhysicalPercent <= 0 )
				return;

			int[] dmgattrs = new int[]
			{
				WeaponFireDamage,
				WeaponColdDamage,
				WeaponPoisonDamage,
				WeaponEnergyDamage
			};

			int TotalDamagePercentBonus = 0;

			for ( int i = 0; i < dmgattrs.Length; i++ )
				TotalDamagePercentBonus += dmgattrs[i];

			if ( TotalDamagePercentBonus > DamagePhysicalPercent )
			{
				for ( int i = 0; i < dmgattrs.Length; i++ )
				{
					if ( dmgattrs[i] >= DamagePhysicalPercent )
					{
						dmgattrs[i] = DamagePhysicalPercent;
						DamagePhysicalPercent = 0;
					}
					else
					{
						DamagePhysicalPercent -= dmgattrs[i];
					}
				}
			}

			if ( !fromLegacyVersion )
			{
				weapon.WeaponAttributes.DamageFirePercent += dmgattrs[0];
				weapon.WeaponAttributes.DamageColdPercent += dmgattrs[1];
				weapon.WeaponAttributes.DamagePoisonPercent += dmgattrs[2];
				weapon.WeaponAttributes.DamageEnergyPercent += dmgattrs[3];
			}
		}

		public void ApplyAttributesTo( BaseArmor armor )
		{
			armor.Attributes.Luck += ArmorLuck;
			armor.Attributes.RegenHits += ArmorRegenHits;
			armor.ArmorAttributes.DurabilityBonus += ArmorDurability;

			if ( armor is BaseShield )
				ApplyShieldSpecificAttributesTo( (BaseShield) armor );
			else
				ApplyArmorSpecificAttributesTo( (BaseArmor) armor );
		}

		private void ApplyArmorSpecificAttributesTo( BaseArmor armor )
		{
			armor.PhysicalBonus += ArmorPhysicalResist;
			armor.FireBonus += ArmorFireResist;
			armor.ColdBonus += ArmorColdResist;
			armor.PoisonBonus += ArmorPoisonResist;
			armor.EnergyBonus += ArmorEnergyResist;
		}

		private void ApplyShieldSpecificAttributesTo( BaseShield shield )
		{
			shield.Attributes.ReflectPhysical += ShieldReflectDamage;
			shield.Attributes.SpellChanneling += ShieldSpellChanneling;
			shield.Attributes.CastSpeed += ShieldFasterCasting;

			shield.PhysicalBonus += ShieldPhysicalResist;
			shield.FireBonus += ShieldFireResist;
			shield.ColdBonus += ShieldColdResist;
			shield.PoisonBonus += ShieldPoisonResist;
			shield.EnergyBonus += ShieldEnergyResist;
		}
	}

	public class CraftResourceInfo
	{
		private int m_Hue;
		private int m_Number;
		private string m_Name;
		private CraftAttributeInfo m_AttributeInfo;
		private CraftResource m_Resource;
		private Type[] m_ResourceTypes;

		public int Hue { get { return m_Hue; } }
		public int Number { get { return m_Number; } }
		public string Name { get { return m_Name; } }
		public CraftAttributeInfo AttributeInfo { get { return m_AttributeInfo; } }
		public CraftResource Resource { get { return m_Resource; } }
		public Type[] ResourceTypes { get { return m_ResourceTypes; } }

		public CraftResourceInfo( int hue, int number, string name, CraftAttributeInfo attributeInfo, CraftResource resource, params Type[] resourceTypes )
		{
			m_Hue = hue;
			m_Number = number;
			m_Name = name;
			m_AttributeInfo = attributeInfo;
			m_Resource = resource;
			m_ResourceTypes = resourceTypes;

			for ( int i = 0; i < resourceTypes.Length; ++i )
				CraftResources.RegisterType( resourceTypes[i], resource );
		}
	}

	public class CraftResources
	{
		private static CraftResourceInfo[] m_MetalInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1053109, "Iron", CraftAttributeInfo.Blank, CraftResource.Iron, typeof( IronIngot ), typeof( IronOre ), typeof( Granite ) ),
				new CraftResourceInfo( 0x973, 1053108, "Dull Copper", CraftAttributeInfo.DullCopper, CraftResource.DullCopper, typeof( DullCopperIngot ), typeof( DullCopperOre ), typeof( DullCopperGranite ) ),
				new CraftResourceInfo( 0x966, 1053107, "Shadow Iron", CraftAttributeInfo.ShadowIron, CraftResource.ShadowIron, typeof( ShadowIronIngot ), typeof( ShadowIronOre ), typeof( ShadowIronGranite ) ),
				new CraftResourceInfo( 0x96D, 1053106, "Copper", CraftAttributeInfo.Copper, CraftResource.Copper, typeof( CopperIngot ), typeof( CopperOre ), typeof( CopperGranite ) ),
				new CraftResourceInfo( 0x972, 1053105, "Bronze", CraftAttributeInfo.Bronze, CraftResource.Bronze, typeof( BronzeIngot ), typeof( BronzeOre ), typeof( BronzeGranite ) ),
				new CraftResourceInfo( 0x8A5, 1053104, "Gold", CraftAttributeInfo.Golden, CraftResource.Gold, typeof( GoldIngot ), typeof( GoldOre ), typeof( GoldGranite ) ),
				new CraftResourceInfo( 0x979, 1053103, "Agapite", CraftAttributeInfo.Agapite, CraftResource.Agapite, typeof( AgapiteIngot ), typeof( AgapiteOre ), typeof( AgapiteGranite ) ),
				new CraftResourceInfo( 0x89F, 1053102, "Verite", CraftAttributeInfo.Verite, CraftResource.Verite, typeof( VeriteIngot ), typeof( VeriteOre ), typeof( VeriteGranite ) ),
				new CraftResourceInfo( 0x8AB, 1053101, "Valorite", CraftAttributeInfo.Valorite, CraftResource.Valorite, typeof( ValoriteIngot ), typeof( ValoriteOre ), typeof( ValoriteGranite ) ),
			};

		private static CraftResourceInfo[] m_ScaleInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x66D, 1053129, "Red Scales", CraftAttributeInfo.RedScales, CraftResource.RedScales, typeof( RedScales ) ),
				new CraftResourceInfo( 0x8A8, 1053130, "Yellow Scales", CraftAttributeInfo.YellowScales, CraftResource.YellowScales, typeof( YellowScales ) ),
				new CraftResourceInfo( 0x455, 1053131, "Black Scales", CraftAttributeInfo.BlackScales, CraftResource.BlackScales, typeof( BlackScales ) ),
				new CraftResourceInfo( 0x851, 1053132, "Green Scales", CraftAttributeInfo.GreenScales, CraftResource.GreenScales, typeof( GreenScales ) ),
				new CraftResourceInfo( 0x8FD, 1053133, "White Scales", CraftAttributeInfo.WhiteScales, CraftResource.WhiteScales, typeof( WhiteScales ) ),
				new CraftResourceInfo( 0x8B0, 1053134, "Blue Scales", CraftAttributeInfo.BlueScales, CraftResource.BlueScales, typeof( BlueScales ) )
			};

		private static CraftResourceInfo[] m_LeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Normal", CraftAttributeInfo.Blank, CraftResource.RegularLeather, typeof( Leather ), typeof( Hides ) ),
				new CraftResourceInfo( 0x8AC, 1049354, "Spined", CraftAttributeInfo.Spined, CraftResource.SpinedLeather, typeof( SpinedLeather ), typeof( SpinedHides ) ),
				new CraftResourceInfo( 0x845, 1049355, "Horned", CraftAttributeInfo.Horned, CraftResource.HornedLeather, typeof( HornedLeather ), typeof( HornedHides ) ),
				new CraftResourceInfo( 0x851, 1049356, "Barbed", CraftAttributeInfo.Barbed, CraftResource.BarbedLeather, typeof( BarbedLeather ), typeof( BarbedHides ) ),
			};

		private static CraftResourceInfo[] m_WoodInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0, 0, "Wood", CraftAttributeInfo.Blank, CraftResource.Wood, typeof( Log ), typeof( Board ) ), 
				new CraftResourceInfo( 2010, 1072533, "Oak", CraftAttributeInfo.Oak, CraftResource.Oak, typeof( OakLog ), typeof( OakBoard ) ), 
				new CraftResourceInfo( 1191, 1072534, "Ash", CraftAttributeInfo.Ash, CraftResource.Ash, typeof( AshLog ), typeof( AshBoard ) ), 
				new CraftResourceInfo( 1192, 1072535, "Yew", CraftAttributeInfo.Yew, CraftResource.Yew, typeof( YewLog ), typeof( YewBoard ) ),
				new CraftResourceInfo( 1193, 1072536, "Heartwood", CraftAttributeInfo.Heartwood, CraftResource.Heartwood, typeof( HeartwoodLog ), typeof( HeartwoodBoard ) ),
				new CraftResourceInfo( 1194, 1072538, "Bloodwood", CraftAttributeInfo.Bloodwood, CraftResource.Bloodwood, typeof( BloodwoodLog ), typeof( BloodwoodBoard ) ),
				new CraftResourceInfo( 1151, 1072539, "Frostwood", CraftAttributeInfo.Frostwood, CraftResource.Frostwood, typeof( FrostwoodLog ), typeof( FrostwoodBoard ) ),
			};

		/// <summary>
		/// Returns true if '<paramref name="resource"/>' is None, Iron, or RegularLeather. False if otherwise.
		/// </summary>
		public static bool IsStandard( CraftResource resource )
		{
			return ( resource == CraftResource.None || resource == CraftResource.Iron || resource == CraftResource.RegularLeather || resource == CraftResource.Wood );
		}

		private static Hashtable m_TypeTable;

		/// <summary>
		/// Registers that '<paramref name="resourceType"/>' uses '<paramref name="resource"/>' so that it can later be queried by <see cref="CraftResources.GetFromType"/>
		/// </summary>
		public static void RegisterType( Type resourceType, CraftResource resource )
		{
			if ( m_TypeTable == null )
				m_TypeTable = new Hashtable();

			m_TypeTable[resourceType] = resource;
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value for which '<paramref name="resourceType"/>' uses -or- CraftResource.None if an unregistered type was specified.
		/// </summary>
		public static CraftResource GetFromType( Type resourceType )
		{
			if ( m_TypeTable == null )
				return CraftResource.None;

			object obj = m_TypeTable[resourceType];

			if ( !( obj is CraftResource ) )
				return CraftResource.None;

			return (CraftResource) obj;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceInfo"/> instance describing '<paramref name="resource"/>' -or- null if an invalid resource was specified.
		/// </summary>
		public static CraftResourceInfo GetInfo( CraftResource resource )
		{
			CraftResourceInfo[] list = null;

			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal:
					list = m_MetalInfo;
					break;
				case CraftResourceType.Leather:
					list = m_LeatherInfo;
					break;
				case CraftResourceType.Scales:
					list = m_ScaleInfo;
					break;
				case CraftResourceType.Wood:
					list = m_WoodInfo;
					break;
			}

			if ( list != null )
			{
				int index = GetIndex( resource );

				if ( index >= 0 && index < list.Length )
					return list[index];
			}

			return null;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceType"/> value indiciating the type of '<paramref name="resource"/>'.
		/// </summary>
		public static CraftResourceType GetType( CraftResource resource )
		{
			if ( resource >= CraftResource.Iron && resource <= CraftResource.Valorite )
				return CraftResourceType.Metal;

			if ( resource >= CraftResource.RegularLeather && resource <= CraftResource.BarbedLeather )
				return CraftResourceType.Leather;

			if ( resource >= CraftResource.RedScales && resource <= CraftResource.BlueScales )
				return CraftResourceType.Scales;

			if ( resource >= CraftResource.Wood && resource <= CraftResource.Frostwood )
				return CraftResourceType.Wood;

			return CraftResourceType.None;
		}

		/// <summary>
		/// Returns the first <see cref="CraftResource"/> in the series of resources for which '<paramref name="resource"/>' belongs.
		/// </summary>
		public static CraftResource GetStart( CraftResource resource )
		{
			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal:
					return CraftResource.Iron;
				case CraftResourceType.Leather:
					return CraftResource.RegularLeather;
				case CraftResourceType.Scales:
					return CraftResource.RedScales;
				case CraftResourceType.Wood:
					return CraftResource.Wood;
			}

			return CraftResource.None;
		}

		/// <summary>
		/// Returns the index of '<paramref name="resource"/>' in the seriest of resources for which it belongs.
		/// </summary>
		public static int GetIndex( CraftResource resource )
		{
			CraftResource start = GetStart( resource );

			if ( start == CraftResource.None )
				return 0;

			return (int) ( resource - start );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Number"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetLocalizationNumber( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Number );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Hue"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetHue( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Hue );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Name"/> property of '<paramref name="resource"/>' -or- an empty string if the resource specified was invalid.
		/// </summary>
		public static string GetName( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? String.Empty : info.Name );
		}
	}
}