using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Spells.Bard
{
	public abstract class Spellsong : Spell
	{
		public abstract BardMastery RequiredMastery { get; }
		public abstract int RequiredMana { get; }
		public abstract int UpkeepCost { get; }

		public virtual int StartEffectMessage { get { return -1; } }

		private bool m_Interrupted;

		private UpkeepTimer m_UpkeepTimer;
		public UpkeepTimer UpkeepTimer { get { return m_UpkeepTimer; } }

		public Spellsong( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public override bool ClearHandsOnCast { get { return false; } }
		public override int FasterCastingCap { get { return 4; } }

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			int mana = ScaleMana( GetMana() );

			if ( Caster.Mana < ScaleMana( mana ) )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return CheckSkills( true );
		}

		public override TimeSpan GetCastDelay()
		{
			if ( GetActiveSpellsong( Caster, this.GetType() ) != null )
				return TimeSpan.FromSeconds( 0.5 );

			return base.GetCastDelay();
		}

		public override int GetMana()
		{
			Spellsong song = GetActiveSpellsong( Caster, this.GetType() );

			if ( song != null )
				return 0;

			return RequiredMana - ComputeManaBonus();
		}

		public override bool CheckFizzle()
		{
			return true;
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Effects.SendPacket( Caster.Location, Caster.Map, new TargetEffect( Caster, 0x37C4, 10, 35, 0x66C, 3 ) );
		}

		public virtual bool CheckSkills( bool message )
		{
			if ( Caster is PlayerMobile )
			{
				PlayerMobile pm = Caster as PlayerMobile;

				if ( pm.BardMastery != this.RequiredMastery )
				{
					if ( message )
						pm.SendLocalizedMessage( 1115664 ); // You are not on the correct path for using this mastery ability.
				}
				else if ( pm.Skills[this.RequiredMastery.Skill].Value < 90.0 )
				{
					if ( message )
						pm.SendLocalizedMessage( 1115709 ); // Your skills are not high enough to invoke this mastery ability.
				}
				else
				{
					return true;
				}
			}

			return false;
		}

		public void Upkeep()
		{
			if ( !CheckSkills( false ) )
			{
				InterruptSong();
			}
			else if ( !Caster.Alive )
			{
				// Your spell song has been interrupted.
				InterruptSong( 1115710 );
			}
			else if ( !ConsumeUpkeepCost() )
			{
				// You do not have enough mana to continue infusing your song with magic.
				InterruptSong( 1115665 );
			}
			else
			{
				Caster.DisruptiveAction();
				Caster.RevealingAction();

				Effects.SendPacket( Caster.Location, Caster.Map, new TargetParticleEffect( Caster, 0x376A, 1, 32, 0x4F0, 0, 0x2365, 178, 0 ) );

				OnUpkeep();
			}
		}

		protected virtual bool ConsumeUpkeepCost()
		{
			int mana = ScaleMana( ComputeUpkeepCost() );

			if ( Caster.Mana >= mana )
			{
				Caster.Mana -= mana;

				return true;
			}

			return false;
		}

		protected virtual int ComputeUpkeepCost()
		{
			return UpkeepCost - ComputeManaBonus();
		}

		private int ComputeManaBonus()
		{
			return BardMastery.AllMasteries.Count( m => m != RequiredMastery && Caster.Skills[m.Skill].Base >= 100.0 );
		}

		protected virtual void OnSongStarted()
		{
		}

		protected virtual void OnSongInterrupted()
		{
		}

		protected virtual void OnUpkeep()
		{
		}

		private static Dictionary<Mobile, SpellsongCollection> m_CasterTable = new Dictionary<Mobile, SpellsongCollection>();

		public static IEnumerable<Spellsong> GetAllActiveSpellsongs( Mobile m )
		{
			if ( !m_CasterTable.ContainsKey( m ) )
				return Enumerable.Empty<Spellsong>();

			return m_CasterTable[m].All();
		}

		public static Spellsong GetActiveSpellsong( Mobile m, Type type )
		{
			if ( !m_CasterTable.ContainsKey( m ) )
				return null;

			return m_CasterTable[m][type] as Spellsong;
		}

		public void StartSong()
		{
			if ( !m_CasterTable.ContainsKey( Caster ) )
				m_CasterTable[Caster] = new SpellsongCollection();

			m_UpkeepTimer = new UpkeepTimer( this );
			m_UpkeepTimer.Start();

			OnSongStarted();

			m_CasterTable[Caster][GetType()] = this;
		}

		public void InterruptSong()
		{
			InterruptSong( -1 );
		}

		public void InterruptSong( int message )
		{
			if ( m_Interrupted )
				return;

			m_Interrupted = true;

			Type type = this.GetType();

			m_UpkeepTimer.Stop();

			OnSongInterrupted();

			m_CasterTable[Caster].Remove( type );

			if ( m_CasterTable[Caster].IsEmpty() )
				m_CasterTable.Remove( Caster );

			if ( message != -1 )
				Caster.SendLocalizedMessage( message );
		}

		private static Dictionary<Mobile, SpellsongCollection> m_EffectTable = new Dictionary<Mobile, SpellsongCollection>();

		public static T GetEffectSpellsong<T>( Mobile m ) where T : Spellsong
		{
			if ( !m_EffectTable.ContainsKey( m ) )
				return null;

			return m_EffectTable[m][typeof( T )] as T;
		}

		public static bool UnderEffect<T>( Mobile m ) where T : Spellsong
		{
			return GetEffectSpellsong<T>( m ) != null;
		}

		public static Spellsong GetEffectSpellsong( Mobile m, Type type )
		{
			if ( !m_EffectTable.ContainsKey( m ) )
				return null;

			return m_EffectTable[m][type] as Spellsong;
		}

		public static bool UnderEffect( Mobile m, Type type )
		{
			return GetEffectSpellsong( m, type ) != null;
		}

		protected void AddTarget( Mobile m )
		{
			if ( !m_EffectTable.ContainsKey( m ) )
				m_EffectTable[m] = new SpellsongCollection();

			Type type = this.GetType();

			if ( m_EffectTable[m][type] == null )
			{
				m_EffectTable[m][type] = this;

				if ( StartEffectMessage != -1 )
					m.SendLocalizedMessage( StartEffectMessage );

				OnTargetAdded( m );
			}
		}

		protected virtual void OnTargetAdded( Mobile m )
		{
		}

		protected void RemoveTarget( Mobile m )
		{
			Type type = this.GetType();

			Spellsong song = GetEffectSpellsong( m, type );

			if ( song != null && song.Caster == Caster )
			{
				m_EffectTable[m].Remove( type );

				if ( m_EffectTable[m].IsEmpty() )
					m_EffectTable.Remove( m );

				OnTargetRemoved( m );
			}
		}

		protected virtual void OnTargetRemoved( Mobile m )
		{
		}

		private class SpellsongCollection
		{
			private Dictionary<Type, Spellsong> m_Table;

			public Spellsong this[Type type]
			{
				get
				{
					if ( m_Table.ContainsKey( type ) )
						return m_Table[type];
					else
						return null;
				}
				set
				{
					m_Table[type] = value;
				}
			}

			public bool IsEmpty()
			{
				return m_Table.Count == 0;
			}

			public void Remove( Type type )
			{
				m_Table.Remove( type );
			}

			public IEnumerable<Spellsong> All()
			{
				return m_Table.Values;
			}

			public SpellsongCollection()
			{
				m_Table = new Dictionary<Type, Spellsong>();
			}
		}

		public void GetSkillBonus( out int music, out int peace, out int provo, out int disco )
		{
			music = Math.Max( 0, (int) ( ( Caster.Skills.Musicianship.Base - 90.0 ) / 10.0 ) );
			peace = Math.Max( 0, (int) ( ( Caster.Skills.Peacemaking.Base - 90.0 ) / 10.0 ) );
			provo = Math.Max( 0, (int) ( ( Caster.Skills.Provocation.Base - 90.0 ) / 10.0 ) );
			disco = Math.Max( 0, (int) ( ( Caster.Skills.Discordance.Base - 90.0 ) / 10.0 ) );
		}
	}

	public class UpkeepTimer : Timer
	{
		private Spellsong m_Owner;

		public UpkeepTimer( Spellsong owner )
			: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
		{
			m_Owner = owner;
		}

		protected override void OnTick()
		{
			m_Owner.Upkeep();
		}
	}

	public static class DamageInterruption
	{
		public static void Initialize()
		{
			Mobile.Damaged += new DamagedEventHandler( Mobile_Damaged );
		}

		private static void Mobile_Damaged( Mobile m, DamagedEventArgs args )
		{
			bool isFromPlayer = args.From != null && args.From.IsPlayer;

			foreach ( Spellsong spellsong in Spellsong.GetAllActiveSpellsongs( m ).ToArray() )
			{
				if ( isFromPlayer || args.Amount > Utility.Random( 50 ) )
				{
					// Your spell song has been interrupted.
					spellsong.InterruptSong( 1115710 );
				}
			}
		}
	}
}