using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class TerMurBaker : Baker
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurBaker()
		{
			Title = "the Baker";
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBBaker() );
		}

		public TerMurBaker( Serial serial )
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
