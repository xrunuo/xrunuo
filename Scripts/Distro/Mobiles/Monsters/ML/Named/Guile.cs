using System;
using System.Collections;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "Guiles corpse" )]
    public class Guile : BaseCreature
    {
        public override bool AlwaysAttackable
        {
            get
            {
                return true;
            }
        }
        public override bool CheckResistancesInItems { get { return false; } }

        private DateTime m_NextBodyChange;
        private Timer m_ResetTimer;

        [Constructable]
        public Guile()
            : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify Fight Mode
        {
            Name = "Guile";
            Body = 264;
            BaseSoundID = 1386; // TODO: Verify
            Hue = 63;

            SetStr( 65, 70 ); // All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
            SetDex( 260, 260 );
            SetInt( 490, 495 );

            SetHits( 1045, 1050 );

            SetDamage( 14, 20 );

            SetDamageType( ResistanceType.Physical, 100 ); // Looked at stratics and looks all like a pixie

            SetResistance( ResistanceType.Physical, 80, 90 );
            SetResistance( ResistanceType.Fire, 40, 50 );
            SetResistance( ResistanceType.Cold, 40, 50 );
            SetResistance( ResistanceType.Poison, 40, 50 );
            SetResistance( ResistanceType.Energy, 40, 50 );

            SetSkill( SkillName.Magery, 105, 110 );
            SetSkill( SkillName.EvalInt, 115, 120 );
            SetSkill( SkillName.Meditation, 115, 120 );
            SetSkill( SkillName.MagicResist, 150, 155 );
            SetSkill( SkillName.Tactics, 15, 20 );
            SetSkill( SkillName.Wrestling, 10, 15 );


            Fame = 7000; // Pixie
            Karma = -7000; // -Pixie

            if ( 0.01 > Utility.RandomDouble() )
                PackItem( new PetParrot() );
        }


        public override void OnDamage( int amount, Mobile from, bool willKill )
        {
            if ( 0.25 > Utility.RandomDouble() && DateTime.UtcNow > m_NextBodyChange )
                ChangeBody();

            base.OnDamage( amount, from, willKill );
        }

        public void RestoreBody()
        {
            Name = "Guile";
            Body = 264;
            Hue = 63;

            ArrayList list = new ArrayList();

            foreach ( Item item in GetEquippedItems() )
                if ( item != null && !( item is Backpack ) )
                    list.Add( item );

            for ( int i = 0; i < list.Count; i++ )
                ( (Item) list[i] ).Delete();

            list.Clear();

            if ( m_ResetTimer != null )
                m_ResetTimer.Stop();
        }

        public void ChangeBody()
        {
            try
            {
                ArrayList list = new ArrayList();

                foreach ( Mobile m in Map.GetMobilesInRange( Location, 5 ) )
                    if ( m.IsPlayer && m.AccessLevel == AccessLevel.Player )
                        list.Add( m );

                if ( list.Count <= 0 )
                {
                    if ( Body != 264 )
                        RestoreBody();

                    return;
                }

                Mobile attacker = (Mobile) list[Utility.Random( list.Count - 1 )];

                if ( attacker == null )
                    return;

                if ( !attacker.Body.IsHuman )
                    return;

                ArrayList list2 = new ArrayList();

                foreach ( Item item in GetEquippedItems() )
                    if ( item != null && !( item is Backpack ) )
                        list2.Add( item );

                for ( int i = 0; i < list2.Count; i++ )
                    ( (Item) list2[i] ).Delete();

                list2.Clear();

                Name = attacker.Name;
                Hue = attacker.Hue;
                Body = attacker.Body;

                HairItemID = attacker.HairItemID;
                HairHue = attacker.HairHue;

                FacialHairItemID = attacker.FacialHairItemID;
                FacialHairHue = attacker.FacialHairHue;

                try
                {
                    DupeItem( Layer.OneHanded, attacker );
                    DupeItem( Layer.TwoHanded, attacker );
                    DupeItem( Layer.Shoes, attacker );
                    DupeItem( Layer.Pants, attacker );
                    DupeItem( Layer.Shirt, attacker );
                    DupeItem( Layer.Helm, attacker );
                    DupeItem( Layer.Gloves, attacker );
                    DupeItem( Layer.Ring, attacker );
                    DupeItem( Layer.Neck, attacker );
                    DupeItem( Layer.Waist, attacker );
                    DupeItem( Layer.InnerTorso, attacker );
                    DupeItem( Layer.Bracelet, attacker );
                    DupeItem( Layer.MiddleTorso, attacker );
                    DupeItem( Layer.Earrings, attacker );
                    DupeItem( Layer.Arms, attacker );
                    DupeItem( Layer.Cloak, attacker );
                    DupeItem( Layer.OuterTorso, attacker );
                    DupeItem( Layer.OuterLegs, attacker );
                    DupeItem( Layer.InnerLegs, attacker );
                }
                catch
                {
                }

                m_NextBodyChange = DateTime.UtcNow + TimeSpan.FromSeconds( 10.0 );

                if ( m_ResetTimer != null )
                    m_ResetTimer.Stop();

                m_ResetTimer = Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), new TimerCallback( RestoreBody ) );
            }
            catch ( NullReferenceException e )
            {
				Logger.Error( e.ToString() );
            }
        }

        public void DupeItem( Layer layer, Mobile attacker )
        {
            Item item = attacker.FindItemOnLayer( layer );

            if ( item != null )
            {
                Item dupeitem = Activator.CreateInstance( item.GetType() ) as Item;

                if ( dupeitem.LootType != LootType.Blessed )
                    dupeitem.LootType = LootType.Newbied;

                dupeitem.Hue = item.Hue;
                dupeitem.Movable = false;

                AddItem( dupeitem );
            }
        }

        public override void GenerateLoot() // TODO: What is it on OSI?
        {
            AddLoot( LootPack.LowScrolls );
            AddLoot( LootPack.Gems, 2 );
        }

        protected override void OnAfterDeath(Container c)
        {
            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

        public Guile( Serial serial )
            : base( serial )
        {
        }

        public override HideType HideType { get { return HideType.Spined; } } // TODO: How is it on OSI?
        public override int Hides { get { return 5; } }
        public override int Meat { get { return 1; } }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            /*int version = */
            reader.ReadInt();
        }
    }
}
