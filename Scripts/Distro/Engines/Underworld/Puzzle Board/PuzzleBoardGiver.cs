using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class PuzzleBoardGiver : Container
	{
		[Constructable]
		public PuzzleBoardGiver()
			: base( 0xE80 )
		{
			Weight = 0.0;
			Movable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null )
			{
				if ( pm.Map != this.Map || !pm.InRange( GetWorldLocation(), 2 ) )
					pm.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				else if ( pm.NextPuzzleAttempt > DateTime.UtcNow )
					pm.SendLocalizedMessage( 1113386 ); // You are too tired to attempt solving more puzzles at this time.
				else if ( pm.Backpack.FindItemByType<PuzzleBoardItem>( false ) != null )
					pm.SendLocalizedMessage( 501885 ); // You already own one of those!
				else
				{
					PuzzleBoardItem puzzle = new PuzzleBoardItem();

					if ( pm.PlaceInBackpack( puzzle ) )
					{
						pm.SendLocalizedMessage( 1072223 ); // An item has been placed in your backpack.
						puzzle.SendTimeRemainingMessage( pm );
					}
					else
						puzzle.Delete();
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			from.SendLocalizedMessage( 1113513 ); // You cannot put items there.
			return false;
		}

		public override void DisplayTo( Mobile to )
		{
			to.SendLocalizedMessage( 1005213 ); // You can't do that
		}

		public PuzzleBoardGiver( Serial serial )
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
	}
}