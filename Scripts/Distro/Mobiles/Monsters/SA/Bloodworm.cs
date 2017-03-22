using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a bloodworm corpse" )]
	public class Bloodworm : BaseCreature
	{
		[Constructable]
		public Bloodworm()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "a bloodworm";
			Body = 287;

			SetStr( 369, 425 );
			SetDex( 80 );
			SetInt( 16, 20 );

			SetHits( 359, 396 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 65, 75 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 35.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 10000; // TODO: Verify
			Karma = -10000;
		}

		public override int GetAngerSound() { return 0x5DF; }
		public override int GetIdleSound() { return 0x5DF; }
		public override int GetAttackSound() { return 0x5DC; }
		public override int GetHurtSound() { return 0x5DE; }
		public override int GetDeathSound() { return 0x5DD; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( Utility.RandomBool() )
			{
				if ( BloodDisease.UnderEffect( defender ) )
				{
					// * The bloodworm is repulsed by your diseased blood. *
					defender.SendLocalizedMessage( 1111668, "", 0x25 );
				}
				else
				{
					// *The bloodworm drains some of your blood to replenish its health.*
					defender.SendLocalizedMessage( 1111698, "", 0x25 );

					Hits += ( defender.HitsMax - defender.Hits );
				}
			}

			if ( 0.1 > Utility.RandomDouble() && !IsAnemic( defender ) && !FontOfFortune.HasBlessing( defender, FontOfFortune.BlessingType.Protection ) )
			{
				defender.SendLocalizedMessage( 1111669 ); // The bloodworm's attack weakens you. You have become anemic.

				defender.AddStatMod( new StatMod( StatType.Str, "[Bloodworm] Str Malus", -40, TimeSpan.FromSeconds( 15.0 ) ) );
				defender.AddStatMod( new StatMod( StatType.Dex, "[Bloodworm] Dex Malus", -40, TimeSpan.FromSeconds( 15.0 ) ) );
				defender.AddStatMod( new StatMod( StatType.Int, "[Bloodworm] Int Malus", -40, TimeSpan.FromSeconds( 15.0 ) ) );

				Effects.SendPacket( defender, defender.Map, new GraphicalEffect( EffectType.FixedFrom, defender.Serial, Serial.Zero, 0x375A, defender.Location, defender.Location, 9, 20, true, false ) );
				Effects.SendTargetParticles( defender, 0x373A, 1, 15, 0x26B9, EffectLayer.Head );
				Effects.SendLocationParticles( defender, 0x11A6, 9, 32, 0x253A );

				defender.PlaySound( 0x1ED );

				Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback(
					delegate
					{
						// You recover from your anemia.
						defender.SendLocalizedMessage( 1111670 );

						defender.RemoveStatMod( "[Bloodworm] Str Malus" );
						defender.RemoveStatMod( "[Bloodworm] Dex Malus" );
						defender.RemoveStatMod( "[Bloodworm] Int Malus" );
					}
				) );
			}
		}

		public static bool IsAnemic( Mobile m )
		{
			return m.GetStatMod( "[Bloodworm] Str Malus" ) != null;
		}

		public override void OnAfterMove( Point3D oldLocation )
		{
			base.OnAfterMove( oldLocation );

			if ( Hits < HitsMax && 0.25 > Utility.RandomDouble() )
			{
				Corpse toAbsorb = null;

				foreach ( Item item in Map.GetItemsInRange( Location, 1 ) )
				{
					if ( item is Corpse )
					{
						Corpse c = (Corpse) item;

						if ( c.ItemID == 0x2006 )
						{
							toAbsorb = c;
							break;
						}
					}
				}

				if ( toAbsorb != null )
				{
					toAbsorb.ProcessDelta();
					toAbsorb.SendRemovePacket();
					toAbsorb.ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
					toAbsorb.Hue = 0;
					toAbsorb.Direction = Direction.North;
					toAbsorb.ProcessDelta();

					Hits = HitsMax;

					// * The bloodworm drains blood from a nearby corpse to heal itself. *
					PublicOverheadMessage( MessageType.Regular, 0x3B2, 1111699 );
				}
			}
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 15; } }

		public Bloodworm( Serial serial )
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
