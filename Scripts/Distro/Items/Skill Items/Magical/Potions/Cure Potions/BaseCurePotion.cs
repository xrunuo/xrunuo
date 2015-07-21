using System;
using Server;

namespace Server.Items
{
	public class CureLevelInfo
	{
		private int m_PoisonLevel;
		private double m_Chance;

		public int PoisonLevel
		{
			get { return m_PoisonLevel; }
		}

		public double Chance
		{
			get { return m_Chance; }
		}

		public CureLevelInfo( int level, double chance )
		{
			m_PoisonLevel = level;
			m_Chance = chance;
		}
	}

	public abstract class BaseCurePotion : BasePotion
	{
		public abstract CureLevelInfo[] LevelInfo { get; }

		public BaseCurePotion( PotionEffect effect )
			: base( 0xF07, effect )
		{
		}

		public BaseCurePotion( Serial serial )
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

		public void DoCure( Mobile from )
		{
			bool cure = false;

			CureLevelInfo[] info = LevelInfo;

			for ( int i = 0; i < info.Length; ++i )
			{
				CureLevelInfo li = info[i];

				if ( li.PoisonLevel == from.Poison.Level && Scale( from, li.Chance ) > Utility.RandomDouble() )
				{
					cure = true;
					break;
				}
			}

			if ( cure && from.CurePoison( from ) )
			{
				from.SendLocalizedMessage( 500231 ); // You feel cured of poison!

				from.FixedEffect( 0x373A, 10, 15 );
				from.PlaySound( 0x1E0 );

				Misc.PoisonResistance.RemoveContext( from );
			}
			else if ( !cure )
			{
				from.SendLocalizedMessage( 500232 ); // That potion was not strong enough to cure your ailment!
			}
		}

		public override void Drink( Mobile from )
		{
			if ( Spells.Necromancy.TransformationSpell.UnderTransformation( from, typeof( Spells.Necromancy.VampiricEmbraceSpell ) ) )
			{
				from.SendLocalizedMessage( 1061652 ); // The garlic in the potion would surely kill you.
			}
			else if ( from.Poisoned )
			{
				DoCure( from );

				PlayDrinkEffect( from );

				from.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
				from.PlaySound( 0x1E0 );

				this.Consume();
			}
			else
			{
				from.SendLocalizedMessage( 1042000 ); // You are not poisoned.
			}
		}
	}
}