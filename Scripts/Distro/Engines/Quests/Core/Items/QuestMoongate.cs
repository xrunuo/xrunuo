using System;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Engines.Quests
{
	public class QuestMoongate : Moongate
	{
		public override int LabelNumber { get { return 1048047; } } // a Moongate

		[Constructable]
		public QuestMoongate()
		{
			Dispellable = false;

			TargetMap = Map.Tokuno;

			Target = new Point3D( 770, 1209, 25 );
		}

		public QuestMoongate( Serial serial )
			: base( serial )
		{
		}

		public override bool ValidateUse( Mobile from, bool message )
		{
			if ( !base.ValidateUse( from, message ) )
			{
				return false;
			}

			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
			{
				return false;
			}

			QuestSystem qs = pm.Quest;

			if ( qs != null )
			{
				pm.CloseGump( typeof( QuestCancelGump ) );

				pm.SendGump( new QuestCancelGump( qs ) );

				return false;
			}

			return true;
		}

		public override bool OnMoveOver( Mobile m )
		{
			base.OnMoveOver( m );

			if ( !ValidateUse( m, false ) )
			{
				return false;
			}

			return true;
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
