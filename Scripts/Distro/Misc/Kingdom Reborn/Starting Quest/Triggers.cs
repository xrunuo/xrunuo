using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
	public class KRStartingQuestTrigger : Item
	{
		private int m_QuestStep;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public int QuestStep
		{
			get { return m_QuestStep; }
			set { m_QuestStep = value; }
		}

		[Constructable]
		public KRStartingQuestTrigger( int queststep )
			: base( 0x1BC3 )
		{
			Name = "KR Starting Quest Trigger";

			Movable = false;
			Visible = false;

			m_QuestStep = queststep;
		}


		public override bool OnMoveOver( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				pm.CheckKRStartingQuestStep( m_QuestStep );
			}

			return base.OnMoveOver( m );
		}

		public KRStartingQuestTrigger( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_QuestStep );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_QuestStep = reader.ReadInt();
		}
	}

	public class KRStartingQuestGate : Item
	{
		private int m_QuestStep;
		private Point3D m_Dest;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public int QuestStep
		{
			get { return m_QuestStep; }
			set { m_QuestStep = value; }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public Point3D Dest
		{
			get { return m_Dest; }
			set { m_Dest = value; }
		}

		[Constructable]
		public KRStartingQuestGate( int queststep, Point3D dest )
			: base( 0x832 ) // 0x830
		{
			Movable = false;

			m_QuestStep = queststep;
			m_Dest = dest;
		}

		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );

			if ( !from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
			else if ( from is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) from;

				if ( pm.KRStartingQuestStep == ( m_QuestStep - 1 ) )
				{
					pm.MoveToWorld( m_Dest, Map.Trammel );

					pm.CloseGump( typeof( KRStartingQuestGump ) );
					pm.CloseGump( typeof( KRStartingQuestCancelGump ) );

					pm.KRStartingQuestStep++;
				}
			}
		}

		public KRStartingQuestGate( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Dest );
			writer.Write( m_QuestStep );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Dest = reader.ReadPoint3D();
			m_QuestStep = reader.ReadInt();
		}
	}

	public class KRStartingQuestTeleporter : Item
	{
		private int m_QuestStep;
		private Point3D m_Dest;
		private bool m_AdvanceLevel;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public bool AdvanceLevel
		{
			get { return m_AdvanceLevel; }
			set { m_AdvanceLevel = value; }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public int QuestStep
		{
			get { return m_QuestStep; }
			set { m_QuestStep = value; }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public Point3D Dest
		{
			get { return m_Dest; }
			set { m_Dest = value; }
		}

		[Constructable]
		public KRStartingQuestTeleporter( int queststep, Point3D dest )
			: base( 0x1BC3 )
		{
			Name = "KR Starting Quest Teleporter";

			Movable = false;
			Visible = false;

			m_QuestStep = queststep;
			m_Dest = dest;
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				if ( pm.KRStartingQuestStep == ( m_QuestStep - 1 ) )
				{
					if ( m_AdvanceLevel )
					{
						pm.CloseGump( typeof( KRStartingQuestGump ) );
						pm.CloseGump( typeof( KRStartingQuestCancelGump ) );

						pm.KRStartingQuestStep++;
					}

					pm.MoveToWorld( m_Dest, Map.Trammel );
				}
			}

			return false;
		}

		public KRStartingQuestTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_AdvanceLevel );
			writer.Write( m_Dest );
			writer.Write( m_QuestStep );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_AdvanceLevel = reader.ReadBool();
			m_Dest = reader.ReadPoint3D();
			m_QuestStep = reader.ReadInt();
		}
	}

	public class KRWaypointRemover : Item
	{
		private int m_QuestStep;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Developer )]
		public int QuestStep
		{
			get { return m_QuestStep; }
			set { m_QuestStep = value; }
		}

		[Constructable]
		public KRWaypointRemover( int queststep )
			: base( 0x1BC3 )
		{
			Name = "KR Waypoint Remover";

			Movable = false;
			Visible = false;

			m_QuestStep = queststep;
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				if ( pm.KRStartingQuestStep <= m_QuestStep )
					pm.Send( new RemoveWaypoint( Serial.MinusOne ) );
			}

			return base.OnMoveOver( m );
		}

		public KRWaypointRemover( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_QuestStep );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_QuestStep = reader.ReadInt();
		}
	}
}