using System;
using Server;
using Server.Items;
using Server.Spells.Ninjitsu;

namespace Server.Mobiles
{
	public class Paragon
	{
		public static double ChestChance = .10; // Chance that a paragon will carry a paragon chest

		public static Map[] Maps = new Map[] // Maps that paragons will spawn on
			{
				Map.Ilshenar
			};

		public static Type[] Artifacts = new Type[] 
			{
				typeof( GoldBricks ),				typeof( PhillipsWoodenSteed ), 
				typeof( AlchemistsBauble ),			typeof( ArcticDeathDealer ),
				typeof( BlazeOfDeath ),				typeof( BowOfTheJukaKing ),
				typeof( BurglarsBandana ),			typeof( CavortingClub ),
				typeof( EnchantedTitanLegBone ),	typeof( GwennosHarp ),
				typeof( IolosLute ),				typeof( LunaLance ),
				typeof( NightsKiss ),				typeof( NoxRangersHeavyCrossbow ),
				typeof( OrcishVisage ),				typeof( PolarBearMask ),
				typeof( ShieldOfInvulnerability ),	typeof( StaffOfPower ),
				typeof( VioletCourage ),			typeof( HeartOfTheLion ),
				typeof( WrathOfTheDryad ),			typeof( PixieSwatter ),
				typeof( GlovesOfThePugilist )
			};

		public static int Hue = 0x501; // Paragon hue

		// Buffs
		public static double HitsBuff = 5.0;
		public static double StrBuff = 1.05;
		public static double IntBuff = 1.20;
		public static double DexBuff = 1.20;
		public static double SkillsBuff = 1.20;
		public static double SpeedBuff = 1.20;
		public static double FameBuff = 1.40;
		public static double KarmaBuff = 1.40;
		public static int DamageBuff = 5;

		public static void Initialize()
		{
			BaseCreature.KilledBy += new BaseCreature.KilledByHandler( OnKilledBy );
		}

		private static void OnKilledBy( BaseCreature bc, Mobile killer )
		{
			if ( bc.IsParagon && CheckArtifactChance( killer, bc ) )
				GiveArtifactTo( killer );
		}

		public static void Convert( BaseCreature bc )
		{
			if ( bc.IsParagon )
				return;

			bc.Hue = Hue;

			if ( bc.HitsMaxSeed >= 0 )
				bc.HitsMaxSeed = (int) ( bc.HitsMaxSeed * HitsBuff );

			bc.RawStr = (int) ( bc.RawStr * StrBuff );
			bc.RawInt = (int) ( bc.RawInt * IntBuff );
			bc.RawDex = (int) ( bc.RawDex * DexBuff );

			bc.Hits = bc.HitsMax;
			bc.Mana = bc.ManaMax;
			bc.Stam = bc.StamMax;

			for ( int i = 0; i < bc.Skills.Length; i++ )
			{
				Skill skill = (Skill) bc.Skills[i];

				if ( skill.Base > 0.0 )
					skill.Base *= SkillsBuff;
			}

			bc.PassiveSpeed /= SpeedBuff;
			bc.ActiveSpeed /= SpeedBuff;

			bc.DamageMin += DamageBuff;
			bc.DamageMax += DamageBuff;

			if ( bc.Fame > 0 )
				bc.Fame = (int) ( bc.Fame * FameBuff );

			if ( bc.Fame > 32000 )
				bc.Fame = 32000;

			// TODO: Mana regeneration rate = Sqrt( buffedFame ) / 4

			if ( bc.Karma != 0 )
			{
				bc.Karma = (int) ( bc.Karma * KarmaBuff );

				if ( Math.Abs( bc.Karma ) > 32000 )
					bc.Karma = 32000 * Math.Sign( bc.Karma );
			}
		}

		public static void UnConvert( BaseCreature bc )
		{
			if ( !bc.IsParagon )
				return;

			bc.Hue = 0;

			if ( bc.HitsMaxSeed >= 0 )
				bc.HitsMaxSeed = (int) ( bc.HitsMaxSeed / HitsBuff );

			bc.RawStr = (int) ( bc.RawStr / StrBuff );
			bc.RawInt = (int) ( bc.RawInt / IntBuff );
			bc.RawDex = (int) ( bc.RawDex / DexBuff );

			bc.Hits = bc.HitsMax;
			bc.Mana = bc.ManaMax;
			bc.Stam = bc.StamMax;

			for ( int i = 0; i < bc.Skills.Length; i++ )
			{
				Skill skill = (Skill) bc.Skills[i];

				if ( skill.Base > 0.0 )
					skill.Base /= SkillsBuff;
			}

			bc.PassiveSpeed *= SpeedBuff;
			bc.ActiveSpeed *= SpeedBuff;

			bc.DamageMin -= DamageBuff;
			bc.DamageMax -= DamageBuff;

			if ( bc.Fame > 0 )
				bc.Fame = (int) ( bc.Fame / FameBuff );
			if ( bc.Karma != 0 )
				bc.Karma = (int) ( bc.Karma / KarmaBuff );
		}

		public static bool CheckConvert( BaseCreature bc )
		{
			return CheckConvert( bc, bc.Location, bc.Map );
		}

		public static bool CheckConvert( BaseCreature bc, Point3D location, Map m )
		{
			if ( Array.IndexOf( Maps, m ) == -1 )
				return false;

			if ( bc is BaseChampion || bc is Harrower || bc is BaseVendor || bc is BaseEscortable || bc.Summoned || bc.Controlled || bc is Clone )
				return false;

			if ( bc is DreadHorn || bc is MonstrousInterredGrizzle || bc is Travesty || bc is ChiefParoxysmus || bc is LadyMelisande || bc is ShimmeringEffusion )
				return false;

			int fame = bc.Fame;

			if ( fame > 32000 )
				fame = 32000;

			double chance = 1 / Math.Round( 20.0 - ( fame / 3200 ) );

			return ( chance > Utility.RandomDouble() );
		}

		public static double SackOfSugarChance = .05; // Chance that a paragon will carry a Sack Of Sugar 
		public static double VanillaChance = .05; // Chance that a paragon will carry Vanilla

		public static bool CheckArtifactChance( Mobile m, BaseCreature bc )
		{
			if ( !m.Alive )
				return false;

			double fame = (double) bc.Fame;

			if ( fame > 32000 )
				fame = 32000;

			double chance = 1 / ( Math.Max( 10, 100 * ( 0.83 - Math.Round( Math.Log( Math.Round( fame / 6000, 3 ) + 0.001, 10 ), 3 ) ) ) * ( 100 - Math.Sqrt( m.Luck ) ) / 100.0 );

			return chance > Utility.RandomDouble();
		}

		public static void GiveArtifactTo( Mobile m )
		{
			Item item = (Item) Activator.CreateInstance( Artifacts[Utility.Random( Artifacts.Length )] );

			if ( m.AddToBackpack( item ) )
			{
				m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
			}
			else
			{
				Container bank = m.BankBox;

				if ( bank != null && bank.TryDropItem( m, item, false ) )
				{
					m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
				}
				else
				{
					m.SendLocalizedMessage( 1072523, "", 64 ); // You find an artifact, but your backpack and bank are too full to hold it.

					item.MoveToWorld( m.Location, m.Map );
				}
			}

			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( m.Location, m.Map, 0x243 );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 4, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 4, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

			Effects.SendTargetParticles( m, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );
		}
	}
}