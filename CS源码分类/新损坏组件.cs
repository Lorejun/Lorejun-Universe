using Engine;
using Game;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;

namespace Game
{
    
    public class FCComponentDamage : ComponentDamage, IUpdateable
    {
        

        public new UpdateOrder UpdateOrder => UpdateOrder.Default;

        public override void Damage(float amount)
        {
            if (amount > 0f && Hitpoints > 0f)
            {
                Hitpoints = MathUtils.Max(Hitpoints - amount, 0f);
            }
        }

        public override void Update(float dt)
        {
            Vector3 position = m_componentBody.Position;
            if (Hitpoints <= 0f)
            {
                m_subsystemParticles.AddParticleSystem(new BlockDebrisParticleSystem(m_subsystemTerrain, position + m_componentBody.StanceBoxSize.Y / 2f * Vector3.UnitY, m_debrisStrength, m_debrisScale, Color.White, m_debrisTextureSlot));
                m_subsystemAudio.PlayRandomSound(DamageSoundName, 1f, 0f, m_componentBody.Position, 4f, autoDelay: true);
                base.Project.RemoveEntity(base.Entity, disposeEntity: true);
            }

            float num = MathUtils.Abs(m_componentBody.CollisionVelocityChange.Y);
            if (num > m_fallResilience)
            {
                float amount = MathUtils.Sqr(MathUtils.Max(num - m_fallResilience, 0f)) / 15f;
                Damage(amount);
            }

            /*if (position.Y < -10f || position.Y > 276f)
            {
                Damage(Hitpoints);
            }*/

            if (m_componentOnFire != null && (m_componentOnFire.IsOnFire || m_componentOnFire.TouchesFire))
            {
                Damage(dt / m_fireResilience);
            }

            HitpointsChange = Hitpoints - m_lastHitpoints;
            m_lastHitpoints = Hitpoints;
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(throwOnError: true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(throwOnError: true);
            m_componentBody = base.Entity.FindComponent<ComponentBody>(throwOnError: true);
            m_componentOnFire = base.Entity.FindComponent<ComponentOnFire>();
            Hitpoints = valuesDictionary.GetValue<float>("Hitpoints");
            AttackResilience = valuesDictionary.GetValue<float>("AttackResilience");
            m_fallResilience = valuesDictionary.GetValue<float>("FallResilience");
            m_fireResilience = valuesDictionary.GetValue<float>("FireResilience");
            m_debrisTextureSlot = valuesDictionary.GetValue<int>("DebrisTextureSlot");
            m_debrisStrength = valuesDictionary.GetValue<float>("DebrisStrength");
            m_debrisScale = valuesDictionary.GetValue<float>("DebrisScale");
            DamageSoundName = valuesDictionary.GetValue<string>("DestructionSoundName");
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue("Hitpoints", Hitpoints);
        }
    }
}
