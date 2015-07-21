using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurMystic : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurMystic()
			: base( "the Mystic" )
		{
			SetSkill( SkillName.EvalInt, 65.0, 90.0 );
			SetSkill( SkillName.Meditation, 65.0, 90.0 );
			SetSkill( SkillName.MagicResist, 65.0, 90.0 );
			SetSkill( SkillName.Mysticism, 65.0, 90.0 );
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new MysticismSpellbook() );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBMystic() );
		}

		public TerMurMystic( Serial serial )
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