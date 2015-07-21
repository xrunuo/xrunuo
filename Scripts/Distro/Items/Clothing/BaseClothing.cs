using System;
using System.Collections;
using System.Linq;

using Server;
using Server.Network;
using Server.Engines.Craft;
using Server.Engines.Imbuing;
using Server.Factions;
using Server.Mobiles;

namespace Server.Items
{
	public interface IArcaneEquip
	{
		bool IsArcane { get; }
		int CurArcaneCharges { get; set; }
		int MaxArcaneCharges { get; set; }
	}

	public abstract class BaseClothing : Item, IDyable, IScissorable, IFactionItem, ICraftable, IMagicalItem, ICloth, IWearableDurability, ISkillBonuses, IResistances, IAbsorption, IImbuable, IArtifactRarity
	{
		#region Factions
		private FactionItem m_FactionState;

		public FactionItem FactionItemState
		{
			get { return m_FactionState; }
			set
			{
				m_FactionState = value;

				if ( m_FactionState == null )
				{
					Hue = 0;
				}

				LootType = ( m_FactionState == null ? LootType.Regular : LootType.Blessed );
			}
		}
		#endregion

		protected CraftResource m_Resource;

		private Mobile m_Crafter;
		private bool m_Exceptional;
		private bool m_PlayerConstructed;

		private bool m_Altered;

		private MagicalAttributes m_MagicalAttributes;
		private ArmorAttributes m_ClothingAttributes;
		private SkillBonuses m_SkillBonuses;
		private ElementAttributes m_Resistances;
		private AbsorptionAttributes m_AbsorptionAttributes;

		public virtual bool AllowMaleWearer { get { return true; } }
		public virtual bool AllowFemaleWearer { get { return true; } }

		public virtual int StrengthReq { get { return 10; } }

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
			set { m_Exceptional = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PlayerConstructed
		{
			get { return m_PlayerConstructed; }
			set { m_PlayerConstructed = value; }
		}

		public virtual CraftResource DefaultResource { get { return CraftResource.None; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue( m_Resource );
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MagicalAttributes Attributes
		{
			get { return m_MagicalAttributes; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ArmorAttributes ClothingAttributes
		{
			get { return m_ClothingAttributes; }
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

		public virtual bool CanLoseDurability { get { return m_HitPoints >= 0 && m_MaxHitPoints > 0; } }

		public virtual int InitMinHits { get { return 0; } }
		public virtual int InitMaxHits { get { return 0; } }

		public virtual bool CanFortify { get { return false; } }
		public virtual bool Brittle { get { return false; } }
		public virtual bool CannotBeRepaired { get { return false; } }

		private int m_MaxHitPoints;
		private int m_HitPoints;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get { return m_MaxHitPoints; }
			set
			{
				m_MaxHitPoints = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitPoints
		{
			get { return m_HitPoints; }
			set
			{
				if ( value != m_HitPoints && MaxHitPoints > 0 )
				{
					m_HitPoints = value;

					if ( m_HitPoints < 0 )
					{
						Delete();
					}
					else if ( m_HitPoints > MaxHitPoints )
					{
						m_HitPoints = MaxHitPoints;
					}

					InvalidateProperties();
				}
			}
		}

		public virtual int BasePhysicalResistance { get { return 0; } }
		public virtual int BaseFireResistance { get { return 0; } }
		public virtual int BaseColdResistance { get { return 0; } }
		public virtual int BasePoisonResistance { get { return 0; } }
		public virtual int BaseEnergyResistance { get { return 0; } }

		public override int PhysicalResistance { get { return BasePhysicalResistance + m_Resistances.Physical; } }
		public override int FireResistance { get { return BaseFireResistance + m_Resistances.Fire; } }
		public override int ColdResistance { get { return BaseColdResistance + m_Resistances.Cold; } }
		public override int PoisonResistance { get { return BasePoisonResistance + m_Resistances.Poison; } }
		public override int EnergyResistance { get { return BaseEnergyResistance + m_Resistances.Energy; } }

		public virtual int ArtifactRarity { get { return 0; } }

		public virtual int BaseStrBonus { get { return 0; } }
		public virtual int BaseDexBonus { get { return 0; } }
		public virtual int BaseIntBonus { get { return 0; } }

		public virtual bool CanBeBlessed { get { return true; } }

		public int ComputeStatBonus( StatType type )
		{
			if ( type == StatType.Str )
			{
				return BaseStrBonus + Attributes.BonusStr;
			}
			else if ( type == StatType.Dex )
			{
				return BaseDexBonus + Attributes.BonusDex;
			}
			else
			{
				return BaseIntBonus + Attributes.BonusInt;
			}
		}

		public virtual void AddStatBonuses( Mobile parent )
		{
			if ( parent == null )
			{
				return;
			}

			int strBonus = ComputeStatBonus( StatType.Str );
			int dexBonus = ComputeStatBonus( StatType.Dex );
			int intBonus = ComputeStatBonus( StatType.Int );

			if ( strBonus == 0 && dexBonus == 0 && intBonus == 0 )
			{
				return;
			}

			string modName = this.Serial.ToString();

			if ( strBonus != 0 )
			{
				parent.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );
			}

			if ( dexBonus != 0 )
			{
				parent.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );
			}

			if ( intBonus != 0 )
			{
				parent.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}
		}

		public virtual int OnHit( int damageTaken )
		{
			int Absorbed = 2; // TODO: verify

			damageTaken -= Absorbed;
			if ( damageTaken < 0 )
			{
				damageTaken = 0;
			}

			if ( 25 > Utility.Random( 100 ) ) // 25% chance to lower durability
			{
				if ( m_ClothingAttributes.SelfRepair > Utility.Random( 10 ) )
				{
					HitPoints += 2;
				}
				else
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
									( (Mobile) Parent ).LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061121 ); // Your equipment is severely damaged.
							}
							else
							{
								Delete();
							}
						}
					}
				}
			}

			return damageTaken;
		}

