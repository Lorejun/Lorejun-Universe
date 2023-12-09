using Engine.Graphics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;
using Test1;


namespace Game
{
    public class ManyBlockData//定义一个方块数据类
    {
        public int Index;//这个必要用到的辅助者，顺序要像添加衣服


        public int TextureIndex;//用int选取方块材质，与原版的blocks是一致
    }
    public class PlantsBlocks : Block
    {
        public const int Index = 986;

        public Texture2D m_texture;

        public ManyBlockData[] m_datas = new ManyBlockData[256];

        public List<ManyBlockData> m_datas2 = new List<ManyBlockData>();//定义所有的方块类型

        public override void Initialize()
        {
            for (int i = 0; i < 79; i++)
            {
                m_datas2.Add(new ManyBlockData()
                {
                    Index = i,

                    TextureIndex = i
                });
            }

               
           

            foreach (var data in m_datas2)//遍历所有方块类型
            {
                for (int i = 0; i < m_datas2.Count; i++)
                {
                    if (data.Index == i)
                    {
                        m_datas[i] = data;//给List弄成数组元素,便于操作
                    }
                }
            }
            m_texture = ContentManager.Get<Texture2D>("Textures/Plants");//获取贴图
            MoneyManager.Initialize();
            base.Initialize();
        }

