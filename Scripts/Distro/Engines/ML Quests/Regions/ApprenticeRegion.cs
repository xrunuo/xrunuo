using System;
using System.Xml;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Regions;
using Server.Engines.Quests;
using Server.Engines.BuffIcons;

namespace Server.Regions
{
	public class ApprenticeRegion : BaseRegion
	{
		public ApprenticeRegion( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
		}

		private Hashtable m_Table = new Hashtable();

		public Hashtable Table
		{
			get { return m_Table; }
		}

		public override void OnEnter( Mobile m )
		{
			base.OnEnter( m );

			if ( m is PlayerMobile )
			{
				PlayerMobile player = (PlayerMobile) m;

				for ( int i = 0; i < player.Quests.Count; i++ )
				{
					BaseQuest quest = player.Quests[i];

					for ( int j = 0; j < quest.Objectives.Count; j++ )
					{
						BaseObjective objective = quest.Objectives[j];

						if ( objective is ApprenticeObjective && !objective.Completed )
						{
							ApprenticeObjective apprentice = (ApprenticeObjective) objective;

							if ( IsPartOf( apprentice.Region ) )
							{
								if ( apprentice.Enter is int )
									player.SendLocalizedMessage( (int) apprentice.Enter );
								else if ( apprentice.Enter is string )
									player.SendMessage( (string) apprentice.Enter );

								BuffInfo info = new BuffInfo( BuffIcon.ArcaneEmpowerment, 1078511 ); // Accelerated Skillgain
								BuffInfo.AddBuff( m, info );
								m_Table[m] = info;
							}
						}
					}
				}
			}
		}

		public override void OnExit( Mobile m )
		{
			base.OnExit( m );

			if ( m is PlayerMobile )
			{
				PlayerMobile player = (PlayerMobile) m;

				for ( int i = 0; i < player.Quests.Count; i++ )
				{
					BaseQuest quest = player.Quests[i];

					for ( int j = 0; j < quest.Objectives.Count; j++ )
					{
						BaseObjective objective = quest.Objectives[j];

						if ( objective is ApprenticeObjective && !objective.Completed )
						{
							ApprenticeObjective apprentice = (ApprenticeObjective) objective;

							if ( IsPartOf( apprentice.Region ) )
							{
								if ( apprentice.Leave is int )
									player.SendLocalizedMessage( (int) apprentice.Leave );
								else if ( apprentice.Leave is string )
									player.SendMessage( (string) apprentice.Leave );

								if ( m_Table[m] is BuffInfo )
									BuffInfo.RemoveBuff( m, (BuffInfo) m_Table[m] );
							}
						}
					}
				}
			}
		}
	}
}