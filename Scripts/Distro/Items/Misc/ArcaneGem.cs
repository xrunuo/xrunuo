using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Targeting;

namespace Server.Items
{
	public class ArcaneGem : Item
	{
		[Constructable]
		public ArcaneGem()
			: base( 0x1EA7 )
		{
			Weight = 1.0;
			Name = "arcane gem";

			Stackable = true;
		}

		public ArcaneGem( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.BeginTarget( 2, false, TargetFlags.None, new TargetCallback( OnTarget ) );
				from.SendMessage( "What do you wish to use the gem on?" );
			}
		}

		public int GetChargesFor( Mobile m )
		{
			int v = (int) ( m.Skills[SkillName.Tailoring].Value / 5 );

			if ( v < 16 )
			{
				return 16;
			}
			else if ( v > 24 )
			{
				return 24;
			}

			return v;
		}

		public const int DefaultArcaneHue = 2117;

		public void OnTarget( Mobile from, object obj )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			if ( obj is IArcaneEquip && obj is Item )
			{
				Item item = (Item) obj;
				IArcaneEquip eq = (IArcaneEquip) obj;

				if ( !item.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
					return;
				}

				int charges = GetChargesFor( from );

				if ( eq.IsArcane )
				{
					if ( eq.CurArcaneCharges >= eq.MaxArcaneCharges )
					{
						from.SendMessage( "That item is already fully charged." );
					}
					else
					{
						if ( eq.CurArcaneCharges <= 0 )
						{
							item.Hue = DefaultArcaneHue;
						}

						if ( ( eq.CurArcaneCharges + charges ) > eq.MaxArcaneCharges )
						{
							eq.CurArcaneCharges = eq.MaxArcaneCharges;
						}
						else
						{
							eq.CurArcaneCharges += charges;
						}

						from.SendMessage( "You recharge the item." );
						Delete();
					}
				}
				else if ( from.Skills[SkillName.Tailoring].Value >= 80.0 )
				{
					bool isExceptional = false;

					if ( item is BaseClothing )
						isExceptional = ( (BaseClothing) item ).Exceptional;
					else if ( item is BaseArmor )
						isExceptional = ( (BaseArmor) item ).Exceptional;
					else if ( item is BaseWeapon )
						isExceptional = ( (BaseWeapon) item ).Exceptional;

					if ( isExceptional )
					{
						if ( item is BaseClothing )
						{
							( (BaseClothing) item ).Exceptional = true;
							( (BaseClothing) item ).Crafter = from;
						}
						else if ( item is BaseArmor )
						{
							( (BaseArmor) item ).Exceptional = true;
							( (BaseArmor) item ).Crafter = from;
						}
						else if ( item is BaseWeapon ) // Sanity, weapons cannot recieve gems...
						{
							( (BaseWeapon) item ).Exceptional = true;
							( (BaseWeapon) item ).Crafter = from;
						}

						eq.CurArcaneCharges = eq.MaxArcaneCharges = charges;

						item.Hue = DefaultArcaneHue;

						from.SendMessage( "You enhance the item with your gem." );
						Delete();
					}
					else
					{
						from.SendMessage( "Only exceptional items can be enhanced with the gem." );
					}
				}
				else
				{
					from.SendMessage( "You do not have enough skill in tailoring to enhance the item." );
				}
			}
			else
			{
				from.SendMessage( "You cannot use the gem on that." );
			}
		}

		public static bool ConsumeCharges( Mobile from, int amount )
		{
			var arcaneEquipment = from.GetEquippedItems().OfType<IArcaneEquip>().Where( eq => eq.IsArcane );

			if ( arcaneEquipment.Sum( eq => eq.CurArcaneCharges ) < amount )
				return false;

			foreach ( var eq in arcaneEquipment )
			{
				if ( eq.CurArcaneCharges > amount )
				{
					eq.CurArcaneCharges -= amount;
					break;
				}
				else
				{
					amount -= eq.CurArcaneCharges;
					eq.CurArcaneCharges = 0;
				}
			}

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( !Stackable )
				Stackable = true;
		}
	}
}