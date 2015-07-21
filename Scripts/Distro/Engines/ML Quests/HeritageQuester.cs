using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells.Fifth;

namespace Server.Engines.Quests
{
	public delegate bool HeritageQuestCheck( PlayerMobile player );

	public interface IHeritageQuestInfo
	{
		object Title { get; }

		bool Check( PlayerMobile player );
		bool Check( PlayerMobile player, bool delete );
	}

	public class DelegateQuestInfo : IHeritageQuestInfo
	{
		private HeritageQuestCheck m_Check;
		private object m_Title;

		public HeritageQuestCheck Quest { get { return m_Check; } }
		public object Title { get { return m_Title; } }

		public DelegateQuestInfo( HeritageQuestCheck check, object title )
		{
			m_Check = check;
			m_Title = title;
		}

		public bool Check( PlayerMobile player )
		{
			return Check( player, false );
		}

		public bool Check( PlayerMobile player, bool delete )
		{
			return m_Check( player );
		}
	}

	public class HeritageQuestInfo : IHeritageQuestInfo
	{
		private Type m_Quest;
		private object m_Title;

		public Type Quest { get { return m_Quest; } }
		public object Title { get { return m_Title; } }

		public HeritageQuestInfo( Type quest, object title )
		{
			m_Quest = quest;
			m_Title = title;
		}

		public bool Check( PlayerMobile player )
		{
			return Check( player, false );
		}

		public bool Check( PlayerMobile player, bool delete )
		{
			int j = 0;

			while ( j < player.DoneQuests.Count && player.DoneQuests[j].QuestType != m_Quest )
				j += 1;

			if ( j == player.DoneQuests.Count )
				return false;
			else if ( delete )
				player.DoneQuests.RemoveAt( j );

			return true;
		}
	}

	public abstract class HeritageQuester : BaseVendor
	{
		#region Vendor stuff
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }
		public override bool IsActiveVendor { get { return false; } }
		public override void InitSBInfo() { }
		#endregion

		public virtual int AutoSpeakRange { get { return 7; } }
		public virtual object ConfirmMessage { get { return 0; } }
		public virtual object IncompleteMessage { get { return 0; } }

		private List<IHeritageQuestInfo> m_Quests;
		private List<object> m_Objectives;
		private List<object> m_Story;
		private bool m_Busy;
		private int m_Index;

		public List<IHeritageQuestInfo> Quests
		{
			get { return m_Quests; }
		}

		public List<object> Objectives
		{
			get { return m_Objectives; }
		}

		public List<object> Story
		{
			get { return m_Story; }
		}

		public HeritageQuester()
			: this( null )
		{
		}

		public HeritageQuester( string name )
			: this( name, null )
		{
		}

		public HeritageQuester( string name, string title )
			: base( title )
		{
			m_Quests = new List<IHeritageQuestInfo>();
			m_Objectives = new List<object>();
			m_Story = new List<object>();

			InitializeQuester();

			Name = name;
			SpeechHue = 0x3B2;
		}

