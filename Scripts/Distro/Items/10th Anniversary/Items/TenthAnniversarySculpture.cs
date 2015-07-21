using System;
using System.Collections.Generic;
using Server.Engines.Housing;
using Server.Engines.Housing.Multis;
using Server.Gumps;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Items
{
	[Flipable( 0x3BB3, 0x3BB4 )]
	public class TenthAnniversarySculpture : Item, ISecurable
	{
		public override int LabelNumber { get { return 1079532; } } // 10th Anniversary Sculpture

		private SecureLevel m_Level;

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level { get { return m_Level; } set { m_Level = value; } }

		[Constructable]
		public TenthAnniversarySculpture()
			: this( Utility.RandomBool() )
		{
		}

		[Constructable]
		public TenthAnniversarySculpture( bool east )
			: base( east ? 0x3BB3 : 0x3BB4 )
		{
			Weight = 1.0;
		}

		private static HashSet<Mobile> m_Bonuses = new HashSet<Mobile>();

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			SetSecureLevelEntry.AddTo( from, this, list );
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			if ( m_Bonuses.Contains( pm ) )
			{
				// You're still feeling lucky from the last time you touched the sculpture.
				pm.SendLocalizedMessage( 1079534 );
			}
			else if ( pm.NextTenthAnniversarySculptureUse > DateTime.Now )
			{
				TimeSpan delta = pm.NextTenthAnniversarySculptureUse - DateTime.Now;

				if ( delta < TimeSpan.FromHours( 1.0 ) )
				{
					// You can improve your fortunes in about ~1_TIME~ minutes.
					pm.SendLocalizedMessage( 1079548, ( (int) delta.TotalMinutes ).ToString() );
				}
				else
				{
					// You can improve your fortunes again in about ~1_TIME~ hours.
					pm.SendLocalizedMessage( 1079550, ( (int) delta.TotalHours ).ToString() );
				}
			}
			else
			{
				int luckBonus = ComputeLuckBonus( pm );

				AttributeMod luckMod = new AttributeMod( MagicalAttribute.Luck, luckBonus );
				pm.AddAttributeMod( luckMod );

				m_Bonuses.Add( pm );
				pm.NextTenthAnniversarySculptureUse = DateTime.Now + TimeSpan.FromDays( 1.0 );

				pm.SendLocalizedMessage( 1079551 ); // Your luck just improved!

				Timer.DelayCall( TimeSpan.FromHours( 1.0 ),
					delegate
					{
						pm.RemoveAttributeMod( luckMod );
						m_Bonuses.Remove( pm );

						pm.SendLocalizedMessage( 1079552 ); // Your luck just ran out.
					} );
			}
		}

		private static int ComputeLuckBonus( Mobile m )
		{
			int bonus = 200;

			Account acct = m.Account as Account;

			if ( acct != null )
			{
				int years = (int) ( acct.Age.TotalDays / 365.0 );
				bonus += ( 50 * years );
			}

			return bonus;
		}

		public TenthAnniversarySculpture( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.WriteEncodedInt( (int) m_Level );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Level = (SecureLevel) reader.ReadEncodedInt();
						break;
					}
			}
		}
	}
}