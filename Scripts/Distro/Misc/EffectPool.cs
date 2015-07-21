using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Misc
{
	public class EffectPool
	{
		public static void ArtifactDrop( Mobile m )
		{
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( m.Location, m.Map, 0x243 );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 4, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 4, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

			Effects.SendTargetParticles( m, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );
		}
	}
}