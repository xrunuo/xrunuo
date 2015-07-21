using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoCook : Cook
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoCook()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBCook() );
			m_SBInfos.Add( new SBTokunoCook() );
		}

		public override void InitOutfit()
		{
			AddItem( new TattsukeHakama() );
			AddItem( new HalfApron() );
			AddItem( new HakamaShita() );
		}

		public TokunoCook( Serial serial )
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