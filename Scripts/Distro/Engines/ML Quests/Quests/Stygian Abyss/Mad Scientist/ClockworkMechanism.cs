using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Quests.MadScientist;

namespace Server.Items
{
	public class ClockworkMechanism : TransientItem
	{
		public override int QuestItemHue { get { return 0x1EAE; } }
		public override bool NonTransferable { get { return true; } }

		private int m_CreatureDef;

		public ClockworkCreatureDef CreatureDef
		{
			get { return ClockworkCreature.Definitions[m_CreatureDef]; }
		}

		[Constructable]
		public ClockworkMechanism()
			: base( 0x1EAE, TimeSpan.FromHours( 1.0 ) )
		{
			m_CreatureDef = Utility.Random( ClockworkCreature.Definitions.Length );

			Weight = 1.0;
			Hue = 0x450;
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1112858, String.Format( "#{0}", ( (int) CreatureDef.CreatureType ).ToString() ) ); // ~1_TYPE~ clockwork mechanism
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( MadScientistQuest.QuestStarted( from ) )
				MadScientistQuest.BarkIngredient( from );
			else if ( !from.HasGump( typeof( MadScientistQuest.BeginQuestGump ) ) )
				from.SendGump( new MadScientistQuest.BeginQuestGump( this ) );
		}

		public void OnCompleted( Mobile from )
		{
			Mobile creature = new ClockworkCreature( CreatureDef );
			Point3D p = from.Location;

			creature.MoveToWorld( p, from.Map );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback(
				delegate
				{
					from.PlaySound( 0xFA );
					from.PlaySound( 0x5BC );
					from.PlaySound( 0x5C7 );

					Effects.SendLocationEffect( p, from.Map, 0x1FD4, 30, 16, 0x21, 4 );

					for ( int j = 0; j < 5; j++ )
					{
						Point3D loc = new Point3D( p.X, p.Y, 10 + p.Z + ( j * 20 ) );

						Effects.SendLocationEffect( loc, from.Map, 0x1AA1, 17, 16, 0x481, 4 );
						Effects.SendLocationEffect( loc, from.Map, 0x1A9F, 10, 16, 0x481, 4 );
						Effects.SendLocationEffect( loc, from.Map, 0x1A8, 25, 16, 0x47E, 4 );
					}

					// The training clockwork fails and the creature vanishes.
					from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1112987, from.NetState );

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
						delegate
						{
							creature.Delete();
						} ) );
				} ) );
		}

		public ClockworkMechanism( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_CreatureDef );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version > 0 )
				m_CreatureDef = reader.ReadInt();
		}
	}
}
