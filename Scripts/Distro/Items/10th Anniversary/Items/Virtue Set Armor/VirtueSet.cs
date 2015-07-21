using System;
using System.Collections;

namespace Server.Items
{
	public class VirtueSet
	{
		public static bool FullSet( Mobile m )
		{
			return m.FindItemOnLayer( Layer.Neck ) is GorgetOfHonesty
				&& m.FindItemOnLayer( Layer.Helm ) is HelmOfSpirituality
				&& m.FindItemOnLayer( Layer.Pants ) is LegsOfHonor
				&& m.FindItemOnLayer( Layer.Arms ) is ArmsOfCompassion
				&& m.FindItemOnLayer( Layer.Gloves ) is GauntletsOfValor
				&& m.FindItemOnLayer( Layer.Shoes ) is SolleretsOfSacrifice
				&& m.FindItemOnLayer( Layer.InnerTorso ) is BreastplateOfJustice
				&& m.FindItemOnLayer( Layer.Cloak ) is CloakOfHumility;
		}

		public static void ApplyBonus( Mobile m )
		{
			ApplyBonus( m.FindItemOnLayer( Layer.Neck ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Helm ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Pants ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Arms ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Gloves ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor );
			ApplyBonus( m.FindItemOnLayer( Layer.Shoes ) as BaseClothing );
			ApplyBonus( m.FindItemOnLayer( Layer.Cloak ) as BaseClothing );

			m.SendLocalizedMessage( 1072391 ); // The magic of your armor combines to assist you!

			Effects.PlaySound( m.Location, m.Map, 503 );
			m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
		}

		public static void RemoveBonus( Mobile m )
		{
			BaseArmor armor;
			BaseClothing clothing;

			if ( ( armor = m.FindItemOnLayer( Layer.Neck ) as BaseArmor ) is GorgetOfHonesty )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Helm ) as BaseArmor ) is HelmOfSpirituality )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Pants ) as BaseArmor ) is LegsOfHonor )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Arms ) as BaseArmor ) is ArmsOfCompassion )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.Gloves ) as BaseArmor ) is GauntletsOfValor )
				RemoveBonus( armor );
			if ( ( armor = m.FindItemOnLayer( Layer.InnerTorso ) as BaseArmor ) is BreastplateOfJustice )
				RemoveBonus( armor );

			if ( ( clothing = m.FindItemOnLayer( Layer.Shoes ) as BaseClothing ) is SolleretsOfSacrifice )
				RemoveBonus( clothing );
			if ( ( clothing = m.FindItemOnLayer( Layer.Cloak ) as BaseClothing ) is CloakOfHumility )
				RemoveBonus( clothing );
		}

		public static void ApplyBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 5;
			ba.Hue = 0;

			ba.Resistances.Physical += 5;
			ba.Resistances.Fire += 5;
			ba.Resistances.Cold += 5;
			ba.Resistances.Poison += 5;
			ba.Resistances.Energy += 5;
		}

		public static void ApplyBonusSingle( BaseArmor ba )
		{
			//ba.Resistances.Physical -= 20;
			//ba.Resistances.Fire -= 20;
			//ba.Resistances.Cold -= 20;
			//ba.Resistances.Poison -= 20;
			//ba.Resistances.Energy -= 20;
		}

		public static void RemoveBonus( BaseArmor ba )
		{
			ba.ArmorAttributes.SelfRepair = 0;
			ba.Hue = 550;

			ba.Resistances.Physical = 0;
			ba.Resistances.Fire = 0;
			ba.Resistances.Cold = 0;
			ba.Resistances.Poison = 0;
			ba.Resistances.Energy = 0;
		}

		public static void ApplyBonus( BaseClothing bc )
		{
			bc.Hue = 0;

			bc.Resistances.Physical += 5;
			bc.Resistances.Fire += 5;
			bc.Resistances.Cold += 5;
			bc.Resistances.Poison += 5;
			bc.Resistances.Energy += 5;
		}

		public static void ApplyBonusSingle( BaseClothing bc )
		{
			//bc.Resistances.Physical -= 20;
			//bc.Resistances.Fire -= 20;
			//bc.Resistances.Cold -= 20;
			//bc.Resistances.Poison -= 20;
			//bc.Resistances.Energy -= 20;			
		}

		public static void RemoveBonus( BaseClothing bc )
		{
			bc.Hue = 550;

			bc.Resistances.Physical = 0;
			bc.Resistances.Fire = 0;
			bc.Resistances.Cold = 0;
			bc.Resistances.Poison = 0;
			bc.Resistances.Energy = 0;
		}

		public static void GetPropertiesFirst( ObjectPropertyList list, Item item )
		{
			list.Add( 1072376, "8" ); // Part Of An Armor Set (8 Pieces)

			if ( item.Hue == 0 )
				list.Add( 1072377 ); // Full Armor Set Present
		}

		public static void GetPropertiesSecond( ObjectPropertyList list, Item item )
		{
			if ( item.Hue != 0 )
			{
				list.Add( 1072378 ); // <br>Only when full set is present:
				list.Add( 1072382, 20.ToString() ); // physical resist +~1_val~%
				list.Add( 1072383, 20.ToString() ); // fire resist +~1_val~%
				list.Add( 1072384, 20.ToString() ); // cold resist +~1_val~%
				list.Add( 1072385, 20.ToString() ); // poison resist +~1_val~%
				list.Add( 1072386, 20.ToString() ); // energy resist +~1_val~%
				list.Add( 1060450, 5.ToString() ); // self repair ~1_val~
			}
		}
	}
}