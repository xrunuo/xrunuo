using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gremlin corpse" )]
	public class Gremlin : BaseCreature
	{
		[Constructable]
		public Gremlin()
			: base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gremlin";
			Body = 724;
			BaseSoundID = 0x45A;

			SetStr( 90, 120 );
			SetDex( 110, 130 );
			SetInt( 36 );

			SetHits( 65, 75 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 35 );
			SetResistance( ResistanceType.Cold, 15, 20 );
			SetResistance( ResistanceType.Poison, 15, 20 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.Anatomy, 80.1, 85.0 );
			SetSkill( SkillName.Healing, 85.1, 90.0 );
			SetSkill( SkillName.MagicResist, 70.1, 75.0 );
			SetSkill( SkillName.Tactics, 55.1, 75.0 );
			SetSkill( SkillName.DetectHidden, 100.1, 120.0 );
			SetSkill( SkillName.Archery, 80.1, 120.0 );
			SetSkill( SkillName.Parry, 40.1, 60.0 );
			SetSkill( SkillName.Hiding, 100.0, 120.0 );
			SetSkill( SkillName.Stealth, 80.1, 120.0 );

			Fame = 2000;
			Karma = -2000;

			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 50, 70 ) ) );
			
			PackItem( new Bandage( Utility.RandomMinMax( 10, 15 ) ) );
			PackItem( new Apple( Utility.RandomMinMax( 3, 6 ) ) );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public Gremlin( Serial serial )
			: base( serial )
		{
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}
		
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
				
		private Mobile FindTarget()
		{
			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m.IsPlayer && m.Hidden && m.AccessLevel == AccessLevel.Player )
					return m;
			}

			return null;
		}
		
		public Point3D FindTeleportPosition( int range )
		{
			Map map = Map;

			if ( map == null )
				return Location;

			for ( int i = 0; i < 10; i++ )
			{
				int x = Location.X + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int y = Location.Y + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int z = Map.GetAverageZ( x, y );

				if ( Map.CanSpawnMobile( new Point2D( x, y ), this.Z ) )
					return new Point3D( x, y, this.Z );
				else if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return this.Location;
		}

		private void TryToDetectHidden()
		{
			Mobile m = FindTarget();

			if ( m != null )
			{
				if ( DateTime.Now >= this.NextSkillTime && UseSkill( SkillName.DetectHidden ) )
				{
					Target targ = this.Target;

					if ( targ != null )
						targ.Invoke( this, this );

					Effects.PlaySound( this.Location, this.Map, 0x340 );
				}
			}
		}

		private void HealSelf()
		{
			if ( BandageContext.GetContext( this ) == null )
				BandageContext.BeginHeal( this, this, null );

			return;
		}
		
		public override void OnThink()
		{
			if ( Utility.RandomDouble() < 0.6 && Hits < ( HitsMax - 15 ) && !Hidden )
				HealSelf();

			if ( Utility.RandomDouble() < 0.2 )
				TryToDetectHidden();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( !willKill && Hits < ( HitsMax / 2 ) && Utility.RandomBool() )
			{
				Point3D point = FindTeleportPosition( 10 );

				if ( point != Location )
				{
					MoveToWorld( point, Map );

					FixedParticles( 0x376A, 9, 32, 0x13AF, EffectLayer.Waist );
					PlaySound( 0x1FE );
				}
			}
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
