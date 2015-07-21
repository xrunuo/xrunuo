using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Plants;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Vasanord's corpse" )]
	public class Vasanord : VoidCreature
	{
		[Constructable]
		public Vasanord()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Vasanord";
			Body = 780;

			Hue = 2069;

			SetStr( 758, 773 );
			SetDex( 53, 75 );
			SetInt( 58, 80 );

			SetHits( 403, 496 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30, 60 );
			SetResistance( ResistanceType.Fire, 30, 60 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 60 );

			SetSkill( SkillName.MagicResist, 70.1, 90.0 );
			SetSkill( SkillName.Tactics, 60.1, 110.0 );
			SetSkill( SkillName.Wrestling, 60.1, 110.0 );
			SetSkill( SkillName.Magery, 90.1, 120.0 );
			SetSkill( SkillName.EvalInt, 90.1, 120.0 );

			Fame = 2500;
			Karma = -2500;

			PackItem( new DaemonBone( 30 ) );
			PackItem( new VoidEssence() );
			PackItem( new VoidOrb() );

			if ( 0.25 > Utility.RandomDouble() )
				PackItem( new Board( 10 ) );
			else
				PackItem( new Log( 10 ) );

			PackReg( 3 );
			PackItem( new Seed() );
			PackItem( new Seed() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public override bool BardImmune { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool PlayerRangeSensitive { get { return false; } }

		public Vasanord( Serial serial )
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

		public void SpawnKorpre( Mobile m )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			Korpre spawned = new Korpre();

			spawned.Team = this.Team;

			bool validLocation = false;
			Point3D loc = this.Location;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = X + Utility.Random( 3 ) - 1;
				int y = Y + Utility.Random( 3 ) - 1;
				int z = map.GetAverageZ( x, y );

				if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
					loc = new Point3D( x, y, Z );
				else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
					loc = new Point3D( x, y, z );
			}

			spawned.MoveToWorld( loc, map );
			spawned.Combatant = m;
		}

		public void EatKorpres()
		{
			List<Mobile> toEat = new List<Mobile>();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m is Korpre )
					toEat.Add( m );
			}

			if ( toEat.Count > 0 )
			{
				PlaySound( Utility.Random( 0x3B, 2 ) ); // Eat sound

				foreach ( Mobile m in toEat )
				{
					Hits += ( m.Hits / 2 );
					m.Delete();
				}
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( this.Hits > ( this.HitsMax / 4 ) )
			{
				if ( 0.25 >= Utility.RandomDouble() )
					SpawnKorpre( attacker );
			}
			else if ( 0.25 >= Utility.RandomDouble() )
			{
				EatKorpres();
			}
		}
	}
}
