using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Monk : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public Monk()
			: base( "the monk" )
		{
			if ( Utility.RandomBool() )
				AddItem( new QuarterStaff() );

			SetSkill( SkillName.EvalInt, 65.0, 88.0 );
			SetSkill( SkillName.Tactics, 36.0, 68.0 );
			SetSkill( SkillName.Macing, 45.0, 68.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Wrestling, 36.0, 68.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBMonk() );
		}

		public override void InitOutfit()
		{
			if ( Utility.RandomBool() )
				AddItem( new QuarterStaff() );

			AddItem( new MonksRobe() );
			AddItem( new Sandals() );
		}

		public Monk( Serial serial )
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