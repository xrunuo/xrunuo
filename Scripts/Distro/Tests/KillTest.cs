using Server.Mobiles;
using Server.Testing;

namespace Server.Tests
{
	[TestCase]
	public class KillTest
	{
		[Test]
		public void TestDamageLessPointsThanCurrentHitPointsShouldNotKillCreature()
		{
			Mobile mobile = new TestMobile();
			mobile.Str = 100;
			mobile.Hits = mobile.HitsMax;
			mobile.Damage(1);

			Assert.True(mobile.Alive);
		}

		[Test]
		public void TestDamageMorePointsThanCurrentHitPointsShouldKillCreature()
		{
			Mobile mobile = new TestMobile();
			mobile.Str = 100;
			mobile.Hits = mobile.HitsMax;
			mobile.Damage(100000);

			Assert.False(mobile.Alive);
		}
	}

	public class TestMobile : Mobile
	{
	}
}
