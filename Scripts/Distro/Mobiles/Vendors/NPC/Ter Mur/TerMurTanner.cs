using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurTanner : Tanner
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurTanner()
		{
			Title = "the Tanner";
		}

		public override void InitOutfit()
		{
			AddItem( new GargishLeatherLeggings( Utility.RandomNeutralHue() ) );
			AddItem( new GargishLeatherChest( GetRandomHue() ) );
			AddItem( new GargishLeatherArms( Utility.RandomNeutralHue() ) );
			AddItem( new GargishLeatherKilt( Utility.RandomNeutralHue() ) );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTerMurTanner() );
		}

		public TerMurTanner( Serial serial )
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