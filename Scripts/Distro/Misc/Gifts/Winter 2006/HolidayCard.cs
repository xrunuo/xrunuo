using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class HolidayCard : Item
	{
		private static string[] m_StaffNames = new string[]
			{
				"Semerkhet"
			};

		[Constructable]
		public HolidayCard( string player )
			: base( 0xEBE )
		{
			Name = String.Format( "A 2007 holiday card from {0}, to {1}", m_StaffNames[Utility.Random( m_StaffNames.Length )], player );

			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = Utility.RandomDyedHue();
		}

		public HolidayCard( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
