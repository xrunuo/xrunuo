using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public static class SetItemsHelper
	{
		public static Item GetRandomSetItem()
		{
			Item result = null;
			//Choose between common or rare
			if ( 5 > Utility.Random( 100 ) )
			{
				//Rare item
				switch ( Utility.RandomMinMax( 1, 16 ) )
				{
					case 1:
						result = new AcolyteTunic(); break;
					case 2:
						result = new AcolyteLeggings(); break;
					case 3:
						result = new AssassinGloves(); break;
					case 4:
						result = new AssassinLeggings(); break;
					case 5:
						result = new HunterTunic(); break;
					case 6:
						result = new HunterSleeves(); break;
					case 7:
						result = new LeafweaveTunic(); break;
					case 8:
						result = new LeafweaveSleeves(); break;
					case 9:
						result = new MyrmidonGorget(); break;
					case 10:
						result = new MyrmidonChest(); break;
					case 11:
						result = new MyrmidonHelm(); break;
					case 12:
						result = new DeathsEssenceHelm(); break;
					case 13:
						result = new DeathsEssenceSleeves(); break;
					case 14:
						result = new PlateOfHonorChest(); break;
					case 15:
						result = new PlateOfHonorLegs(); break;
					case 16:
						result = new PlateOfHonorHelm(); break;
				}

			}
			else
			{
				//Common item
				switch ( Utility.RandomMinMax( 1, 21 ) )
				{
					case 1:
						result = new AcolyteSleeves(); break;
					case 2:
						result = new AcolyteGloves(); break;
					case 3:
						result = new AssassinSleeves(); break;
					case 4:
						result = new AssassinTunic(); break;
					case 5:
						result = new HunterLeggings(); break;
					case 6:
						result = new HunterGloves(); break;
					case 7:
						result = new LeafweaveLeggings(); break;
					case 8:
						result = new LeafweaveGloves(); break;
					case 9:
						result = new MyrmidonArms(); break;
					case 10:
						result = new MyrmidonLegs(); break;
					case 11:
						result = new MyrmidonGloves(); break;
					case 12:
						result = new DeathsEssenceLeggings(); break;
					case 13:
						result = new DeathsEssenceTunic(); break;
					case 14:
						result = new DeathsEssenceGloves(); break;
					case 15:
						result = new PlateOfHonorArms(); break;
					case 16:
						result = new PlateOfHonorGloves(); break;
					case 17:
						result = new PlateOfHonorGorget(); break;
					case 18:
						result = new Evocaricus(); break;
					case 19:
						result = new MalekisHonor(); break;
					case 20:
						result = new Feathernock(); break;
					case 21:
						result = new Swiftflight(); break;
				}
			}




			return result;
		}
	}

}