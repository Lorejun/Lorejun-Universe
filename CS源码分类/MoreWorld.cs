using Engine.Graphics;
using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TemplatesDatabase;
using XmlUtilities;

namespace Game
{
    /*
     * 这个文件的代码无特殊需要不用改动，仅供参考
     * 如果有能力可以改规则，比如传送门开启方式，鼓励创新
     * 多维世界功能的价值最大化是融入完整的生存Mod体系，所以允许Mod开发者套用和更改
     * by再回首
     */

    public class SubsystemWorld : Subsystem, IUpdateable
    {

        //子系统所依赖的其他子系统
        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemBodies m_subsystemBodies;

        public ComponentPlayer m_componentPlayer;

        // 当前世界的类型
        public WorldType m_worldType;

        //// 存储所有传送门的字典，以Point3结构作为键
        public Dictionary<Point3, WorldDoor> m_worldDoors = new Dictionary<Point3, WorldDoor>();

        // 存储被传送的动物实体列表
        private List<Entity> m_creatureEntitys = new List<Entity>();

        // 当前世界的路径
        private string m_worldPath;

        // 标识是否可以生成传送门
        private bool m_canGenerateDoor;

        // // 标识系统是否完成初始化
        private bool m_initialize;

        //// 距离上次更新的时间
        private float m_lastDtime = 0f;

        public UpdateOrder UpdateOrder => UpdateOrder.Default;

        //更新方法，每帧执行一次
        public void Update(float dt)
        {
            // // 如果没有玩家组件，则不执行更新
            if (m_componentPlayer == null) return;
            //新世界构建传送门
            //检查是否可以生成新的传送门
            if (m_canGenerateDoor)
            {
                // 累加时间
                m_lastDtime += dt;
                if (m_lastDtime > 1f)
                {
                    //// 生成传送门，并重置生成传送门的标识
                    GenerateDoor(new Point3(m_componentPlayer.ComponentBody.Position) + new Point3(0, 1, 2));
                    m_canGenerateDoor = false;
                }
            }
            //生成被传送的动物
            if (!m_initialize)
            {
                // 如果系统尚未初始化，则执行初始化操作
                m_initialize = true;
                // 获取玩家的位置
                Point3 p = new Point3(m_componentPlayer.ComponentBody.Position);
                // 遍历所有待传送的动物实体
                foreach (Entity creatureEntity in m_creatureEntitys)
                {
                    // 初始化一个空的位置变量
                    Point3 v = new Point3(0, 0, 0);
                    // 尝试在玩家附近生成动物
                    int num = 0;
                    while (num <= 5)
                    {
                        //// 在玩家周围随机选择位置，p是玩家位置
                        int x = p.X + (new Random()).Int(-5, 5);
                        int y = p.Y + num;
                        int z = p.Z + (new Random()).Int(-5, 5);
                        // 检查选定位置是否为空
                        int i = m_subsystemTerrain.Terrain.GetCellContents(x, y, z);
                        if (i == 0)
                        {
                            // 如果为空，则设置动物的位置
                            v = new Point3(x, y, z);
                            break;
                        }
                        else
                        {
                            // 如果不为空，尝试在更高的地方生成
                            num++;
                        }
                    }
                    // 设置动物的位置和旋转，然后将其添加到世界中
                    creatureEntity.FindComponent<ComponentFrame>(true).Position = new Vector3(v.X, v.Y, v.Z);
                    creatureEntity.FindComponent<ComponentFrame>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (new Random()).Float(0f, (float)MathUtils.PI * 2f));
                    creatureEntity.FindComponent<ComponentSpawn>(true).SpawnDuration = 0f;
                    base.Project.AddEntity(creatureEntity);
                }
                // 清空待传送的动物实体列表
                m_creatureEntitys.Clear();
            }
            //捕捉实体位置（是否进入传送门）
            foreach (WorldDoor worldDoor in m_worldDoors.Values)
            {
                Vector3 v1 = worldDoor.MinPoint;
                Vector3 v2 = worldDoor.MaxPoint;
                //玩家拟进入传送门
                Vector3 p = m_componentPlayer.ComponentBody.Position;
                // 玩家拟进入传送门的检测，玩家进入门内，执行传送
                if (p.X >= v1.X && p.Y >= v1.Y && p.Z >= v1.Z && p.X <= v2.X && p.Y <= v2.Y && p.Z <= v2.Z)
                {
                    //如果当前世界等同于传送门的类型（同类传送），则传送回主世界。在沙子世界进沙门
                    if (m_worldType == worldDoor.WorldType)
                    {
                        // 玩家从子世界传送回主世界
                        ChildToMajorWorld();
                    }
                    else
                    {
                        // 获取目标世界的名称
                        string worldName = GetWorldName(worldDoor.WorldType);
                        // 玩家从主世界传送到子世界
                        if (m_worldType == WorldType.Default)
                        {
                            //如果当前世界是主世界，则传送到目标子世界
                            MajorToChildWorld(worldName);
                        }
                        else
                        {
                            //如果当前世界不是主世界，则是子世界直接相互传送
                            // 玩家从子世界传送到另一个子世界
                            ChildToChildWorld(worldName);
                        }
                    }
                }
                //动物拟进入传送门
                DynamicArray<ComponentBody> dynamicArray = new DynamicArray<ComponentBody>();
                m_subsystemBodies.FindBodiesInArea(v1.XZ - new Vector2(8f), v2.XY + new Vector2(8f), dynamicArray);
                foreach (ComponentBody creatureBody in dynamicArray)
                {
                    // 动物进入传送门的检测
                    Vector3 p2 = creatureBody.Position;
                    if (p2.X >= v1.X && p2.Y >= v1.Y && p2.Z >= v1.Z && p2.X <= v2.X && p2.Y <= v2.Y && p2.Z <= v2.Z)
                    {
                        ComponentPlayer player = creatureBody.Entity.FindComponent<ComponentPlayer>();
                        //  确保该实体不是玩家
                        if (player == null)
                        {
                            // 获取动物的名称
                            string TransmittedAnimalName = creatureBody.Entity.ValuesDictionary.DatabaseObject.Name;
                            // 从世界中移除动物实体
                            base.Project.RemoveEntity(creatureBody.Entity, true);
                            // 判断动物该如何传送
                            if (m_worldType == worldDoor.WorldType)
                            {
                                ChildToMajorWorld(IsAnimal: true, TransmittedAnimalName);
                            }
                            else
                            {
                                string worldName = GetWorldName(worldDoor.WorldType);
                                if (m_worldType == WorldType.Default)
                                {
                                    MajorToChildWorld(worldName, IsAnimal: true, TransmittedAnimalName);
                                }
                                else
                                {
                                    ChildToChildWorld(worldName, IsAnimal: true, TransmittedAnimalName);
                                }
                            }
                        }
                    }
                }
            }
        }


