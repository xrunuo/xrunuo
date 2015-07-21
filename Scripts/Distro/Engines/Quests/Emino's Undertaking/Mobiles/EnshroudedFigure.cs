using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class EnshroudedFigure : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public EnshroudedFigure()
			: base( "" )
		{
		}

		public EnshroudedFigure( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83FE;

			Body = 0x190;

			Female = false;

			Name = "Enshrouded Figure";
		}

		public override void InitOutfit()
		{
			AddItem( new ThighBoots( 0x1 ) );

			DeathShroud ds = new DeathShroud();

			ds.AccessLevel = AccessLevel.Player;

			ds.Hue = 0x1;

			AddItem( ds );
		}

		public override bool CanTalkTo( PlayerMobile pm )
		{
			return false;
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			return;
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