        public override int GetFaceTextureSlot(int face, int value)
        {
            return m_datas[Terrain.ExtractData(value)].TextureIndex;//返回获取贴图所用到的index
        }
        private static string[] m_displayNames = new string[]
        {

            "极寒菇", // 1 极寒地带，
            "血莲蓬", // 2//血泪之池水
            "绿菇", // 3
            "蓝银微笑菇", // 4
            "幽冥菇", // 5
            "草菇",   // 6 T
            "硫磺菌", // 7 岩浆         
            "赤红帽", // 8
            "绯红伞", // 9
            "大白口蘑", // 10 T
            "蜜环菌", // 11 T
            "墨汁鬼伞", // 12 T
            "粗杆菇", // 13
            "香菇", // 14 T
            "杏鲍菇", // 15 T
            "牛肝茵", // 16 T
            "毒蝇蕈", // 17 T
            "血肉菌", // 18 血泪之池
            "石墨团菌", // 19
            "恶魔菇", // 20
            "蓝环菌", // 21          
            "蓝顶白褶菌", // 22
            "三棱光暗菇", // 23
            "鬼头菌群", // 24
            "“蝴蝶”菌", // 25
            "紫帽子菌", // 26
            "三棱奶酪菌", // 27
            "毒蘑菇", // 28   T
            "巧克力菌", // 29
            "“悲伤”", // 30
            "白顶青褶菌", // 31
            "赭红拟口蘑", // 32 T
            "羊肚菌", // 33 T
            "豹斑毒鹅膏菌", // 34 T
            "拟虫", // 35
            "迷幻菇", // 36
            "拟虫", // 37
            "霓裳藤", // 38
            "晓光莓", // 39
            "暗影草", // 40
            "翡翠苔", // 41
            "琉璃兰", // 42
            "银雾蕨", // 43
            "龙鳞藻", // 44
            "星河苇", // 45
            "朝露菲", // 46
            "炎阳芝", // 47
            "虹光苔", // 48
            "雷鸣根", // 49
            "碧波荇", // 50
            "彩虹蕊", // 51
            "暗夜萝", // 52
            "翼叶蕙", // 53
            "珍珠菰", // 54
            "云雾杉", // 55
            "飞絮柳", // 56
            "空中葵", // 57
            "漫天星", // 58
            "霜晶草", // 59
            "羽毛藓", // 60
            "风铃花", // 61
            "水晶蔓", // 62
            "海潮苹", // 63
            "七彩葵", // 64
            "夜光菌", // 65
            "梦境草", // 66
            "雪绒花", // 67
            "天边云", // 68
            "翠影竹", // 69
            "隐匿蓑", // 70
            "尘世蔷", // 71
            "珊瑚蕉", // 72
            "幽兰绒", // 73
            "寒露瓣", // 74
            "火石榴", // 75
            "镜面草", // 76
            "玉露葵", // 77
            "银河萍", // 78
            "朱砂藤", // 79
        };
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)//重写命名方法
        {
            return m_displayNames[Terrain.ExtractData(value)];//提取列表信息，并进行反馈新赋值
        }
        private static string[] m_Description = new string[]
        {
            "极寒菇", // 1 极寒地带，
            "血莲蓬", // 2//血泪之池水
            "绿菇", // 3
            "蓝银微笑菇", // 4
            "幽冥菇", // 5
            "草菇",   // 6 T
            "硫磺菌", // 7 岩浆         
            "赤红帽", // 8
            "绯红伞", // 9
            "大白口蘑", // 10 T
            "蜜环菌", // 11 T
            "墨汁鬼伞", // 12 T
            "粗杆菇", // 13
            "香菇", // 14 T
            "杏鲍菇", // 15 T
            "牛肝茵", // 16 T
            "毒蝇蕈", // 17 T
            "血肉菌", // 18 血泪之池
            "石墨团菌", // 19
            "恶魔菇", // 20
            "蓝环菌", // 21          
            "蓝顶白褶菌", // 22
            "三棱光暗菇", // 23
            "鬼头菌群", // 24
            "“蝴蝶”菌", // 25
            "紫帽子菌", // 26
            "三棱奶酪菌", // 27
            "毒蘑菇", // 28   T
            "巧克力菌", // 29
            "“悲伤”", // 30
            "白顶青褶菌", // 31
            "赭红拟口蘑", // 32 T
            "羊肚菌", // 33 T
            "豹斑毒鹅膏菌", // 34 T
            "拟虫", // 35
            "迷幻菇", // 36
            "拟虫", // 37
            "霓裳藤", // 38
            "晓光莓", // 39
            "暗影草", // 40
            "翡翠苔", // 41
            "琉璃兰", // 42
            "银雾蕨", // 43
            "龙鳞藻", // 44
            "星河苇", // 45
            "朝露菲", // 46
            "炎阳芝", // 47
            "虹光苔", // 48
            "雷鸣根", // 49
            "碧波荇", // 50
            "彩虹蕊", // 51
            "暗夜萝", // 52
            "翼叶蕙", // 53
            "珍珠菰", // 54
            "云雾杉", // 55
            "飞絮柳", // 56
            "空中葵", // 57
            "漫天星", // 58
            "霜晶草", // 59
            "羽毛藓", // 60
            "风铃花", // 61
            "水晶蔓", // 62
            "海潮苹", // 63
            "七彩葵", // 64
            "夜光菌", // 65
            "梦境草", // 66
            "雪绒花", // 67
            "天边云", // 68
            "翠影竹", // 69
            "隐匿蓑", // 70
            "尘世蔷", // 71
            "珊瑚蕉", // 72
            "幽兰绒", // 73
            "寒露瓣", // 74
            "火石榴", // 75
            "镜面草", // 76
            "玉露葵", // 77
            "银河萍", // 78
            "朱砂藤", // 79
        };
        public override string GetDescription(int value)
        {
            
            return m_Description[Terrain.ExtractData(value)];//提取列表信息，并进行反馈新赋值
        }

        public override int GetDamage(int value)
        {
            return (Terrain.ExtractData(value) >> 8) & 0xF;//重新定义损坏的行为
        }

        public override int SetDamage(int value, int damage)
        {
            int num = Terrain.ExtractData(value);
            num = ((num & -3841) | ((damage & 0xF) << 8));
            return Terrain.ReplaceData(value, num);//重新定义设置损坏行为
        }

