using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurScribe : Scribe
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurScribe()
		{
			Title = "the Scribe";
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTokunoScribe() );
			m_SBInfos.Add( new SBScribe() );
		}

		public TerMurScribe( Serial serial )
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