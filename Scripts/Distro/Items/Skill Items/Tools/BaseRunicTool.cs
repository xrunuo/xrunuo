using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public abstract class BaseRunicTool : BaseTool
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); InvalidateProperties(); }
		}

		public BaseRunicTool( CraftResource resource, int itemID )
			: base( itemID )
		{
			m_Resource = resource;
		}

		public BaseRunicTool( CraftResource resource, int uses, int itemID )
			: base( uses, itemID )
		{
			m_Resource = resource;
		}

		public BaseRunicTool( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Resource = (CraftResource) reader.ReadInt();
						break;
					}
			}
		}

		private static bool m_IsRunicTool;
		private static int m_LuckChance;

		private static int Scale( int min, int max, int low, int high )
		{
			int percent;

			if ( m_IsRunicTool )
			{
				percent = Utility.RandomMinMax( min, max );
			}
			else
			{
				// Behold, the worst system ever!
				int v = Utility.RandomMinMax( 0, 10000 );

				v = (int) Math.Sqrt( v );
				v = 100 - v;

				if ( LootPack.CheckLuck( m_LuckChance ) )
					v += 10; // TODO: is this right? who knows

				if ( v < min )
					v = min;
				else if ( v > max )
					v = max;

				percent = v;
			}

			int scaledBy = Math.Abs( high - low ) + 1;

			if ( scaledBy != 0 )
				scaledBy = 10000 / scaledBy;

			percent *= ( 10000 + scaledBy );

			return low + ( ( ( high - low ) * percent ) / 1000001 );
		}

		private static void ApplyAttribute( MagicalAttributes attrs, int min, int max, MagicalAttribute attr, int low, int high )
		{
			ApplyAttribute( attrs, min, max, attr, low, high, 1 );
		}

		private static void ApplyAttribute( MagicalAttributes attrs, int min, int max, MagicalAttribute attr, int low, int high, int scale )
		{
			if ( attr == MagicalAttribute.CastSpeed )
				attrs[attr] += Scale( min, max, low / scale, high / scale ) * scale;
			else
				attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static void ApplyAttribute( ArmorAttributes attrs, int min, int max, ArmorAttribute attr, int low, int high )
		{
			attrs[attr] = Scale( min, max, low, high );
		}

		private static void ApplyAttribute( ArmorAttributes attrs, int min, int max, ArmorAttribute attr, int low, int high, int scale )
		{
			attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static void ApplyAttribute( WeaponAttributes attrs, int min, int max, WeaponAttribute attr, int low, int high )
		{
			attrs[attr] = Scale( min, max, low, high );
		}

		private static void ApplyAttribute( WeaponAttributes attrs, int min, int max, WeaponAttribute attr, int low, int high, int scale )
		{
			attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static void ApplyAttribute( ElementAttributes attrs, int min, int max, ElementAttribute attr, int low, int high )
		{
			ApplyAttribute( attrs, min, max, attr, low, high, 0 );
		}

		private static void ApplyAttribute( ElementAttributes attrs, int min, int max, ElementAttribute attr, int low, int high, int offset )
		{
			attrs[attr] = Scale( min, max, low + offset, high + offset );
		}

		private static SkillName[] m_PossibleBonusSkills = new SkillName[]
			{
				SkillName.Swords,
				SkillName.Fencing,
				SkillName.Macing,
				SkillName.Archery,
				SkillName.Wrestling,
				SkillName.Parry,
				SkillName.Tactics,
				SkillName.Anatomy,
				SkillName.Healing,
				SkillName.Magery,
				SkillName.Meditation,
				SkillName.EvalInt,
				SkillName.MagicResist,
				SkillName.AnimalTaming,
				SkillName.AnimalLore,
				SkillName.Veterinary,
				SkillName.Musicianship,
				SkillName.Provocation,
				SkillName.Discordance,
				SkillName.Peacemaking,
				SkillName.Chivalry,
				SkillName.Focus,
				SkillName.Necromancy,
				SkillName.Stealing,
				SkillName.Stealth,
				SkillName.SpiritSpeak,
				SkillName.Bushido,
				SkillName.Ninjitsu
			};

		private static SkillName[] m_PossibleSpellbookSkills = new SkillName[]
			{
				SkillName.Magery,
				SkillName.Meditation,
				SkillName.EvalInt,
				SkillName.MagicResist
			};

		private static void ApplySkillBonus( SkillBonuses attrs, int min, int max, int index, int low, int high )
		{
			SkillName[] possibleSkills = ( attrs.Owner is Spellbook ? m_PossibleSpellbookSkills : m_PossibleBonusSkills );
			int count = possibleSkills.Length;

			SkillName sk, check;
			double bonus;
			bool found;

			do
			{
				found = false;
				sk = possibleSkills[Utility.Random( count )];

				for ( int i = 0; !found && i < 5; ++i )
					found = ( attrs.GetValues( i, out check, out bonus ) && check == sk );
			} while ( found );

			attrs.SetValues( (short) index, sk, (short) Scale( min, max, low, high ) );
		}

		private const int MaxProperties = 32;
		private static BitArray m_Props = new BitArray( MaxProperties );
		private static int[] m_Possible = new int[MaxProperties];

		public static int GetUniqueRandom( int count )
		{
			int avail = 0;

			for ( int i = 0; i < count; ++i )
			{
				if ( !m_Props[i] )
					m_Possible[avail++] = i;
			}

			if ( avail == 0 )
				return -1;

			int v = m_Possible[Utility.Random( avail )];

			m_Props.Set( v, true );

			return v;
		}

		public void ApplyAttributesTo( BaseWeapon weapon )
		{
			CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

			if ( resInfo == null )
				return;

			CraftAttributeInfo attrs = resInfo.AttributeInfo;

			if ( attrs == null )
				return;

			int attributeCount = Utility.RandomMinMax( attrs.RunicMinAttributes, attrs.RunicMaxAttributes );
			int min = attrs.RunicMinIntensity;
			int max = attrs.RunicMaxIntensity;

			ApplyAttributesTo( weapon, true, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseWeapon weapon, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( weapon, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseWeapon weapon, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			MagicalAttributes primary = weapon.Attributes;
			WeaponAttributes secondary = weapon.WeaponAttributes;
			ElementAttributes resistances = weapon.Resistances;

			m_Props.SetAll( false );

			if ( weapon is BaseRanged || weapon is BaseThrowing )
			{
				m_Props.Set( 2, true ); // ranged weapons cannot be ubws or mageweapon
			}

			if ( !( weapon is BaseRanged ) )
			{
				m_Props.Set( 28, true ); // Solo los arcos pueden llevar la propiedad Balanced
				m_Props.Set( 29, true ); // Solo los arcos pueden llevar la propiedad Velocity
			}

			if ( isRunicTool )
				m_Props.Set( 16, true ); // Lower requirements don't spawn from runic tools

			bool isRunicToolRanged = isRunicTool && weapon is BaseRanged;

			for ( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 30 );

				if ( random == -1 )
					break;

				switch ( random )
				{
					case 0:
						{
							switch ( Utility.Random( 5 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, WeaponAttribute.HitPhysicalArea, 2, 50, 2 ); break;
								case 1: ApplyAttribute( secondary, min, max, WeaponAttribute.HitFireArea, 2, 50, 2 ); break;
								case 2: ApplyAttribute( secondary, min, max, WeaponAttribute.HitColdArea, 2, 50, 2 ); break;
								case 3: ApplyAttribute( secondary, min, max, WeaponAttribute.HitPoisonArea, 2, 50, 2 ); break;
								case 4: ApplyAttribute( secondary, min, max, WeaponAttribute.HitEnergyArea, 2, 50, 2 ); break;
							}

							break;
						}
					case 1:
						{
							switch ( Utility.Random( 4 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, WeaponAttribute.HitMagicArrow, 2, 50, 2 ); break;
								case 1: ApplyAttribute( secondary, min, max, WeaponAttribute.HitHarm, 2, 50, 2 ); break;
								case 2: ApplyAttribute( secondary, min, max, WeaponAttribute.HitFireball, 2, 50, 2 ); break;
								case 3: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLightning, 2, 50, 2 ); break;
							}

							break;
						}
					case 2:
						{
							switch ( Utility.Random( 2 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, WeaponAttribute.UseBestSkill, 1, 1 ); break;
								case 1: ApplyAttribute( secondary, min, max, WeaponAttribute.MageWeapon, 1, 10 ); break;
							}

							break;
						}
					case 3:
						{
							MagicalAttribute attr = MagicalAttribute.WeaponDamage;
							int oldValue = primary[attr];

							ApplyAttribute( primary, min, max, attr, 1, 50 );

							if ( oldValue >= primary[attr] )
							{
								primary[attr] = oldValue;
								i--;
							}

							break;
						}
					case 4: ApplyAttribute( primary, min, max, MagicalAttribute.DefendChance, 1, isRunicToolRanged ? 25 : 15 ); break;
					case 5: ApplyAttribute( primary, min, max, MagicalAttribute.CastSpeed, 1, 1 ); break;
					case 6: ApplyAttribute( primary, min, max, MagicalAttribute.AttackChance, 1, isRunicToolRanged ? 25 : 15 ); break;
					case 7: ApplyAttribute( primary, min, max, MagicalAttribute.Luck, 1, isRunicToolRanged ? 120 : 100 ); break;
					case 8: ApplyAttribute( primary, min, max, MagicalAttribute.WeaponSpeed, 5, 30, 5 ); break;
					case 9: ApplyAttribute( primary, min, max, MagicalAttribute.SpellChanneling, 1, 1 ); break;
					case 10: ApplyAttribute( secondary, min, max, WeaponAttribute.HitDispel, 2, 50, 2 ); break;
					case 11: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLeechHits, 2, 50, 2 ); break;
					case 12: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLowerAttack, 2, 50, 2 ); break;
					case 13: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLowerDefend, 2, 50, 2 ); break;
					case 14: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLeechMana, 2, 50, 2 ); break;
					case 15: ApplyAttribute( secondary, min, max, WeaponAttribute.HitLeechStam, 2, 50, 2 ); break;
					case 16: ApplyAttribute( secondary, min, max, WeaponAttribute.LowerStatReq, 10, 100, 10 ); break;
					case 17: ApplyAttribute( resistances, min, max, ElementAttribute.Physical, 1, 15 ); break;
					case 18: ApplyAttribute( resistances, min, max, ElementAttribute.Fire, 1, 15 ); break;
					case 19: ApplyAttribute( resistances, min, max, ElementAttribute.Cold, 1, 15 ); break;
					case 20: ApplyAttribute( resistances, min, max, ElementAttribute.Poison, 1, 15 ); break;
					case 21: ApplyAttribute( resistances, min, max, ElementAttribute.Energy, 1, 15 ); break;
					case 22: ApplyAttribute( secondary, min, max, WeaponAttribute.DurabilityBonus, 10, 100, 10 ); i--; break;
					case 23: SlayerName slayer = GetRandomSlayer();
						if ( weapon.Slayer == SlayerName.None )
							weapon.Slayer = slayer;
						else
							weapon.Slayer2 = slayer;
						break;
					case 24: AssignElementalDamage( weapon, ElementAttribute.Fire ); i--; break;
					case 25: AssignElementalDamage( weapon, ElementAttribute.Cold ); i--; break;
					case 26: AssignElementalDamage( weapon, ElementAttribute.Poison ); i--; break;
					case 27: AssignElementalDamage( weapon, ElementAttribute.Energy ); i--; break;
					case 28: ApplyAttribute( secondary, min, max, WeaponAttribute.Balanced, 1, 1 ); break;
					case 29: ApplyAttribute( secondary, min, max, WeaponAttribute.Velocity, 10, 50, 1 ); break;
				}
			}
		}

		public static void AssignElementalDamage( BaseWeapon weapon, ElementAttribute attr )
		{
			int fire, phys, cold, nrgy, pois;

			weapon.GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy );

			AssignElementalDamage( weapon, attr, phys );

			weapon.Hue = weapon.GetElementalDamageHue();
		}

		private static int AssignElementalDamage( BaseWeapon weapon, ElementAttribute attr, int totalDamage )
		{
			if ( totalDamage <= 0 )
				return 0;

			int random = Utility.Random( (int) ( totalDamage / 10 ) + 1 ) * 10;

			// TODO: Refactor this
			// weapon.ElementDamages[attr] = random;

			switch ( attr )
			{
				case ElementAttribute.Cold:
					weapon.WeaponAttributes.DamageColdPercent = random;
					break;
				case ElementAttribute.Energy:
					weapon.WeaponAttributes.DamageEnergyPercent = random;
					break;
				case ElementAttribute.Fire:
					weapon.WeaponAttributes.DamageFirePercent = random;
					break;
				case ElementAttribute.Poison:
					weapon.WeaponAttributes.DamagePoisonPercent = random;
					break;
			}

			return ( totalDamage - random );
		}

		public static SlayerName GetRandomSlayer()
		{
			// TODO: Check random algorithm on OSI

			SlayerGroup[] groups = SlayerGroup.Groups;

			if ( groups.Length == 0 )
				return SlayerName.None;

			SlayerGroup group = groups[Utility.Random( groups.Length - 1 )]; //-1 To Exclude the Fey Slayer which appears ONLY on a certain artifact.
			SlayerEntry entry;

			if ( 10 > Utility.Random( 100 ) ) // 10% chance to do super slayer
			{
				entry = group.Super;
			}
			else
			{
				SlayerEntry[] entries = group.Entries;

				if ( entries.Length == 0 )
					return GetRandomSlayer();

				entry = entries[Utility.Random( entries.Length )];

				// Daemon Slayers will no longer spawn
				if ( entry.Name == SlayerName.Unused1 || entry.Name == SlayerName.Unused2 )
					return GetRandomSlayer(); // Recursiva, no veo una manera mas aleatoria...
			}

			return entry.Name;
		}

		public void ApplyAttributesTo( BaseArmor armor )
		{
			CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

			if ( resInfo == null )
				return;

			CraftAttributeInfo attrs = resInfo.AttributeInfo;

			if ( attrs == null )
				return;

			int attributeCount = Utility.RandomMinMax( attrs.RunicMinAttributes, attrs.RunicMaxAttributes );
			int min = attrs.RunicMinIntensity;
			int max = attrs.RunicMaxIntensity;

			ApplyAttributesTo( armor, true, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseArmor armor, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( armor, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseArmor armor, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			MagicalAttributes primary = armor.Attributes;
			ArmorAttributes secondary = armor.ArmorAttributes;
			ElementAttributes resistances = armor.Resistances;

			m_Props.SetAll( false );

			bool isShield = ( armor is BaseShield );
			int baseCount = ( isShield ? 8 : 20 );
			int baseOffset = ( isShield ? 0 : 4 );

			if ( !isShield && armor.Meditable )
				m_Props.Set( 4, true ); // remove mage armor from possible properties

			if ( armor.PlayerConstructed )
				m_Props.Set( 2, true ); // remove durability bonus from crafted armor

			if ( armor.MaterialType == ArmorMaterialType.Leather )
				m_Props.Set( 0, true ); // remove lower stat requiments from leather armor

			for ( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( baseCount );

				if ( random == -1 )
					break;

				random += baseOffset;

				switch ( random )
				{
					/* Begin Shields */
					case 0: ApplyAttribute( primary, min, max, MagicalAttribute.SpellChanneling, 1, 1 ); break;
					case 1: ApplyAttribute( primary, min, max, MagicalAttribute.DefendChance, 1, 15 ); break;
					case 2: ApplyAttribute( primary, min, max, MagicalAttribute.AttackChance, 1, 15 ); break;
					case 3: ApplyAttribute( primary, min, max, MagicalAttribute.CastSpeed, 1, 1 ); break;
					/* Begin Armor */
					case 4: ApplyAttribute( secondary, min, max, ArmorAttribute.LowerStatReq, 10, 100, 10 ); break;
					case 5: ApplyAttribute( secondary, min, max, ArmorAttribute.SelfRepair, 1, 5 ); break;
					case 6: ApplyAttribute( secondary, min, max, ArmorAttribute.DurabilityBonus, 10, 100, 10 ); break;
					case 7: ApplyAttribute( primary, min, max, MagicalAttribute.ReflectPhysical, 1, 15 ); break;
					/* End Shields */
					case 8: ApplyAttribute( secondary, min, max, ArmorAttribute.MageArmor, 1, 1 ); break;
					case 9: ApplyAttribute( primary, min, max, MagicalAttribute.RegenHits, 1, 2 ); break;
					case 10: ApplyAttribute( primary, min, max, MagicalAttribute.RegenStam, 1, 3 ); break;
					case 11: ApplyAttribute( primary, min, max, MagicalAttribute.RegenMana, 1, 2 ); break;
					case 12: ApplyAttribute( primary, min, max, MagicalAttribute.NightSight, 1, 1 ); break;
					case 13: ApplyAttribute( primary, min, max, MagicalAttribute.BonusHits, 1, 5 ); break;
					case 14: ApplyAttribute( primary, min, max, MagicalAttribute.BonusStam, 1, 8 ); break;
					case 15: ApplyAttribute( primary, min, max, MagicalAttribute.BonusMana, 1, 8 ); break;
					case 16: ApplyAttribute( primary, min, max, MagicalAttribute.LowerManaCost, 1, 8 ); break;
					case 17: ApplyAttribute( primary, min, max, MagicalAttribute.LowerRegCost, 1, 20 ); break;
					case 18: ApplyAttribute( primary, min, max, MagicalAttribute.Luck, 1, 100 ); break;
					case 19:
						{
							ApplyAttribute( resistances, min, max, ElementAttribute.Physical, 1, 15, armor.PhysicalBonus );
							armor.PhysicalBonus = 0;
							break;
						}
					case 20:
						{
							ApplyAttribute( resistances, min, max, ElementAttribute.Fire, 1, 15, armor.FireBonus );
							armor.FireBonus = 0;
							break;
						}
					case 21:
						{
							ApplyAttribute( resistances, min, max, ElementAttribute.Cold, 1, 15, armor.ColdBonus );
							armor.ColdBonus = 0;
							break;
						}
					case 22:
						{
							ApplyAttribute( resistances, min, max, ElementAttribute.Poison, 1, 15, armor.PoisonBonus );
							armor.PoisonBonus = 0;
							break;
						}
					case 23:
						{
							ApplyAttribute( resistances, min, max, ElementAttribute.Energy, 1, 15, armor.EnergyBonus );
							armor.EnergyBonus = 0;
							break;
						}
					/* End Armor */
				}
			}
		}

		public static void ApplyAttributesTo( BaseHat hat, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( hat, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseHat hat, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			MagicalAttributes primary = hat.Attributes;
			ArmorAttributes secondary = hat.ClothingAttributes;
			ElementAttributes resists = hat.Resistances;

			m_Props.SetAll( false );

			for ( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 19 );

				if ( random == -1 )
					break;

				switch ( random )
				{
					case 0: ApplyAttribute( secondary, min, max, ArmorAttribute.LowerStatReq, 10, 100, 10 ); break;
					case 1: ApplyAttribute( secondary, min, max, ArmorAttribute.SelfRepair, 1, 5 ); break;
					case 2: ApplyAttribute( secondary, min, max, ArmorAttribute.DurabilityBonus, 10, 100, 10 ); break;
					case 3: ApplyAttribute( primary, min, max, MagicalAttribute.ReflectPhysical, 1, 15 ); break;
					case 4: ApplyAttribute( primary, min, max, MagicalAttribute.RegenHits, 1, 2 ); break;
					case 5: ApplyAttribute( primary, min, max, MagicalAttribute.RegenStam, 1, 3 ); break;
					case 6: ApplyAttribute( primary, min, max, MagicalAttribute.RegenMana, 1, 2 ); break;
					case 7: ApplyAttribute( primary, min, max, MagicalAttribute.NightSight, 1, 1 ); break;
					case 8: ApplyAttribute( primary, min, max, MagicalAttribute.BonusHits, 1, 5 ); break;
					case 9: ApplyAttribute( primary, min, max, MagicalAttribute.BonusStam, 1, 8 ); break;
					case 10: ApplyAttribute( primary, min, max, MagicalAttribute.BonusMana, 1, 8 ); break;
					case 11: ApplyAttribute( primary, min, max, MagicalAttribute.LowerManaCost, 1, 8 ); break;
					case 12: ApplyAttribute( primary, min, max, MagicalAttribute.LowerRegCost, 1, 20 ); break;
					case 13: ApplyAttribute( primary, min, max, MagicalAttribute.Luck, 1, 100 ); break;
					case 14: ApplyAttribute( resists, min, max, ElementAttribute.Physical, 1, 15 ); break;
					case 15: ApplyAttribute( resists, min, max, ElementAttribute.Fire, 1, 15 ); break;
					case 16: ApplyAttribute( resists, min, max, ElementAttribute.Cold, 1, 15 ); break;
					case 17: ApplyAttribute( resists, min, max, ElementAttribute.Poison, 1, 15 ); break;
					case 18: ApplyAttribute( resists, min, max, ElementAttribute.Energy, 1, 15 ); break;
				}
			}
		}

		public static void ApplyAttributesTo( BaseJewel jewelry, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( jewelry, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseJewel jewelry, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			MagicalAttributes primary = jewelry.Attributes;
			ElementAttributes resists = jewelry.Resistances;
			SkillBonuses skills = jewelry.SkillBonuses;

			m_Props.SetAll( false );

			for ( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 24 );

				if ( random == -1 )
					break;

				switch ( random )
				{
					case 0: ApplyAttribute( resists, min, max, ElementAttribute.Physical, 1, 15 ); break;
					case 1: ApplyAttribute( resists, min, max, ElementAttribute.Fire, 1, 15 ); break;
					case 2: ApplyAttribute( resists, min, max, ElementAttribute.Cold, 1, 15 ); break;
					case 3: ApplyAttribute( resists, min, max, ElementAttribute.Poison, 1, 15 ); break;
					case 4: ApplyAttribute( resists, min, max, ElementAttribute.Energy, 1, 15 ); break;
					case 5: ApplyAttribute( primary, min, max, MagicalAttribute.WeaponDamage, 1, 25 ); break;
					case 6: ApplyAttribute( primary, min, max, MagicalAttribute.DefendChance, 1, 15 ); break;
					case 7: ApplyAttribute( primary, min, max, MagicalAttribute.AttackChance, 1, 15 ); break;
					case 8: ApplyAttribute( primary, min, max, MagicalAttribute.BonusStr, 1, 8 ); break;
					case 9: ApplyAttribute( primary, min, max, MagicalAttribute.BonusDex, 1, 8 ); break;
					case 10: ApplyAttribute( primary, min, max, MagicalAttribute.BonusInt, 1, 8 ); break;
					case 11: ApplyAttribute( primary, min, max, MagicalAttribute.EnhancePotions, 5, 25, 5 ); break;
					case 12: ApplyAttribute( primary, min, max, MagicalAttribute.CastSpeed, 1, 1 ); break;
					case 13: ApplyAttribute( primary, min, max, MagicalAttribute.CastRecovery, 1, 3 ); break;
					case 14: ApplyAttribute( primary, min, max, MagicalAttribute.LowerManaCost, 1, 8 ); break;
					case 15: ApplyAttribute( primary, min, max, MagicalAttribute.LowerRegCost, 1, 20 ); break;
					case 16: ApplyAttribute( primary, min, max, MagicalAttribute.Luck, 1, 100 ); break;
					case 17: ApplyAttribute( primary, min, max, MagicalAttribute.SpellDamage, 1, 12 ); break;
					case 18: ApplyAttribute( primary, min, max, MagicalAttribute.NightSight, 1, 1 ); break;
					case 19: ApplySkillBonus( skills, min, max, 0, 1, 15 ); break;
					case 20: ApplySkillBonus( skills, min, max, 1, 1, 15 ); break;
					case 21: ApplySkillBonus( skills, min, max, 2, 1, 15 ); break;
					case 22: ApplySkillBonus( skills, min, max, 3, 1, 15 ); break;
					case 23: ApplySkillBonus( skills, min, max, 4, 1, 15 ); break;
				}
			}
		}

		public static void ApplyAttributesTo( Spellbook spellbook, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( spellbook, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( Spellbook spellbook, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			MagicalAttributes primary = spellbook.Attributes;
			SkillBonuses skills = spellbook.SkillBonuses;

			m_Props.SetAll( false );

			for ( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 12 );

				if ( random == -1 )
					break;

				switch ( random )
				{
					case 0: ApplyAttribute( primary, min, max, MagicalAttribute.LowerRegCost, 1, 16 ); break;
					case 1: ApplyAttribute( primary, min, max, MagicalAttribute.LowerManaCost, 1, 6 ); break;
					case 2: ApplyAttribute( primary, min, max, MagicalAttribute.RegenMana, 1, 1 ); break;
					case 3:
						{
							ApplyAttribute( primary, min, max, MagicalAttribute.BonusInt, 1, 6 );

							for ( int j = 0; j < 4; ++j )
								m_Props.Set( j, true );

							break;
						}
					case 4: ApplyAttribute( primary, min, max, MagicalAttribute.BonusMana, 1, 6 ); break;
					case 5: ApplyAttribute( primary, min, max, MagicalAttribute.CastSpeed, 1, 1 ); break;
					case 6: ApplyAttribute( primary, min, max, MagicalAttribute.CastRecovery, 1, 2 ); break;
					case 7: ApplyAttribute( primary, min, max, MagicalAttribute.SpellDamage, 1, 9 ); break;
					case 8: ApplySkillBonus( skills, min, max, 0, 1, 12 ); break;
					case 9: ApplySkillBonus( skills, min, max, 1, 1, 12 ); break;
					case 10: ApplySkillBonus( skills, min, max, 2, 1, 12 ); break;
					case 11: ApplySkillBonus( skills, min, max, 3, 1, 12 ); break;
				}
			}
		}
	}
}
