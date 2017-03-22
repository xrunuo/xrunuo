using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Engines.BuffIcons;
using Server.Engines.PartySystem;
using Server.Mobiles;

namespace Server.Spells.Bard
{
	public abstract class AreaSpellsong : Spellsong
	{
		public abstract BuffIcon BuffIcon { get; }

		private ISet<AttributeMod> m_Mods;
		private ISet<StatMod> m_StatMods;

		private ISet<Mobile> m_Targets;
		public ISet<Mobile> Targets { get { return m_Targets; } }

		private BuffInfo m_BuffInfo;
		public BuffInfo BuffInfo { get { return m_BuffInfo; } protected set { m_BuffInfo = value; } }

		protected AreaSpellsong( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public override void OnCast()
		{
			Spellsong song = GetActiveSpellsong( Caster, this.GetType() );

			if ( song != null )
			{
				// You halt your spellsong.
				song.InterruptSong( 1115774 );
			}
			else if ( CheckSequence() )
			{
				m_Targets = new HashSet<Mobile>();

				StartSong();
			}

			FinishSequence();
		}

		protected override int ComputeUpkeepCost()
		{
			return base.ComputeUpkeepCost() + ( m_Targets.Count / 5 );
		}

		private static IEnumerable<Mobile> GetValidTargets( Mobile caster )
		{
			var partyPlayers = new HashSet<Mobile>( GetPartyPlayers( caster ) );

			foreach ( Mobile m in caster.GetMobilesInRange( 8 ) )
			{
				if ( !caster.CanBeBeneficial( m, false ) )
					continue;

				if ( m.Player )
				{
					if ( m.Alive && partyPlayers.Contains( m ) )
					{
						yield return m;

						Mobile mount = m.Mount as Mobile;

						if ( mount != null )
							yield return mount;
					}
				}
				else if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature) m;

					if ( bc.Controlled && !bc.IsDeadBondedPet && partyPlayers.Contains( bc.ControlMaster ) )
						yield return bc;
				}
			}
		}

		private static IEnumerable<Mobile> GetPartyPlayers( Mobile caster )
		{
			Party party = Party.Get( caster );

			if ( party != null )
				return party.Members.Select( i => i.Mobile );
			else
				return new[] { caster };
		}

		private void ProcessTargets()
		{
			ISet<Mobile> oldTargets = m_Targets;
			ISet<Mobile> newTargets = new HashSet<Mobile>( GetValidTargets( Caster ) );

			foreach ( Mobile m in newTargets )
			{
				if ( !oldTargets.Contains( m ) )
					AddTarget( m );
			}

			foreach ( Mobile m in oldTargets )
			{
				if ( !newTargets.Contains( m ) )
					RemoveTarget( m );
			}

			m_Targets = newTargets;

			foreach ( Mobile m in m_Targets )
			{
				Caster.DoBeneficial( m );
			}
		}

		protected override void OnSongStarted()
		{
			m_Mods = new HashSet<AttributeMod>();
			m_StatMods = new HashSet<StatMod>();

			AddMods( m_Mods, m_StatMods );
		}

		protected virtual void AddMods( ISet<AttributeMod> mods, ISet<StatMod> statMods )
		{
		}

		protected override void OnTargetAdded( Mobile m )
		{
			BuffInfo.AddBuff( m, this.BuffInfo );

			foreach ( AttributeMod mod in m_Mods )
			{
				m.AddAttributeMod( mod );
			}

			foreach ( StatMod mod in m_StatMods )
			{
				m.AddStatMod( mod );
			}
		}

		protected override void OnTargetRemoved( Mobile m )
		{
			BuffInfo.RemoveBuff( m, this.BuffIcon );

			foreach ( AttributeMod mod in m_Mods )
			{
				m.RemoveAttributeMod( mod );
			}

			foreach ( StatMod mod in m_StatMods )
			{
				m.RemoveStatMod( mod.Name );
			}
		}

		protected override void OnSongInterrupted()
		{
			foreach ( Mobile m in m_Targets )
			{
				RemoveTarget( m );
			}
		}

		protected override void OnUpkeep()
		{
			ProcessTargets();
		}
	}
}