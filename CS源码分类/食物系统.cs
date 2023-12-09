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
    #region 1.0食物和食物系统

    public class FCORFoodBlock : FCPlatBlock
    {
        public const int Index = 988;
        private Texture2D[] m_texture = new Texture2D[30];
        public BlockMesh[] m_meshesByData = new BlockMesh[100];
        public Random m_random = new Random();
        public SubsystemTime m_subsystemTime;

        public override int GetTextureSlotCount(int value)
        {
            return 1;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            if (face == -1) return 0;
            return DefaultTextureSlot;
        }
        public override void Initialize()
        {
            for (int i = 0; i < 27; i++)
            {
                this.m_texture[i] = ContentManager.Get<Texture2D>(FCORFoodBlock.m_textureNames[i], null);
            }
            /*for (int j = 0; j < 7; j++)
			{
				this.m_frostPowerTexture[j] = ContentManager.Get<Texture2D>(FCItemBlock.m_frostPower[j], null);
			}*/
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            int num = Terrain.ExtractData(value);
            float num2 = (num >= 0 && num < FCORFoodBlock.m_sizes.Length) ? (size * FCORFoodBlock.m_sizes[num]) : size;
            /*switch (num)
			{
				case 14:
					BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, this.m_frostPowerTexture[Time.FrameIndex / 12 % 7], color, 1.5f * num2, ref matrix, environmentData);
					return;
				
			}*/
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, this.m_texture[num], color, 2f * num2, ref matrix, environmentData);
        }





        private static float[] m_sizes = new float[]
        {
            0.8f,//0
            1f,//1
            1f,//2
            0.7f,//3
            1f,//4
            1f,//5
            0.7f,//6
            1f,//7
            1f,//8
            1f,//9
            0.7f,//10
            1f,//11
            0.7f,//12
            0.7f,//13
            1f,//14
            1f,//15
            1f,//16
            0.7f,//17
            1f,//18
            0.7f,//19
			0.7f,//20
            1f,//21
            1f,//22
            1f,//23
            1f,//24
            1f,//25
            1f,//26
        };

        public override IEnumerable<int> GetCreativeValues()
        {
            foreach (int data in EnumUtils.GetEnumValues(typeof(FCORFoodBlock.ItemType)))
            {
                yield return Terrain.MakeBlockValue(988, 0, data);
            }

            yield break;

        }
        //手持物数量
        public override int GetMaxStacking(int value)
        {
            int num = Terrain.ExtractData(value);
            return this.m_maxStick[num];
        }

        private int[] m_maxStick = new int[]
        {
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,
            100,//20
			100,
            100,
            100,
            100,
            100,
            100,
            100,
        };
        //类名字
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int num = Terrain.ExtractData(value);
            if (num < 0 || num >= FCORFoodBlock.m_displayNames.Length)
            {
                return string.Empty;
            }
            return FCORFoodBlock.m_displayNames[num];
        }

        private static string[] m_displayNames = new string[]
        {
            "蛋糕",
            "啤酒",
            "玻璃瓶",
            "葱",
            "瓜子",
            "汉堡",
            "黄焖鸡",
            "酱油瓶",
            "面包片",
            "青团",
            "清蒸鱼",
            "生瓜子",
            "生鸡块",
            "咸蛋",
            "待处理的肉片",
            "盐",
            "寿司",
            "馅饼",
            "小酥肉",
            "腌鸡肉",//20
			"腌鱼肉",
            "腌肉",
            "油瓶",
            "炸鸡",
            "西瓜片",
            "生巧克力",
            "水瓶",




        };

        private static string[] m_textureNames = new string[]
        {
            "Textures/amod/dangao",//0蛋糕
            "Textures/amod/beer",//1啤酒
            "Textures/amod/boliping",//2玻璃瓶
            "Textures/amod/cong",//3葱
            "Textures/amod/guazi",//4瓜子
            "Textures/amod/hamburger",//5汉堡
            "Textures/amod/huangmengji",//6黄焖鸡
            "Textures/amod/jiangyou",//7酱油
            "Textures/amod/mianbaopian",//8面包片
            "Textures/amod/qingtuan",//9青团
            "Textures/amod/qingzhengyu",//10清蒸鱼
            "Textures/amod/rawguazi",//11生瓜子
            "Textures/amod/rawjikuai",//12生鸡块
            "Textures/amod/shuxiandan",//13咸蛋
            "Textures/amod/readymeat",//14待处理的肉片
            "Textures/amod/salt",//15盐分
            "Textures/amod/shousi",//16寿司
            "Textures/amod/xianbing",//17馅饼
            "Textures/amod/xiaosurou",//18小酥肉
            "Textures/amod/yanchicken", //19腌鸡肉
			"Textures/amod/yanfish",//20腌鱼肉
            "Textures/amod/yanrou",//21腌肉
            "Textures/amod/youping",//22油瓶
            "Textures/amod/zhaji",//23炸鸡
			"Textures/amod/xiguapian",//24西瓜片
			"Textures/amod/rawqiaokeli",//25生巧克力
			"Textures/amod/shuiping",//26水瓶

        };
        //动态材质
        private static string[] m_frostPower = new string[]
        {







        };

        private Texture2D[] m_frostPowerTexture = new Texture2D[7];

        public enum ItemType
        {
            Cake,				//1蛋糕
            Beer,				//2啤酒
            Boliping,			//3玻璃瓶
            Cong,				//4.葱
            Guazi,				//5.瓜子
            Hamburger,			//6汉堡
            Huangmengji,		//7黄焖鸡
            Jiangyou,			//8酱油瓶
            Mianbaopian,		//9面包片
            Qingtuan,			//10青团
            Qingzhengyu,		//11清蒸鱼
            Rawguazi,		    //12生瓜子
            Rawjikuai,          //13生鸡块
            Rawxiandan,			//14咸蛋
            Readymeat,			//15待处理的肉
            Salt,			    //16盐
            Shousi,             //17寿司
            Xianbing,			//18馅饼筒
            Xiaosurou,			//19小酥肉
            Yanchicken,         //20腌鸡肉
            Yanfish,            //21腌鱼肉
            Yanrou,             //22腌肉
            Youping,            //23油瓶
            Zhaji,              //24炸鸡
            Xiguapian,          //25西瓜片
            Rawqiaokeli,         //26生巧克力
            Shuiping,            //27水瓶


        }

        public override string GetDescription(int value)
        {
            int num = Terrain.ExtractData(value);
            if (num < 0 || num >= FCORFoodBlock.m_Description.Length)
            {
                return string.Empty;
            }
            return FCORFoodBlock.m_Description[num];

        }

        private static string[] m_Description = new string[]
        {
            "蛋糕",
            "啤酒",
            "玻璃瓶",
            "葱",
            "瓜子",
            "汉堡",
            "黄焖鸡",
            "酱油瓶",
            "面包片",
            "青团",
            "清蒸鱼",
            "生瓜子",
            "生鸡块",
            "咸蛋",
            "待处理的肉片",
            "盐",
            "寿司",
            "馅饼",
            "小酥肉",
            "腌鸡肉",//20
			"腌鱼肉",
            "腌肉",
            "油瓶",
            "炸鸡",
            "西瓜片",
            "生巧克力",
            "水瓶",



        };
    }//原始食物988

    public class FoodSystem : SubsystemBlockBehavior
    {

        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                  988,//食物
				};
            }
        }

        public Random random = new Random();

        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            //1 heal 2.Atk 3.速度
            ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
            BuffManager buffManager = new BuffManager(componentPlayer);
            var m_componentTest1 = componentPlayer.Entity.FindComponent<ComponentTest1>();
            int F988ID = Terrain.ExtractContents(componentMiner.ActiveBlockValue);//先获取Id，再获取特殊值
            if (componentPlayer != null)
            {

                if (F988ID == 988)
                {
                    int id = Terrain.ExtractData(componentMiner.ActiveBlockValue);
                    if (id == 0)//蛋糕
                    {
                        if (m_componentTest1 != null)
                        {
                            m_componentTest1.m_sen += 15f;
                        }

                        componentPlayer.ComponentGui.DisplaySmallMessage("吃了蛋糕，你感到很幸福……sen+15", Color.DarkRed, true, false);
                        AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (id == 1)//啤酒
                    {
                        if (m_componentTest1 != null)
                        {
                            m_componentTest1.m_sen += 20f;
                        }

                        componentPlayer.ComponentGui.DisplaySmallMessage("啤酒让你晕乎乎的~……sen+20！", Color.DarkRed, true, false);
                        float num3 = MathUtils.DegToRad(MathUtils.Lerp(-35f, -65f, SimplexNoise.Noise(4f * (float)MathUtils.Remainder(this.m_SubsystemTime.GameTime, 10000.0))));
                        componentPlayer.ComponentBody.ApplyImpulse(-1.2f * componentPlayer.ComponentCreatureModel.EyeRotation.GetForwardVector());
                        componentPlayer.ComponentLocomotion.LookOrder = new Vector2(componentPlayer.ComponentLocomotion.LookOrder.X, MathUtils.Clamp(num3 - componentPlayer.ComponentLocomotion.LookAngles.Y, -3f, 3f));
                        buffManager.AddBuff(4, 10, 0.9f);//致盲
                        componentPlayer.ComponentCreatureSounds.PlayMoanSound();
                        AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (id == 4 || id == 5 || id == 6 || id == 8 || id == 9 || id == 10 || id == 13 || id == 16 || id == 17 || id == 18 || id == 24)
                    {
                        if (m_componentTest1 != null)
                        {
                            m_componentTest1.m_sen += 10f;
                        }

                        componentPlayer.ComponentGui.DisplaySmallMessage("美食让你感到心情愉悦~……sen+10.", Color.DarkRed, true, false);
                        AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }


                    else if (id == 3)//葱治疗感冒
                    {
                        if (random.Bool(0.7f))
                        {
                            componentPlayer.ComponentSickness.m_sicknessDuration -= 10f;
                            componentPlayer.ComponentFlu.m_fluDuration -= 10f;
                        }
                        componentPlayer.ComponentGui.DisplaySmallMessage("葱有概率能治疗感冒和疾病，但不能充饥(疾病时长减去10）", Color.DarkRed, true, false);
                        AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (id == 23)//炸鸡
                    {
                        if (m_componentTest1 != null)
                        {
                            m_componentTest1.m_sen += 30f;
                        }

                        componentPlayer.ComponentGui.DisplaySmallMessage("炸鸡对于流落荒岛的你来说无疑是绝味珍馐，你感到幸福极了！sen+30！", Color.DarkRed, true, false);
                        AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (id == 7 || id == 11 || id == 12 || id == 14 || id == 25)//半生不熟的东西
                    {
                        if (random.Bool(0.5f))
                        {

                            if (m_componentTest1 != null && componentPlayer.ComponentSickness.m_sicknessDuration <= 0)
                            {
                                componentPlayer.ComponentSickness.StartSickness();
                                m_componentTest1.m_sen -= 20f;
                            }
                        }
                        componentPlayer.ComponentGui.DisplaySmallMessage("半生不熟和奇怪的食物有较大概率让你得病！不要再吃了", Color.White, true, false);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (id == 13 || id == 19 || id == 20 || id == 21)//腌制品
                    {
                        if (m_componentTest1 != null)
                        {
                            m_componentTest1.m_sen += 5f;
                        }
                        if (random.Bool(0.05f))
                        {

                            if (m_componentTest1 != null && componentPlayer.ComponentSickness.m_sicknessDuration <= 0)
                            {
                                m_componentTest1.m_sen -= 20f;
                                componentPlayer.ComponentSickness.StartSickness();
                            }
                        }
                        componentPlayer.ComponentGui.DisplaySmallMessage("腌制品口味独特，但是吃多了容易生病……", Color.DarkRed, true, false);
                        AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    if (id != 3)
                    {
                        componentPlayer.ComponentVitalStats.Food += 0.2f;//饱食度恒+2
                    }


                }


            }

            return true;
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_SubsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);


        }

        public ComponentTest1 m_componentTest1;
        public ComponentHealth componentHealth;
        public SubsystemTime m_SubsystemTime;
        public ComponentVitalStats componentVitalStats;

        public ComponentSickness componentSickness;

        public SubsystemTerrain m_subsystemTerrain;
    }//食物系统





    #endregion
}
