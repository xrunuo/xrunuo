using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
	public class CreepyWeed : Item
	{
		public override int LabelNumber { get { return 1113504; } } // creepy weed

		public override bool ForceShowProperties { get { return true; } }

		[Constructable]
		public CreepyWeed()
			: base( 0xCA5 )
		{
			Hue = 2071;
			Movable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( Location, 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
			else
			{
				PlayerMobile pm = from as PlayerMobile;

				if ( pm != null )
				{
					InTheWeedsQuest quest = QuestHelper.GetQuest<InTheWeedsQuest>( pm );

					if ( quest == null )
					{
						// Weeding a Gargoyle’s garden without just cause is strictly forbidden.
						SendLocalizedMessageTo( from, 1113514 );
					}
					else if ( quest.GivenPitchfork )
					{
						// You have discovered the pitchfork, you should return it to Farmer Nash and let him tend his own garden.
						SendLocalizedMessageTo( from, 1113508 );
					}
					else
					{
						if ( 0.1 > Utility.RandomDouble() )
						{
							// You find Farmer Nash's pitchfork under one of the brambles of weeds.
							SendLocalizedMessageTo( from, 1113516 );

							Item pitchfork = new FarmerNashPitchfork();

							if ( from.AddToBackpack( pitchfork ) )
							{
								// You pick up the pitchfork and put it in your pack.
								SendLocalizedMessageTo( from, 1113564 );
							}
							else
							{
								// Your backpack is full.  You will need to remove something before you can pick up the pitchfork.
								SendLocalizedMessageTo( from, 1113565 );

								pitchfork.MoveToWorld( Location, Map );
							}

							quest.GivenPitchfork = true;
						}
						else
						{
							BaseCreature bc = Activator.CreateInstance( m_MonsterTypes[Utility.Random( m_MonsterTypes.Length )] ) as BaseCreature;

							bc.RemoveOnSave = true;
							bc.MoveToWorld( Location, Map );
						}

						Visible = false;

						Timer.DelayCall( TimeSpan.FromMinutes( 15.0 ), new TimerCallback(
							delegate { Visible = true; } ) );
					}
				}
			}
		}

		private static Type[] m_MonsterTypes = new Type[]
			{
				typeof( Snake ),	typeof( Mongbat ),
				typeof( Raptor ),	typeof( SilverSerpent ),
				typeof( Ballem )
			};

		public CreepyWeed( Serial serial )
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

			if ( !Visible )
			{
				Timer.DelayCall( TimeSpan.FromMinutes( 15.0 ), new TimerCallback(
					delegate { Visible = true; } ) );
			}
		}
	}
}
