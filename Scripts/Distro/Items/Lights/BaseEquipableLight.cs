using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseEquipableLight : BaseLight, IMagicalItem
	{
		private MagicalAttributes m_Attributes;

		[CommandProperty( AccessLevel.GameMaster )]
		public MagicalAttributes Attributes
		{
			get { return m_Attributes; }
			set { }
		}

		[Constructable]
		public BaseEquipableLight( int itemID )
			: base( itemID )
		{
			Layer = Layer.TwoHanded;

			m_Attributes = new MagicalAttributes( this );
		}

		public BaseEquipableLight( Serial serial )
			: base( serial )
		{
		}

		public override void Ignite()
		{
			if ( !( Parent is Mobile ) && RootParent is Mobile )
			{
				Mobile holder = (Mobile) RootParent;

				if ( holder.EquipItem( this ) )
				{
					if ( this is Candle )
						holder.SendLocalizedMessage( 502969 ); // You put the candle in your left hand.
					else if ( this is Torch )
						holder.SendLocalizedMessage( 502971 ); // You put the torch in your left hand.

					base.Ignite();
				}
				else
				{
					holder.SendLocalizedMessage( 502449 ); // You cannot hold this item.
				}
			}
			else
			{
				base.Ignite();
			}
		}

		public override void OnAdded( object parent )
		{
			if ( Burning && parent is Container )
				Douse();

			base.OnAdded( parent );
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( base.AllowEquipedCast( from ) )
				return true;

			return ( m_Attributes.SpellChanneling != 0 );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

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
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			m_Attributes.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Attributes = new MagicalAttributes( this, reader );

						goto case 0;
					}
				case 0:
					{
						if ( version == 0 )
							m_Attributes = new MagicalAttributes( this );

						break;
					}
			}
		}
	}
}
