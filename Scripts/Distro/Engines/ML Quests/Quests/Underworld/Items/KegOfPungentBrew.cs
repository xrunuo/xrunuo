using System;
using Server;

namespace Server.Items
{
	public class KegOfPungentBrew : Item
	{
		public override int LabelNumber { get { return 1113608; } } // a keg of Flint's Pungent Brew

		private int m_Held;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Held
		{
			get { return m_Held; }
			set
			{
				m_Held = value;
				UpdateWeight();
			}
		}

		[Constructable]
		public KegOfPungentBrew()
			: base( 0x1940 )
		{
			Hue = 0x60A;

			m_Held = 20;
			UpdateWeight();
		}

		public KegOfPungentBrew( Serial serial )
			: base( serial )
		{
		}

		public void UpdateWeight()
		{
			Weight = 20 + m_Held;
		}

		public override void OnDoubleClick( Mobile from )
		{
			BottleOfPungentBrew bottle;

			if ( m_Held <= 0 )
			{
				// A foggy memory is recalled and you have to ask yourself, "Why is the Pungent Brew always gone?"
				from.PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 1113610, from.Client );
			}
			else if ( ( bottle = from.Backpack.FindItemByType<BottleOfPungentBrew>() ) == null )
			{
				// Where is your special bottle for Flint's brew?
				from.SendLocalizedMessage( 1113619 );
			}
			else if ( bottle.Empty )
			{
				// You refill the special bottle with Flint's Pungent Brew.  Boy, it's lucky you kept that bottle!
				from.PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 1113609, from.Client );

				bottle.Empty = false;

				Held--;
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int number;

			if ( m_Held <= 0 )
				number = 502246; // The keg is empty.
			else if ( m_Held < 2 )
				number = 502248; // The keg is nearly empty.
			else if ( m_Held < 4 )
				number = 502249; // The keg is not very full.
			else if ( m_Held < 6 )
				number = 502250; // The keg is about one quarter full.
			else if ( m_Held < 8 )
				number = 502251; // The keg is about one third full.
			else if ( m_Held < 10 )
				number = 502252; // The keg is almost half full.
			else if ( m_Held < 12 )
				number = 502254; // The keg is approximately half full.
			else if ( m_Held < 14 )
				number = 502253; // The keg is more than half full.
			else if ( m_Held < 16 )
				number = 502255; // The keg is about three quarters full.
			else if ( m_Held < 18 )
				number = 502256; // The keg is very full.
			else if ( m_Held < 20 )
				number = 502257; // The liquid is almost to the top of the keg.
			else
				number = 502258; // The keg is completely full.

			list.Add( number );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (int) m_Held );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Held = reader.ReadInt();
		}
	}
}