        //装载方法，进入存档时执行一次
        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemBodies = base.Project.FindSubsystem<SubsystemBodies>(true);
            GameLoadingScreen gameLoadingScreen = ScreensManager.FindScreen<GameLoadingScreen>("GameLoading");
            m_worldPath = gameLoadingScreen.m_worldInfo.DirectoryName;
            m_worldType = GetWorldType(m_worldPath);
            valuesDictionary.SetValue<string>("WorldPath", m_worldPath);
            m_canGenerateDoor = false;
            m_initialize = false;
            //获取从其他世界传送来的动物集合
            if (valuesDictionary.ContainsKey("Creatures"))
            {
                string Creatures = valuesDictionary.GetValue<string>("Creatures");
                if (Creatures != "Null")
                {
                    string[] CreatureArray = Creatures.Split(new char[1] { ',' });
                    foreach (string creatureName in CreatureArray)
                    {
                        Entity entity = DatabaseManager.CreateEntity(base.Project, creatureName, true);
                        m_creatureEntitys.Add(entity);
                    }
                    valuesDictionary.SetValue<string>("Creatures", "Null");
                }
            }
            base.Load(valuesDictionary);
        }

        //保存方法，退出存档时执行一次，且理论每120s执行一次
        public override void Save(ValuesDictionary valuesDictionary)
        {
            //保存当前世界的存档路径
            valuesDictionary.SetValue<string>("WorldPath", m_worldPath);
            base.Save(valuesDictionary);
        }

        //实体添加方法，每当实体被添加时执行一次
        public override void OnEntityAdded(Entity entity)
        {
            //此方法在一个实体被添加到游戏世界中时调用。

            //它检查添加的实体是否包含玩家组件；如果是，它会更新 m_componentPlayer 引用以指向当前玩家的组件。

            //如果玩家数据中的生成位置是原点（表示这是首次生成玩家），它会寻找一个合适的生成位置，并将玩家的数据更新为该位置，
            //接着将 m_canGenerateDoor 标志设置为 true，表示接下来可以生成新的传送门。

            //最后，它调用基类的 OnEntityAdded 方法完成实体添加的其余处理。
            ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
            {
                m_componentPlayer = componentPlayer;
                if (m_componentPlayer.PlayerData.SpawnPosition == new Vector3(0, 0, 0))
                {
                    Vector3 coarsePosition = m_subsystemTerrain.TerrainContentsGenerator.FindCoarseSpawnPosition();
                    m_componentPlayer.PlayerData.SpawnPosition = coarsePosition;
                    m_componentPlayer.ComponentBody.Position = coarsePosition;
                    m_canGenerateDoor = true;
                }
            }
            base.OnEntityAdded(entity);
        }

        //实体移除方法，每当实体被移除时执行一次
        public override void OnEntityRemoved(Entity entity)
        {
            base.OnEntityRemoved(entity);
        }

        //释放方法，关闭存档时执行一次
        public override void Dispose()
        {
            base.Dispose();
        }

        //子世界传送到主世界
        public void ChildToMajorWorld(bool IsAnimal = false, string AnimalName = null)
        {
            /*用于将实体从子世界（如一个特定的维度或区域）传送回主世界。

			path 变量获取当前世界的目录名称。

			wpath 变量获取当前世界目录的父目录，通常指向主世界目录。

			检查当前是否在子世界中，如果不是，则显示信息，并结束方法执行。

			如果传送的实体不是动物，则调用 ChangeWorld 方法来实现玩家的世界切换。

			如果传送的实体是动物，则调用 SaveCreatures 方法来保存动物信息，以便在主世界中重新生成。*/
            string path = GameManager.WorldInfo.DirectoryName;
            string wpath = Storage.GetDirectoryName(path);
            if (!IsChildWorld(path))
            {
                m_componentPlayer?.ComponentGui.DisplaySmallMessage("提示：当前世界不为子世界", Color.Yellow, false, false);
                return;
            }
            if (!IsAnimal)
            {
                ChangeWorld(path, wpath);
            }
            else
            {
                SaveCreatures(wpath, AnimalName);
            }
        }

        //主世界传送到子世界
        public void MajorToChildWorld(string worldName, bool IsAnimal = false, string AnimalName = null)
        {
            /*用于将实体从主世界传送到子世界。

			path 变量获取当前世界的目录名称。

			wpath 变量通过组合当前目录路径和子世界名称来获取子世界的完整路径。

			首先确认当前世界确实是主世界，然后确认目标世界是子世界，如果不是，则显示相应信息，并结束方法执行。

			接下来的逻辑与 ChildToMajorWorld 类似，针对非动物实体调用 ChangeWorld 方法，对于动物实体调用 SaveCreatures 方法。*/
            string path = GameManager.WorldInfo.DirectoryName;
            string wpath = Storage.CombinePaths(path, worldName);
            if (IsChildWorld(path))
            {
                m_componentPlayer?.ComponentGui.DisplaySmallMessage("提示：当前世界不为主世界", Color.Yellow, false, false);
                return;
            }
            if (!IsChildWorld(wpath))
            {
                m_componentPlayer?.ComponentGui.DisplaySmallMessage("提示：传送世界不为子世界", Color.Yellow, false, false);
                return;
            }
            if (!IsAnimal)
            {
                ChangeWorld(path, wpath);
            }
            else
            {
                SaveCreatures(wpath, AnimalName);
            }
        }

        //子世界传送到子世界
        public void ChildToChildWorld(string worldName, bool IsAnimal = false, string AnimalName = null)
        {
            /*用于将实体从一个子世界传送到另一个子世界。

			path 变量获取当前世界的目录名称。

			wpath 变量通过组合当前世界的父目录和目标子世界名称来获取目标子世界的完整路径。

			与前两个方法类似，这个方法也会首先验证当前和目标世界是否都是子世界，如果不是，显示信息并结束方法执行。

			传送逻辑与前两个方法相同，决定是更改玩家的世界还是保存动物信息。*/
            string path = GameManager.WorldInfo.DirectoryName;
            string wpath = Storage.CombinePaths(Storage.GetDirectoryName(path), worldName);
            if (!IsChildWorld(path))
            {
                m_componentPlayer?.ComponentGui.DisplaySmallMessage("提示：当前世界不为子世界", Color.Yellow, false, false);
                return;
            }
            if (!IsChildWorld(wpath))
            {
                m_componentPlayer?.ComponentGui.DisplaySmallMessage("提示：传送世界不为子世界", Color.Yellow, false, false);
                return;
            }
            if (!IsAnimal)
            {
                ChangeWorld(path, wpath);
            }
            else
            {
                SaveCreatures(wpath, AnimalName);
            }
        }

        //切换世界
        public void ChangeWorld(string path, string wpath, bool IsAnimal = false)
        {

            bool isNewSubWorld = false;
            bool isExtendWorld = false;
            // 判断目标世界是否已存在，如果不存在则创建新的子世界目录。
            if (!Storage.DirectoryExists(wpath))
            {
                Storage.CreateDirectory(wpath);
                //读取外部存档
                Dictionary<string, Stream> fileEntries = GetScworldList(ModsManager.ModsPath);
                string worldName = Storage.GetFileNameWithoutExtension(wpath);
                // 如果外部存档文件中包含目标世界，则将其解压至目标路径。
                if (fileEntries.ContainsKey(worldName))
                {
                    WorldsManager.UnpackWorld(wpath, fileEntries[worldName], importEmbeddedExternalContent: true);
                    isExtendWorld = true;// 标记这是一个扩展世界，而不是新创建的。
                }
                // 如果没有找到外部存档，表示这是一个新的子世界，进行初始化设置。
                if (!isExtendWorld)
                {
                    //创建新的子世界存档
                    isNewSubWorld = true;
                    // 使用当前世界的设置作为新子世界的基础设置。
                    WorldSettings worldSettings = GameManager.WorldInfo.WorldSettings;
                    int num;
                    // 确定世界种子。
                    if (string.IsNullOrEmpty(worldSettings.Seed))
                    {
                        num = (int)(long)(Time.RealTime * 1000.0);
                    }
                    else if (worldSettings.Seed == "0")
                    {
                        num = 0;
                    }
                    else
                    {
                        num = 0;
                        int num2 = 1;
                        string seed = worldSettings.Seed;
                        foreach (char c in seed)
                        {
                            num += c * num2;
                            num2 += 29;
                        }
                    }
                    // 创建新的ValuesDictionary实例并设置世界参数。
                    ValuesDictionary valuesDictionary = new ValuesDictionary();
                    worldSettings.Save(valuesDictionary, liveModifiableParametersOnly: false);
                    valuesDictionary.SetValue("WorldDirectoryName", wpath);
                    valuesDictionary.SetValue("WorldSeed", num);
                    ValuesDictionary valuesDictionary2 = new ValuesDictionary();
                    valuesDictionary2.SetValue("Players", new ValuesDictionary());
                    DatabaseObject databaseObject = DatabaseManager.GameDatabase.Database.FindDatabaseObject("GameProject", DatabaseManager.GameDatabase.ProjectTemplateType, throwIfNotFound: true);
                    XElement xElement = new XElement("Project");
                    XmlUtils.SetAttributeValue(xElement, "Guid", databaseObject.Guid);
                    XmlUtils.SetAttributeValue(xElement, "Name", "GameProject");
                    XmlUtils.SetAttributeValue(xElement, "Version", VersionsManager.SerializationVersion);
                    XElement xElement2 = new XElement("Subsystems");
                    xElement.Add(xElement2);
                    XElement xElement3 = new XElement("Values");
                    XmlUtils.SetAttributeValue(xElement3, "Name", "GameInfo");
                    valuesDictionary.Save(xElement3);
                    xElement2.Add(xElement3);
                    XElement xElement4 = new XElement("Values");
                    XmlUtils.SetAttributeValue(xElement4, "Name", "Players");
                    valuesDictionary2.Save(xElement4);
                    xElement2.Add(xElement4);
                    XElement xElement5 = new XElement("Values");
                    XmlUtils.SetAttributeValue(xElement5, "Name", "PlayerStats");
                    valuesDictionary2.Save(xElement5);
                    xElement2.Add(xElement5);
                    // 将项目XML文档写入文件。
                    using (Stream stream = Storage.OpenFile(Storage.CombinePaths(wpath, "Project.xml"), OpenFileMode.Create))
                    {
                        XmlUtils.SaveXmlToStream(xElement, stream, null, throwOnError: true);
                    }
                }
            }
            //获取子世界的Info对象，关闭当前存档
            WorldInfo subworldInfo = WorldsManager.GetWorldInfo(wpath);
            GameManager.SaveProject(true, true);
            GameManager.DisposeProject();
            try
            {
                //获取当前存档与目标存档的XElement对象
                XElement rxElement = null;
                XElement subXElement = null;
                using (Stream stream = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Read))
                {
                    rxElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
                }
                using (Stream stream = Storage.OpenFile(Storage.CombinePaths(wpath, "Project.xml"), OpenFileMode.Read))
                {
                    subXElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
                }
                //同步玩家基本数据
                XElement statsElement = null;
                foreach (XElement element in rxElement.Element("Subsystems").Elements())
                {
                    if (XmlUtils.GetAttributeValue<string>(element, "Name") == "PlayerStats")
                    {
                        statsElement = element;
                        break;
                    }
                }
                foreach (XElement element in subXElement.Element("Subsystems").Elements())
                {
                    if (XmlUtils.GetAttributeValue<string>(element, "Name") == "PlayerStats")
                    {
                        ReplaceNodes(statsElement, element, null);
                        break;
                    }
                }
                if (isNewSubWorld)
                {
                    //同步玩家信息
                    XElement playersElement = rxElement.Element("Subsystems").Elements().ElementAt(1);
                    ReplaceNodes(playersElement, subXElement.Element("Subsystems").Elements().ElementAt(1), null);
                    XElement subPlayerElement = subXElement.Element("Subsystems").Elements().ElementAt(1).Elements().ElementAt(2);
                    foreach (XElement element in subPlayerElement.Element("Values").Elements())
                    {
                        string elementName = XmlUtils.GetAttributeValue<string>(element, "Name");
                        if (elementName == "SpawnPosition")
                        {
                            XmlUtils.SetAttributeValue(element, "Value", new Point3(0, 0, 0));
                            break;
                        }
                    }
                    //同步玩家实体
                    XElement playerEntityElement = rxElement.Element("Entities").Element("Entity");
                    subXElement.Add(new XElement("Entities"));
                    subXElement.Element("Entities").Add(playerEntityElement);
                }
                else
                {
                    //同步玩家信息
                    XElement playerElement = null;
                    foreach (XElement element in rxElement.Element("Subsystems").Elements().ElementAt(1).Elements())
                    {
                        if (XmlUtils.GetAttributeValue<string>(element, "Name") == "Players")
                        {
                            playerElement = element;
                            break;
                        }
                    }
                    foreach (XElement element in subXElement.Element("Subsystems").Elements().ElementAt(1).Elements())
                    {
                        if (XmlUtils.GetAttributeValue<string>(element, "Name") == "Players")
                        {
                            string[] parameters = { "SpawnPosition" };
                            ReplaceNodes(playerElement.Element("Values"), element.Element("Values"), parameters);
                            break;
                        }
                    }
                    //同步玩家实体
                    XElement playerEntityElement = rxElement.Element("Entities").Element("Entity");
                    string[] rparameters = { "Body" };
                    ReplaceNodes(playerEntityElement, subXElement.Element("Entities").Element("Entity"), rparameters);
                }
                //保存同步后的目标存档
                using (Stream stream2 = Storage.OpenFile(Storage.CombinePaths(wpath, "Project.xml"), OpenFileMode.Create))
                {
                    XmlUtils.SaveXmlToStream(subXElement, stream2, null, throwOnError: true);
                }
                //同步存档基本数据
                SynchronizeGameInfo(rxElement, subXElement, path, wpath, isNewSubWorld);
                //保存目标存档路径
                m_worldPath = wpath;
                if (IsChildWorld(path))
                {
                    path = Storage.GetDirectoryName(path);
                }
                SavePathToMajorWorld(path, m_worldPath);
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                //加载目标存档
                ScreensManager.SwitchScreen("GameLoading", subworldInfo, null);
            }
        }

        //创建完整传送门
        public void GenerateDoor(Point3 position)
        {
            int cid = GetWorldDoorBlock(m_worldType);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    int id = 0;
                    if (x == 0 || x == 3) id = cid;
                    if ((x == 1 || x == 2) && (y == 0 || y == 4)) id = cid;
                    m_subsystemTerrain.ChangeCell(position.X + x, position.Y + y, position.Z, id);
                }
            }
            SubsystemTransferBlockBehavior transferBlockBehavior = base.Project.FindSubsystem<SubsystemTransferBlockBehavior>(true);
            transferBlockBehavior.CreateEntrance(position + new Point3(1, 0, 0), position + new Point3(2, 0, 0), true, m_worldType);
        }

        //判断是否为子世界
        public static bool IsChildWorld(string cpath)
        {
            string path = Storage.GetDirectoryName(cpath);
            return (path != WorldsManager.WorldsDirectoryName && Storage.GetDirectoryName(path) == WorldsManager.WorldsDirectoryName);
        }

        //获取外部存档
        public static Dictionary<string, Stream> GetScworldList(string path)
        {
            Dictionary<string, Stream> fileEntrys = new Dictionary<string, Stream>();
            foreach (string fname in Storage.ListFileNames(path))
            {
                string extension = Storage.GetExtension(fname);
                string pathName = Storage.CombinePaths(path, fname);
                //Stream stream = Storage.OpenFile(pathName, OpenFileMode.Read);
                Stream stream = ModsManageContentScreen.GetDecipherStream(Storage.OpenFile(pathName, OpenFileMode.Read));

                try
                {
                    if (extension == ".scmod")
                    {
                        ZipArchive zipArchive = ZipArchive.Open(stream, true);
                        foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.ReadCentralDir())
                        {
                            if (Storage.GetExtension(zipArchiveEntry.FilenameInZip) == ".scworld")
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                zipArchive.ExtractFile(zipArchiveEntry, memoryStream);
                                memoryStream.Position = 0L;
                                string filename = Storage.GetFileNameWithoutExtension(zipArchiveEntry.FilenameInZip);
                                fileEntrys.Add(filename, memoryStream);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return fileEntrys;
        }

        //替换XElement对象的子元素
        public static void ReplaceNodes(XElement replaceElement, XElement sourceElement, string[] reserveParameters)
        {
            Dictionary<string, XElement> valuePairs = new Dictionary<string, XElement>();
            if (reserveParameters != null)
            {
                foreach (XElement element in sourceElement.Elements())
                {
                    string attributeName = XmlUtils.GetAttributeValue<string>(element, "Name");
                    foreach (string parameter in reserveParameters)
                    {
                        if (attributeName == parameter)
                        {
                            valuePairs.Add(attributeName, element);
                            break;
                        }
                    }
                }
            }
            sourceElement.RemoveNodes();
            if (reserveParameters != null)
            {
                foreach (XElement element in replaceElement.Elements())
                {
                    string attributeName = XmlUtils.GetAttributeValue<string>(element, "Name");
                    if (valuePairs.ContainsKey(attributeName))
                    {
                        sourceElement.Add(valuePairs[attributeName]);
                    }
                    else
                    {
                        sourceElement.Add(element);
                    }
                }
            }
            else
            {
                foreach (XElement element in replaceElement.Elements())
                {
                    sourceElement.Add(element);
                }
            }
        }

        //同步存档GameInfo
        public static void SynchronizeGameInfo(XElement rxElement, XElement subXElement, string path, string wpath, bool newWorld)
        {
            XElement gameInfoElement = null;
            if (newWorld)
            {
                if (IsChildWorld(path)) path = Storage.GetDirectoryName(path);
                using (Stream stream = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Read))
                {
                    rxElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
                }
            }
            foreach (XElement element in rxElement.Element("Subsystems").Elements())
            {
                if (XmlUtils.GetAttributeValue<string>(element, "Name") == "GameInfo")
                {
                    gameInfoElement = element;
                    break;
                }
            }
            foreach (XElement element in subXElement.Element("Subsystems").Elements())
            {
                if (XmlUtils.GetAttributeValue<string>(element, "Name") == "GameInfo")
                {
                    string[] parameters = {
                        "WorldName","OriginalSerializationVersion",
                        "EnvironmentBehaviorMode", "TimeOfDayMode", "AreWeatherEffectsEnabled",
                        "TerrainGenerationMode", "IslandSize", "TerrainLevel",
                        "ShoreRoughness", "TerrainBlockIndex", "TerrainOceanBlockIndex",
                        "TemperatureOffset", "HumidityOffset", "SeaLevelOffset",
                        "BiomeSize", "StartingPositionMode", "BlockTextureName",
                        "Palette", "WorldSeed", "TotalElapsedGameTime"
                    };
                    if (newWorld) parameters = null;
                    ReplaceNodes(gameInfoElement, element, parameters);
                    break;
                }
            }
            using (Stream stream2 = Storage.OpenFile(Storage.CombinePaths(wpath, "Project.xml"), OpenFileMode.Create))
            {
                XmlUtils.SaveXmlToStream(subXElement, stream2, null, throwOnError: true);
            }
        }

        //保存存档路径到主世界存档中
        public static void SavePathToMajorWorld(string path, string wpath)
        {
            XElement xElement = null;
            using (Stream stream = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Read))
            {
                xElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
            }
            foreach (XElement e in xElement.Element("Subsystems").Elements())
            {
                if (XmlUtils.GetAttributeValue<string>(e, "Name") == "SubWorld")
                {
                    XmlUtils.SetAttributeValue(e.Element("Value"), "Value", wpath);
                }
            }
            using (Stream stream2 = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Create))
            {
                XmlUtils.SaveXmlToStream(xElement, stream2, null, throwOnError: true);
            }
        }

        //保存被传送的动物到对应世界
        public static void SaveCreatures(string path, string creatureName)
        {
            XElement xElement = null;
            if (!Storage.DirectoryExists(path)) return;
            using (Stream stream = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Read))
            {
                xElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
            }
            foreach (XElement element in xElement.Element("Subsystems").Elements())
            {
                if (XmlUtils.GetAttributeValue<string>(element, "Name") == "SubWorld")
                {
                    if (element.Elements().Count() == 1)
                    {
                        XElement creatureXElement = new XElement("Value");
                        element.Add(creatureXElement);
                        XmlUtils.SetAttributeValue(element.Elements().ElementAt(1), "Name", "Creatures");
                        XmlUtils.SetAttributeValue(element.Elements().ElementAt(1), "Type", "string");
                        XmlUtils.SetAttributeValue(element.Elements().ElementAt(1), "Value", "Null");
                    }
                    string creatureNames = XmlUtils.GetAttributeValue<string>(element.Elements().ElementAt(1), "Value");
                    if (creatureNames == "Null")
                    {
                        creatureNames = creatureName;
                    }
                    else
                    {
                        creatureNames = creatureNames + "," + creatureName;
                    }
                    XmlUtils.SetAttributeValue(element.Elements().ElementAt(1), "Value", creatureNames);
                    break;
                }
            }
            using (Stream stream2 = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Create))
            {
                XmlUtils.SaveXmlToStream(xElement, stream2, null, throwOnError: true);
            }
        }

        //获取世界类型
        public static WorldType GetWorldType(string pathOrName)
        {
            WorldType worldType = WorldType.Default;
            pathOrName = pathOrName.Replace("\\", "/");
            string[] worldPaths = pathOrName.Split(new char[1] { '/' });
            string worldName = worldPaths[worldPaths.Length - 1];
            foreach (WorldType type in Enum.GetValues(typeof(WorldType)))
            {
                if (type.ToString() == worldName)
                {
                    worldType = type;
                    break;
                }
            }
            return worldType;
        }

        //获取世界名称
        public static string GetWorldName(WorldType worldType)
        {
            string worldName = worldType.ToString();
            //foreach (string[] parameter in WorldParameter.Collections)
            //{
            //    if (worldType.ToString() == parameter[0])
            //    {
            //        worldName = parameter[1];
            //        break;
            //    }
            //}
            return worldName;
        }

        //获取世界对应传送门的方块ID
        public static int GetWorldDoorBlock(WorldType worldType)
        {
            int id = 0;
            foreach (string[] parameter in WorldParameter.Collections)
            {
                if (worldType.ToString() == parameter[0])
                {
                    id = int.Parse(parameter[2]);
                    break;
                }
            }
            return id;
        }

        //获取世界对应传送门贴图颜色
        public static Color GetWorldDoorColor(WorldType worldType)
        {
            Color color = Color.White;
            foreach (string[] parameter in WorldParameter.Collections)
            {
                if (worldType.ToString() == parameter[0])
                {
                    string[] scolor = parameter[3].Split(new char[1] { ',' });
                    int r = int.Parse(scolor[0]);
                    int g = int.Parse(scolor[1]);
                    int b = int.Parse(scolor[2]);
                    int a = int.Parse(scolor[3]);
                    color = new Color(r, g, b, a);
                    break;
                }
            }
            return color;
        }
    }

    public class WorldDoor
    {
        public Vector3 MinPoint; //传送门位置最小点

        public Vector3 MaxPoint; //传送门位置最大点

        public WorldType WorldType; //传送门对应世界类型
    }

    public class Chartlet
    {
        public Vector3 Position;  //贴图位置

        public Color Color;  //贴图颜色

        public Vector3 Right;

        public Vector3 Up;

        public Vector3 Forward;

        public float Size = 1.5f;

        public float FarSize = 1.5f;

        public float FarDistance = 1f;
    }

    public class SubsystemChartlet : Subsystem, IDrawable
    {
        public SubsystemSky m_subsystemSky;

        public Dictionary<Point3, Chartlet[]> m_chartlets = new Dictionary<Point3, Chartlet[]>();

        private PrimitivesRenderer3D PrimitivesRenderer = new PrimitivesRenderer3D();

        private TexturedBatch3D BatchesByType = new TexturedBatch3D();

        public int[] DrawOrders => new int[1] { 110 };

        public void Draw(Camera camera, int drawOrder)
        {
            foreach (Point3 point3 in m_chartlets.Keys)
            {
                Chartlet[] keys = m_chartlets[point3];
                foreach (Chartlet key in keys)
                {
                    Vector3 vector = key.Position - camera.ViewPosition;
                    float num = Vector3.Dot(vector, camera.ViewDirection);
                    if (num > 0.01f)
                    {
                        float num2 = vector.Length();
                        if (num2 < m_subsystemSky.ViewFogRange.Y)
                        {
                            float num3 = key.Size;
                            if (key.FarDistance > 0f)
                            {
                                num3 += (key.FarSize - key.Size) * MathUtils.Saturate(num2 / key.FarDistance);
                            }
                            Vector3 v = (0f - (0.01f + 0.02f * num)) / num2 * vector;
                            Vector3 p = key.Position + num3 * (-key.Right - key.Up) + v;
                            Vector3 p2 = key.Position + num3 * (key.Right - key.Up) + v;
                            Vector3 p3 = key.Position + num3 * (key.Right + key.Up) + v;
                            Vector3 p4 = key.Position + num3 * (-key.Right + key.Up) + v;
                            BatchesByType.QueueQuad(p, p2, p3, p4, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f), key.Color);
                        }
                    }
                }
            }
            PrimitivesRenderer.Flush(camera.ViewProjectionMatrix);
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemSky = base.Project.FindSubsystem<SubsystemSky>(true);
            BatchesByType = PrimitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("传送门贴图"), false, 0, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwiseScissor, BlendState.AlphaBlend, SamplerState.LinearClamp);
        }

        public void CreateChartlet(Point3 point3, bool IsAxisX, WorldType worldType)
        {
            Chartlet[] chartlets = new Chartlet[2];
            Chartlet chartlet = new Chartlet();
            Chartlet chartlet2 = new Chartlet();
            Color color = SubsystemWorld.GetWorldDoorColor(worldType);
            Vector3 rposition = new Vector3(point3) + new Vector3(1.0f, 2.5f, 0.5f);
            Vector3 forward = new Vector3(0f, 0f, -1f);
            Vector3 up = new Vector3(0f, -1f, 0f);
            Vector3 right = new Vector3(-1f, 0f, 0f);
            Vector3 forward2 = new Vector3(0f, 0f, -1f);
            Vector3 up2 = new Vector3(0f, -1f, 0f);
            Vector3 right2 = new Vector3(1f, 0f, 0f);
            if (!IsAxisX)
            {
                rposition = new Vector3(point3) + new Vector3(0.5f, 2.5f, 1.0f);
                forward = new Vector3(-1f, 0f, 0f);
                up = new Vector3(0f, -1f, 0f);
                right = new Vector3(0f, 0f, -1f);
                forward2 = new Vector3(-1f, 0f, 0f);
                up2 = new Vector3(0f, -1f, 0f);
                right2 = new Vector3(0f, 0f, 1f);
            }
            chartlet.Position = rposition;
            chartlet.Forward = forward;
            chartlet.Up = up;
            chartlet.Right = right;
            chartlet.Color = color;
            chartlet2.Position = rposition;
            chartlet2.Forward = forward2;
            chartlet2.Up = up2;
            chartlet2.Right = right2;
            chartlet2.Color = color;
            chartlets[0] = chartlet;
            chartlets[1] = chartlet2;
            if (!m_chartlets.ContainsKey(point3))
            {
                m_chartlets.Add(point3, chartlets);
            }
        }
    }

    public class SubsystemTransferBlockBehavior : SubsystemBlockBehavior
    {
        public SubsystemWorld m_subsystemWorld;

        public SubsystemChartlet m_subsystemChartlet;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemParticles m_subsystemParticles;

        public override int[] HandledBlocks => new int[1] { 985 };

        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemWorld = base.Project.FindSubsystem<SubsystemWorld>(true);
            m_subsystemChartlet = base.Project.FindSubsystem<SubsystemChartlet>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
        }

        //使用传送石触发
        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            object raycastResult = componentMiner.Raycast(ray, RaycastMode.Interaction);
            if (raycastResult is TerrainRaycastResult)
            {
                CellFace cellFace = ((TerrainRaycastResult)raycastResult).CellFace;
                if (cellFace.Face != 4) return false;
                Point3 point = cellFace.Point;
                int id = m_subsystemTerrain.Terrain.GetCellContents(point.X, point.Y, point.Z);
                foreach (string[] parameters in WorldParameter.Collections)
                {
                    if (id == int.Parse(parameters[2]))
                    {
                        string[] colors = parameters[3].Split(new char[1] { ',' });
                        if (colors.Length > 3)
                        {
                            Color color = new Color(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]), int.Parse(colors[3]));
                            m_subsystemParticles.AddParticleSystem(new FireworksParticleSystem(new Vector3(point) + new Vector3(0.5f, 1f, 0.5f), color, FireworksBlock.Shape.SmallBurst, 0.8f, 0.3f));
                            break;
                        }
                    }
                }
                Point3 point2 = point + new Point3(1, 0, 0);
                bool createSuccess = CreateEntrance(point, point2, false, WorldType.Default);
                if (createSuccess) return true;
                point2 = point + new Point3(-1, 0, 0);
                createSuccess = CreateEntrance(point, point2, false, WorldType.Default);
                if (createSuccess) return true;
                point2 = point + new Point3(0, 0, 1);
                createSuccess = CreateEntrance(point, point2, false, WorldType.Default);
                if (createSuccess) return true;
                point2 = point + new Point3(0, 0, -1);
                createSuccess = CreateEntrance(point, point2, false, WorldType.Default);
                if (createSuccess) return true;
            }
            return false;
        }

        //判断搭建方块是否为传送门
        public bool JudgePass(Point3 point, Point3 point2, int cid)
        {
            for (int i = 0; i < 5; i++)
            {
                int id1 = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValue(point.X, point.Y + i, point.Z));
                int id2 = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValue(point2.X, point2.Y + i, point2.Z));
                if ((i == 0 || i == 4) && (id1 != cid || id2 != cid)) return false;
                if (i > 0 && i < 4 && (id1 != 0 || id2 != 0)) return false;
            }
            Point3 point3 = point2 - point;
            point = point - point3;
            point2 = point2 + point3;
            for (int i = 0; i < 5; i++)
            {
                int id3 = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValue(point.X, point.Y + i, point.Z));
                int id4 = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValue(point2.X, point2.Y + i, point2.Z));
                if (id3 != cid || id4 != cid) return false;
            }
            return true;
        }

        //创建传送门入口
        public bool CreateEntrance(Point3 point, Point3 point2, bool skipJudge, WorldType fastType)
        {
            foreach (string[] parameter in WorldParameter.Collections)
            {
                int cid = int.Parse(parameter[2]);
                bool pass = JudgePass(point, point2, cid);
                if (pass || skipJudge)
                {
                    WorldDoor worldDoor = new WorldDoor();
                    if (point.X - point2.X > 0 || point.Y - point2.Y > 0 || point.Z - point2.Z > 0)
                    {
                        Point3 temp = point;
                        point = point2;
                        point2 = temp;
                    }
                    bool AxisX = false;
                    Vector3 vector = new Vector3(1f, 0, 0);
                    Vector3 vector2 = new Vector3(0, 3f, 1f);
                    if (point2.X - point.X > 0 && point2.Z - point.Z == 0)
                    {
                        AxisX = true;
                        vector = new Vector3(0, 0, 1f);
                        vector2 = new Vector3(1f, 3f, 0);
                    }
                    worldDoor.WorldType = skipJudge ? fastType : SubsystemWorld.GetWorldType(parameter[0]);
                    worldDoor.MinPoint = new Vector3(point) + new Vector3(0, 1f, 0) + vector * 0.4f;
                    worldDoor.MaxPoint = new Vector3(point2) + new Vector3(0, 1f, 0) + vector * 0.6f + vector2;
                    if (!m_subsystemWorld.m_worldDoors.ContainsKey(point))
                    {
                        m_subsystemWorld.m_worldDoors.Add(point, worldDoor);
                        m_subsystemChartlet.CreateChartlet(point, AxisX, worldDoor.WorldType);
                    }
                    return true;
                }
            }
            return false;
        }
    }

    public class TransferBlock : Block//789
    {
        public const int Index = 985;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();

        public Color m_color;

        public TransferBlock()
        {
            DefaultDisplayName = "传送石";
            DefaultDescription = "传送石的制作很困难……毕竟它是曾经用来跨界的神器，有了它，你就能去任何世界了。制作他需要多维度水晶，和万界定位器，本初子以太。其余的材料我觉得应该不是问题。";
            DefaultCategory = "Items";
            CraftingId = "transfer";
            DisplayOrder = 1;
            IsPlaceable = false;
            FirstPersonScale = 0.4f;
            FirstPersonOffset = new Vector3(0.5f, -0.5f, -0.6f);
            InHandScale = 0.3f;
            InHandOffset = new Vector3(0, 0.12f, 0);
        }

        public override void Initialize()
        {
            m_color = new Color(192, 255, 128, 192);
            Model model = ContentManager.Get<Model>("Models/Diamond");
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Diamond").ParentBone);
            m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Diamond").MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, 0f, 0f), makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_color, 2f * size, ref matrix, environmentData);
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
        }
    }

    public class LoadScreenLoader : ModLoader
    {
        public override void __ModInitialize()
        {
            ModsManager.RegisterHook("BeforeGameLoading", this);
        }

        public override void OnLoadingFinished(List<Action> actions)
        {
            actions.Add(delegate
            {
                CraftingRecipe craftingRecipe = new CraftingRecipe();
                craftingRecipe.Description = "合成传送石";
                craftingRecipe.Message = "提示：等级达到3级才能合成传送石";
                craftingRecipe.Ingredients = new string[9] { null, "experience", null, "experience", "diamond", "experience", null, "experience", null };
                craftingRecipe.ResultValue = 985;
                craftingRecipe.ResultCount = 1;
                craftingRecipe.RemainsValue = 0;
                craftingRecipe.RemainsCount = 0;
                craftingRecipe.RequiredHeatLevel = 0;
                craftingRecipe.RequiredPlayerLevel = 3;
                CraftingRecipesManager.m_recipes.Add(craftingRecipe);
            });
        }

        public override object BeforeGameLoading(PlayScreen playScreen, object item)
        {
            string path = ((WorldInfo)item).DirectoryName;
            string wpath = string.Empty;
            XElement xElement = null;
            XElement subXElement = null;
            using (Stream stream = Storage.OpenFile(Storage.CombinePaths(path, "Project.xml"), OpenFileMode.Read))
            {
                xElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
            }
            foreach (XElement e in xElement.Element("Subsystems").Elements())
            {
                if (XmlUtils.GetAttributeValue<string>(e, "Name") == "SubWorld")
                {
                    wpath = XmlUtils.GetAttributeValue<string>(e.Element("Value"), "Value");
                }
            }
            if (wpath != string.Empty)
            {
                using (Stream stream = Storage.OpenFile(Storage.CombinePaths(wpath, "Project.xml"), OpenFileMode.Read))
                {
                    subXElement = XmlUtils.LoadXmlFromStream(stream, null, throwOnError: true);
                }
                //存档设置与主世界同步
                SubsystemWorld.SynchronizeGameInfo(xElement, subXElement, path, wpath, false);
            }
            else
            {
                wpath = path;
            }
            //进入上一次退出的存档
            GameLoadingScreen gameLoadingScreen = ScreensManager.FindScreen<GameLoadingScreen>("GameLoading");
            gameLoadingScreen.m_worldInfo = WorldsManager.GetWorldInfo(wpath);
            return gameLoadingScreen.m_worldInfo;
        }
    }
}//多维度子系统处理保存加载等功能



