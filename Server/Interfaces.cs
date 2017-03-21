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
using System.Collections.Generic;

using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public interface IMount
	{
		Mobile Rider { get; set; }
		void OnRiderDamaged( int amount, Mobile from, bool willKill );
	}

	public interface IMountItem
	{
		IMount Mount { get; }
	}
}

namespace Server
{
	public interface IPoint2D
	{
		int X { get; }
		int Y { get; }
	}

	public interface IPoint3D : IPoint2D
	{
		int Z { get; }
	}

	public interface IMap
	{
		int MapID { get; }

		IEnumerable<NetState> GetClientsInRange( IPoint2D p, int range = 18 );
	}

	public interface IEntity : IPoint3D
	{
		Serial Serial { get; }
		IPoint3D Location { get; }
		IMap Map { get; }
	}

	public interface IMobile : IEntity
	{
		Body Body { get; }
		int Hue { get; }
		Race Race { get; }
		Direction Direction { get; }
		int Hits { get; }
		int HitsMax { get; }
		int Stam { get; }
		int StamMax { get; }
		int Mana { get; }
		int ManaMax { get; }
		int Str { get; }
		int Dex { get; }
		int Int { get; }
		int StatCap { get; }
		int PhysicalResistance { get; }
		int FireResistance { get; }
		int ColdResistance { get; }
		int PoisonResistance { get; }
		int EnergyResistance { get; }
		int TithingPoints { get; }
	}

	public interface IVendor
	{
		bool OnBuyItems( Mobile from, List<BuyItemResponse> list );
		bool OnSellItems( Mobile from, List<SellItemResponse> list );

		DateTime LastRestock { get; set; }
		TimeSpan RestockDelay { get; }
		void Restock();
	}

	public interface ICarvable
	{
		void Carve( Mobile from, Item item );
	}

	public interface IWeapon : IMagicalItem, IMagicalBonus, IResistances, ISkillBonuses, IAbsorption
	{
		int GetMaxRange( Mobile attacker, Mobile defender );
		int MaxRange { get; }
		void OnBeforeSwing( Mobile attacker, Mobile defender );
		TimeSpan OnSwing( Mobile attacker, Mobile defender );
		void GetStatusDamage( Mobile from, out int min, out int max );
		void UnscaleDurability();
		void ScaleDurability();
		TimeSpan GetDelay( Mobile attacker );
		WeaponAttributes WeaponAttributes { get; set; }
	}

	public interface IArmor : IMagicalItem, IMagicalBonus, IResistances, ISkillBonuses, IAbsorption
	{
		void UnscaleDurability();
		void ScaleDurability();
		ArmorAttributes ArmorAttributes { get; set; }
	}

	public interface ICloth
	{
		ArmorAttributes ClothingAttributes { get; set; }
	}

	public interface IHued
	{
		int HuedItemID { get; }
	}

	public interface ISpell
	{
		bool IsCasting { get; }
		void OnCasterHurt();
		void OnCasterKilled();
		void OnConnectionChanged();
		bool OnCasterMoving( Direction d );
		bool OnCasterEquiping( Item item );
		bool OnCasterUsingObject( object o );
		bool OnCastInTown( Region r );
	}

	public interface IParty
	{
		void OnStamChanged( Mobile m );
		void OnManaChanged( Mobile m );
		void OnStatsQuery( Mobile beholder, Mobile beheld );
	}

	public interface IMagicalItem
	{
		MagicalAttributes Attributes { get; set; }
	}

	public interface IMagicalBonus
	{
		int GetAttributeBonus( MagicalAttribute attr );
	}

	public interface IResistances
	{
		ElementAttributes Resistances { get; set; }
	}

	public interface ISkillBonuses
	{
		SkillBonuses SkillBonuses { get; set; }
	}

	public interface IAbsorption
	{
		AbsorptionAttributes AbsorptionAttributes { get; set; }
	}

	public interface ISpawner
	{
		bool UnlinkOnTaming { get; }
		Point3D HomeLocation { get; }
		Map Map { get; }
		int HomeRange { get; }

		void Remove( ISpawnable spawn );
		void Replace( ISpawnable oldEntity, ISpawnable newEntity );
	}

	public interface ISpawnable : IEntity
	{
		void OnBeforeSpawn( Point3D location, Map map );
		void MoveToWorld( Point3D location, Map map );
		void OnAfterSpawn();
		void Delete();

		ISpawner Spawner { get; set; }
	}
}