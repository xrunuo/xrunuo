using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Sixth;

namespace Server.Items
{
	public class InvisibilityPotion : BasePotion
	{
		public override int LabelNumber { get { return 1072941; } } // Potion of Invisibility

		[Constructable]
		public InvisibilityPotion()
			: base( 0xF06, PotionEffect.Invisibility )
		{
			Hue = 0x48D;
		}

		public InvisibilityPotion( Serial serial )
			: base( serial )
		{
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

		public override void Drink( Mobile from )
		{
			if ( from.Hidden )
			{
				from.SendLocalizedMessage( 1073185 ); // You are already unseen.
				return;
			}

			PlayDrinkEffect( from );

			this.Consume();

			Point3D loc = from.Location;

			Timer.DelayCall( ComputeDelay( from ), () =>
				{
					if ( loc != from.Location )
					{
						from.SendLocalizedMessage( 1073187 ); // The invisibility effect is interrupted.
					}
					else
					{
						Effects.SendLocationParticles( EffectItem.Create( new Point3D( from.X, from.Y, from.Z + 16 ), from.Map, EffectItem.DefaultDuration ), 0x376A, 10, 15, 5045 );
						from.PlaySound( 0x3C4 );

						from.Hidden = true;
						from.Warmode = false;

						InvisibilitySpell.RemoveTimer( from );
						InvisibilitySpell.AddTimer( from, ComputeDuration( from ) );
					}
				} );
		}

		public virtual TimeSpan ComputeDelay( Mobile from )
		{
			return TimeSpan.FromSeconds( 2.0 );
		}

		public virtual TimeSpan ComputeDuration( Mobile from )
		{
			double alchemy = from.Skills[SkillName.Alchemy].Value;
			double seconds = BasePotion.Scale( from, 20 + ( alchemy / 10 ) );

			return TimeSpan.FromSeconds( seconds );
		}

		public override void PlayDrinkEffect( Mobile m )
		{
			m.PlaySound( 0x31 );
			m.AddToBackpack( new Bottle() );

			if ( m.Body.IsHuman )
				m.Animate( 34, 5, 1, true, false, 0 );
		}
	}
}