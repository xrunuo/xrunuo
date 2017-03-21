//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using Server.Network;

namespace Server
{
	public enum EffectLayer
	{
		Head = 0,
		RightHand = 1,
		LeftHand = 2,
		Waist = 3,
		LeftFoot = 4,
		RightFoot = 5,
		CenterFeet = 7
	}

	public enum ParticleSupportType
	{
		Full,
		Detect,
		None
	}

	public class Effects
	{
		private static ParticleSupportType m_ParticleSupportType = ParticleSupportType.Full;

		public static ParticleSupportType ParticleSupportType
		{
			get { return m_ParticleSupportType; }
			set { m_ParticleSupportType = value; }
		}

		public static bool SendParticlesTo( NetState state )
		{
			// TODO: Does current 2d client support particle effects packets?
			return ( m_ParticleSupportType == ParticleSupportType.Full || ( m_ParticleSupportType == ParticleSupportType.Detect && state.Version.IsEnhanced ) );
		}

		public static void PlaySound( IPoint3D p, Map map, int soundId )
		{
			if ( soundId <= -1 )
				return;

			if ( map != null )
			{
				Packet playSound = null;

				foreach ( NetState state in map.GetClientsInRange( p ) )
				{
					state.Mobile.ProcessDelta();

					if ( playSound == null )
						playSound = Packet.Acquire( GenericPackets.PlaySound( soundId, p ) );

					state.Send( playSound );
				}

				Packet.Release( playSound );
			}
		}

		public static void SendBoltEffect( IEntity e, bool sound = true, int hue = 0 )
		{
			IMap map = e.Map;

			if ( map == null )
				return;

			if ( e is Item )
				( (Item) e ).ProcessDelta();
			else if ( e is Mobile )
				( (Mobile) e ).ProcessDelta();

			Packet preEffect = null, boltEffect = null, playSound = null;

			foreach ( NetState state in map.GetClientsInRange( e.Location ) )
			{
				if ( state.Mobile.CanSee( e ) )
				{
					if ( SendParticlesTo( state ) )
					{
						if ( preEffect == null )
							preEffect = Packet.Acquire( new TargetParticleEffect( e, 0, 10, 5, 0, 0, 5031, 3, 0 ) );

						state.Send( preEffect );
					}

					if ( boltEffect == null )
						boltEffect = Packet.Acquire( new BoltEffect( e, hue ) );

					state.Send( boltEffect );

					if ( sound )
					{
						if ( playSound == null )
							playSound = Packet.Acquire( GenericPackets.PlaySound( 0x29, e ) );

						state.Send( playSound );
					}
				}
			}

			Packet.Release( preEffect );
			Packet.Release( boltEffect );
			Packet.Release( playSound );
		}

		public static void SendLocationEffect( IPoint3D p, Map map, int itemID, int duration )
		{
			SendLocationEffect( p, map, itemID, duration, 10, 0, 0 );
		}

		public static void SendLocationEffect( IPoint3D p, Map map, int itemID, int duration, int speed )
		{
			SendLocationEffect( p, map, itemID, duration, speed, 0, 0 );
		}

		public static void SendLocationEffect( IPoint3D p, Map map, int itemID, int duration, int hue, int renderMode )
		{
			SendLocationEffect( p, map, itemID, duration, 10, hue, renderMode );
		}

		public static void SendLocationEffect( IPoint3D p, Map map, int itemID, int duration, int speed, int hue, int renderMode )
		{
			SendPacket( p, map, new LocationEffect( p, itemID, speed, duration, hue, renderMode ) );
		}

		public static void SendLocationParticles( IEntity e, int itemID, int speed, int duration, int effect )
		{
			SendLocationParticles( e, itemID, speed, duration, 0, 0, effect, 0 );
		}

		public static void SendLocationParticles( IEntity e, int itemID, int speed, int duration, int effect, int unknown )
		{
			SendLocationParticles( e, itemID, speed, duration, 0, 0, effect, unknown );
		}

		public static void SendLocationParticles( IEntity e, int itemID, int speed, int duration, int hue, int renderMode, int effect, int unknown )
		{
			IMap map = e.Map;

			if ( map != null )
			{
				Packet particles = null, regular = null;

				foreach ( NetState state in map.GetClientsInRange( e.Location ) )
				{
					state.Mobile.ProcessDelta();

					if ( SendParticlesTo( state ) )
					{
						if ( particles == null )
							particles = Packet.Acquire( new LocationParticleEffect( e, itemID, speed, duration, hue, renderMode, effect, unknown ) );

						state.Send( particles );
					}
					else if ( itemID != 0 )
					{
						if ( regular == null )
							regular = Packet.Acquire( new LocationEffect( e, itemID, speed, duration, hue, renderMode ) );

						state.Send( regular );
					}
				}

				Packet.Release( particles );
				Packet.Release( regular );
			}
		}

