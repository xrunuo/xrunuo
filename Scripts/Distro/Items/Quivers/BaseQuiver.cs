using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Engines.Imbuing;

namespace Server.Items
{
	public interface IQuiver
	{
		int PhysicalDamage { get; }
		int FireDamage { get; }
		int ColdDamage { get; }
		int PoisonDamage { get; }
		int EnergyDamage { get; }
		int ChaosDamage { get; }
		int DirectDamage { get; }

		int DamageModifier { get; }
	}

	[Alterable( typeof( DefTailoring ), typeof( GargishLeatherWingArmor ), true )]
	public abstract class BaseQuiver : Container, IMagicalItem, IImbuable, ICraftable, IQuiver
	{
		public override int MaxWeight { get { return 50; } }

		private AosAttributes m_Attributes;

		private bool m_Exceptional;
		private Mobile m_Crafter;

		public virtual int PhysicalDamage { get { return 0; } }
		public virtual int FireDamage { get { return 0; } }
		public virtual int ColdDamage { get { return 0; } }
		public virtual int PoisonDamage { get { return 0; } }
		public virtual int EnergyDamage { get { return 0; } }
		public virtual int ChaosDamage { get { return 0; } }
		public virtual int DirectDamage { get { return 0; } }

		public virtual int WeightReduction { get { return 0; } }
		public virtual int DamageModifier { get { return 10; } }

		public override bool CanInsure { get { return true; } }

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
		public AosAttributes Attributes
		{
			get { return m_Attributes; }
			set { }
		}

		public Item Ammo
		{
			get { return Items.Count > 0 ? (Item) Items[0] : null; }
		}

		public int MaxAmmo
		{
			get { return MaxWeight * 10; }
		}

		public BaseQuiver()
			: base( 0x2FB7 )
		{
			Weight = 2.0;
			Layer = Layer.Cloak;

			GumpID = 0x108;
			DropSound = 0x4F;
			MaxItems = 1;

			m_Attributes = new AosAttributes( this );
		}

		public BaseQuiver( Serial serial )
			: base( serial )
		{
		}

		private static Type[] m_Ammo = new Type[]
			{
				typeof( Arrow ), typeof( Bolt )
			};

		public bool CheckType( Item item )
		{
			Type type = item.GetType();
			Item ammo = Ammo;

			if ( ammo != null )
			{
				if ( ammo.GetType() == type )
					return true;
			}
			else
			{
				for ( int i = 0; i < m_Ammo.Length; i++ )
				{
					if ( type == m_Ammo[i] )
						return true;
				}
			}

			return false;
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			if ( !CheckType( item ) )
			{
				if ( message )
					m.SendLocalizedMessage( 1074836 ); // The container can not hold that type of object.

				return false;
			}

			Item ammo = this.Ammo;

			if ( ammo != null )
			{
				if ( ammo.Amount + item.Amount > MaxAmmo )
				{
					if ( message )
						m.SendLocalizedMessage( 1080017 ); // That container cannot hold more items.

					return false;
				}
			}

			return base.CheckHold( m, item, message, checkItems, plusItems, plusWeight );
		}

		// TODO: Once TotalType is implemented, remove manual Weight Reduction

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p, byte gridloc )
		{
			bool result = base.OnDragDropInto( from, item, p, gridloc );

			if ( result && WeightReduction > 0 )
				item.Weight = item.Weight * ( 100 - WeightReduction ) / 100;

			return result;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			bool result = base.OnDragDrop( from, dropped );

			if ( result && WeightReduction > 0 )
				dropped.Weight = dropped.Weight * ( 100 - WeightReduction ) / 100;

			return result;
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

			int ammoamount = ( Ammo != null ? Ammo.Amount : 0 );

			list.Add( ( Ammo is Bolt ? 1075266 : 1075265 ), "{0}\t{1}", ammoamount.ToString(), MaxAmmo.ToString() ); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows/bolts

			if ( DamageModifier != 0 )
				list.Add( 1074762, DamageModifier.ToString() ); // Damage Modifier: ~1_PERCENT~%

			list.Add( 1075085 ); // Requirement: Mondain's Legacy

			int prop;

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

			if ( ( prop = Attributes.CastSpeed ) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( ( prop = Attributes.AttackChance ) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( ( prop = Attributes.BonusHits ) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( ( prop = Attributes.BonusInt ) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( ( prop = Attributes.LowerManaCost ) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( ( prop = Attributes.LowerRegCost ) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( ( prop = Attributes.LowerAmmoCost ) != 0 )
				list.Add( 1075208, prop.ToString() ); // Lower Ammo Cost ~1_Percentage~%

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

			if ( DirectDamage != 0 )
				list.Add( 1079978, DirectDamage.ToString() ); // Direct Damage: ~1_PERCENT~%

			if ( PhysicalDamage != 0 )
				list.Add( 1060403, PhysicalDamage.ToString() ); // physical damage ~1_val~%

			if ( FireDamage != 0 )
				list.Add( 1060405, FireDamage.ToString() ); // fire damage ~1_val~%

			if ( ColdDamage != 0 )
				list.Add( 1060404, ColdDamage.ToString() ); // cold damage ~1_val~%

			if ( PoisonDamage != 0 )
				list.Add( 1060406, PoisonDamage.ToString() ); // poison damage ~1_val~%

			if ( EnergyDamage != 0 )
				list.Add( 1060407, EnergyDamage.ToString() ); // energy damage ~1_val~%

			if ( ChaosDamage != 0 )
				list.Add( 1072846, ChaosDamage.ToString() ); // chaos damage ~1_val~%

			GetSetArmorPropertiesFirst( list );

			this.AddContentProperty( list );

			if ( WeightReduction != 0 )
				list.Add( 1072210, WeightReduction.ToString() ); // Weight reduction: ~1_PERCENTAGE~%

			GetSetArmorPropertiesSecond( list );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( (bool) m_Exceptional );
			writer.Write( (Mobile) m_Crafter );

			m_Attributes.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
					{
						m_Exceptional = reader.ReadBool();
						m_Crafter = reader.ReadMobile();
						goto case 2;
					}
				case 2:
					{
						m_Attributes = new AosAttributes( this, reader );
						break;
					}
			}

			MaxItems = 1;
		}

		#region ICraftable
		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Exceptional = exceptional;

			if ( makersMark )
				Crafter = from;

			return exceptional;
		}
		#endregion

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