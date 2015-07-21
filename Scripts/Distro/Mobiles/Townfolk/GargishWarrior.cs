using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class GargishWarrior : BaseCreature
	{
		[Constructable]
		public GargishWarrior()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			// TODO (SA): Gargish name
			Name = NameList.RandomName( "female" );
			Title = "the Warrior";
			Female = true;
			Race = Race.Gargoyle;

			Hue = Race.RandomSkinHue();
			Utility.AssignRandomHair( this );

			SetSkill( SkillName.Anatomy, 80.0, 100.0 );
			SetSkill( SkillName.Parry, 80.0, 100.0 );
			SetSkill( SkillName.MagicResist, 80.0, 100.0 );
			SetSkill( SkillName.Tactics, 80.0, 100.0 );
			SetSkill( SkillName.Swords, 80.0, 100.0 );
			SetSkill( SkillName.Macing, 80.0, 100.0 );
			SetSkill( SkillName.Fencing, 80.0, 100.0 );
			SetSkill( SkillName.Wrestling, 80.0, 100.0 );
			SetSkill( SkillName.Throwing, 80.0, 100.0 );

			AddItem( new Bloodblade() );

			AddItem( new FemaleGargishLeatherLeggings() );
			AddItem( new FemaleGargishLeatherChest() );
			AddItem( new FemaleGargishLeatherArms() );
			AddItem( new FemaleGargishLeatherKilt() );
		}

		public override bool CanTeach { get { return true; } }
		public override bool ClickTitle { get { return false; } }

		public GargishWarrior( Serial serial )
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