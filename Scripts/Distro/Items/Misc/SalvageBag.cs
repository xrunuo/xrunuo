using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class SalvageBag : Bag
	{
		public enum SalvageOption
		{
			Ingots = 0x1,
			Cloth = 0x2,
			All = Ingots | Cloth
		}

		private class UseBagEntry : ContextMenuEntry
		{
			private SalvageBag m_Bag;
			private SalvageOption m_Option;

			public UseBagEntry( SalvageBag bag, SalvageOption option, int number, bool enabled )
				: base( number )
			{
				m_Bag = bag;
				m_Option = option;

				if ( !enabled )
					Flags |= CMEFlags.Disabled;
			}

			public override void OnClick()
			{
				if ( m_Bag.Deleted )
					return;

				Mobile from = Owner.From;

				if ( from.CheckAlive() )
					m_Bag.Use( from, m_Option );
			}
		}

		public override int LabelNumber { get { return 1079931; } } // Salvage Bag

		[Constructable]
		public SalvageBag()
		{
			Hue = 590;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			bool enabled = from.Backpack != null && this.IsChildOf( from.Backpack );

			if ( from.Alive )
			{
				list.Add( new UseBagEntry( this, SalvageOption.All, 6276, enabled ) );
				list.Add( new UseBagEntry( this, SalvageOption.Ingots, 6277, enabled ) );
				list.Add( new UseBagEntry( this, SalvageOption.Cloth, 6278, enabled ) );
			}
		}

		public void SalvageIngots( Mobile from )
		{
			bool hasTool = false;

			foreach ( Item item in from.Backpack.Items )
			{
				if ( item is BaseTool && ( (BaseTool) item ).CraftSystem == DefBlacksmithy.CraftSystem )
				{
					hasTool = true;
					break;
				}
			}

			if ( !hasTool )
			{
				from.SendLocalizedMessage( 1079822 ); // You need a blacksmithing tool in order to salvage ingots.
				return;
			}

			bool anvil, forge;
			DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( !anvil )
			{
				from.SendLocalizedMessage( 1044266 ); // You must be near an anvil
				return;
			}

			if ( !forge )
			{
				from.SendLocalizedMessage( 1044265 ); // You must be near a forge
				return;
			}

			int count = 0;
			int oldCount = this.TotalItems;

			int message = -1;

			for ( int i = 0; i < this.Items.Count; i++ )
			{
				Item item = (Item) this.Items[i];

				bool success = false;
				bool isStoreBought = false;
				bool lackMining = false;

				Resmelt.Process( DefBlacksmithy.CraftSystem, from, item, false, out success, out isStoreBought, out lackMining );

				if ( lackMining )
				{
					message = 1079975; // You failed to smelt some metal for lack of skill.
				}
				else if ( success )
				{
					count++;
					i--;
				}
			}

			if ( message != -1 )
				from.SendLocalizedMessage( message );

			if ( count > 0 )
			{
				from.PlaySound( 0x2A );
				from.PlaySound( 0x240 );
			}

			from.SendLocalizedMessage( 1079973, String.Format( "{0}\t{1}", count, oldCount ) ); // Salvaged: ~1_COUNT~/~2_NUM~ blacksmithed items
		}

		public void SalvageCloth( Mobile from )
		{
			Scissors scissors = from.Backpack.FindItemByType( typeof( Scissors ) ) as Scissors;

			if ( scissors == null )
			{
				from.SendLocalizedMessage( 1079823 ); // You need scissors in order to salvage cloth.
				return;
			}

			int count = 0;
			int oldCount = this.TotalItems;

			for ( int i = 0; i < this.Items.Count; i++ )
			{
				Item item = (Item) this.Items[i];

				if ( item is IScissorable && ( (IScissorable) item ).Scissor( from, scissors ) )
				{
					count++;
					i--;
				}
			}

			from.SendLocalizedMessage( 1079974, String.Format( "{0}\t{1}", count, oldCount ) ); // Salvaged: ~1_COUNT~/~2_NUM~ tailored items
		}

		public void Use( Mobile from, SalvageOption option )
		{
			if ( from.Backpack == null || !this.IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
				return;
			}

			if ( ( option & SalvageOption.Ingots ) != 0 )
				SalvageIngots( from );

			if ( ( option & SalvageOption.Cloth ) != 0 )
				SalvageCloth( from );
		}

		public SalvageBag( Serial serial )
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