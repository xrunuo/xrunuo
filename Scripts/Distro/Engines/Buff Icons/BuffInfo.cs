using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.BuffIcons
{
	public class BuffInfo
	{
		public static bool Enabled { get { return true; } }

		public static void Initialize()
		{
			// To Ensure that it's sent only AFTER we get the client's version, and right after.
			if ( Enabled )
				PacketHandlers.Instance.Register( 0xBD, 0, false, new OnPacketReceive( HandleBuff ) );
		}

		private static void HandleBuff( NetState state, PacketReader pvSrc )
		{
			PacketHandlers.Instance.ClientVersion( state, pvSrc );

			PlayerMobile pm = state.Mobile as PlayerMobile;

			if ( pm != null )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( pm.ResendBuffs ) );
		}

		#region Properties
		private BuffIcon m_Id;
		public BuffIcon Id { get { return m_Id; } }

		private int m_TitleCliloc;
		public int TitleCliloc { get { return m_TitleCliloc; } }

		private int m_SecondaryCliloc;
		public int SecondaryCliloc { get { return m_SecondaryCliloc; } }

		private TimeSpan m_TimeLength;
		public TimeSpan TimeLength { get { return m_TimeLength; } }

		private DateTime m_TimeStart;
		public DateTime TimeStart { get { return m_TimeStart; } }

		private Timer m_Timer;
		public Timer Timer { get { return m_Timer; } }

		private bool m_RetainThroughDeath;
		public bool RetainThroughDeath { get { return m_RetainThroughDeath; } }

		private TextDefinition m_Args;
		public TextDefinition Args { get { return m_Args; } }
		#endregion

		#region Constructors
		public BuffInfo( BuffIcon iconID, int titleCliloc )
			: this( iconID, titleCliloc, titleCliloc + 1 )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc )
		{
			m_Id = iconID;
			m_TitleCliloc = titleCliloc;
			m_SecondaryCliloc = secondaryCliloc;
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, TimeSpan length, Mobile m )
			: this( iconID, titleCliloc, titleCliloc + 1, length, m )
		{
		}

		// Only the timed one needs to Mobile to know when to automagically remove it.
		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m )
			: this( iconID, titleCliloc, secondaryCliloc, length, m, false )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m, bool retainThroughDeath )
			: this( iconID, titleCliloc, secondaryCliloc )
		{
			m_TimeLength = length;
			m_TimeStart = DateTime.Now;

			m_Timer = Timer.DelayCall( length, new TimerStateCallback( RemoveBuffDelegate ), m );

			m_RetainThroughDeath = retainThroughDeath;
		}

		public void RemoveBuffDelegate( object o )
		{
			PlayerMobile pm = o as PlayerMobile;

			if ( pm != null )
				pm.RemoveBuff( this );
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, TextDefinition args )
			: this( iconID, titleCliloc, titleCliloc + 1, args )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, TextDefinition args )
			: this( iconID, titleCliloc, secondaryCliloc )
		{
			m_Args = args;
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, bool retainThroughDeath )
			: this( iconID, titleCliloc, titleCliloc + 1, retainThroughDeath )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, bool retainThroughDeath )
			: this( iconID, titleCliloc, secondaryCliloc )
		{
			m_RetainThroughDeath = retainThroughDeath;
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, TextDefinition args, bool retainThroughDeath )
			: this( iconID, titleCliloc, titleCliloc + 1, args, retainThroughDeath )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, TextDefinition args, bool retainThroughDeath )
			: this( iconID, titleCliloc, secondaryCliloc, args )
		{
			m_RetainThroughDeath = retainThroughDeath;
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, TimeSpan length, Mobile m, TextDefinition args )
			: this( iconID, titleCliloc, titleCliloc + 1, length, m, args )
		{
		}

		public BuffInfo( BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m, TextDefinition args )
			: this( iconID, titleCliloc, secondaryCliloc, length, m )
		{
			m_Args = args;
		}
		#endregion

		#region Convience Methods
		public static void AddBuff( Mobile m, BuffInfo b )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
				pm.AddBuff( b );
		}

		public static void RemoveBuff( Mobile m, BuffInfo b )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
				pm.RemoveBuff( b );
		}

		public static void RemoveBuff( Mobile m, BuffIcon b )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
				pm.RemoveBuff( b );
		}
		#endregion
	}
}
