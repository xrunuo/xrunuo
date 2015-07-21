using System;
using System.Linq;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public enum EnhanceResult
	{
		NotInBackpack,
		BadItem,
		BadResource,
		AlreadyEnhanced,
		Success,
		Failure,
		Broken,
		NoResources,
		NoSkill
	}

	public class Enhance
	{
		private static Type[] m_CheckForSubclass = new Type[] {
			typeof( SurgeShield ), typeof( GargishSurgeShield )
		};

		public static EnhanceResult Invoke( Mobile from, CraftSystem craftSystem, BaseTool tool, Item item, CraftResource resource, Type resType, ref object resMessage )
		{
			if ( item == null )
				return EnhanceResult.BadItem;

			if ( !item.IsChildOf( from.Backpack ) )
				return EnhanceResult.NotInBackpack;

			if ( !( item is BaseArmor ) && !( item is BaseWeapon ) )
				return EnhanceResult.BadItem;

			if ( CraftResources.IsStandard( resource ) )
				return EnhanceResult.BadResource;

			CraftItem craftItem;

			if ( m_CheckForSubclass.Contains( item.GetType() ) )
				craftItem = craftSystem.CraftItems.SearchForSubclass( item.GetType() );
			else
				craftItem = craftSystem.CraftItems.SearchFor( item.GetType() );

			if ( craftItem == null || craftItem.Ressources.Count == 0 || craftItem.ForceCannotEnhance )
				return EnhanceResult.BadItem;

			if ( CraftableArtifacts.IsCraftableArtifact( item ) )
				return EnhanceResult.BadItem;

			bool exceptional = false;
			bool allRequiredSkills = false;

			if ( !craftItem.CheckSkills( from, resType, craftSystem, ref exceptional, ref allRequiredSkills, false ) )
				return EnhanceResult.NoSkill;

			CraftResourceInfo info = CraftResources.GetInfo( resource );

			if ( info == null || info.ResourceTypes.Length == 0 )
				return EnhanceResult.BadResource;

			CraftAttributeInfo attributes = info.AttributeInfo;

			if ( attributes == null )
				return EnhanceResult.BadResource;

			int resHue = 0, maxAmount = 0;

			if ( !craftItem.ConsumeRes( from, resType, craftSystem, ref resHue, ref maxAmount, ConsumeType.None, ref resMessage ) )
				return EnhanceResult.NoResources;

			AncientSmithyHammer hammer = from.FindItemOnLayer( Layer.OneHanded ) as AncientSmithyHammer;

			if ( hammer != null )
			{
				if ( !( tool != null && ( tool is SewingKit || tool is RunicSewingKit ) ) )
				{
					hammer.UsesRemaining--;

					if ( hammer.UsesRemaining < 1 )
						hammer.Delete();
				}
			}

			int phys = 0, fire = 0, cold = 0, pois = 0, nrgy = 0;
			int dura = 0, luck = 0, lreq = 0, dinc = 0;
			int baseChance = 0;

			bool physBonus = false;
			bool fireBonus = false;
			bool coldBonus = false;
			bool nrgyBonus = false;
			bool poisBonus = false;
			bool duraBonus = false;
			bool luckBonus = false;
			bool lreqBonus = false;
			bool dincBonus = false;

			if ( item is BaseWeapon )
			{
				BaseWeapon weapon = (BaseWeapon) item;

				if ( !CraftResources.IsStandard( weapon.Resource ) )
					return EnhanceResult.AlreadyEnhanced;

				baseChance = 20;

				dura = weapon.MaxHitPoints;
				luck = weapon.Attributes.Luck;
				lreq = weapon.WeaponAttributes.LowerStatReq;
				dinc = weapon.Attributes.WeaponDamage;

				fireBonus = ( attributes.WeaponFireDamage > 0 );
				coldBonus = ( attributes.WeaponColdDamage > 0 );
				nrgyBonus = ( attributes.WeaponEnergyDamage > 0 );
				poisBonus = ( attributes.WeaponPoisonDamage > 0 );

				duraBonus = ( attributes.WeaponDurability > 0 );
				luckBonus = ( attributes.WeaponLuck > 0 );
				lreqBonus = ( attributes.WeaponLowerRequirements > 0 );
				dincBonus = ( dinc > 0 );
			}
			else
			{
				BaseArmor armor = (BaseArmor) item;

				if ( !CraftResources.IsStandard( armor.Resource ) )
					return EnhanceResult.AlreadyEnhanced;

				baseChance = 20;

				phys = armor.PhysicalResistance;
				fire = armor.FireResistance;
				cold = armor.ColdResistance;
				pois = armor.PoisonResistance;
				nrgy = armor.EnergyResistance;

				dura = armor.MaxHitPoints;
				luck = armor.Attributes.Luck;
				lreq = armor.ArmorAttributes.LowerStatReq;

				physBonus = ( attributes.ArmorPhysicalResist > 0 );
				fireBonus = ( attributes.ArmorFireResist > 0 );
				coldBonus = ( attributes.ArmorColdResist > 0 );
				nrgyBonus = ( attributes.ArmorEnergyResist > 0 );
				poisBonus = ( attributes.ArmorPoisonResist > 0 );

				duraBonus = ( attributes.ArmorDurability > 0 );
				luckBonus = ( attributes.ArmorLuck > 0 );
				lreqBonus = ( attributes.ArmorLowerRequirements > 0 );
				dincBonus = false;
			}

			int skill = from.Skills[craftSystem.MainSkill].Fixed / 10;

			if ( skill >= 100 )
				baseChance -= ( skill - 90 ) / 10;

			EnhanceResult res = EnhanceResult.Success;

			PlayerMobile user = from as PlayerMobile;

			if ( physBonus )
				CheckResult( ref res, baseChance + phys );

			if ( fireBonus )
				CheckResult( ref res, baseChance + fire );

			if ( coldBonus )
				CheckResult( ref res, baseChance + cold );

			if ( nrgyBonus )
				CheckResult( ref res, baseChance + nrgy );

			if ( poisBonus )
				CheckResult( ref res, baseChance + pois );

			if ( duraBonus )
				CheckResult( ref res, baseChance + ( dura / 40 ) );

			if ( luckBonus )
				CheckResult( ref res, baseChance + 10 + ( luck / 2 ) );

			if ( lreqBonus )
				CheckResult( ref res, baseChance + ( lreq / 4 ) );

			if ( dincBonus )
				CheckResult( ref res, baseChance + ( dinc / 4 ) );

			if ( user.NextEnhanceSuccess )
			{
				user.NextEnhanceSuccess = false;
				user.SendLocalizedMessage( 1149969 ); // The magical aura that surrounded you disipates and you feel that your item enhancement chances have returned to normal.
				res = EnhanceResult.Success;
			}

			switch ( res )
			{
				case EnhanceResult.Broken:
					{
						if ( !craftItem.ConsumeRes( from, resType, craftSystem, ref resHue, ref maxAmount, ConsumeType.Half, ref resMessage ) )
							return EnhanceResult.NoResources;

						item.Delete();
						break;
					}
				case EnhanceResult.Success:
					{
						if ( !craftItem.ConsumeRes( from, resType, craftSystem, ref resHue, ref maxAmount, ConsumeType.All, ref resMessage ) )
							return EnhanceResult.NoResources;

						if ( item is BaseWeapon )
						{
							BaseWeapon w = (BaseWeapon) item;

							w.Resource = resource;

							int hue = w.GetElementalDamageHue();

							if ( hue > 0 )
								w.Hue = hue;
						}
						else if ( item is BaseArmor )
							( (BaseArmor) item ).Resource = resource;

						break;
					}
				case EnhanceResult.Failure:
					{
						if ( !craftItem.ConsumeRes( from, resType, craftSystem, ref resHue, ref maxAmount, ConsumeType.Half, ref resMessage ) )
							return EnhanceResult.NoResources;

						break;
					}
			}

			return res;
		}

		public static void CheckResult( ref EnhanceResult res, int chance )
		{
			if ( res != EnhanceResult.Success )
			{
				// we've already failed..
				return;
			}

			int random = Utility.Random( 100 );

			if ( 10 > random )
				res = EnhanceResult.Failure;
			else if ( chance > random )
				res = EnhanceResult.Broken;
		}

		public static void BeginTarget( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			CraftContext context = craftSystem.GetContext( from );
			PlayerMobile user = from as PlayerMobile;

			if ( context == null )
				return;

			int lastRes = context.LastResourceIndex;
			CraftSubResCol subRes = craftSystem.CraftSubRes;

			if ( lastRes >= 0 && lastRes < subRes.Count )
			{
				CraftSubRes res = subRes.GetAt( lastRes );

				if ( from.Skills[craftSystem.MainSkill].Value < res.RequiredSkill )
				{
					from.SendGump( new CraftGump( from, craftSystem, tool, res.Message ) );
				}
				else
				{
					CraftResource resource = CraftResources.GetFromType( res.ItemType );

					if ( resource != CraftResource.None )
					{
						from.Target = new InternalTarget( craftSystem, tool, res.ItemType, resource );
						if ( user.NextEnhanceSuccess )
						{
							from.SendLocalizedMessage( 1149869, "100" ); // Target an item to enhance with the properties of your selected material (Success Rate: ~1_VAL~%).
						}
						else
						{
							from.SendLocalizedMessage( 1061004 ); // Target an item to enhance with the properties of your selected material.
						}
					}
					else
					{
						from.SendGump( new CraftGump( from, craftSystem, tool, 1061010 ) ); // You must select a special material in order to enhance an item with its properties.
					}
				}
			}
			else
			{
				from.SendGump( new CraftGump( from, craftSystem, tool, 1061010 ) ); // You must select a special material in order to enhance an item with its properties.
			}

		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;
			private Type m_ResourceType;
			private CraftResource m_Resource;

			public InternalTarget( CraftSystem craftSystem, BaseTool tool, Type resourceType, CraftResource resource )
				: base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
				m_ResourceType = resourceType;
				m_Resource = resource;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Item )
				{
					object message = null;
					EnhanceResult res = Enhance.Invoke( from, m_CraftSystem, m_Tool, (Item) targeted, m_Resource, m_ResourceType, ref message );

					switch ( res )
					{
						case EnhanceResult.NotInBackpack:
							message = 1061005;
							break; // The item must be in your backpack to enhance it.
						case EnhanceResult.AlreadyEnhanced:
							message = 1061012;
							break; // This item is already enhanced with the properties of a special material.
						case EnhanceResult.BadItem:
							message = 1061011;
							break; // You cannot enhance this type of item with the properties of the selected special material.
						case EnhanceResult.BadResource:
							message = 1061010;
							break; // You must select a special material in order to enhance an item with its properties.
						case EnhanceResult.Broken:
							message = 1061080;
							break; // You attempt to enhance the item, but fail catastrophically. The item is lost.
						case EnhanceResult.Failure:
							message = 1061082;
							break; // You attempt to enhance the item, but fail. Some material is lost in the process.
						case EnhanceResult.Success:
							message = 1061008;
							break; // You enhance the item with the properties of the special material.
						case EnhanceResult.NoSkill:
							message = 1044153;
							break; // You don't have the required skills to attempt this item.
					}

					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, message ) );
				}
			}
		}
	}
}
