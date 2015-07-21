using System;
using System.Collections;
using Server.Regions;
using Server.Mobiles;

namespace Server.Items
{
	public class CreepyCake : Food
	{
		[Constructable]
		public CreepyCake()
			: base( 0x469F )
		{
			Weight = 1.0;
			Name = "Creepy cake";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 );
			}
			else if ( from.Region.Name == "Tele Center Tram" || from.Region.Name == "Tele Center Fel" )
			{
				from.SendMessage( "You are not allowed to do that in the Tele Center" );
			}
			else
			{
				from.SendMessage( "You eat the cake and begin to feel sick..." );
				from.Poison = Poison.Lesser;
				from.Say( "*cough cough*" );

				this.Delete();
			}
		}

		public CreepyCake( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}




