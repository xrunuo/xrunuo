using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "Stygian Dragon's corpse" )]
	public class StygianDragon : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			if ( Utility.RandomBool() )
				return WeaponAbility.TalonStrike;
			else
				return WeaponAbility.Bladeweave;
		}

		private const int MinDelay = 25;
		private const int MaxDelay = 35;

		private DateTime m_NextMeteorTime;

		[Constructable]
		public StygianDragon()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Stygian Dragon";
			Body = 826;

			SetStr( 809, 894 );
			SetDex( 239, 345 );
			SetInt( 108, 183 );

			SetHits( 30000 );
			SetStam( 471, 486 );

			SetDamage( 33, 55 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 80, 85 );
			SetResistance( ResistanceType.Cold, 65, 70 );
			SetResistance( ResistanceType.Poison, 85, 90 );
			SetResistance( ResistanceType.Energy, 80, 85 );

			SetSkill( SkillName.Anatomy, 110.0, 120.0 );
			SetSkill( SkillName.MagicResist, 120.0, 130.0 );
			SetSkill( SkillName.Tactics, 110.0, 120.0 );
			SetSkill( SkillName.Wrestling, 110.0, 120.0 );

			Fame = 22500;
			Karma = -22500;

			SpeechHue = 0x21;
		}

		public override int GetAttackSound() { return 0x63E; }
		public override int GetDeathSound() { return 0x63F; }
		public override int GetHurtSound() { return 0x640; }
		public override int GetIdleSound() { return 0x641; }

		public override bool CausesTrueFear { get { return true; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.UtcNow > m_NextMeteorTime && 0.1 > Utility.RandomDouble() )
			{
				Say( true, "*Des-ailem Flam*" );
				MonsterHelper.CrimsonMeteor( this );

				m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( MinDelay, MaxDelay ) );
			}

			if ( 0.1 > Utility.RandomDouble() )
			{
				Mobile target = from;

				if ( target == null )
					target = Combatant;

				if ( target == null )
					return;

				int random = Utility.Random( 2 );

				switch ( random )
				{
					default:
					case 0:
						{
							MonsterHelper.StygianFireball( this, target, 0, 0x46E9, 0, 50, 0, 0, 50 );
							break;
						}
					case 1:
						{
							MonsterHelper.FireWall( this, target );
							break;
						}
				}
			}
		}

		public override int Hides { get { return 88; } }
		public override int Meat { get { return 19; } }
		public override int Blood { get { return 48; } }

		public override int TreasureMapLevel { get { return 5; } }

		// Flying overrides all Z checks
		public override bool Flying { get { return true; } set { } }

		public override bool CanFlee { get { return false; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 150; } }

		#region Loot
		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
		}

		private static Type[] UniqueArtifacts = new Type[]
			{
				typeof( BurningAmber ),		typeof( DraconisWrath ),
				typeof( DragonHideShield ),	typeof( FallenMysticsSpellbook ),
				typeof( LifeSyphon ),		typeof( SignOfOrder ),
				typeof( VampiricEssence ),	typeof( GargishSignOfOrder )
			};

		private static Type[] SharedArtifacts = new Type[]
			{
				typeof( AxesOfFury ),		typeof( GiantSteps ),
				typeof( PetrifiedSnake ),	typeof( StoneDragonsTooth ),
				typeof( SummonersKilt ),	typeof( TokenOfHolyFavor )
			};

		private static Item CreateArtifact( Type[] types )
		{
			try { return (Item) Activator.CreateInstance( types[Utility.Random( types.Length )] ); }
			catch { }

			return null;
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new StygianDragonHead() );

			double random = Utility.RandomDouble();

			Item artifact = null;

			if ( 0.025 > random ) // 2.5% of getting a unique artifact
				artifact = CreateArtifact( UniqueArtifacts );
			else if ( 0.20 > random ) // 17.5% of getting a shared artifact
				artifact = CreateArtifact( SharedArtifacts );

			if ( artifact != null )
			{
				Mobile m = MonsterHelper.GetTopAttacker( this );

				if ( m != null )
					MonsterHelper.GiveArtifactTo( m, artifact );
				else
					artifact.Delete();
			}
		}
		#endregion

		public StygianDragon( Serial serial )
			: base( serial )
		{
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
		}
	}
}
