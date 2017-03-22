using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Mobiles;
using Server.Spells.Mysticism;
using Server.Engines.Imbuing;

namespace Server.Items
{
	public abstract class BaseRanged : BaseMeleeWeapon
	{
		public abstract int EffectID { get; }
		public abstract Type AmmoType { get; }
		public abstract Item Ammo { get; }

		public override int HitSound { get { return 0x234; } }
		public override int MissSound { get { return 0x238; } }

		public override SkillName Skill { get { return SkillName.Archery; } }
		public override WeaponType Type { get { return WeaponType.Ranged; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.ShootXBow; } }

		private static bool m_IsFalseArrow; // True = Arrow lanzada con Lower Ammo Cost

		public BaseRanged( int itemID )
			: base( itemID )
		{
		}

		public BaseRanged( Serial serial )
			: base( serial )
		{
		}

		public override int GetHitAttackSound( Mobile attacker, Mobile defender )
		{
			return HitSound;
		}

		public override int GetMissAttackSound( Mobile attacker, Mobile defender )
		{
			return MissSound;
		}

		public override TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			// Make sure we've been standing still for 0.5 seconds
			if ( DateTime.UtcNow > ( attacker.LastMoveTime + TimeSpan.FromSeconds( 0.5 ) ) || ( WeaponAbility.GetCurrentAbility( attacker ) is MovingShot ) || !attacker.Player )
			{
				if ( CanSwing( attacker ) && attacker.HarmfulCheck( defender ) )
				{
					attacker.DisruptiveAction();
					attacker.Send( new Swing( 0, attacker, defender ) );
					
					if ( OnFired( attacker, defender ) )
					{
						if ( CheckHit( attacker, defender ) )
							OnHit( attacker, defender );
						else
							OnMiss( attacker, defender );
					}
				}

				return GetDelay( attacker );
			}
			else
			{
				return TimeSpan.FromSeconds( 0.5 + ( 0.25 * Utility.RandomDouble() ) );
			}
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			if ( attacker.Player && !defender.Player && ( defender.Body.IsAnimal || defender.Body.IsMonster ) && 0.4 >= Utility.RandomDouble() )
			{
				defender.AddToBackpack( Ammo );
			}

			base.OnHit( attacker, defender, damageBonus );
		}

		public class RecoveryTimer : Timer
		{
			private Mobile a;
			private Item Ammo;

			public void RecoveryAmmo( Mobile attacker, Item ammo )
			{
				int number = 0;

				if ( ammo is Arrow )
					number = 1023903;
				else if ( ammo is Bolt )
					number = 1027163;
				else
					return;

				string arguments = String.Format( "{0}	#{1}", ammo.Amount, number );

				bool success = false;

				Item cloak = attacker.FindItemOnLayer( Layer.Cloak );

				if ( cloak is BaseQuiver )
				{
					BaseQuiver quiver = cloak as BaseQuiver;

					if ( quiver.Ammo != null )
					{
						if ( quiver.Ammo.GetType() == ammo.GetType() && ( ( quiver.Ammo.Amount + ammo.Amount ) <= quiver.MaxAmmo ) )
						{
							quiver.Ammo.Amount += ammo.Amount;
							quiver.InvalidateProperties();

							ammo.Delete();

							success = true;
						}
					}
				}

				if ( !ammo.Deleted && attacker.AddToBackpack( ammo ) )
					success = true;

				if ( success )
					attacker.SendLocalizedMessage( 1073504, arguments ); // You recover ~1_NUM~ ~2_AMMO~.
				else
					attacker.SendLocalizedMessage( 1073559, arguments ); // You attempt to recover ~1_NUM~ ~2_AMMO~, but there is no room in your backpack, and they are lost.
			}

			public RecoveryTimer( Mobile attacker, Item ammo )
				: base( TimeSpan.FromSeconds( 5.0 ) )
			{
				if ( m_IsFalseArrow )
				{
					Stop();
					return;
				}

				a = attacker;

				Ammo = ammo;

				}

			protected override void OnTick()
			{
				RecoveryAmmo( a, Ammo );

				Stop();
			}
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			if ( attacker.Player/* && 0.4 >= Utility.RandomDouble()*/ )
			{
				RecoveryTimer timer = new RecoveryTimer( attacker, Ammo );

				timer.Start();
			}

			base.OnMiss( attacker, defender );
		}

		public virtual bool OnFired( Mobile attacker, Mobile defender )
		{
			Container pack = attacker.Backpack;

			if ( Attributes.LowerAmmoCost < Utility.RandomMinMax( 1, 100 ) && attacker.Player )
			{
				m_IsFalseArrow = false;

				bool success = false;

				Item cloak = attacker.FindItemOnLayer( Layer.Cloak );

				if ( cloak is BaseQuiver )
				{
					BaseQuiver quiver = cloak as BaseQuiver;

					if ( quiver.ConsumeTotal( AmmoType, 1 ) )
					{
						success = true;
						quiver.InvalidateProperties();
					}
				}

				if ( !success )
				{
					if ( pack != null && pack.ConsumeTotal( AmmoType, 1 ) )
					{
						success = true;
					}
				}

				if ( !success )
					return false;
			}
			else
			{
				m_IsFalseArrow = true;
			}

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

		public override ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Ranged; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				case 1:
					{
						break;
					}
				case 0:
					{
						/*m_EffectID =*/
						reader.ReadInt();
						break;
					}
			}

			if ( version < 2 )
			{
				WeaponAttributes.MageWeapon = 0;
				WeaponAttributes.UseBestSkill = 0;
			}
		}
	}
}
