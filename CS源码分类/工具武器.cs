using Engine.Graphics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class FCGMItemsData
    {
        public FCGMItemsData(string name, string description, string model, string texture)
        {
            this.Name = name;
            this.Description = description;
            this.Model = model;
            this.Texture = texture;
            this.UVCoord = 0;
            this.UVScale = 1f;
            this.Scale = 1f;
            this.Offset = Vector3.Zero;
            this.Rotation = Vector3.Zero;
            this.Color = Color.White;
            this.SA = false;
        }

        public string Name;

        public string Description;

        public string Model;

        public string Texture;

        public int UVCoord;

        public float UVScale;

        public float Scale;

        public Vector3 Offset;

        public Vector3 Rotation;

        public Color Color;

        public bool SA;
    }
    public class FCWeaponBlock : FCPlatBlock
    {
        public const int Index = 989;
        private Texture2D[] m_texture = new Texture2D[20];
        public BlockMesh[] m_meshesByData = new BlockMesh[100];
        public Random m_random = new Random();
        public SubsystemTime m_subsystemTime;
        public static int GetData(string name)
        {
            for (int i = 0; i < 2; i++)
            {
                bool flag = FCWeaponBlock.Datas[i].Name == name;
                if (flag)
                {
                    return i;
                }
            }
            return 0;
        }

        public static int GetValue(string name)
        {
            return Terrain.MakeBlockValue(989, 0, FCWeaponBlock.GetData(name));
        }
        public static int GetIndex(int data)
        {
             return data % 100; 
        }
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
            for (int i = 0; i < 2; i++)
            {
                this.m_texture[i] = ContentManager.Get<Texture2D>(FCWeaponBlock.m_textureNames[i], null);
            }
            /*for (int j = 0; j < 7; j++)
			{
				this.m_frostPowerTexture[j] = ContentManager.Get<Texture2D>(FCWeaponBlock.m_frostPower[j], null);
			}*/
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            int num = GetIndex(Terrain.ExtractData(value));
            float num2 = (num >= 0 && num < FCWeaponBlock.m_sizes.Length) ? (size * FCWeaponBlock.m_sizes[num]) : size;
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
            1f,
            1f,
           
        };

        public override IEnumerable<int> GetCreativeValues()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return Terrain.MakeBlockValue(989, 0, i);
            }

            yield break;

        }
        //手持物数量
        public override int GetMaxStacking(int value)
        {
            int num = GetIndex(Terrain.ExtractData(value));
            return this.m_maxStick[num];
        }

        private int[] m_maxStick = new int[]
        {
            1,
            1,
           
        };
        
        //类名字
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int num = GetIndex(Terrain.ExtractData(value));
            if (num < 0 || num >= FCWeaponBlock.m_displayNames.Length)
            {
                return string.Empty;
            }
            return FCWeaponBlock.m_displayNames[num];
        }

        private static string[] m_displayNames = new string[]
        {
            "英灵手刃",//0
			"恶魔匕首",//1
			


		};

        private static string[] m_textureNames = new string[]
        {
            "Textures/FCWeapon/Heroknife",
            "Textures/FCWeapon/Demonknife",
            

        };
        //动态材质
        private static string[] m_frostPower = new string[]
        {







        };

        private Texture2D[] m_frostPowerTexture = new Texture2D[7];

        public enum ItemType
        {
            Heroknife, //1铜板
            Demoknife, //2铁板
            

        }

        public override string GetDescription(int value)
        {
            int num = GetIndex(Terrain.ExtractData(value));
            if (num < 0 || num >= FCWeaponBlock.m_Description.Length)
            {
                return string.Empty;
            }
            return FCWeaponBlock.m_Description[num];

        }

        private static string[] m_Description = new string[]
        {
            "英灵手刃，来自奇幻世界的武器，如今也随着飘渺的灵力来到了这个世界，继续为玩家征战沙场。（来自奇幻武器的英灵手刃。我记得我第一个学习的参考mod就是奇幻武器包，尽管我从来没联系上这个mod的作者（貌似我来的时候他已经退圈了）。现在我开始制作属于自己mod的武器，我突然想起来了英灵手刃，这个十分好用的前期武器。所以我就拿来测试了。这是颇有纪念意义的，他的武器还是会继续在万象宇宙活下去。）",
            "恶魔匕首，来自奇幻世界的武器，如今也随着飘渺的灵力来到了这个世界，继续为玩家征战沙场。（来自奇幻武器的恶魔匕首。）",
        
        };
        private static int[] m_maxDu = new int[]
        {
            200,
            180,

        };
        private static int[] m_maxPower = new int[]
        {
            15,
            20,

        };
        private static float[] m_maxhitp = new float[]
        {
            0.98f,
            0.7f,

        };
        public override float GetMeleePower(int value)//获取攻击力
        {
            int num = GetIndex(Terrain.ExtractData(value));
            if (num < 0 || num >= FCWeaponBlock.m_maxPower.Length)
            {
                return 1;
            }
            return FCWeaponBlock.m_maxPower[num];
        }

        public override int GetDurability(int value)//获取耐久
        {
            int num = GetIndex(Terrain.ExtractData(value));
            if (num < 0 || num >= FCWeaponBlock.m_maxDu.Length)
            {
                return 1;
            }
            return FCWeaponBlock.m_maxDu[num];
        }
        public override float GetMeleeHitProbability(int value)//获取近战命中率
        {
            int num = GetIndex(Terrain.ExtractData(value));
            if (num < 0 || num >= FCWeaponBlock.m_maxhitp.Length)
            {
                return 1;
            }
            return FCWeaponBlock.m_maxhitp[num];
        }
        public override int GetDamage(int value)
        {
            int num = Terrain.ExtractData(value);
            num = MathUtils.Abs(num);
            return (num % 100000 - num % 100) / 100;//损坏值的上限是6w多，所以取前三位，后两位为data
        }

        public override int SetDamage(int value, int damage)
        {
            int num = Terrain.ExtractData(value);
            num = MathUtils.Abs(num);
            return Terrain.ReplaceData(value, num - num % 100000 + num % 100 + damage * 100);
        }
        public static FCGMItemsData[] Datas = new FCGMItemsData[]
        {
            new FCGMItemsData("英灵手刃", "英灵手刃", "Models/Chunk", "Textures/MeleeWeapons/Crowbar")
            {
               
            },
            new FCGMItemsData("恶魔匕首", "恶魔匕首", "Models/Chunk", "Textures/MeleeWeapons/Crowbar")
            {

            },
        };
    }
}
