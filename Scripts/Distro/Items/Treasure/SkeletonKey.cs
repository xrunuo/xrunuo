using System;
using Server;

namespace Server.Items
{
	public class SkeletonKey : Lockpick, IUsesRemaining
	{
		public override int LabelNumber { get { return 1095522; } } // skeleton key

		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		public bool ShowUsesRemaining
		{
			get { return true; }
			set
			{
			}
		}

		[Constructable]
		public SkeletonKey()
		{
			ItemID = 0x410A;
			Stackable = false;
			Weight = 1.0;
			UsesRemaining = 10;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override void OnSuccess( Mobile from, ILockpickable target )
		{
			ConsumeUse();
		}

		public override void OnFailure( Mobile from, ILockpickable target )
		{
			ConsumeUse();
		}

		public void ConsumeUse()
		{
			Effects.PlaySound( this.Location, this.Map, 0x4A );

			UsesRemaining--;

			if ( UsesRemaining <= 0 )
				Delete();
		}

		public override bool CheckSuccess( Mobile from, ILockpickable target )
		{
			// Skeleton key gives +10.0 virtual bonus to Lockpicking skill
			double value = from.Skills[SkillName.Lockpicking].Value + 10.0;

			double minSkill = target.LockLevel;
			double maxSkill = target.MaxLockLevel;

			double chance = ( value - minSkill ) / ( maxSkill - minSkill );

			return from.CheckTargetSkill( SkillName.Lockpicking, target, chance );
		}

		public SkeletonKey( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_UsesRemaining = reader.ReadInt();
		}
	}
}
