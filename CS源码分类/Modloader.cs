using Engine;
using Engine.Graphics;
using Engine.Media;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;
using Test1;

namespace Game
{
    #region 村庄坐标储存子系统
    public class FCSubsystemTown : Subsystem //村庄坐标子系统
    {

        private static Vector3 initialSpawnPosition = new Vector3(100, 65, 100);
        private static bool hasRecordedSpawnPoint = false;
        private static Vector3 spawnPosition;

        public static List<Point3> Village_start = new List<Point3>(); //记录村庄起点。
        public Vector3 GetPlayerPosition()
        {
            if (!hasRecordedSpawnPoint)
            {
                SubsystemPlayers m_subsystemplayers = new SubsystemPlayers();
                ReadOnlyList<PlayerData> player_data = m_subsystemplayers.PlayersData;
                Vector3 position1 = m_subsystemplayers.GlobalSpawnPosition;
                foreach (PlayerData playerData in m_subsystemplayers.PlayersData)
                {
                    spawnPosition = playerData.SpawnPosition;
                    hasRecordedSpawnPoint = true;
                    break;
                }
            }

            return hasRecordedSpawnPoint ? spawnPosition : initialSpawnPosition;
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            Village_start.Clear();

            ValuesDictionary value = valuesDictionary.GetValue<ValuesDictionary>("Towns");
            for (int i = 0; i < valuesDictionary.GetValue<int>("Count", 0); i++)
            {
                ValuesDictionary value2 = value.GetValue<ValuesDictionary>("Town" + i.ToString(CultureInfo.InvariantCulture), null);
                if (value2 != null)
                {
                    Village_start.Add(value2.GetValue<Point3>("Point"));
                }
            }
        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
            valuesDictionary.SetValue<int>("Count", Village_start.Count);
            ValuesDictionary valuesDictionary2 = new ValuesDictionary();
            valuesDictionary.SetValue<ValuesDictionary>("Towns", valuesDictionary2);
            int num = 0;
            foreach (Point3 value in Village_start)
            {
                ValuesDictionary valuesDictionary3 = new ValuesDictionary();
                valuesDictionary2.SetValue<ValuesDictionary>("Town" + num.ToString(CultureInfo.InvariantCulture), valuesDictionary3);
                valuesDictionary3.SetValue<Point3>("Point", value);
                num++;
            }
        }


    }
    #endregion
    #region 村庄区块储存子系统
    public class FCSubsystemTownChunk : Subsystem //村庄坐标子系统
    {
        public static Dictionary<Point2, int> Dic_Chunk_Village = new Dictionary<Point2, int>();  //村庄区块
        NewModLoaderShengcheng m_modloader = new NewModLoaderShengcheng();



        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_modloader.tg_num = 0;//强行中止
            m_modloader.Bg_num = 0;
            NewModLoaderShengcheng.listRD.Clear();
            NewModLoaderShengcheng.listBD.Clear();
            ValuesDictionary dicChunkVillage = valuesDictionary.GetValue<ValuesDictionary>("Dic_Chunk_Village", null);
            if (dicChunkVillage != null)
            {
                Dic_Chunk_Village.Clear();
                NewModLoaderShengcheng.Dic_Chunk_Village3.Clear();
                foreach (KeyValuePair<string, object> kvp in dicChunkVillage)
                {
                    string[] key = kvp.Key.Split(new char[] { ',' }, StringSplitOptions.None);
                    bool flag = int.TryParse(key[0], out int num);
                    bool flag2 = int.TryParse(key[1], out int num2);
                    if (flag && flag2)
                    {
                        Dic_Chunk_Village.Add(new Point2(num, num2), (int)kvp.Value);
                    }

                }
            }
            else
            {
                Dic_Chunk_Village.Clear();
                Dic_Chunk_Village.Add((0, 0), 0);
            }

        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
            if (m_modloader.tg_num == 0)
            {
                Dictionary<Point2, int> Dic_Chunk_Village2 = NewModLoaderShengcheng.Dic_Chunk_Village3;  //获取modlaoder的字典
                ValuesDictionary dicChunkVillage = new ValuesDictionary();//创建一个根元素，从属于子系统主根元素
                foreach (KeyValuePair<Point2, int> kvp in Dic_Chunk_Village2)
                {
                    string key = kvp.Key.ToString();
                    int value = kvp.Value;
                    dicChunkVillage.SetValue<int>(key, value);
                }
                valuesDictionary.SetValue<ValuesDictionary>("Dic_Chunk_Village", dicChunkVillage);
            }


        }


    }
    #endregion

    #region Modloader生成区
    public class NewModLoaderShengcheng : ModLoader
    {
        public ComponentTest1 m_componentTest1;
        public ComponentPlayer m_componentPlayer;

        public List<Point3> Village_start = FCSubsystemTown.Village_start;
        //public List<Point3> Village_start => m_subsystemTerrain.Project.FindSubsystem<FCSubsystemTown>().Village_start; //记录村庄起点。
        public SubsystemGameInfo m_subsystemGameInfo;
        public bool TGExtras;
        public int m_seed;
        public SubsystemTerrain m_subsystemTerrain;
        public Vector2 m_temperatureOffset;
        public Vector2 m_humidityOffset;
        public float TGBiomeScaling;
        public WorldSettings m_worldSettings;
        public SubsystemParticles m_subsystemParticles;
        public bool TGCavesAndPockets;
        public TerrainSerializer23 m_terrainSerializer23;
        public FCSubsystemTown m_subsystemtown;
        public FCSubsystemTownChunk m_subsystemtownchunk;
        public SubsystemSky m_subsystemSky;
        public SubsystemNaturallyBuildings m_subsystemNaturallyBuildings;
        public SubsystemBodies m_subsystemBodies;
        public bool IsNightVisionActive { get; set; }
        public SubsystemPlayers m_subsystemPlayers;
        public SubsystemWorldDemo m_subsystemworldDemo;
        //public  Dictionary<Point2, int> Dic_Chunk_Village ;  //村庄区块

        public override void OnLoadingFinished(List<System.Action> actions)
        {
            actions.Add(delegate {
                XCraftingRecipesManager.Initialize();
                
            });
        }
        public override void __ModInitialize()
        {
            ModsManager.RegisterHook("OnTerrainContentsGenerated", this);
            ModsManager.RegisterHook("OnProjectLoaded", this);
            ModsManager.RegisterHook("AttackBody", this);
            ModsManager.RegisterHook("InitializeCreatureTypes", this);
            ModsManager.RegisterHook("ToFreeChunks", this);
            ModsManager.RegisterHook("ToAllocateChunks", this);
            ModsManager.RegisterHook("CalculateLighting", this);//这个获取光照的钩子是有问题的
            ModsManager.RegisterHook("OnCreatureInjure", this);
            ModsManager.RegisterHook("SetRainAndSnowColor", this);
            ModsManager.RegisterHook("OnModelRendererDrawExtra", this);
            ModsManager.RegisterHook("OnEntityAdd", this);
            ModsManager.RegisterHook("OnLoadingFinished", this);
            SubsystemNaturallyBuildings.Initialize();
            
        }
        public override void OnEntityAdd(Entity entity)
        {
            ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
            {
                m_componentPlayer = componentPlayer;
                m_componentTest1 =  m_componentPlayer.Entity.FindComponent<ComponentTest1>();
               
            }
            base.OnEntityAdd(entity);
            
        }
        public override bool SetRainAndSnowColor(ref Color rainColor, ref Color snowColor)
        {
            
                if (m_componentTest1.Areaname== "血泪之池")
                {
                    rainColor = Color.DarkRed;
                    snowColor = Color.DarkRed;
                    return false;
                }
                else
                {

                    return false;
                }
            
           
        }

        public override void OnProjectLoaded(Project project)
        {

            m_subsystemGameInfo = project.FindSubsystem<SubsystemGameInfo>();
            m_subsystemNaturallyBuildings = project.FindSubsystem<SubsystemNaturallyBuildings>();
            m_seed = m_subsystemGameInfo.WorldSeed;
            m_subsystemTerrain = project.FindSubsystem<SubsystemTerrain>();
            m_worldSettings = m_subsystemGameInfo.WorldSettings;
            m_subsystemParticles = project.FindSubsystem<SubsystemParticles>();
            m_subsystemtown = project.FindSubsystem<FCSubsystemTown>();
            m_subsystemtownchunk = project.FindSubsystem<FCSubsystemTownChunk>();
            m_subsystemPlayers = project.FindSubsystem<SubsystemPlayers>();
            m_subsystemworldDemo = project.FindSubsystem<SubsystemWorldDemo>();
            TGExtras = true;
            TGCavesAndPockets = true;
            m_subsystemBodies=project.FindSubsystem<SubsystemBodies>();
            
            


        }
       
        public override void OnModelRendererDrawExtra(SubsystemModelsRenderer modelsRenderer, SubsystemModelsRenderer.ModelData modelData , Camera camera, float? alphaThreshold)
        {
            ComponentModel componentModel = modelData.ComponentModel;
            
            //ComponentLevel componentLevel = componentModel.Entity.FindComponent<ComponentLevel>();
            //如果被骑乘就不显示
            // Entity entity1 = componentModel.Entity;
            ComponentBody componentBody = componentModel.Entity.FindComponent<ComponentBody>();
            ComponentDamage componentDamage;
            if(componentBody!=null)
            {
                if (componentModel != null && componentBody.ParentBody == null)
                {
                    ComponentCreature componentCreature = componentModel.Entity.FindComponent<ComponentCreature>();
                    ComponentHealth componentHealth = componentModel.Entity.FindComponent<ComponentHealth>();




                    if (componentHealth == null) return;
                    Vector3 position = Vector3.Transform(componentCreature.ComponentBody.Position + 1.2f * Vector3.UnitY * componentCreature.ComponentBody.BoxSize.Y, camera.ViewMatrix);
                    if (position.Z < 0f)
                    {
                        Color color = Color.Lerp(Color.White, Color.Transparent, MathUtils.Saturate((position.Length() - 4f) / 3f));
                        if (color.A > 8)
                        {
                            float light = 15f;
                            Color borderColor = Color.White * new Color(light, light, light, 255);
                            Color fontColor = new Color((1f - componentHealth.Health), componentHealth.Health, 0) * new Color(light, light, light);
                            Vector3 hpos = position + Vector3.UnitY * 0.3f;
                            Vector3 right = Vector3.TransformNormal(0.005f * Vector3.Normalize(Vector3.Cross(camera.ViewDirection, Vector3.UnitY)), camera.ViewMatrix);
                            Vector3 down = Vector3.TransformNormal(-0.005f * Vector3.UnitY, camera.ViewMatrix);
                            BitmapFont font = LabelWidget.BitmapFont;
                            FontBatch3D fontBatch3D = modelsRenderer.PrimitivesRenderer.FontBatch(font, 1, DepthStencilState.DepthRead, RasterizerState.CullNoneScissor, BlendState.AlphaBlend, SamplerState.LinearClamp);
                            FlatBatch3D flatBatch3D = modelsRenderer.PrimitivesRenderer.FlatBatch(1, DepthStencilState.DepthRead, RasterizerState.CullNoneScissor, BlendState.AlphaBlend);
                            //画线框
                            Vector3 p1 = hpos + new Vector3(-0.51f, 0.01f, 0f);
                            Vector3 p2 = hpos + new Vector3(0.51f, 0.01f, 0f);
                            Vector3 p3 = hpos + new Vector3(0.51f, -0.13f, 0f);
                            Vector3 p4 = hpos + new Vector3(-0.51f, -0.13f, 0f);
                            Vector3 p5 = hpos + new Vector3(-0.5f, 0f, 0f);
                            Vector3 p6 = hpos + new Vector3(-0.5f + componentHealth.Health, 0f, 0f);
                            Vector3 p7 = hpos + new Vector3(-0.5f + componentHealth.Health, -0.12f, 0f);
                            Vector3 p8 = hpos + new Vector3(-0.5f, -0.12f, 0f);
                            if (componentBody.Entity.ValuesDictionary.DatabaseObject.Name == "spaceship1")
                            {
                                componentDamage = componentBody.Entity.FindComponent<ComponentDamage>();
                                if (componentDamage != null)
                                {
                                    flatBatch3D.QueueLine(p1, p2, borderColor);
                                    flatBatch3D.QueueLine(p2, p3, borderColor);
                                    flatBatch3D.QueueLine(p3, p4, borderColor);
                                    flatBatch3D.QueueLine(p4, p1, borderColor);
                                    //画血条
                                    Vector3 positionText1 = hpos + Vector3.UnitY * 0.3f;
                                    string txt3 = string.Format("耐久:{0}/{1}", (int)(componentDamage.Hitpoints), (int)componentDamage.AttackResilience);//获取最大生命值和当前生命值
                                    fontBatch3D.QueueText(componentCreature.DisplayName, positionText1, right, down, borderColor, TextAnchor.HorizontalCenter | TextAnchor.Top);
                                    fontBatch3D.QueueText(txt3, positionText1, right, down, Color.LightRed, TextAnchor.HorizontalCenter | TextAnchor.Bottom);
                                    flatBatch3D.QueueQuad(p5, p6, p7, p8, fontColor);
                                }

                            }
                            else
                            {
                                flatBatch3D.QueueLine(p1, p2, borderColor);
                                flatBatch3D.QueueLine(p2, p3, borderColor);
                                flatBatch3D.QueueLine(p3, p4, borderColor);
                                flatBatch3D.QueueLine(p4, p1, borderColor);
                                //画血条
                                Vector3 positionText = hpos + Vector3.UnitY * 0.3f;
                                string txt2 = string.Format("生命:{0}/{1}", (int)(componentHealth.Health * componentHealth.AttackResilience), (int)componentHealth.AttackResilience);//获取最大生命值和当前生命值
                                fontBatch3D.QueueText(componentCreature.DisplayName, positionText, right, down, borderColor, TextAnchor.HorizontalCenter | TextAnchor.Top);
                                fontBatch3D.QueueText(txt2, positionText, right, down, Color.LightRed, TextAnchor.HorizontalCenter | TextAnchor.Bottom);
                                flatBatch3D.QueueQuad(p5, p6, p7, p8, fontColor);
                            }

                        }
                    }
                }
            }
            
            
        }

        public override void OnCreatureInjure(ComponentHealth componentHealth, float amount, ComponentCreature attacker, bool ignoreInvulnerability, string cause, out bool Skip)
        {
            Skip = false;
            /*if (cause == LanguageControl.Get(componentHealth.GetType().Name, 3))
			{
			       Skip = true;

            }*/
            Entity entity = componentHealth.Entity;
            if (entity != null)
            {
                string name = entity.ValuesDictionary.DatabaseObject.Name;
                if (name == "BSLord")
                {
                    Skip = true;
                }
            }


        }
        public override void OnTerrainContentsGenerated(TerrainChunk chunk)
        {
            WorldType worldType = m_subsystemworldDemo.worldType;

            if (worldType == WorldType.Default)
            {
                生成西瓜(chunk);//生成西瓜测试
                            // 生成向日葵(chunk);
                //GenerateFCPockets(chunk);//空腔地形生成，岩浆空腔
                FCGenerateSurface(chunk);
                //generatedbuildfirst(chunk);
                foreach (BuildingInfo buildinfo in m_subsystemNaturallyBuildings.BuildingInfos)
                {
                    if (buildinfo != null)
                    {
                        if (buildinfo.CalculatPlainRange(chunk.Coords) == true)
                        {
                            generatedbuild(chunk);
                        }
                        else
                        {
                            //FCGenerateVillage(chunk);
                        }
                    }

                }
                
                
                

            }

            FCGenerateGrassAndPlants(chunk);//蘑菇植物生成
            FCGenerateCaves(chunk);
            //GenerateFCTrees(chunk);
            //GenerateFCCaves(chunk);//洞穴

            //FCGenerateVillage(chunk);
            generateMineralLikeCoal(chunk, 934, 3, 0, 200);


        }

        public void generatedbuildfirst(TerrainChunk chunk)
        {

            SubsystemNaturallyBuildings.GenerateBuildingPrepare(chunk, m_subsystemNaturallyBuildings);
        }
        public void generatedbuild(TerrainChunk chunk)
        {
            SubsystemNaturallyBuildings.GenerateBuilding(chunk, m_subsystemNaturallyBuildings);
        }




        #region 区块强制加载
        public List<Point2> m_terrainChunks007 = new List<Point2>();
        public override void ToFreeChunks(TerrainUpdater terrainUpdater, TerrainChunk chunk, out bool KeepWorking)
        {
            KeepWorking = (m_terrainChunks007.Contains(chunk.Coords));
        }

       

        public override bool ToAllocateChunks(TerrainUpdater terrainUpdater, TerrainUpdater.UpdateLocation[] locations)
        {
            bool result = false;
            foreach (Point2 coord in m_terrainChunks007)
            {
                TerrainChunk chunkAtCoords = terrainUpdater.m_terrain.GetChunkAtCoords(coord.X, coord.Y);
                if (chunkAtCoords == null)
                {
                    result = true;
                    chunkAtCoords = terrainUpdater.m_terrain.AllocateChunk(coord.X, coord.Y);
                    do
                    {
                        terrainUpdater.UpdateChunkSingleStep(chunkAtCoords, terrainUpdater.m_subsystemSky.SkyLightValue);
                    }
                    while (chunkAtCoords.ThreadState < TerrainChunkState.InvalidVertices1);
                }
            }
            return result;
        }

        public bool AddChunks007(int x, int y)
        {
            Point2 item = new Point2(x, y);
            bool flag = !this.m_terrainChunks007.Contains(item);
            bool result;
            if (flag)
            {
                this.m_terrainChunks007.Add(item);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool RemoveChunks007(int x, int y)
        {
            Point2 item = new Point2(x, y);
            bool flag = this.m_terrainChunks007.Contains(item);
            bool result;
            if (flag)
            {
                this.m_terrainChunks007.Remove(item);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
        #region 生物生成区域
        public override void InitializeCreatureTypes(SubsystemCreatureSpawn spawn, List<SubsystemCreatureSpawn.CreatureType> creatureTypes)
        {
            
               
                
                
            creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("Cave Spider", SpawnLocationType.Cave, true, true)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                        
                    WorldType worldType = m_subsystemworldDemo.worldType;//获取当前所在世界
                    if (worldType == WorldType.Default)//如果是主世界（地球）
                    {
                        int temperature = m_subsystemTerrain.Terrain.GetTemperature(point.X, point.Z);
                        int humidity = m_subsystemTerrain.Terrain.GetHumidity(point.X, point.Z);
                        int num = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValueFast(point.X, point.Y - 1, point.Z));
                        if ((num != 3 && num != 67 && num != 4 && num != 66 && num != 2 && num != 7) || temperature <= 2 || humidity < 2)
                        {
                            return 0f;
                        }
                        return 0.85f;
                    }
                    return 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => spawn.SpawnCreatures(creatureType, "Cave_Spider", point, 1).Count)//3是生成数量
            });
            creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("Zombie", SpawnLocationType.Surface, true, true)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    int temperature = m_subsystemTerrain.Terrain.GetTemperature(point.X, point.Z);
                    int humidity = m_subsystemTerrain.Terrain.GetHumidity(point.X, point.Z);
                    int num = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValueFast(point.X, point.Y - 1, point.Z));

                    /*if (m_componentTest1.Areaname == "血泪之池")
                    {

                        return 1f;

                    }*/
                    WorldType worldType = m_subsystemworldDemo.worldType;//获取当前所在世界
                    if (worldType == WorldType.Default)//如果是主世界（地球）
                    {
                        if (point.X > 1018 && point.X < 1590 && point.Z > 1018 && point.Z < 1590)
                        {
                            return 100f;
                        }
                    }
                          
                    return 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => spawn.SpawnCreatures(creatureType, "StrongZombie1_Strength", point, 5).Count)//3是生成数量
            });

                
            
           
            
            
        }
        #endregion

        #region 西瓜生成
        public void 生成西瓜(TerrainChunk chunk)
        {

            /// 生成钻石块测试
            int x = chunk.Coords.X;
            int y = chunk.Coords.Y;
            Random random = new Random(m_seed + x + 1495 * y);
            if (random.Bool(0.2f))//80%生成
            {
                return;
            }
            int num = random.Int(0, MathUtils.Max(1, 1));
            for (int i = 0; i < num; i++)
            {
                int num2 = random.Int(1, 14);
                int num3 = random.Int(1, 14);
                int humidityFast = chunk.GetHumidityFast(num2, num3);
                int temperatureFast = chunk.GetTemperatureFast(num2, num3);
                if (humidityFast < 10 || temperatureFast <= 6)
                {
                    continue;
                }
                for (int j = 0; j < 5; j++)
                {
                    int x2 = num2 + random.Int(-1, 1);
                    int z = num3 + random.Int(-1, 1);
                    for (int num4 = 254; num4 >= 0; num4--)
                    {
                        switch (Terrain.ExtractContents(chunk.GetCellValueFast(x2, num4, z)))
                        {
                            case 8:
                                chunk.SetCellValueFast(x2, num4 + 1, z, random.Bool(0.25f) ? Terrain.MakeBlockValue(936) : Terrain.MakeBlockValue(938));
                                break;
                            case 0:
                                continue;
                        }
                        break;
                    }
                }
            }
        }
        #endregion
        #region
        public void FCGenerateGrassAndPlants(TerrainChunk chunk)
        {
            if (!this.TGExtras)
            {
                return;
            }
            Random random = new Random(this.m_seed + chunk.Coords.X + 3900 * chunk.Coords.Y);
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int k = 254;
                    while (k >= 0)
                    {
                        int cellValueFast = chunk.GetCellValueFast(i, k, j);
                        int num = Terrain.ExtractContents(cellValueFast);
                        if (num != 0)
                        {
                            if (BlocksManager.Blocks[num] is FluidBlock)
                            {
                                break;
                            }
                            int temperatureFast = chunk.GetTemperatureFast(i, j);
                            int humidityFast = chunk.GetHumidityFast(i, j);
                            int num2 = FCGenerateRandomPlantValue(random, cellValueFast, temperatureFast, humidityFast, k + 1);
                            if (num2 != 0)
                            {
                                chunk.SetCellValueFast(i, k + 1, j, num2);
                            }

                            break;
                        }
                        else
                        {
                            k--;
                        }
                    }
                }
            }
        }
        public int FCGenerateRandomPlantValue(Random random, int groundValue, int temperature, int humidity, int y)
        {
            WorldType worldType = m_subsystemworldDemo.worldType;//获取当前所在世界
            int num = Terrain.ExtractContents(groundValue);
            if (worldType == WorldType.Default)//如果是主世界（地球）
            {
               
                if (num != 2)//2泥土
                {
                    if (num != 3 )//如果不是花岗岩，玄武岩或者鹅卵石、冰块
                    {
                        if(num!=67)
                        {
                            if (num != 5)
                            {
                                if (num != 66)
                                {
                                    if (num != 62)
                                    {
                                        if (num != 7)//7沙子
                                        {
                                            if (num != 8) //8草地
                                            {
                                                return 0;
                                            }
                                        }
                                        else//如果是沙子
                                        {
                                            if (humidity >= 8 || random.Float(0f, 1f) >= 0.01f)
                                            {
                                                return 0;
                                            }
                                            if (random.Float(0f, 1f) < 0.05f)
                                            {
                                                return Terrain.MakeBlockValue(99, 0, 0);//大干灌木
                                            }
                                            return Terrain.MakeBlockValue(28, 0, 0); //干枯木
                                        }
                                    }
                                }
                            }
                        }
                        
                    }

                }
                /*if (temperature <= 2)//如果是极寒区域
                {
                    int result = Terrain.MakeBlockValue(0, 0, TallGrassBlock.SetIsSmall(0, false));//19高草


                    float num2 = random.Float(0f, 1f);


                    if (num2 < 0.04f)
                    {
                        int[] indexnum = { 0 };
                        int mushroomnum = random.Int(0, 3);
                        result = Terrain.MakeBlockValue(986, 0, indexnum[0]); //极寒蘑菇
                    }


                    return result;
                }*/
                if (humidity >= 6)
                {
                    if (!SubsystemWeather.IsPlaceFrozen(temperature, y)&&temperature>6)//如果不是极寒区域
                    {
                        if (random.Float(0f, 1f) < (float)humidity / 60f) //随着湿度变大，植物生成概率最高到25%
                        {
                            //当湿度大于等于6时，有湿度值除以60的概率生成高草（ID 19），设置成非小型（大型）。
                            int result = Terrain.MakeBlockValue(19, 0, TallGrassBlock.SetIsSmall(0, false));//19高草
                            if (num == 2 || num == 8)
                            {
                                float num2 = random.Float(0f, 1f);


                                if (num2 < 0.04f)
                                {
                                    result = Terrain.MakeBlockValue(937); //向日葵
                                }
                                else if (num2 < 0.075f)
                                {
                                   // int mushroomnum = random.Int(36, 78);
                                    //result = Terrain.MakeBlockValue(986, 0, mushroomnum); //随机植物
                                }

                            }
                            return result;
                        }
                        else if (random.Float(0f, 1f) < (float)humidity / 60f)
                        {
                            if (humidity >= 10)
                            {
                                int result = Terrain.MakeBlockValue(0, 0, 0);//19高草
                                float num2 = random.Float(0f, 1f);
                                if (num2 < 0.05f)
                                {
                                    int[] indexnum = {5,9,10,11,13,14,15,16,27,31,32,33 };
                                    int mushroomnum = random.Int(0, 11);
                                    result = Terrain.MakeBlockValue(986, 0, indexnum[mushroomnum]); //随机蘑菇
                                }

                                return result;

                            }

                        }
                    }
                    else if(temperature<=2)//如果极寒
                    {
                        if (random.Float(0f, 1f) < (float)humidity / 16f) //随着湿度变大，植物生成概率最高到25%
                        {
                            //当湿度大于等于6时，有湿度值除以60的概率生成高草（ID 19），设置成非小型（大型）。
                            int result = Terrain.MakeBlockValue(0, 0, TallGrassBlock.SetIsSmall(0, false));//19高草
                           
                            
                            float num2 = random.Float(0f, 1f);


                            if (num2 < 0.1f)
                            {
                                int[] indexnum = {0,3,20,21};
                                int mushroomnum = random.Int(0, 3);
                                result = Terrain.MakeBlockValue(986, 0, indexnum[mushroomnum]); //随机蘑菇
                            }
                                
                            
                            return result;
                        }
                    }

                }
                
                
                   
                


            }
            return 0;
        }
        #endregion
        #region 向日葵生成
        public void 生成向日葵(TerrainChunk chunk)
        {
            if (!this.TGExtras)
            {
                return;
            }
            Random random = new Random(this.m_seed + chunk.Coords.X + 3943 * chunk.Coords.Y);
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int k = 254;
                    while (k >= 0)
                    {
                        int cellValueFast = chunk.GetCellValueFast(i, k, j);
                        int num = Terrain.ExtractContents(cellValueFast);
                        if (num != 0)
                        {
                            if (BlocksManager.Blocks[num] is FluidBlock)
                            {
                                break;
                            }
                            int temperatureFast = chunk.GetTemperatureFast(i, j);
                            int humidityFast = chunk.GetHumidityFast(i, j);
                            int num2 = FCPlantManager.GenerateRandomPlantValue(random, cellValueFast, temperatureFast, humidityFast, k + 1);
                            if (num2 != 0)
                            {
                                chunk.SetCellValueFast(i, k + 1, j, num2);
                            }
                            if (num == 2)
                            {
                                chunk.SetCellValueFast(i, k, j, Terrain.MakeBlockValue(8, 0, 0));
                                break;
                            }
                            break;
                        }
                        else
                        {
                            k--;
                        }
                    }
                }
            }

        }
        #endregion

        #region 萤石生成
        public void generateMineralLikeCoal(TerrainChunk chunk, int value, int replacevalue, int minHeight, int maxHeight)
        { //生成矿物算法-类似煤的生成概率                                             
            int cx = chunk.Coords.X;
            int cy = chunk.Coords.Y;
            List<TerrainBrush> terrainBrushes = new List<TerrainBrush>();

            Random random = new Random(17);
            for (int i = 0; i < 16; i++)
            {
                TerrainBrush terrainBrush = new TerrainBrush();
                int num = random.Int(3, 15);//矿物数量的生成概率
                for (int j = 0; j < num; j++)
                {
                    Vector3 vector = 0.5f * Vector3.Normalize(new Vector3(random.Float(-1f, 1f), random.Float(-1f, 1f), random.Float(-1f, 1f)));//
                    int num2 = random.Int(3, 10);//矿物数量的生成概率2
                    Vector3 zero = Vector3.Zero;
                    for (int k = 0; k < num2; k++)
                    {
                        terrainBrush.AddBox((int)MathUtils.Floor(zero.X), (int)MathUtils.Floor(zero.Y), (int)MathUtils.Floor(zero.Z), 1, 1, 1, value);
                        zero += vector;
                    }
                }
                if (i == 0)
                    terrainBrush.AddCell(0, 0, 0, 934);//方块id 这里的000是方块位置，以该方块为中心生成矿物簇
                terrainBrush.Compile();
                terrainBrushes.Add(terrainBrush);
            }
            for (int i = cx - 1; i <= cx + 1; i++)
            {
                for (int j = cy - 1; j <= cy + 1; j++)
                {
                    float num2 = CalculateMountainRangeFactor(i * 16, j * 16);
                    int num3 = (int)(5f + 2f * num2 * SimplexNoise.OctavedNoise(i, j, 0.33f, 1, 1f, 1f));
                    for (int l = 0; l < num3; l++)
                    {
                        int x2 = i * 16 + random.Int(0, 15);
                        int y2 = random.Int(minHeight, maxHeight);
                        int cz = j * 16 + random.Int(0, 15);
                        terrainBrushes[random.Int(0, terrainBrushes.Count - 1)].PaintFastSelective(chunk, x2, y2, cz, replacevalue);
                    }
                }
            }
        }

        public float CalculateMountainRangeFactor(float x, float z)
        {
            return 1f - MathUtils.Abs(2f * SimplexNoise.OctavedNoise(x, z, 0.0014f, 3, 1.91f, 0.75f) - 1f);
        }
        #endregion
        #region 树木生成
        public void GenerateFCTrees(TerrainChunk chunk)
        {

            Terrain terrain = m_subsystemTerrain.Terrain;
            Point2 origin = chunk.Origin;
            Point2 origin2 = chunk.Origin;
            int x = chunk.Coords.X;
            int y = chunk.Coords.Y;
            for (int i = x; i <= x; i++)
            {
                for (int j = y; j <= y; j++)
                {
                    Random random = new Random(m_seed + i + 3943 * j);
                    int humidity = CalculateHumidity(i * 16, j * 16); //计算湿度
                    int num = CalculateTemperature(i * 16, j * 16);  //计算温度
                    float num2 = MathUtils.Saturate((SimplexNoise.OctavedNoise(i, j, 0.1f, 2, 2f, 0.5f) - 0.25f) / 0.2f + (random.Bool(0.25f) ? 0.5f : 0f));
                    int num3 = (int)(5f * num2);
                    int num4 = 0;
                    for (int k = 0; k < 32; k++) //生成k 32棵树
                    {
                        if (num4 >= num3) //如果已生成的树木大于num3，则停止
                        {
                            break;
                        }//噪声算法

                        int num5 = i * 16 + random.Int(2, 13); //num5 num6用来计算树生成的位置
                        int num6 = j * 16 + random.Int(2, 13);
                        int num7 = terrain.CalculateTopmostCellHeight(num5, num6);//获取预生成位置的最大高度块
                        if (num7 < 66)//高度大于66，不然跳过，继续循环
                        {
                            continue;
                        }

                        int cellContentsFast = terrain.GetCellContentsFast(num5, num7, num6); //获取这个预生成方块的值

                        if (cellContentsFast != 2 && cellContentsFast != 8)//如果不是泥土或者草地
                        {
                            continue;
                        }//选择方块

                        num7++; //高度加一，继续生成
                                //这个if是检查树周围的方块是否可碰撞。
                        if (!BlocksManager.Blocks[terrain.GetCellContentsFast(num5 + 1, num7, num6)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(num5 - 1, num7, num6)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(num5, num7, num6 + 1)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(num5, num7, num6 - 1)].IsCollidable)
                        {
                            FCTreeType? treeType = FCPlantManager.GenerateRandomTreeType(random, num + SubsystemWeather.GetTemperatureAdjustmentAtHeight(num7), humidity, num7);
                            if (treeType.HasValue)
                            {
                                if (treeType == FCTreeType.Yinghua)
                                {

                                    // 获取樱花树的地形刷子
                                    ReadOnlyList<TerrainBrush> cherryBlossomBrushes = FCPlantManager.GetTreeBrushes(treeType.Value);
                                    TerrainBrush terrainBrush = cherryBlossomBrushes[random.Int(cherryBlossomBrushes.Count)];

                                    // 应用樱花树的地形刷子到地形上
                                    terrainBrush.PaintFast(chunk, num5, num7, num6);
                                    chunk.AddBrushPaint(num5, num7, num6, terrainBrush);

                                    // 生成落樱 
                                    for (int dx = -3; dx <= 3; dx++)
                                    {
                                        for (int dz = -3; dz <= 3; dz++)
                                        {



                                            int lx = num5 + dx;
                                            int ly = num7; // 落樱的高度为树根加1，即num7
                                            int lz = num6 + dz;
                                            if (Terrain.ExtractContents(terrain.GetCellValueFast(lx, ly, lz)) == 0) // 如果该位置为空，则生成落樱
                                            {
                                                if (dx == 0 && dz == 0)
                                                {
                                                    continue; // 跳过中心点的位置
                                                }
                                                else
                                                {
                                                    chunk.SetCellValueFast(lx, ly, lz, 963);
                                                }

                                            }

                                        }
                                    }


                                }
                                else
                                {
                                    ReadOnlyList<TerrainBrush> treeBrushes = FCPlantManager.GetTreeBrushes(treeType.Value);
                                    TerrainBrush terrainBrush = treeBrushes[random.Int(treeBrushes.Count)];
                                    terrainBrush.PaintFast(chunk, num5, num7, num6);
                                    chunk.AddBrushPaint(num5, num7, num6, terrainBrush);
                                }

                            }
                            num4++; //成功生成一个树木，num4加1
                        }
                    }
                }
            }
        }


        public int CalculateTemperature(float x, float z)
        {
            return MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + this.m_temperatureOffset.X, z + this.m_temperatureOffset.Y, 0.0015f / this.TGBiomeScaling, 5, 2f, 0.6f, false) - 1.1f + this.m_worldSettings.TemperatureOffset / 16f) * 16f), 0, 15);
        }

        public int CalculateHumidity(float x, float z)
        {
            return MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + this.m_humidityOffset.X, z + this.m_humidityOffset.Y, 0.0012f / this.TGBiomeScaling, 5, 2f, 0.6f, false) - 0.9f + this.m_worldSettings.HumidityOffset / 16f) * 16f), 0, 15);
        }
        #endregion

        #region 洞穴

        static NewModLoaderShengcheng()
        {
            NewModLoaderShengcheng.FCCreateBrushes();
        }

        public class CavePoint
        {
            public Vector3 Position;

            public Vector3 Direction;

            public int BrushType;

            public int Length;

            public int StepsTaken;
        }
        public void FCGenerateCaves(TerrainChunk chunk)
        {
            if (!this.TGCavesAndPockets)
            {
                return;
            }
            List<NewModLoaderShengcheng.CavePoint> list = new List<NewModLoaderShengcheng.CavePoint>();
            //创建一个名为list的空列表，用于存储洞穴点的信息。
            int x = chunk.Coords.X;
            int y = chunk.Coords.Y;
            for (int i = x - 3; i <= x + 3; i++)////执行一个循环，循环变量i从x-2开始，一直遍历到x+2。,这是一个双重循环，遍历地形块附近的区域。
            {
                for (int j = y - 3; j <= y + 3; j++)////在外层循环内部，执行一个嵌套循环，循环变量j从y-2开始，一直遍历到y+2。
                {
                    list.Clear();//在每轮循环开始时清空洞穴点列表。
                    Random random = new Random(this.m_seed + i + 90 * j);
                    int num = i * 16 + random.Int(0, 15);//计算区块真实坐标
                    int num2 = j * 16 + random.Int(0, 15);
                    float probability = 1f;
                    if (random.Bool(probability))//按照概率随机决定是否要创建洞穴
                    {
                        //下面的代码块主要是计算洞穴生成的位置、方向等参数，然后创建一个CavePoint对象，并添加到列表中。

                        int num3 = (int)this.CalculateHeight((float)num, (float)num2);
                        //这一行调用了 CalculateHeight 方法，计算了二维坐标 (num, num2) 处的地形高度，并将其转换为整数 num3。这个高度很可能是从地形生成算法中得出的表面高度。

                        int num4 = (int)this.CalculateHeight((float)(num + 3), (float)num2);
                        //再次调用 CalculateHeight 方法，计算偏移3个单位的X坐标 (num + 3, num2) 处的地形高度，并将其转换为整数 num4。这样做可能是为了获取高度的梯度或变化情况。
                        
                        
                        int num5 = (int)this.CalculateHeight((float)num, (float)(num2 + 3));
                        //同样的，计算偏移3个单位的Z坐标 (num, num2 + 3) 处的地形高度，并将其转换为整数 num5。这也是为了获得高度在不同方向上的变化。

                        Vector3 position = new Vector3((float)num, (float)(num3 - 1), (float)num2);//num是x，num2是z，num3代表这个位置的高度的方块-1
                        //创建一个三维向量 position，它的X和Z坐标对应于前面计算的 num 和 num2，Y坐标为 num3 - 1，表示洞穴的起始点位于地面之下1个单位的位置。

                        Vector3 v = new Vector3(3f, (float)(num4 - num3), 0f);
                        //创建一个三维向量 v，其X分量为3，Y分量为 num4 - num3（即X方向上的高度差），Z分量为0。这个向量表示X方向的坡度。

                        Vector3 v2 = new Vector3(0f, (float)(num5 - num3), 3f);
                        //创建一个三维向量 v2，其X分量为0，Y分量为 num5 - num3（即Z方向上的高度差），Z分量为3。这个向量表示Z方向的坡度。
                       
                        Vector3 vector = Vector3.Normalize(Vector3.Cross(v, v2));
                        //计算 v 和 v2 的叉积（这代表了两个向量所定义平面的法线向量），然后对该叉积向量进行归一化，得到单位向量 vector。这个单位向量表示洞穴在三维空间中的入口方向。

                        if (vector.Y > -0.6f)//这行代码检查 vector 的Y分量是否大于-0.6，这是一个用来判断地面的倾斜程度是否适合生成洞穴的条件。如果Y分量太小（即向量太过水平或向下），可能不适合生成洞穴。
                        {
                            list.Add(new NewModLoaderShengcheng.CavePoint
                            {
                                Position = position,
                                Direction = vector,
                                BrushType = 0,
                                Length = random.Int(80, 240)
                            });
                        }
                        else if(random.Bool(0.04f))
                        {
                            Vector3 position1 = new Vector3((float)num, random.Float(30,54), (float)num2);//num是x，num2是z，num3代表这个位置的高度的方块-1;
                            list.Add(new NewModLoaderShengcheng.CavePoint
                            {
                                Position = position1,
                                Direction = vector,
                                BrushType = 0,
                                Length = random.Int(80, 240)
                            });
                        }
                        else
                        {
                            if (random.Bool(0.02f))
                            {
                                list.Add(new NewModLoaderShengcheng.CavePoint
                                {
                                    Position = position,
                                    Direction = vector,
                                    BrushType = 0,
                                    Length = random.Int(80, 240)
                                });
                            }
                        }
                        
                        int num6 = i * 16 + 8;
                        int num7 = j * 16 + 8;
                        int k = 0;
                        while (k < list.Count)//这是一个while循环，它会遍历list中索引为k到list元素数量之间的CavePoint。
                        {
                            NewModLoaderShengcheng.CavePoint cavePoint = list[k];
                            //从list中获取索引为k的CavePoint，并命名为cavePoint。

                            List<TerrainBrush> list2 = NewModLoaderShengcheng.m_fccaveBrushesByType[cavePoint.BrushType];
                            //获取CavePoint的BrushType属性，并从m_caveBrushesByType中获取对应的TerrainBrush列表，命名为list2。

                            
                            list2[random.Int(0, list2.Count - 1)].PaintFastAvoidWater(chunk, Terrain.ToCell(cavePoint.Position.X), Terrain.ToCell(cavePoint.Position.Y), Terrain.ToCell(cavePoint.Position.Z));
                            //从list2中随机获取一个TerrainBrush，并对它执行PaintFastAvoidWater方法，传入地形块（chunk）和CavePoint在X、Y和Z方向上的位置作为参数，这个方法用于在指定位置生成洞穴。

                            cavePoint.Position += 2f * cavePoint.Direction;
                            //将CavePoint的Position属性加上其Direction属性的两倍，表示洞穴在其方向上前进两个单位。

                            cavePoint.StepsTaken += 2;
                            //将CavePoint的StepsTaken属性加2，表示洞穴已经前进了两步。

                            float num8 = cavePoint.Position.X - (float)num6;
                            float num9 = cavePoint.Position.Z - (float)num7;
                            //计算cavePoint的Position属性的X坐标与num6之间的差值，赋值给num8；计算cavePoint的Position属性的Z坐标与num7之间的差值，赋值给num9。这两个差值可能用来判断洞穴是否已经超出了其应该存在的范围。

                            //以下代码根据一系列的判断条件来调整CavePoint的方向、长度和笔刷类型，例如：
                            if (random.Bool(0.5f))
                            {
                                Vector3 vector2 = Vector3.Normalize(random.Vector3(1f));
                                //生成一个单位长度（长度为1）的随机三维向量 vector2，并对其进行归一化，使其长度为1。

                                if ((num8 < -25.5f && vector2.X < 0f) || (num8 > 25.5f && vector2.X > 0f))
                                {
                                    //如果 vector2 的X分量将导致洞穴点的X坐标超出预设的边界（-25.5至25.5之外），则反转 vector2 的X分量，使洞穴点不会超出这个范围。
                                    vector2.X = 0f - vector2.X;
                                }
                                if ((num9 < -25.5f && vector2.Z < 0f) || (num9 > 25.5f && vector2.Z > 0f))
                                {
                                    //如果 vector2 的Z分量将导致洞穴点的Z坐标超出预设的边界（同样是-25.5至25.5之外），则反转 vector2 的Z分量。
                                    vector2.Z = 0f - vector2.Z;
                                }
                                if ((cavePoint.Direction.Y < -0.5f && vector2.Y < -10f) || (cavePoint.Direction.Y > 0.1f && vector2.Y > 0f))
                                {
                                    //如果 vector2 的Y分量与当前洞穴点的方向Y分量相反，并且超出一定的界限（Y方向上太陡），则反转 vector2 的Y分量。
                                    vector2.Y = 0f - vector2.Y;
                                }
                                //更新cavePoint的Direction属性为当前方向加上 vector2 的半分（即方向调整了一点，但没完全按照 vector2 调整）。然后再次归一化方向使其成为单位向量。
                                cavePoint.Direction = Vector3.Normalize(cavePoint.Direction + 0.5f * vector2);
                            }
                            if (cavePoint.StepsTaken > 20 && random.Bool(0.06f))
                            {
                                //如果cavePoint已经前进了超过20步，并且随机数生成器返回的布尔值为真（有6%的概率），那么将cavePoint的Direction属性设置为一个新的随机方向，但Y分量被缩放为原来的0.33倍，这导致洞穴在垂直方向上不会太陡。
                                cavePoint.Direction = Vector3.Normalize(random.Vector3(1f) * new Vector3(1f, 0.33f, 1f));
                            }
                            
                            if (cavePoint.StepsTaken > 20 && random.Bool(0.05f))
                            {
                                //如果cavePoint已经前进了超过20步，并且有5%的概率，洞穴的方向将被更新为水平方向（Y分量设置为0），同时BrushType增加2但不超过m_caveBrushesByType中的最大索引。
                                cavePoint.Direction.Y = 0f;
                                cavePoint.BrushType = MathUtils.Min(cavePoint.BrushType + 2, NewModLoaderShengcheng.m_fccaveBrushesByType.Count - 1);
                            }
                            if (cavePoint.StepsTaken > 30 && random.Bool(0.03f))
                            {
                                //如果cavePoint已经前进了超过30步，并且有3%的概率，将cavePoint的方向设置为垂直向下。
                                cavePoint.Direction.X = 0f;
                                cavePoint.Direction.Y = -1f;
                                cavePoint.Direction.Z = 0f;
                            }
                            if (cavePoint.StepsTaken > 30 && cavePoint.Position.Y < 30f && random.Bool(0.02f))
                            {
                                //如果cavePoint已经前进了超过30步，且Y坐标小于30，并且有2%的概率，将cavePoint的方向设置为垂直向上。
                                cavePoint.Direction.X = 0f;
                                cavePoint.Direction.Y = 1f;
                                cavePoint.Direction.Z = 0f;
                            }
                            if (random.Bool(0.33f))//这个if语句在满足一定的概率（33%）时，会随机选择一个新的笔刷类型。
                            {
                                cavePoint.BrushType = (int)(MathUtils.Pow(random.Float(0f, 0.999f), 7f) * (float)NewModLoaderShengcheng.m_fccaveBrushesByType.Count);
                            }

                            if (random.Bool(0.06f) && list.Count < 12 && cavePoint.StepsTaken > 20 && cavePoint.Position.Y < 58f)
                            {
                                //这个if语句在满足一定的概率（6%）和其他条件时，会在list中添加一个新的CavePoint，这个新的CavePoint与当前的CavePoint在同一位置，但可能有不同的方向和笔刷类型。
                                list.Add(new NewModLoaderShengcheng.CavePoint
                                {
                                    Position = cavePoint.Position,
                                    Direction = Vector3.Normalize(random.Vector3(1f, 1f) * new Vector3(1f, 0.33f, 1f)),
                                    BrushType = (int)(MathUtils.Pow(random.Float(0f, 0.999f), 7f) * (float)NewModLoaderShengcheng.m_fccaveBrushesByType.Count),
                                    Length = random.Int(40, 180)
                                });
                            }
                            if (cavePoint.StepsTaken >= cavePoint.Length || MathUtils.Abs(num8) > 100f || MathUtils.Abs(num9) > 100f || cavePoint.Position.Y < 10f || cavePoint.Position.Y > 246f)
                            {
                                //这个if语句检查是否应该结束当前CavePoint的处理并开始处理下一个。结束的条件包括：洞穴已经前进的步数达到了预设的长度、洞穴已经超出了应该存在的范围、洞穴的深度过浅或过深。
                                k++;
                            }
                            else if (cavePoint.StepsTaken % 20 == 0)
                            {
                                //这个else if语句在每前进20步时，会检查洞穴是否已经接近地面。如果是，则结束当前CavePoint的处理并开始处理下一个。
                                float num10 = this.CalculateHeight(cavePoint.Position.X, cavePoint.Position.Z);
                                if (cavePoint.Position.Y > num10 + 1f)
                                {
                                    k++;
                                }
                            }
                        }
                    }
                }
            }
        }
       

        public float CalculateHeight(float x, float z)
        {
            float num = this.TGOceanSlope + this.TGOceanSlopeVariation * MathUtils.PowSign(2f * SimplexNoise.OctavedNoise(x + this.m_mountainsOffset.X, z + this.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
            float num2 = this.CalculateOceanShoreDistance(x, z);
            float num3 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(num2));
            float num4 = MathUtils.Saturate(MathUtils.Sin(this.TGIslandsFrequency * num2));
            float num5 = MathUtils.Saturate(MathUtils.Saturate((0f - num) * num2) - 0.85f * num4);
            float num6 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - num2 - 10f)) - num4);
            float v = this.CalculateMountainRangeFactor(x, z);
            float f = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.001f / this.TGBiomeScaling, 2, 2f, 0.5f, false);
            float f2 = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.0017f / this.TGBiomeScaling, 2, 4f, 0.7f, false);
            float num7 = (1f - num6) * (1f - num3) * TerrainContentsGenerator23.Squish(v, 1f - this.TGHillsPercentage, 1f - this.TGMountainsPercentage);
            float num8 = (1f - num6) * TerrainContentsGenerator23.Squish(v, 1f - this.TGMountainsPercentage, 1f);
            float num9 = 1f * SimplexNoise.OctavedNoise(x, z, this.TGHillsFrequency, this.TGHillsOctaves, 1.93f, this.TGHillsPersistence, false);
            float amplitudeStep = MathUtils.Lerp(0.75f * TerrainContentsGenerator23.TGMountainsDetailPersistence, 1.33f * TerrainContentsGenerator23.TGMountainsDetailPersistence, f);
            float num10 = 1.5f * SimplexNoise.OctavedNoise(x, z, TerrainContentsGenerator23.TGMountainsDetailFreq, TerrainContentsGenerator23.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
            float num11 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * num8 + 0.5f * num7 + MathUtils.Saturate(1f - num2 / 30f)));
            float x2 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(num8 + 0.5f * num7));
            float num12 = MathUtils.Saturate(1.5f - num11 * MathUtils.Abs(2f * SimplexNoise.OctavedNoise(x + this.m_riversOffset.X, z + this.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
            float num13 = -50f * num5 + this.TGHeightBias;
            float num14 = MathUtils.Lerp(0f, 8f, f);
            float num15 = MathUtils.Lerp(0f, -6f, f2);
            float num16 = this.TGHillsStrength * num7 * num9;
            float num17 = this.TGMountainsStrength * num8 * num10;
            float f3 = this.TGRiversStrength * num12;
            float num18 = num13 + num14 + num15 + num17 + num16;
            float num19 = MathUtils.Min(MathUtils.Lerp(num18, x2, f3), num18);
            return MathUtils.Clamp(64f + num19, 10f, 251f);
        }
        public float CalculateOceanShoreDistance(float x, float z)
        {
            if (this.m_islandSize != null)
            {
                float num = this.CalculateOceanShoreX(z);
                float num2 = this.CalculateOceanShoreZ(x);
                float num3 = this.CalculateOceanShoreX(z + 1000f) + this.m_islandSize.Value.X;
                float num4 = this.CalculateOceanShoreZ(x + 1000f) + this.m_islandSize.Value.Y;
                return MathUtils.Min(x - num, z - num2, num3 - x, num4 - z);
            }
            float num5 = this.CalculateOceanShoreX(z);
            float num6 = this.CalculateOceanShoreZ(x);
            return MathUtils.Min(x - num5, z - num6);
        }

        public float CalculateOceanShoreX(float z)
        {
            return this.m_oceanCorner.X + this.TGShoreFluctuations * SimplexNoise.OctavedNoise(z, 0f, 0.005f / this.TGShoreFluctuationsScaling, 4, 1.95f, 1f, false);
        }

        public float CalculateOceanShoreZ(float x)
        {
            return this.m_oceanCorner.Y + this.TGShoreFluctuations * SimplexNoise.OctavedNoise(0f, x, 0.005f / this.TGShoreFluctuationsScaling, 4, 1.95f, 1f, false);
        }
        public float TGOceanSlopeVariation;
        public float TGOceanSlope;
        public Vector2 m_mountainsOffset;
        public Vector2 m_riversOffset;
        public float TGIslandsFrequency;
        public float TGHillsPercentage;
        public float TGHillsStrength;
        public float TGMountainsStrength;
        public float TGMountainRangeFreq;
        public float TGMountainsPercentage;
        public int TGHillsOctaves;
        public float TGHillsFrequency;
        public float TGHillsPersistence;
        public float TGHeightBias;
        public float TGRiversStrength;
        public Vector2? m_islandSize;
        public Vector2 m_oceanCorner;
        public float TGShoreFluctuations;
        public float TGShoreFluctuationsScaling;
        public float TGTurbulenceStrength;
        public float TGTurbulenceFreq;
        public int TGTurbulenceOctaves;
        public float TGTurbulencePersistence;
        public float TGMinTurbulence;
        public float TGTurbulenceZero;
        public float TGDensityBias;
        #endregion

        #region 空腔替换
        public void GenerateFCPockets(TerrainChunk chunk)
        {
            if (!this.TGCavesAndPockets)//检查是否启用了岩浆口袋的生成。如果未启用，则直接返回，不进行生成岩浆口袋的操作。
            {
                return;
            }
            for (int i = -1; i <= 1; i++) //双重循环，遍历以当前区块为中心的相邻区块。i和j的值为-1、0和1，用于遍历所有相邻的区块。
            {
                for (int j = -1; j <= 1; j++)
                {
                    int num = i + chunk.Coords.X; //计算相邻区块的坐标，以当前区块的坐标为基础。
                    int num2 = j + chunk.Coords.Y;
                    Random random = new Random(this.m_seed + num + 71 * num2); //创建一个随机数生成器，种子使用了当前区块的坐标和一个固定的偏移量。
                    int num3 = random.Int(0, 10);//生成一个介于0和10之间的随机数，用于决定生成岩浆口袋的数量。
                    for (int k = 0; k < num3; k++) //根据上一步生成的随机数，进行循环，决定生成岩浆口袋的具体位置。
                    {
                        random.Int(0, 1);
                    }
                    float num4 = this.CalculateMountainRangeFactor((float)(num * 16), (float)(num2 * 16));//根据相邻区块的坐标计算一个山脉范围因子。



                    if (random.Bool(0.06f + 0.05f * num4)) //根据之前计算的山脉范围因子和随机数生成器，判断是否生成岩浆口袋。判断的条件为一个布尔值，其概率为0.06加上0.05乘以山脉范围因子。
                    {
                        int num18 = num * 16;
                        int num19 = random.Int(35, 42); //生成一个介于15和42之间的随机数，用于确定岩浆口袋的高度位置。
                        int num20 = num2 * 16;
                        int num21 = random.Int(1, 2); //生成一个介于1和2之间的随机数，用于确定生成岩浆口袋的数量。



                        for (int num22 = 0; num22 < num21; num22++) //据上一步生成的数量，进行循环，确定生成岩浆口袋的具体位置。
                        {
                            Vector2 vector2 = random.Vector2(7f); //使用随机数生成器生成一个二维向量，向量的每个分量介于-7和7之间。
                            int num23 = 8 + (int)MathUtils.Round(vector2.X); //根据生成的二维向量，确定岩浆口袋在区块内的具体位置。
                            int num24 = random.Int(0, 1);
                            int num25 = 8 + (int)MathUtils.Round(vector2.Y); //根据随机数生成器，选择一个岩浆口袋的绘制方法，并使用给定的参数在区块内绘制岩浆口袋。
                            NewModLoaderShengcheng.m_fcmagmaPocketBrushes[random.Int(0, NewModLoaderShengcheng.m_fcmagmaPocketBrushes.Count - 1)].PaintFast(chunk, num18 + num23, num19 + num24, num20 + num25);
                        }
                    }
                }
            }
        }
        #endregion

        #region 静态地形刷子（岩浆湖之类的）
        public static void FCCreateBrushes()
        {
            Random random = new Random(17);





            #region 已合并到模组的刷子

            int[] array3 = new int[]//洞穴生成的空腔
			{
                8,
                12,
                14,
                16
            };
            for (int num73 = 0; num73 < 4 * array3.Length; num73++)
            {
                TerrainBrush terrainBrush16 = new TerrainBrush();
                int num74 = array3[num73 / 4];
                int num75 = num74 + 2;
                float num76 = (num73 % 4 == 2) ? 0.5f : 1f;
                int num77 = (num73 % 4 == 1) ? (num74 * num74) : (2 * num74 * num74);
                for (int num78 = 0; num78 < num77; num78++)
                {
                    Vector2 vector16 = random.Vector2(0f, (float)num74);
                    float num79 = vector16.Length();
                    int num80 = random.Int(3, 4);
                    int sizeY2 = 1 + (int)MathUtils.Lerp(MathUtils.Max((float)(num74 / 3), 2.5f) * num76, 0f, num79 / (float)num74) + random.Int(0, 1);
                    int num81 = 1 + (int)MathUtils.Lerp((float)num75, 0f, num79 / (float)num74) + random.Int(0, 1);
                    terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), 0, (int)MathUtils.Floor(vector16.Y), num80, sizeY2, num80, 0);
                    terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), -num81, (int)MathUtils.Floor(vector16.Y), num80, num81, num80, 0);
                }
                terrainBrush16.Compile();
                NewModLoaderShengcheng.m_fcmagmaPocketBrushes2.Add(terrainBrush16);
            }

            #region 空腔
            NumberWeightPair[] numberWeightPairs = new NumberWeightPair[]
            {
                new NumberWeightPair(16, 8),   // 煤的权重为 （这里的权重判断其实不是很管用，因为会被重复的空气盒子替换，所以建议用下面的bool判断
				new NumberWeightPair(0, 80),   // 空气的权重为
				new NumberWeightPair(41, 3),  //铜
				new NumberWeightPair(39, 3), //铁
				new NumberWeightPair(101, 1),//硫
				new NumberWeightPair(112, 1),//钻石
				new NumberWeightPair(934, 4), // 萤石的权重为
												// 其他数字和权重的对应关系...
			};
            int totalWeight = 0;
            foreach (NumberWeightPair pair in numberWeightPairs)
            {
                totalWeight += pair.weight; //总权重
            }
            int randomWeight = random.Int(0, totalWeight - 1);




            int[] array2 = new int[]//定义了一个整型数组array2，包含了4个不同的整数值，用于确定岩浆池刷子的尺寸。
			{
                16,
                24,
                28,
                30
            };
            for (int num73 = 0; num73 < 4 * array2.Length; num73++)//外层循环，循环次数为4乘以array2数组的长度，用于生成多个岩浆池刷子。num73 < 16
            {
                TerrainBrush terrainBrush16 = new TerrainBrush(); //创建一个新的空岩浆池刷子实例。
                int num74 = array2[num73 / 4]; //根据循环索引来选择对应的岩浆池大小，从array2数组中获取值。8,12,14,16
                int num75 = num74 + 2; //计算岩浆池刷子尺寸的偏移量，为岩浆池大小加上2。
                float num76 = (num73 % 4 == 2) ? 0.5f : 1f;//根据循环索引的模运算结果，确定岩浆池刷子的缩放因子。当索引模4等于2时，缩放因子为0.5，否则为1。


                int num77 = (num73 % 4 == 1) ? (num74 * num74) : (2 * num74 * num74);//根据循环索引的模运算结果，确定岩浆池刷子需要绘制的盒子数量。如果索引模4等于1，盒子数量为岩浆池大小的平方，否则为岩浆池大小的两倍平方。
                for (int num78 = 0; num78 < num77; num78++)//内层循环，循环次数为岩浆池需要绘制的盒子数量。
                {
                    Vector2 vector16 = random.Vector2(0f, (float)num74);//生成一个在二维平面上的随机向量，x和y分量的范围为0到岩浆池大小。
                    float num79 = vector16.Length();//计算随机向量的长度。 向量长度为洞穴大小的值。8-16
                    int num80 = random.Int(3, 4);//随机生成一个高度值，范围为3到4。
                                                 //根据岩浆池大小、缩放因子和随机向量长度，计算盒子的尺寸。使用线性插值函数MathUtils.Lerp来计算盒子的Y轴尺寸，并添加一个随机值。
                    int sizeY2 = 1 + (int)MathUtils.Lerp(MathUtils.Max((float)(num74 / 3), 2.5f) * num76, 0f, num79 / (float)num74) + random.Int(0, 1);
                    //根据岩浆池大小、偏移量和随机向量长度，计算盒子的高度。使用线性插值函数MathUtils.Lerp来计算盒子的Y轴高度，并添加一个随机值。

                    int num81 = 1 + (int)MathUtils.Lerp((float)num75, 0f, num79 / (float)num74) + random.Int(0, 1);
                    //在岩浆池刷子中添加一个盒子形状，位置和尺寸由随机生成的参数确定。


                    int selectedNumber = -1;
                    int selectedNumber2 = 0;
                    int accumulatedWeight = 0;
                    foreach (NumberWeightPair pair in numberWeightPairs)//权重分配所有可能的矿物
                    {
                        accumulatedWeight += pair.weight;
                        if (randomWeight < accumulatedWeight)
                        {
                            selectedNumber = pair.number;

                            break;
                        }
                    }
                    selectedNumber2 = selectedNumber;
                    if (selectedNumber2 == 0)
                    {
                        selectedNumber2 = 980;

                    }
                    if (num81 - 15 > 0)
                    {
                        if (random.Bool(0.0001f) && selectedNumber == 0) //煤
                        {
                            selectedNumber = 16;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0) //铁
                        {
                            selectedNumber = 39;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//钻石
                        {
                            selectedNumber = 112;
                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//铜
                        {
                            selectedNumber = 41;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//硫
                        {
                            selectedNumber = 101;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//锗
                        {
                            selectedNumber = 148;

                        }
                    }


                    terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), 0, (int)MathUtils.Floor(vector16.Y), num80, sizeY2, num80, 0);
                    /*if (random.Bool(0.1f) )//生成钟乳石
					{
						terrainBrush16.AddRay((int)MathUtils.Floor(vector16.X), 5, (int)MathUtils.Floor(vector16.Y), (int)MathUtils.Floor(vector16.X), 0, (int)MathUtils.Floor(vector16.Y), 1, 1, 1, selectedNumber2);
						//生成钟乳石
					}*/
                    if (selectedNumber != 0)//如果不为0
                    {
                        if (random.Bool(0.1f))//进行二次判断 概率为x生成矿物
                        {
                            terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), -num81, (int)MathUtils.Floor(vector16.Y), num80, num81, num80, selectedNumber);//生成矿物分布
                        }

                    }
                    else if (selectedNumber == 0)
                    {
                        terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), -num81, (int)MathUtils.Floor(vector16.Y), num80, num81, num80, selectedNumber);//生成矿物分布

                    }
                }
                terrainBrush16.Compile();
                NewModLoaderShengcheng.m_fcmagmaPocketBrushes.Add(terrainBrush16);
            }
            #endregion
            #region 月球空腔
            NumberWeightPair[] MoonnumberWeightPairs = new NumberWeightPair[]
            {
                new NumberWeightPair(16, 8),   // 煤的权重为 （这里的权重判断其实不是很管用，因为会被重复的空气盒子替换，所以建议用下面的bool判断
				new NumberWeightPair(0, 80),   // 空气的权重为
				new NumberWeightPair(41, 3),  //铜
				new NumberWeightPair(39, 3), //铁
				new NumberWeightPair(101, 1),//硫
				new NumberWeightPair(112, 1),//钻石
				new NumberWeightPair(934, 4), // 萤石的权重为
												// 其他数字和权重的对应关系...
			};
            int MoontotalWeight = 0;
            foreach (NumberWeightPair pair in numberWeightPairs)
            {
                MoontotalWeight += pair.weight; //总权重
            }
            int MoonrandomWeight = random.Int(0, MoontotalWeight - 1);




            int[] Moonarray2 = new int[]//定义了一个整型数组array2，包含了4个不同的整数值，用于确定岩浆池刷子的尺寸。
			{
                8,
                24,
                28,
                30
            };
            for (int num73 = 0; num73 < 4 * Moonarray2.Length; num73++)//外层循环，循环次数为4乘以array2数组的长度，用于生成多个岩浆池刷子。num73 < 16
            {
                TerrainBrush terrainBrush16 = new TerrainBrush(); //创建一个新的空岩浆池刷子实例。
                int num74 = array2[num73 / 4]; //根据循环索引来选择对应的岩浆池大小，从array2数组中获取值。8,12,14,16
                int num75 = num74 + 2; //计算岩浆池刷子尺寸的偏移量，为岩浆池大小加上2。
                float num76 = (num73 % 4 == 2) ? 0.5f : 1f;//根据循环索引的模运算结果，确定岩浆池刷子的缩放因子。当索引模4等于2时，缩放因子为0.5，否则为1。


                int num77 = (num73 % 4 == 1) ? (num74 * num74) : (2 * num74 * num74);//根据循环索引的模运算结果，确定岩浆池刷子需要绘制的盒子数量。如果索引模4等于1，盒子数量为岩浆池大小的平方，否则为岩浆池大小的两倍平方。
                for (int num78 = 0; num78 < num77; num78++)//内层循环，循环次数为岩浆池需要绘制的盒子数量。
                {
                    Vector2 vector16 = random.Vector2(0f, (float)num74);//生成一个在二维平面上的随机向量，x和y分量的范围为0到岩浆池大小。
                    float num79 = vector16.Length();//计算随机向量的长度。 向量长度为洞穴大小的值。8-16
                    int num80 = random.Int(3, 4);//随机生成一个高度值，范围为3到4。
                                                 //根据岩浆池大小、缩放因子和随机向量长度，计算盒子的尺寸。使用线性插值函数MathUtils.Lerp来计算盒子的Y轴尺寸，并添加一个随机值。
                    int sizeY2 = 1 + (int)MathUtils.Lerp(MathUtils.Max((float)(num74 / 3), 2.5f) * num76, 0f, num79 / (float)num74) + random.Int(0, 1);
                    //根据岩浆池大小、偏移量和随机向量长度，计算盒子的高度。使用线性插值函数MathUtils.Lerp来计算盒子的Y轴高度，并添加一个随机值。

                    int num81 = 1 + (int)MathUtils.Lerp((float)num75, 0f, num79 / (float)num74) + random.Int(0, 1);
                    //在岩浆池刷子中添加一个盒子形状，位置和尺寸由随机生成的参数确定。


                    int selectedNumber = -1;
                    int selectedNumber2 = 0;
                    int accumulatedWeight = 0;
                    foreach (NumberWeightPair pair in MoonnumberWeightPairs)//权重分配所有可能的矿物
                    {
                        accumulatedWeight += pair.weight;
                        if (MoonrandomWeight < accumulatedWeight)
                        {
                            selectedNumber = pair.number;

                            break;
                        }
                    }
                    selectedNumber2 = selectedNumber;
                    if (selectedNumber2 == 0)
                    {
                        selectedNumber2 = 980;

                    }
                    if (num81 - 15 > 0)
                    {
                        if (random.Bool(0.0001f) && selectedNumber == 0) //煤
                        {
                            selectedNumber = 16;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0) //铁
                        {
                            selectedNumber = 39;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//钻石
                        {
                            selectedNumber = 112;
                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//铜
                        {
                            selectedNumber = 41;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//硫
                        {
                            selectedNumber = 101;

                        }
                        if (random.Bool(0.0001f) && selectedNumber == 0)//锗
                        {
                            selectedNumber = 148;

                        }
                    }


                    terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), 0, (int)MathUtils.Floor(vector16.Y), num80, sizeY2, num80, 0);
                    /*if (random.Bool(0.1f) )//生成钟乳石
					{
						terrainBrush16.AddRay((int)MathUtils.Floor(vector16.X), 5, (int)MathUtils.Floor(vector16.Y), (int)MathUtils.Floor(vector16.X), 0, (int)MathUtils.Floor(vector16.Y), 1, 1, 1, selectedNumber2);
						//生成钟乳石
					}*/
                    if (selectedNumber != 0)//如果不为0
                    {
                        if (random.Bool(0.1f))//进行二次判断 概率为x生成矿物
                        {
                            terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), -num81, (int)MathUtils.Floor(vector16.Y), num80, num81, num80, selectedNumber);//生成矿物分布
                        }

                    }
                    else if (selectedNumber == 0)
                    {
                        terrainBrush16.AddBox((int)MathUtils.Floor(vector16.X), -num81, (int)MathUtils.Floor(vector16.Y), num80, num81, num80, selectedNumber);//生成矿物分布

                    }
                }
                terrainBrush16.Compile();
                NewModLoaderShengcheng.m_fcMoonPocketBrushes.Add(terrainBrush16);
            }
            #endregion
            #region 洞穴刷子
            for (int num82 = 0; num82 < 7; num82++) //外层循环，生成7种不同类型的洞穴刷子。
            {
                NewModLoaderShengcheng.m_fccaveBrushesByType.Add(new List<TerrainBrush>());//为每种洞穴类型创建一个空的刷子列表。
                for (int num83 = 0; num83 < 3; num83++)//内层循环，为每种洞穴类型生成3个刷子实例。
                {
                    TerrainBrush terrainBrush17 = new TerrainBrush();//创建一个新的空洞穴刷子实例。
                    int num84 = 6 + 4 * num82; //根据洞穴类型计算刷子的大小。每种类型增加4个单位，起始值为6。
                    int max = 7 + num82 / 3; //根据洞穴类型计算随机参数的上限。每3种类型增加1个单位，起始值为3。
                    int max2 = 12 + num82;  //根据洞穴类型计算随机参数的上限。每种类型增加1个单位，起始值为9。
                    for (int num85 = 0; num85 < num84; num85++) //循环生成刷子中的盒子形状，盒子的数量由num84定义。
                    {
                        int num86 = random.Int(2, max);//随机生成盒子的尺寸，范围为2到max之间。
                        int num87 = random.Int(11, max2) - num86;//随机生成盒子的数量，范围为8到max2之间，减去2倍盒子的尺寸。
                        Vector3 v15 = 0.5f * new Vector3(random.Float(-1f, 1f), random.Float(0f, 1f), random.Float(-1f, 1f));//生成一个随机的向量，用于确定盒子的位置偏移。
                        Vector3 vector17 = Vector3.Zero;//初始化盒子的起始位置为零向量。
                        for (int num88 = 0; num88 < num87; num88++)//循环生成多个盒子。
                        {
                            //在洞穴刷子中添加一个盒子形状，位置和尺寸由随机生成的参数确定。
                            terrainBrush17.AddBox((int)MathUtils.Floor(vector17.X) - num86 / 2, (int)MathUtils.Floor(vector17.Y) - num86 / 2, (int)MathUtils.Floor(vector17.Z) - num86 / 2, num86, num86, num86, 0);
                            //更新盒子的位置，加上随机生成的向量
                            vector17 += v15;
                        }
                    }
                    terrainBrush17.Compile();
                    NewModLoaderShengcheng.m_fccaveBrushesByType[num82].Add(terrainBrush17);
                }
            }
            #endregion
        }
        #endregion
        #endregion
        #region 岩浆周围生成
        /*public void PaintFastWithLava(TerrainChunk chunk, int x, int y, int z)
		{
			Terrain terrain = chunk.Terrain;
			x -= chunk.Origin.X;
			z -= chunk.Origin.Y;
			foreach (TerrainBrush.Cell cell in this.Cells)
			{
				int num = (int)cell.X + x;
				int num2 = (int)cell.Y + y;
				int num3 = (int)cell.Z + z;
				if (num >= 0 && num < 16 && num2 >= 0 && num2 < 255 && num3 >= 0 && num3 < 16)
				{
					int num4 = num + chunk.Origin.X;
					int y2 = num2;
					int num5 = num3 + chunk.Origin.Y;
					if (chunk.GetCellContentsFast(num, num2, num3) == 92 && terrain.GetCellContents(num4 - 1, y2, num5) == 92 && terrain.GetCellContents(num4 + 1, y2, num5) == 92 && terrain.GetCellContents(num4, y2, num5 - 1) == 92 && terrain.GetCellContents(num4, y2, num5 + 1) == 92 && chunk.GetCellContentsFast(num, num2 + 1, num3) == 92)
					{
						chunk.SetCellValueFast(num, num2, num3, cell.Value);
					}
				}
			}
		}*/


        #endregion
        public struct NumberWeightPair
        {
            public int number;
            public int weight;

            public NumberWeightPair(int number, int weight)
            {
                this.number = number;
                this.weight = weight;
            }
        }


        public static List<TerrainBrush> m_fcmagmaPocketBrushes = new List<TerrainBrush>();
        public static List<TerrainBrush> m_fcmagmaPocketBrushes2 = new List<TerrainBrush>();
        public static List<TerrainBrush> m_fcMoonPocketBrushes = new List<TerrainBrush>();
        public static List<List<TerrainBrush>> m_fccaveBrushesByType = new List<List<TerrainBrush>>();


        #region 地形板块生成(还未应用）
        public void FCGenerateTerrain(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            int num = x2 - x1;
            int num2 = z2 - z1;
            Terrain terrain = this.m_subsystemTerrain.Terrain;
            int num3 = chunk.Origin.X + x1;
            int num4 = chunk.Origin.Y + z1;
            TerrainContentsGenerator23.Grid2d grid2d = new TerrainContentsGenerator23.Grid2d(num, num2);
            TerrainContentsGenerator23.Grid2d grid2d2 = new TerrainContentsGenerator23.Grid2d(num, num2);
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    grid2d.Set(j, i, this.CalculateOceanShoreDistance((float)(j + num3), (float)(i + num4)));
                    grid2d2.Set(j, i, this.CalculateMountainRangeFactor((float)(j + num3), (float)(i + num4)));
                }
            }
            TerrainContentsGenerator23.Grid3d grid3d = new TerrainContentsGenerator23.Grid3d(num / 4 + 1, 33, num2 / 4 + 1);
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int num5 = k * 4 + num3;
                    int num6 = l * 4 + num4;
                    float num7 = this.CalculateHeight((float)num5, (float)num6);
                    float v = this.CalculateMountainRangeFactor((float)num5, (float)num6);
                    float num8 = MathUtils.Lerp(this.TGMinTurbulence, 1f, TerrainContentsGenerator23.Squish(v, this.TGTurbulenceZero, 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        int num9 = m * 8;
                        float num10 = this.TGTurbulenceStrength * num8 * MathUtils.Saturate(num7 - (float)num9) * (2f * SimplexNoise.OctavedNoise((float)num5, (float)num9, (float)num6, this.TGTurbulenceFreq, this.TGTurbulenceOctaves, 4f, this.TGTurbulencePersistence, false) - 1f);
                        float num11 = (float)num9 + num10;
                        float num12 = num7 - num11;
                        num12 += MathUtils.Max(4f * (this.TGDensityBias - (float)num9), 0f);
                        grid3d.Set(k, m, l, num12);
                    }
                }
            }
            SubsystemTerrain subsystemTerrain = new SubsystemTerrain();
            TerrainContentsGenerator23 generator = new TerrainContentsGenerator23(subsystemTerrain);
            int oceanLevel = generator.OceanLevel;

            for (int n = 0; n < grid3d.SizeX - 1; n++)
            {
                for (int num13 = 0; num13 < grid3d.SizeZ - 1; num13++)
                {
                    for (int num14 = 0; num14 < grid3d.SizeY - 1; num14++)
                    {
                        float num15;
                        float num16;
                        float num17;
                        float num18;
                        float num19;
                        float num20;
                        float num21;
                        float num22;
                        grid3d.Get8(n, num14, num13, out num15, out num16, out num17, out num18, out num19, out num20, out num21, out num22);
                        float num23 = (num16 - num15) / 4f;
                        float num24 = (num18 - num17) / 4f;
                        float num25 = (num20 - num19) / 4f;
                        float num26 = (num22 - num21) / 4f;
                        float num27 = num15;
                        float num28 = num17;
                        float num29 = num19;
                        float num30 = num21;
                        for (int num31 = 0; num31 < 4; num31++)
                        {
                            float num32 = (num29 - num27) / 4f;
                            float num33 = (num30 - num28) / 4f;
                            float num34 = num27;
                            float num35 = num28;
                            for (int num36 = 0; num36 < 4; num36++)
                            {
                                float num37 = (num35 - num34) / 8f;
                                float num38 = num34;
                                int num39 = num31 + n * 4;
                                int num40 = num36 + num13 * 4;
                                int x3 = x1 + num39;
                                int z3 = z1 + num40;
                                float num41 = grid2d.Get(num39, num40);
                                float num42 = grid2d2.Get(num39, num40);
                                int temperatureFast = chunk.GetTemperatureFast(x3, z3);
                                int humidityFast = chunk.GetHumidityFast(x3, z3);
                                float num43 = num42 - 0.01f * (float)humidityFast;
                                float num44 = MathUtils.Lerp(100f, 0f, num43);
                                float num45 = MathUtils.Lerp(300f, 30f, num43);
                                bool flag = (temperatureFast > 8 && humidityFast < 8 && num42 < 0.97f) || (MathUtils.Abs(num41) < 16f && num42 < 0.97f);
                                int num46 = TerrainChunk.CalculateCellIndex(x3, 0, z3);
                                for (int num47 = 0; num47 < 8; num47++)
                                {
                                    int num48 = num47 + num14 * 8;
                                    int value = 0;
                                    if (num38 < 0f)
                                    {
                                        if (num48 <= oceanLevel)
                                        {
                                            value = 18;
                                        }
                                    }
                                    else
                                    {
                                        value = ((!flag) ? ((num38 >= num45) ? 67 : 3) : ((num38 >= num44) ? ((num38 >= num45) ? 67 : 3) : 4));
                                    }
                                    chunk.SetCellValueFast(num46 + num48, value);
                                    num38 += num37;
                                }
                                num34 += num32;
                                num35 += num33;
                            }
                            num27 += num23;
                            num28 += num24;
                            num29 += num25;
                            num30 += num26;
                        }
                    }
                }
            }
        }

        #endregion

        #region 地表群系生成/村庄生成

        public class RoadPoint//路径点
        {
            public Point3 Position;



            public int BrushType;//这是道路类型，0为沿x轴的纵向路面，1为沿着z轴的横向路面，2为弯道。

            public bool TG;//代表该路径点是否已经生成

            public bool TFirst;//判断该路径点是否为村庄起点。

            public bool Tturn;//判断该路径点是否为转折点。

            public bool is_Vice;//判断是否为副路

            public Point2 chunkCoords;//储存区块绝对坐标

        }
        public static List<NewModLoaderShengcheng.RoadPoint> listRD = new List<NewModLoaderShengcheng.RoadPoint>();//创建一个名为listRD的空列表，用于储存路径点的信息。
        public static List<NewModLoaderShengcheng.BuildPoint> listBD = new List<NewModLoaderShengcheng.BuildPoint>();//创建一个名为listBD的空列表，用于存储建筑点的信息。
        public static List<NewModLoaderShengcheng.RoadPoint> listRDF = new List<NewModLoaderShengcheng.RoadPoint>();//创建一个名为listRDF的空列表，用于存储村庄起点的信息。
        public static Dictionary<Point2, int> Dic_Chunk_Village3 = new Dictionary<Point2, int>();  //村庄区块
        public static Dictionary<Point2, TerrainChunk> Dic_Chunk_Village_Build = new Dictionary<Point2, TerrainChunk>(); //建筑点区块。


        public int StepsTaken;//判断村庄生成的步长。
        public int StepsTaken_Max;//判断村庄生成的最大步长。
        public class BuildPoint//建筑点
        {
            public Point3 Position;



            public int BrushType;

            public bool TG;//代表该路径点是否已经生成


        }



        /*              try
						{public ComponentPlayer m_componentPlayer;

						}
						catch (Exception ex)
						{
							Log.Error($"报错是,{ex.Message}");
						}
		*/

        public void FCGenerateSurface(TerrainChunk chunk)
        {
            //Dictionary<Point2, int> Dic_Chunk_Village = m_subsystemtownchunk.Dic_Chunk_Village;
            ComponentPlayer m_componentPlayer = new ComponentPlayer();
            FCSubsystemTown fCSubsystemTown = new FCSubsystemTown();
            bool flag_tree = true;//是否生成树。当一个区块生成村庄的时候，那么这个区块不会生成树
            bool flag_v2 = true;//是否生成村庄起点，如果已经生成过，则改为false。优化算法。
            bool YH_tree = true;//默认樱花树生成一次，优化算法，避免重复检测区块进行无用生成。
            bool RD_tree = true;//默认单区块热带树生成一次。优化算法。
            bool flag_v = false;//根据高度差判断是否生成村庄
            int origin = chunk.Origin.X;
            int origin2 = chunk.Origin.Y;
            int x1 = chunk.Coords.X;//获取区块的绝对坐标。
            int y1 = chunk.Coords.Y;
            int middle_x = origin + 8;
            int middle_z = origin2 + 8;
            Vector3 player_position = m_subsystemTerrain.TerrainContentsGenerator.FindCoarseSpawnPosition();//获取玩家出生点

            Point2 player_positon2 = ((int)player_position.X, (int)player_position.Z);//转化成point2坐标
            Point2 point_village_T = (x1, y1);//获取当前区块的绝对坐标，用于和区块字典进行比对，判断是否为村庄区块，如果是村庄区块则不进行起点生成。

            Terrain terrain = this.m_subsystemTerrain.Terrain;
            int middle_y = terrain.CalculateTopmostCellHeight(middle_x, middle_z);//获取当前区块中心点的高度
            Random random = new Random(this.m_seed + chunk.Coords.X + 101 * chunk.Coords.Y);


            List<int> height = new List<int>(); // 创建一个空的 List<int>,储存区块高度

            bool isvillage_chunk = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_village_T);
            bool ischunkload2 = Dic_Chunk_Village3.ContainsKey(point_village_T);
            foreach (BuildingInfo buildinfo in m_subsystemNaturallyBuildings.BuildingInfos)
            {
                if (listRD.Count == 0 && isvillage_chunk == false && ischunkload2 == false && buildinfo.CalculatPlainRange(chunk.Coords) == false) ;//如果路径点为空，且不为村庄区块，则执行遍历区块，村庄符合第一条件。
                {
                    int num_villageRange = 0;//用来检测村庄之间的距离，计数。


                    for (int i = 0; i < 16; i++)//遍历当前区块
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            int num1 = i + chunk.Origin.X;//x坐标

                            int num2 = j + chunk.Origin.Y;//z坐标
                            int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度
                            height.Add(num7);//统计区块内的所有地表高度
                        }
                    }

                    int max1 = height[0];
                    int min1 = height[0];

                    for (int i = 1; i < height.Count; i++)//计算高度差
                    {
                        if (height[i] > max1)
                        {
                            max1 = height[i];
                        }
                        if (height[i] < min1)
                        {
                            min1 = height[i];//该方法第一部分先对该区块进行高度差判断。
                        }
                    }

                    int height1 = (max1 - min1);

                    if (num_villageRange < Village_start.Count)
                    {
                        for (int i1 = 0; i1 < Village_start.Count; i1++)
                        {
                            int point_x = Village_start[i1].X;
                            int point_z = Village_start[i1].Z;

                            double distance1 = MathUtils.Sqrt(((MathUtils.Abs(middle_x - point_x)) * (MathUtils.Abs(middle_x - point_x))) + ((MathUtils.Abs(middle_z - point_z)) * (MathUtils.Abs(middle_z - point_z))));
                            if (distance1 > 1000)
                            {
                                num_villageRange++;
                            }

                        }
                    }

                    if (height1 < 5 && middle_y > 64 && middle_y < 84 && Village_start.Count == 0 && random.Bool(0.73f))//当地图还没有村庄的时候，先找到第一个可生成点生成村庄。
                    {


                        double distance1 = MathUtils.Sqrt(((MathUtils.Abs(middle_x - player_positon2.X)) * (MathUtils.Abs(middle_x - player_positon2.X))) + ((MathUtils.Abs(middle_z - player_positon2.Y)) * (MathUtils.Abs(middle_z - player_positon2.Y))));

                        if (distance1 > 500)//大于出生点300距离
                        {
                            flag_v = true;
                        }

                    }
                    if (height1 < 5 && middle_y > 64 && middle_y < 84 && ischunkloding2 == false && num_villageRange == Village_start.Count && num_villageRange != 0 && Village_start.Count != 0 && random.Bool(0.73f))//如果高度差小于5，且中心点位于高度66-86之间,距离每一个村庄点都大于300。生成村庄。
                    {
                        flag_v = true;
                    }
                }
            }





            for (int i = 0; i < 16; i++)//遍历当前区块  这里的循环执行的是泥土，村庄路径点建筑点，树生成。
            {
                for (int j = 0; j < 16; j++)
                {
                    int num = i + chunk.Origin.X;
                    int num2 = j + chunk.Origin.Y;
                    int num3 = TerrainChunk.CalculateCellIndex(i, 254, j);
                    int k = 254;

                    while (k >= 64)
                    {

                        int cellValueFast = chunk.GetCellValueFast(num3);//获取当前单元格的值。
                        int num4 = Terrain.ExtractContents(cellValueFast);//从单元格值中提取方块的内容
                        if (!BlocksManager.Blocks[num4].IsTransparent_(cellValueFast)) //检查当前单元格的方块是否为不透明的方块。如果是不透明的方块，则说明已经到达地表下方，结束当前网格点的生成。
                        {

                            float num5 = this.CalculateMountainRangeFactor((float)num, (float)num2);
                            //根据当前网格点的全局坐标计算山脉范围因子。这个因子可能用于调整地表的特征。
                            int temperature = terrain.GetTemperature(num, num2);
                            int humidity = terrain.GetHumidity(num, num2);
                            //获取当前网格点的温度和湿度。这些值可能影响地表生成中的一些特征。
                            int num6 = 8;//定义一个整数6，这代表要生成的内容。

                            int num8 = (k + 1 < 255) ? chunk.GetCellContentsFast(i, k + 1, j) : 0;
                            int numfc1 = (k + 1 < 255) ? chunk.GetCellContentsFast(i, k - 1, j) : 0;
                            //获取当前网格点上方单元格的内容。
                            //下面是核心判断生成语句

                            if (num4 == 8)//如果是草方块
                            {




                                if (num8 != 233 && num8 != 232 && num8 != 18 && numfc1 == 2 && humidity >= 5 && temperature >= 3 && temperature <= 10)
                                //否则，根据上方单元格的内容（num8）、湿度、湿度模 2 余数（humidity % 2），
                                //以及温度模 3 余数（temperature % 3）进行条件判断：
                                //如果上方单元格的内容不是砂岩（ID为 4），或者湿度小于等于 8，或者湿度模 2 余数不为 0，
                                //或者温度模 3 余数不为 0，方块内容为普通的土壤（ID为 2）。
                                {
                                    num6 = 984;//土

                                }
                                else if (num8 != 233 && num8 != 232 && num8 != 18 && numfc1 == 2 && humidity >= 7 && temperature >= 8)
                                {
                                    num6 = 985;//热带土
                                }

                            }
                            else
                            {
                                return;
                            }








                            /*//根据温度、湿度、高度和上方单元格的内容来确定当前网格点的方块内容。
							int num9;//声明一个变量用于存储当前网格点的高度。
							
							float num10 = MathUtils.Saturate(((float)k - 100f) * 0.05f);//根据当前高度计算一个饱和度值。
							float num11 = MathUtils.Saturate(MathUtils.Saturate((num5 - 0.9f) / 0.1f) - MathUtils.Saturate(((float)humidity - 3f) / 12f) + TerrainContentsGenerator23.TGSurfaceMultiplier * num10);
							//根据山脉范围因子、湿度和饱和度计算一个调整值。


							int min = (int)MathUtils.Lerp(4f, 0f, num11);//根据调整值将方块的高度范围限制在最小值和最大值之间。
							int max = (int)MathUtils.Lerp(7f, 0f, num11);
							num9 = MathUtils.Min(random.Int(min, max), k);//使用随机数生成器在高度范围内生成一个随机高度。
							//以上未使用*/


                            /*int num12 = TerrainChunk.CalculateCellIndex(i, k + 1, j);//计算当前网格点上方单元格的索引。
							Block block1 = BlocksManager.Blocks[Terrain.ExtractContents(chunk.GetCellValueFast(num12))];
							if(isvillage_chunk == false&& tg_num == 0)//如果不是村庄区块则生成。
                            {
								if (Terrain.ExtractContents(chunk.GetCellValueFast(num12)) == 0||block1.IsTransparent==true)//如果当前单元格不为空方块。
								{

									int value1 = Terrain.ReplaceContents(0, num6);//将当前网格点的方块内容替换为新的方块内容。
									chunk.SetCellValueFast(num12 - 1, value1);


								}
							}*/

                            //上面是生成地表群系泥土。


                            if (num6 != 8)//如果生成了群系土，说明群系生成成功，接下来判断是否开始生成村庄。
                            {

                                if (flag_v == true && flag_v2 == true)//如果当前区块生成村庄起点。高度差合适！
                                {
                                    if (listRD.Count == 0)//如果当前的列表是空的。说明村庄还没有起点。则生成起点。
                                    {
                                        listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                        {
                                            Position = (middle_x, middle_y, middle_z),
                                            BrushType = 0,
                                            TG = false,//代表该路径点是否已经生成
                                            TFirst = true,//判断该路径点是否为村庄起点。
                                            Tturn = false,//判断该路径点是否为转折点。
                                            is_Vice = false,
                                            chunkCoords = (chunk.Coords.X, chunk.Coords.Y),

                                        });
                                        Point3 point_start = (middle_x, middle_y, middle_z);
                                        Village_start.Add(point_start);//记录村庄起点。


                                        listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                        {
                                            Position = (middle_x, middle_y, middle_z + 16),
                                            BrushType = 0,
                                            TG = false,//代表该路径点是否已经生成
                                            TFirst = true,//判断该路径点是否为村庄起点。
                                            Tturn = false,//判断该路径点是否为转折点。
                                            is_Vice = true,
                                            chunkCoords = (chunk.Coords.X, chunk.Coords.Y + 1),

                                        });
                                        listBD.Add(new NewModLoaderShengcheng.BuildPoint//同时生成建筑点
                                        {
                                            Position = (middle_x, middle_y, middle_z + 16),
                                            BrushType = 0,
                                            TG = false,//代表该路径点是否已经生成


                                        });

                                        Log.Information(string.Format("村庄起点的坐标是：{0}, {1}, {2}", middle_x, middle_y, middle_z));

                                        for (int a = 1; a < 5; a++)
                                        {
                                            if (a == 2)//如果生成到第三个路径点，则第三个为转折点
                                            {
                                                listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                                {
                                                    Position = (listRD[a].Position.X + 16, middle_y, listRD[a].Position.Z),
                                                    BrushType = 2,//拐弯
                                                    TG = false,//代表该路径点是否已经生成
                                                    TFirst = false,//判断该路径点是否为村庄起点。
                                                    Tturn = true,//判断该路径点是否为转折点。
                                                    chunkCoords = (chunk.Coords.X + 2, chunk.Coords.Y),

                                                });
                                                Log.Information(string.Format("村庄转折点的坐标是：{0}, {1}, {2}", listRD[a].Position.X + 16, middle_y, listRD[a].Position.Z));

                                            }
                                            else
                                            {
                                                if (a > 2)//拐弯后
                                                {
                                                    if (a == 3)
                                                    {
                                                        listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                                        {
                                                            Position = (listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16),
                                                            BrushType = 1,//z轴路径
                                                            TG = false,//代表该路径点是否已经生成
                                                            TFirst = false,//判断该路径点是否为村庄起点。
                                                            Tturn = false,//判断该路径点是否为转折点。
                                                            is_Vice = false,
                                                            chunkCoords = (chunk.Coords.X + 2, chunk.Coords.Y + 1),

                                                        });
                                                        Log.Information(string.Format("村庄z轴路径的坐标是：{0}, {1}, {2}", listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16));
                                                    }

                                                    if (a == 4)
                                                    {
                                                        listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                                        {
                                                            Position = (listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16),
                                                            BrushType = 1,//z轴路径
                                                            TG = false,//代表该路径点是否已经生成
                                                            TFirst = false,//判断该路径点是否为村庄起点。
                                                            Tturn = false,//判断该路径点是否为转折点。
                                                            is_Vice = false,
                                                            chunkCoords = (chunk.Coords.X + 2, chunk.Coords.Y + 2),

                                                        });
                                                        Log.Information(string.Format("村庄z轴路径的坐标是：{0}, {1}, {2}", listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16));
                                                        listRD.Add(new NewModLoaderShengcheng.RoadPoint
                                                        {
                                                            Position = (listRD[6].Position.X - 16, middle_y, listRD[6].Position.Z),
                                                            BrushType = 1,//z轴路径
                                                            TG = false,//代表该路径点是否已经生成
                                                            TFirst = false,//判断该路径点是否为村庄起点。
                                                            Tturn = false,//判断该路径点是否为转折点。
                                                            is_Vice = true,
                                                            chunkCoords = (chunk.Coords.X + 1, chunk.Coords.Y + 2),

                                                        });
                                                        listBD.Add(new NewModLoaderShengcheng.BuildPoint//同时生成建筑点
                                                        {
                                                            Position = (listRD[a - 1].Position.X - 16, middle_y, listRD[a - 1].Position.Z + 16),
                                                            BrushType = 1,
                                                            TG = false,//代表该路径点是否已经生成


                                                        });
                                                    }

                                                }
                                                else
                                                {
                                                    listRD.Add(new NewModLoaderShengcheng.RoadPoint//主路，x轴方向
                                                    {
                                                        Position = (listRD[0].Position.X + 16, middle_y, listRD[0].Position.Z),
                                                        BrushType = 0,//x轴路径
                                                        TG = false,//代表该路径点是否已经生成
                                                        TFirst = false,//判断该路径点是否为村庄起点。
                                                        Tturn = false,//判断该路径点是否为转折点。
                                                        is_Vice = false,
                                                        chunkCoords = (chunk.Coords.X + 1, chunk.Coords.Y),
                                                    });
                                                    Log.Information(string.Format("村庄主路的坐标是：{0}, {1}, {2}", listRD[0].Position.X + 16, middle_y, listRD[0].Position.Z));
                                                    listRD.Add(new NewModLoaderShengcheng.RoadPoint//主路，x轴方向
                                                    {
                                                        Position = (listRD[2].Position.X, middle_y, listRD[2].Position.Z + 16),
                                                        BrushType = 0,//x轴路径
                                                        TG = false,//代表该路径点是否已经生成
                                                        TFirst = false,//判断该路径点是否为村庄起点。
                                                        Tturn = false,//判断该路径点是否为转折点。
                                                        is_Vice = true,
                                                        chunkCoords = (chunk.Coords.X + 1, chunk.Coords.Y + 1),
                                                    });
                                                    listBD.Add(new NewModLoaderShengcheng.BuildPoint//同时生成建筑点右侧
                                                    {
                                                        Position = (listRD[a - 1].Position.X + 16, middle_y, listRD[a - 1].Position.Z + 16),
                                                        BrushType = 0,
                                                        TG = false,//代表该路径点是否已经生成


                                                    });
                                                }

                                            }

                                        }


                                        flag_v2 = false;//让本区块的村庄起点判断只执行一次。
                                    }



                                }
                            }



                            #region 生成树
                            if (num6 != 8)//如果允许生成树
                            {

                                if (listRD.Count != 0)
                                {
                                    if (isvillage_chunk == true) ;
                                    {
                                        flag_tree = false;

                                    }
                                }




                                if (flag_tree == true)
                                {
                                    if (num6 == 984 && YH_tree == true)//如果当前生成的是樱花土，则执行樱花树生成判定
                                    {
                                        for (int a = x1; a <= x1; a++)
                                        {
                                            for (int b = y1; b <= y1; b++)
                                            {
                                                float num80 = MathUtils.Saturate((SimplexNoise.OctavedNoise(a, b, 0.1f, 2, 2f, 0.5f) - 0.25f) / 0.2f + (random.Bool(0.25f) ? 0.5f : 0f));
                                                int num81 = (int)(5f * num80);
                                                int num82 = 0;
                                                for (int k11 = 0; k11 < 20; k11++) //生成k 32棵树
                                                {
                                                    if (num82 >= num81) //如果已生成的树木大于num3，则停止
                                                    {
                                                        break;
                                                    }//噪声算法

                                                    int cx = a * 16 + random.Int(2, 13); //cx cz用来计算树生成的位置
                                                    int cz = b * 16 + random.Int(2, 13);
                                                    int num7 = terrain.CalculateTopmostCellHeight(cx, cz);//获取预生成位置的最大高度块
                                                    if (num7 < 66)//高度大于66，不然跳过，继续循环
                                                    {
                                                        continue;
                                                    }


                                                    int cellContentsFast = terrain.GetCellContentsFast(cx, num7, cz); //获取这个预生成方块的值

                                                    if (cellContentsFast != 2 && cellContentsFast != 8 && cellContentsFast != 984)//如果不是泥土或者草地
                                                    {
                                                        continue;
                                                    }//选择方块

                                                    num7++; //高度加一，继续生成
                                                            //这个if是检查树周围的方块是否可碰撞。
                                                    if (!BlocksManager.Blocks[terrain.GetCellContentsFast(cx + 1, num7, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx - 1, num7, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, num7, cz + 1)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, num7, cz - 1)].IsCollidable)
                                                    {

                                                        // 生成樱花树
                                                        // 获取樱花树的地形刷子
                                                        ReadOnlyList<TerrainBrush> cherryBlossomBrushes = FCPlantManager.GetTreeBrushes(FCTreeType.Yinghua);
                                                        TerrainBrush terrainBrush = cherryBlossomBrushes[random.Int(cherryBlossomBrushes.Count)];
                                                        // 应用樱花树的地形刷子到地形上

                                                        terrainBrush.PaintFast(chunk, cx, num7, cz);
                                                        chunk.AddBrushPaint(cx, num7, cz, terrainBrush);

                                                        num82++; //成功生成一个树木，num82加1

                                                    }
                                                }


                                            }
                                        }

                                        YH_tree = false;


                                    }

                                    if (num6 == 985 && RD_tree == true)//如果当前生成的是热带群系土，则执行果树的生成判定
                                    {

                                        for (int a = x1; a <= x1; a++)
                                        {
                                            for (int b = y1; b <= y1; b++)
                                            {
                                                float num80 = MathUtils.Saturate((SimplexNoise.OctavedNoise(a, b, 0.1f, 2, 2f, 0.5f) - 0.25f) / 0.2f + (random.Bool(0.25f) ? 0.5f : 0f));
                                                int num81 = (int)(5f * num80);
                                                int num82 = 0;
                                                for (int c = 0; c < 10; c++) //生成c 10棵树
                                                {
                                                    if (num82 >= num81) //如果已生成的树木大于num3，则停止
                                                    {
                                                        break;
                                                    }//噪声算法

                                                    int cx = a * 16 + random.Int(2, 13); //cx cz用来计算树生成的位置
                                                    int cz = b * 16 + random.Int(2, 13);
                                                    int num7 = terrain.CalculateTopmostCellHeight(cx, cz);//获取预生成位置的最大高度块
                                                    FCTreeType[] treeTypes = new FCTreeType[] { FCTreeType.Apple, FCTreeType.Orange, FCTreeType.Coco };
                                                    FCTreeType selectedTreeType = treeTypes[random.Int(treeTypes.Length)];

                                                    int cellContentsFast = terrain.GetCellContentsFast(cx, num7, cz); //获取这个预生成方块的值

                                                    if (cellContentsFast != 2 && cellContentsFast != 8 && cellContentsFast != 984)//如果不是泥土或者草地
                                                    {
                                                        continue;
                                                    }//选择方块

                                                    num7++; //高度加一，继续生成
                                                            //这个if是检查树周围的方块是否可碰撞。
                                                    if (!BlocksManager.Blocks[terrain.GetCellContentsFast(cx + 1, num7, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx - 1, num7, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, num7, cz + 1)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, num7, cz - 1)].IsCollidable)
                                                    {

                                                        if (random.Bool(0.1f))
                                                        {
                                                            selectedTreeType = FCTreeType.Lorejun;//十分之一的概率为老君树
                                                        }
                                                        ReadOnlyList<TerrainBrush> treeBrushes = FCPlantManager.GetTreeBrushes(selectedTreeType);
                                                        TerrainBrush terrainBrush = treeBrushes[random.Int(treeBrushes.Count)];
                                                        terrainBrush.PaintFast(chunk, cx, num7, cz);
                                                        chunk.AddBrushPaint(cz, num7, cz, terrainBrush);

                                                        num82++; //成功生成一个树木，num82加1
                                                    }
                                                }
                                            }
                                        }

                                        RD_tree = false;


                                    }



                                    break;//生成树之后才会打破循环
                                }


                            }
                            #endregion

                        }
                        k--;
                        num3--;
                    }


                }
            }
        }

        #endregion

        #region 村庄生成子方法


        public bool ischunkloding2 = false;//用来保证村庄生成的全过程独立性
        public int Bg_num = 0;//计算副建筑区块的生成个数，到3清零。//建筑列表暂时未使用过。
        public int tg_num = 0;//用来计算村庄的生成个数，到5清零。
        public bool ischunkloding = false;//用来保证区块强制加载只执行一次
        public async void FCGenerateVillage(TerrainChunk chunk)
        {
            ischunkloding2 = true;
            SubsystemBlockEntities entityScan = new SubsystemBlockEntities();//实体检测
            SubsystemCreatureSpawn spawn1 = new SubsystemCreatureSpawn();
            FCSubsystemTownChunk m_subsystemtownchunk = new FCSubsystemTownChunk();
            int origin = chunk.Origin.X;
            int origin2 = chunk.Origin.Y;
            int x1 = chunk.Coords.X;//获取区块的绝对坐标。
            int y1 = chunk.Coords.Y;
            int middle_x = origin + 8;
            int middle_z = origin2 + 8;
            Terrain terrain = m_subsystemTerrain.Terrain;

            int middle_y = terrain.CalculateTopmostCellHeight(middle_x, middle_z);//获取当前区块中心点的高度
            Random random = new Random(this.m_seed + chunk.Coords.X + 555 * chunk.Coords.Y);
            if (ischunkloding == false)
            {
                for (int i = 0; i < listRD.Count; i++)
                {

                    AddChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);//加载区块
                    await System.Threading.Tasks.Task.Delay(100);

                }
                ischunkloding = true;
            }
            
                if (listRD.Count != 0 )//如果路径点等于5，说明已经路径点已经完毕，执行生成。
                {
                    //await Task.Delay(100);
                    for (int i = 0; i < listRD.Count; i++)
                    {
                        TerrainChunk chunk_v = terrain.GetChunkAtCell(listRD[i].Position.X, listRD[i].Position.Z);


                        if (chunk_v == null || listRD[i].TG == true)//如果区块为空，则跳过该路径点的生成。
                        {
                            continue;
                        }
                        else  //如果不为空，则准备生成
                        {
                            int chunkX1 = chunk_v.Coords.X; //根据坐标判断
                            int chunkY1 = chunk_v.Coords.Y;

                            if (x1 > chunkX1 && y1 > chunkY1)
                            {


                                if (listRD[i].TG == false && listRD[i].BrushType == 0)//x轴主路
                                {
                                    List<int> height_T = new List<int>(); // 创建一个空的 List<int>,储存区块高度
                                    int x_T = chunk_v.Origin.X;//获取该路径点区块的原始坐标。
                                    int z_T = chunk_v.Origin.Y;
                                    int x_T_coords = chunk_v.Coords.X;
                                    int z_T_coords = chunk_v.Coords.Y;
                                    Point2 point_t1 = (x_T_coords, z_T_coords);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

                                    bool ischunkload = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_t1);//检测存档保存的坐标
                                    bool ischunkload2 = Dic_Chunk_Village3.ContainsKey(point_t1);//检测游戏进行时候的坐标

                                    if (ischunkload == false && ischunkload2 == false)
                                    {
                                        for (int a1 = 0; a1 < 16; a1++)//遍历路径点区块这里是先摧毁区块的多余物质。
                                        {
                                            for (int j1 = 0; j1 < 16; j1++)
                                            {

                                                int num1 = a1 + x_T;//x坐标

                                                int num2 = j1 + z_T;//z坐标
                                                                    //int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度
                                                int k = 110;//从86的高度开始遍历

                                                while (k > 64)// 当k高度大于65的时候。 
                                                {

                                                    int num3 = terrain.GetCellContentsFast(num1, k, num2);//获取起始的高度的方块
                                                    if (num3 != 8 || num3 != 3 || num3 != 7 || num3 != 4 || num3 != 2 || num3 != 985 || num3 != 984 || num3 != 18 || num3 != 6 || num3 != 66)//如果不是草地、沙子、砂岩、花岗岩，水，则销毁
                                                    {

                                                        terrain.SetCellValueFast(num1, k + 1, num2, 0);

                                                    }
                                                    if (num3 == 8 || num3 == 3 || num3 == 7 || num3 == 4 || num3 == 2 || num3 == 985 || num3 == 18 || num3 == 984 || num3 == 6 || num3 == 66)
                                                    {


                                                        break;
                                                    }

                                                    k--;

                                                }
                                                int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度块
                                                height_T.Add(num7);//统计区块内的所有地表高度


                                            }

                                        }
                                        int list_count = height_T.Count;
                                        Log.Information(String.Format("列表元素个数为：{0}", list_count));
                                        int max1 = height_T[0];
                                        int min1 = height_T[0];

                                        for (int i1 = 1; i1 < height_T.Count; i1++)//计算高度差
                                        {
                                            if (height_T[i1] > max1)
                                            {
                                                max1 = height_T[i1];
                                            }
                                            if (height_T[i1] < min1)
                                            {
                                                min1 = height_T[i1];
                                            }
                                        }

                                        int height_T1 = (max1 - min1);//高度差


                                        /*if (height_T1 > 7)// 如果高度大于13，过于崎岖，不生成建筑。
                                        {
                                            Point2 pointRD2 = (chunk_v.Coords.X, chunk_v.Coords.Y);//把当前区块记录下来，作为村庄区块。
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD2) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD2, 0);
                                            }
                                            listRD[i].TG = true;
                                            tg_num++;

                                            continue;
                                        }*/
                                        if (listRD[i].is_Vice == false)//如果不是副路，是主路
                                        {
                                            for (int a1 = 14; a1 < 16; a1++)//遍历路径点区块的路径z轴
                                            {
                                                for (int j1 = 0; j1 < 16; j1++)//x轴
                                                {
                                                    int top_height = height_T[a1 + j1 * 16];
                                                    terrain.SetCellValueFast(x_T + j1, top_height, z_T + a1, 5);
                                                }
                                            }
                                            Log.Information(string.Format("村庄起点和主路x轴生成完毕：{0}, {1}, {2}", listRD[i].Position.X, listRD[i].Position.Y, listRD[i].Position.Z));
                                            Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD1, 0);
                                            }

                                            listRD[i].TG = true;

                                        }


                                        /*for (int a2 = 0; a2 < 14; a2++) //z轴 执行建筑生成。
                                        {
                                            for(int j2 = 0; j2 < 16; j2++)//x轴
                                            {
                                                int num1 = j2 + x_T;//x坐标

                                                int num2 = a2 + z_T;//z坐标


                                                int Y_t = (min1 + max1) / 2;
                                                int k = max1 -Y_t;//从86的高度开始遍历
                                                while (k > Y_t)// 当k高度大于65的时候。 
                                                {
                                                    terrain.SetCellValueFast(num1, k, z_T + num2, 0);
                                                    k--;
                                                }
                                            }
                                        }*/

                                        List<string> HouseX = new List<string>() { "House/IronHouse", "House/chunkTwohouse" }; ;
                                        if (listRD[i].is_Vice == false)//如果不是副路
                                        {
                                            if (i == 2)//如果是主路第二条。
                                            {
                                                if (random.Bool(0.5f))
                                                {
                                                    try
                                                    {
                                                        string blocks = ContentManager.Get<string>("House/Livinghouse02");
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z;
                                                                int Block_reset = Terrain.ExtractContents(terrain.GetCellValueFast(x1t, y1t, z1t));
                                                                try
                                                                {
                                                                    if (Block_reset == 27)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation1 failed.");
                                                                    break;
                                                                }



                                                                if (Terrain.ExtractContents(block_house) == 27)//如果是工作台
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }

                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }


                                                    tg_num++;
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        string blocks = ContentManager.Get<string>("House/IronHouse");
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z;
                                                                int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                                try
                                                                {
                                                                    if (Block_reset == 64 || Block_reset == 45)
                                                                    {
                                                                        continue;

                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation2 failed.");
                                                                    break;
                                                                }


                                                                if (Terrain.ExtractContents(block_house) == 64)//如果是熔炉
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }
                                                                if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                        ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                        int Item_num;
                                                                        //126钻石块，46铁块，71孔雀石，47铜块，231锗块,228燃烧炸弹，201炸弹，236燃烧炸药，107普通炸药,150煤块，167磁铁，248经验，132南瓜灯
                                                                        //196羽毛，198线，206布 ，40铁锭，22煤

                                                                        int[] blocks1 = { 40, 22 };
                                                                        for (int a11 = 0; a11 < 5; a11++)//遍历箱子格子
                                                                        {
                                                                            if (a11 < 4)//煤矿的分布概率
                                                                            {
                                                                                if (random.Bool(0.1f))
                                                                                {
                                                                                    Item_num = 3;

                                                                                }
                                                                                else
                                                                                {
                                                                                    Item_num = 1;
                                                                                }
                                                                                chest_componentchest.AddSlotItems(a11, blocks1[1], Item_num);
                                                                            }
                                                                            else if (a11 == 4)
                                                                            {
                                                                                if (random.Bool(0.05f))
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[0], 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[1], 2);
                                                                                }
                                                                            }

                                                                        }
                                                                    }




                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }


                                                    tg_num++;

                                                }
                                            }
                                            else if (i == 0)//如果不是主路第二条
                                            {
                                                if (random.Bool(0.5f))
                                                {
                                                    try
                                                    {
                                                        string blocks = ContentManager.Get<string>("House/chunkTwohouse");
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z;
                                                                int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                                try
                                                                {
                                                                    if (Block_reset == 27)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation3 failed.");
                                                                    break;
                                                                }



                                                                if (Terrain.ExtractContents(block_house) == 27)//如果是工作台
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }

                                                    tg_num++;
                                                }
                                                else
                                                {
                                                    try

                                                    {
                                                        string blocks = ContentManager.Get<string>("House/IronHouse");
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z;
                                                                int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                                try
                                                                {
                                                                    if (Block_reset == 64 || Block_reset == 45)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation4 failed.");
                                                                    break;
                                                                }


                                                                if (Terrain.ExtractContents(block_house) == 64)//如果是熔炉
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }


                                                                }
                                                                if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);

                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                        ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                        int Item_num;
                                                                        //126钻石块，46铁块，71孔雀石，47铜块，231锗块,228燃烧炸弹，201炸弹，236燃烧炸药，107普通炸药,150煤块，167磁铁，248经验，132南瓜灯
                                                                        //196羽毛，198线，206布 ，40铁锭，22煤

                                                                        int[] blocks1 = { 40, 22, };
                                                                        for (int a11 = 0; a11 < 5; a11++)//遍历箱子格子
                                                                        {
                                                                            if (a11 < 4)//煤矿的分布概率
                                                                            {
                                                                                if (random.Bool(0.1f))
                                                                                {
                                                                                    Item_num = 3;

                                                                                }
                                                                                else
                                                                                {
                                                                                    Item_num = 1;
                                                                                }
                                                                                chest_componentchest.AddSlotItems(a11, blocks1[1], Item_num);
                                                                            }
                                                                            else if (a11 == 4)
                                                                            {
                                                                                if (random.Bool(0.05f))
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[0], 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[1], 2);
                                                                                }
                                                                            }

                                                                        }
                                                                    }




                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }

                                                    tg_num++;

                                                }
                                            }

                                        }
                                        else//如果是辅路
                                        {
                                            if (i - 1 == 0)//如果是副区块第一个，则生成养殖间。
                                            {
                                                try
                                                {
                                                    string blocks = ContentManager.Get<string>("House/PetHouse");
                                                    blocks = blocks.Replace("\n", "#");
                                                    string[] blockArray = blocks.Split(new char[1] { '#' });
                                                    foreach (string blockLine in blockArray)
                                                    {
                                                        string[] block = blockLine.Split(new char[1] { ',' });
                                                        if (block.Length > 3)
                                                        {
                                                            int x = int.Parse(block[0]);
                                                            int y = int.Parse(block[1]);
                                                            int z = int.Parse(block[2]);
                                                            int block_house = int.Parse(block[3]);
                                                            int x1t = x + x_T;
                                                            int y1t = y + max1 - 3;
                                                            int z1t = z_T + z + 2;
                                                            m_subsystemTerrain.ChangeCell(x1t, y1t, z1t, block_house);
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    Log.Error("Generation failed.");

                                                }

                                                Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                                if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                                {
                                                    Dic_Chunk_Village3.Add(pointRD1, 0);
                                                }
                                                listRD[i].TG = true;

                                                tg_num++;
                                            }
                                            else
                                            {
                                                if (random.Bool(0.05f))
                                                {
                                                    try
                                                    {
                                                        string blocks = ContentManager.Get<string>("House/IronHouse");//0.1的概率再生成铁匠铺。
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z;
                                                                int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                                try
                                                                {
                                                                    if (Block_reset == 64 || Block_reset == 45)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation5 failed.");
                                                                    break;
                                                                }


                                                                if (Terrain.ExtractContents(block_house) == 64)//如果是熔炉
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }
                                                                if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                        ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                        int Item_num;
                                                                        //126钻石块，46铁块，71孔雀石，47铜块，231锗块,228燃烧炸弹，201炸弹，236燃烧炸药，107普通炸药,150煤块，167磁铁，248经验，132南瓜灯
                                                                        //196羽毛，198线，206布 ，40铁锭，22煤

                                                                        int[] blocks1 = { 40, 22, };
                                                                        for (int a11 = 0; a11 < 5; a11++)//遍历箱子格子
                                                                        {
                                                                            if (a11 < 4)//煤矿的分布概率
                                                                            {
                                                                                if (random.Bool(0.1f))
                                                                                {
                                                                                    Item_num = 3;

                                                                                }
                                                                                else
                                                                                {
                                                                                    Item_num = 1;
                                                                                }
                                                                                chest_componentchest.AddSlotItems(a11, blocks1[1], Item_num);
                                                                            }
                                                                            else if (a11 == 4)
                                                                            {
                                                                                if (random.Bool(0.05f))
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[0], 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    chest_componentchest.AddSlotItems(4, blocks1[1], 2);
                                                                                }
                                                                            }

                                                                        }
                                                                    }




                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }

                                                    Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                                    if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                                    {
                                                        Dic_Chunk_Village3.Add(pointRD1, 0);
                                                    }
                                                    listRD[i].TG = true;

                                                    tg_num++;
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        string blocks = ContentManager.Get<string>("House/LivingHouse1");
                                                        blocks = blocks.Replace("\n", "#");
                                                        string[] blockArray = blocks.Split(new char[1] { '#' });
                                                        foreach (string blockLine in blockArray)
                                                        {
                                                            string[] block = blockLine.Split(new char[1] { ',' });
                                                            if (block.Length > 3)
                                                            {
                                                                int x = int.Parse(block[0]);
                                                                int y = int.Parse(block[1]);
                                                                int z = int.Parse(block[2]);
                                                                int block_house = int.Parse(block[3]);
                                                                int x1t = x + x_T;
                                                                int y1t = y + max1 - 3;
                                                                int z1t = z_T + z + 2;
                                                                int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                                try
                                                                {
                                                                    if (Block_reset == 64 || Block_reset == 45 || Block_reset == 27)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Log.Error("Generation6 failed.");
                                                                    break;
                                                                }



                                                                if (Terrain.ExtractContents(block_house) == 27)//如果是工作台
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }
                                                                if (Terrain.ExtractContents(block_house) == 64)//如果是熔炉
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                    }

                                                                }
                                                                if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                                {
                                                                    ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                                    if (result == null)
                                                                    {
                                                                        DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                        ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                        valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                        valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                        Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                        m_subsystemTerrain.Project.AddEntity(entity);
                                                                        ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                                                                                                     //167磁铁，132南瓜灯
                                                                                                                                                     //196羽毛，198线，206布 ，40铁锭，22煤，207毛皮,159 皮革
                                                                        int diamond_num;//小麦数量
                                                                        int[] blocks1 = { 167, 132, 196, 198, 206, 207, 22, 114862, 159 };
                                                                        for (int a11 = 0; a11 < 16; a11++)//遍历箱子格子
                                                                        {
                                                                            if (random.Bool(0.1f))//随机钻石数量
                                                                            {
                                                                                diamond_num = 2;
                                                                            }
                                                                            else
                                                                            {
                                                                                diamond_num = 1;
                                                                            }
                                                                            if (random.Bool(0.4f))
                                                                            {
                                                                                chest_componentchest.AddSlotItems(a11, blocks1[a11 % 9], diamond_num);
                                                                            }

                                                                        }
                                                                    }


                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation failed.");

                                                    }

                                                    Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                                    if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                                    {
                                                        Dic_Chunk_Village3.Add(pointRD1, 0);
                                                    }
                                                    listRD[i].TG = true;

                                                    tg_num++;
                                                }
                                            }
                                        }





                                        m_subsystemTerrain.ChangeCell(x_T, 3, z_T, 0);
                                        /*Time.QueueTimeDelayedExecution(Time.FrameStartTime + 1.0, delegate
                                        {
                                            m_subsystemTerrain.TerrainUpdater.DowngradeAllChunksState(TerrainChunkState.InvalidLight, forceGeometryRegeneration: false);

                                        });*/

                                    }

                                }
                                if (listRD[i].TG == false && listRD[i].BrushType == 2)//转折点
                                {
                                    List<int> height_T = new List<int>(); // 创建一个空的 List<int>,储存区块高度
                                    int x_T = chunk_v.Origin.X;//获取该路径点区块的原始坐标。
                                    int z_T = chunk_v.Origin.Y;
                                    int x_T_coords = chunk_v.Coords.X;
                                    int z_T_coords = chunk_v.Coords.Y;
                                    Point2 point_t1 = (x_T_coords, z_T_coords);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

                                    bool ischunkload = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_t1);
                                    bool ischunkload2 = Dic_Chunk_Village3.ContainsKey(point_t1);
                                    if (ischunkload == false && ischunkload2 == false)
                                    {
                                        for (int a1 = 0; a1 < 16; a1++)//遍历路径点区块这里是先摧毁区块的多余物质。
                                        {
                                            for (int j1 = 0; j1 < 16; j1++)
                                            {

                                                int num1 = a1 + x_T;//x坐标

                                                int num2 = j1 + z_T;//z坐标
                                                                    //int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度
                                                int k = 120;//从86的高度开始遍历

                                                while (k > 64)// 当k高度大于65的时候。 
                                                {

                                                    int num3 = terrain.GetCellContentsFast(num1, k, num2);//获取起始的高度的方块
                                                    if (num3 != 8 || num3 != 3 || num3 != 7 || num3 != 4 || num3 != 2 || num3 != 985 || num3 != 984 || num3 != 18 || num3 != 6 || num3 != 66)//如果不是草地、沙子、砂岩、花岗岩，水，则销毁
                                                    {

                                                        terrain.SetCellValueFast(num1, k + 1, num2, 0);

                                                    }
                                                    if (num3 == 8 || num3 == 3 || num3 == 7 || num3 == 4 || num3 == 2 || num3 == 985 || num3 == 18 || num3 == 984 || num3 == 6 || num3 == 66)
                                                    {


                                                        break;
                                                    }

                                                    k--;

                                                }
                                                int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度块
                                                height_T.Add(num7);//统计区块内的所有地表高度


                                            }

                                        }
                                        int list_count = height_T.Count;
                                        Log.Information(String.Format("列表元素个数为：{0}", list_count));
                                        int max1 = height_T[0];
                                        int min1 = height_T[0];

                                        for (int i1 = 1; i1 < height_T.Count; i1++)//计算高度差
                                        {
                                            if (height_T[i1] > max1)
                                            {
                                                max1 = height_T[i1];
                                            }
                                            if (height_T[i1] < min1)
                                            {
                                                min1 = height_T[i1];
                                            }
                                        }

                                        int height_T1 = (max1 - min1);//高度差


                                        /*if (height_T1 > 7)// 如果高度大于13，过于崎岖，不生成建筑。
                                        {
                                            Point2 pointRD2 = (chunk_v.Coords.X, chunk_v.Coords.Y);//把当前区块记录下来，作为村庄区块。
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD2) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD2, 0);
                                            }
                                            listRD[i].TG = true;
                                            tg_num++;
                                            continue;
                                        }*/
                                        for (int a1 = 14; a1 < 16; a1++)//遍历路径点区块的路径z轴
                                        {
                                            for (int j1 = 0; j1 < 2; j1++)//x轴
                                            {
                                                int top_height = height_T[a1 + j1 * 16];
                                                terrain.SetCellValueFast(x_T + j1, top_height, z_T + a1, 5);
                                            }
                                        }
                                        Log.Information(string.Format("村庄转折点生成完毕：{0}, {1}, {2}", listRD[i].Position.X, listRD[i].Position.Y, listRD[i].Position.Z));
                                        Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);//把当前区块记录下来，作为村庄区块。
                                        if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                        {
                                            Dic_Chunk_Village3.Add(pointRD1, 0);
                                        }
                                        listRD[i].TG = true;

                                        try
                                        {
                                            string blocks = ContentManager.Get<string>("House/jiaotang2");
                                            blocks = blocks.Replace("\n", "#");
                                            string[] blockArray = blocks.Split(new char[1] { '#' });
                                            foreach (string blockLine in blockArray)
                                            {
                                                string[] block = blockLine.Split(new char[1] { ',' });
                                                if (block.Length > 3)
                                                {
                                                    int x = int.Parse(block[0]);
                                                    int y = int.Parse(block[1]);
                                                    int z = int.Parse(block[2]);
                                                    int block_house = int.Parse(block[3]);
                                                    int x1t = x + x_T;
                                                    int y1t = y + max1 - 3;
                                                    int z1t = z_T + z;
                                                    int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                    try
                                                    {
                                                        if (Block_reset == 45)
                                                        {
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Log.Error("Generation7 failed.");
                                                        break;
                                                    }


                                                    if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                    {
                                                        ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                        if (result == null)
                                                        {
                                                            DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                            ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                            valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                            valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                            Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                            m_subsystemTerrain.Project.AddEntity(entity);
                                                            ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                            int Item_num;
                                                            //126钻石块，46铁块，71孔雀石，47铜块，231锗块,228燃烧炸弹，201炸弹，236燃烧炸药，107普通炸药,150煤块，167磁铁，248经验，132南瓜灯
                                                            //196羽毛，198线，206布
                                                            if (block_house == 49197)//如果是最高的箱子，则进行特殊奖励生成。
                                                            {
                                                                int[] blocks1 = { 126, 46, 71, 47, 231, 228, 201, 236, 107, 150, 167, 248, 132, 196, 198, 206 };
                                                                for (int a11 = 0; a11 < 16; a11++)//遍历箱子格子
                                                                {
                                                                    if (blocks1[a11] == 248)
                                                                    {

                                                                        chest_componentchest.AddSlotItems(a11, blocks1[a11], 40);
                                                                    }
                                                                    else
                                                                    {
                                                                        chest_componentchest.AddSlotItems(a11, blocks1[a11], 1);
                                                                    }

                                                                }

                                                            }
                                                            else
                                                            {
                                                                int[] blocks1 = { 248, 196, 198, 206 };
                                                                for (int a11 = 0; a11 < 4; a11++)//遍历箱子格子
                                                                {
                                                                    if (random.Bool(0.01f))//随机数量
                                                                    {
                                                                        Item_num = 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        Item_num = 1;
                                                                    }
                                                                    if (random.Bool(0.67f))
                                                                    {
                                                                        chest_componentchest.AddSlotItems(a11, blocks1[a11], Item_num);
                                                                    }

                                                                }
                                                            }
                                                        }


                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            Log.Error("Generation failed.");

                                        }

                                        tg_num++;
                                        m_subsystemTerrain.ChangeCell(x_T, 3, z_T, 0);
                                    }

                                }
                                if (listRD[i].TG == false && listRD[i].BrushType == 1)//z轴小路
                                {
                                    List<int> height_T = new List<int>(); // 创建一个空的 List<int>,储存区块高度
                                    int x_T = chunk_v.Origin.X;//获取该路径点区块的原始坐标。
                                    int z_T = chunk_v.Origin.Y;
                                    int x_T_coords = chunk_v.Coords.X;
                                    int z_T_coords = chunk_v.Coords.Y;
                                    Point2 point_t1 = (x_T_coords, z_T_coords);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

                                    bool ischunkload = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_t1);
                                    bool ischunkload2 = Dic_Chunk_Village3.ContainsKey(point_t1);
                                    if (ischunkload == false && ischunkload2 == false)
                                    {
                                        for (int a1 = 0; a1 < 16; a1++)//遍历路径点区块这里是先摧毁区块的多余物质。
                                        {
                                            for (int j1 = 0; j1 < 16; j1++)
                                            {

                                                int num1 = a1 + x_T;//x坐标

                                                int num2 = j1 + z_T;//z坐标
                                                                    //int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度
                                                int k = 120;//从86的高度开始遍历

                                                while (k > 64)// 当k高度大于65的时候。 
                                                {

                                                    int num3 = terrain.GetCellContentsFast(num1, k, num2);//获取起始的高度的方块
                                                    if (num3 != 8 || num3 != 3 || num3 != 7 || num3 != 4 || num3 != 2 || num3 != 985 || num3 != 984 || num3 != 18 || num3 != 6 || num3 != 66)//如果不是草地、沙子、砂岩、花岗岩，水，则销毁
                                                    {

                                                        terrain.SetCellValueFast(num1, k + 1, num2, 0);

                                                    }
                                                    if (num3 == 8 || num3 == 3 || num3 == 7 || num3 == 4 || num3 == 2 || num3 == 985 || num3 == 18 || num3 == 984 || num3 == 6 || num3 == 66)
                                                    {


                                                        break;
                                                    }

                                                    k--;

                                                }
                                                int num7 = terrain.CalculateTopmostCellHeight(num1, num2);//获取预生成位置的最大高度块
                                                height_T.Add(num7);//统计区块内的所有地表高度


                                            }

                                        }
                                        int list_count = height_T.Count;
                                        Log.Information(String.Format("列表元素个数为：{0}", list_count));
                                        int max1 = height_T[0];
                                        int min1 = height_T[0];

                                        for (int i1 = 1; i1 < height_T.Count; i1++)//计算高度差
                                        {
                                            if (height_T[i1] > max1)
                                            {
                                                max1 = height_T[i1];
                                            }
                                            if (height_T[i1] < min1)
                                            {
                                                min1 = height_T[i1];
                                            }
                                        }

                                        int height_T1 = (max1 - min1);//高度差


                                        /*if (height_T1 > 7)// 如果高度大于13，过于崎岖，不生成建筑。
                                        {
                                            Point2 pointRD2 = (chunk_v.Coords.X, chunk_v.Coords.Y);//把当前区块记录下来，作为村庄区块。
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD2) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD2, 0);
                                            }
                                            listRD[i].TG = true;
                                            tg_num++;
                                            continue;
                                        }*/
                                        if (listRD[i].is_Vice == false)//如果不是副路
                                        {
                                            for (int a1 = 0; a1 < 2; a1++)//遍历路径点区块的路径x轴
                                            {
                                                for (int j1 = 0; j1 < 16; j1++)//z轴
                                                {
                                                    int top_height = height_T[j1 + a1 * 16];
                                                    terrain.SetCellValueFast(x_T + a1, top_height, z_T + j1, 5);
                                                }
                                            }
                                            Log.Information(string.Format("村庄z轴小路生成完毕：{0}, {1}, {2}", listRD[i].Position.X, listRD[i].Position.Y, listRD[i].Position.Z));
                                            Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD1, 0);
                                            }
                                            listRD[i].TG = true;

                                        }


                                        if (listRD[i].is_Vice == false)//如果不是副路
                                        {
                                            try
                                            {
                                                string blocks = ContentManager.Get<string>("House/field");
                                                blocks = blocks.Replace("\n", "#");
                                                string[] blockArray = blocks.Split(new char[1] { '#' });
                                                foreach (string blockLine in blockArray)
                                                {
                                                    string[] block = blockLine.Split(new char[1] { ',' });
                                                    if (block.Length > 3)
                                                    {
                                                        int x = int.Parse(block[0]);
                                                        int y = int.Parse(block[1]);
                                                        int z = int.Parse(block[2]);
                                                        int block_house = int.Parse(block[3]);
                                                        int x1t = x + x_T + 2;
                                                        int y1t = y + max1 - 3;
                                                        int z1t = z_T + z;
                                                        int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                        try
                                                        {
                                                            if (Block_reset == 45)
                                                            {
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Log.Error("Generation8 failed.");
                                                            break;
                                                        }


                                                        if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                        {
                                                            ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                            if (result == null)
                                                            {
                                                                DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                m_subsystemTerrain.Project.AddEntity(entity);
                                                                ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                int diamond_num;//小麦数量
                                                                for (int a11 = 0; a11 < 16; a11++)//遍历箱子格子
                                                                {
                                                                    if (random.Bool(0.1f))//
                                                                    {
                                                                        diamond_num = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        diamond_num = 1;
                                                                    }
                                                                    chest_componentchest.AddSlotItems(a11, 114862, diamond_num);
                                                                }
                                                            }


                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                Log.Error("Generation failed.");

                                            }

                                            Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);

                                            listRD[i].TG = true;
                                            tg_num++;
                                        }
                                        else if (listRD[i].is_Vice == true && i == 7)
                                        {

                                            try
                                            {
                                                string blocks = ContentManager.Get<string>("House/shuijing");
                                                blocks = blocks.Replace("\n", "#");
                                                string[] blockArray = blocks.Split(new char[1] { '#' });
                                                foreach (string blockLine in blockArray)
                                                {
                                                    string[] block = blockLine.Split(new char[1] { ',' });
                                                    if (block.Length > 3)
                                                    {
                                                        int x = int.Parse(block[0]);
                                                        int y = int.Parse(block[1]);
                                                        int z = int.Parse(block[2]);
                                                        int block_house = int.Parse(block[3]);
                                                        int x1t = x + x_T + 2;
                                                        int y1t = y + max1 - 4;
                                                        int z1t = z_T + z;
                                                        int Block_reset = terrain.GetCellContentsFast(x1t, y1t, z1t);
                                                        try
                                                        {
                                                            if (Block_reset == 45)
                                                            {
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Log.Error("Generation9 failed.");
                                                            break;
                                                        }


                                                        if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
                                                        {
                                                            ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
                                                            if (result == null)
                                                            {
                                                                DatabaseObject databaseObject = m_subsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_subsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
                                                                ValuesDictionary valuesDictionary = new ValuesDictionary();
                                                                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                                                                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
                                                                Entity entity = m_subsystemTerrain.Project.CreateEntity(valuesDictionary);
                                                                m_subsystemTerrain.Project.AddEntity(entity);
                                                                ComponentChest chest_componentchest = entity.FindComponent<ComponentChest>();//创建实体，并获取组件
                                                                int diamond_num;
                                                                if (random.Bool(0.01f))//随机钻石数量
                                                                {
                                                                    diamond_num = 5;
                                                                }
                                                                else
                                                                {
                                                                    diamond_num = 2;
                                                                }
                                                                chest_componentchest.AddSlotItems(0, 111, diamond_num);
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                Log.Error("Generation failed.");

                                            }

                                            Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
                                            if (Dic_Chunk_Village3.ContainsKey(pointRD1) == false)
                                            {
                                                Dic_Chunk_Village3.Add(pointRD1, 0);
                                            }

                                            tg_num++;
                                            listRD[i].TG = true;
                                        }

                                        m_subsystemTerrain.ChangeCell(x_T, 3, z_T, 0);


                                    }

                                }
                                try
                                {
                                    int height_Villager = terrain.CalculateTopmostCellHeight(chunk_v.Origin.X, chunk_v.Origin.Y);
                                    int height_cell = terrain.GetCellContentsFast(chunk_v.Origin.X, height_Villager - 1, chunk_v.Origin.Y);//如果是悬空方块
                                    int true_height = height_Villager;
                                    if (height_cell == 0)
                                    {
                                        true_height = height_Villager - 1;

                                    }
                                    Entity entity1 = DatabaseManager.CreateEntity(m_subsystemTerrain.Project, "FCVillager", true);
                                    entity1.FindComponent<ComponentBody>(true).Position = (chunk_v.Origin.X, true_height, chunk_v.Origin.Y);
                                    entity1.FindComponent<ComponentBody>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, random.Float(0f, 6.2831855f));
                                    entity1.FindComponent<ComponentCreature>(true).ConstantSpawn = false;

                                    m_subsystemTerrain.Project.AddEntity(entity1);
                                }
                                catch
                                {
                                    Log.Error("Generation villager failed.");

                                }

                            }


                        }


                    }


                    Log.Information(string.Format("生成完毕的区块总数：{0}", tg_num));



                    if (tg_num == 8)
                    {

                        if (ischunkloding == true)
                        {
                            for (int i = 0; i < listRD.Count; i++)
                            {

                                RemoveChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);//加载区块
                                

                            }
                            ischunkloding = false;
                        }
                        tg_num = 0;
                        Bg_num = 0;
                        listRD.Clear();
                        listBD.Clear();
                        ischunkloding2 = false;
                    }

                    /*for (int i = 0; i < listRD.Count; i++)
                    {
                        if(listRD[i].TG == true)
                        {

                            RemoveChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);
                            Bg_num++;
                        }
                        if(Bg_num==listRD.Count)
                        {
                            tg_num = 0;
                            Bg_num = 0;
                            listRD.Clear();
                            listBD.Clear();
                        }
                        else
                        {
                            Bg_num = 0;
                        }


                    }*/



                }
            






            /*if (listRD.Count == 5)//如果路径点等于5，说明已经路径点已经完毕，执行生成。
			{
				TGNum tgNum = new TGNum();
				await GenerateVillageAsync(chunk, terrain, tgNum);
				if (tgNum.Value == 5)
				{
					
					listRD.Clear();
					listBD.Clear();
				}




				


			}*/
        }

        #endregion
        #region 城市生成



        #endregion
        
    }



    #endregion
}
