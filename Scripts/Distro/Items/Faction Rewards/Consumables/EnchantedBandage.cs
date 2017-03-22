using System;
using System.Collections.Generic;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class EnchantedBandage : Bandage
	{
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 15.0 );

		public override int LabelNumber { get { return 1094712; } } // Enchanted Bandage

		[Constructable]
		public EnchantedBandage()
			: this( 10 )
		{
		}

		[Constructable]
		public EnchantedBandage( int amount )
			: base( amount )
		{
			Hue = 1174;
		}

		public EnchantedBandage( Serial serial )
			: base( serial )
		{
		}

		public override bool Dye( Mobile from, IDyeTub sender )
		{
			return false;
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

			if ( faction == null )
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
				base.OnDoubleClick( from );
			}
		}

		public override void OnHealStarted( BandageContext context )
		{
			Mobile healer = context.Healer;

			if ( m_CooldownTable.ContainsKey( healer ) )
				m_CooldownTable[healer].Stop();

			m_CooldownTable[healer] = Timer.DelayCall( Cooldown + context.Delay, new TimerCallback( delegate { m_CooldownTable.Remove( healer ); } ) );
		}

		public override void OnHealFinished( BandageContext context )
		{
			Mobile healer = context.Healer, patient = context.Patient;

			if ( !healer.Alive || !patient.Alive )
				return;

			if ( !patient.Player )
				return;

			if ( !healer.InRange( patient, Bandage.Range ) )
				return;

			EnchantedApple.RemoveCurses( patient );

			// Any curses on you have been lifted
			patient.SendLocalizedMessage( 1072408 );
		}

		public override bool AttemptsMidlifeCure { get { return false; } }

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