using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class UntanglingTheWebQuest : BaseQuest
	{
		/* Untangling the Web */
		public override object Title { get { return 1095050; } }

		public override QuestChain ChainID { get { return QuestChain.VernixChain; } }
		public override bool DoneOnce { get { return true; } }
		public override Type NextQuest { get { return typeof( GreenWithEnvyQuest ); } }

		/* Kill Acid Slugs and Acid Elementals to fill Vernix's jars.  Return to
		 * Vernix with the filled jars for your reward.
		 * 
		 * -----
		 * 
		 * Vernix say, stranger has proven big power.  You now ready to help Green
		 * Goblins big time.  Green Gobin and outsider not need to be enemy.  Need
		 * to be friend against common enemy.  You help Green Goblins with important
		 * mission.  We tell you important information.  Help your people not be eaten.
		 * 
		 * Go find acid slugs and acid elementals.  They very dangerous, but me think
		 * you can handle it.  Fill these jars with acid from these.
		 */
		public override object Description { get { return 1095052; } }

		/* Hmm... Perhaps you are afraid.  Hmm... Very good to know.  Ok, you go and do.
		 * You come back.  Let me know if you stop being afraid of acid.
		 */
		public override object Refuse { get { return 1095053; } }

		/* Acid very important to stopping master plan of Gray Goblins.  You get acid, ok?
		 */
		public override object Uncomplete { get { return 1095054; } }

		/* This very good.  Now King Vernix tell you valuable secret.  Acid good for
		 * melting wolf spider webs.  Webs very strong, but not stronger than acid.
		 * Vernix gives to you pay for good work.
		 */
		public override object Complete { get { return 1095057; } }

		/* Vernix's Jars are now full. */
		public override int CompleteMessage { get { return 1095056; } }

		public UntanglingTheWebQuest()
		{
			AddObjective( new InternalObjective() );

			AddReward( new BaseReward( 1095058 ) ); // Acid Popper
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Owner.SendLocalizedMessage( 1074360, "#1095058" ); // You receive a reward: Acid Popper
			Owner.AddToBackpack( new AcidPooper() );
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

		private class InternalObjective : SlayObjective
		{
			public InternalObjective()
				: base( typeof( AcidSlug ), "Acid Creatures", 12, "Underworld" )
			{
			}

			public override void OnKill( Mobile killed )
			{
				base.OnKill( killed );

				// You collect acid from the creature into the jar.
				Quest.Owner.SendLocalizedMessage( 1095055 );
			}
		}
	}

	public class GreenWithEnvyQuest : BaseQuest
	{
		/* Green with Envy */
		public override object Title { get { return 1095118; } }

		public override QuestChain ChainID { get { return QuestChain.VernixChain; } }
		public override bool DoneOnce { get { return true; } }

		/* Kill the queen spider, Navrey Night-Eyes. Bring proof of her death to
		 * King Vernix.
		 * 
		 * -----
		 * 
		 * Vernix plan is now ready. Man thing from outside make good champion for
		 * Green Goblins. King Vernix will let him in on plan. Gray goblin power
		 * comes from their god, Navery Night-Eyes. Navery is great spider. Very
		 * nasty. Gray Goblins and Green Goblins used to be one tribe, but Gray
		 * Goblins gain power from Navery and make Green Goblins slaves. Green
		 * Goblins escape tribe and find own place.
		 * 
		 * Navery Night-Eyes has big hunger. Always need more blood. Gray Goblins
		 * want to sacrifice outside kind to Navery so she not eat them. You kill
		 * Navery, you solve big problem for outside kind and goblin kind.
		 * 
		 * If you do this, you take big risk so Vernix make it worth your while. 
		 * Kill Navery Night-Eyes. */
		public override object Description { get { return 1095120; } }

		/* You no kill her, then many outside people disappear at night when others sleep. 
		 * You think about it, then come back when you ready to make deal. */
		public override object Refuse { get { return 1095121; } }

		/* You not have much time. Navery Night-Eyes is hungry. */
		public override object Uncomplete { get { return 1095122; } }

		/* You do good service to your people. Now Green Goblins will do the rest.
		 * Without power from Navery Night-Eyes, we will have our revenge. Vernix
		 * keep Green Goblin end of deal. */
		public override object Complete { get { return 1095123; } }

		public GreenWithEnvyQuest()
		{
			AddObjective( new ObtainObjective( typeof( EyeOfNavrey ), "Eye of Navrey Night-Eyes", 1 ) );

			AddReward( new BaseReward( typeof( LargeTreasureBag ), 1072706 ) );
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

		private class InternalObjective : SlayObjective
		{
			public InternalObjective()
				: base( new Type[] { typeof( Bloodworm ), typeof( BloodElemental ) }, "Blood Creatures", 12, "Underworld" )
			{
			}

			public override void OnKill( Mobile killed )
			{
				base.OnKill( killed );

				// Blood from the creature goes into Jaacar’s barrel.
				Quest.Owner.SendLocalizedMessage( 1095037 );
			}
		}
	}

	public class Vernix : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( UntanglingTheWebQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
			Say( 1095119 ); // The plan is ready! The time has come!
		}

		[Constructable]
		public Vernix()
			: base( "Vernix" )
		{
		}

		public Vernix( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Body = 723;

			CantWalk = true;
		}

		public override void InitOutfit()
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

			/*int version = */
			reader.ReadInt();
		}
	}
}