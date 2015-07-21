using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoCobbler : Cobbler
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoCobbler()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBCobbler() );
			m_SBInfos.Add( new SBTokunoCobbler() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new TattsukeHakama() );

			if ( Female )
				AddItem( new Doublet() );
			else
				AddItem( new FancyShirt() );
		}

		public TokunoCobbler( Serial serial )
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