using System;
using System.Collections;
using System.Linq;

using Server;
using Server.Network;
using Server.Engines.Craft;
using Server.Factions;
using Server.Engines.Imbuing;
using AMT = Server.Items.ArmorMaterialType;
using AMI = Server.Items.ArmorMaterialInfo;
using ABT = Server.Items.ArmorBodyType;

namespace Server.Items
{
	public abstract class BaseArmor : Item, IArmor, IFactionItem, IScissorable, ICraftable, IWearableDurability, IImbuable, IGorgonCharges, IArtifactRarity
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

		// Instance values. These values must are unique to each armor piece.
		private int m_MaxHitPoints;
		private int m_HitPoints;

		private CraftResource m_Resource;
		private Mobile m_Crafter;
		private bool m_PlayerConstructed;
		private bool m_Exceptional;

		private int m_TimesImbued;

		private bool m_Altered;

		private int m_PhysicalBonus, m_FireBonus, m_ColdBonus, m_PoisonBonus, m_EnergyBonus;

		private AosAttributes m_AosAttributes;
		private AosArmorAttributes m_AosArmorAttributes;
		private SkillBonuses m_SkillBonuses;
		private AosElementAttributes m_Resistances;
		private AbsorptionAttributes m_AbsorptionAttributes;

		// Overridable values. These values are provided to override the defaults which get defined in the individual armor scripts.

		public virtual bool AllowMaleWearer { get { return true; } }
		public virtual bool AllowFemaleWearer { get { return true; } }

		public virtual int StrBonus { get { return 0; } }
		public virtual int DexBonus { get { return 0; } }
		public virtual int IntBonus { get { return 0; } }

		public virtual int StrengthReq { get { return 0; } }

		public abstract ArmorMaterialType MaterialType { get; }

		public ArmorMaterialInfo MaterialInfo { get { return ArmorMaterialInfo.GetInfo( MaterialType ); } }

		public virtual bool Meditable { get { return MaterialInfo.Meditable; } }
		public virtual CraftResource DefaultResource { get { return MaterialInfo.DefaultResource; } }

