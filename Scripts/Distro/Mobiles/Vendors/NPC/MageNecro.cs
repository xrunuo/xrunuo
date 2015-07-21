using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class MageNecro : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public MageNecro()
			: base( "the necromancer" )
		{
			SetSkill( SkillName.EvalInt, 80.0, 100.0 );
			SetSkill( SkillName.Inscribe, 80.0, 100.0 );
			SetSkill( SkillName.Necromancy, 80.0, 100.0 );
			SetSkill( SkillName.Meditation, 80.0, 100.0 );
			SetSkill( SkillName.MagicResist, 80.0, 100.0 );

			Hue = 0x3C6;
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBNecromancer() );
		}


		public override void InitOutfit()
		{
			base.InitOutfit();
			AddItem( new Server.Items.Shoes( 0x151 ) );
			AddItem( new Server.Items.Robe( 0x455 ) );
			AddItem( new Server.Items.FancyShirt( 0x455 ) );

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = 0x3c6;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			Item beard = new Item( 0x0 );
			beard.Layer = Layer.FacialHair;
			AddItem( beard );



		}

		public MageNecro( Serial serial )
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