		public static void SendTargetEffect( IEntity target, int itemID, int duration )
		{
			SendTargetEffect( target, itemID, duration, 0, 0 );
		}

		public static void SendTargetEffect( IEntity target, int itemID, int speed, int duration )
		{
			SendTargetEffect( target, itemID, speed, duration, 0, 0 );
		}

		public static void SendTargetEffect( IEntity target, int itemID, int duration, int hue, int renderMode )
		{
			SendTargetEffect( target, itemID, 10, duration, hue, renderMode );
		}

		public static void SendTargetEffect( IEntity target, int itemID, int speed, int duration, int hue, int renderMode )
		{
			if ( target is Mobile )
				( (Mobile) target ).ProcessDelta();

			SendPacket( target.Location, target.Map, new TargetEffect( target, itemID, speed, duration, hue, renderMode ) );
		}

		public static void SendTargetParticles( IEntity target, int itemID, int speed, int duration, int effect, EffectLayer layer )
		{
			SendTargetParticles( target, itemID, speed, duration, 0, 0, effect, layer, 0 );
		}

		public static void SendTargetParticles( IEntity target, int itemID, int speed, int duration, int effect, EffectLayer layer, int unknown )
		{
			SendTargetParticles( target, itemID, speed, duration, 0, 0, effect, layer, unknown );
		}

		public static void SendTargetParticles( IEntity target, int itemID, int speed, int duration, int hue, int renderMode, int effect, EffectLayer layer, int unknown )
		{
			if ( target is Mobile )
				( (Mobile) target ).ProcessDelta();

			IMap map = target.Map;

			if ( map != null )
			{
				Packet particles = null, regular = null;

				foreach ( NetState state in map.GetClientsInRange( target.Location ) )
				{
					state.Mobile.ProcessDelta();

					if ( SendParticlesTo( state ) )
					{
						if ( particles == null )
							particles = Packet.Acquire( new TargetParticleEffect( target, itemID, speed, duration, hue, renderMode, effect, (int) layer, unknown ) );

						state.Send( particles );
					}
					else if ( itemID != 0 )
					{
						if ( regular == null )
							regular = Packet.Acquire( new TargetEffect( target, itemID, speed, duration, hue, renderMode ) );

						state.Send( regular );
					}
				}

				Packet.Release( particles );
				Packet.Release( regular );
			}
		}

		public static void SendMovingEffect( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes )
		{
			SendMovingEffect( from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0 );
		}

		public static void SendMovingEffect( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode )
		{
			if ( from is Mobile )
				( (Mobile) from ).ProcessDelta();

			if ( to is Mobile )
				( (Mobile) to ).ProcessDelta();

			SendPacket( from.Location, from.Map, new MovingEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode ) );
		}

		public static void SendMovingParticles( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound )
		{
			SendMovingParticles( from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0, effect, explodeEffect, explodeSound, 0 );
		}

		public static void SendMovingParticles( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound, int unknown )
		{
			SendMovingParticles( from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0, effect, explodeEffect, explodeSound, unknown );
		}

		public static void SendMovingParticles( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, int unknown )
		{
			SendMovingParticles( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, (EffectLayer) 255, unknown );
		}

		public static void SendMovingParticles( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
		{
			if ( from is Mobile )
				( (Mobile) from ).ProcessDelta();

			if ( to is Mobile )
				( (Mobile) to ).ProcessDelta();

			IMap map = from.Map;

			if ( map != null )
			{
				Packet particles = null, regular = null;

				foreach ( NetState state in map.GetClientsInRange( from.Location ) )
				{
					state.Mobile.ProcessDelta();

					if ( SendParticlesTo( state ) )
					{
						if ( particles == null )
							particles = Packet.Acquire( new MovingParticleEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, layer, unknown ) );

						state.Send( particles );
					}
					else if ( itemID > 1 )
					{
						if ( regular == null )
							regular = Packet.Acquire( new MovingEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode ) );

						state.Send( regular );
					}
				}

				Packet.Release( particles );
				Packet.Release( regular );
			}
		}

		public static void SendPacket( IPoint3D origin, IMap map, Packet p )
		{
			if ( map != null )
			{
				p.Acquire();

				foreach ( NetState state in map.GetClientsInRange( origin ) )
				{
					state.Mobile.ProcessDelta();

					state.Send( p );
				}

				if ( p != null )
					p.Release();
			}
		}
	}
}