		public virtual bool CanFortify { get { return m_TimesImbued == 0; } }

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
		public CraftResource Resource
		{
			get
			{
				return m_Resource;
			}
			set
			{
				if ( m_Resource != value )
				{
					UnscaleDurability();

					m_Resource = value;
					Hue = CraftResources.GetHue( m_Resource );

					ApplyResourceAttribtues( m_Resource );

					InvalidateProperties();

					if ( Parent is Mobile )
						( (Mobile) Parent ).UpdateResistances();

					ScaleDurability();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get { return m_MaxHitPoints; }
			set
			{
				if ( m_MaxHitPoints != value )
				{
					m_MaxHitPoints = value;

					if ( m_HitPoints > m_MaxHitPoints )
						m_HitPoints = m_MaxHitPoints;

					InvalidateProperties();
				}
			}
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

		public virtual int ArtifactRarity
		{
			get { return 0; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get { return m_AosAttributes; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosArmorAttributes ArmorAttributes
		{
			get { return m_AosArmorAttributes; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillBonuses SkillBonuses
		{
			get { return m_SkillBonuses; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get { return m_Resistances; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AbsorptionAttributes AbsorptionAttributes
		{
			get { return m_AbsorptionAttributes; }
			set { }
		}

		public int ComputeStrReq()
		{
			return AOS.Scale( StrengthReq, 100 - GetLowerStatReq() );
		}

		public int ComputeStatBonus( StatType type )
		{
			if ( type == StatType.Str )
				return StrBonus + Attributes.BonusStr;
			else if ( type == StatType.Dex )
				return DexBonus + Attributes.BonusDex;
			else
				return IntBonus + Attributes.BonusInt;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalBonus { get { return m_PhysicalBonus; } set { m_PhysicalBonus = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireBonus { get { return m_FireBonus; } set { m_FireBonus = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdBonus { get { return m_ColdBonus; } set { m_ColdBonus = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonBonus { get { return m_PoisonBonus; } set { m_PoisonBonus = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyBonus { get { return m_EnergyBonus; } set { m_EnergyBonus = value; InvalidateProperties(); } }

		public virtual int BasePhysicalResistance { get { return 0; } }
		public virtual int BaseFireResistance { get { return 0; } }
		public virtual int BaseColdResistance { get { return 0; } }
		public virtual int BasePoisonResistance { get { return 0; } }
		public virtual int BaseEnergyResistance { get { return 0; } }

		public override int PhysicalResistance
		{
			get { return BasePhysicalResistance + PhysicalBonus + Resistances.Physical; }
		}

		public override int FireResistance
		{
			get { return BaseFireResistance + FireBonus + Resistances.Fire; }
		}

		public override int ColdResistance
		{
			get { return BaseColdResistance + ColdBonus + Resistances.Cold; }
		}

		public override int PoisonResistance
		{
			get { return BasePoisonResistance + PoisonBonus + Resistances.Poison; }
		}

		public override int EnergyResistance
		{
			get { return BaseEnergyResistance + EnergyBonus + Resistances.Energy; }
		}

		public virtual bool CanLoseDurability { get { return m_HitPoints >= 0 && m_MaxHitPoints > 0; } }

		public virtual int InitMinHits { get { return 0; } }
		public virtual int InitMaxHits { get { return 0; } }

		public virtual bool Brittle { get { return false; } }
		public virtual bool CannotBeRepaired { get { return false; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public ArmorBodyType BodyPosition
		{
			get
			{
				switch ( this.Layer )
				{
					default:
					case Layer.Neck:
						return ArmorBodyType.Gorget;
					case Layer.TwoHanded:
						return ArmorBodyType.Shield;
					case Layer.Gloves:
						return ArmorBodyType.Gloves;
					case Layer.Helm:
						return ArmorBodyType.Helmet;
					case Layer.Arms:
						return ArmorBodyType.Arms;

					case Layer.InnerLegs:
					case Layer.OuterLegs:
					case Layer.Pants:
						return ArmorBodyType.Legs;

					case Layer.InnerTorso:
					case Layer.OuterTorso:
					case Layer.Shirt:
						return ArmorBodyType.Chest;
				}
			}
		}

		public CraftAttributeInfo GetResourceAttrs()
		{
			CraftResourceInfo info = CraftResources.GetInfo( m_Resource );

			if ( info == null )
				return CraftAttributeInfo.Blank;

			return info.AttributeInfo;
		}

		public void UnscaleDurability()
		{
			int scale = 100 + ArmorAttributes.DurabilityBonus;

			m_HitPoints = ( ( m_HitPoints * 100 ) + ( scale - 1 ) ) / scale;
			m_MaxHitPoints = ( ( m_MaxHitPoints * 100 ) + ( scale - 1 ) ) / scale;
			InvalidateProperties();
		}

		public void ScaleDurability()
		{
			int scale = 100 + ArmorAttributes.DurabilityBonus;

			m_HitPoints = Math.Min( 255, ( ( m_HitPoints * scale ) + 99 ) / 100 );
			m_MaxHitPoints = Math.Min( 255, ( ( m_MaxHitPoints * scale ) + 99 ) / 100 );

			InvalidateProperties();
		}

		public bool Scissor( Mobile from, Scissors scissors )
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
					Item res = (Item) Activator.CreateInstance( CraftResources.GetInfo( m_Resource ).ResourceTypes[0] );

					ScissorHelper( from, res, m_PlayerConstructed ? ( item.Ressources.GetAt( 0 ).Amount / 2 ) : 1 );
					return true;
				}
				catch
				{
				}
			}

			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}

		public static void ValidateMobile( Mobile m )
		{
			foreach ( var armor in m.GetEquippedItems().OfType<BaseArmor>() )
			{
				if ( !armor.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster )
				{
					if ( armor.AllowFemaleWearer )
						m.SendLocalizedMessage( 1010388 ); // Only females can wear this.
					else
						m.SendMessage( "You may not wear this." );

					m.AddToBackpack( armor );
				}
				else if ( !armor.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster )
				{
					if ( armor.AllowMaleWearer )
						m.SendLocalizedMessage( 1063343 ); // Only males can wear this.
					else
						m.SendMessage( "You may not wear this." );

					m.AddToBackpack( armor );
				}
			}
		}

		public int GetLowerStatReq()
		{
			int v = m_AosArmorAttributes.LowerStatReq;

			CraftResourceInfo info = CraftResources.GetInfo( m_Resource );

			if ( info != null )
			{
				CraftAttributeInfo attrInfo = info.AttributeInfo;

				if ( attrInfo != null )
					v += attrInfo.ArmorLowerRequirements;
			}

			if ( v > 100 )
				v = 100;

			return v;
		}

		public override void OnAdded( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				m_SkillBonuses.AddTo( from );

				from.Delta( MobileDelta.Armor ); // Tell them armor rating has changed
			}
		}

		public virtual double ScaleArmorByDurability( double armor )
		{
			int scale = 100;

			if ( m_MaxHitPoints > 0 && m_HitPoints < m_MaxHitPoints )
				scale = 50 + ( ( 50 * m_HitPoints ) / m_MaxHitPoints );

			return ( armor * scale ) / 100;
		}

		public BaseArmor( Serial serial )
			: base( serial )
		{
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

		[Flags]
		private enum SaveFlag
		{
			None = 0x00000000,
			Attributes = 0x00000001,
			ArmorAttributes = 0x00000002,
			PhysicalBonus = 0x00000004,
			FireBonus = 0x00000008,
			ColdBonus = 0x00000010,
			PoisonBonus = 0x00000020,
			EnergyBonus = 0x00000040,
			//Unused = 0x00000080,
			MaxHitPoints = 0x00000100,
			HitPoints = 0x00000200,
			Crafter = 0x00000400,
			Exceptional = 0x00000800,
			//Unused = 0x00001000,
			//Unused = 0x00002000,
			Resource = 0x00004000,
			//Unused = 0x00008000,
			//Unused = 0x00010000,
			//Unused = 0x00020000,
			//Unused = 0x00040000,
			//Unused = 0x00080000,
			//Unused = 0x00100000,
			GorgonCharges = 0x00200000,
			//Unused = 0x00400000,
			SkillBonuses = 0x00800000,
			PlayerConstructed = 0x01000000,
			TimesImbued = 0x02000000,
			Resistances = 0x04000000,
			Altered = 0x08000000,
			//Unused = 0x10000000,
			//Unused = 0x20000000,
			EngravedText = 0x40000000,
			AbsorptionAttributes = unchecked( (int) 0x80000000 )
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 12 ); // version

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.Attributes, !m_AosAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.ArmorAttributes, !m_AosArmorAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.PhysicalBonus, m_PhysicalBonus != 0 );
			SetSaveFlag( ref flags, SaveFlag.FireBonus, m_FireBonus != 0 );
			SetSaveFlag( ref flags, SaveFlag.ColdBonus, m_ColdBonus != 0 );
			SetSaveFlag( ref flags, SaveFlag.PoisonBonus, m_PoisonBonus != 0 );
			SetSaveFlag( ref flags, SaveFlag.EnergyBonus, m_EnergyBonus != 0 );
			SetSaveFlag( ref flags, SaveFlag.MaxHitPoints, m_MaxHitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.HitPoints, m_HitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.Crafter, m_Crafter != null );
			SetSaveFlag( ref flags, SaveFlag.Exceptional, m_Exceptional != false );
			SetSaveFlag( ref flags, SaveFlag.Resource, m_Resource != DefaultResource );
			SetSaveFlag( ref flags, SaveFlag.SkillBonuses, !m_SkillBonuses.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Resistances, !m_Resistances.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.TimesImbued, m_TimesImbued != 0 );
			SetSaveFlag( ref flags, SaveFlag.Altered, m_Altered );
			SetSaveFlag( ref flags, SaveFlag.EngravedText, !String.IsNullOrEmpty( m_EngravedText ) );
			SetSaveFlag( ref flags, SaveFlag.AbsorptionAttributes, !m_AbsorptionAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.GorgonCharges, m_GorgonCharges > 0 );
			SetSaveFlag( ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed != false );

			writer.WriteEncodedInt( (int) flags );

			if ( GetSaveFlag( flags, SaveFlag.GorgonCharges ) )
			{
				writer.Write( (int) m_GorgonCharges );
				writer.Write( (int) m_GorgonQuality );
			}

			if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
				m_AosAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.ArmorAttributes ) )
				m_AosArmorAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.PhysicalBonus ) )
				writer.WriteEncodedInt( (int) m_PhysicalBonus );

			if ( GetSaveFlag( flags, SaveFlag.FireBonus ) )
				writer.WriteEncodedInt( (int) m_FireBonus );

			if ( GetSaveFlag( flags, SaveFlag.ColdBonus ) )
				writer.WriteEncodedInt( (int) m_ColdBonus );

			if ( GetSaveFlag( flags, SaveFlag.PoisonBonus ) )
				writer.WriteEncodedInt( (int) m_PoisonBonus );

			if ( GetSaveFlag( flags, SaveFlag.EnergyBonus ) )
				writer.WriteEncodedInt( (int) m_EnergyBonus );

			if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
				writer.WriteEncodedInt( (int) m_MaxHitPoints );

			if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
				writer.WriteEncodedInt( (int) m_HitPoints );

			if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
				writer.Write( (Mobile) m_Crafter );

			if ( GetSaveFlag( flags, SaveFlag.Resource ) )
				writer.WriteEncodedInt( (int) m_Resource );

			if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
				m_SkillBonuses.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
				m_Resistances.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.TimesImbued ) )
				writer.WriteEncodedInt( (int) m_TimesImbued );

			if ( GetSaveFlag( flags, SaveFlag.EngravedText ) )
				writer.Write( (string) m_EngravedText );

			if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
				m_AbsorptionAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.PlayerConstructed ) )
				m_PlayerConstructed = true;
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 12:
				case 11:
					{
						SaveFlag flags = (SaveFlag) reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.GorgonCharges ) )
						{
							m_GorgonCharges = reader.ReadInt();
							m_GorgonQuality = (GorgonQuality) reader.ReadInt();
						}

						if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
							m_AosAttributes = new AosAttributes( this, reader );
						else
							m_AosAttributes = new AosAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.ArmorAttributes ) )
							m_AosArmorAttributes = new AosArmorAttributes( this, reader );
						else
							m_AosArmorAttributes = new AosArmorAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.PhysicalBonus ) )
							m_PhysicalBonus = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.FireBonus ) )
							m_FireBonus = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.ColdBonus ) )
							m_ColdBonus = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.PoisonBonus ) )
							m_PoisonBonus = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.EnergyBonus ) )
							m_EnergyBonus = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
							m_MaxHitPoints = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
							m_HitPoints = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
							m_Crafter = reader.ReadMobile();

						if ( GetSaveFlag( flags, SaveFlag.Exceptional ) )
							m_Exceptional = true;

						if ( GetSaveFlag( flags, SaveFlag.Resource ) )
							m_Resource = (CraftResource) reader.ReadEncodedInt();
						else
							m_Resource = DefaultResource;

						if ( m_Resource == CraftResource.None )
							m_Resource = DefaultResource;

						if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
							m_SkillBonuses = new SkillBonuses( this, reader );
						else
							m_SkillBonuses = new SkillBonuses( this );

						if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
							m_Resistances = new AosElementAttributes( this, reader );
						else
							m_Resistances = new AosElementAttributes( this );

						if ( GetSaveFlag( flags, SaveFlag.TimesImbued ) )
							m_TimesImbued = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.Altered ) )
							m_Altered = true;

						if ( GetSaveFlag( flags, SaveFlag.EngravedText ) )
							m_EngravedText = reader.ReadString();

						if ( GetSaveFlag( flags, SaveFlag.AbsorptionAttributes ) )
							m_AbsorptionAttributes = new AbsorptionAttributes( this, reader );
						else
							m_AbsorptionAttributes = new AbsorptionAttributes( this );

						break;
					}
			}

			if ( Parent is Mobile )
				m_SkillBonuses.AddTo( (Mobile) Parent );

			int strBonus = ComputeStatBonus( StatType.Str );
			int dexBonus = ComputeStatBonus( StatType.Dex );
			int intBonus = ComputeStatBonus( StatType.Int );

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

			if ( !( this is BaseShield ) && Meditable && m_AosArmorAttributes.MageArmor != 0 )
				m_AosArmorAttributes.MageArmor = 0;

			if ( m_PlayerConstructed )
				m_AosArmorAttributes.DurabilityBonus = 0;

			if ( MaterialType == ArmorMaterialType.Leather )
				m_AosArmorAttributes.LowerStatReq = 0;

			if ( MaxHitPoints > 255 )
				MaxHitPoints = 255;
		}

		public BaseArmor( int itemID )
			: base( itemID )
		{
			m_Resource = DefaultResource;
			Hue = CraftResources.GetHue( m_Resource );

			m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );

			this.Layer = (Layer) ItemData.Quality;

			m_AosAttributes = new AosAttributes( this );
			m_AosArmorAttributes = new AosArmorAttributes( this );
			m_SkillBonuses = new SkillBonuses( this );
			m_Resistances = new AosElementAttributes( this );
			m_AbsorptionAttributes = new AbsorptionAttributes( this );
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
				{
					from.SendMessage( "Only males can wear this." );
				}
				else
				{
					from.SendMessage( "You may not wear this." );
				}

				return false;
			}
			else
			{
				int strBonus = ComputeStatBonus( StatType.Str );
				int strReq = ComputeStrReq();

				if ( from.Str < strReq || ( from.Str + strBonus ) < 1 )
				{
					from.SendLocalizedMessage( 500213 ); // You are not strong enough to equip that.
					return false;
				}
			}

			return base.CanEquip( from );
		}

		public override bool CheckPropertyConfliction( Mobile m )
		{
			if ( base.CheckPropertyConfliction( m ) )
				return true;

			if ( Layer == Layer.Pants )
				return ( m.FindItemOnLayer( Layer.InnerLegs ) != null );

			if ( Layer == Layer.Shirt )
				return ( m.FindItemOnLayer( Layer.InnerTorso ) != null );

			return false;
		}

		public override bool OnEquip( Mobile from )
		{
			from.CheckStatTimers();

			int strBonus = ComputeStatBonus( StatType.Str );
			int dexBonus = ComputeStatBonus( StatType.Dex );
			int intBonus = ComputeStatBonus( StatType.Int );

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

			return base.OnEquip( from );
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile m = (Mobile) parent;
				string modName = this.Serial.ToString();

				m.RemoveStatMod( modName + "Str" );
				m.RemoveStatMod( modName + "Dex" );
				m.RemoveStatMod( modName + "Int" );

				m_SkillBonuses.Remove();

				( (Mobile) parent ).Delta( MobileDelta.Armor ); // Tell them armor rating has changed
				m.CheckStatTimers();
			}

			base.OnRemoved( parent );
		}

		public virtual int OnHit( int damageTaken )
		{
			if ( 25 > Utility.Random( 100 ) ) // 25% chance to lower durability
			{
				if ( ArmorAttributes.SelfRepair > Utility.Random( 10 ) )
				{
					HitPoints += 2;
				}
				else
				{
					int wear = Utility.Random( 2 );

					if ( wear > 0 && MaxHitPoints > 0 )
					{
						if ( HitPoints >= wear )
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
							if ( MaxHitPoints > wear )
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
			}

			return damageTaken;
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
			set { base.Hue = value; InvalidateProperties(); }
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

		public virtual int GetCastSpeedBonus()
		{
			int bonus = 0;

			if ( Attributes.SpellChanneling != 0 )
				bonus -= 1;

			return bonus;
		}

		public virtual int GetAttributeBonus( AosAttribute attr )
		{
			int value = 0;

			switch ( attr )
			{
				case AosAttribute.CastSpeed:
					value += GetCastSpeedBonus();
					break;
			}

			return value;
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

			m_SkillBonuses.GetProperties( list );

			int woodType;

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

			if ( woodType != 0 )
				list.Add( woodType );

			AddMagicalProperties( list );

			if ( Brittle )
				list.Add( 1116209 ); // Brittle

			base.AddResistanceProperties( list );

			int prop;

			if ( ( prop = ArmorAttributes.MageArmor ) != 0 )
				list.Add( 1060437 ); // mage armor

			if ( ( prop = ArmorAttributes.SelfRepair ) != 0 )
				list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

			if ( ( prop = ArmorAttributes.SoulCharge ) != 0 )
				list.Add( 1113630, prop.ToString() ); // Soul Charge ~1_val~%

			if ( ( prop = ArmorAttributes.ReactiveParalyze ) != 0 )
				list.Add( 1112364 ); // reactive paralyze

			if ( ( prop = ComputeStrReq() ) > 0 )
				list.Add( 1061170, prop.ToString() ); // strength requirement ~1_val~

			if ( CanLoseDurability )
				list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~

			GetSetArmorPropertiesSecond( list );
		}

		public virtual void AddMagicalProperties( ObjectPropertyList list )
		{
			int prop;

			if ( ( prop = ArtifactRarity ) > 0 )
				list.Add( 1061078, prop.ToString() ); // artifact rarity ~1_val~

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

			if ( ( prop = ( GetCastSpeedBonus() + m_AosAttributes.CastSpeed ) ) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( ( prop = Attributes.AttackChance ) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( ( prop = Attributes.IncreasedKarmaLoss ) != 0 )
				list.Add( 1075210, prop.ToString() ); // Increased Karma Loss ~1_val~%

			if ( ( prop = Attributes.CastingFocus ) != 0 )
				list.Add( 1113696, prop.ToString() ); // Casting Focus ~1_val~%

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

			if ( ( prop = Attributes.SpellChanneling ) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( ( prop = Attributes.SpellDamage ) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( ( prop = Attributes.BonusStam ) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( ( prop = Attributes.BonusStr ) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( ( prop = Attributes.WeaponSpeed ) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%
		}

		#region ICraftable Members

		public virtual bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			PlayerConstructed = true;

			Resource = CraftResources.GetFromType( resourceType );

			CraftContext context = craftSystem.GetContext( from );

			if ( context != null && context.DoNotColor )
				Hue = 0;

			if ( exceptional )
			{
				ArmorAttributes.DurabilityBonus += 20;

				DistributeBonuses( ( tool is BaseRunicTool ? 6 : 15 ) );

				if ( !( this is BaseShield ) )
				{
					int bonus = (int) ( from.Skills.ArmsLore.Value / 20 );

					DistributeBonuses( bonus );

					from.CheckSkill( SkillName.ArmsLore, 0, 100 );
				}
			}

			if ( tool is BaseRunicTool )
			{
				if ( !CraftableArtifacts.IsCraftableArtifact( this ) && !DarkwoodSet.IsPart( this ) )
					( (BaseRunicTool) tool ).ApplyAttributesTo( this );
			}

			InvalidateProperties();

			return exceptional;
		}

		private void DistributeBonuses( int amount )
		{
			for ( int i = 0; i < amount; ++i )
			{
				switch ( Utility.Random( 5 ) )
				{
					case 0:
						++PhysicalBonus;
						break;
					case 1:
						++FireBonus;
						break;
					case 2:
						++ColdBonus;
						break;
					case 3:
						++PoisonBonus;
						break;
					case 4:
						++EnergyBonus;
						break;
				}
			}
		}

		private void ApplyResourceAttribtues( CraftResource resource )
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
						if ( this is BaseShield )
							Attributes.Luck += 40;

						break;
					}
			}
		}

		protected virtual void AddHeartwoodBonus()
		{
			switch ( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1:
					Attributes.AttackChance += 5;
					break;
				case 2:
					Attributes.WeaponDamage += 10;
					break;
				case 3:
					Attributes.Luck += 40;
					break;
				case 4:
					ArmorAttributes.LowerStatReq += 20;
					break;
				default:
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

		public virtual ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Armor; } }
		public bool IsSpecialMaterial { get { return !CraftResources.IsStandard( m_Resource ); } }
		public virtual int MaxIntensity { get { return ( Exceptional ? 500 : 400 ); } }
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

		public virtual void AlterFrom( BaseArmor orig )
		{
			m_Altered = true;

			Resource = orig.Resource;

			MaxHitPoints = orig.MaxHitPoints;
			HitPoints = orig.HitPoints;

			m_AosAttributes = orig.Attributes;
			m_Resistances = orig.Resistances;
			m_SkillBonuses = orig.SkillBonuses;
			m_AosArmorAttributes = orig.ArmorAttributes;
			m_AbsorptionAttributes = orig.AbsorptionAttributes;

			EngravedText = orig.EngravedText;

			TimesImbued = orig.TimesImbued;
			Crafter = orig.Crafter;
			Exceptional = orig.Exceptional;
		}
	}
}
