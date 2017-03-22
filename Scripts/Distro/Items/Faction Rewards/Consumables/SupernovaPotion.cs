using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Network;
using Server.Factions;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
	public class SupernovaPotion : Item
	{
		public static readonly TimeSpan Cooldown = TimeSpan.FromMinutes( 2.0 );

		public override int LabelNumber { get { return 1094718; } } // Supernova Potion

		[Constructable]
		public SupernovaPotion()
			: this( 1 )
		{
		}

		[Constructable]
		public SupernovaPotion( int amount )
			: base( 0xF09 )
		{
			Stackable = true;
			Weight = 2.0;
			Hue = 13;
			Amount = amount;
		}

		public SupernovaPotion( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
		}

		private static Dictionary<Mobile, Timer> m_CooldownTable = new Dictionary<Mobile, Timer>();

		public override void OnDoubleClick( Mobile from )
		{
			Faction faction = Faction.Find( from );

			if ( !IsChildOf( from.Backpack ) )
			{
				// That is not in your backpack.
				from.SendLocalizedMessage( 1042593 );
			}
			else if ( faction == null )
			{
				// You may not use this unless you are a faction member!
				from.SendLocalizedMessage( 1010376, null, 0x25 );
			}
			else if ( m_CooldownTable.ContainsKey( from ) )
			{
				Timer cooldownTimer = m_CooldownTable[from];

				// You must wait ~1_seconds~ seconds before you can use this item.
				from.SendLocalizedMessage( 1079263, ( cooldownTimer.Next - DateTime.UtcNow ).Seconds.ToString() );
			}
			else
			{
				for ( int x = -5; x <= 5; x++ )
				{
					for ( int y = -5; y <= 5; y++ )
					{
						Point3D p = new Point3D( from.Location.X + x, from.Location.Y + y, from.Location.Z );
						int dist = (int) Utility.GetDistanceToSqrt( from.Location, p );

						if ( dist <= 5 )
						{
							Timer.DelayCall( TimeSpan.FromSeconds( 0.2 * dist ), new TimerCallback(
								delegate
								{
									Effects.SendPacket( from, from.Map, new HuedEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x3709, p, p, 20, 30, true, false, 1502, 4 ) );
								}
							) );
						}
					}
				}

				double alchemy = from.Skills[SkillName.Alchemy].Value;

				int damage = (int) BasePotion.Scale( from, 19 + alchemy / 5 );

				foreach ( Mobile to in from.GetMobilesInRange( 5 ).ToArray() )
				{
					int distance = (int) from.GetDistanceToSqrt( to );

					if ( to != from && distance <= 5 && from.CanSee( to ) && from.InLOS( to ) && SpellHelper.ValidIndirectTarget( from, to ) && from.CanBeHarmful( to ) && !to.Hidden )
						AOS.Damage( to, from, damage - distance, 0, 100, 0, 0, 0 );
				}

				Consume();

				m_CooldownTable[from] = Timer.DelayCall( Cooldown, new TimerCallback( delegate { m_CooldownTable.Remove( from ); } ) );
			}
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