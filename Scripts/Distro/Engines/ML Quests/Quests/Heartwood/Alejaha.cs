using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class ItsElementalQuest : BaseQuest
	{
		/* It's Elemental */
		public override object Title { get { return 1073089; } }

		/* The universe is all about balance my friend. Tip one end, you must balance the other. That's 
		why I must ask you to kill not just one kind of elemental, but three kinds. Snuff out some Fire, 
		douse a few Water, and crush some Earth elementals and I'll pay you for your trouble. */
		public override object Description { get { return 1073579; } }

		/* I hope you'll reconsider. Until then, farwell.	 */
		public override object Refuse { get { return 1073580; } }

		/* Four of each, that's all I ask. Water, earth and fire. */
		public override object Uncomplete { get { return 1073599; } }

		public ItsElementalQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( FireElemental ), "fire elementals", 4 ) );
			AddObjective( new SlayObjective( typeof( WaterElemental ), "water elementals", 4 ) );
			AddObjective( new SlayObjective( typeof( EarthElemental ), "earth elementals", 4 ) );

			AddReward( new BaseReward( typeof( TreasureBag ), 1072583 ) );
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

	public class Alejaha : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[]
					{
						typeof( ItsElementalQuest )
					};
			}
		}

		[Constructable]
		public Alejaha()
			: base( "Elder Alejaha", "the wise" )
		{
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}

		public Alejaha( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Elf;

			Hue = 0x8361;
			HairItemID = 0x2FCD;
			HairHue = 0x852;
		}

		public override void InitOutfit()
		{
			AddItem( new Sandals( 0x1BB ) );
			AddItem( new Cloak( 0x59 ) );
			AddItem( new Skirt( 0x901 ) );
			AddItem( new GemmedCirclet() );
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