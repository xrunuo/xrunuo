using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "Slasher of Veils's corpse" )]
	public class SlasherOfVeils : BaseCreature
	{
		private const int MinDelay = 6;
		private const int MaxDelay = 9;

		private DateTime m_NextAbilityTime;

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public SlasherOfVeils()
			: base( AIType.AI_Mage, FightMode.Strongest, 10, 1, 0.2, 0.4 )
		{
			Name = "Slasher of Veils";
			Body = 741;

			SetStr( 917, 1039 );
			SetDex( 126, 142 );
			SetInt( 1009, 1256 );

			SetHits( 50000 );
			SetMana( 50000 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.Anatomy, 113.9, 125.0 );
			SetSkill( SkillName.MagicResist, 111.1, 188.7 );
			SetSkill( SkillName.Tactics, 120.8, 126.8 );
			SetSkill( SkillName.Wrestling, 119.8, 128.3 );
			SetSkill( SkillName.EvalInt, 111.1, 128.7 );
			SetSkill( SkillName.Magery, 115.3, 123.8 );
			SetSkill( SkillName.Meditation, 110.9, 127.0 );

			Fame = 32000;
			Karma = -32000;

			SpeechHue = 0x21;
		}

		#region Pet Slayer
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			base.AlterMeleeDamageTo( to, ref damage );

			if ( !to.Player )
			{
				damage *= 5;

				Hits += damage;
			}
		}
		#endregion

		#region AngryFire
		private void AngryFire( Mobile defender )
		{
			int damage = defender.Hits / 2;

			AOS.Damage( defender, this, damage, 0, 100, 0, 0, 0 );

			defender.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
			defender.PlaySound( 0x208 );

			defender.SendLocalizedMessage( 1070823 ); // The creature hits you with its Angry Fire.
		}
		#endregion

		#region Paralyze
		private void Paralyze( Mobile defender )
		{
			defender.Paralyze( TimeSpan.FromSeconds( 3.0 ) );

			defender.FixedEffect( 0x376A, 6, 1 );
			defender.PlaySound( 0x204 );

			defender.SendLocalizedMessage( 1060164 ); // The attack has temporarily paralyzed you!
		}
		#endregion

		#region Spell Chain
		private bool m_InSpellChain;

		public override bool InstantCast { get { return m_InSpellChain; } }

		public override bool CheckSpellCast( ISpell spell )
		{
			if ( m_InSpellChain )
				Say( true, "*Ku Ort*" );

			return base.CheckSpellCast( spell );
		}

		private void SpellChain()
		{
			Say( true, "*Mur Kal Ort*" );

			Effects.SendPacket( this.Location, this.Map, new TargetEffect( this, 0x3709, 10, 30, 0x480, 4 ) );

			m_InSpellChain = true;

			Timer.DelayCall( TimeSpan.FromSeconds( 4.0 + ( 2.0 * Utility.RandomDouble() ) ), new TimerCallback(
				delegate { m_InSpellChain = false; } ) );
		}
		#endregion

		#region Rock Rain
		private void RockRain( Mobile target )
		{
			target.SendLocalizedMessage( 1114817, "", 33 ); // The Slasher emits a powerful howl, shaking the very walls around you and suppressing your ability to move.
			target.Frozen = true;

			target.Send( new FlashEffect( FlashType.FadeIn ) );

			Timer rockRainTimer = new RockRainTimer( target );
			rockRainTimer.Start();
		}

		private class RockRainTimer : Timer
		{
			private Mobile m_Target;
			private Point3D m_EffectLocation;
			private Map m_EffectMap;
			private int m_Ticks;

			public RockRainTimer( Mobile target )
				: base( TimeSpan.FromSeconds( 3.0 ), TimeSpan.FromSeconds( 0.25 ) )
			{
				
				m_Target = target;

				m_EffectLocation = target.Location;
				m_EffectMap = target.Map;

				m_Ticks = 20;
			}

			protected override void OnTick()
			{
				Point3D dest = m_EffectLocation;
				Point3D orig = new Point3D( dest.X - Utility.RandomMinMax( 3, 4 ), dest.Y - Utility.RandomMinMax( 8, 9 ), dest.Z + Utility.RandomMinMax( 41, 43 ) );

				int itemId = Utility.RandomMinMax( 0x1362, 0x136D );
				int speed = Utility.RandomMinMax( 5, 10 );
				int hue = Utility.RandomBool() ? 0 : Utility.RandomMinMax( 0x456, 0x45F );

				Effects.SendPacket( m_EffectLocation, m_EffectMap, new HuedEffect( EffectType.Moving, Serial.Zero, Serial.Zero, itemId, orig, dest, speed, 0, false, false, hue, 4 ) );
				Effects.SendPacket( m_EffectLocation, m_EffectMap, new HuedEffect( EffectType.Moving, Serial.Zero, Serial.Zero, itemId, orig, dest, speed, 0, false, false, hue, 4 ) );

				Effects.PlaySound( m_EffectLocation, m_EffectMap, 0x15E + Utility.Random( 3 ) );
				Effects.PlaySound( m_EffectLocation, m_EffectMap, Utility.RandomList( 0x305, 0x306, 0x307, 0x309 ) );

				if ( m_Target.Alive )
					AOS.Damage( m_Target, Utility.RandomMinMax( 25, 35 ), 100, 0, 0, 0, 0 );

				if ( --m_Ticks == 0 )
				{
					m_Target.Frozen = false;
					m_Target.SendLocalizedMessage( 1005603 ); // You can move again!

					Stop();
				}
			}
		}
		#endregion

		public override bool CausesTrueFear { get { return true; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			switch ( Utility.Random( 10 ) )
			{
				case 0: AngryFire( defender ); break;
				case 1: Paralyze( defender ); break;
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.UtcNow > m_NextAbilityTime && 0.1 > Utility.RandomDouble() )
			{
				Mobile target = from;

				if ( target == null )
					target = Combatant;

				if ( target == null )
					return;

				int random = Utility.Random( 10 );

				switch ( random )
				{
					case 0:
					case 1:
						{
							Say( true, "*Ul Flam*" );
							MonsterHelper.StygianFireball( this, target, 0x5DE, 0x36D4, 0, 100, 0, 0, 0 );
							break;
						}
					case 2:
						{
							RockRain( target );
							break;
						}
					case 3:
						{
							MonsterHelper.FireWall( this, target );
							break;
						}
					default:
						{
							SpellChain();
							break;
						}
				}

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( MinDelay, MaxDelay ) );
			}
		}

		// Return to home if we go away from the ancient gargoyle city
		protected override bool OnMove( Direction d )
		{
			if ( !base.OnMove( d ) )
				return false;

			if ( Spawner != null && !this.InRange( Spawner.HomeLocation, RangeHome ) )
			{
				MoveToWorld( Spawner.HomeLocation, Spawner.Map );
				return false;
			}

			return true;
		}

		// This one plus FightMode.Weakest = Daemonic Targeting ;)
		public override double ChangeCombatantChance { get { return 0.1; } }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int GetAttackSound() { return 0x632; }
		public override int GetDeathSound() { return 0x633; }
		public override int GetHurtSound() { return 0x634; }
		public override int GetIdleSound() { return 0x635; }

		#region Loot
		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
		}

		private static Type[] UniqueArtifacts = new Type[]
			{
				typeof( Lavaliere ),		typeof( ClawsOfTheBerserker ),
				typeof( Mangler ),			typeof( SignOfChaos ),
				typeof( GargishSignOfChaos )
			};

		private static Type[] SharedArtifacts = new Type[]
			{
				typeof( BladeOfBattle ),	typeof( DemonBridleRing ),
				typeof( PetrifiedSnake ),	typeof( SwordOfShatteredHopes ),
				typeof( SummonersKilt ),	typeof( BreastplateOfTheBerserker ),
				typeof( PillarOfStrength ),
			};

		private static Item CreateArtifact( Type[] types )
		{
			try
			{
				return (Item) Activator.CreateInstance( types[Utility.Random( types.Length )] );
			}
			catch
			{
			}

			return null;
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new ClawOfSlasherOfVeils() );

			// These 2 drop always
			c.DropItem( new AxesOfFury() );
			c.DropItem( new StandardOfChaos() );

			double random = Utility.RandomDouble();

			Item artifact = null;

			if ( 0.05 > random ) // 5% of getting a unique artifact
			{
				artifact = CreateArtifact( UniqueArtifacts );
			}
			else if ( 0.20 > random ) // 15% of getting a shared artifact
			{
				artifact = CreateArtifact( SharedArtifacts );
			}
			else if ( 0.50 > random ) // 30% of getting a cursed doom artifact
			{
				artifact = CreateArtifact( Engines.Doom.DoomArtifactGiver.Artifacts );

				if ( artifact != null )
					artifact.LootType = LootType.Cursed;
			}

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

		public override bool CanFlee { get { return false; } }

		public SlasherOfVeils( Serial serial )
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
