using System;
using Server.Mobiles;
using Server.Items;
using BunnyHole = Server.Mobiles.VorpalBunny.BunnyHole;

namespace Server.Mobiles
{
	[CorpseName( "a hare corpse" )]
	public class FireRabbit : BaseCreature
	{
		[Constructable]
		public FireRabbit()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a fire rabbit";
			Body = 205;

			Hue = 0x550;

			SetStr( 130 );
			SetDex( 4500 );
			SetInt( 2500 );

			SetHits( 2500 );
			SetMana( 1500 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Physical, 49 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.MagicResist, 200 );
			SetSkill( SkillName.Tactics, 10.2 );
			SetSkill( SkillName.Wrestling, 80.0 );
			SetSkill( SkillName.Anatomy, 18.0 );

			Fame = 150;
			Karma = -150;

			PackItem( new Carrot( Utility.RandomMinMax( 5, 10 ) ) );
		}

		public override void OnThink()
		{
			if ( Hits < ( HitsMax / 5 ) )
				BeginTunnel();
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( Utility.RandomDouble() < 0.5 )
				c.DropItem( new AnimalPheromone() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich, 3 );
		}

		public virtual void BeginTunnel()
		{
			if ( Deleted )
				return;

			new BunnyHole().MoveToWorld( Location, Map );

			Frozen = true;
			Say( "* The bunny begins to dig a tunnel back to its underground lair *" );
			PlaySound( 0x247 );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( Delete ) );
		}

		public override bool IsScaryToPets { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }
		public override bool BardImmune { get { return true; } }

		public FireRabbit( Serial serial )
			: base( serial )
		{
		}

		public override int GetAttackSound()
		{
			return 0xC9;
		}

		public override int GetHurtSound()
		{
			return 0xCA;
		}

		public override int GetDeathSound()
		{
			return 0xCB;
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