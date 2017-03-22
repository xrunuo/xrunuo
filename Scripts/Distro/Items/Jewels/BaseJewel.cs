using System;
using Server;
using Server.Engines.Craft;
using Server.Engines.Imbuing;
using Server.Network;

namespace Server.Items
{
	public enum GemType
	{
		None,
		StarSapphire,
		Emerald,
		Sapphire,
		Ruby,
		Citrine,
		Amethyst,
		Tourmaline,
		Amber,
		Diamond
	}

	public abstract class BaseJewel : Item, ICraftable, IMagicalItem, IResistances, ISkillBonuses, IImbuable, IAbsorption, IWearableDurability, IArtifactRarity
	{
		private AosAttributes m_AosAttributes;
		private AosElementAttributes m_AosResistances;
		private SkillBonuses m_SkillBonuses;
		private AbsorptionAttributes m_AbsorptionAttributes;

		private CraftResource m_Resource;
		private GemType m_GemType;
		private Mobile m_Crafter;
		private bool m_PlayerConstructed;
		private bool m_Exceptional;

		private int m_MaxHitPoints;
		private int m_HitPoints;

		private int m_TimesImbued;

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

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get { return m_AosAttributes; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get { return m_AosResistances; }
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
		public AbsorptionAttributes AbsorptionAttributes
		{
			get { return m_AbsorptionAttributes; }
			set
			{
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue( m_Resource );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public GemType GemType
		{
			get { return m_GemType; }
			set
			{
				m_GemType = value;
				InvalidateProperties();
			}
		}

		public override int PhysicalResistance { get { return m_AosResistances.Physical; } }
		public override int FireResistance { get { return m_AosResistances.Fire; } }
		public override int ColdResistance { get { return m_AosResistances.Cold; } }
		public override int PoisonResistance { get { return m_AosResistances.Poison; } }
		public override int EnergyResistance { get { return m_AosResistances.Energy; } }

		public virtual int BaseGemTypeNumber { get { return 0; } }

		public virtual bool CanLoseDurability { get { return m_HitPoints >= 0 && m_MaxHitPoints > 0; } }

		public virtual int InitMinHits { get { return 0; } }
		public virtual int InitMaxHits { get { return 0; } }

		public virtual bool Brittle { get { return false; } }
		public virtual bool CannotBeRepaired { get { return false; } }

		public override int LabelNumber
		{
			get
			{
				if ( m_GemType == GemType.None )
					return base.LabelNumber;

				return BaseGemTypeNumber + (int) m_GemType - 1;
			}
		}

		public override bool WearableByGargoyles { get { return true; } }

		public override void OnAfterDuped( Item newItem )
		{
			// TODO: Copy Attributes
		}

		public virtual int ArtifactRarity { get { return 0; } }

		public BaseJewel( int itemID, Layer layer )
			: base( itemID )
		{
			m_AosAttributes = new AosAttributes( this );
			m_AosResistances = new AosElementAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );
			m_AbsorptionAttributes = new AbsorptionAttributes( this );

			m_Resource = CraftResource.Iron;
			m_GemType = GemType.None;

			Layer = layer;

			m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );
		}

		public override void OnAdded( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				m_SkillBonuses.AddTo( from );

				int strBonus = m_AosAttributes.BonusStr;
				int dexBonus = m_AosAttributes.BonusDex;
				int intBonus = m_AosAttributes.BonusInt;

				if ( strBonus != 0 || dexBonus != 0 || intBonus != 0 )
				{
					string modName = this.Serial.ToString();

					if ( strBonus != 0 )
					{
						from.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );
					}

					if ( dexBonus != 0 )
					{
						from.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );
					}

					if ( intBonus != 0 )
					{
						from.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
					}
				}

				from.CheckStatTimers();
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
			}
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

