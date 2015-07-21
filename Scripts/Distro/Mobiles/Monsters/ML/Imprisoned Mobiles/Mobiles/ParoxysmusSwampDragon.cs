using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a swamp dragon corpse" )]
	public class ParoxysmusSwampDragon : SwampDragon
	{
		[Constructable]
		public ParoxysmusSwampDragon()
			: this( "Paroxysmus' Swamp Dragon" )
		{
		}

		[Constructable]
		public ParoxysmusSwampDragon( string name )
			: base( name )
		{
			SetSkill( SkillName.Anatomy, 100 );
			SetSkill( SkillName.MagicResist, 100 );
			SetSkill( SkillName.Tactics, 100 );
			SetSkill( SkillName.Wrestling, 100 );

			BardingExceptional = true;
			BardingResource = CraftResource.Iron;
			HasBarding = true;
			Hue = 0x851;
			BardingHP = BardingMaxHP;
		}

		public ParoxysmusSwampDragon( Serial serial )
			: base( serial )
		{
		}

		public override bool DeleteOnRelease { get { return true; } }

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