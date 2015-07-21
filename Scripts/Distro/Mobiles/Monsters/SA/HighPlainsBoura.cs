using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a high plains boura's corpse" )]
	public class HighPlainsBoura : BaseCreature, ICarvable
	{
		private bool m_Shorn;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Shorn
		{
			get { return m_Shorn; }
			set { m_Shorn = value; }
		}

		[Constructable]
		public HighPlainsBoura()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6 )
		{
			Name = "a high plains boura";
			Body = 715;

			SetStr( 404, 509 );
			SetDex( 86, 100 );
			SetInt( 26, 30 );

			SetHits( 600, 664 );

			SetDamage( 20, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Anatomy, 95.0, 105.0 );
			SetSkill( SkillName.MagicResist, 65.0, 75.0 );
			SetSkill( SkillName.Tactics, 95.0, 105.0 );
			SetSkill( SkillName.Wrestling, 100.0, 110.0 );

			Fame = 5000;
			Karma = -5000;

			Tamable = true;
			MinTameSkill = 47.1;
			ControlSlots = 3;
		}

		public override int GetAngerSound() { return 0x5DF; }
		public override int GetIdleSound() { return 0x5E3; }
		public override int GetAttackSound() { return 0x5E0; }
		public override int GetHurtSound() { return 0x5E2; }
		public override int GetDeathSound() { return 0x5E1; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 15; } }

		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override int Blood { get { return 8; } }
		public override int Fur { get { return m_Shorn ? 0 : 30; } }
		public override FurType FurType { get { return FurType.Yellow; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Boura; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 > Utility.RandomDouble() )
			{
				defender.FixedParticles( 0x374A, 1, 17, 0x15B6, 0, 0, EffectLayer.Waist );
				defender.PlaySound( 0x233 );

				defender.Damage( 10, this );

				switch ( Utility.Random( 3 ) )
				{
					case 0:
						{
							defender.SendLocalizedMessage( 1112391 ); // The creature's tail smashes into you!

							break;
						}
					case 1:
						{
							defender.Freeze( TimeSpan.FromSeconds( 3.0 ) );

							defender.SendLocalizedMessage( 1112554 ); // You're stunned as the creature's tail knocks the wind out of you.

							break;
						}
					case 2:
						{
							defender.SendLocalizedMessage( 1112555 ); // You're left confused as the creature's tail catches you right in the face!
							defender.PublicOverheadMessage( MessageType.Regular, 946, 502039 ); // *looks confused*

							defender.AddStatMod( new StatMod( StatType.Dex, "Boura Dex Malus", -20, TimeSpan.FromSeconds( 5.0 ) ) );
							defender.AddStatMod( new StatMod( StatType.Int, "Boura Int Malus", -20, TimeSpan.FromSeconds( 5.0 ) ) );

							defender.FixedParticles( 0x37B9, 1, 10, 0x232A, 5, 0, EffectLayer.Head );
							defender.PlaySound( 0xF9 );

							break;
						}
				}
			}
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( !Controlled )
			{
				c.DropItem( new BouraPelt() );

				if ( 0.2 > Utility.RandomDouble() )
					c.DropItem( new BouraSkin() );

				if ( 0.01 > Utility.RandomDouble() )
					c.DropItem( new BouraTailShield() );
			}
		}

		public void Carve( Mobile from, Item item )
		{
			if ( m_Shorn )
			{
				// The boura glares at you and will not let you shear its fur.
				PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1112354, from.Client );
				return;
			}

			Item fur = new YellowFur( Fur );

			if ( from.PlaceInBackpack( fur ) )
			{
				from.SendLocalizedMessage( 1112353 ); // You place the gathered boura fur into your backpack.
			}
			else
			{
				from.SendLocalizedMessage( 1112352 ); // You would not be able to place the gathered boura fur in your backpack!
				fur.MoveToWorld( from.Location, from.Map );
			}

			m_Shorn = true;
		}

		public HighPlainsBoura( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (bool) m_Shorn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Shorn = reader.ReadBool();
						break;
					}
			}
		}
	}
}
