using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurWanderingHealer : BaseHealer
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }
		public override bool CanTeach { get { return false; } }

		[Constructable]
		public TerMurWanderingHealer()
		{
			Title = "the wandering healer";
		}

		public override int GetRobeColor()
		{
			return Utility.RandomNondyedHue();
		}

		public override void InitOutfit()
		{
			AddItem( new Robe( GetRobeColor() ) );
		}

		public override bool ClickTitle { get { return false; } }

		public override bool CheckResurrect( Mobile m )
		{
			if ( m.Criminal )
			{
				Say( 501222 ); // Thou art a criminal.  I shall not resurrect thee.
				return false;
			}

			return true;
		}

		public TerMurWanderingHealer( Serial serial )
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
