//Author:Neon Destiny
//First Alpha:11/11/03
//First part of my Matrix series
//email:NeonDestiny@hotmail.com
//ICQ:308073073
//Shard: Neon Destiny PVP




using System;
using System.Collections;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class IceMagi : BaseCreature
	{
		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
			"Thy blood shall be spilt.",
			"In Corp Del",
			"An Ex Del",
			"Des Corp Del"
		};

		[Constructable]
		public IceMagi() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{

			Name = "Arcane Magician";
			Title = "of Defiance";
			Hue= 0;
			Body = 400;
			SpeechHue= 1153;
			BaseSoundID = 0;
			Team = 0;

			new SilverSteed().Rider = this;

			SetStr( 275, 375 );
			SetDex( 40, 75 );
			SetInt( 100, 150 );

			SetHits( 230, 375 );
			SetMana( 100, 150 );

			SetDamage( 10, 15 );


			SetSkill( SkillName.Wrestling, 100.2, 100.6 );
			SetSkill( SkillName.Tactics, 100.7, 100.4 );
			SetSkill( SkillName.Anatomy, 100.5, 100.3 );
			SetSkill( SkillName.MagicResist, 110.4, 110.7 );
			SetSkill( SkillName.Magery, 120.4, 120.7 );
			SetSkill( SkillName.Macing, 110.4, 110.7 );
			SetSkill( SkillName.EvalInt, 110.4, 110.7 );

			Fame = 7000;
			Karma = -10000;

			VirtualArmor= 45;

			Item item = new GlacialStaff();
			item.LootType = LootType.Blessed;
			item.Hue = 1152;
			EquipItem( item );

			item = new WizardsHat();
			item.Movable = false;
			item.Hue = 1153;
			EquipItem( item );

			EquipItem( Rehued( Immovable( new Tunic() ), 1153 ) );

			Item PlateGloves = new PlateGloves();
			PlateGloves.Movable=false;
			PlateGloves.Hue=1165;
			EquipItem( PlateGloves );

			Item LongPants = new LongPants();
			LongPants.Movable=false;
			LongPants.Hue=1153;
			EquipItem( LongPants );

			Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=1150;
			EquipItem( Sandals );

			Item Cloak = new Cloak();
			Cloak.Movable=false;
			Cloak.Hue=1150;
			EquipItem( Cloak );

			HairItemID = 0x203B;
			HairHue = 1150;

			PackGold( 100, 300 );
			PackMagicItems( 3, 7 );

			if ( Utility.Random( 4 ) == 0 )
				PackItem( new IceCube() );
		}

		public override int GetHurtSound()
		{
			return 0x167;
		}

		public override int GetDeathSound()
		{
			return 0xBC;
		}

		public override int GetAttackSound()
		{
			return 0x28B;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell ) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell ) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 290;
				m.HueMod = 0;
				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 0.5 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}


////
		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 5.0 );

			this.Say( true, "Defiance Magician Council will end you scum" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
			{
				this.Say( true, "I will Summon the white mages" );
			}
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.75;
				else if ( list.Count == 2 )
					scalar = 0.50;
				else
					scalar = 0.25;

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );
					this.Hits += AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}

				this.Say( true, "the power of Ice will vanquish thee!" );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
		}

		private void DoFocusedLeech_Stage1( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				this.MovingParticles( combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1,    0, (EffectLayer)255, 0x100 );
				this.MovingParticles( combatant, 0x0001, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x1FB );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage2 ), combatant );
			}
		}

		private void DoFocusedLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				combatant.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				combatant.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

				this.PlaySound( 0x209 );
				this.DoHarmful( combatant );
				this.Hits += AOS.Damage( combatant, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );
			}
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );

					int ability = Utility.Random( 4 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "In Corp Del" ); break;
						case 1: DoFocusedLeech( combatant, "Des Corp Del" ); break;
						case 2: DoFocusedLeech( combatant, "An Ex Del" ); break;
						case 3: DoAreaLeech(); break;
							// TODO: Resurrect ability
					}
				}
			}
}
////
		public void SpawnEvil( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int spawned = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
if ( m is WhiteMage || m is IceSerpent || m is IceSerpent )
					++spawned;
			}

			if ( spawned < 10 )
			{
				int newSpawned = Utility.RandomMinMax( 1, 2 );

				for ( int i = 0; i < newSpawned; ++i )
				{
					BaseCreature spawn;

					switch ( Utility.Random( 3 ) )
					{
						default:
						case 0: case 1:	spawn = new WhiteMage(); break;
						case 2: case 3:	spawn = new IceSerpent(); break;
						case 4:			spawn = new IceSerpent(); break;
					}

					spawn.Team = this.Team;
					spawn.Map = map;

					bool validLocation = false;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							spawn.Location = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							spawn.Location = new Point3D( x, y, z );
					}

					if ( !validLocation )
						spawn.Location = this.Location;

					spawn.Combatant = target;
				}
			}
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.8 >= Utility.RandomDouble() ) // 80% chance to polymorph attacker
				Polymorph( target );

			if ( 0.2 >= Utility.RandomDouble() ) // 20% chance to more ratmen
				SpawnEvil( target );

			//if ( Hits < 500 && !IsBodyMod ) // polymorph on low life
			//	Polymorph( this );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );
		}

		public IceMagi( Serial serial ) : base( serial )
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

			int version = reader.ReadInt();
		}
	}
}