		public override void OnAdded( object parent )
		{
			Mobile mob = parent as Mobile;

			if ( mob != null )
			{
				m_SkillBonuses.AddTo( mob );

				AddStatBonuses( mob );
				mob.CheckStatTimers();
			}

			base.OnAdded( parent );
		}

		public override void OnRemoved( object parent )
		{
			Mobile mob = parent as Mobile;

			if ( mob != null )
			{
				m_SkillBonuses.Remove();

				string modName = this.Serial.ToString();

				mob.RemoveStatMod( modName + "Str" );
				mob.RemoveStatMod( modName + "Dex" );
				mob.RemoveStatMod( modName + "Int" );

				mob.CheckStatTimers();
			}

			base.OnRemoved( parent );
		}

		public BaseClothing( int itemID, Layer layer )
			: this( itemID, layer, 0 )
		{
		}

		public BaseClothing( int itemID, Layer layer, int hue )
			: base( itemID )
		{
			Layer = layer;
			Hue = hue;

			m_Resource = DefaultResource;

			m_MagicalAttributes = new MagicalAttributes( this );
			m_ClothingAttributes = new ArmorAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );
			m_Resistances = new ElementAttributes( this );
			m_AbsorptionAttributes = new AbsorptionAttributes( this );

			m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );
		}

		public BaseClothing( Serial serial )
			: base( serial )
		{
		}

		public override void OnAfterDuped( Item newItem )
		{
			// TODO: Copy Attributes
		}

		public static void ValidateMobile( Mobile m )
		{
			if ( m.AccessLevel >= AccessLevel.GameMaster )
				return;

			foreach ( var clothing in m.GetEquippedItems().OfType<BaseClothing>() )
			{
				if ( !clothing.AllowMaleWearer && m.Body.IsMale )
				{
					if ( clothing.AllowFemaleWearer )
						m.SendLocalizedMessage( 1010388 ); // Only females can wear this.
					else
						m.SendMessage( "You may not wear this." );

					m.AddToBackpack( clothing );
				}
				else if ( !clothing.AllowFemaleWearer && m.Body.IsFemale )
				{
					if ( clothing.AllowMaleWearer )
						m.SendMessage( "Only males can wear this." );
					else
						m.SendMessage( "You may not wear this." );

					m.AddToBackpack( clothing );
				}
			}
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( base.AllowEquipedCast( from ) )
			{
				return true;
			}

			return ( m_MagicalAttributes.SpellChanneling != 0 );
		}

		public override bool CheckPropertyConfliction( Mobile m )
		{
			if ( base.CheckPropertyConfliction( m ) )
			{
				return true;
			}

			if ( Layer == Layer.Pants )
			{
				return ( m.FindItemOnLayer( Layer.InnerLegs ) != null );
			}

			if ( Layer == Layer.Shirt )
			{
				return ( m.FindItemOnLayer( Layer.InnerTorso ) != null );
			}

			return false;
		}

		private string GetNameString()
		{
			string name = this.Name;

			if ( name == null )
			{
				name = String.Format( "#{0}", LabelNumber );
			}

			return name;
		}

		public override LocalizedText GetNameProperty()
		{
			int oreType;

			if ( Hue == 0 )
			{
				oreType = 0;
			}
			else
			{
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
			}

			if ( oreType != 0 )
			{
				return new LocalizedText( 1053099, "#{0}\t{1}", oreType, GetNameString() ); // ~1_oretype~ ~2_armortype~
			}
			else if ( Name == null )
			{
				return new LocalizedText( LabelNumber );
			}
			else
			{
				return new LocalizedText( Name );
			}
		}

		public virtual void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
		}

		public virtual void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Exceptional )
				list.Add( 1060636 ); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Altered )
				list.Add( 1111880 ); // Altered

			#region Factions
			if ( m_FactionState != null )
			{
				list.Add( 1041350 ); // faction item
			}
			#endregion

			GetSetArmorPropertiesFirst( list );

			m_SkillBonuses.GetProperties( list );

			int prop;

			if ( ( prop = ArtifactRarity ) > 0 )
				list.Add( 1061078, prop.ToString() ); // artifact rarity ~1_val~

			if ( ( prop = m_AbsorptionAttributes.KineticEater ) != 0 )
				list.Add( 1113597, prop.ToString() ); // Kinetic Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.FireEater ) != 0 )
				list.Add( 1113593, prop.ToString() ); // Fire Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.ColdEater ) != 0 )
				list.Add( 1113594, prop.ToString() ); // Cold Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.PoisonEater ) != 0 )
				list.Add( 1113595, prop.ToString() ); // Poison Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.EnergyEater ) != 0 )
				list.Add( 1113596, prop.ToString() ); // Energy Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.DamageEater ) != 0 )
				list.Add( 1113598, prop.ToString() ); // Damage Eater ~1_Val~%

			if ( ( prop = m_AbsorptionAttributes.KineticResonance ) != 0 )
				list.Add( 1113695, prop.ToString() ); // Kinetic Resonance ~1_val~%

			if ( ( prop = m_AbsorptionAttributes.FireResonance ) != 0 )
				list.Add( 1113691, prop.ToString() ); // Fire Resonance ~1_val~%

			if ( ( prop = m_AbsorptionAttributes.ColdResonance ) != 0 )
				list.Add( 1113692, prop.ToString() ); // Cold Resonance ~1_val~%

			if ( ( prop = m_AbsorptionAttributes.PoisonResonance ) != 0 )
				list.Add( 1113693, prop.ToString() ); // Poison Resonance ~1_val~%

			if ( ( prop = m_AbsorptionAttributes.EnergyResonance ) != 0 )
				list.Add( 1113694, prop.ToString() ); // Energy Resonance ~1_val~%

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

			if ( ( prop = m_ClothingAttributes.LowerStatReq ) != 0 )
				list.Add( 1060435, prop.ToString() ); // lower requirements ~1_val~%

			if ( ( prop = m_MagicalAttributes.Luck ) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( ( prop = m_ClothingAttributes.MageArmor ) != 0 )
				list.Add( 1060437 ); // mage armor

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

			if ( ( prop = m_ClothingAttributes.SelfRepair ) != 0 )
				list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

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
				list.Add( 1075210, prop.ToString() ); // Increased Karma Loss ~1_val~%

			if ( ( prop = m_MagicalAttributes.CastingFocus ) != 0 )
				list.Add( 1113696, prop.ToString() ); // Casting Focus ~1_val~%

			if ( ( prop = m_MagicalAttributes.LowerAmmoCost ) != 0 )
				list.Add( 1075208, prop.ToString() ); // Lower Ammo Cost ~1_Percentage~%

			base.AddResistanceProperties( list );

			if ( Brittle )
				list.Add( 1116209 ); // Brittle

			if ( CanLoseDurability )
				list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~

			if ( StrengthReq > 0 )
				list.Add( 1061170, StrengthReq.ToString() ); // strength requirement ~1_val~

			GetSetArmorPropertiesSecond( list );
		}

		public override bool CanEquip( Mobile from )
		{
			if ( !AllowMaleWearer && from.Body.IsMale && from.AccessLevel < AccessLevel.GameMaster )
			{
				if ( AllowFemaleWearer )
					from.SendLocalizedMessage( 1010388 ); // Only females can wear this.
				else
					from.SendMessage( "You may not wear this." );

				return false;
			}
			else if ( !AllowFemaleWearer && from.Body.IsFemale && from.AccessLevel < AccessLevel.GameMaster )
			{
				if ( AllowMaleWearer )
					from.SendMessage( "Only males can wear this." );
				else
					from.SendMessage( "You may not wear this." );

				return false;
			}
			else if ( from.Str < StrengthReq )
			{
				from.SendLocalizedMessage( 500213 ); // You are not strong enough to equip that.
				return false;
			}
			else
			{
				return base.CanEquip( from );
			}
		}

		#region Serialization
		private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
		{
			if ( setIf )
				flags |= toSet;
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( ( flags & toGet ) != 0 );
		}

		[Flags]
		private enum SaveFlag
		{
			None = 0x00000000,
			Resource = 0x00000001,
			Attributes = 0x00000002,
			ClothingAttributes = 0x00000004,
			SkillBonuses = 0x00000008,
			Resistances = 0x00000010,
			MaxHitPoints = 0x00000020,
			HitPoints = 0x00000040,
			PlayerConstructed = 0x00000080,
			Crafter = 0x00000100,
			Exceptional = 0x00000200,
			AbsorptionAttributes = 0x00000400,
			Altered = 0x00000800
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 9 ); // version

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.Resource, m_Resource != DefaultResource );
			SetSaveFlag( ref flags, SaveFlag.Attributes, !m_MagicalAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.ClothingAttributes, !m_ClothingAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.SkillBonuses, !m_SkillBonuses.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Resistances, !m_Resistances.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.MaxHitPoints, m_MaxHitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.HitPoints, m_HitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed != false );
			SetSaveFlag( ref flags, SaveFlag.Crafter, m_Crafter != null );
			SetSaveFlag( ref flags, SaveFlag.Exceptional, m_Exceptional != false );
			SetSaveFlag( ref flags, SaveFlag.AbsorptionAttributes, !m_AbsorptionAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Altered, m_Altered );

			writer.WriteEncodedInt( (int) flags );

			if ( GetSaveFlag( flags, SaveFlag.Resource ) )
				writer.Write( (int) m_Resource );

			if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
				m_MagicalAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.ClothingAttributes ) )
				m_ClothingAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
				m_SkillBonuses.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
				m_Resistances.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
				writer.Write( (int) m_MaxHitPoints );

			if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
				writer.Write( (int) m_HitPoints );

			if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
				writer.Write( (Mobile) m_Crafter );

			if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
				m_AbsorptionAttributes.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 9:
					{
						SaveFlag flags = (SaveFlag) reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.Resource ) )
							m_Resource = (CraftResource) reader.ReadInt();
						else
							m_Resource = DefaultResource;

						if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
							m_MagicalAttributes = new MagicalAttributes( this, reader );
						else
							m_MagicalAttributes = new MagicalAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.ClothingAttributes ) )
							m_ClothingAttributes = new ArmorAttributes( this, reader );
						else
							m_ClothingAttributes = new ArmorAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
							m_SkillBonuses = new SkillBonuses( this, reader );
						else
							m_SkillBonuses = new SkillBonuses( this );

						if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
							m_Resistances = new ElementAttributes( this, reader );
						else
							m_Resistances = new ElementAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
							m_MaxHitPoints = reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
							m_HitPoints = reader.ReadInt();

						if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
							m_Crafter = reader.ReadMobile();

						if ( GetSaveFlag( flags, SaveFlag.Exceptional ) )
							m_Exceptional = true;

						if ( GetSaveFlag( flags, SaveFlag.PlayerConstructed ) )
							m_PlayerConstructed = true;

						if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
							m_AbsorptionAttributes = new AbsorptionAttributes( this, reader );
						else
							m_AbsorptionAttributes = new AbsorptionAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.Altered ) )
							m_Altered = true;

						break;
					}
				case 8:
					{
						m_AbsorptionAttributes = new AbsorptionAttributes( this, reader );

						m_MaxHitPoints = reader.ReadInt();
						m_HitPoints = reader.ReadInt();

						m_Resource = (CraftResource) reader.ReadInt();

						m_MagicalAttributes = new MagicalAttributes( this, reader );
						m_ClothingAttributes = new ArmorAttributes( this, reader );
						m_SkillBonuses = new SkillBonuses( this, reader );
						m_Resistances = new ElementAttributes( this, reader );

						break;
					}
			}

			Mobile parent = Parent as Mobile;

			if ( parent != null )
			{
				m_SkillBonuses.AddTo( parent );

				AddStatBonuses( parent );
				parent.CheckStatTimers();
			}
		}
		#endregion

		public virtual bool Dye( Mobile from, IDyeTub sender )
		{
			if ( Deleted )
			{
				return false;
			}
			else if ( RootParent is Mobile && from != RootParent )
			{
				return false;
			}

			Hue = sender.DyedHue;

			return true;
		}

		public virtual bool Scissor( Mobile from, Scissors scissors )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 502437 ); // Items you wish to cut must be in your backpack.
				return false;
			}

			CraftSystem system = DefTailoring.CraftSystem;

			CraftItem item = system.CraftItems.SearchFor( GetType() );

			if ( item != null && item.Ressources.Count == 1 && item.Ressources.GetAt( 0 ).Amount >= 2 )
			{
				try
				{
					Type resourceType = null;

					CraftResourceInfo info = CraftResources.GetInfo( m_Resource );

					if ( info != null && info.ResourceTypes.Length > 0 )
					{
						resourceType = info.ResourceTypes[0];
					}

					if ( resourceType == null )
					{
						resourceType = item.Ressources.GetAt( 0 ).ItemType;
					}

					Item res = (Item) Activator.CreateInstance( resourceType );

					ScissorHelper( from, res, m_PlayerConstructed ? ( item.Ressources.GetAt( 0 ).Amount / 2 ) : 1 );

					res.LootType = LootType.Regular;

					return true;
				}
				catch
				{
				}
			}

			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}

		#region ICraftable Members
		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			if ( DefaultResource != CraftResource.None )
			{
				Type resourceType = typeRes;

				if ( resourceType == null )
					resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

				Resource = CraftResources.GetFromType( resourceType );
			}
			else
			{
				Hue = resHue;
			}

			PlayerConstructed = true;

			CraftContext context = craftSystem.GetContext( from );

			if ( context != null && context.DoNotColor )
				Hue = 0;

			return exceptional;
		}
		#endregion

		#region IImbuable Members

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int TimesImbued { get { return 0; } set { } }

		public virtual ImbuingFlag ImbuingFlags { get { return ImbuingFlag.None; } }
		public virtual bool IsSpecialMaterial { get { return false; } }
		public virtual int MaxIntensity { get { return 500; } }
		public virtual bool CanImbue { get { return false; } }

		public virtual void OnImbued()
		{
		}
		#endregion

		#region Alter
		public virtual void AlterFrom( BaseClothing orig )
		{
			m_Altered = true;

			m_MagicalAttributes = orig.Attributes;
			m_ClothingAttributes = orig.ClothingAttributes;
			m_SkillBonuses = orig.SkillBonuses;
			m_Resistances = orig.Resistances;
			m_AbsorptionAttributes = orig.AbsorptionAttributes;

			Crafter = orig.Crafter;
			Exceptional = orig.Exceptional;

			MaxHitPoints = orig.MaxHitPoints;
			HitPoints = orig.HitPoints;
		}

		public virtual void AlterFrom( BaseQuiver orig )
		{
			m_Altered = true;

			m_MagicalAttributes = orig.Attributes;

			Crafter = orig.Crafter;
			Exceptional = orig.Exceptional;
		}
		#endregion
	}
}