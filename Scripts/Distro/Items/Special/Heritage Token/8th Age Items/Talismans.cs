using System;

namespace Server.Items
{
	public enum TalismanForm
	{
		Ferret = 1031672,
		Squirrel = 1031671,
		CuSidhe = 1031670,
		Reptalon = 1075202
	}

	public class BaseFormTalisman : Item
	{
		private TalismanForm m_Form;

		[CommandProperty( AccessLevel.GameMaster )]
		public TalismanForm Form
		{
			get { return m_Form; }
			set { m_Form = value; InvalidateProperties(); }
		}

		[Constructable]
		public BaseFormTalisman( TalismanForm form )
			: base( 0x2F59 )
		{
			m_Form = form;

			LootType = LootType.Blessed;
			Layer = Layer.Talisman;
			Weight = 1.0;
		}

		public BaseFormTalisman( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1075200, String.Format( "#{0}", (int) m_Form ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version

			writer.Write( (int) m_Form );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Form = (TalismanForm) reader.ReadInt();
		}
	}

	public class FerretFormTalisman : BaseFormTalisman
	{
		[Constructable]
		public FerretFormTalisman()
			: base( TalismanForm.Ferret )
		{
		}

		public FerretFormTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class SquirrelFormTalisman : BaseFormTalisman
	{
		[Constructable]
		public SquirrelFormTalisman()
			: base( TalismanForm.Squirrel )
		{
		}

		public SquirrelFormTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class CuSidheFormTalisman : BaseFormTalisman
	{
		[Constructable]
		public CuSidheFormTalisman()
			: base( TalismanForm.CuSidhe )
		{
		}

		public CuSidheFormTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class ReptalonFormTalisman : BaseFormTalisman
	{
		[Constructable]
		public ReptalonFormTalisman()
			: base( TalismanForm.Reptalon )
		{
		}

		public ReptalonFormTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
