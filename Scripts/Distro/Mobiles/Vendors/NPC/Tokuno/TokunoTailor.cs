using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoTailor : Tailor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoTailor()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTailor() );
			m_SBInfos.Add( new SBTokunoTailor() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new Obi( Utility.RandomYellowHue() ) );

			if ( Female )
				AddItem( new FemaleKimono( Utility.RandomBlueHue() ) );
			else
				AddItem( new MaleKimono( Utility.RandomBlueHue() ) );
		}

		public TokunoTailor( Serial serial )
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