		public HeritageQuester( Serial serial )
			: base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && !m.Hidden && m is PlayerMobile )
			{
				int range = AutoSpeakRange;

				if ( range >= 0 && this.InRange( m, range ) && !this.InRange( oldLocation, range ) )
					OnTalk( m );
			}
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.Alive )
				OnTalk( m );
		}

		public virtual void OnTalk( Mobile m )
		{
			if ( m.Hidden || m_Busy || m.Race == Race )
			{
				m.SendLocalizedMessage( 1074017 ); // He's too busy right now, so he ignores you.
				return;
			}

			m_Busy = true;
			m_Index = 0;

			SpeechHue = Utility.RandomDyedHue();
			Say( m.Name );
			SpeechHue = 0x3B2;

			if ( CheckCompleted( m ) )
				Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 10 ), Story.Count + 1, new TimerStateCallback( SayStory ), m );
			else
			{
				List<object> incomplete = FindIncompleted( m );
				TimeSpan delay = TimeSpan.FromSeconds( 2 );

				if ( incomplete.Count == m_Quests.Count + 1 )
				{
					incomplete = m_Objectives;
					delay = TimeSpan.FromSeconds( 10 );
				}

				Timer.DelayCall( TimeSpan.Zero, delay, incomplete.Count, new TimerStateCallback( SayInstructions ), incomplete );
			}
		}

		public bool CheckCompleted( Mobile m )
		{
			return CheckCompleted( m, false );
		}

		public bool CheckCompleted( Mobile m, bool delete )
		{
			for ( int i = 0; i < m_Quests.Count; i += 1 )
			{
				IHeritageQuestInfo info = m_Quests[i];

				if ( !info.Check( (PlayerMobile) m, delete ) )
					return false;
			}

			return true;
		}

		public List<object> FindIncompleted( Mobile m )
		{
			List<object> incomplete = new List<object>();

			incomplete.Add( IncompleteMessage );

			for ( int i = 0; i < m_Quests.Count; i += 1 )
			{
				IHeritageQuestInfo info = m_Quests[i];

				if ( !info.Check( (PlayerMobile) m ) )
					incomplete.Add( info.Title );
			}

			return incomplete;
		}

		private void SayInstructions( object args )
		{
			if ( args is List<object> )
				SayInstructions( (List<object>) args );
		}

		public void SayInstructions( List<object> incomplete )
		{
			Say( this, incomplete[m_Index] );

			m_Index += 1;

			if ( m_Index == incomplete.Count )
				m_Busy = false;
		}

		private void SayStory( object args )
		{
			if ( args is Mobile )
				SayStory( (Mobile) args );
		}

		public void SayStory( Mobile m )
		{
			if ( m_Index < m_Story.Count )
				Say( this, m_Story[m_Index] );
			else
			{
				m_Busy = false;

				m.CloseGump( typeof( ConfirmHeritageGump ) );
				m.SendGump( new ConfirmHeritageGump( this ) );
			}

			m_Index += 1;
		}

		#region Static
		private static Hashtable m_Pending = new Hashtable();

		public static void AddPending( Mobile m, HeritageQuester quester )
		{
			m_Pending[m] = quester;
		}

		public static void RemovePending( Mobile m )
		{
			m_Pending[m] = null;
		}

		public static bool IsPending( Mobile m )
		{
			return m_Pending[m] != null;
		}

		public static HeritageQuester Pending( Mobile m )
		{
			return m_Pending[m] as HeritageQuester;
		}

		public static void Say( Mobile m, object message )
		{
			if ( message is int )
				m.Say( (int) message );
			else if ( message is string )
				m.Say( (string) message );
		}

		public static bool Check( Mobile m )
		{
			if ( !m.Alive )
				m.SendLocalizedMessage( 1073646 ); // Only the living may proceed...			
			else if ( m.Mounted )
				m.SendLocalizedMessage( 1073647 ); // You may not continue while mounted...			
			else if ( m.IsBodyMod || m.HueMod > 0 || !m.CanBeginAction( typeof( IncognitoSpell ) ) )
				m.SendLocalizedMessage( 1073648 ); // You may only proceed while in your original state...						
			else if ( m.Spell != null && m.Spell.IsCasting )
				m.SendLocalizedMessage( 1073649 ); // One may not proceed while embracing magic...			
			else if ( IsUnburdened( m ) )
				m.SendLocalizedMessage( 1073650 ); // To proceed you must be unburdened by equipment...
			else if ( m.Hits < m.HitsMax )
				m.SendLocalizedMessage( 1073652 ); // You must be healthy to proceed...				
			else
				return true;

			return false;
		}

		public static bool IsUnburdened( Mobile m )
		{
			int count = m.GetEquippedItems().Count() - 1;

			if ( m.Backpack != null )
				count -= 1;

			return count > 0;
		}
		#endregion

		public virtual void InitializeQuester()
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

			int version = reader.ReadInt();

			m_Quests = new List<IHeritageQuestInfo>();
			m_Objectives = new List<object>();
			m_Story = new List<object>();

			InitializeQuester();
		}
	}
}