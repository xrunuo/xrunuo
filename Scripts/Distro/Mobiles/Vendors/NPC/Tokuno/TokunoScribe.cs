using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoScribe : Scribe
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoScribe()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBScribe() );
			m_SBInfos.Add( new SBTokunoScribe() );
		}

		public override void InitOutfit()
		{
			if ( Female )
				AddItem( new FemaleKimono( Utility.RandomBlueHue() ) );
			else
				AddItem( new MaleKimono( Utility.RandomBlueHue() ) );
		}

		public TokunoScribe( Serial serial )
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