		public BaseJewel( Serial serial )
			: base( serial )
		{
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

			if ( m_TimesImbued > 0 )
				list.Add( 1080418 ); // (Imbued)

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

			if ( ( prop = m_AosAttributes.WeaponDamage ) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( ( prop = m_AosAttributes.DefendChance ) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( ( prop = m_AosAttributes.BonusDex ) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( ( prop = m_AosAttributes.EnhancePotions ) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( ( prop = m_AosAttributes.CastRecovery ) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( ( prop = m_AosAttributes.CastSpeed ) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( ( prop = m_AosAttributes.AttackChance ) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( ( prop = m_AosAttributes.BonusHits ) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( ( prop = m_AosAttributes.BonusInt ) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( ( prop = m_AosAttributes.LowerManaCost ) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( ( prop = m_AosAttributes.LowerRegCost ) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( ( prop = m_AosAttributes.Luck ) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( ( prop = m_AosAttributes.BonusMana ) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( ( prop = m_AosAttributes.RegenMana ) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( ( prop = m_AosAttributes.NightSight ) != 0 )
				list.Add( 1060441 ); // night sight

			if ( ( prop = m_AosAttributes.ReflectPhysical ) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( ( prop = m_AosAttributes.RegenStam ) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( ( prop = m_AosAttributes.RegenHits ) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( ( prop = m_AosAttributes.SpellChanneling ) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( ( prop = m_AosAttributes.SpellDamage ) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( ( prop = m_AosAttributes.BonusStam ) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( ( prop = m_AosAttributes.BonusStr ) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( ( prop = m_AosAttributes.WeaponSpeed ) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

			base.AddResistanceProperties( list );

			if ( CanLoseDurability )
				list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~

			GetSetArmorPropertiesSecond( list );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 8 ); // version

			writer.Write( (bool) m_Exceptional );
			writer.Write( (bool) m_PlayerConstructed );
			writer.Write( (Mobile) m_Crafter );

			m_AbsorptionAttributes.Serialize( writer );

			writer.WriteEncodedInt( (int) m_TimesImbued );

			writer.WriteEncodedInt( (int) m_MaxHitPoints );
			writer.WriteEncodedInt( (int) m_HitPoints );

			writer.WriteEncodedInt( (int) m_Resource );
			writer.WriteEncodedInt( (int) m_GemType );

			m_AosAttributes.Serialize( writer );
			m_AosResistances.Serialize( writer );
			m_SkillBonuses.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 8:
					{
						m_Exceptional = reader.ReadBool();
						m_PlayerConstructed = reader.ReadBool();
						m_Crafter = reader.ReadMobile();

						goto case 7;
					}
				case 7:
					{
						m_AbsorptionAttributes = new AbsorptionAttributes( this, reader );

						m_TimesImbued = reader.ReadEncodedInt();

						m_MaxHitPoints = reader.ReadEncodedInt();
						m_HitPoints = reader.ReadEncodedInt();

						m_Resource = (CraftResource) reader.ReadEncodedInt();
						m_GemType = (GemType) reader.ReadEncodedInt();

						m_AosAttributes = new AosAttributes( this, reader );
						m_AosResistances = new AosElementAttributes( this, reader );
						m_SkillBonuses = new SkillBonuses( this, reader );

						if ( Parent is Mobile )
							m_SkillBonuses.AddTo( (Mobile) Parent );

						break;
					}
			}

			int strBonus = m_AosAttributes.BonusStr;
			int dexBonus = m_AosAttributes.BonusDex;
			int intBonus = m_AosAttributes.BonusInt;

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

			if ( 1 < craftItem.Ressources.Count )
			{
				resourceType = craftItem.Ressources.GetAt( 1 ).ItemType;

				if ( resourceType == typeof( StarSapphire ) )
					GemType = GemType.StarSapphire;
				else if ( resourceType == typeof( Emerald ) )
					GemType = GemType.Emerald;
				else if ( resourceType == typeof( Sapphire ) )
					GemType = GemType.Sapphire;
				else if ( resourceType == typeof( Ruby ) )
					GemType = GemType.Ruby;
				else if ( resourceType == typeof( Citrine ) )
					GemType = GemType.Citrine;
				else if ( resourceType == typeof( Amethyst ) )
					GemType = GemType.Amethyst;
				else if ( resourceType == typeof( Tourmaline ) )
					GemType = GemType.Tourmaline;
				else if ( resourceType == typeof( Amber ) )
					GemType = GemType.Amber;
				else if ( resourceType == typeof( Diamond ) )
					GemType = GemType.Diamond;
			}

			return exceptional;
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

		public virtual ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Jewelry; } }
		public bool IsSpecialMaterial { get { return false; } }
		public virtual int MaxIntensity { get { return 500; } }
		public virtual bool CanImbue { get { return ArtifactRarity == 0; } }

		public void OnImbued()
		{
			if ( m_TimesImbued == 0 )
			{
				m_HitPoints = m_MaxHitPoints = 255;
				InvalidateProperties();
			}
		}
		#endregion
	}
}