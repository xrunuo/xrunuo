using System;
using Server;
using Server.Factions;
using Server.Targeting;

namespace Server.Items
{
	public class PowderOfPerseverance : Item, IUsesRemaining
	{
		public override int LabelNumber { get { return 1094756; } } // Powder of Perseverance

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

		public bool ShowUsesRemaining { get { return true; } set { } }

		[Constructable]
		public PowderOfPerseverance()
			: base( 0x1006 )
		{
			Hue = 0x548;
			Weight = 1.0;
			m_UsesRemaining = 1;
		}

		public PowderOfPerseverance( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

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
			else
			{
				from.BeginTarget( 1, false, TargetFlags.None, new TargetCallback( Target_Callback ) );
			}
		}

		private void Target_Callback( Mobile from, object targeted )
		{
			if ( targeted is Item && !( (Item) targeted ).IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042593 ); // That is not in your backpack.
			else if ( !( targeted is IFactionArtifact ) )
				from.SendLocalizedMessage( 1049083 ); // You cannot use the powder on that item.
			else if ( !( targeted is IDurability ) )
				from.SendLocalizedMessage( 1049083 ); // You cannot use the powder on that item.
			else
			{
				IDurability item = targeted as IDurability;

				if ( item.HitPoints == item.MaxHitPoints )
					from.SendLocalizedMessage( 1094761 ); // This item is already in perfect condition.
				else
				{
					if ( item.MaxHitPoints > 225 )
						item.MaxHitPoints = item.HitPoints = 225;
					else if ( item.MaxHitPoints > 200 )
						item.MaxHitPoints = item.HitPoints = 200;
					else if ( item.MaxHitPoints > 175 )
						item.MaxHitPoints = item.HitPoints = 175;
					else if ( item.MaxHitPoints > 150 )
						item.MaxHitPoints = item.HitPoints = 150;
					else if ( item.MaxHitPoints > 125 )
						item.MaxHitPoints = item.HitPoints = 125;
					else
					{
						from.SendLocalizedMessage( 1049085 ); // The item cannot be improved any further.
						return;
					}

					from.SendLocalizedMessage( 1049084 ); // You successfully use the powder on the item.
					from.PlaySound( 0x247 );

					m_UsesRemaining--;

					if ( m_UsesRemaining == 0 )
					{
						from.SendLocalizedMessage( 1094760 ); // You have used up your Powder of Perseverance.
						Delete();
					}
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_UsesRemaining = reader.ReadInt();
		}
	}
}