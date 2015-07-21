using System;
using Server;
using AMI = Server.Items.ArmorMaterialInfo;

namespace Server.Items
{
	public enum ArmorMaterialType
	{
		Cloth,
		Leather,
		Studded,
		Bone,
		Ringmail,
		Chainmail,
		Plate,
		Scale,
		Wood,
		Stone
	}

	public class ArmorMaterialInfo
	{
		private CraftResource m_DefaultResource;
		private bool m_Meditable;

		public CraftResource DefaultResource { get { return m_DefaultResource; } }
		public bool Meditable { get { return m_Meditable; } }

		public ArmorMaterialInfo( CraftResource defaultResource, bool meditable )
		{
			m_DefaultResource = defaultResource;
			m_Meditable = meditable;
		}

		public static ArmorMaterialInfo[] Table { get { return m_Table; } }

		private static readonly ArmorMaterialInfo[] m_Table = new ArmorMaterialInfo[]
			{
				new AMI( CraftResource.RegularLeather,	true  ), /* Cloth */
				new AMI( CraftResource.RegularLeather,	true  ), /* Leather */
				new AMI( CraftResource.RegularLeather,	false ), /* Studded */
				new AMI( CraftResource.RegularLeather,	false ), /* Bone */
				new AMI( CraftResource.Iron,			false ), /* Ringmail */
				new AMI( CraftResource.Iron,			false ), /* Chainmail */
				new AMI( CraftResource.Iron,			false ), /* Plate */
				new AMI( CraftResource.RedScales,		false ), /* Scale */
				new AMI( CraftResource.Wood,		false ), /* Wood */
				new AMI( CraftResource.Iron,			false ), /* Stone */
			};

		public static ArmorMaterialInfo GetInfo( ArmorMaterialType type )
		{
			int v = (int) type;

			if ( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}
	}
}