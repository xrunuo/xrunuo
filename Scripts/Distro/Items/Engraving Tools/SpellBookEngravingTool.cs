using System;

namespace Server.Items
{
	public class SpellBookEngravingTool : BaseEngravingTool
	{
		public override int LabelNumber { get { return 1072151; } } // Spellbook Engraving Tool

		public override bool ValidateItem( Item item )
		{
			return item is Spellbook;
		}

		protected override void DoEngrave( Item item, string text )
		{
			if ( item is Spellbook )
			{
				Spellbook book = (Spellbook) item;

				if ( !string.IsNullOrEmpty( text ) )
					book.EngraveString = text;
				else
					book.EngraveString = null;
			}
		}

		[Constructable]
		public SpellBookEngravingTool()
			: base( 0x0FBF )
		{
			Hue = 1165;
			Weight = 5.0;
		}

		public SpellBookEngravingTool( Serial serial )
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