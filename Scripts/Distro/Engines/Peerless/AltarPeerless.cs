using System;
using System.Collections.Generic;

using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
	public enum PeerlessList
	{
		None,
		DreadHorn,
		MelisandeTrammel,
		MelisandeFelucca,
		Travesty,
		InterredGrizzle,
		ParoxysmusTrammel,
		ParoxysmusFelucca,
		ShimmeringEffusionTrammel,
		ShimmeringEffusionFelucca,
		Dummy
	}

	public class AltarPeerless : Container
	{
		public static bool IsPeerlessBoss( Mobile m )
		{
			return m is DreadHorn || m is LadyMelisande || m is Travesty || m is ChiefParoxysmus || m is ShimmeringEffusion || m is MonstrousInterredGrizzle;
		}

		public override int DefaultMaxWeight { get { return 0; } }

		public override bool IsDecoContainer { get { return false; } }

		private PeerlessRegion m_Region;
		private bool m_Activated;
		private BaseActivation[] m_key = new BaseActivation[3];
		private Mobile m_Boss;
		private PeerlessList m_Peerless = 0;
		private Timer m_ResetTimer;
		private Timer m_PeerlessTimer;
		private Timer m_ClearTimer;
		private Mobile m_Owner;

		public PeerlessRegion Region
		{
			get { return m_Region; }
		}

		public Mobile Boss
		{
			get { return m_Boss; }
		}

		public bool Activated
		{
			get { return m_Activated; }
			set { m_Activated = value; }
		}

		public BaseActivation[] key { get { return m_key; } set { m_key = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public PeerlessList Peerless
		{
			get { return m_Peerless; }
			set
			{
				m_Peerless = value;

				if ( m_Peerless == PeerlessList.ParoxysmusTrammel || m_Peerless == PeerlessList.ParoxysmusFelucca )
				{
					Name = "Cauldron";
					Hue = 1125;
					ItemID = 0x207A;
				}
				else if ( m_Peerless == PeerlessList.MelisandeTrammel || m_Peerless == PeerlessList.MelisandeFelucca )
				{
					Name = "Basket";
					Hue = 0;
					ItemID = 0x207B;
				}
				else if ( m_Peerless == PeerlessList.DreadHorn )
				{
					Name = "Statue Of The Faie";
					Hue = 0;
					ItemID = 0x207C;
				}
				else if ( m_Peerless == PeerlessList.Travesty || m_Peerless == PeerlessList.InterredGrizzle )
				{
					Name = "Keyed Table";
					Hue = 0;
					ItemID = 0x207E;
				}
				else if ( m_Peerless == PeerlessList.ShimmeringEffusionTrammel || m_Peerless == PeerlessList.ShimmeringEffusionFelucca )
				{
					Name = "Pillar";
					Hue = 1153;
					ItemID = 8317;
				}
				else
				{
					Name = null;
					Hue = 0;
					ItemID = 0x9AB;
				}

				if ( m_Peerless != PeerlessList.None )
					UpdateRegion();
			}
		}


		public Timer PeerlessT
		{
			get { return m_PeerlessTimer; }
			set { m_PeerlessTimer = value; }
		}

		public Timer clearT
		{
			get { return m_ClearTimer; }
			set { m_ClearTimer = value; }
		}

		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		private readonly Type[] m_Keys = new Type[6];

		[Constructable]
		public AltarPeerless()
			: base( 0x9AB )
		{
			Movable = false;
			DropSound = 0x48;
		}

		public AltarPeerless( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
			writer.Write( (int) m_Peerless );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Peerless = (PeerlessList) reader.ReadInt();

			if ( m_Peerless != PeerlessList.None )
			{
				UpdateRegion();
				m_Region.Register();
			}
		}

		public override bool DisplayWeight { get { return false; } }

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !base.OnDragDrop( from, dropped ) || m_Peerless == PeerlessList.None )
			{
				return false;
			}

			if ( PeerlessEntry.IsPeerlessKey( m_Peerless, dropped ) )
			{
				if ( m_Owner != from && m_Activated )
				{
					if ( Boss != null && Boss.CheckAlive() )
						from.SendLocalizedMessage( 1075213 ); // The master of this realm has already been summoned and is engaged in combat.  Your opportunity will come after he has squashed the current batch of intruders!
					else
						from.SendLocalizedMessage( 1072683, m_Owner.Name ); // ~1_NAME~ has already activated the Prism, please wait...

					return false;
				}

				for ( int i = 0; i < m_Keys.Length; i++ )
				{
					if ( m_Keys[i] == dropped.GetType() )
					{
						from.SendLocalizedMessage( 1072682 ); // This is not the proper key.
						return false;
					}

					if ( m_Keys[i] == null )
					{
						m_Keys[i] = dropped.GetType();

						if ( i == 0 )
						{
							m_Activated = true;
							m_Owner = from;
							from.SendLocalizedMessage( 1074575 ); // You have activated this object!
							m_ResetTimer = new ResetTimer( this );
							m_ResetTimer.Start();
						}

						if ( PeerlessEntry.GetAltarKeys( m_Peerless ) == ( i + 1 ) )
						{
							m_ResetTimer.Stop();
							from.SendLocalizedMessage( 1072678 ); // You have awakened the master of this realm. You need to hurry to defeat it in time!

							GiveKeys( from );

							Mobile boss = Activator.CreateInstance( PeerlessEntry.GetBoss( m_Peerless ) ) as Mobile;
							m_Boss = boss;
							boss.MoveToWorld( PeerlessEntry.GetSpawnPoint( m_Peerless ), PeerlessEntry.GetMap( m_Peerless ) );
							boss.OnBeforeSpawn( boss.Location, boss.Map );

							PeerlessT = new PeerlessTimer( this );
							PeerlessT.Start();
						}

						dropped.Delete();
						return true;
					}
				}

				from.SendLocalizedMessage( 1072682 ); // This is not the proper key.
				return false;
			}

			from.SendLocalizedMessage( 1072682 ); // This is not the proper key.
			return false;
		}

		public void UpdateRegion()
		{
			if ( m_Region != null )
				m_Region.Unregister();

			string regionName;
			Map regionMap;
			List<Rectangle2D> regionBounds = new List<Rectangle2D>();

			switch ( m_Peerless )
			{
				default:
				case PeerlessList.DreadHorn:
					{
						regionName = "DreadHorn";
						regionMap = Map.Ilshenar;

						regionBounds.Add( new Rectangle2D( 2126, 1237, 22, 22 ) );
						regionBounds.Add( new Rectangle2D( 2148, 1238, 4, 21 ) );
						regionBounds.Add( new Rectangle2D( 2152, 1246, 6, 13 ) );
						regionBounds.Add( new Rectangle2D( 2130, 1259, 27, 4 ) );
						regionBounds.Add( new Rectangle2D( 2135, 1263, 21, 6 ) );
						regionBounds.Add( new Rectangle2D( 2137, 1269, 18, 5 ) );
						regionBounds.Add( new Rectangle2D( 2157, 1253, 4, 8 ) );

						break;
					}
				case PeerlessList.MelisandeFelucca:
					{
						regionName = "MelisandeFelucca";
						regionMap = Map.Felucca;

						regionBounds.Add( new Rectangle2D( 6456, 922, 86, 44 ) );

						break;
					}
				case PeerlessList.MelisandeTrammel:
					{
						regionName = "MelisandeTrammel";
						regionMap = Map.Trammel;

						regionBounds.Add( new Rectangle2D( 6456, 922, 86, 44 ) );

						break;
					}
				case PeerlessList.Travesty:
					{
						regionName = "Travesty";
						regionMap = Map.Malas;

						regionBounds.Add( new Rectangle2D( new Point2D( 64, 1933 ), new Point2D( 117, 1978 ) ) );

						break;
					}
				case PeerlessList.ParoxysmusTrammel:
					{
						regionName = "ParoxysmusTrammel";
						regionMap = Map.Trammel;

						regionBounds.Add( new Rectangle2D( new Point2D( 6486, 335 ), new Point2D( 6552, 398 ) ) );

						break;
					}
				case PeerlessList.ParoxysmusFelucca:
					{
						regionName = "ParoxysmusFelucca";
						regionMap = Map.Felucca;

						regionBounds.Add( new Rectangle2D( new Point2D( 6486, 335 ), new Point2D( 6552, 398 ) ) );

						break;
					}
				case PeerlessList.InterredGrizzle:
					{
						regionName = "InterredGrizzle";
						regionMap = Map.Malas;

						regionBounds.Add( new Rectangle2D( new Point2D( 148, 1721 ), new Point2D( 198, 1765 ) ) );

						break;
					}
				case PeerlessList.ShimmeringEffusionTrammel:
					{
						regionName = "ShimmeringEffusionTrammel";
						regionMap = Map.Trammel;

						regionBounds.Add( new Rectangle2D( new Point2D( 6499, 111 ), new Point2D( 6545, 145 ) ) );

						break;
					}
				case PeerlessList.ShimmeringEffusionFelucca:
					{
						regionName = "ShimmeringEffusionFelucca";
						regionMap = Map.Trammel;

						regionBounds.Add( new Rectangle2D( new Point2D( 6499, 111 ), new Point2D( 6545, 145 ) ) );

						break;
					}
			}

			m_Region = new PeerlessRegion( regionName, regionMap, this, regionBounds.ToArray() );
		}

		public void Clear()
		{
			for ( int i = 0; i < m_Keys.Length; i++ )
				m_Keys[i] = null;

			Activated = false;
		}

		public void GiveKeys( Mobile from )
		{
			if ( m_Peerless != PeerlessList.None )
			{
				for ( int i = 0; i < m_key.Length; i++ )
				{
					if ( m_Peerless == PeerlessList.DreadHorn )
						m_key[i] = new DreadHornActivation();
					else if ( m_Peerless == PeerlessList.ParoxysmusFelucca )
						m_key[i] = new SlimyOintmentFelucca();
					else if ( m_Peerless == PeerlessList.ParoxysmusTrammel )
						m_key[i] = new SlimyOintmentTrammel();
					else if ( m_Peerless == PeerlessList.MelisandeTrammel )
						m_key[i] = new MelisandeActivationTrammel();
					else if ( m_Peerless == PeerlessList.MelisandeFelucca )
						m_key[i] = new MelisandeActivationFelucca();
					else if ( m_Peerless == PeerlessList.Travesty )
						m_key[i] = new BlackOrderKey();
					else if ( m_Peerless == PeerlessList.InterredGrizzle )
						m_key[i] = new MasterKey();
					else if ( m_Peerless == PeerlessList.ShimmeringEffusionTrammel )
						m_key[i] = new ShimmeringEffusionActivationTrammel();
					else if ( m_Peerless == PeerlessList.ShimmeringEffusionFelucca )
						m_key[i] = new ShimmeringEffusionActivationFelucca();


					from.AddToBackpack( m_key[i] );
					from.SendLocalizedMessage( 1072680 ); // You have been given the key to the boss.
				}
			}
		}

		public void DeleteKeys( String name )
		{
			Effects.SendLocationParticles( EffectItem.Create( m_Owner.Location, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 2100, 3, 5042, 0 );

			m_Owner.PlaySound( 0x201 );

			m_Owner.CloseGump( typeof( BaseActivation.ConfirmPeerlessGump ) );
			m_Owner.CloseGump( typeof( BaseActivation.ConfirmPeerlessPartyGump ) );

			for ( int i = 0; i < m_key.Length; i++ )
			{
				if ( m_key[i] != null && !m_key[i].Deleted )
				{
					if ( name != null )
						m_Owner.SendLocalizedMessage( 1072515, name );

					m_key[i].Delete();
					m_key[i] = null;
				}
			}
		}

		public override void OnDoubleClick( Mobile m )
		{
		}

		private class ResetTimer : Timer
		{
			private AltarPeerless m_Altar;

			public ResetTimer( AltarPeerless altar )
				: base( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 1.0 ), 1 )
			{
				m_Altar = altar;
			}

			protected override void OnTick()
			{
				m_Altar.Owner.SendLocalizedMessage( 1072679 ); // You realm offering has reset. You will need to start over.
				m_Altar.Clear();
				Stop();
			}
		}

		private class PeerlessTimer : Timer
		{
			private AltarPeerless m_Altar;
			private int cont = 1800;

			public PeerlessTimer( AltarPeerless altar )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Altar = altar;
			}

			protected override void OnTick()
			{
				if ( m_Altar.Boss == null || m_Altar.Boss.Deleted || !m_Altar.Boss.CheckAlive() )
				{
					m_Altar.DeleteKeys( PeerlessEntry.GetLabelNum( m_Altar.Peerless ) );

					m_Altar.clearT = new ClearTimer( m_Altar );
					m_Altar.clearT.Start();

					Stop();
				}

				if ( cont <= 0 )
				{
					PeerlessRegion reg = m_Altar.Region;

					if ( reg != null )
						reg.KickAll( m_Altar.Peerless );

					m_Altar.Clear();
					m_Altar.DeleteKeys( PeerlessEntry.GetLabelNum( m_Altar.Peerless ) );

					if ( m_Altar.Boss != null && !m_Altar.Boss.Deleted )
						m_Altar.Boss.Delete();

					Stop();
				}

				cont--;
			}
		}

		private class ClearTimer : Timer
		{
			private readonly AltarPeerless m_Altar;

			public ClearTimer( AltarPeerless altar )
				: base( TimeSpan.FromMinutes( 2.0 ) )
			{
				m_Altar = altar;

				PeerlessRegion region = m_Altar.Region;

				if ( region != null )
					region.BroadcastLocalizedMessage( 1072681 ); // The master of this realm has been slain! You may only stay here so long.
			}

			protected override void OnTick()
			{
				PeerlessRegion region = m_Altar.Region;

				if ( region != null )
					region.KickAll( m_Altar.Peerless );

				m_Altar.Clear();
				m_Altar.DeleteKeys( PeerlessEntry.GetLabelNum( m_Altar.Peerless ) );

				Stop();
			}
		}
	}
}
