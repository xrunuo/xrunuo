using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Necromancer : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public Necromancer()
			: base( "The Necromancer" )
		{
			SetSkill( SkillName.EvalInt, 65.0, 88.0 );
			SetSkill( SkillName.Inscribe, 60.0, 83.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Necromancy, 64.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBMage() );
		}

		public Item ApplyHue( Item item, int hue )
		{
			item.Hue = hue;

			return item;
		}

		public override void InitOutfit()
		{
			AddItem( ApplyHue( new Robe(), 0x497 ) );

			Utility.AssignRandomHair( this );

			PackGold( 100, 200 );
		}

		public Necromancer( Serial serial )
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