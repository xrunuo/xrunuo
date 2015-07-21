using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;

namespace Server.Misc
{
	public class RegenRates
	{
		[CallPriority( 10 )]
		public static void Configure()
		{
			Mobile.DefaultHitsRate = TimeSpan.FromSeconds( 11.0 );
			Mobile.DefaultStamRate = TimeSpan.FromSeconds( 7.0 );
			Mobile.DefaultManaRate = TimeSpan.FromSeconds( 7.0 );

			Mobile.ManaRegenRateHandler = new RegenRateHandler( Mobile_ManaRegenRate );
			Mobile.StamRegenRateHandler = new RegenRateHandler( Mobile_StamRegenRate );
			Mobile.HitsRegenRateHandler = new RegenRateHandler( Mobile_HitsRegenRate );
		}

		private static void CheckBonusSkill( Mobile m, int cur, int max, SkillName skill )
		{
			if ( !m.Alive )
				return;

			double n = (double) cur / max;
			double v = Math.Sqrt( m.Skills[skill].Value * 0.005 );

			n *= ( 1.0 - v );
			n += v;

			m.CheckSkill( skill, n );
		}

		private static bool CheckTransform( Mobile m, Type type )
		{
			return TransformationSpell.UnderTransformation( m, type );
		}

		private static bool CheckAnimal( Mobile m, Type type )
		{
			return AnimalForm.UnderTransformation( m, type );
		}

		private static TimeSpan Mobile_HitsRegenRate( Mobile from )
		{
			int points = from.GetMagicalAttribute( MagicalAttribute.RegenHits );

			if ( from is BaseCreature && !( (BaseCreature) from ).IsAnimatedDead )
				points += 4;

			if ( CheckTransform( from, typeof( HorrificBeastSpell ) ) )
				points += 20;

			if ( CheckAnimal( from, typeof( Dog ) ) || CheckAnimal( from, typeof( Cat ) ) )
				points += from.Skills[SkillName.Ninjitsu].Fixed / 200;

			if ( from.IsPlayer && from.Race == Race.Human )
				points += 2;

			if ( SurgeShield.IsUnderSurgeEffect( from, SurgeEffect.HitPoint ) )
				points += 10;

			Utility.FixMin( ref points, 0 );

			// Publish 42: Added caps to stat regen rates: HP Regeneration: 18
			if ( from.IsPlayer )
				Utility.FixMax( ref points, 18 );

			if ( CheckBump( from ) )
				points += 40;

			return TimeSpan.FromSeconds( 1.0 / ( 0.1 * ( 1 + points ) ) );
		}

		private static TimeSpan Mobile_StamRegenRate( Mobile from )
		{
			if ( from.Skills == null )
				return Mobile.DefaultStamRate;

			CheckBonusSkill( from, from.Stam, from.StamMax, SkillName.Focus );

			int points = from.GetMagicalAttribute( MagicalAttribute.RegenStam ) + (int) ( from.Skills[SkillName.Focus].Value * 0.1 );

			if ( CheckTransform( from, typeof( VampiricEmbraceSpell ) ) )
				points += 15;

			if ( CheckAnimal( from, typeof( Kirin ) ) )
				points += 20;

			if ( CheckBump( from ) )
				points += 40;

			if ( SurgeShield.IsUnderSurgeEffect( from, SurgeEffect.Stamina ) )
				points += 10;

			Utility.FixMin( ref points, -1 );

			// Publish 42: Added caps to stat regen rates: Stamina Regeneration: 24
			if ( from.IsPlayer )
				Utility.FixMax( ref points, 24 );

			return TimeSpan.FromSeconds( 1.0 / ( 0.1 * ( 2 + points ) ) );
		}

		private static TimeSpan Mobile_ManaRegenRate( Mobile from )
		{
			if ( from.Skills == null )
				return Mobile.DefaultManaRate;

			if ( !from.Meditating )
				CheckBonusSkill( from, from.Mana, from.ManaMax, SkillName.Meditation );

			double basePoints = 2.0;

			double meditation = from.Skills[SkillName.Meditation].Value;
			double focus = from.Skills[SkillName.Focus].Value;

			double meditationBonus = 0.0;

			if ( AllowMeditation( from ) )
			{
				meditationBonus = ( from.Int + ( 3 * meditation ) ) / 40.0;

				if ( meditation >= 100.0 )
					meditationBonus *= 1.1;

				if ( from.Meditating )
					meditationBonus *= 2.0;

				CheckBonusSkill( from, from.Mana, from.ManaMax, SkillName.Focus );
			}

			double focusBonus = (int) ( focus * 0.05 );

			double baseItemBonus = ( meditation / 2.0 + focus / 4.0 ) / 90.0;
			baseItemBonus = ( baseItemBonus * 0.65 ) + 2.35;

			double intensityBonus = from.GetMagicalAttribute( MagicalAttribute.RegenMana );

			// ********** Special Bonuses **********
			if ( CheckTransform( from, typeof( VampiricEmbraceSpell ) ) )
				intensityBonus += 3;
			else if ( CheckTransform( from, typeof( LichFormSpell ) ) )
				intensityBonus += 13;
			// *************************************

			// Gargoyle Racial Ability: Mystic Insight
			if ( from.IsPlayer && from.Race == Race.Gargoyle )
				intensityBonus += 2;
			// *************************************

			if ( intensityBonus > 30 )
				intensityBonus = 30;

			intensityBonus = Math.Sqrt( intensityBonus );

			double itemBonus = ( baseItemBonus * intensityBonus ) - ( baseItemBonus - 1 );
			if ( itemBonus < 0 )
				itemBonus = 0;

			double totalPoints = basePoints + meditationBonus + focusBonus + itemBonus;

			if ( CheckBump( from ) )
				totalPoints += 40;

			if ( SurgeShield.IsUnderSurgeEffect( from, SurgeEffect.Mana ) )
				totalPoints += 10;

			if ( totalPoints < 1 )
				totalPoints = 1;

			return TimeSpan.FromSeconds( 1.0 / ( 0.1 * ( (int) totalPoints ) ) );
		}

		private static bool CheckBump( Mobile from )
		{
			if ( from is BaseCreature && ( (BaseCreature) from ).IsParagon )
				return true;

			if ( from is Leviathan )
				return true;

			return false;
		}

		public static bool AllowMeditation( Mobile from )
		{
			bool allowed = true;

			allowed &= IsMeditable( from.NeckArmor as BaseArmor );
			allowed &= IsMeditable( from.HandArmor as BaseArmor );
			allowed &= IsMeditable( from.HeadArmor as BaseArmor );
			allowed &= IsMeditable( from.ArmsArmor as BaseArmor );
			allowed &= IsMeditable( from.LegsArmor as BaseArmor );
			allowed &= IsMeditable( from.ChestArmor as BaseArmor );

			return allowed;
		}

		private static bool IsMeditable( BaseArmor ar )
		{
			if ( ar == null || ar.ArmorAttributes.MageArmor != 0 || ar.Attributes.SpellChanneling != 0 )
				return true;

			return ar.Meditable;
		}
	}
}