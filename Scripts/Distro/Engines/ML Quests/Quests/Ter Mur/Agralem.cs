using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class IntoTheVoidQuest : BaseQuest
	{
		/* Into the Void */
		public override object Title { get { return 1112687; } }

		/* Kill Void Daemons and return to Agralem the Bladeweaver for your reward.
		 * -----
		 * Welcome to the Royal City. My name is Agralem. I am a Bladeweaver.
		 * Ter-Mur needs your help. Daemons are invading our lands from the Void.
		 * The Void Daemons must be slain. Our existance is in great danger. Please
		 * help us protect Ter-Mur, and I will reward you. */
		public override object Description { get { return 1112690; } }

		/* I understand what I ask is a perilous task. I hope someone brave will help us. */
		public override object Refuse { get { return 1112691; } }

		/* Welcome back. I must remind you that we are still in great danger. Please
		 * help protect Ter-Mur from the Void Daemons. */
		public override object Uncomplete { get { return 1112692; } }

		/* Thank you for helping keep us safe! We are very grateful. As promised, here
		 * is your reward. */
		public override object Complete { get { return 1112693; } }

		public IntoTheVoidQuest()
		{
			AddObjective( new SlayObjective( typeof( VoidCreature ), "Void Daemons", 10 ) );

			AddReward( new BaseReward( typeof( AbyssReaver ), 1112694 ) ); // Abyss Reaver
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

	public class Agralem : MondainQuester
	{
		private static Type[] m_Quests = new Type[] { typeof( IntoTheVoidQuest ) };
		public override Type[] Quests { get { return m_Quests; } }

		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBBladeweaver() );
		}

		[Constructable]
		public Agralem()
			: base( "Agralem", "the Bladeweaver" )
		{
			SetSkill( SkillName.Anatomy, 65.0, 90.0 );
			SetSkill( SkillName.MagicResist, 65.0, 90.0 );
			SetSkill( SkillName.Tactics, 65.0, 90.0 );
			SetSkill( SkillName.Throwing, 65.0, 90.0 );
		}

		public Agralem( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
			Say( 1112688 ); // Daemons from the void! They must be vanquished!
		}

		public override void InitBody()
		{
			Female = false;
			CantWalk = true;
			Race = Race.Gargoyle;

			Hue = 0x86ED;
			HairItemID = 0x425D;
			HairHue = 0x31D;
		}

		public override void InitOutfit()
		{
			AddItem( new Cyclone() );
			AddItem( new GargishLeatherKilt( 0x901 ) );
			AddItem( new GargishLeatherChest( 0x901 ) );
			AddItem( new GargishLeatherArms( 0x901 ) );
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
