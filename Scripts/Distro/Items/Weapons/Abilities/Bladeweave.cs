using System;

namespace Server.Items
{
	/// <summary>
	/// Warriors becomes one with their weapon, allowing it to guide their hand. The effects of this attack are unpredictable, but effective.
	/// </summary>
	public class Bladeweave : WeaponAbility
	{
		public Bladeweave()
		{
		}

		public override int BaseMana { get { return 30; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
			{
				return;
			}

			attacker.PlaySound( 0x5BC ); // Bladeweave sound effect
			attacker.FixedParticles( 0x376A, 1, 20, 0x7F5, 0x960, 0x3, EffectLayer.Waist ); // TODO: Revisar efecto

			switch ( Utility.RandomMinMax( 1, 7 ) )
			{
				case 1:
					{
						attacker.SendLocalizedMessage( 1072841, "Paralyzing Blow" ); // You weave your blade to execute a ~1_attack~.
						ParalyzingBlow at = new ParalyzingBlow();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 2:
					{
						attacker.SendLocalizedMessage( 1072841, "Bleed Attack" ); // You weave your blade to execute a ~1_attack~.
						BleedAttack at = new BleedAttack();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 3:
					{
						attacker.SendLocalizedMessage( 1072841, "Double Strike" ); // You weave your blade to execute a ~1_attack~.
						DoubleStrike at = new DoubleStrike();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 4:
					{
						attacker.SendLocalizedMessage( 1072841, "Feint" ); // You weave your blade to execute a ~1_attack~.
						Feint at = new Feint();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 5:
					{
						attacker.SendLocalizedMessage( 1072841, "Mortal Strike" ); // You weave your blade to execute a ~1_attack~.
						MortalStrike at = new MortalStrike();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 6:
					{
						attacker.SendLocalizedMessage( 1072841, "block" ); // You weave your blade to execute a ~1_attack~.
						Block at = new Block();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
				case 7:
					{
						attacker.SendLocalizedMessage( 1072841, "Crushing Blow" ); // You weave your blade to execute a ~1_attack~.
						CrushingBlow at = new CrushingBlow();
						at.IsBladeweaveAttack = true;
						at.OnHit( attacker, defender, damage );
						break;
					}
			}
		}
	}
}