using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public interface IGatherer
	{
		bool DoesGather { get; }
		Mobile GatherTarget { get; set; }
		DateTime NextGatherAttempt { get; set; }
	}

	public abstract class VoidCreature : BaseCreature
	{
		public static readonly Map InvasionMap = Map.TerMur;

		private List<EvolutionHandler> m_Evolutions;
		private EvolutionTimer m_Timer;

		public VoidCreature( AIType ai, FightMode mode, int iRangePerception, int iRangeFight, double dActiveSpeed, double dPassiveSpeed )
			: base( ai, mode, iRangePerception, iRangeFight, dActiveSpeed, dPassiveSpeed )
		{
			m_Evolutions = new List<EvolutionHandler>();

			CreateEvolutionHandlers();

			m_Timer = new EvolutionTimer( this );
			m_Timer.Start();

			Team = 1;
		}

		protected virtual void CreateEvolutionHandlers()
		{
		}

		protected void AddEvolutionHandler( EvolutionHandler handler )
		{
			m_Evolutions.Add( handler );
		}

		public virtual void Evolve( Type evolutionType, bool inGroup )
		{
			Mobile evolution = SpawnEvolution( evolutionType );

			if ( evolution != null )
			{
				if ( inGroup )
				{
					List<Mobile> toEvolve = new List<Mobile>();

					foreach ( Mobile m in this.GetMobilesInRange( 3 ) )
					{
						if ( m != this && m.GetType() == this.GetType() )
							toEvolve.Add( m );
					}

					foreach ( Mobile m in toEvolve )
					{
						m.Delete();
					}
				}

				// TODO: taken from revenant death, check at OSI
				Effects.PlaySound( Location, Map, 0x653 );
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, TimeSpan.FromSeconds( 10.0 ) ), 0x37CC, 1, 50, 0x49A, 7, 9909, 0 );

				evolution.MoveToWorld( this.Location, InvasionMap );

				if ( this.Spawner != null )
				{
					evolution.Spawner = this.Spawner;
					this.Spawner.Replace( this, evolution );
					this.Spawner = null;
				}

				this.Delete();

				if ( evolution is BaseCreature )
				{
					BaseCreature bc = evolution as BaseCreature;

					bc.Home = bc.Location;
					bc.RangeHome = this.RangeHome + 20;
				}

				if ( 0.1 > Utility.RandomDouble() )
					Ortanord.Spawn( evolution.Location, InvasionMap );
			}
		}

		private Mobile SpawnEvolution( Type type )
		{
			Mobile m = null;

			try
			{
				m = (Mobile) Activator.CreateInstance( type );
			}
			catch
			{
			}

			return m;
		}

		public void OnKill( Mobile m )
		{
			foreach ( EvolutionHandler handler in m_Evolutions )
			{
				handler.OnKill( m );
			}
		}

		public void OnTick()
		{
			foreach ( EvolutionHandler handler in m_Evolutions )
			{
				handler.OnTick();
			}
		}

		private class EvolutionTimer : Timer
		{
			private VoidCreature m_Owner;

			public EvolutionTimer( VoidCreature owner )
				: base( TimeSpan.FromSeconds( 10.0 ), TimeSpan.FromSeconds( 10.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
					Stop();
				else
					m_Owner.OnTick();
			}
		}

		// So they can evolve without having any players around
		public override bool PlayerRangeSensitive { get { return false; } }

		public override bool AlwaysMurderer { get { return true; } }

		public VoidCreature( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Evolutions = new List<EvolutionHandler>();

			CreateEvolutionHandlers();

			m_Timer = new EvolutionTimer( this );
			m_Timer.Start();
		}
	}

	public abstract class EvolutionHandler
	{
		private VoidCreature m_Owner;
		private Type m_EvolutionType;
		private int m_RequiredPoints;

		protected VoidCreature Owner { get { return m_Owner; } }
		protected Type EvolutionType { get { return m_EvolutionType; } }
		protected int RequiredPoints { get { return m_RequiredPoints; } }

		private int m_CurrentPoints;

		protected int CurrentPoints { get { return m_CurrentPoints; } set { m_CurrentPoints = value; } }

		public EvolutionHandler( VoidCreature owner, Type evolutionType, int requiredPoints )
		{
			m_Owner = owner;
			m_EvolutionType = evolutionType;
			m_RequiredPoints = requiredPoints;
		}

		public virtual void OnKill( Mobile m )
		{
			CheckEvolution();
		}

		public virtual void OnTick()
		{
			CheckEvolution();
		}

		protected virtual bool GroupEvolution { get { return false; } }

		private void CheckEvolution()
		{
			if ( CurrentPoints >= RequiredPoints )
			{
				Owner.Evolve( EvolutionType, GroupEvolution );
			}
		}
	}

	public class KillingPathHandler : EvolutionHandler
	{
		public KillingPathHandler( VoidCreature owner, Type evolutionType, int requiredPoints )
			: base( owner, evolutionType, requiredPoints )
		{
		}

		public override void OnKill( Mobile m )
		{
			CurrentPoints += m.Fame;

			base.OnKill( m );
		}
	}

	public class GroupingPathHandler : EvolutionHandler
	{
		protected override bool GroupEvolution { get { return true; } }

		public GroupingPathHandler( VoidCreature owner, Type evolutionType, int requiredPoints )
			: base( owner, evolutionType, requiredPoints )
		{
		}

		public override void OnTick()
		{
			bool found = false;

			foreach ( Mobile m in Owner.GetMobilesInRange( 3 ) )
			{
				if ( m != Owner && m.GetType() == Owner.GetType() )
				{
					CurrentPoints += 1;

					found = true;
				}
			}

			if ( !found )
				CurrentPoints = 0;

			base.OnTick();
		}
	}

	public class SurvivalPathHandler : EvolutionHandler
	{
		public SurvivalPathHandler( VoidCreature owner, Type evolutionType, int requiredPoints )
			: base( owner, evolutionType, requiredPoints )
		{
		}

		public override void OnTick()
		{
			if ( Owner.Combatant != null )
				CurrentPoints += Owner.Combatant.Fame / 10;

			base.OnTick();
		}
	}
}