using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class KeyRing : Item
	{
		private ArrayList m_Keys;

		public ArrayList Keys { get { return m_Keys; } }

		[Constructable]
		public KeyRing()
			: base( 0x1011 )
		{
			m_Keys = new ArrayList();
		}

		public KeyRing( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (int) m_Keys.Count );

			foreach ( KeyInfo key in m_Keys )
			{
				key.Serialize( writer );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			int KeyCount = reader.ReadInt();

			m_Keys = new ArrayList();

			for ( int i = 0; i < KeyCount; i++ )
			{
				m_Keys.Add( new KeyInfo( reader ) );
			}
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			base.OnDragDrop( from, item );

			if ( item is Key && ( (Key) item ).KeyValue != 0 )
			{
				if ( m_Keys.Count < 20 )
				{
					from.SendLocalizedMessage( 501691 ); // You put the key on the keyring.

					m_Keys.Add( new KeyInfo( (Key) item ) );

					item.Delete();

					if ( m_Keys.Count > 0 && m_Keys.Count < 3 )
					{
						ItemID = 0x1769;
					}
					else if ( m_Keys.Count > 2 && m_Keys.Count < 5 )
					{
						ItemID = 0x176A;
					}
					else if ( m_Keys.Count > 4 )
					{
						ItemID = 0x176B;
					}
				}
				else
				{
					from.SendLocalizedMessage( 1008138 ); // This keyring is full.
				}
			}
			else
			{
				from.SendLocalizedMessage( 501689 ); // Only non-blank keys can be put on a keyring.
			}

			return false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 501680 ); // What do you want to unlock?

			from.Target = new UnlockTarget( this );
		}

		private class UnlockTarget : Target
		{
			private KeyRing m_KeyRing;

			public UnlockTarget( KeyRing keyring )
				: base( 10, false, TargetFlags.None )
			{
				m_KeyRing = keyring;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted == m_KeyRing )
				{
					from.SendLocalizedMessage( 501685 ); // You open the keyring.

					m_KeyRing.ItemID = 0x1011;

					foreach ( KeyInfo k in m_KeyRing.Keys )
					{
						Key key = new Key( k.type, k.KeyValue, k.Link );

						key.Description = k.Description;

						from.AddToBackpack( key );
					}

					m_KeyRing.Keys.Clear();
				}
				else if ( targeted is Item )
				{
					Item item = (Item) targeted;

					ILockable locker = targeted as ILockable;

					if ( locker != null )
					{
						bool IsMatch = false;

						foreach ( KeyInfo key in m_KeyRing.Keys )
						{
							if ( key.KeyValue == locker.KeyValue )
							{
								IsMatch = true;

								break;
							}
						}

						if ( IsMatch )
						{
							if ( locker.Locked )
							{
								locker.Locked = false;

								item.SendLocalizedMessageTo( from, 1008137, item.Name ); // Unlocked :
							}
							else
							{
								locker.Locked = true;

								item.SendLocalizedMessageTo( from, 1008136, item.Name ); // Locked : 
							}
						}
						else
						{
							from.SendLocalizedMessage( 1008140 ); // You do not have a key for that.
						}
					}
					else
					{
						item.SendLocalizedMessageTo( from, 1008140 ); // You do not have a key for that.
					}
				}
				else if ( targeted is Mobile )
				{
					Mobile m = targeted as Mobile;

					m.Say( 1008140 ); // You do not have a key for that.
				}
			}
		}
	}

	public class KeyInfo
	{
		private string m_Description;
		private uint m_KeyVal;
		private Item m_Link;
		private int m_MaxRange;
		private KeyType m_Type;

		public string Description { get { return m_Description; } }
		public int MaxRange { get { return m_MaxRange; } }
		public uint KeyValue { get { return m_KeyVal; } }
		public Item Link { get { return m_Link; } }
		public KeyType type { get { return m_Type; } }

		public KeyInfo( Key key )
		{
			m_KeyVal = key.KeyValue;
			m_Description = key.Description;
			m_MaxRange = key.MaxRange;
			m_Link = key.Link;
			m_Type = (KeyType) key.ItemID;
		}

		public KeyInfo( GenericReader reader )
		{
			/*int version = */
			reader.ReadInt();

			m_KeyVal = reader.ReadUInt();
			m_Description = reader.ReadString();
			m_MaxRange = reader.ReadInt();
			m_Link = reader.ReadItem();
			m_Type = (KeyType) reader.ReadInt();
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); // version

			writer.Write( (uint) m_KeyVal );
			writer.Write( (string) m_Description );
			writer.Write( (int) m_MaxRange );
			writer.Write( (Item) m_Link );
			writer.Write( (int) m_Type );
		}
	}
}
