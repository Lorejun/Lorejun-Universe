using Engine.Graphics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
   /* public class SpaceShip : Block
    {
        public Texture2D m_texture;
        public override void Initialize()
        {
            Model model = ContentManager.Get<Model>("Models/SpaceShip/starchaser", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("starchaser", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("starchaser", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.4f, 0f), false, false, false, false, new Color(96, 96, 96));
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("starchaser", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.4f, 0f), false, true, false, false, new Color(255, 255, 255));
            m_texture = ContentManager.Get<Texture2D>("Textures/SpaceShip/starchaser", null);  //外置材质
            base.Initialize();
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {

        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, 0.5f, ref matrix, environmentData);
        }

        public const int Index = 965;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }
    public class SubsystemSpaceShipBehavior : SubsystemBlockBehavior
    {
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    965
                };
            }
        }

        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            IInventory inventory = componentMiner.Inventory;
            if (Terrain.ExtractContents(componentMiner.ActiveBlockValue) == 965)
            {
                TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging, true, true, true);
                if (terrainRaycastResult != null)
                {
                    Vector3 vector = terrainRaycastResult.Value.HitPoint(0f);
                    DynamicArray<ComponentBody> dynamicArray = new DynamicArray<ComponentBody>();
                    this.m_subsystemBodies.FindBodiesInArea(new Vector2(vector.X, vector.Z) - new Vector2(8f), new Vector2(vector.X, vector.Z) + new Vector2(8f), dynamicArray);
                    if (Enumerable.Count<ComponentBody>(dynamicArray, (ComponentBody b) => b.Entity.ValuesDictionary.DatabaseObject.Name == "SpaceShip1") < 6 || this.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative)
                    {
                        Entity entity = DatabaseManager.CreateEntity(base.Project, "SpaceShip1", true);
                        entity.FindComponent<ComponentFrame>(true).Position = vector;
                        entity.FindComponent<ComponentFrame>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.m_random.Float(0f, 6.2831855f));
                        entity.FindComponent<ComponentSpawn>(true).SpawnDuration = 0f;
                        base.Project.AddEntity(entity);
                        componentMiner.RemoveActiveTool(1);
                        this.m_subsystemAudio.PlaySound("Audio/BlockPlaced", 1f, 0f, vector, 3f, true);
                    }
                    else
                    {
                        ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
                        if (componentPlayer != null)
                        {
                            componentPlayer.ComponentGui.DisplaySmallMessage("太多飞船了", Color.White, true, false);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            this.m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            this.m_subsystemBodies = base.Project.FindSubsystem<SubsystemBodies>(true);
            this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
        }

        public SubsystemAudio m_subsystemAudio;

        public SubsystemBodies m_subsystemBodies;

        public SubsystemGameInfo m_subsystemGameInfo;

        public Random m_random = new Random();

        public static string fName = "SubsystemSpaceShipBehavior";
    }

    /*public class ComponentSpaceShip1 : ComponentBoat, IUpdateable
    {
       
        public Vector3? FlyOrder_;
        public Vector3? FlyOrder
        {
            get
            {
                return this.FlyOrder_;
            }
            set
            {
                this.FlyOrder_ = value;
                if (this.FlyOrder_ != null)
                {
                    float num = this.FlyOrder_.Value.LengthSquared();
                    if (num > 1f)
                    {
                        this.FlyOrder_ = new Vector3?(this.FlyOrder_.Value / MathUtils.Sqrt(num));
                    }
                }
            }
        }

       

       

        public new void Update(float dt)
        {
            ComponentPlayer componentPlayer = m_componentMount.Rider.Entity.FindComponent<ComponentPlayer>();
            ComponentOnFire componentOnFire = base.Entity.FindComponent<ComponentOnFire>();
            bool flag = this.m_componentBody.ImmersionFactor > 0.95f;
            bool flag2 = this.m_componentBody.ImmersionFactor < 0.33f;
            if (this.m_componentDamage.Health == 0f && componentOnFire.IsOnFire)
            {
                SubsystemExplosions subsystemExplosions = base.Project.FindSubsystem<SubsystemExplosions>();
                subsystemExplosions.TryExplodeBlock((int)this.m_componentBody.Position.X, (int)this.m_componentBody.Position.Y, (int)this.m_componentBody.Position.Z, Terrain.MakeBlockValue(201));
            }
            else if (this.m_componentDamage.Health < 0.1f)
            {
                if (this.m_componentDamage.HealthChange < 0f && flag2)
                {
                    componentOnFire.SetOnFire(null, 3f);
                }
            }
            else if (this.m_componentDamage.Health < 0.33f)
            {
                this.m_componentBody.Density = 1.15f;
                if (this.m_componentDamage.Health - this.m_componentDamage.HealthChange >= 0.33f)
                {
                    if (flag2)
                    {
                        componentOnFire.SetOnFire(null, 2f);
                    }
                    else
                    {
                        this.m_subsystemAudio.PlaySound("Audio/Sinking", 1f, 0f, this.m_componentBody.Position, 4f, true);
                    }
                }
            }
            else if (this.m_componentDamage.Health < 0.66f)
            {
                this.m_componentBody.Density = 0.7f;
                if (this.m_componentDamage.Health - this.m_componentDamage.HealthChange >= 0.66f)
                {
                    if (flag2)
                    {
                        componentOnFire.SetOnFire(null, 1f);
                    }
                    else
                    {
                        this.m_subsystemAudio.PlaySound("Audio/Sinking", 1f, 0f, this.m_componentBody.Position, 4f, true);
                    }
                }
            }
            object obj = !flag && !flag2 && this.m_componentBody.StandingOnValue == null && this.m_componentBody.StandingOnBody == null;
            this.m_turnSpeed += 2.5f * this.m_subsystemTime.GameTimeDelta * (1f * base.TurnOrder - this.m_turnSpeed);
            Quaternion rotation = this.m_componentBody.Rotation;
            float num = MathUtils.Atan2(2f * rotation.Y * rotation.W - 2f * rotation.X * rotation.Z, 1f - 2f * rotation.Y * rotation.Y - 2f * rotation.Z * rotation.Z);
            
                if (!flag)
                {
                    Vector3 vector = this.m_componentBody.Velocity;
                    Vector3 vector2 = new Vector3(base.TurnOrder, 0f, base.MoveOrder);
                    if (this.FlyOrder != null)
                    {
                        vector2 += this.FlyOrder.Value;
                        Vector3 right = this.m_componentBody.Matrix.Right;
                        Vector3 v = Vector3.Zero;
                        if (this.m_componentMount.Rider != null)
                        {
                            v = Vector3.Transform(this.m_componentMount.Rider.ComponentCreature.ComponentBody.Matrix.Forward, Quaternion.CreateFromAxisAngle(right, this.m_componentMount.Rider.ComponentCreature.ComponentLocomotion.LookAngles.Y));
                        }
                        bool flag3;
                        if (SettingsManager.HorizontalCreativeFlight)
                        {
                            ComponentRider rider = this.m_componentMount.Rider;
                            if (((rider != null) ? rider.ComponentCreature : null) == null && this.m_componentMount.Rider != null)
                            {
                                flag3 = this.m_componentMount.Rider.Entity.FindComponent<ComponentPlayer>().ComponentInput.IsControlledByTouch;
                                goto IL_3EB;
                            }
                        }
                        flag3 = true;
                    IL_3EB:
                        Vector3 v2 = flag3 ? Vector3.Normalize(v + 0.5f * Vector3.UnitY) : Vector3.Normalize(v * new Vector3(1f, 0f, 1f));
                        Vector3 v3 = base.Entity.FindComponent<ComponentLocomotion>().FlySpeed * (right * vector2.X + Vector3.UnitY * vector2.Y + v2 * vector2.Z);
                        float num2 = (vector2 == Vector3.Zero) ? 6f : 3f;
                        vector += MathUtils.Saturate(base.Entity.FindComponent<ComponentLocomotion>().AccelerationFactor * num2 * dt) * (v3 - vector);
                        if (base.Entity.FindComponent<ComponentLocomotion>().FlySpeed > 0f)
                        {
                            Vector3 value = this.FlyOrder.Value;
                            Vector3 v4 = base.Entity.FindComponent<ComponentLocomotion>().FlySpeed * value;
                            vector += MathUtils.Saturate(2f * base.Entity.FindComponent<ComponentLocomotion>().AccelerationFactor * dt) * (v4 - vector);
                            this.m_componentBody.IsGravityEnabled = this.m_gravityEnabled;
                        }
                        if (MathUtils.Abs(this.m_componentBody.CollisionVelocityChange.Y) > 3f)
                        {
                            base.Entity.FindComponent<ComponentCreature>().ComponentCreatureSounds.PlayFootstepSound(2f);
                            base.Entity.FindComponent<ComponentLocomotion>().m_subsystemNoise.MakeNoise(this.m_componentBody, 0.25f, 10f);
                        }
                    }
                    vector += dt * 50f * base.MoveOrder * this.m_componentBody.Matrix.Forward;
                    this.m_componentBody.Velocity = vector;
                    num -= 3f * this.m_turnSpeed * dt;
                }
                else
                {
                    this.m_componentBody.Velocity += dt * 1f * base.MoveOrder * this.m_componentBody.Matrix.Forward;
                    this.m_componentBody.Density += 0.0001f;
                    this.m_subsystemAudio.PlaySound("Audio/Sinking", 1f, 0f, this.m_componentBody.Position, 4f, true);
                }
            
          
            if (obj != null)
            {
                num -= this.m_turnSpeed * dt;
                if (base.MoveOrder != 0f)
                {
                    this.m_componentBody.Velocity += dt * 3f * base.MoveOrder * this.m_componentBody.Matrix.Forward;
                }
            }
            if (flag)
            {
                this.m_componentDamage.Injure(0.005f * dt, null, false, "Broken part");
                if (this.m_componentMount.Rider != null)
                {
                    this.m_componentMount.Rider.StartDismounting();
                }
            }
            this.m_componentBody.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, num);
            base.MoveOrder = 0f;
            base.TurnOrder = 0f;
            this.FlyOrder = null;
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            this.m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            this.m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            this.m_componentMount = base.Entity.FindComponent<ComponentMount>(true);
            this.m_componentBody = base.Entity.FindComponent<ComponentBody>(true);
            this.m_componentDamage = base.Entity.FindComponent<ComponentHealth>(true);
            this.m_vehicleType = valuesDictionary.GetValue<string>("VehicleType");
            this.m_gravityEnabled = valuesDictionary.GetValue<bool>("GravityEnabled", false);
        }
        public new ComponentHealth m_componentDamage;
        public string m_vehicleType;

        public bool m_gravityEnabled;
    }*/


    public class SpaceShip : Block
    {
        public const int Index = 965;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
        public Texture2D m_texture;
        public override void Initialize()
        {
           Model model = ContentManager.Get<Model>("Models/SpaceShip/starchaser", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("starchaser", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("starchaser", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.4f, 0f), false, false, false, false, new Color(96, 96, 96));
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("starchaser", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.4f, 0f), false, true, false, false, new Color(255, 255, 255));
            m_texture = ContentManager.Get<Texture2D>("Textures/SpaceShip/starchaser", null);  //外置材质
            base.Initialize();
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, 0.5f, ref matrix, environmentData);
        }
    }

    public class SubsystemCarBlockBehavior : SubsystemBlockBehavior
    {
        public SubsystemAudio m_subsystemAudio;

        public SubsystemBodies m_subsystemBodies;

        public SubsystemGameInfo m_subsystemGameInfo;

        public Random m_random = new Random();

        public static string fName = "SubsystemBoatBlockBehavior";

        public override int[] HandledBlocks => new int[1]
        {
            965
        };

        public Color[] color = new Color[4]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gray
        };


        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            _ = componentMiner.Inventory;
            if (Terrain.ExtractContents(componentMiner.ActiveBlockValue) == 965)
            {
                TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging);
                if (terrainRaycastResult.HasValue)
                {
                    Vector3 position = terrainRaycastResult.Value.HitPoint();
                    var dynamicArray = new DynamicArray<ComponentBody>();
                    m_subsystemBodies.FindBodiesInArea(new Vector2(position.X, position.Z) - new Vector2(8f), new Vector2(position.X, position.Z) + new Vector2(8f), dynamicArray);
                    if ((dynamicArray.Count((ComponentBody b) => b.Entity.ValuesDictionary.DatabaseObject.Name == "SpaceShip1") < 3) || m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative)
                    {
                        Entity entity = DatabaseManager.CreateEntity(Project, "SpaceShip1", throwIfNotFound: true);
                        entity.FindComponent<ComponentFrame>(throwOnError: true).Position = position;
                        entity.FindComponent<ComponentFrame>(throwOnError: true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, m_random.Float(0f, (float)Math.PI * 2f));
                        entity.FindComponent<ComponentSpawn>(throwOnError: true).SpawnDuration = 0f;
                        Project.AddEntity(entity);
                        componentMiner.RemoveActiveTool(1);
                        m_subsystemAudio.PlaySound("Audio/BlockPlaced", 1f, 0f, position, 3f, autoDelay: true);
                        componentMiner.ComponentPlayer?.ComponentGui.DisplaySmallMessage("随机颜色", color[m_random.Int(0, 3)], blinking: true, playNotificationSound: false);
                    }
                    else
                    {
                        componentMiner.ComponentPlayer?.ComponentGui.DisplaySmallMessage("车太多了", color[m_random.Int(0, 4)], blinking: true, playNotificationSound: false);
                    }
                    return true;
                }
            }
            return false;
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(throwOnError: true);
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(throwOnError: true);
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(throwOnError: true);
        }
    }

    public class ComponentCar : Component, IUpdateable
    {
        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public ComponentMount m_componentMount;

        public ComponentBody m_componentBody;

        public ComponentDamage m_componentDamage;

        public ComponentPlayer componentPlayer;

        public float m_turnSpeed;

        public float TurnOrder
        {
            get;
            set;
        }

        public float Health
        {
            get;
            set;
        }

        public SubsystemTerrain m_subsystemTerrain;

        public double m_lastAutoJumpTime;

        public float m_jumpStrength;

        public bool isflying=false;
        public bool islanding=true;

        public UpdateOrder UpdateOrder => UpdateOrder.Default;

        public void Update(float dt)
        {
            bool obj1 = this.m_componentBody.StandingOnValue == null && this.m_componentBody.StandingOnBody == null;
            if (m_componentBody.ImmersionFactor <= 0f)
            {
                if(obj1==true&&isflying==false)
                {
                    m_subsystemAudio.PlaySound("Audio/Flyup", 1f, 0f, this.m_componentBody.Position, 4f, true);
                    isflying = true;
                }
                else if(obj1 == false && isflying == true)
                {
                    m_subsystemAudio.PlaySound("Audio/Down", 1f, 0f, this.m_componentBody.Position, 4f, true);
                    isflying = false;
                }

                bool flag = m_subsystemTime.PeriodicGameTimeEvent(2.3, 0.0);
                if(flag&&isflying==true)
                {
                    m_subsystemAudio.PlaySound("Audio/Flying", 1f, 0f, this.m_componentBody.Position, 4f, true);
                }
                BodyJump(dt);
            }
            else
            {
                if (this.m_componentDamage.Hitpoints - this.m_componentDamage.HitpointsChange >= 0.66f && this.m_componentBody.ImmersionFactor > 0f)
                {
                    this.m_subsystemAudio.PlaySound("Audio/Sinking", 0.1f, 0f, this.m_componentBody.Position, 4f, true);
                }
                
                
                BodyJump(dt);

            }
            TurnOrder = 0f;
        }

        public void BodyJump(float dt)
        {
            if (m_componentMount.Rider == null)
            {
                return; // 如果没有骑手，直接返回
            }
            ComponentPlayer componentPlayer = m_componentMount.Rider.Entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
            {
                Vector3 v5 =componentPlayer.GameWidget.ActiveCamera.ViewDirection;
                bool obj =  this.m_componentBody.StandingOnValue == null && this.m_componentBody.StandingOnBody == null;
                var playerInput = componentPlayer.ComponentInput.PlayerInput;
                Vector2 WalkOrder = new Vector2(playerInput.Move.X, playerInput.Move.Z); //x是左右，控制旋转，z是前后
                Vector3 FlyOrder = new Vector3(0f, playerInput.Move.Y, 0f);
                float MoveOrder = WalkOrder.Y;
                Vector3 velocity = m_componentBody.Velocity;
                Vector3 right = m_componentBody.Matrix.Right;
                var vector = Vector3.Transform(m_componentBody.Matrix.Forward, Quaternion.CreateFromAxisAngle(right, componentPlayer.ComponentLocomotion.LookAngles.Y));
                // var v = new Vector3(WalkOrder.X, 0f, WalkOrder.Y);
                var v = new Vector3(0f, 0f,0f);
                v += FlyOrder;//1飞行
                Vector3 v2 = (!SettingsManager.HorizontalCreativeFlight || componentPlayer == null || componentPlayer.ComponentInput.IsControlledByTouch) ? Vector3.Normalize(vector + 0.1f * Vector3.UnitY) : Vector3.Normalize(vector * new Vector3(1f, 0f, 1f));
                Vector3 v3 = 30f * (right * v.X + Vector3.UnitY * v.Y + v2 * v.Z);
                float num = (v == Vector3.Zero) ? 5f : 3f;
                //m_componentBody.Velocity += MathUtils.Saturate(num * dt) * (v3 - m_componentBody.Velocity);
                componentPlayer.ComponentLocomotion.LookOrder = new Vector2(playerInput.Look.X, playerInput.Look.Y);
                if (obj==true)//如果浮空
                {
                    if (playerInput.Move.Y > 0f)
                    {
                        Vector3 v1 = v * 10f;//如果向上飞，10倍速
                        m_componentBody.Velocity += dt * 30f * MoveOrder * this.m_componentBody.Matrix.Forward + v1;
                    }
                    else if (playerInput.Move.Y == 0f)
                    {
                        m_componentBody.m_velocity.Y += 10f * dt;
                        m_componentBody.Velocity += dt * 30f * MoveOrder * this.m_componentBody.Matrix.Forward+v ;
                    }
                    else
                    {
                        float speedrate = MathUtils.Lerp(5f, 10f, MathUtils.Abs( v.Y));
                        m_componentBody.m_velocity.Y += 10f * dt;
                        m_componentBody.Velocity += dt * speedrate * v;
                    }
                    
                }
                else
                {
                    m_componentBody.Velocity += dt * 30f * MoveOrder * this.m_componentBody.Matrix.Forward + v;
                }
                
                if (playerInput.VrMove.HasValue) m_componentBody.ApplyDirectMove(playerInput.VrMove.Value);
                TurnOrder = playerInput.Move.X;
                Quaternion rotation = m_componentBody.Rotation;
                float num3 = MathUtils.Atan2(2f * rotation.Y * rotation.W - 2f * rotation.X * rotation.Z, 1f - 2f * rotation.Y * rotation.Y - 2f * rotation.Z * rotation.Z);
                m_turnSpeed += 2.5f * m_subsystemTime.GameTimeDelta * (1f * TurnOrder - m_turnSpeed);
                num3 -= m_turnSpeed * dt*4f;//倍数转速
                m_componentBody.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, num3);
               // m_componentDamage.Damage(0.005f * dt);
            }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(throwOnError: true);
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(throwOnError: true);
            m_componentMount = Entity.FindComponent<ComponentMount>(throwOnError: true);
            m_componentBody = Entity.FindComponent<ComponentBody>(throwOnError: true);
            m_componentDamage = Entity.FindComponent<ComponentDamage>(throwOnError: true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            isflying = valuesDictionary.GetValue<bool>("isflying",false);
            islanding = valuesDictionary.GetValue<bool>("islanding", true);
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<bool>("islanding", islanding);
            valuesDictionary.SetValue<bool>("isflying", isflying);
            base.Save(valuesDictionary, entityToIdMap);
        }

    }

}
