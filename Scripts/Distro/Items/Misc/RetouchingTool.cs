using System;

using Server;
using Server.Engines.VeteranRewards;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class RetouchingTool : Item, IRewardItem
	{
		public override int LabelNumber { get { return 1095966; } } // retouching tool

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set
			{
				m_IsRewardItem = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RetouchingTool()
			: base( 17094 )
		{
			Weight = 1.0;
			Stackable = false;
			Movable = true;
			LootType = LootType.Blessed;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1080458 ); // 11th Year Veteran Reward
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage( 1113815 ); // Target the ethereal mount you wish to retouch.
				from.Target = new InternalTarget( this );
			}
		}

		private class InternalTarget : Target
		{
			private RetouchingTool m_Tool;

			public InternalTarget( RetouchingTool tool )
				: base( 2, false, TargetFlags.None )
			{
				m_Tool = tool;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !m_Tool.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
				else if ( targeted is EtherealMount )
				{
					EtherealMount ethereal = (EtherealMount)targeted;

					ethereal.SolidHue = !ethereal.SolidHue;

					if ( ethereal.SolidHue )
						from.SendLocalizedMessage( 1113816 ); // Your ethereal mount's body has been solidified.
					else
						from.SendLocalizedMessage( 1113817 ); // Your ethereal mount's transparency has been restored.
				}
				else
				{
					from.SendMessage( "You cannot use your retouching tool on that." );
				}
			}
		}

		public RetouchingTool( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( (bool)m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_IsRewardItem = reader.ReadBool();
		}
	}
}
