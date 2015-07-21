using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class InTheWeedsQuest : BaseQuest
	{
		/* In the Weeds */
		public override object Title { get { return 1113499; } }

		/* Help Farmer Nash find his pitchfork by pulling the weeds in his garden until you uncover it.
		 * (Pull weeds by double clicking them)  When you find it, return it to him for your reward.
		 * <br><center>-----</center><br>I hate to trouble you, but sometimes a problem needs a plow
		 * and sometimes it needs a sword.  I am good with a plow, but terrible with a sword.<br><br>
		 * I have been plagued with a strange weed for some time.  Every day I have to clean them out
		 * of my garden and carry them away with my pitch fork.  Yesterday I was working there and…
		 * well, I must have nodded off because when I woke my pitchfork was gone!<br><br>I have heard
		 * talk of thieves who seek treasure in the sacred tomb, but I really don’t think they took my
		 * pitchfork, in fact I think it just got lost in the weeds!<br><br>I would find it myself, but
		 * now that we are so close to the edge of the world, many wild creatures are lurking about and
		 * some might be hiding in these weeds.  I’ve seen the creatures that have been roaming these
		 * parts recently and I fear for my life!  The problem is, if I don’t get my crop in the ground
		 * soon, we won’t make it through the winter.  Will you help? */
		public override object Description { get { return 1113500; } }

		/* I understand.  I certainly don’t want you to do something you don’t want to do. */
		public override object Refuse { get { return 1113501; } }

		/* Did you find my pitchfork?  I'm sure it is under those weeds somewhere.  It was a gift from
		 * King Draxinusom when he assigned me this job, I can’t bear to lose it! */
		public override object Uncomplete { get { return 1113502; } }

		/* Oh, thank you!  Here is your reward as promised.  I will get right back to work in a few
		 * minutes. */
		public override object Complete { get { return 1113503; } }

		private bool m_GivenPitchfork;

		public bool GivenPitchfork { get { return m_GivenPitchfork; } set { m_GivenPitchfork = value; } }

		public InTheWeedsQuest()
		{
			AddObjective( new ObtainObjective( typeof( FarmerNashPitchfork ), "Farmer Nash's Pitchfork", 1 ) );

			AddReward( new BaseReward( 1072583 ) );
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Bag bag = new Bag();
			bag.Hue = BaseReward.RewardBagHue();

			for ( int i = 0; i < 2; i++ )
			{
				Item gem = Loot.RandomGem();
				gem.Amount = 5;
				bag.DropItem( gem );
			}

			for ( int i = 0; i < 3; i++ )
			{
				bag.DropItem( BaseReward.RandomItem( Utility.RandomMinMax( 1, 3 ), 10, 70 ) );
			}

			Owner.SendLocalizedMessage( 1074360, "#1023702" ); // You receive a reward: bag
			Owner.AddToBackpack( bag );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_GivenPitchfork );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_GivenPitchfork = reader.ReadBool();
		}
	}

	public class FarmerNash : MondainQuester
	{
		private static Type[] m_Quests = new Type[] { typeof( InTheWeedsQuest ) };
		public override Type[] Quests { get { return m_Quests; } }

		public override bool IsActiveVendor { get { return false; } }

		[Constructable]
		public FarmerNash()
			: base( "Farmer Nash" )
		{
		}

		public FarmerNash( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
			Say( 1113507 ); // To have work to do!  To not want to be eaten!
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Gargoyle;

			Hue = 0x86E8;
			HairItemID = 0x425A;
			HairHue = 0x385;
		}

		public override void InitOutfit()
		{
			AddItem( new GargishClothKilt( 0x592 ) );
			AddItem( new GargishClothChest( 0x5A6 ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
