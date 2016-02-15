using System;
using Server;
using Server.Items;

namespace Server.Spells
{
	public abstract class MagerySpell : Spell
	{
		public MagerySpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public abstract SpellCircle Circle { get; }
		public virtual int CastDelayCircleScalar{ get{ return 1; } }

		public override bool ConsumeReagents()
		{
			if( base.ConsumeReagents() )
				return true;

			int count = 0;
			for ( int i = 0;i < Caster.Items.Count; i++ )
				if ( Caster.Items[i] is ILowerRegCost )
					count += ((ILowerRegCost)Caster.Items[i]).LowerRegCost;

			if ( Utility.Random( 100 ) > count )
				return true;

			if( ArcaneGem.ConsumeCharges( Caster, (Core.SE ? 1 : 1 + (int)Circle) ) )
				return true;

			return false;
		}

		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

		public override void GetCastSkills( out double min, out double max )
		{
			int circle = (int)Circle;

			if( Scroll != null )
				circle -= 2;

			double avg = ChanceLength * circle;

			min = avg - ChanceOffset;
			max = avg + ChanceOffset;
		}

		private static int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };

		public override int GetMana()
		{
			if( Scroll is BaseWand || Scroll is GnarledStaff )
				return 0;

			return m_ManaTable[(int)Circle];
		}

		public override double GetResistSkill( Mobile m )
		{
			int maxSkill = ( (1 + (int)Circle) * 10 ) + ( (1 + ((int)Circle / 6)) * 25 );

			if( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, m.Skills[SkillName.MagicResist].Cap );

			return m.Skills[SkillName.MagicResist].Value;
		}

		public virtual bool CheckResisted( Mobile target )
		{
			double n = GetResistPercent( target );

			n /= 100.0;

			if( n <= 0.0 )
				return false;

			if( n >= 1.0 )
				return true;

			int maxSkill = ( (1 + (int)Circle) * 10 ) + ( (1 + ((int)Circle / 6)) * 25 );

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap );

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}

		public override TimeSpan GetDisturbRecovery()
		{
			if ( Core.AOS )
				return TimeSpan.Zero;

			double delay = (0.90 + (0.20 * (int)Circle)) * ( 1.0 - Math.Sqrt( (DateTime.Now - StartCastTime).TotalSeconds / GetCastDelay().TotalSeconds ) );
/*
			if ( delay < 0.2 )
				delay = 0.2;
*/
			return TimeSpan.FromSeconds( delay );
		}

		public override TimeSpan GetCastDelay()
		{
			if( Scroll is BaseWand || Scroll is GnarledStaff )
				return TimeSpan.Zero;

			if( !Core.AOS )
				return TimeSpan.FromSeconds( 0.5 + (0.25 * (int)Circle) );

			return base.GetCastDelay();
		}

		public override TimeSpan CastDelayBase //Only used on AOS
		{
			get
			{
				return TimeSpan.FromSeconds( 3.0 );
				//return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
	}
}