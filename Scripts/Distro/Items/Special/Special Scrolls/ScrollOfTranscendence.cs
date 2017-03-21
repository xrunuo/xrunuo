using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Misc;

namespace Server.Items
{
	public class ScrollOfTranscendence : Item
	{
		public override int LabelNumber { get { return 1094934; } } // Scroll of Transcendence

		private SkillName m_Skill;
		private double m_Value;

		public static ScrollOfTranscendence CreateRandom( int val )
		{
			return CreateRandom( val, val );
		}

		public static ScrollOfTranscendence CreateRandom( int min, int max )
		{
			return new ScrollOfTranscendence( Utility.RandomSkill(), (double) Utility.RandomMinMax( min, max ) / 10 );
		}

		[Constructable]
		public ScrollOfTranscendence( SkillName skill, double value )
			: base( 0x14F0 )
		{
			Hue = 1168;
			Weight = 1.0;

			LootType = LootType.Cursed;

			m_Skill = skill;
			m_Value = value;
		}

		public ScrollOfTranscendence( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillName Skill { get { return m_Skill; } set { m_Skill = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public double Value { get { return m_Value; } set { m_Value = value; } }

		public virtual Gump BuildGump( Mobile from, ScrollOfTranscendence scroll )
		{
			return new InternalGump( from, scroll );
		}

		public void Use( Mobile from, bool firstStage )
		{
			if ( Deleted )
				return;

			if ( SkillCheck.HasAnyAcceleratedSkillGain( from ) )
			{
				from.SendLocalizedMessage( 1077951 ); // You are already under the effect of an accelerated skillgain scroll.
				return;
			}

			if ( IsChildOf( from.Backpack ) )
			{
				Skill skill = from.Skills[m_Skill];

				if ( skill != null )
				{
					if ( firstStage )
					{
						from.CloseGump( typeof( ScrollOfTranscendence.InternalGump ) );
						from.SendGump( BuildGump( from, this ) );
					}
					else
					{
						// calculamos las skills que puede bajar
						double potentialFreeSkill = 0;

						for ( int i = 0; i < from.Skills.Length; i++ )
						{
							Skill sk = from.Skills[i];

							if ( sk.Lock == SkillLock.Down )
								potentialFreeSkill += sk.Base;
						}

						if ( skill.Lock == SkillLock.Locked || skill.Lock == SkillLock.Down || skill.Base >= skill.Cap || from.Skills.Total - ( 10 * potentialFreeSkill ) >= from.Skills.Cap )
						{
							from.SendLocalizedMessage( 1094935 ); // You cannot increase this skill at this time. The skill may be locked or set to lower in your skill menu. If you are at your total skill cap, you must use a Powerscroll to increase your current skill cap. 
						}
						else
						{
							double toRaise = Math.Min( m_Value, skill.Cap - skill.Base );

							// bajamos las skills que necesitemos, o las que podamos bajar
							double toFree = toRaise - ( (double) ( from.Skills.Cap - from.Skills.Total ) / 10 );

							if ( toFree < 0.0 )
								toFree = 0.0;

							for ( int i = 0; i < from.Skills.Length && toFree > 0; i++ )
							{
								Skill sk = from.Skills[i];

								if ( sk.Lock == SkillLock.Down )
								{
									if ( sk.Base > toFree )
									{
										sk.Base -= toFree;
										toFree = 0;
									}
									else
									{
										toFree -= sk.Base;
										sk.Base = 0;
									}
								}
							}

							toRaise -= toFree;

							skill.Base += toRaise;

							Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
							Effects.PlaySound( from.Location, from.Map, 0x243 );

							Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 6, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
							Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 4, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
							Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 6, from.Y - 4, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

							Effects.SendTargetParticles( from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );

							Delete();
						}
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			Use( from, true );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1076759, String.Format( "{0}\t{1} skill points", SkillInfo.Table[(int) m_Skill].Name, m_Value ) ); // Skill: ~1_skillname~ ~2_skillamount~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Skill );
			writer.Write( (double) m_Value );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Skill = (SkillName) reader.ReadInt();
						m_Value = reader.ReadDouble();

						break;
					}
			}
		}

		public class InternalGump : Gump
		{
			public override int TypeID { get { return 0x232E; } }

			private Mobile m_Mobile;
			private ScrollOfTranscendence m_Scroll;

			/* Scroll of Transcendence */
			public virtual int TitleMsg { get { return 1094934; } }

			/* Do you wish to use this scroll? */
			public virtual int QuestionMsg { get { return 1049478; } }

			/* Using a Scroll of Transcendence for a given skill will
			 * permanently increase your current level in that skill by the
			 * amount of points displayed on the scroll. As you may
			 * not gain skills beyond your maximum skill cap, any excess points
			 * will be lost. */
			public virtual int InfoMsg { get { return 1094933; } }

			public InternalGump( Mobile mobile, ScrollOfTranscendence scroll )
				: base( 25, 50 )
			{
				m_Mobile = mobile;
				m_Scroll = scroll;

				AddPage( 0 );

				AddBackground( 25, 10, 420, 200, 5054 );

				AddImageTiled( 33, 20, 401, 181, 2624 );
				AddAlphaRegion( 33, 20, 401, 181 );

				AddHtmlLocalized( 40, 48, 387, 100, InfoMsg, true, true );

				AddHtmlLocalized( 125, 148, 200, 20, QuestionMsg, 0xFFFFFF, false, false );

				AddHtmlLocalized( 135, 172, 120, 20, 1046362, 0xFFFFFF, false, false ); // Yes
				AddHtmlLocalized( 310, 172, 120, 20, 1046363, 0xFFFFFF, false, false ); // No

				AddButton( 100, 172, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				AddButton( 275, 172, 4005, 4007, 0, GumpButtonType.Reply, 0 );

				AddHtmlLocalized( 40, 20, 260, 20, TitleMsg, 0xFFFFFF, false, false );

				AddHtmlLocalized( 310, 20, 120, 20, 1044060 + (int) scroll.m_Skill, 0xFFFFFF, false, false );
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				if ( info.ButtonID == 1 )
					m_Scroll.Use( m_Mobile, false );
			}
		}
	}
}