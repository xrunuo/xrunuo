using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class GargishCitizen : BaseCreature
	{
		[Constructable]
		public GargishCitizen()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Title = "the mystic llamaherder";
			Female = Utility.RandomBool();
			Race = Race.Gargoyle;
			Blessed = true;

			// TODO (SA): Gargish name
			if ( !Female )
				Name = NameList.RandomName( "male" );
			else
				Name = NameList.RandomName( "female" );

			Hue = Race.RandomSkinHue();
			Utility.AssignRandomHair( this );

			AddItem( new GargishClothChest( GetRandomHue() ) );
			AddItem( new GargishClothKilt( Utility.RandomNeutralHue() ) );
		}

		public int GetRandomHue()
		{
			switch ( Utility.Random( 5 ) )
			{
				default:
				case 0:
					return Utility.RandomBlueHue();
				case 1:
					return Utility.RandomGreenHue();
				case 2:
					return Utility.RandomRedHue();
				case 3:
					return Utility.RandomYellowHue();
				case 4:
					return Utility.RandomNeutralHue();
			}
		}

		public override bool CanTeach { get { return false; } }
		public override bool ClickTitle { get { return false; } }
		public override bool PropertyTitle { get { return false; } }

		public GargishCitizen( Serial serial )
			: base( serial )
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