using System;
using Server;

namespace Server.Mobiles
{
	public class TerMurHealer : Healer
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		public override bool CheckTeach( SkillName skill, Mobile from )
		{
			if ( !base.CheckTeach( skill, from ) )
				return false;

			return ( skill == SkillName.Forensics )
				|| ( skill == SkillName.Healing )
				|| ( skill == SkillName.SpiritSpeak )
				|| ( skill == SkillName.Anatomy );
		}

		[Constructable]
		public TerMurHealer()
		{
			Title = "the Healer";

			SetSkill( SkillName.Forensics, 80.0, 100.0 );
			SetSkill( SkillName.SpiritSpeak, 80.0, 100.0 );
			SetSkill( SkillName.Healing, 80.0, 100.0 );
			SetSkill( SkillName.Anatomy, 80.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBHealer() );
		}

		public TerMurHealer( Serial serial )
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
