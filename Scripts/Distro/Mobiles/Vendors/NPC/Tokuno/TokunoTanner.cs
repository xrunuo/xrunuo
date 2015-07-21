using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoTanner : Tanner
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoTanner()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTanner() );
			m_SBInfos.Add( new SBTokunoLeatherArmor() );
			m_SBInfos.Add( new SBTokunoStuddedArmor() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new HalfApron( Utility.RandomYellowHue() ) );
			AddItem( new Hakama( Utility.RandomPinkHue() ) );
			AddItem( new HakamaShita( Utility.RandomYellowHue() ) );
		}

		public TokunoTanner( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}