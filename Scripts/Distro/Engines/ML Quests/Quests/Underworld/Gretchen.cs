using System;
using System.Collections.Generic;
using System.Text;
using Server.Engines.Quests;
using Server.Items;
using Server;

namespace Server.Engines.Quests
{
	public class CuriositiesQuest : BaseQuest
	{
		/* Curiosities */
		public override object Title { get { return 1094976; } }

		/* Travel into the Abyss and seek three samples of ... */
		public override object Description { get { return 1094978; } }

		/* Reluctant ye are for our purposes to marry. */
		public override object Refuse { get { return 1094979; } }

		/* How bold ye are when encountering me, ... */
		public override object Uncomplete { get { return 1094980; } }

		/* This day I saw in dreams of joy, ... */
		public override object Complete { get { return 1094981; } }

		public CuriositiesQuest()
			: base()
		{
			AddObjective( new ObtainBoneObjective() );
			AddObjective( new ObtainFertileDirtObjective() );

			AddReward( new BaseReward( typeof( ExplodingTarPotion ), 1095147 ) );
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

		private class ObtainBoneObjective : ObtainObjective
		{
			/* This appears to be one of the reagents that Gretchen is looking for! */
			public override int ProgressMessage { get { return 1094982; } }

			public ObtainBoneObjective()
				: base( typeof( Bone ), "Bone", 3, 0xF7E )
			{
			}
		}

		private class ObtainFertileDirtObjective : ObtainObjective
		{
			/* This appears to be one of the reagents that Gretchen is looking for! */
			public override int ProgressMessage { get { return 1094982; } }

			public ObtainFertileDirtObjective()
				: base( typeof( FertileDirt ), "Fertile Dirt", 3, 0xF81 )
			{
			}
		}
	}

	public class Gretchen : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( CuriositiesQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
		}

		[Constructable]
		public Gretchen()
			: base( "Gretchen", "the Alchemist" )
		{
		}

		public Gretchen( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Race = Race.Elf;
			Body = 0x25E;
			Hue = 0x824D;
			HairItemID = 0x2FD1;
			HairHue = 0x325;

			CantWalk = true;
		}

		public override void InitOutfit()
		{
			AddItem( new QuarterStaff() );
			AddItem( new Shoes( 0x70D ) );
			AddItem( new Backpack() );
			AddItem( new Robe( 0x1BB ) );
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
