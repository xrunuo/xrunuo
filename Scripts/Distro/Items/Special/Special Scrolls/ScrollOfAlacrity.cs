using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Engines.Quests;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class ScrollOfAlacrity : Item
	{
		public override int LabelNumber { get { return 1078604; } } // Scroll of Alacrity

		private SkillName m_Skill;
		private TimeSpan m_Duration;

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillName Skill
		{
			get { return m_Skill; }
			set { m_Skill = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Duration
		{
			get { return m_Duration; }
			set { m_Duration = value; }
		}

		public static ScrollOfAlacrity CreateRandom()
		{
			return new ScrollOfAlacrity( Utility.RandomSkill() );
		}

		[Constructable]
		public ScrollOfAlacrity()
			: this( SkillName.Alchemy )
		{
		}

		[Constructable]
		public ScrollOfAlacrity( SkillName skill )
			: this( skill, TimeSpan.FromMinutes( 15.0 ) )
		{
		}

		[Constructable]
		public ScrollOfAlacrity( SkillName skill, TimeSpan duration )
			: base( 0x14EF )
		{
			m_Skill = skill;
			m_Duration = duration;

			LootType = LootType.Cursed;

			Weight = 1.0;
			Hue = 0x4AB;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1076759, String.Format( "{0}\t{1} minutes", SkillInfo.Table[(int) m_Skill].Name, m_Duration.TotalMinutes ) ); // Skill: ~1_skillname~ ~2_skillamount~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !CanUse( from ) )
				return;

			from.CloseGump<InternalGump>();
			from.SendGump( new InternalGump( from, this ) );
		}

		private bool CanUse( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return false;
			}

			if ( SkillCheck.HasAnyAcceleratedSkillGain( from ) )
			{
				from.SendLocalizedMessage( 1077951 ); // You are already under the effect of an accelerated skillgain scroll.
				return false;
			}

			return true;
		}

		public void Use( Mobile from )
		{
			if ( !CanUse( from ) )
				return;

			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			from.SendLocalizedMessage( 1077956 ); // You are infused with intense energy. You are under the effects of an accelerated skillgain scroll.

			Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( from.Location, from.Map, 0x243 );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 6, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 4, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( from.X - 6, from.Y - 4, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

			Effects.SendTargetParticles( from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );

			Delete();

			var expireTimer = Timer.DelayCall( m_Duration,
				delegate
				{
					m_Table.Remove( from );

					from.PlaySound( 0x1F8 );

					from.SendLocalizedMessage( 1077957 ); // The intense energy dissipates. You are no longer under the effects of an accelerated skillgain scroll.
				} );

			m_Table[from] = new ScrollOfAlacrityContext( Skill, expireTimer );
		}

		private static Dictionary<Mobile, ScrollOfAlacrityContext> m_Table = new Dictionary<Mobile, ScrollOfAlacrityContext>();

		public static bool HasAcceleratedSkillGain( Mobile from, Skill skill )
		{
			if ( !m_Table.ContainsKey( from ) )
				return false;

			var context = m_Table[from];

			return context.SkillName == skill.SkillName;
		}

		public static bool HasAnyAcceleratedSkillGain( Mobile from )
		{
			return m_Table.ContainsKey( from );
		}

		private class ScrollOfAlacrityContext
		{
			public SkillName SkillName { get; private set; }
			public Timer ExpireTimer { get; private set; }

			public ScrollOfAlacrityContext( SkillName skillName, Timer expireTimer )
			{
				SkillName = skillName;
				ExpireTimer = expireTimer;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Skill );
			writer.Write( (TimeSpan) m_Duration );
		}

		public ScrollOfAlacrity( Serial serial )
			: base( serial )
		{
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
						m_Duration = reader.ReadTimeSpan();

						break;
					}
			}
		}

		private class InternalGump : Gump
		{
			public override int TypeID { get { return 0x232E; } }

			private Mobile m_Mobile;
			private ScrollOfAlacrity m_Scroll;

			/* Scroll of Alacrity */
			public virtual int TitleMsg { get { return 1078604; } }

			/* Do you wish to use this scroll? */
			public virtual int QuestionMsg { get { return 1049478; } }

			/* Using a Scroll of Alacrity for a given skill will increase the amount of
			 * skillgain you receive for that skill. Once the Scroll of Alacrity duration
			 * has expired, skillgain will return to normal for that skill. */
			public virtual int InfoMsg { get { return 1078602; } }

			public InternalGump( Mobile mobile, ScrollOfAlacrity scroll )
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
					m_Scroll.Use( m_Mobile );
			}
		}
	}
}
