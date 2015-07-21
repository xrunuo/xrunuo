//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Server.Network;

namespace Server.Items
{
	public class BaseMulti : Item
	{
		[Constructable]
		public BaseMulti( int itemID )
			: base( itemID )
		{
			Movable = false;
		}

		public BaseMulti( Serial serial )
			: base( serial )
		{
		}

		public virtual void RefreshComponents()
		{
			if ( Parent != null )
				return;

			Map map = Map;

			if ( map != null )
			{
				map.OnLeave( this );
				map.OnEnter( this );
			}
		}

		public override GraphicData GraphicData { get { return GraphicData.MultiData; } }

		public override int LabelNumber
		{
			get
			{
				MultiComponentList mcl = this.Components;

				if ( mcl.List.Length > 0 )
				{
					int id = mcl.List[0].m_ItemID;

					if ( id < 0x4000 )
						return 1020000 + id;
					else
						return 1078872 + id;
				}

				return base.LabelNumber;
			}
		}

		public override int GetMaxUpdateRange()
		{
			return 22;
		}

		public override int GetUpdateRange( Mobile m )
		{
			return 22;
		}

		public virtual MultiComponentList Components
		{
			get
			{
				return MultiData.GetComponents( ItemID );
			}
		}

		public virtual bool Contains( Point2D p )
		{
			return Contains( p.X, p.Y );
		}

		public virtual bool Contains( Point3D p )
		{
			return Contains( p.X, p.Y );
		}

		public virtual bool Contains( IPoint3D p )
		{
			return Contains( p.X, p.Y );
		}

		public virtual bool Contains( int x, int y )
		{
			MultiComponentList mcl = this.Components;

			x -= this.X + mcl.Min.X;
			y -= this.Y + mcl.Min.Y;

			return x >= 0
				&& x < mcl.Width
				&& y >= 0
				&& y < mcl.Height
				&& mcl.Tiles[x][y].Length > 0;
		}

		public bool Contains( Mobile m )
		{
			if ( m.Map == this.Map )
				return Contains( m.X, m.Y );
			else
				return false;
		}

		public bool Contains( Item item )
		{
			if ( item.Map == this.Map )
				return Contains( item.X, item.Y );
			else
				return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
				ItemID -= 0x8000;
		}
	}
}