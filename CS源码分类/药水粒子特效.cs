using Engine.Graphics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

namespace Game
{
    public class BuffParticleSystem : ParticleSystem<BuffParticleSystem.Particle>
    {
        public bool IsStopped { get; set; }

        public Vector3 Position { get; set; }

        public Color Color { get; set; } = Color.White;

        public Vector2 Size { get; set; } = new Vector2(0.1f);

        public float Radius { get; set; }

        public float Speed { get; set; } = 2f;

        public bool MoveAble { get; set; } = true;

        public new Texture2D Texture
        {
            get
            {
                return base.Texture;
            }
            set
            {
                bool flag = value != base.Texture;
                if (flag)
                {
                    base.Texture = value;
                }
            }
        }

        public new int TextureSlotsCount
        {
            get
            {
                return base.TextureSlotsCount;
            }
            set
            {
                bool flag = value != base.TextureSlotsCount;
                if (flag)
                {
                    base.TextureSlotsCount = value;
                }
            }
        }

        public BuffParticleSystem(int particleCount = 10) : base(particleCount)
        {
            this.Texture = ContentManager.Get<Texture2D>("Textures/Star", null);
            this.TextureSlotsCount = 1;
        }

        public override bool Simulate(float dt)
        {
            bool flag = false;
            bool visible = this.m_visible;
            if (visible)
            {
                this.m_toGenerate += 20f * dt;
                float s = MathUtils.Pow(0.02f, dt);
                for (int i = 0; i < base.Particles.Length; i++)
                {
                    BuffParticleSystem.Particle particle = base.Particles[i];
                    bool isActive = particle.IsActive;
                    if (isActive)
                    {
                        flag = true;
                        particle.Time += dt;
                        bool flag2 = particle.Time <= particle.Duration;
                        if (flag2)
                        {
                            bool moveAble = this.MoveAble;
                            if (moveAble)
                            {
                                particle.Position += particle.Velocity * dt;
                                particle.Velocity *= s;
                                particle.Velocity.Y = particle.Velocity.Y + this.Speed * dt;
                            }
                            else
                            {
                                particle.Position += particle.Velocity * dt;
                                particle.Velocity *= s;
                            }
                            particle.Size = this.Size;
                            particle.TextureSlot = (int)MathUtils.Min(MathUtils.Pow((float)this.TextureSlotsCount, 2f) * particle.Time / particle.Duration * 1.2f, MathUtils.Pow((float)this.TextureSlotsCount, 2f) - 1f);
                        }
                        else
                        {
                            particle.IsActive = false;
                        }
                    }
                    else
                    {
                        bool flag3 = !this.IsStopped;
                        if (flag3)
                        {
                            bool flag4 = this.m_toGenerate >= 1f;
                            if (flag4)
                            {
                                particle.IsActive = true;
                                bool moveAble2 = this.MoveAble;
                                if (moveAble2)
                                {
                                    Vector3 v = new Vector3(this.m_random.Float(-1f, 1f), this.m_random.Float(0f, 1f), this.m_random.Float(-1f, 1f));
                                    particle.Position = this.Position + 0.75f * this.Radius * v;
                                    particle.Velocity = 1.5f * v;
                                    particle.Duration = this.m_random.Float(0.5f, 1.5f);
                                }
                                else
                                {
                                    particle.Position = this.Position;
                                    particle.Duration = 0.05f;
                                }
                                particle.Color = this.Color;
                                particle.Size = this.Size;
                                particle.Time = 0f;
                                particle.FlipX = this.m_random.Bool();
                                particle.FlipY = this.m_random.Bool();
                                this.m_toGenerate -= 1f;
                            }
                        }
                        else
                        {
                            this.m_toGenerate = 0f;
                        }
                    }
                }
                this.m_toGenerate = MathUtils.Remainder(this.m_toGenerate, 1f);
                this.m_visible = false;
            }
            return this.IsStopped && !flag;
        }

        public override void Draw(Camera camera)
        {
            bool moveAble = this.MoveAble;
            if (moveAble)
            {
                float num = Vector3.Dot(this.Position - camera.ViewPosition, camera.ViewDirection);
                bool flag = num > -5f && num <= 48f;
                if (flag)
                {
                    this.m_visible = true;
                    base.Draw(camera);
                }
            }
            else
            {
                this.m_visible = true;
                base.Draw(camera);
            }
        }

        private Game.Random m_random = new Game.Random();

        private float m_toGenerate;

        private bool m_visible;

        public class Particle : Game.Particle
        {
            public float Time;

            public float Duration;

            public Vector3 Velocity;
        }
    }
}
