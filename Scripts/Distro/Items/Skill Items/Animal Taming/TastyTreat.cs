using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public interface IPetBooster
	{
		bool OnUsed( Mobile from, BaseCreature pet );
	}

	public abstract class BaseTastyTreat : Item, IPetBooster
	{
		public abstract TimeSpan Duration { get; }
		public abstract TimeSpan Cooldown { get; }
		public abstract double StatBonus { get; }

		public BaseTastyTreat()
			: base( 0x978 )
		{
			Stackable = true;
			Weight = 1.0;
		}

		public BaseTastyTreat( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1113213 ); // * For Pets Only *

			AddBonusProperty( list );

			list.Add( 1113212, Duration.TotalMinutes.ToString() ); // Duration: ~1_val~ minutes
			list.Add( 1113218, Cooldown.TotalMinutes.ToString() ); // Cooldown: ~1_val~ minutes
		}

		public abstract void AddBonusProperty( ObjectPropertyList list );

		public bool OnUsed( Mobile from, BaseCreature pet )
		{
			if ( pet.GetStatMod( "[Tasty Treat] Str" ) != null )
			{
				from.SendLocalizedMessage( 1113051 ); // Your pet is still enjoying the last tasty treat!
				return false;
			}
			else if ( DateTime.UtcNow < pet.NextTastyTreat )
			{
				from.SendLocalizedMessage( 1113049 ); // Your pet is still recovering from the last tasty treat.
				return false;
			}
			else
			{
				pet.SayTo( from, 1113050 ); // Your pet looks much happier.

				pet.FixedEffect( 0x375A, 10, 15 );
				pet.PlaySound( 0x1E7 );

				AddEffect( pet );

				pet.NextTastyTreat = DateTime.UtcNow + Duration + Cooldown;

				if ( this.Amount > 1 )
				{
					this.Amount -= 1;
					from.Backpack.DropItem( this );
				}
				else
				{
					Delete();
				}

				return true;
			}
		}

		public virtual void AddEffect( BaseCreature pet )
		{
			pet.AddStatMod( new StatMod( StatType.Str, "[Tasty Treat] Str", (int) ( pet.Str * StatBonus ), Duration ) );
			pet.AddStatMod( new StatMod( StatType.Dex, "[Tasty Treat] Dex", (int) ( pet.Dex * StatBonus ), Duration ) );
			pet.AddStatMod( new StatMod( StatType.Int, "[Tasty Treat] Int", (int) ( pet.Int * StatBonus ), Duration ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class TastyTreat : BaseTastyTreat
	{
		public override int LabelNumber { get { return 1113003; } } // Tasty Treat

		public override TimeSpan Duration { get { return TimeSpan.FromMinutes( 5.0 ); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromMinutes( 2.0 ); } }

		public override double StatBonus { get { return 0.05; } }

		[Constructable]
		public TastyTreat()
		{
		}

		public TastyTreat( Serial serial )
			: base( serial )
		{
		}

		public override void AddBonusProperty( ObjectPropertyList list )
		{
			list.Add( 1113214 ); // Stats Increased by 5%
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class DeliciouslyTastyTreat : BaseTastyTreat
	{
		public override int LabelNumber { get { return 1113004; } } // Deliciously Tasty Treat

		public override TimeSpan Duration { get { return TimeSpan.FromMinutes( 10.0 ); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromMinutes( 60.0 ); } }

		public override double StatBonus { get { return 0.10; } }

		[Constructable]
		public DeliciouslyTastyTreat()
		{
		}

		public DeliciouslyTastyTreat( Serial serial )
			: base( serial )
		{
		}

		public override void AddBonusProperty( ObjectPropertyList list )
		{
			list.Add( 1113215 ); // Stats Increased by 10%
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class IrresistiblyTastyTreat : BaseTastyTreat
	{
		public override int LabelNumber { get { return 1113005; } } // Irresistibly Tasty Treat

		public override TimeSpan Duration { get { return TimeSpan.FromMinutes( 10.0 ); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromMinutes( 120.0 ); } }

		public override double StatBonus { get { return 0.15; } }

		[Constructable]
		public IrresistiblyTastyTreat()
		{
		}

		public IrresistiblyTastyTreat( Serial serial )
			: base( serial )
		{
		}

		public override void AddBonusProperty( ObjectPropertyList list )
		{
			list.Add( 1113216 ); // Stats Increased by 15%
			list.Add( 1113217 ); // Damage Increased by 10%
		}

		public override void AddEffect( BaseCreature pet )
		{
			base.AddEffect( pet );

			AttributeMod dmgMod = new AttributeMod( AosAttribute.WeaponDamage, 10 );

			pet.AddAttributeMod( dmgMod );

			Timer.DelayCall( Duration, new TimerCallback( delegate { pet.RemoveAttributeMod( dmgMod ); } ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
