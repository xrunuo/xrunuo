using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public class BottleOfPungentBrew : Item
	{
		public override int LabelNumber { get { return m_Empty ? 1113607 : 1094967; } } // a bottle of Flint's Pungent Brew

		private bool m_Empty;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Empty
		{
			get { return m_Empty; }
			set { m_Empty = value; InvalidateProperties(); }
		}

		[Constructable]
		public BottleOfPungentBrew()
			: base( 0x99F )
		{
			Weight = 1.0;

			Hue = 0x60A;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( Empty )
			{
				// A foggy memory is recalled and you have to ask yourself, "Why is the Pungent Brew always gone?"
				from.PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 1113610, from.NetState );
			}
			else
			{
				from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );

				from.BAC = 60;
				BaseBeverage.CheckHeaveTimer( from );

				if ( !m_Table.ContainsKey( from ) )
				{
					from.SendLocalizedMessage( 1095139 ); // Flint wasn't kidding. This brew is strong!

					InternalTimer t = m_Table[from] = new InternalTimer( from );
					t.Start();
				}
				else
				{
					InternalTimer t = m_Table[from];
					t.Counter = 20;
				}

				Empty = true;
			}
		}

		public BottleOfPungentBrew( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (bool) m_Empty );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Empty = reader.ReadBool();
		}

		private static Dictionary<Mobile, InternalTimer> m_Table = new Dictionary<Mobile, InternalTimer>();

		public static bool UnderEffect( Mobile from )
		{
			return m_Table.ContainsKey( from );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private int m_Counter;

			public int Counter
			{
				get { return m_Counter; }
				set { m_Counter = value; }
			}

			public InternalTimer( Mobile owner )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 15.0 ) )
			{
				m_Owner = owner;
				m_Counter = 20;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.Alive )
				{
					m_Table.Remove( m_Owner );
					Stop();
				}

				if ( m_Counter == 0 )
				{
					m_Owner.SendLocalizedMessage( 1095142 ); // You no longer stink from Flint's Pungent Brew.

					m_Table.Remove( m_Owner );
					Stop();
				}
				else
				{
					Effects.SendLocationParticles( m_Owner, 0x36B0, 1, 10, 0xA6, 0, 0x1F78, 0 );

					// TODO (SA): Chance de emborrachar a los que están a tu alrededor.

					--m_Counter;
				}
			}
		}
	}
}