using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoInnKeeper : InnKeeper
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoInnKeeper()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBInnKeeper() );
			m_SBInfos.Add( new SBTokunoCook() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new Obi() );

			if ( Female )
				AddItem( new FemaleKimono( Utility.RandomBlueHue() ) );
			else
				AddItem( new MaleKimono( Utility.RandomBlueHue() ) );
		}

		public TokunoInnKeeper( Serial serial )
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