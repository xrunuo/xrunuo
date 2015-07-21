using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;

namespace Server.Items
{
	public class GrapesOfWrath : Food, ICommodity
	{
		public static readonly TimeSpan Duration = TimeSpan.FromSeconds( 20.0 );
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 120.0 );

		[Constructable]
		public GrapesOfWrath()
			: base( 0x2FD7 )
		{
			Weight = 1.0;
			Hue = 0x482;
		}

		public GrapesOfWrath( Serial serial )
			: base( serial )
		{
		}

		public override bool Eat( Mobile from )
		{
			if ( !CheckCooldown( from ) )
				return false;

			// The grapes of wrath invigorate you for a short time, allowing you to deal extra damage.
			from.SendLocalizedMessage( 1074847 );

			m_InfluenceList.Add( from );
			m_CooldownTable.Add( from, DateTime.Now + Cooldown );

			Timer.DelayCall( Duration, new TimerStateCallback( delegate { m_InfluenceList.Remove( from ); } ), from );

			// Play a random "eat" sound
			from.PlaySound( Utility.Random( 0x3A, 3 ) );

			from.Animate( 6 );

			if ( Poison != null )
				from.ApplyPoison( Poisoner, Poison );

			Consume();

			return true;
		}

		private static List<Mobile> m_InfluenceList = new List<Mobile>();
		private static Dictionary<Mobile, DateTime> m_CooldownTable = new Dictionary<Mobile, DateTime>();

		private static bool CheckCooldown( Mobile m )
		{
			if ( m_CooldownTable.ContainsKey( m ) )
			{
				DateTime cooldownEnd = m_CooldownTable[m];

				if ( cooldownEnd > DateTime.Now )
				{
					// You must wait ~1_seconds~ seconds before you can use this item.
					m.SendLocalizedMessage( 1079263, ( (int) ( cooldownEnd - DateTime.Now ).TotalSeconds + 1 ).ToString() );

					return false;
				}
				else
				{
					m_CooldownTable.Remove( m );
				}
			}

			return true;
		}

		public static bool IsUnderInfluence( Mobile mob )
		{
			return m_InfluenceList.Contains( mob );
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