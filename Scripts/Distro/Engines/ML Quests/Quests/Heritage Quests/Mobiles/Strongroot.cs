using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Strongroot : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
		{
			typeof( CaretakerOfTheLandQuest )
		};
			}
		}

		[Constructable]
		public Strongroot()
			: base( "Strongroot" )
		{
		}

		public Strongroot( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Body = 301;
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