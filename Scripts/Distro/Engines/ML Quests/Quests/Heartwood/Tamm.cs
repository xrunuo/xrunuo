using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Tamm : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( TheyreBreedingLikeRabbitsQuest ),
				typeof( ThinningTheHerdQuest ),
				typeof( TheyllEatAnythingQuest ),
				typeof( NoGoodFishStealingQuest ),
				typeof( HeroInTheMakingQuest ),
				typeof( WildBoarCullQuest ),
				typeof( ForcedMigrationQuest ),
				typeof( BullfightingSortOfQuest ),
				typeof( FineFeastQuest ),
				typeof( OverpopulationQuest ),
				typeof( DeadManWalkingQuest ),
				typeof( ForkedTonguesQuest ),
				typeof( TrollingForTrollsQuest )
			};
			}
		}

		[Constructable]
		public Tamm()
			: base( "Tamm", "the guard" )
		{
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}

		public Tamm( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Elf;

			Hue = 0x8353;
			HairItemID = 0x2FBF;
			HairHue = 0x386;
		}

		public override void InitOutfit()
		{
			AddItem( new ElvenBoots( 0x901 ) );
			AddItem( new ElvenCompositeLongBow() );
			AddItem( new HidePants() );
			AddItem( new HidePauldrons() );
			AddItem( new HideTunic() );
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