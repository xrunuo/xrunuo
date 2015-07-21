using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public class ParasiticPotion : BasePoisonPotion
	{
		public override int LabelNumber { get { return 1072848; } } // Parasitic Poison

		public override Poison Poison { get { return Poison.Parasitic; } }

		public override double MinPoisoningSkill { get { return 95.0; } }
		public override double MaxPoisoningSkill { get { return 100.0; } }

		private static Hashtable m_Table = new Hashtable();

		public static void AddInfo( Mobile victim, Mobile poisoner )
		{
			m_Table[victim] = poisoner;
		}

		public static void RemoveInfo( Mobile victim )
		{
			if ( m_Table.ContainsKey( victim ) )
				m_Table.Remove( victim );
		}

		public static Mobile GetPoisoner( Mobile victim )
		{
			if ( m_Table.ContainsKey( victim ) )
				return (Mobile) m_Table[victim];

			return null;
		}

		[Constructable]
		public ParasiticPotion()
			: base( PotionEffect.ParasiticPoison )
		{
			Hue = 380;
		}

		public ParasiticPotion( Serial serial )
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