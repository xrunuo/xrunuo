using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseMaskOfDeathPotion : BasePotion
	{
		public abstract TimeSpan Duration { get; }

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m.SendLocalizedMessage( 503325 ); // You are no longer ignored by monsters.

			m_Table.Remove( m );
		}

		public BaseMaskOfDeathPotion( PotionEffect effect )
			: base( 0xF06, effect )
		{
			Hue = 0x482;
		}

		public BaseMaskOfDeathPotion( Serial serial )
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

		public override TimeSpan GetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				return ( (PlayerMobile) from ).NextDrinkMaskOfDeathPotion;

			return TimeSpan.Zero;
		}

		public override void SetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				( (PlayerMobile) from ).NextDrinkMaskOfDeathPotion = TimeSpan.FromHours( 1.0 );
		}

		public override void Drink( Mobile from )
		{
			if ( !UnderEffect( from ) )
			{
				Timer t = (Timer) m_Table[from];

				if ( t != null )
					t.Stop();

				m_Table[from] = t = Timer.DelayCall( Duration, new TimerStateCallback( Expire_Callback ), from );

				Effects.SendPacket( from, from.Map, new GraphicalEffect( EffectType.FixedFrom, from.Serial, Serial.Zero, 0x375A, from.Location, from.Location, 10, 15, true, false ) );

				from.SendLocalizedMessage( 503326 ); // You are now ignored by monsters.

				Consume();

				SetNextDrinkTime( from );
			}
		}
	}
}