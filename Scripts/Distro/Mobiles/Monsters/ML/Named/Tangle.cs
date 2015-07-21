using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Tangles corpse" )]
	public class Tangle : BaseCreature
	{
		[Constructable]
		public Tangle()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 1.2 )
		{
			Name = "Tangle";
			Body = 780;
			Hue = 1194;

			SetStr( 840, 845 );
			SetDex( 65, 70 );
			SetInt( 55, 60 );

			SetHits( 2505, 2505 );

			SetDamage( 10, 23 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 50, 55 );
			SetResistance( ResistanceType.Fire, 40, 40 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 40, 45 );

			SetSkill( SkillName.MagicResist, 105, 110 );
			SetSkill( SkillName.Tactics, 95, 100 );
			SetSkill( SkillName.Wrestling, 85, 90 );

			Fame = 8000;
			Karma = -8000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );		
							
			if ( Utility.RandomDouble() < 0.3 )
				c.DropItem( new TaintedSeeds() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

		}

		public override void GenerateLoot()
		{
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public Tangle( Serial serial )
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
			/*int version = */reader.ReadInt();
		}

		public void SpawnBogling( Mobile m )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			Bogling spawned = new Bogling();

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

		public void EatBoglings()
		{
			ArrayList toEat = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m is Bogling )
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
					SpawnBogling( attacker );
			}
			else if ( 0.25 >= Utility.RandomDouble() )
			{
				EatBoglings();
			}
		}
	}
}
