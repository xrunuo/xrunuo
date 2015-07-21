using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a green goblin alchemist corpse" )]
	public class GreenGoblinAlchemist : BaseCreature
	{
		[Constructable]
		public GreenGoblinAlchemist()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "a green goblin alchemist";
			Body = 723;

			SetStr( 258, 342 );
			SetDex( 61, 80 );
			SetInt( 100, 150 );

			SetHits( 150, 200 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 120.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 95.1, 110.0 );

			Fame = 5000;
			Karma = -5000;

			if ( 0.25 > Utility.RandomDouble() )
				PackItem( new GoblinBlood() );
		}

		public override int GetAngerSound() { return 0x600; }
		public override int GetAttackSound() { return 0x5FD; }
		public override int GetHurtSound() { return 0x5FF; }
		public override int GetDeathSound() { return 0x5FE; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Gems, 1 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( from == null && ( ( from = Combatant ) == null ) )
				return;

			if ( 0.5 > Utility.RandomDouble() )
			{
				Point3D loc = from.Location;

				// Throw a bottle!
				Effects.SendMovingParticles( this, from, 0x1C19, 10, 0, false, false, 0x1395, 1, 0xFFFF );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
					delegate
					{
						if ( from.Map == null )
							return;

						switch ( Utility.Random( 3 ) )
						{
							case 0: // The bottle was empty
								{
									from.PlaySound( 0x3F );
									AOS.Damage( from, this, Utility.RandomMinMax( 5, 10 ), 100, 0, 0, 0, 0 );

									break;
								}
							case 1: // Supernova Potion
								{
									Effects.PlaySound( loc, from.Map, 0x11D );

									List<Mobile> targets = new List<Mobile>();

									foreach ( Mobile target in from.Map.GetMobilesInRange( loc, 2 ) )
									{
										if ( target is GreenGoblinAlchemist )
											continue;

										targets.Add( target );
									}

									for ( int i = 0; i < targets.Count; i++ )
									{
										Mobile target = targets[i];

										AOS.Damage( target, this, Utility.RandomMinMax( 45, 55 ), 0, 100, 0, 0, 0 );
									}

									for ( int x = -2; x <= 2; x++ )
									{
										for ( int y = -2; y <= 2; y++ )
										{
											Point3D p = new Point3D( loc.X + x, loc.Y + y, loc.Z );
											int dist = (int) Math.Round( Utility.GetDistanceToSqrt( loc, p ) );

											if ( dist <= 2 )
											{
												Timer.DelayCall( TimeSpan.FromSeconds( 0.2 * dist ), new TimerCallback(
													delegate
													{
														Effects.SendPacket( loc, from.Map, new HuedEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x3709, p, p, 20, 30, true, false, 1502, 4 ) );
													}
												) );
											}
										}
									}

									break;
								}
							case 2: // Acid potion
								{
									Effects.SendTargetParticles( from, 0x374A, 1, 10, 0x557, 0, 0x139D, EffectLayer.Waist, 0 );

									from.PlaySound( 0x22F );
									from.SendLocalizedMessage( 1077483 ); // The acid burns you!

									AcidTimer timer = new AcidTimer( from );
									timer.Start();

									break;
								}
						}
					} ) );
			}
		}

		public GreenGoblinAlchemist( Serial serial )
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

		private class AcidTimer : Timer
		{
			private Mobile m_Target;
			private int m_Count;

			public AcidTimer( Mobile target )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				
				m_Target = target;
				m_Count = 3;
			}

			protected override void OnTick()
			{
				m_Count--;

				if ( !m_Target.Alive || m_Count == 0 )
					Stop();

				AOS.Damage( m_Target, Utility.RandomMinMax( 10, 15 ), 0, 0, 0, 100, 0 );
			}
		}
	}
}