        public override IEnumerable<int> GetCreativeValues()
        {
            for (int i = 0; i < m_datas2.Count; i++)
            {
                yield return Terrain.MakeBlockValue(986, 0, i);//返回所有的特殊值,给予能够观看的功能
            }
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);//绘制地形顶点
        }

        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, m_datas[Terrain.ExtractData(value)].TextureIndex, m_texture);//重新定义方块破坏粒子
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);//渲染手持方块
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
           
           
                //int value = Terrain.ExtractContents(oldValue);

                dropValues.Add(new BlockDropValue//控制额外掉落
                {
                    Value = oldValue,
                    Count = 1
                });
            
            showDebris = true;
        }
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return this.m_collisionBoxes;
        }
        public BoundingBox[] m_collisionBoxes = new BoundingBox[] //碰撞箱
		{
           new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(1f, 0.0625f, 1f))
          // new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f))
        };
        public override bool HasCollisionBehavior_(int value)
        {
            return true;
        }
    }

    public class NewPlantSystem : SubsystemBlockBehavior
    {
        public SubsystemAudio m_subsystemAudio;
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                  
					986,//新植物
				};
            }
        }

        private string[] foodmessages = new string[] // 声明并初始化字符串数组
       {
            "这些菌子很美味！",
            "蘑菇好吃，但不能乱吃，请注意分辨不可食用的蘑菇",
            "如果不知道蘑菇能不能吃，可以查看帮助图鉴",
            "该死，这些菌子真好吃！",
            "美味！或许把他们做成菜是个不错的选择。"
       };
        public Random random = new Random();

        


        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging, true, true, true);
            if(terrainRaycastResult==null)
            {
                //1 heal 2.Atk 3.速度
                ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
                BuffManager buffManager = new BuffManager(componentPlayer);
                var m_componentTest1 = componentPlayer.Entity.FindComponent<ComponentTest1>();
                int F986ID = Terrain.ExtractContents(componentMiner.ActiveBlockValue);//先获取Id，再获取特殊值
                if (componentPlayer != null)
                {

                    if (F986ID == 986)
                    {
                        int id = Terrain.ExtractData(componentMiner.ActiveBlockValue);
                        if (id == 1)//血莲蓬
                        {
                            if (m_componentTest1 != null)
                            {
                                m_componentTest1.m_sen -= 1f;
                            }
                            if (componentPlayer.ComponentMiner.AttackPower < 3)
                            {
                                buffManager.AddBuff(2, 10, 10);
                            }
                            componentPlayer.ComponentGui.DisplaySmallMessage("啊……这些血肉一样的蘑菇真不错！（sen-1，攻击力加1，封顶3）", Color.DarkRed, true, false);
                            AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                            componentPlayer.ComponentCreatureSounds.PlayMoanSound();
                            componentMiner.RemoveActiveTool(1);
                        }
                        else if (id == 17)//血肉菌
                        {
                            if (m_componentTest1 != null)
                            {
                                m_componentTest1.m_sen -= 2f;
                            }
                            if (componentPlayer.ComponentMiner.AttackPower < 3)
                            {
                                buffManager.AddBuff(2, 10, 10);
                            }
                            componentPlayer.ComponentGui.DisplaySmallMessage("更多的血肉！（sen-2！攻击力加1，封顶3）", Color.DarkRed, true, false);
                            buffManager.AddBuff(4, 5, 0.3f);//致盲
                            componentPlayer.ComponentCreatureSounds.PlayMoanSound();
                            AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                            componentMiner.RemoveActiveTool(1);
                        }
                        else if (id == 5 || id == 9 || id == 10 || id == 13 || id == 14 || id == 15 || id == 32)//普通蘑菇
                        {
                            if (m_componentTest1 != null)
                            {
                                m_componentTest1.m_sen += 10f;
                            }
                            int index = random.Int(0, 4);
                            componentPlayer.ComponentGui.DisplaySmallMessage(foodmessages[index], Color.DarkRed, true, false);
                            AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                            componentMiner.RemoveActiveTool(1);
                        }



                        else if (id == 11 || id == 16 || id == 27 || id == 31 || id == 33)//毒蘑菇
                        {


                            if (m_componentTest1 != null && componentPlayer.ComponentSickness.m_sicknessDuration <= 0)
                            {
                                componentPlayer.ComponentSickness.StartSickness();
                                componentPlayer.ComponentFlu.StartFlu();
                                m_componentTest1.m_sen -= 30f;
                            }

                            componentPlayer.ComponentGui.DisplaySmallMessage("你吃了毒蘑菇！你感觉难受的想死……（sen-20！）", Color.White, true, false);
                            AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                            componentPlayer.ComponentCreatureSounds.PlayMoanSound();
                            componentMiner.RemoveActiveTool(1);
                        }

                        if (id != 11 && id != 16 && id != 27 && id != 31 && id != 33)
                        {
                            componentPlayer.ComponentVitalStats.Food += 0.2f;//饱食度恒+2
                        }


                    }


                }
                return true;
            }
            else
            {
                return false;
            }
            

           
        }


        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);


        }

        public ComponentTest1 m_componentTest1;
        public ComponentHealth componentHealth;

        public ComponentVitalStats componentVitalStats;

        public ComponentSickness componentSickness;

        public SubsystemTerrain m_subsystemTerrain;
    }
    public class FCSubsystemPlantBlockBehavior : SubsystemPollableBlockBehavior, IUpdateable
    {
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    986
                };
            }
        }

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }
        public override void OnCollide(CellFace cellFace, float velocity, ComponentBody componentBody)
        {
            ComponentCreature componentCreature = componentBody.Entity.FindComponent<ComponentCreature>();
            ComponentPlayer componentPlayer = componentBody.Entity.FindComponent<ComponentPlayer>();
            if (componentPlayer == null)
            {
                return;
            }
            else
            {
                
                BuffManager buffManager = new BuffManager(componentPlayer);
                int x = (int)componentBody.Position.X;
                int y = (int)componentBody.Position.Y;
                int z = (int)componentBody.Position.Z;
                int num = Terrain.ExtractContents(SubsystemTerrain.Terrain.GetCellValue(x, y, z));
                if (num == 986)
                {
                    int data1 = Terrain.ExtractData(num);
                    if (data1 == 0)
                    {
                        buffManager.AddBuff(6, 3f);
                    }



                }
                
                componentCreature.ComponentHealth.Injure(0.01f * MathUtils.Abs(velocity), null, false, "Spiked by cactus");

            }

        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            int num = Terrain.ExtractContents(SubsystemTerrain.Terrain.GetCellValue(x, y, z));
            int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 1, z);
            int num2 = Terrain.ExtractContents(cellValue);

            if (num2 != 8 && num2 != 2 && num2 != 7 && num2 != 168 && num2 != 3 && num2 != 4 && num2 != 5 && num2 != 6 && num2 != 62 )
            {
                base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
                return;
            }




            Block block = BlocksManager.Blocks[num2];
            if (block.IsFaceTransparent(base.SubsystemTerrain, 4, cellValue) && !(block is FenceBlock))
            {
                base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
                return;
            }



            
        }

        public override void OnPoll(int value, int x, int y, int z, int pollPass)
        {
            if (this.m_subsystemGameInfo.WorldSettings.EnvironmentBehaviorMode != EnvironmentBehaviorMode.Living || y <= 0 || y >= 255)
            {
                return;
            }
            int num = Terrain.ExtractContents(value);
            Block block = BlocksManager.Blocks[num];

        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            this.m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
        }

        public void Update(float dt)
        {
            /*if (this.m_subsystemTime.PeriodicGameTimeEvent(30.0, 0.0))
            {
                foreach (KeyValuePair<Point3, SubsystemPlantBlockBehavior.Replacement> keyValuePair in this.m_toReplace)
                {
                    Point3 key = keyValuePair.Key;
                    if (Terrain.ReplaceLight(base.SubsystemTerrain.Terrain.GetCellValue(key.X, key.Y, key.Z), 0) == Terrain.ReplaceLight(keyValuePair.Value.RequiredValue, 0))
                    {
                        base.SubsystemTerrain.ChangeCell(key.X, key.Y, key.Z, keyValuePair.Value.Value, true);
                    }
                }
                this.m_toReplace.Clear();
            }*/
        }



        public SubsystemTime m_subsystemTime;

        public SubsystemGameInfo m_subsystemGameInfo;

        public Random m_random = new Random();

        public Dictionary<Point3, SubsystemPlantBlockBehavior.Replacement> m_toReplace = new Dictionary<Point3, SubsystemPlantBlockBehavior.Replacement>();

        public struct Replacement
        {
            public int RequiredValue;

            public int Value;
        }
    }
}
