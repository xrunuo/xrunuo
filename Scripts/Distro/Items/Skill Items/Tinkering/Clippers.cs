using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.Craft;
using Server.Engines.Plants;
using Server.ContextMenus;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0xDFC, 0xDFD )]
	public class Clippers : Item, IUsesRemaining, ICraftable
	{
		public override int LabelNumber { get { return 1112117; } } // clippers

		private int m_UsesRemaining;
		private bool m_CutReeds;

		public bool ShowUsesRemaining
		{
			get { return true; }
			set { }
		}

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

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CutReeds
		{
			get { return m_CutReeds; }
			set { m_CutReeds = value; }
		}

		[Constructable]
		public Clippers()
			: this( Utility.RandomMinMax( 30, 50 ) )
		{
		}

		[Constructable]
		public Clippers( int uses )
			: base( Utility.RandomBool() ? 0xDFC : 0xDFD )
		{
			Weight = 1.0;
			Hue = 0x490;

			UsesRemaining = uses;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1112118 ); // What plant do you wish to use these clippers on?
			from.Target = new InternalTarget( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public Clippers( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_UsesRemaining );
			writer.Write( (bool) m_CutReeds );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				case 0:
					{
						if ( version < 1 )
						{
							reader.ReadString(); // crafter name
							reader.ReadMobile(); // crafter
						}

						m_UsesRemaining = reader.ReadInt();

						if ( version < 1 )
							reader.ReadEncodedInt(); // quality

						m_CutReeds = reader.ReadBool();

						break;
					}
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive )
			{
				list.Add( new SetClipModeEntry( this, true, 1112283, !m_CutReeds ) ); // Set to cut reeds
				list.Add( new SetClipModeEntry( this, false, 1112282, m_CutReeds ) ); // Set to clip plants
			}
		}

		private class SetClipModeEntry : ContextMenuEntry
		{
			private Clippers m_Clippers;
			private bool m_EnableCutReeds;

			public SetClipModeEntry( Clippers clippers, bool enableCutReeds, int number, bool enabled )
				: base( number )
			{
				m_Clippers = clippers;
				m_EnableCutReeds = enableCutReeds;

				if ( !enabled )
					Flags |= CMEFlags.Disabled;
			}

			public override void OnClick()
			{
				if ( m_Clippers.Deleted )
					return;

				Mobile from = Owner.From;

				if ( m_Clippers.CutReeds && m_EnableCutReeds )
					from.SendLocalizedMessage( 1112287 ); // You are already set to cut reeds!
				else if ( !m_Clippers.CutReeds && !m_EnableCutReeds )
					from.SendLocalizedMessage( 1112284 ); // You are already set to make plant clippings!
				else
				{
					m_Clippers.CutReeds = m_EnableCutReeds;

					from.SendLocalizedMessage( m_EnableCutReeds
							? 1112286 // You are now set to cut reeds.
							: 1112285 // You are now set to cut plant clippings.
						);
				}
			}
		}

		private class InternalTarget : Target
		{
			private Clippers m_Clippers;

			public InternalTarget( Clippers clippers )
				: base( 1, false, TargetFlags.None )
			{
				m_Clippers = clippers;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Clippers.Deleted )
					return;

				if ( targeted is PlantItem )
				{
					PlantItem plant = (PlantItem) targeted;

					if ( !plant.IsChildOf( from.Backpack ) )
						from.SendLocalizedMessage( 502437 ); // Items you wish to cut must be in your backpack.
					if ( plant.PlantStatus != PlantStatus.DecorativePlant )
						from.SendLocalizedMessage( 1112119 ); // You may only use these clippers on decorative plants.
					else
					{
						// TODO (SA): ¿Se puede podar cualquier tipo de planta?

						Item item;
						if ( m_Clippers.CutReeds )
							item = new DryReeds( plant.PlantHue );
						else
							item = new PlantClippings( plant.PlantHue );

						plant.Delete();

						if ( !from.PlaceInBackpack( item ) )
							item.MoveToWorld( from.Location, from.Map );

						from.SendLocalizedMessage( 1112120 ); // You cut the plant into small pieces and place them in your backpack.

						m_Clippers.UsesRemaining--;

						if ( m_Clippers.UsesRemaining <= 0 )
						{
							m_Clippers.Delete();
							from.SendLocalizedMessage( 1112126 ); // Your clippers break as you use up the last charge.
						}
					}
				}
				else
					from.SendLocalizedMessage( 1112119 ); // You may only use these clippers on decorative plants.
			}
		}

		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			if ( exceptional )
				UsesRemaining *= 2;

			return exceptional;
		}
	}
}