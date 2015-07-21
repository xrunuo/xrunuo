using System;
using System.Collections.Generic;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class GreaterStaminaPotion : Item
	{
		public override int LabelNumber { get { return 1094764; } } // Greater Stamina Potion

		[Constructable]
		public GreaterStaminaPotion()
			: this( 1 )
		{
		}

		[Constructable]
		public GreaterStaminaPotion( int amount )
			: base( 0xF09 )
		{
			Stackable = true;
			Weight = 2.0;
			Hue = 0x1B5;
			Amount = amount;
		}

		public GreaterStaminaPotion( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
		}

		private static Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();

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
			else if ( m_Table.ContainsKey( from ) )
			{
				// You are already under a similar effect.
				from.SendLocalizedMessage( 502173 );
			}
			else
			{
				from.SendLocalizedMessage( 1094713 ); // You are starting to feel refreshed.
				from.PlaySound( 0x2D6 );

				Delete();

				Timer t = m_Table[from] = new InternalTimer( from );
				t.Start();
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

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private int m_Count;

			public InternalTimer( Mobile owner )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;

				m_Count = 5;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Alive && m_Count > 0 )
				{
					m_Owner.Stam += 20;
					m_Count--;
				}
				else
				{
					m_Owner.SendLocalizedMessage( 1094714 ); // The effects of the potion seem to have faded.
					Stop();
					m_Table.Remove( m_Owner );
				}
			}
		}
	}
}