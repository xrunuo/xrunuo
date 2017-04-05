using System;
using System.Collections.Generic;
using System.Linq;
using Server;

namespace Server
{
	public class TimedSkillMod : SkillMod
	{
		private readonly DateTime m_Expire;

		public TimedSkillMod( SkillName skill, bool relative, double value, TimeSpan delay )
			: this( skill, relative, value, DateTime.UtcNow + delay )
		{
		}

		public TimedSkillMod( SkillName skill, bool relative, double value, DateTime expire )
			: base( skill, relative, value )
		{
			m_Expire = expire;
		}

		public override bool CheckCondition()
		{
			return ( DateTime.UtcNow < m_Expire );
		}
	}

	public class EquipedSkillMod : SkillMod
	{
		private readonly Item m_Item;
		private readonly Mobile m_Mobile;

		public EquipedSkillMod( SkillName skill, bool relative, double value, Item item, Mobile mobile )
			: base( skill, relative, value )
		{
			m_Item = item;
			m_Mobile = mobile;
		}

		public override bool CheckCondition()
		{
			return ( !m_Item.Deleted && !m_Mobile.Deleted && m_Item.Parent == m_Mobile );
		}
	}

	public class DefaultSkillMod : SkillMod
	{
		public DefaultSkillMod( SkillName skill, bool relative, double value )
			: base( skill, relative, value )
		{
		}

		public override bool CheckCondition()
		{
			return true;
		}
	}

	public abstract class SkillMod
	{
		private Mobile m_Owner;
		private SkillName m_Skill;
		private bool m_Relative;
		private double m_Value;
		private bool m_ObeyCap;

		public SkillMod( SkillName skill, bool relative, double value )
		{
			m_Skill = skill;
			m_Relative = relative;
			m_Value = value;
		}

		public bool ObeyCap
		{
			get { return m_ObeyCap; }
			set
			{
				m_ObeyCap = value;

				if ( m_Owner != null )
				{
					Skill sk = m_Owner.Skills[m_Skill];

					if ( sk != null )
						sk.Update();
				}
			}
		}

		public Mobile Owner
		{
			get
			{
				return m_Owner;
			}
			set
			{
				if ( m_Owner != value )
				{
					if ( m_Owner != null )
						m_Owner.RemoveSkillMod( this );

					m_Owner = value;

					if ( m_Owner != value )
						m_Owner.AddSkillMod( this );
				}
			}
		}

		public void Remove()
		{
			Owner = null;
		}

		public SkillName Skill
		{
			get
			{
				return m_Skill;
			}
			set
			{
				if ( m_Skill != value )
				{
					Skill oldUpdate = ( m_Owner == null ? m_Owner.Skills[m_Skill] : null );

					m_Skill = value;

					if ( m_Owner != null )
					{
						Skill sk = m_Owner.Skills[m_Skill];

						if ( sk != null )
							sk.Update();
					}

					if ( oldUpdate != null )
						oldUpdate.Update();
				}
			}
		}

		public bool Relative
		{
			get
			{
				return m_Relative;
			}
			set
			{
				if ( m_Relative != value )
				{
					m_Relative = value;

					if ( m_Owner != null )
					{
						Skill sk = m_Owner.Skills[m_Skill];

						if ( sk != null )
							sk.Update();
					}
				}
			}
		}

		public bool Absolute
		{
			get
			{
				return !m_Relative;
			}
			set
			{
				if ( m_Relative == value )
				{
					m_Relative = !value;

					if ( m_Owner != null )
					{
						Skill sk = m_Owner.Skills[m_Skill];

						if ( sk != null )
							sk.Update();
					}
				}
			}
		}

		public double Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				if ( m_Value != value )
				{
					m_Value = value;

					if ( m_Owner != null )
					{
						Skill sk = m_Owner.Skills[m_Skill];

						if ( sk != null )
							sk.Update();
					}
				}
			}
		}

		public abstract bool CheckCondition();
	}

	public static class MobileSkillModExtensions
	{
		public static void UpdateSkillMods( this Mobile m )
		{
			m.ValidateSkillMods();

			foreach ( SkillMod mod in m.GetSkillMods() )
			{
				Skill sk = m.Skills[mod.Skill];

				if ( sk != null )
					sk.Update();
			}
		}

		public static void ValidateSkillMods( this Mobile m )
		{
			var modsToRemove = m.GetSkillMods().Where( ( mod ) => !mod.CheckCondition() ).ToArray();

			foreach ( SkillMod mod in modsToRemove )
			{
				m.InternalRemoveSkillMod( mod );
			}
		}

		public static void AddSkillMod( this Mobile m, SkillMod mod )
		{
			if ( mod == null )
				return;

			m.ValidateSkillMods();

			m.InternalAddSkillMod( mod );
		}

		public static void RemoveSkillMod( this Mobile m, SkillMod mod )
		{
			if ( mod == null )
				return;

			m.ValidateSkillMods();

			m.InternalRemoveSkillMod( mod );
		}

		public static IEnumerable<SkillMod> GetSkillMods( this Mobile m )
		{
			if ( !m_SkillMods.ContainsKey( m ) )
				return Enumerable.Empty<SkillMod>();

			return m_SkillMods[m];
		}

		private static readonly Dictionary<Mobile, IList<SkillMod>> m_SkillMods = new Dictionary<Mobile, IList<SkillMod>>();

		private static void InternalAddSkillMod( this Mobile m, SkillMod mod )
		{
			if ( !m_SkillMods.ContainsKey( m ) )
				m_SkillMods[m] = new List<SkillMod>();

			IList<SkillMod> mods = m_SkillMods[m];

			if ( !mods.Contains( mod ) )
			{
				mods.Add( mod );

				mod.Owner = m;

				Skill sk = m.Skills[mod.Skill];

				if ( sk != null )
					sk.Update();
			}
		}

		private static void InternalRemoveSkillMod( this Mobile m, SkillMod mod )
		{
			if ( !m_SkillMods.ContainsKey( m ) )
				return;

			IList<SkillMod> mods = m_SkillMods[m];

			if ( mods.Contains( mod ) )
			{
				mods.Remove( mod );

				mod.Owner = null;

				Skill sk = m.Skills[mod.Skill];

				if ( sk != null )
					sk.Update();

				if ( mods.Count == 0 )
					m_SkillMods.Remove( m );
			}
		}
	}
}
