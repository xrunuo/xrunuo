using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	public class TormentedMinotaur : Minotaur
	{
		public override int Meat { get { return 2; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override int TreasureMapLevel { get { return 5; } }

		[Constructable]
		public TormentedMinotaur()
			: base()
		{
			Name = "Tormented Minotaur";

			Body = 262;
			BaseSoundID = 427;

			SetStr( 800, 950 );
			SetDex( 400, 450 );
			SetInt( 120, 140 );

			SetHits( 4000, 4200 );

			SetDamage( 16, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );
			SetResistance( ResistanceType.Fire, 70, 80 );

			SetSkill( SkillName.MagicResist, 105, 115 );
			SetSkill( SkillName.Tactics, 105, 120 );
			SetSkill( SkillName.Wrestling, 100, 105 );

			Fame = 15000;
			Karma = -15000;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.05 >= Utility.RandomDouble() )
			{
				GroundSlap();
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
			AddLoot( LootPack.FilthyRich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 3000 > Utility.Random( 100000 ) )
			{
				c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}

		}

		public void GroundSlap()
		{
			Map map = this.Map;

			if ( map == null )
			{
				return;
			}

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 8 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
				{
					continue;
				}

				if ( m is BaseCreature && ( ( (BaseCreature) m ).Controlled || ( (BaseCreature) m ).Summoned || ( (BaseCreature) m ).Team != this.Team ) )
				{
					targets.Add( m );
				}
				else if ( m.Player )
				{
					targets.Add( m );
				}
			}

			PlaySound( 0x140 );


			for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile) targets[i];

				int distance = (int) m.GetDistanceToSqrt( this );

				if ( distance == 0 )
					distance = 1;

				double damage = 75 / distance;
				damage = targets.Count * damage;

				damage = 10 * targets.Count;

				if ( damage < 40.0 )
				{
					damage = 40.0;
				}
				else if ( damage > 75.0 )
				{
					damage = 75.0;
				}

				DoHarmful( m );

				AOS.Damage( m, this, (int) damage, 100, 0, 0, 0, 0 );

				m.FixedParticles( 0x3728, 10, 15, 9955, EffectLayer.Waist );
				BaseMount.Dismount( m );

				if ( m.Alive && m.Body.IsHuman && !m.Mounted )
				{
					m.Animate( 20, 7, 1, true, false, 0 ); // take hit
				}
			}
		}

		public TormentedMinotaur( Serial serial )
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