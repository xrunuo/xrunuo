using System;
using Server;
using Server.Engines.Plants;
using Server.Targeting;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class PoppiesDust : Item, IUsesRemaining
	{
		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}
		public bool ShowUsesRemaining
		{
			get { return true; }
			set
			{
			}
		}
		[Constructable]
		public PoppiesDust()
			: this( 8 )
		{
		}

		[Constructable]
		public PoppiesDust( int charges )
			: base( 0xD2F )
		{
			Weight = 1.0;
			Stackable = false;
			Name = "Poppies Dust";
			Hue = 0x841;
			UsesRemaining = charges;

		}

		public PoppiesDust( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_UsesRemaining = reader.ReadInt();
						break;
					}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042664 ); // You must have the object in your backpack to use it.
				return;
			}

			from.Target = new InternalTarget( this );
			from.SendLocalizedMessage( 500343 ); // What do you wish to appraise and identify?
		}

		private class InternalTarget : Target
		{
			private PoppiesDust m_Dust;

			public InternalTarget( PoppiesDust dust )
				: base( 3, false, TargetFlags.None )
			{
				m_Dust = dust;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Dust.Deleted || m_Dust.UsesRemaining <= 0 )
				{
					from.SendMessage( 32, "Your Poppies Dust do not have any charges left." );
					return;
				}
				if ( targeted is Seed )
				{
					Seed m_Seed = (Seed) targeted;
					m_Seed.ShowType = true;

					from.SendLocalizedMessage( 1049084 ); // You successfully use the powder on the item.

					--m_Dust.UsesRemaining;

					if ( m_Dust.UsesRemaining <= 0 )
					{
						from.SendLocalizedMessage( 1049086 ); // You have used up your powder of temperament.
						//m_Dust.Delete();
					}
				}

				else
				{
					from.SendMessage( 32, "You must use this on a seed!." );
				}
			}
		}
	}
}
