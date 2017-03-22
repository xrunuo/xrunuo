using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class VialOfArmorEssence : Item, IPetBooster
	{
		public override int LabelNumber { get { return 1113018; } } // Vial of Armor Essence

		public TimeSpan Duration { get { return TimeSpan.FromMinutes( 10.0 ); } }
		public TimeSpan Cooldown { get { return TimeSpan.FromHours( 1.0 ); } }

		[Constructable]
		public VialOfArmorEssence()
			: base( 0xE24 )
		{
			Weight = 1.0;
			Hue = 2405;
		}

		public VialOfArmorEssence( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1113213 ); // * For Pets Only *

			list.Add( 1113219 ); // Damage Absorption 10%
			list.Add( 1113212, Duration.TotalMinutes.ToString() ); // Duration: ~1_val~ minutes
			list.Add( 1113218, Cooldown.TotalMinutes.ToString() ); // Cooldown: ~1_val~ minutes
		}

		public bool OnUsed( Mobile from, BaseCreature pet )
		{
			if ( m_UnderEffect.Contains( pet ) )
			{
				from.SendLocalizedMessage( 1113075 ); // Your pet is still under the effect of armor essence.
				return false;
			}
			else if ( DateTime.UtcNow < pet.NextArmorEssence )
			{
				from.SendLocalizedMessage( 1113076 ); // Your pet is still recovering from the last armor essence it consumed.
				return false;
			}
			else
			{
				pet.SayTo( from, 1113050 ); // Your pet looks much happier.

				pet.FixedEffect( 0x375A, 10, 15 );
				pet.PlaySound( 0x1E7 );

				List<ResistanceMod> mods = new List<ResistanceMod>();

				mods.Add( new ResistanceMod( ResistanceType.Physical, 15 ) );
				mods.Add( new ResistanceMod( ResistanceType.Fire, 10 ) );
				mods.Add( new ResistanceMod( ResistanceType.Cold, 10 ) );
				mods.Add( new ResistanceMod( ResistanceType.Poison, 10 ) );
				mods.Add( new ResistanceMod( ResistanceType.Energy, 10 ) );

				for ( int i = 0; i < mods.Count; i++ )
					pet.AddResistanceMod( mods[i] );

				m_UnderEffect.Add( pet );

				Timer.DelayCall( Duration, new TimerCallback(
					delegate
					{
						for ( int i = 0; i < mods.Count; i++ )
							pet.RemoveResistanceMod( mods[i] );

						m_UnderEffect.Remove( pet );
					} ) );

				pet.NextArmorEssence = DateTime.UtcNow + Duration + Cooldown;

				Delete();

				return true;
			}
		}

		private static ISet<Mobile> m_UnderEffect = new HashSet<Mobile>();

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