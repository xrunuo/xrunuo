using System;
using Server.Targeting;

namespace Server.Items
{
	public class ValentineCard : Item
	{
		private static readonly int[] m_Names = new int[]
		{
			1077589, // To my one true love, ~1_target_player~. Signed: ~2_player~
			1077590, // You’ve pwnd my heart, ~1_target_player~. Signed: ~2_player~
			1077591, // Happy Valentine’s Day, ~1_target_player~. Signed: ~2_player~
			1077592, // Blackrock has driven me crazy... for ~1_target_player~! Signed: ~2_player~
			1077593, // You light my Candle of Love, ~1_target_player~! Signed: ~2_player~
		};

		private static readonly string m_EmptyField = "___";

		private int m_TitleCliloc;
		private string m_SenderName, m_ReceiverName;

		[CommandProperty( AccessLevel.GameMaster )]
		public int TitleCliloc
		{
			get { return m_TitleCliloc; }
			set { m_TitleCliloc = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string SenderName
		{
			get { return m_SenderName; }
			set { m_SenderName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string ReceiverName
		{
			get { return m_ReceiverName; }
			set { m_ReceiverName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Filled
		{
			get { return m_SenderName != null; }
		}

		[Constructable]
		public ValentineCard()
			: base( Utility.RandomList( 3773, 3774 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			if ( 0.01 > Utility.RandomDouble() )
				Hue = 1153;
			else
				Hue = Utility.RandomList( 232, 635, 634, 633 );

			m_TitleCliloc = m_Names[Utility.Random( m_Names.Length )];
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( Filled )
				return;

			if ( !IsChildOf( from.Backpack ) )
			{
				// That must be in your backpack for you to use it.
				from.SendLocalizedMessage( 1116249 );
			}
			else
			{
				// To whom do you wish to give this card?
				from.SendLocalizedMessage( 1077497 );

				from.BeginTarget( -1, false, TargetFlags.None, ( _, o ) =>
					{
						Mobile targeted = o as Mobile;

						if ( !IsChildOf( from.Backpack ) )
						{
							// That must be in your backpack for you to use it.
							from.SendLocalizedMessage( 1116249 );
						}
						else if ( targeted == null || !targeted.IsPlayer )
						{
							// That's not another player!
							from.SendLocalizedMessage( 1077488 );
						}
						else if ( from == targeted )
						{
							// You can’t give yourself a card, silly!
							from.SendLocalizedMessage( 1077495 );
						}
						else
						{
							m_SenderName = from.Name;
							m_ReceiverName = targeted.Name;

							InvalidateProperties();

							// You fill out the card. Hopefully the other person actually likes you...
							from.SendLocalizedMessage( 1077498 );
						}
					} );
			}
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( m_TitleCliloc, string.Format( "{0}\t{1}", m_ReceiverName ?? m_EmptyField, m_SenderName ?? m_EmptyField ) );
		}

		public ValentineCard( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_TitleCliloc );
			writer.Write( (string) m_SenderName );
			writer.Write( (string) m_ReceiverName );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_TitleCliloc = reader.ReadInt();
			m_SenderName = reader.ReadString();
			m_ReceiverName = reader.ReadString();
		}
	}
}
