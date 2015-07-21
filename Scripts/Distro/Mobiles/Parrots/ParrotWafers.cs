using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ParrotWafers : Item
	{
		private string[] m_Towns = new string[]
			{
				"Moonglow", "Britain", "Jhelom", "Yew", "Minoc",
				"Magincia", "Vesper", "Trinsic", "Skara Brae",
				"Haven", "Buccaneer's Den", "Ocllo", "Luna", "Umbra"
			};

		public override int LabelNumber { get { return 1072904; } } // Parrot Wafers

		[Constructable]
		public ParrotWafers()
			: base( 0x2FD6 )
		{
			Hue = 56;
			Stackable = true;
			Weight = 1.0;
		}

		public override bool OnDroppedToMobile( Mobile from, Mobile target )
		{
			if ( !base.OnDroppedToMobile( from, target ) )
				return false;

			if ( target is Parrot )
			{
				int msg = Utility.RandomMinMax( 1072599, 1072607 );

				if ( msg != 1072602 )
					target.Say( msg );
				else
					target.Say( msg, m_Towns[Utility.RandomMinMax( 0, m_Towns.Length - 1 )] ); // I just flew in from ~1_CITYNAME~ and boy are my wings tired!

				target.PlaySound( Utility.RandomMinMax( 0xBF, 0xC3 ) );
				Spells.SpellHelper.Turn( target, from );

				Delete();

				return false;
			}

			return true;
		}

		public ParrotWafers( Serial serial )
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