namespace Game
{
    /*
     * 添加一个世界的方法：
     * 1.在WorldType枚举类添加一个类型值，如Test
     * 2.在WorldParameter类的Collections数组添加一行相似元素，如 new string[4] { "Test", "测试世界", "26", "0,255,128,128" } ，Collections数组长度也要改（new string[5][]）
     * 3.为新添加的世界类型写对应的地形构造类，如果不写默认原版地形
     * 4.就行了（去除世界是一样的，逆过来）
     * 5.如果新世界要引用外部存档，则把存档塞scmod里，存档名称和世界类型名一样即可，如Exist.scworld（超平坦存档要换超平坦对应的地形构造器，其他同理）
     */

    public enum WorldType
    {
        //Default对应主世界，无特别需要不修改
        Default, Ashes, Desert, Snowfield, Limit, Exist, Moon, StationMoon
    }

    public static class WorldParameter
    {
        //世界类型名，中文标识名，世界传送门方块ID，世界传送门贴图颜色
        public static string[][] Collections = new string[7][]
        {
            new string[4] { "Ashes", "灰烬世界", "67", "255,0,255,128" },
            new string[4] { "Desert", "沙漠世界", "4", "255,255,0,128" },
            new string[4] { "Snowfield", "冰雪世界", "62", "0,255,255,128" },
            new string[4] { "Limit", "限制世界", "2", "255,128,128,128" },
            new string[4] { "Exist", "现有世界", "46", "128,255,128,128" },
            new string[4] { "Moon", "月球", "9", "128,255,128,128" },
            new string[4] { "StationMoon", "地月空间站", "10", "128,255,128,128" },

        };
    }

