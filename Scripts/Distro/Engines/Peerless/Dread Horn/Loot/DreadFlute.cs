using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class DreadFlute : BaseInstrument
	{
		public override int LabelNumber { get { return 1075089; } } // Dread Flute

		public override int InitMinUses { get { return 700; } }
		public override int InitMaxUses { get { return 700; } }

		[Constructable]
		public DreadFlute()
			: base( 0x315D, 0x482, 0x485 )
		{
			ItemID = 0x315D;
			Hue = 0x1F9;
			Weight = 1.0;

			BeginTimer();
		}

		public void BeginTimer()
		{
			ReplenishTimer timer = new ReplenishTimer( this );

			timer.Start();
		}

		public DreadFlute( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1070928 ); // Replenish Charges
		}

		public override void PlayInstrumentWell( Mobile from )
		{
			from.PlaySound( 0x58B );
		}

		public override void PlayInstrumentBadly( Mobile from )
		{
			from.PlaySound( 0x58C );
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

			BeginTimer();
		}

		private class ReplenishTimer : Timer
		{
			private DreadFlute m_flute;

			public ReplenishTimer( DreadFlute flute )
				: base( TimeSpan.FromMinutes( 5.0 ) )
			{
				m_flute = flute;
			}

			protected override void OnTick()
			{
				if ( m_flute.UsesRemaining < 700 )
					m_flute.UsesRemaining++;

				m_flute.BeginTimer();

				Stop();
			}
		}
	}
}