using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class SwiftAsAnArrowQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		/* Swift as an Arrow */
		public override object Title { get { return 1078201; } }

		/* Head East out of town and go to Old Haven. While wielding your bow or crossbow, battle monster there until you have 
		raised your Archery skill to 50. Well met, friend. Imagine yourself in a distant grove of trees, You raise your bow, 
		take slow, careful aim, and with the twitch of a finger, you impale your prey with a deadly arrow. You look like you 
		would make a excellent archer, but you will need practice. There is no better way to practice Archery than when you 
		life is on the line. I have a challenge for you. Head East out of town and go to Old Haven. While wielding your bow 
		or crossbow, battle the undead that reside there. Make sure you bring a healthy supply of arrows (or bolts if you 
		prefer a crossbow). If you wish to purchase a bow, crossbow, arrows, or bolts, you can purchase them from me or the 
		Archery shop in town. You can also make your own arrows with the Bowcraft/Fletching skill. You will need fletcher's 
		tools, wood to turn into sharft's, and feathers to make arrows or bolts. Come back to me after you have achived the 
		rank of Apprentice Archer, and i will reward you with a fine Archery weapon. */
		public override object Description { get { return 1078205; } }

		/* I understand that Archery may not be for you. Feel free to visit me in the future if you change your mind. */
		public override object Refuse { get { return 1078206; } }

		/* You're doing great as an Archer! however, you need more practice. Head East out of town and go to Old Haven. come 
		back to me after you have acived the rank of Apprentice Archer. */
		public override object Uncomplete { get { return 1078207; } }

		/* Congratulation! I want to reward you for your accomplishment. Take this composite bow. It is called " Heartseeker". 
		With it, you will shoot with swiftness, precision, and power. I hope "Heartseeker" serves you well. */
		public override object Complete { get { return 1078209; } }

		public SwiftAsAnArrowQuest()
			: base()
		{
			AddObjective( new ApprenticeObjective( SkillName.Archery, 50, "Old Haven Training", 1078203, 1078204 ) );

			// 1078203 You feel more steady and dexterous here. Your Archery skill is enhanced in this area.
			// 1078204 You feel less steady and dexterous here. Your Archery learning potential is no longer enhanced.

			AddReward( new BaseReward( typeof( Heartseeker ), 1078210 ) );
		}

		public override bool CanOffer()
		{
			return Owner.Skills.Archery.Base < 50;
		}

		public override void OnCompleted()
		{
			Owner.SendLocalizedMessage( 1078208, null, 0x23 ); // You have achieved the rank of Apprentice Archer. Return to Robyn in New Haven to claim your reward.
			Owner.PlaySound( CompleteSound );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class Robyn : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( SwiftAsAnArrowQuest )
			};
			}
		}

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBRanger() );
		}

		[Constructable]
		public Robyn()
			: base( "Robyn", "The Archer Instructor" )
		{
			SetSkill( SkillName.Anatomy, 65.0, 90.0 );
			SetSkill( SkillName.Parry, 65.0, 90.0 );
			SetSkill( SkillName.Fletching, 65.0, 90.0 );
			SetSkill( SkillName.Healing, 65.0, 90.0 );
			SetSkill( SkillName.Tactics, 65.0, 90.0 );
			SetSkill( SkillName.Archery, 65.0, 90.0 );
			SetSkill( SkillName.Focus, 65.0, 90.0 );
		}

		public Robyn( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
			Say( 1078202 ); // Archery requires a steady aim and dexterous fingers.
		}

		public override void OnOfferFailed()
		{
			Say( 1077772 ); // I cannot teach you, for you know all I can teach!
		}

		public override void InitBody()
		{
			Female = true;
			CantWalk = true;
			Race = Race.Human;

			base.InitBody();
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Boots( 0x592 ) );
			AddItem( new Cloak( 0x592 ) );
			AddItem( new Bandana( 0x592 ) );
			AddItem( new CompositeBow() );

			Item item;

			item = new StuddedLegs();
			item.Hue = 0x592;
			AddItem( item );

			item = new StuddedGloves();
			item.Hue = 0x592;
			AddItem( item );

			item = new StuddedGorget();
			item.Hue = 0x592;
			AddItem( item );

			item = new StuddedChest();
			item.Hue = 0x592;
			AddItem( item );

			item = new StuddedArms();
			item.Hue = 0x592;
			AddItem( item );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}