    public class SubsystemWorldDemo : SubsystemWorld, IUpdateable
    {
        public List<Point3> m_originatePointList = new List<Point3>(); //建筑群生成点
        public string[][] m_buildings;  //以区块为单位的建筑文本数组
        public int coordsXnum; //建筑群在X轴方向的区块数
        public int coordsYnum; //建筑群在Z轴方向的区块数


        public SubsystemTerrain subsystemTerrain;

        public SubsystemWeather subsystemWeather;

        public WorldType worldType;//这个是用来标识世界用的

        public new void Update(float dt)//世界子系统逐帧更新来控制世界变量
        {
            base.Update(dt);
            if (worldType == WorldType.Default)
            {
                //如果为主世界，则怎么样
            }
            else if (worldType == WorldType.Snowfield)
            {
                //如果为冰雪世界，常年下雪
                subsystemWeather.m_precipitationStartTime = 0f;
            }
            else if (worldType == WorldType.Limit)
            {
                //如果为限制世界，玩家属性最优，可以多段跳跃
                if (base.m_componentPlayer != null)
                {
                    base.m_componentPlayer.ComponentFlu.m_fluDuration = 0f;
                    base.m_componentPlayer.ComponentFlu.m_coughDuration = 0f;
                    base.m_componentPlayer.ComponentSickness.m_sicknessDuration = 0f;
                    base.m_componentPlayer.ComponentSickness.m_greenoutDuration = 0f;
                    base.m_componentPlayer.ComponentVitalStats.Sleep = 1f;
                    base.m_componentPlayer.ComponentVitalStats.Stamina = 1f;
                    base.m_componentPlayer.ComponentVitalStats.Temperature = 12f;
                    base.m_componentPlayer.ComponentVitalStats.Wetness = 8f;
                    //多段跳跃
                    if (base.m_componentPlayer.ComponentInput.PlayerInput.Jump && base.m_componentPlayer.ComponentLocomotion.m_falling)
                    {
                        Vector3 velocity = base.m_componentPlayer.ComponentBody.Velocity;
                        base.m_componentPlayer.ComponentBody.Velocity = new Vector3(velocity.X, 7.5f, velocity.Z);
                    }
                }
            }
            else if (worldType == WorldType.Moon)
            {
                //如果为月球,重力为6分之一
                /*if (m_componentbody != null)
                {
                    m_componentbody.IsGravityEnabled = false;
                    m_componentbody.m_velocity.Y = m_componentbody.m_velocity.Y - 10f/6f * dt;
                    if (m_componentbody.ImmersionFactor > 0f)
                    {
                        float num = m_componentbody.ImmersionFactor * (1f + 0.03f * MathUtils.Sin((float)MathUtils.Remainder(2.0 * m_componentbody.m_subsystemTime.GameTime, 6.2831854820251465)));
                        m_componentbody.m_velocity.Y = m_componentbody.m_velocity.Y + 10f * (1f / m_componentbody.Density * num) * dt;
                    }

                }*/



            }
            else if (worldType == WorldType.StationMoon)
            {





            }
        }
        public bool IsCityGenerated;
        public override void Save(ValuesDictionary valuesDictionary)
        {
            base.Save(valuesDictionary);
            valuesDictionary.SetValue<bool>("IsCityGenrated", IsCityGenerated);
        }
        public override void Load(ValuesDictionary valuesDictionary)//加载世界时候的预设
        {
            base.Load(valuesDictionary);
            IsCityGenerated = valuesDictionary.GetValue<bool>("IsCityGenrated", false);
            subsystemTerrain = base.m_subsystemTerrain;
            subsystemWeather = base.Project.FindSubsystem<SubsystemWeather>(true);
            worldType = base.m_worldType;
            //如果为子世界，则更改地形构造
            switch (worldType)
            {
                case WorldType.Ashes: subsystemTerrain.TerrainContentsGenerator = new AshesTerrainGenerator(subsystemTerrain); break;
                case WorldType.Desert: subsystemTerrain.TerrainContentsGenerator = new DesertTerrainGenerator(subsystemTerrain); break;
                case WorldType.Snowfield: subsystemTerrain.TerrainContentsGenerator = new SnowfieldTerrainGenerator(subsystemTerrain); break;
                case WorldType.Limit: subsystemTerrain.TerrainContentsGenerator = new LimitTerrainGenerator(subsystemTerrain); break;
                case WorldType.Exist: subsystemTerrain.TerrainContentsGenerator = new TerrainContentsGeneratorFlat(subsystemTerrain); break;
                case WorldType.StationMoon: subsystemTerrain.TerrainContentsGenerator = new StationTerrainGenerator(subsystemTerrain); break;
                case WorldType.Moon: subsystemTerrain.TerrainContentsGenerator = new MoonTerrainGenerator(subsystemTerrain); break;
                default: break;
            }
            //如果为灰烬世界，则修改材质包，时间固定为黄昏，天气效果关闭
            if (worldType == WorldType.Ashes)
            {
                base.Project.FindSubsystem<SubsystemBlocksTexture>(true).BlocksTexture = ContentManager.Get<Texture2D>("寂静岭材质包");
                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.TimeOfDayMode = TimeOfDayMode.Sunset;
                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.AreWeatherEffectsEnabled = false;
            }
            //如果为冰雪世界，则更改动物生成
            else if (worldType == WorldType.Snowfield)
            {
                ChangeCreatureTypes();
            }

            else if (worldType == WorldType.StationMoon)//如果是空间站
            {

                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.TimeOfDayMode = TimeOfDayMode.Changing;//恒为白天
                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.AreWeatherEffectsEnabled = false;


            }
            else if (worldType == WorldType.Moon)//如果是月球
            {

                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.TimeOfDayMode = TimeOfDayMode.Changing;//恒为白天
                base.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.AreWeatherEffectsEnabled = false;

                /*if (m_componentLocomotion != null)
                {
                    m_componentLocomotion.JumpSpeed = 12f;
					m_componentPlayer.ComponentLocomotion.JumpSpeed = 12f;
                }*/


            }
            else if (worldType == WorldType.Default)
            {


            }
        }
        public ComponentBody m_componentbody;
        public ComponentBody m_componentbody2;
        public ComponentLocomotion m_componentLocomotion;
        public ComponentLocomotion m_componentLocomotion2;
        public override void OnEntityAdded(Entity entity)
        {
            m_componentbody = entity.FindComponent<ComponentBody>();

            m_componentLocomotion = entity.FindComponent<ComponentLocomotion>();

            //如果为灰烬世界，则更改动物的部分属性
            if (worldType == WorldType.Ashes)
            {
                ComponentCreature componentCreature = entity.FindComponent<ComponentCreature>();
                ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();

                if (componentCreature != null && componentPlayer == null)
                {
                    componentCreature.ComponentCreatureModel.TextureOverride = ContentManager.Get<Texture2D>("Textures/Creatures/Jaguar");
                    componentCreature.ComponentLocomotion.FlySpeed = componentCreature.ComponentLocomotion.WalkSpeed * 3;
                    componentCreature.ComponentLocomotion.WalkSpeed = componentCreature.ComponentLocomotion.WalkSpeed * 2f;
                    componentCreature.ComponentHealth.Attacked += delegate
                    {
                        Random random = new Random();
                        if (random.Float(0, 1f) <= 0.04f)
                        {
                            Vector3 creaturePosition = componentCreature.ComponentBody.Position;
                            base.Project.FindSubsystem<SubsystemPickables>().AddPickable(111, 1, creaturePosition, null, null);
                        }
                    };
                }
            }
            /*if(worldType == WorldType.Moon)
			{
				m_componentbody = entity.FindComponent<ComponentBody>();
				m_componentLocomotion= entity.FindComponent<ComponentLocomotion>();
			}
            if (worldType == WorldType.StationMoon)
            {
                m_componentbody = entity.FindComponent<ComponentBody>();
            }*/
            base.OnEntityAdded(entity);
        }

