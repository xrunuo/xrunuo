using System;
using System.Linq;

namespace Server.Items
{
	public class FoodDecorationTool : BaseEngravingTool
	{
		public override int LabelNumber { get { return 1073168; } } // Food Decoration Tool

		public override int SelectTargetMessage { get { return 1073171; } } // Select a food to decorate.
		public override int EngraveMessage { get { return 1073174; } } // You decorate the food.

		public override int GumpTitle { get { return 1072359; } } // <CENTER>Food Decoration Tool</CENTER>

		private static Type[] m_FoodTypes = new Type[]
			{
				typeof( CheesePizza ),	typeof( SausagePizza ),
				typeof( Cake ),			typeof( Cookies )
			};

		public override bool ValidateItem( Item item )
		{
			return m_FoodTypes.Contains( item.GetType() );
		}

		protected override void DoEngrave( Item item, string text )
		{
			if ( item is Food )
			{
				Food food = (Food) item;

				if ( !string.IsNullOrEmpty( text ) )
					food.DecorateString = text;
				else
					food.DecorateString = null;
			}
		}

		[Constructable]
		public FoodDecorationTool()
			: base( 0x1BD1 )
		{
			Hue = 1165;
			Weight = 5.0;
		}

		public FoodDecorationTool( Serial serial )
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