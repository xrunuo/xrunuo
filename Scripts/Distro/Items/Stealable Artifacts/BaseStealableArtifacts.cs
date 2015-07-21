using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public abstract class StealableArtifact : Item, IArtifactRarity
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableArtifact( int itemID )
			: base( itemID )
		{
			Stackable = false;

			Weight = 10.0;

			Movable = false;
		}

		public StealableArtifact( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ArtifactRarity { get { return 0; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableContainerArtifact : BaseContainer, IArtifactRarity
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableContainerArtifact( int itemID )
			: base( itemID )
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableContainerArtifact( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ArtifactRarity { get { return 0; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableLightArtifact : BaseLight, IArtifactRarity
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableLightArtifact( int itemID )
			: base( itemID )
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableLightArtifact( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ArtifactRarity { get { return 0; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableLongswordArtifact : Longsword, IArtifactRarity
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableLongswordArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableLongswordArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealablePlateGlovesArtifact : PlateGloves
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealablePlateGlovesArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealablePlateGlovesArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableWarHammerArtifact : WarHammer
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableWarHammerArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableWarHammerArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableExecutionersAxeArtifact : ExecutionersAxe
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableExecutionersAxeArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableExecutionersAxeArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableFoodArtifact : Food, IArtifactRarity
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableFoodArtifact( int i, int id )
			: base( i, id )
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableFoodArtifact( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ArtifactRarity { get { return 0; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	#region Stealables Underworld & Stygian Abyss

	public abstract class StealableSmallPlateShieldArtifact : SmallPlateShield
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableSmallPlateShieldArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableSmallPlateShieldArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableCircletArtifact : Circlet
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableCircletArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableCircletArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableGargishTalwarArtifact : GargishTalwar
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableGargishTalwarArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableGargishTalwarArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableGlassStaffArtifact : GlassStaff
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableGlassStaffArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableGlassStaffArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	public abstract class StealableSoulGlaiveArtifact : SoulGlaive
	{
		public override bool ForceShowProperties { get { return true; } }

		public StealableSoulGlaiveArtifact()
		{
			Stackable = false;

			Movable = false;

			Weight = 10.0;
		}

		public StealableSoulGlaiveArtifact( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( Weight != 10 )
				Weight = 10;
		}
	}

	#endregion
}