        //更改自然生成的动物
        public void ChangeCreatureTypes()
        {
            SubsystemCreatureSpawn subsystemCreatureSpawn = base.Project.FindSubsystem<SubsystemCreatureSpawn>(true);
            subsystemCreatureSpawn.m_creatureTypes.Clear();
            subsystemCreatureSpawn.m_creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("Seagull", SpawnLocationType.Surface, randomSpawn: true, constantSpawn: false)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    float oceanShoreDistance = m_subsystemTerrain.TerrainContentsGenerator.CalculateOceanShoreDistance(point.X, point.Z);
                    return (oceanShoreDistance < 8f) ? 5f : 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => subsystemCreatureSpawn.SpawnCreatures(creatureType, "Seagull", point, 1).Count)
            });
            subsystemCreatureSpawn.m_creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("White Tigers", SpawnLocationType.Surface, randomSpawn: true, constantSpawn: false)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    float oceanShoreDistance = m_subsystemTerrain.TerrainContentsGenerator.CalculateOceanShoreDistance(point.X, point.Z);
                    return (oceanShoreDistance > 8f) ? 5f : 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => subsystemCreatureSpawn.SpawnCreatures(creatureType, "Tiger_White", point, 1).Count)
            });
            subsystemCreatureSpawn.m_creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("White Bull", SpawnLocationType.Surface, randomSpawn: true, constantSpawn: false)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    float oceanShoreDistance = m_subsystemTerrain.TerrainContentsGenerator.CalculateOceanShoreDistance(point.X, point.Z);
                    return (oceanShoreDistance > 8f) ? 5f : 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => subsystemCreatureSpawn.SpawnCreatures(creatureType, "Bull_White", point, 1).Count)
            });
            subsystemCreatureSpawn.m_creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("Polar Bears", SpawnLocationType.Surface, randomSpawn: true, constantSpawn: false)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    float oceanShoreDistance = m_subsystemTerrain.TerrainContentsGenerator.CalculateOceanShoreDistance(point.X, point.Z);
                    return (oceanShoreDistance > 8f) ? 5f : 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => subsystemCreatureSpawn.SpawnCreatures(creatureType, "Bear_Polar", point, 1).Count)
            });
            subsystemCreatureSpawn.m_creatureTypes.Add(new SubsystemCreatureSpawn.CreatureType("White Horse", SpawnLocationType.Surface, randomSpawn: true, constantSpawn: false)
            {
                SpawnSuitabilityFunction = delegate (SubsystemCreatureSpawn.CreatureType creatureType, Point3 point)
                {
                    float oceanShoreDistance = m_subsystemTerrain.TerrainContentsGenerator.CalculateOceanShoreDistance(point.X, point.Z);
                    return (oceanShoreDistance > 8f) ? 5f : 0f;
                },
                SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => subsystemCreatureSpawn.SpawnCreatures(creatureType, "Horse_White", point, 1).Count)
            });
        }
    }

    public class SubsystemEntityBlockBehavior : SubsystemBlockBehavior
    {
        public override int[] HandledBlocks => new int[4] { 45, 64, 27, 216 };

        public SubsystemBlockEntities SubsystemBlockEntities;

        public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
        {
            Point3 point3 = new Point3(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
            ComponentBlockEntity blockEntity = SubsystemBlockEntities.GetBlockEntity(point3.X, point3.Y, point3.Z);
            //交互时，如果箱子、熔炉、工作台和发射器的方块实体为空，则创建方块实体并添加指定物品
            if (blockEntity == null && componentMiner.ComponentPlayer != null)
            {
                int id = base.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellContents(point3.X, point3.Y, point3.Z);
                switch (id)
                {
                    case 45:
                        {
                            DatabaseObject databaseObject = base.Project.GameDatabase.Database.FindDatabaseObject("Chest", base.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                            ValuesDictionary valuesDictionary = new ValuesDictionary();
                            valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                            valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                            Entity entity = base.Project.CreateEntity(valuesDictionary);
                            base.Project.AddEntity(entity);
                            ComponentChest componentChest = entity.FindComponent<ComponentChest>(throwOnError: true);
                            for (int i = 0; i < 15; i++)
                            {
                                componentChest.m_slots[i].Value = new Random().Int(1, BlocksManager.Blocks.Length - 1);
                                componentChest.m_slots[i].Count = 1;
                            }
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new ChestWidget(componentMiner.Inventory, componentChest);
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            return true;
                        }
                    case 64:
                        {
                            DatabaseObject databaseObject = base.Project.GameDatabase.Database.FindDatabaseObject("Furnace", base.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                            ValuesDictionary valuesDictionary = new ValuesDictionary();
                            valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                            valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                            Entity entity = base.Project.CreateEntity(valuesDictionary);
                            base.Project.AddEntity(entity);
                            ComponentFurnace componentFurnace = entity.FindComponent<ComponentFurnace>(throwOnError: true);
                            int[] items = new int[3] { 77, 88, 176 };
                            if (new Random().Float(0, 1f) < 0.4f)
                            {
                                componentFurnace.m_slots[0].Value = items[new Random().Int(0, items.Length - 1)];
                                componentFurnace.m_slots[0].Count = 1;
                            }
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new FurnaceWidget(componentMiner.Inventory, componentFurnace);
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            return true;
                        }
                    case 27:
                        {
                            DatabaseObject databaseObject = base.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", base.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                            ValuesDictionary valuesDictionary = new ValuesDictionary();
                            valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                            valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                            Entity entity = base.Project.CreateEntity(valuesDictionary);
                            base.Project.AddEntity(entity);
                            ComponentCraftingTable componentCraftingTable = entity.FindComponent<ComponentCraftingTable>(throwOnError: true);
                            int[] items = new int[15] { 29, 165, 37, 222, 36, 38, 218, 219, 171, 169, 90, 117, 121, 120, 230 };
                            for (int i = 0; i < 9; i++)
                            {
                                if (new Random().Float(0, 1f) < 0.1f)
                                {
                                    componentCraftingTable.m_slots[i].Value = items[new Random().Int(0, items.Length - 1)];
                                    componentCraftingTable.m_slots[i].Count = 1;
                                }
                            }
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new CraftingTableWidget(componentMiner.Inventory, componentCraftingTable);
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            return true;
                        }
                    case 216:
                        {
                            DatabaseObject databaseObject = base.Project.GameDatabase.Database.FindDatabaseObject("Dispenser", base.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                            ValuesDictionary valuesDictionary = new ValuesDictionary();
                            valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                            valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                            Entity entity = base.Project.CreateEntity(valuesDictionary);
                            base.Project.AddEntity(entity);
                            ComponentDispenser componentDispenser = entity.FindComponent<ComponentDispenser>(throwOnError: true);
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new DispenserWidget(componentMiner.Inventory, componentDispenser);
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            return true;
                        }
                }
            }
            return false;
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            SubsystemBlockEntities = base.Project.FindSubsystem<SubsystemBlockEntities>(true);
            SubsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
        }
    }

    public class AirWallBlock : AlphaTestCubeBlock
    {
        public const int Index = 1023;

        //空气墙方块
        public AirWallBlock()
        {
            DefaultCreativeData = -1;
        }

        public override bool ShouldGenerateFace(SubsystemTerrain subsystemTerrain, int face, int value, int neighborValue)
        {
            return false;
        }

        public override float GetDigResilience(int value)
        {
            return float.PositiveInfinity;
        }

        public override float GetProjectileResilience(int value)
        {
            return float.PositiveInfinity;
        }

        public override float GetExplosionResilience(int value)
        {
            return float.PositiveInfinity;
        }
    }

    //灰烬世界构造
    public class AshesTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public AshesTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            GenerateSurfaceParameters(chunk, 0, 0, 16, 8);
            NGenerateTerrain(chunk, 0, 0, 16, 8);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
            GenerateSurfaceParameters(chunk, 0, 8, 16, 16);
            NGenerateTerrain(chunk, 0, 8, 16, 16);
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            GenerateCaves(chunk);
            GeneratePockets(chunk);
            GenerateMinerals(chunk);
            PropagateFluidsDownwards(chunk);
        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            GenerateBedrockAndAir(chunk);
        }

        public void NGenerateTerrain(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            int num = x2 - x1;
            int num2 = z2 - z1;
            _ = m_subsystemTerrain.Terrain;
            int num3 = chunk.Origin.X + x1;
            int num4 = chunk.Origin.Y + z1;
            Grid2d grid2d = new Grid2d(num, num2);
            Grid2d grid2d2 = new Grid2d(num, num2);
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    grid2d.Set(j, i, CalculateOceanShoreDistance(j + num3, i + num4));
                    grid2d2.Set(j, i, CalculateMountainRangeFactor(j + num3, i + num4));
                }
            }
            Grid3d grid3d = new Grid3d(num / 4 + 1, 33, num2 / 4 + 1);
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int num5 = k * 4 + num3;
                    int num6 = l * 4 + num4;
                    float num7 = CalculateHeight(num5, num6);
                    float v = CalculateMountainRangeFactor(num5, num6);
                    float num8 = MathUtils.Lerp(TGMinTurbulence, 1f, Squish(v, TGTurbulenceZero, 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        int num9 = m * 8;
                        float num10 = TGTurbulenceStrength * num8 * MathUtils.Saturate(num7 - (float)num9) * (2f * SimplexNoise.OctavedNoise(num5, num9, num6, TGTurbulenceFreq, TGTurbulenceOctaves, 4f, TGTurbulencePersistence) - 1f);
                        float num11 = (float)num9 + num10;
                        float num12 = num7 - num11;
                        num12 += MathUtils.Max(4f * (TGDensityBias - (float)num9), 0f);
                        grid3d.Set(k, m, l, num12);
                    }
                }
            }
            int oceanLevel = OceanLevel;
            for (int n = 0; n < grid3d.SizeX - 1; n++)
            {
                for (int num13 = 0; num13 < grid3d.SizeZ - 1; num13++)
                {
                    for (int num14 = 0; num14 < grid3d.SizeY - 1; num14++)
                    {
                        grid3d.Get8(n, num14, num13, out float v2, out float v3, out float v4, out float v5, out float v6, out float v7, out float v8, out float v9);
                        float num15 = (v3 - v2) / 4f;
                        float num16 = (v5 - v4) / 4f;
                        float num17 = (v7 - v6) / 4f;
                        float num18 = (v9 - v8) / 4f;
                        float num19 = v2;
                        float num20 = v4;
                        float num21 = v6;
                        float num22 = v8;
                        for (int num23 = 0; num23 < 4; num23++)
                        {
                            float num24 = (num21 - num19) / 4f;
                            float num25 = (num22 - num20) / 4f;
                            float num26 = num19;
                            float num27 = num20;
                            for (int num28 = 0; num28 < 4; num28++)
                            {
                                float num29 = (num27 - num26) / 8f;
                                float num30 = num26;
                                int num31 = num23 + n * 4;
                                int num32 = num28 + num13 * 4;
                                int x3 = x1 + num31;
                                int z3 = z1 + num32;
                                float x4 = grid2d.Get(num31, num32);
                                float num33 = grid2d2.Get(num31, num32);
                                int temperatureFast = chunk.GetTemperatureFast(x3, z3);
                                int humidityFast = chunk.GetHumidityFast(x3, z3);
                                float f = num33 - 0.01f * (float)humidityFast;
                                float num34 = MathUtils.Lerp(100f, 0f, f);
                                float num35 = MathUtils.Lerp(300f, 30f, f);
                                bool flag = (temperatureFast > 8 && humidityFast < 8 && num33 < 0.97f) || (MathUtils.Abs(x4) < 16f && num33 < 0.97f);
                                int num36 = TerrainChunk.CalculateCellIndex(x3, 0, z3);
                                for (int num37 = 0; num37 < 8; num37++)
                                {
                                    int num38 = num37 + num14 * 8;
                                    int value = 0;
                                    if (num30 < 0f)
                                    {
                                        if (num38 <= oceanLevel)
                                        {
                                            value = 92;
                                        }
                                    }
                                    else
                                    {
                                        value = ((!flag) ? ((!(num30 < num35)) ? 67 : 3) : ((!(num30 < num34)) ? ((!(num30 < num35)) ? 67 : 3) : 4));
                                    }
                                    chunk.SetCellValueFast(num36 + num38, value);
                                    num30 += num29;
                                }
                                num26 += num24;
                                num27 += num25;
                            }
                            num19 += num15;
                            num20 += num16;
                            num21 += num17;
                            num22 += num18;
                        }
                    }
                }
            }
        }
    }

    //沙漠世界构造
    public class DesertTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public DesertTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 0, 16, 8);
            NGenerateTerrain(chunk, 0, 0, 16, 8);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 8, 16, 16);
            NGenerateTerrain(chunk, 0, 8, 16, 16);
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            GenerateCaves(chunk);
            GeneratePockets(chunk);
            GenerateMinerals(chunk);
            PropagateFluidsDownwards(chunk);
        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            int cx = chunk.Coords.X;
            int cy = chunk.Coords.Y;
            if (!((cy - 4 * cx) % 5 == 0 && (cy + 7 * cx) % 4 == 0 && ((int)MathUtils.Sqrt(cx * cx + cy * cy)) % 7 == 0))
            {
                GenerateCacti(chunk);
                return;
            }
            int level = 10;
            bool canGenerate = true;
            for (int y = 200; y > 10; y--)
            {
                int id = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X, y, chunk.Origin.Y);
                if (id != 0)
                {
                    level = y;
                    break;
                }
            }
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int id1 = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X + x, level, chunk.Origin.Y + z);
                    int id2 = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X + x, level + 1, chunk.Origin.Y + z);
                    if (!(id1 != 0 && id1 != 18 && id2 == 0))
                    {
                        canGenerate = false;
                        break;
                    }
                }
            }
            if (canGenerate)
            {
                string blocks = ContentManager.Get<string>("沙漠房子");
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
                        int i = int.Parse(block[3]);
                        subsystemTerrain.Terrain.SetCellValueFast(chunk.Origin.X + x, level + 1 + y, chunk.Origin.Y + z, i);
                    }
                }
            }
            else
            {
                GenerateCacti(chunk);
            }
        }

        public void NGenerateSurfaceParameters(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            for (int i = x1; i < x2; i++)
            {
                for (int j = z1; j < z2; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;
                    int temperature = MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + m_temperatureOffset.X, z + m_temperatureOffset.Y, 0.0015f / TGBiomeScaling, 5, 2f, 0.6f) - 1.1f + m_worldSettings.TemperatureOffset / 16f) * 16f), 12, 15);
                    int humidity = MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + m_humidityOffset.X, z + m_humidityOffset.Y, 0.0012f / TGBiomeScaling, 5, 2f, 0.6f) - 0.9f + m_worldSettings.HumidityOffset / 16f) * 16f), 0, 3);
                    chunk.SetTemperatureFast(i, j, temperature);
                    chunk.SetHumidityFast(i, j, humidity);
                }
            }
        }

        public void NGenerateTerrain(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            int num = x2 - x1;
            int num2 = z2 - z1;
            _ = m_subsystemTerrain.Terrain;
            int num3 = chunk.Origin.X + x1;
            int num4 = chunk.Origin.Y + z1;
            Grid2d grid2d = new Grid2d(num, num2);
            Grid2d grid2d2 = new Grid2d(num, num2);
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    grid2d.Set(j, i, CalculateOceanShoreDistance(j + num3, i + num4));
                    grid2d2.Set(j, i, CalculateMountainRangeFactor(j + num3, i + num4));
                }
            }
            Grid3d grid3d = new Grid3d(num / 4 + 1, 33, num2 / 4 + 1);
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int num5 = k * 4 + num3;
                    int num6 = l * 4 + num4;
                    float num7 = CalculateHeight(num5, num6);
                    float v = CalculateMountainRangeFactor(num5, num6);
                    float num8 = MathUtils.Lerp(TGMinTurbulence, 1f, Squish(v, TGTurbulenceZero, 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        int num9 = m * 8;
                        float num10 = TGTurbulenceStrength * num8 * MathUtils.Saturate(num7 - (float)num9) * (2f * SimplexNoise.OctavedNoise(num5, num9, num6, TGTurbulenceFreq, TGTurbulenceOctaves, 4f, TGTurbulencePersistence) - 1f);
                        float num11 = (float)num9 + num10;
                        float num12 = num7 - num11;
                        num12 += MathUtils.Max(4f * (TGDensityBias - (float)num9), 0f);
                        grid3d.Set(k, m, l, num12);
                    }
                }
            }
            int oceanLevel = OceanLevel;
            for (int n = 0; n < grid3d.SizeX - 1; n++)
            {
                for (int num13 = 0; num13 < grid3d.SizeZ - 1; num13++)
                {
                    for (int num14 = 0; num14 < grid3d.SizeY - 1; num14++)
                    {
                        grid3d.Get8(n, num14, num13, out float v2, out float v3, out float v4, out float v5, out float v6, out float v7, out float v8, out float v9);
                        float num15 = (v3 - v2) / 4f;
                        float num16 = (v5 - v4) / 4f;
                        float num17 = (v7 - v6) / 4f;
                        float num18 = (v9 - v8) / 4f;
                        float num19 = v2;
                        float num20 = v4;
                        float num21 = v6;
                        float num22 = v8;
                        for (int num23 = 0; num23 < 4; num23++)
                        {
                            float num24 = (num21 - num19) / 4f;
                            float num25 = (num22 - num20) / 4f;
                            float num26 = num19;
                            float num27 = num20;
                            for (int num28 = 0; num28 < 4; num28++)
                            {
                                float num29 = (num27 - num26) / 8f;
                                float num30 = num26;
                                int num31 = num23 + n * 4;
                                int num32 = num28 + num13 * 4;
                                int x3 = x1 + num31;
                                int z3 = z1 + num32;
                                float x4 = grid2d.Get(num31, num32);
                                float num33 = grid2d2.Get(num31, num32);
                                int temperatureFast = chunk.GetTemperatureFast(x3, z3);
                                int humidityFast = chunk.GetHumidityFast(x3, z3);
                                float f = num33 - 0.01f * (float)humidityFast;
                                float num34 = MathUtils.Lerp(100f, 0f, f);
                                float num35 = MathUtils.Lerp(300f, 30f, f);
                                bool flag = (temperatureFast > 8 && humidityFast < 8 && num33 < 0.97f) || (MathUtils.Abs(x4) < 16f && num33 < 0.97f);
                                int num36 = TerrainChunk.CalculateCellIndex(x3, 0, z3);
                                for (int num37 = 0; num37 < 8; num37++)
                                {
                                    int num38 = num37 + num14 * 8;
                                    int value = 0;
                                    if (num30 >= 0f)
                                    {
                                        value = ((!flag) ? ((!(num30 < num35)) ? 67 : 3) : ((!(num30 < num34)) ? ((!(num30 < num35)) ? 67 : 3) : 4));
                                    }
                                    chunk.SetCellValueFast(num36 + num38, value);
                                    num30 += num29;
                                }
                                num26 += num24;
                                num27 += num25;
                            }
                            num19 += num15;
                            num20 += num16;
                            num21 += num17;
                            num22 += num18;
                        }
                    }
                }
            }
        }
    }

    //冰雪世界构造
    public class SnowfieldTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public SnowfieldTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 0, 16, 8);
            GenerateTerrain(chunk, 0, 0, 16, 8);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 8, 16, 16);
            GenerateTerrain(chunk, 0, 8, 16, 16);
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            GenerateCaves(chunk);
            GeneratePockets(chunk);
            GenerateMinerals(chunk);
            GenerateSurface(chunk);
            PropagateFluidsDownwards(chunk);
        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            GenerateGrassAndPlants(chunk);
            GenerateTreesAndLogs(chunk);
            GenerateCacti(chunk);
            GeneratePumpkins(chunk);
            GenerateKelp(chunk);
            GenerateSeagrass(chunk);
            GenerateBottomSuckers(chunk);
            GenerateTraps(chunk);
            GenerateIvy(chunk);
            GenerateGraves(chunk);
            GenerateSnowAndIce(chunk);
            GenerateBedrockAndAir(chunk);
            UpdateFluidIsTop(chunk);
        }

        public void NGenerateSurfaceParameters(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            for (int i = x1; i < x2; i++)
            {
                for (int j = z1; j < z2; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;
                    int temperature = 0;
                    int humidity = 15;
                    chunk.SetTemperatureFast(i, j, temperature);
                    chunk.SetHumidityFast(i, j, humidity);
                }
            }
        }
    }

    //限制世界构造
    public class LimitTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public Vector3 playerSpawnPosition;

        public LimitTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
            playerSpawnPosition = FindCoarseSpawnPosition();
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk);
            NGenerateTerrain(chunk);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            GenerateCaves(chunk);
            //GeneratePockets(chunk);
            GenerateMinerals(chunk);
            GenerateSurface(chunk);
            PropagateFluidsDownwards(chunk);
        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            GenerateGrassAndPlants(chunk);
            GenerateTreesAndLogs(chunk);
            GenerateCacti(chunk);
            GeneratePumpkins(chunk);
            GenerateKelp(chunk);
            GenerateSeagrass(chunk);
            GenerateBottomSuckers(chunk);
            GenerateTraps(chunk);
            GenerateIvy(chunk);
            GenerateGraves(chunk);
            //GenerateSnowAndIce(chunk);
            //GenerateBedrockAndAir(chunk);
            UpdateFluidIsTop(chunk);
        }

        public void NGenerateSurfaceParameters(TerrainChunk chunk)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int num = i + chunk.Origin.X;
                    int num2 = j + chunk.Origin.Y;
                    int temperature = CalculateTemperature(num, num2);
                    int humidity = CalculateHumidity(num, num2);
                    chunk.SetTemperatureFast(i, j, temperature);
                    chunk.SetHumidityFast(i, j, humidity);
                }
            }
        }

        public void NGenerateTerrain(TerrainChunk chunk)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;
                    float sr = (int)(x - playerSpawnPosition.X) * (x - playerSpawnPosition.X) + (z - playerSpawnPosition.Z) * (z - playerSpawnPosition.Z);
                    //生成倒圆锥
                    for (int k = 0; k < 100; k++)
                    {
                        int y = k - 34;
                        if (sr < k * k && y > 0)
                        {
                            chunk.SetCellValueFast(i, y, j, 2);
                        }
                    }
                    //生成空气墙
                    for (int l = 0; l < 255; l++)
                    {
                        if (sr >= 10000 && sr <= 105 * 105)
                        {
                            chunk.SetCellValueFast(i, l, j, 1023);
                        }
                    }
                }
            }
        }
    }//限制世界

    public class StationTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public Vector3 playerSpawnPosition;

        public StationTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
            playerSpawnPosition = FindCoarseSpawnPosition();
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk);
            StationGenerateTerrain(chunk);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {

        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {

        }

        public void NGenerateSurfaceParameters(TerrainChunk chunk)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int num = i + chunk.Origin.X;
                    int num2 = j + chunk.Origin.Y;
                    int temperature = CalculateTemperature(num, num2);
                    int humidity = CalculateHumidity(num, num2);
                    chunk.SetTemperatureFast(i, j, 0);//空间站始终为0
                    chunk.SetHumidityFast(i, j, 0);
                }
            }
        }

        public void StationGenerateTerrain(TerrainChunk chunk)
        {
            /*for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;

                    //生成平台
                    for (int k = 0; k < 2; k++)
                    {
                        int y = k;
                        chunk.SetCellValueFast(i, y, j, 46);
                    }
                }
            }*/
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;
                    float sr = (int)MathUtils.Sqrt((x - playerSpawnPosition.X) * (x - playerSpawnPosition.X) + (z - playerSpawnPosition.Z) * (z - playerSpawnPosition.Z));
                    //计算当前格子的中心点到玩家出生点的平方距离。
                    //生成倒圆锥
                    //这是嵌套在j循环内部的另一个for循环，用于迭代100次，k值为0到99，代表当前格子的高度。
                    for (int k = 0; k < 2; k++)
                    {
                        int y = (int)playerSpawnPosition.Y - k - 2;
                        if (sr < 32 && y > 0)
                        //判断当前格子的高度是否小于平方距离，并且高度大于0。
                        {
                            chunk.SetCellValueFast(i, y, j, 46);
                        }
                    }
                    //生成空气墙
                    /*for (int l = 0; l < 255; l++)
                    {
                        //这是嵌套在j循环内部的另一个for循环，用于迭代255次，l值为0到254，代表当前格子的高度。
                        if (sr >= 10000 && sr <= 105 * 105)
                        {
                            //判断当前格子的平方距离是否在10000到11025之间。
                            chunk.SetCellValueFast(i, l, j, 1023);
                        }
                    }*/
                }
            }
        }
    }//空间站地形构筑
    public class MoonTerrainGenerator : TerrainContentsGenerator22, ITerrainContentsGenerator
    {
        public SubsystemTerrain subsystemTerrain;

        public MoonTerrainGenerator(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            this.subsystemTerrain = subsystemTerrain;
        }

        public new void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 0, 16, 8);
            NGenerateTerrain(chunk, 0, 0, 16, 8);
        }

        public new void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
            NGenerateSurfaceParameters(chunk, 0, 8, 16, 16);
            NGenerateTerrain(chunk, 0, 8, 16, 16);
        }

        public new void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            GenerateCaves(chunk);
            //GeneratePockets(chunk);
            GenerateMoonPockets(chunk);
            GenerateMinerals(chunk);
            //PropagateFluidsDownwards(chunk);
        }

        public new void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            /*int cx = chunk.Coords.X;
            int cy = chunk.Coords.Y;
            if (!((cy - 4 * cx) % 5 == 0 && (cy + 7 * cx) % 4 == 0 && ((int)MathUtils.Sqrt(cx * cx + cy * cy)) % 7 == 0))
            {
                GenerateCacti(chunk);
                return;
            }
            int level = 10;
            bool canGenerate = true;
            for (int y = 200; y > 10; y--)
            {
                int id = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X, y, chunk.Origin.Y);
                if (id != 0)
                {
                    level = y;
                    break;
                }
            }
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int id1 = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X + x, level, chunk.Origin.Y + z);
                    int id2 = subsystemTerrain.Terrain.GetCellContentsFast(chunk.Origin.X + x, level + 1, chunk.Origin.Y + z);
                    if (!(id1 != 0 && id1 != 18 && id2 == 0))
                    {
                        canGenerate = false;
                        break;
                    }
                }
            }
            if (canGenerate)
            {
                string blocks = ContentManager.Get<string>("沙漠房子");
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
                        int i = int.Parse(block[3]);
                        subsystemTerrain.Terrain.SetCellValueFast(chunk.Origin.X + x, level + 1 + y, chunk.Origin.Y + z, i);
                    }
                }
            }
            else
            {
                GenerateCacti(chunk);
            }*/
        }
        public void GenerateMoonPockets(TerrainChunk chunk)
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
                        int num19 = random.Int(40, 48); //生成一个介于15和42之间的随机数，用于确定岩浆口袋的高度位置。
                        int num20 = num2 * 16;
                        int num21 = random.Int(0, 1); //生成一个介于1和2之间的随机数，用于确定生成岩浆口袋的数量。



                        for (int num22 = 0; num22 < num21; num22++) //据上一步生成的数量，进行循环，确定生成岩浆口袋的具体位置。
                        {
                            Vector2 vector2 = random.Vector2(7f); //使用随机数生成器生成一个二维向量，向量的每个分量介于-7和7之间。
                            int num23 = 8 + (int)MathUtils.Round(vector2.X); //根据生成的二维向量，确定岩浆口袋在区块内的具体位置。
                            int num24 = random.Int(0, 1);
                            int num25 = 8 + (int)MathUtils.Round(vector2.Y); //根据随机数生成器，选择一个岩浆口袋的绘制方法，并使用给定的参数在区块内绘制岩浆口袋。
                            NewModLoaderShengcheng.m_fcMoonPocketBrushes[random.Int(0, NewModLoaderShengcheng.m_fcMoonPocketBrushes.Count - 1)].PaintFast(chunk, num18 + num23, num19 + num24, num20 + num25);
                        }
                    }
                }
            }
        }
        public void NGenerateSurfaceParameters(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            for (int i = x1; i < x2; i++)
            {
                for (int j = z1; j < z2; j++)
                {
                    int x = i + chunk.Origin.X;
                    int z = j + chunk.Origin.Y;
                    int temperature = MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + m_temperatureOffset.X, z + m_temperatureOffset.Y, 0.0015f / TGBiomeScaling, 5, 2f, 0.6f) - 1.1f + m_worldSettings.TemperatureOffset / 16f) * 16f), 12, 15);
                    int humidity = MathUtils.Clamp((int)(MathUtils.Saturate(3f * SimplexNoise.OctavedNoise(x + m_humidityOffset.X, z + m_humidityOffset.Y, 0.0012f / TGBiomeScaling, 5, 2f, 0.6f) - 0.9f + m_worldSettings.HumidityOffset / 16f) * 16f), 0, 3);
                    chunk.SetTemperatureFast(i, j, 0);//月球温度湿度都为0
                    chunk.SetHumidityFast(i, j, 0);
                }
            }
        }

        public void NGenerateTerrain(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            //定义局部变量 num 和 num2，分别计算输入参数 x1、z1、x2、z2 所定义的区域的宽度和深度。
            int num = x2 - x1;
            int num2 = z2 - z1;
            _ = m_subsystemTerrain.Terrain;
            //获取地形块原点的 x 和 y 坐标，并将它们存储在 num3 和 num4 中。
            int num3 = chunk.Origin.X + x1;
            int num4 = chunk.Origin.Y + z1;
            //创建两个 Grid2d 实例 grid2d 和 grid2d2。这些网格用于存储指定区域内每个位置的海岸线距离和山脉因子。
            Grid2d grid2d = new Grid2d(num, num2);
            Grid2d grid2d2 = new Grid2d(num, num2);
            //双层循环遍历指定区域的每个位置，并调用 CalculateOceanShoreDistance 和 CalculateMountainRangeFactor 函数来填充上面创建的两个网格。
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    grid2d.Set(j, i, CalculateOceanShoreDistance(j + num3, i + num4));
                    grid2d2.Set(j, i, CalculateMountainRangeFactor(j + num3, i + num4));
                }
            }

            //创建一个 Grid3d 实例 grid3d，用于存储每个位置的地形密度。
            Grid3d grid3d = new Grid3d(num / 4 + 1, 33, num2 / 4 + 1);
            //三层嵌套循环填充 grid3d，使用 CalculateHeight 函数计算地形高度，然后根据山脉因子和其他参数调整地形。
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int num5 = k * 4 + num3;
                    int num6 = l * 4 + num4;
                    float num7 = CalculateHeight(num5, num6);
                    float v = CalculateMountainRangeFactor(num5, num6);
                    float num8 = MathUtils.Lerp(TGMinTurbulence, 1f, Squish(v, TGTurbulenceZero, 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        int num9 = m * 8;
                        float num10 = TGTurbulenceStrength * num8 * MathUtils.Saturate(num7 - (float)num9) * (2f * SimplexNoise.OctavedNoise(num5, num9, num6, TGTurbulenceFreq, TGTurbulenceOctaves, 4f, TGTurbulencePersistence) - 1f);
                        float num11 = (float)num9 + num10;
                        float num12 = num7 - num11;
                        num12 += MathUtils.Max(4f * (TGDensityBias - (float)num9), 0f);
                        grid3d.Set(k, m, l, num12);
                    }
                }
            }
            int oceanLevel = OceanLevel;
            //四层嵌套循环遍历 grid3d 中的每个立体单元，根据网格数据和其他计算出的参数确定每个单元格的内容（例如，是空气、水、土壤还是砂岩）。
            //1-2. 外两层 for 循环定义了遍历 grid3d 网格的 x 和 z 轴。注意循环是从 0 迭代到 SizeX - 1 和 SizeZ - 1，这意味着在每个方向上都避免了最后一个单元。
            for (int n = 0; n < grid3d.SizeX - 1; n++)
            {
                for (int num13 = 0; num13 < grid3d.SizeZ - 1; num13++)
                {
                    //3-4. 内部循环遍历 y 轴。这里同样避免了最后一个单元。
                    for (int num14 = 0; num14 < grid3d.SizeY - 1; num14++)
                    {
                        //Get8 方法获取了当前位置和其相邻位置的地形密度值。这些值将用于计算地形高度的插值。
                        grid3d.Get8(n, num14, num13, out float v2, out float v3, out float v4, out float v5, out float v6, out float v7, out float v8, out float v9);
                        //6 - 9.计算沿着每个方向的密度变化，并将这些变化存储在 num15、num16、num17 和 num18 中。这些值用于在周围单元之间插值密度。
                        float num15 = (v3 - v2) / 4f;
                        float num16 = (v5 - v4) / 4f;
                        float num17 = (v7 - v6) / 4f;
                        float num18 = (v9 - v8) / 4f;
                        // 10-13. 定义了四个变量以存储当前迭代点的密度值。
                        float num19 = v2;
                        float num20 = v4;
                        float num21 = v6;
                        float num22 = v8;
                        //14-15. 第四层循环处理一个 4x4x4 的方块，分别对 x 和 z 轴进行插值以计算地形高度。
                        for (int num23 = 0; num23 < 4; num23++)
                        {
                            //计算 x 和 z 方向上的插值步长。
                            float num24 = (num21 - num19) / 4f;
                            float num25 = (num22 - num20) / 4f;

                            float num26 = num19;
                            float num27 = num20;
                            for (int num28 = 0; num28 < 4; num28++)
                            {
                                //使用插值步长，通过线性插值计算当前迭代点的地形密度。
                                float num29 = (num27 - num26) / 8f;
                                float num30 = num26;
                                //定义变量 num31 和 num32 以获得当前迭代点在 grid2d 网格中的坐标。
                                int num31 = num23 + n * 4;
                                int num32 = num28 + num13 * 4;
                                //计算块内的实际 x 和 z 轴坐标。
                                int x3 = x1 + num31;
                                int z3 = z1 + num32;
                                //24-25.使用实际坐标从 grid2d 和 grid2d2 网格中获取海岸线距离和山脉因子。
                                float x4 = grid2d.Get(num31, num32);
                                float num33 = grid2d2.Get(num31, num32);
                                //26-27. 获取当前坐标的温度和湿度。
                                int temperatureFast = chunk.GetTemperatureFast(x3, z3);
                                int humidityFast = chunk.GetHumidityFast(x3, z3);
                                //计算一个用于决定是否在当前位置生成水的因子。
                                float f = num33 - 0.01f * (float)humidityFast;
                                //29-30. 计算用于决定地形特征的阈值。
                                float num34 = MathUtils.Lerp(100f, 0f, f);
                                float num35 = MathUtils.Lerp(300f, 30f, f);
                                bool flag = (temperatureFast > 8 && humidityFast < 8 && num33 < 0.97f) || (MathUtils.Abs(x4) < 16f && num33 < 0.97f);
                                //使用 CalculateCellIndex 计算当前地形块中 x 和 z 位置的索引。
                                int num36 = TerrainChunk.CalculateCellIndex(x3, 0, z3);
                                for (int num37 = 0; num37 < 8; num37++)
                                {
                                    //内部循环处理 y 方向的插值。
                                    //计算 y 轴的当前高度。
                                    int num38 = num37 + num14 * 8;
                                    int value = 0;
                                    //根据地形密度确定方块的类型，并将其设置到地形块的相应位置。
                                    if (num30 >= 0f)
                                    {
                                        //67	BasaltBlock	玄武岩 //3	GraniteBlock	花岗岩//4	SandstoneBlock	砂岩
                                        //value = ((!flag) ? ((!(num30 < num35)) ? 960 : 960) : ((!(num30 < num34)) ? ((!(num30 < num35)) ? 960 : 960) : 960));
                                        value = 960;
                                    }
                                    chunk.SetCellValueFast(num36 + num38, value);
                                    //更新插值变量，为下一个迭代准备数据。
                                    num30 += num29;
                                }
                                //41-44. 更新外层插值变量
                                num26 += num24;
                                num27 += num25;
                            }
                            num19 += num15;
                            num20 += num16;
                            num21 += num17;
                            num22 += num18;
                        }
                    }
                }
            }
        }
    }//月球地形构筑
}//多维度世界设置（地形生成）

