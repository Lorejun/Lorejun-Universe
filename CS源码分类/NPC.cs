using Engine.Graphics;
using Engine.Media;
using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TemplatesDatabase;
using XmlUtilities;
using System.Globalization;
using Test1;
using OpenTK.Graphics;
using System.Reflection;

namespace Game
{
    public class LordButtonsPanelWidget : CanvasWidget
    {
        public static string[] stringLord = new string[5]{ "所谓的创作，是一种孤独又自命不凡的过家家。没有别人来评价的话就没有办法在舞台上散发光彩，" +
            "观众们为了接受新生的事物，就会和过去的产物进行比较。比起那位古仙温和宽松的产物，我的孩子才是毫无疑问的杰作!毋庸置疑！", "你今天去探索这个世界了吗？还是依旧停滞不前呢？别担心，无论多久，我都会在这里和你一起！" 
            ,"我说你啊，你的脑子里难道没有齿轮在转动吗？你听到那个声音了吗？","如果你想买卖属性宝珠或者灵能，尽管召唤我吧！","你觉得这个世界怎么样？很美丽，对吧？我也那么认为！毕竟是“那位”的杰作！"};
        public bool YHLZ = false;
        public bool BS = false;//召唤商人标记
        public ComponentTest1 m_componentTest1;
        public ComponentMiner m_componentMiner;
        public ComponentHealth m_componentHealth;
        string entityname1 = "洛德";
        public Random random = new Random();
        public ComponentPlayer m_componentPlayer;
        public SubsystemNPCBehavior m_subsystemNPCBehavior;
        public ComponentMoney m_componentMoney;
        public BevelledButtonWidget m_button6;
        public BevelledButtonWidget m_button7;
        public BevelledButtonWidget m_button8;
        public BevelledButtonWidget m_button9;
        public BevelledButtonWidget m_button10;
        public LordButtonsPanelWidget(ComponentMiner componentMiner, SubsystemNPCBehavior subsystemNPCBehavior,ComponentMoney componentMoney, IInventory inventory,  ComponentPlayer componentPlayer, ComponentHealth componentHealth)
        {
            m_componentMiner = componentMiner;
            m_subsystemNPCBehavior = subsystemNPCBehavior;
            m_componentMoney = componentMoney;
            m_componentHealth = componentHealth;
            m_componentPlayer = componentPlayer;
            XElement node = ContentManager.Get<XElement>("Talk/Widgets/LordButtonsPanelWidget");
            LoadContents(this, node);
           
            m_button6 = Children.Find<BevelledButtonWidget>("Button6");
            m_button7 = Children.Find<BevelledButtonWidget>("Button7");
            m_button8 = Children.Find<BevelledButtonWidget>("Button8");
            m_button9 = Children.Find<BevelledButtonWidget>("Button9");
            m_button10 = Children.Find<BevelledButtonWidget>("Button10");

        }

        public override void Update()
        {
           
            if (m_button6.IsClicked)
            {
                int num = random.Int(0, 4);
                m_subsystemNPCBehavior.OpenText(stringLord[num], entityname1);
            }
            if (m_button7.IsClicked)
            {
                m_componentPlayer.ComponentGui.ModalPanelWidget = new TradingWidget(m_componentMiner.Inventory, m_componentMoney, m_componentMiner.ComponentPlayer, m_componentHealth);
            }
            if (m_button8.IsClicked)
            {
            }

           



        }
    }
    public class DodoButtonsPanelWidget : CanvasWidget
    {
        public static string[] stringDodo = new string[5]{ "渡渡可不是笨鸟!吾可是最聪明的鸟，渡渡是也！上知天文，下知地理，无所不知！有什么疑惑尽管问吾吧！哇哈哈哈！", "渡渡一出生就生活在血泪之池，什么？你问吾血泪之池是怎么来的？em……（转三圈）你知道【鸟转三圈就会遗忘】这句格言吗？就是这个道理，你还是别问了吧！哇哈哈哈"
            ,"你要小心血泪之池那些丧尸……虽然他们不会攻击吾，但他们貌似会无差别攻击其他生物。吾不记得什么时候血泪之池变成了这个样子……不对！血泪之池一直都是这个样子！对，没错！","这个国家的人口有100亿6564万！国土有7000平方千米，总之就是非常广阔！总之就是非常巨大！","咳咳……你找吾有什么事情吗？"};
        public bool YHLZ = false;
        public bool BS = false;//召唤商人标记
        public ComponentTest1 m_componentTest1;
        public ComponentMiner m_componentMiner;
        public ComponentHealth m_componentHealth;
        string entityname1 = "愚鸟渡渡";
        public Random random = new Random();
        public ComponentPlayer m_componentPlayer;
        public SubsystemNPCBehavior m_subsystemNPCBehavior;
        public ComponentMoney m_componentMoney;
        public BevelledButtonWidget m_button6;//闲聊
        public BevelledButtonWidget m_button7;//交易
        public BevelledButtonWidget m_button8;//剧情对话
        public BevelledButtonWidget m_button9;//杀害
        public BevelledButtonWidget m_button10;//奉献灵能
        public DodoButtonsPanelWidget(ComponentMiner componentMiner, SubsystemNPCBehavior subsystemNPCBehavior, ComponentMoney componentMoney, IInventory inventory, ComponentPlayer componentPlayer, ComponentHealth componentHealth)
        {
            m_componentMiner = componentMiner;
            m_subsystemNPCBehavior = subsystemNPCBehavior;
            m_componentMoney = componentMoney;
            m_componentHealth = componentHealth;
            m_componentPlayer = componentPlayer;
            XElement node = ContentManager.Get<XElement>("Talk/Widgets/DodoButtonsPanelWidget");
            LoadContents(this, node);

            m_button6 = Children.Find<BevelledButtonWidget>("Button6");
            m_button7 = Children.Find<BevelledButtonWidget>("Button7");
            m_button8 = Children.Find<BevelledButtonWidget>("Button8");
            m_button9 = Children.Find<BevelledButtonWidget>("Button9");
            m_button10 = Children.Find<BevelledButtonWidget>("Button10");

        }

        public override void Update()
        {

            if (m_button6.IsClicked)
            {
                int num = random.Int(0, 4);
                m_subsystemNPCBehavior.OpenText(stringDodo[num], entityname1);
            }
            if (m_button7.IsClicked)
            {
                m_componentPlayer.ComponentGui.ModalPanelWidget = new TradingWidget(m_componentMiner.Inventory, m_componentMoney, m_componentMiner.ComponentPlayer, m_componentHealth);
            }
            if (m_button8.IsClicked)
            {

            }





        }
    }
    public class MCCGPurchaseWidget : CanvasWidget
    {
        public ComponentMiner m_componentMiner;
        public ComponentTest1 m_componentTest1;
        public ListPanelWidget m_listPanelWidget;
        public GridPanelWidget m_inventoryGrid;
        public BevelledButtonWidget m_changeButton;
        public BevelledButtonWidget m_purchaseButton;
        public BevelledButtonWidget m_leftButton;
        public BevelledButtonWidget m_rightButton;
        public SliderWidget m_slider;
        public BlockIconWidget m_blockIcon;
        public LabelWidget m_selectedText;
        public LabelWidget m_pFlorin;
        public LabelWidget m_price;
        public LabelWidget m_categoryDisplay;
        public BevelledButtonWidget m_mpButton;

        public int SelectedValue;
        public int m_lastSelectedValue;

        public List<string> m_categories = new List<string>();
        public int CurrentCategoryIndex;
        public string CurrentCategory
        {//当前的种类
            get
            {
                if (CurrentCategoryIndex < 0 || CurrentCategoryIndex >= m_categories.Count)
                    CurrentCategoryIndex = 0;
                return m_categories[CurrentCategoryIndex];
            }
        }
        public MCCGPurchaseWidget(ComponentMiner componentMiner)
        {
            m_componentMiner = componentMiner;
            m_componentTest1=m_componentMiner.Entity.FindComponent<ComponentTest1>();
            XElement node = ContentManager.Get<XElement>("Widgets/Systemshop/MCCGPurchaseWidget");
            LoadContents(this, node);
            m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
            m_changeButton = Children.Find<BevelledButtonWidget>("ChangeButton");
            m_purchaseButton = Children.Find<BevelledButtonWidget>("PurchaseButton");
            m_leftButton = Children.Find<BevelledButtonWidget>("Left");
            m_mpButton = Children.Find<BevelledButtonWidget>("MPButton");
            m_rightButton = Children.Find<BevelledButtonWidget>("Right");
            m_slider = Children.Find<SliderWidget>("Slider");
            m_blockIcon = Children.Find<BlockIconWidget>("Icon");
            m_selectedText = Children.Find<LabelWidget>("SelectedText");
            m_pFlorin = Children.Find<LabelWidget>("PFlorin");
            m_price = Children.Find<LabelWidget>("Price");
            m_categoryDisplay = Children.Find<LabelWidget>("Category");
            int num2 = 10;
            for (int l = 0; l < m_inventoryGrid.RowsCount; l++)
            {
                for (int i = 0; i < m_inventoryGrid.ColumnsCount; i++)
                {
                    InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(componentMiner.Inventory, num2++);
                    m_inventoryGrid.Children.Add(inventorySlotWidget);
                    m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(i, l));
                }
            }
            m_listPanelWidget = Children.Find<ListPanelWidget>("List");
            m_listPanelWidget.ItemWidgetFactory = delegate (object item)
            {
                int value = (int)item;
                int id = Terrain.ExtractContents(value), data = Terrain.ExtractData(value);
                XElement node2 = ContentManager.Get<XElement>("Widgets/RecipaediaItem");
                ContainerWidget containerWidget = (ContainerWidget)LoadWidget(this, node2, null);
                BlockIconWidget blockIconWidget = containerWidget.Children.Find<BlockIconWidget>("RecipaediaItem.Icon", true);
                blockIconWidget.Value = value;
                blockIconWidget.Size = new Vector2(60f, 60f);
                LabelWidget text = containerWidget.Children.Find<LabelWidget>("RecipaediaItem.Text", true);
                LabelWidget details = containerWidget.Children.Find<LabelWidget>("RecipaediaItem.Details", true);

                text.Text = MoneyManager.GetBlockDisplayName(value);
                text.FontScale = 0.6f;
                text.DropShadow = true;
                details.Text = string.Format("单价:{0} 出售:{1}" + MoneyManager.CoinChar, MoneyManager.GetItemMoney(value),MoneyManager.GetReturnMoney(value));
                details.FontScale = 0.5f;
                details.DropShadow = true;
                details.Color = Color.Yellow;
                //details.Ellipsis = true;
                return containerWidget;
            };

            CurrentCategoryIndex = 0;
            m_categories.Add("全部");//初始化种类列表
            foreach (var item in MoneyManager.m_recipes)
            {//遍历所有商品信息，若该商品的种类没有添加至种类列表，将其添加
                string category = BlocksManager.Blocks[Terrain.ExtractContents(item.ResultValue)].GetCategory(item.ResultValue);
                if (!m_categories.Contains(category))
                    m_categories.Add(category);
            }
            UpdateList();
        }

        public override void Update()
        {
            if (!m_componentMiner.IsAddedToProject || m_componentMiner.ComponentCreature.ComponentHealth.Health <= 0)
            {
                base.ParentWidget.Children.Remove(this);
            }
            if(m_mpButton.IsClicked)
            {
                if(m_componentTest1!=null)
                {
                    float mp = m_componentTest1.m_mp * m_componentTest1.m_Maxmp;
                    
                    if(m_componentTest1.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative)
                    {
                        m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("提取灵能成功！", Color.White, true, false);
                        MoneyManager.AddItem(m_componentMiner, m_componentMiner.Project.FindSubsystem<SubsystemPickables>(), 987, 1000);
                    }
                    else if (mp < 1000)
                    {
                        m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("提取失败，至少需要1000灵能才能提取！", Color.White, true, false);
                    }
                    else if (mp >= 1000)
                    {
                        m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("提取灵能成功！", Color.White, true, false);
                        m_componentTest1.m_mp -= 1000 / m_componentTest1.m_Maxmp;
                        MoneyManager.AddItem(m_componentMiner, m_componentMiner.Project.FindSubsystem<SubsystemPickables>(), 987, 1000);
                    }
                }
            }
            if (m_listPanelWidget.SelectedItem != null)
                SelectedValue = (int)m_listPanelWidget.SelectedItem;
            else
                SelectedValue = 0;
            if (m_changeButton.IsClicked)
                m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MCCGSellWidget(m_componentMiner);
            m_slider.MaxValue = SelectedValue <= 0 ? 10 : BlocksManager.Blocks[Terrain.ExtractContents(SelectedValue)].GetMaxStacking(SelectedValue);
            if (m_lastSelectedValue != SelectedValue)
            {
                m_slider.Value = 1;
            }
            m_lastSelectedValue = SelectedValue;
            int coins = MoneyManager.GetItemCountInInventory(MoneyManager.CoinValue, m_componentMiner.Inventory);//玩家持有的货币数量
            if (SelectedValue > 0)
            {
                int price = (int)m_slider.Value * (int)(MoneyManager.GetItemMoney(SelectedValue));
                m_slider.Text = m_slider.Value.ToString();
                m_blockIcon.Value = SelectedValue;
                m_selectedText.Text = string.Format("{0}x{1}", MoneyManager.GetBlockDisplayName(SelectedValue), (int)m_slider.Value);
                m_purchaseButton.Color = Color.White;
                m_price.Text = "-" + price.ToString() + MoneyManager.CoinChar;
                if (m_purchaseButton.IsClicked)
                {
                    if (coins >= price)
                    {
                        m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format("已购买{0}个{1}，失去{2}{3}", m_slider.Value, MoneyManager.GetBlockDisplayName(SelectedValue), price, MoneyManager.CoinChar), Color.White, true, false);
                        MoneyManager.RemoveItemFromInventory(MoneyManager.CoinValue, price, m_componentMiner.Inventory);
                        MoneyManager.AddItem(m_componentMiner, m_componentMiner.Project.FindSubsystem<SubsystemPickables>(), SelectedValue, (int)m_slider.Value);
                    }
                    else
                        m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("你的" + MoneyManager.CoinChar + "不够了", Color.White, true, false);
                }
            }
            else
            {
                m_slider.Text = "未选择";
                m_blockIcon.Value = 0;
                m_selectedText.Text = string.Empty;
                m_purchaseButton.Color = Color.Gray;
                m_price.Text = string.Empty;//

            }
            m_pFlorin.Text = string.Format("你的余额:{0}{1}", coins, MoneyManager.CoinChar);
            m_categoryDisplay.Text = LanguageControl.Get("BlocksManager", CurrentCategory);//翻译种类名
            m_leftButton.IsEnabled = CurrentCategoryIndex > 0;
            m_rightButton.IsEnabled = CurrentCategoryIndex < m_categories.Count - 1;
            if (m_leftButton.IsEnabled && m_leftButton.IsClicked)
            {
                CurrentCategoryIndex = MathUtils.Max(0, CurrentCategoryIndex - 1);
                UpdateList();
            }
            if (m_rightButton.IsEnabled && m_rightButton.IsClicked)
            {
                CurrentCategoryIndex = MathUtils.Min(m_categories.Count - 1, CurrentCategoryIndex + 1);
                UpdateList();
            }
        }

        public void UpdateList()
        {//更新商品列表
            m_slider.Value = 1;
            m_listPanelWidget.ClearItems();
            m_listPanelWidget.ScrollPosition = 0f;
            foreach (var item in MoneyManager.m_recipes)
            {
                string category = BlocksManager.Blocks[Terrain.ExtractContents(item.ResultValue)].GetCategory(item.ResultValue);
                if (CurrentCategory == "全部" || CurrentCategory == category)
                    m_listPanelWidget.AddItem(item.ResultValue);
            }
        }

    }

    public class MCCGSellWidget : CanvasWidget
    {
        public ComponentMiner m_componentMiner;
        public Test1.ComponentTest1 m_componentTest1;

        public GridPanelWidget m_inventoryGrid;
        public InventorySlotWidget m_sellSlot;
        public BevelledButtonWidget m_changeButton;
        public BevelledButtonWidget m_sellButton;
        public LabelWidget m_price;

        public MCCGSellWidget(ComponentMiner componentMiner)
        {
            m_componentMiner = componentMiner;
            m_componentTest1 = m_componentMiner.Entity.FindComponent<Test1.ComponentTest1>();
            XElement node = ContentManager.Get<XElement>("Widgets/Systemshop/MCCGSellWidget");
            LoadContents(this, node);
            m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
            m_sellSlot = Children.Find<InventorySlotWidget>("SellSlot");
            m_changeButton = Children.Find<BevelledButtonWidget>("ChangeButton");
            m_sellButton = Children.Find<BevelledButtonWidget>("SellButton");
            m_price = Children.Find<LabelWidget>("Price");
            int num2 = 10;
            for (int l = 0; l < m_inventoryGrid.RowsCount; l++)
            {
                for (int i = 0; i < m_inventoryGrid.ColumnsCount; i++)
                {
                    InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(componentMiner.Inventory, num2++);
                    m_inventoryGrid.Children.Add(inventorySlotWidget);
                    m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(i, l));
                }
            }
            m_sellSlot.AssignInventorySlot(m_componentTest1, 0);
        }

        public override void Update()
        {
            if (!m_componentMiner.IsAddedToProject || m_componentMiner.ComponentCreature.ComponentHealth.Health <= 0)
            {
                base.ParentWidget.Children.Remove(this);
            }
            if (m_changeButton.IsClicked)
                m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MCCGPurchaseWidget(m_componentMiner);
            int value = m_componentTest1.GetSlotValue(0), id = Terrain.ExtractContents(value), data = Terrain.ExtractData(value),
                price = 0, count = m_componentTest1.GetSlotCount(0);
            if (count > 0)
            {
                price = MoneyManager.GetReturnMoney(value) * count;
            }
            if (price > 0)
            {
                m_price.Text = string.Format("+{0}{1}", price, MoneyManager.CoinChar);
                m_sellButton.Color = Color.White;
                if (m_sellButton.IsClicked)
                {
                    m_componentTest1.RemoveSlotItems(0, count);
                    MoneyManager.AddItem(m_componentMiner, m_componentMiner.Project.FindSubsystem<SubsystemPickables>(), MoneyManager.CoinValue, price);
                    m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format("已出售{0}个{1}，获得{2}{3}", count, MoneyManager.GetBlockDisplayName(value), price, MoneyManager.CoinChar), Color.White, true, false);
                }
            }
            else
            {
                m_price.Text = string.Empty;
                m_sellButton.Color = Color.Gray;
                if (count > 0 && m_sellButton.IsClicked)
                    m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("这个物品无法出售", Color.White, true, false);
            }
        }

    }

    #region 交易


    public class SubsystemNPCBehavior : SubsystemBlockBehavior, IUpdateable
    {
        
        
        public SubsystemParticles m_subsystemParticles;

        public Random m_random = new Random();
        private ContainerWidget m_screenLabelCanvasWidget;
        public ComponentPlayer m_componentPlayer;
        public SubsystemPlayers m_subsystemPlayers;
        private float m_screenLabelCloseTime;
        public bool firsttime = true;
        public bool firstSpawn ;

        public override int[] HandledBlocks => new int[1]
        {
            0
        };
        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }

        
        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            //这是一个重写的方法，当玩家使用（如点击或交互）一个方块时被调用。它接收一个射线 Ray3 和一个 ComponentMiner 对象，后者包含玩家的挖掘和交互组件信息。
            IInventory inventory = componentMiner.Inventory;
            //获取 componentMiner 的 Inventory 属性，这是玩家的库存接口。
            int activeBlockValue = componentMiner.ActiveBlockValue;
            //获取玩家当前正在使用的方块的值。
            int num = Terrain.ExtractContents(activeBlockValue);
           // ComponentPlayer m_componentPlayer =componentMiner.Entity.FindComponent<ComponentPlayer>();
            //从玩家当前激活的方块值中提取内容，得到内容的id。
            if (num == 0)
            {
                object obj = componentMiner.Raycast(ray, RaycastMode.Gathering);
                //使用 Raycast 方法来检测玩家瞄准的对象，使用 Gathering 模式来收集信息。
                if (obj is BodyRaycastResult)
                {
                    //检查射线投射的结果是否为 BodyRaycastResult 类型，这意味着射线命中了一个实体的身体（比如NPC或玩家）。
                    ComponentMoney componentMoney = ((BodyRaycastResult)obj).ComponentBody.Entity.FindComponent<ComponentMoney>();
                    //如果射线击中了一个实体，尝试从该实体中查找 ComponentMoney 和 ComponentHealth 组件。
                    ComponentHealth componentHealth = ((BodyRaycastResult)obj).ComponentBody.Entity.FindComponent<ComponentHealth>();
                    ComponententityLord m_entitylord = ((BodyRaycastResult)obj).ComponentBody.Entity.FindComponent<ComponententityLord>();
                    ComponententityDodo m_entitydodo = ((BodyRaycastResult)obj).ComponentBody.Entity.FindComponent<ComponententityDodo>();
                    if (componentHealth.Health > 0f)
                    {
                        //如果被击中实体的健康值大于0，即实体还活着
                        if (componentMoney != null&&m_entitylord!=null)
                        {
                            //如果找到了 ComponentMoney 组件。
                            //创建一个新的 TradingWidget 实例，这是交易界面的一个组件，传入玩家的库存、找到的金钱组件、玩家组件以及健康组件，并将其设置为玩家的模态面板组件。返回 true 表示使用行为已处理。
                            //note type:onlyread text:title size:100,100 vec2:0,0 color1:255,255,255 v1:10 color2:255,255,255 color3:255,255,255 v2:10
                            //OpenText();
                            componentMiner.Entity.FindComponent<ComponentTask>().Lordtask1 = true;
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new LordButtonsPanelWidget(componentMiner,this, componentMoney, componentMiner.Inventory,  componentMiner.ComponentPlayer, componentHealth);

                            return true;
                        }
                        else if(componentMoney != null && m_entitydodo != null)
                        {
                            componentMiner.Entity.FindComponent<ComponentTask>().Dodo1 = true;
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new DodoButtonsPanelWidget(componentMiner, this, componentMoney, componentMiner.Inventory, componentMiner.ComponentPlayer, componentHealth);

                            return true;
                        }
                        
                    }
                }
            }
            return false;
        }
        public void Update(float dt)
        {
            if(firsttime==true&&m_componentPlayer!=null)
            {
                string obj = "bslord";
                ErgodicBody(obj, delegate (ComponentBody body)
                {
                    Project.RemoveEntity(body.Entity, true);
                    return false;
                });


                
                firsttime = false;
            }
            if(m_componentPlayer!=null )
            {
                if(m_componentPlayer.Entity.FindComponent<ComponentTest1>().Areaname == "血泪之池"&&firstSpawn==true)
                {
                    Point3 pointplayer = m_componentPlayer.Entity.FindComponent<ComponentTest1>().point1;
                    double distance1 = Math.Sqrt(((Math.Abs(1235 - pointplayer.X)) * (Math.Abs(1235 - pointplayer.X))) + ((Math.Abs(1245- pointplayer.Z)) * (Math.Abs(1245 - pointplayer.Z))));
                    int distance_int = (int)distance1;//计算路径点目标位置与玩家真实距离。
                    //double distance2 = Math.Sqrt(((Math.Abs(chunkX - point.X)) * (Math.Abs(chunkX - point.X))) + ((Math.Abs(chunkY - point.Z)) * (Math.Abs(chunkY - point.Z))));
                    //int distance_int2 = (int)distance2;//计算目标真实区块与玩家真实距离。
                    int visiblerange = SettingsManager.VisibilityRange;//视距
                    if(distance_int<60)
                    {
                        if (firstSpawn == true)
                        {
                            Point3 pos = (1233, 71, 1243);
                            Entity entity = DatabaseManager.CreateEntity(Project, "DoDo", true);
                            ComponentFrame componentFrame = entity.FindComponent<ComponentFrame>();
                            ComponentSpawn componentSpawn = entity.FindComponent<ComponentSpawn>();
                            if (componentFrame != null && componentSpawn != null)
                            {
                                componentFrame.Position = new Vector3(pos) + new Vector3(0.5f, 0f, 0.5f);
                                componentFrame.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (new Random()).Float(0f, (float)MathUtils.PI * 2f));
                                componentSpawn.SpawnDuration = 0f;
                                Project.AddEntity(entity);
                            }
                            firstSpawn = false;
                        }
                    }
                    
                }
                
            }
            

            if (this.m_screenLabelCloseTime > 0f)
            {
                this.m_screenLabelCloseTime -= dt;
                if (this.m_screenLabelCloseTime < 0.1f)
                {
                    this.m_screenLabelCanvasWidget.IsVisible = false;
                    this.m_screenLabelCloseTime = 0f;
                }
            }
        }
        public override void Save(ValuesDictionary valuesDictionary)
        {
            base.Save(valuesDictionary);
            valuesDictionary.SetValue<bool>("FS", firstSpawn);
        }
        public SubsystemBodies m_subsystemBodies;
        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(throwOnError: true);
            m_subsystemPlayers= base.Project.FindSubsystem<SubsystemPlayers>(throwOnError: true);
            m_subsystemBodies= base.Project.FindSubsystem<SubsystemBodies>(throwOnError: true);
            firsttime = valuesDictionary.GetValue<bool>("FT", true);
            firstSpawn = valuesDictionary.GetValue<bool>("FS", true);

        }

        public override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);
            ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
                
            {
                m_componentPlayer = componentPlayer;

            }
            if (entity.FindComponent<ComponentPlayer>() != null)
            {
                if (m_componentPlayer != null)
                {
                    InitScreenLabelCanvas();
                    
                    
                    
                }
                
            }
            
            
        }
        public void InitScreenLabelCanvas()
        {
            
            XElement node = ContentManager.Get<XElement>("Widgets/TextLabelWidget");
            m_screenLabelCanvasWidget = (ContainerWidget)Widget.LoadWidget(m_componentPlayer.GameWidget, node, null);
            m_screenLabelCanvasWidget.IsVisible = false;
            m_componentPlayer.GameWidget.Children.Add(m_screenLabelCanvasWidget);
        }

        public void OpenText(string Text1,string entityname)
        {
            Vector2 vec2 = (-330f, -150f);
            m_screenLabelCanvasWidget.IsVisible = true;
            m_screenLabelCloseTime = 8f;
            Vector2 canvasSize = (300f, 150f);
            CanvasWidget canvasWidget = (CanvasWidget)m_screenLabelCanvasWidget;
            canvasWidget.Size = canvasSize;
            int mx = (int)(m_componentPlayer.GameWidget.ActualSize.X / 2) - (int)(canvasSize.X / 2);
            int my = (int)(m_componentPlayer.GameWidget.ActualSize.Y / 2) - (int)(canvasSize.Y / 2);
            m_componentPlayer.GameWidget.SetWidgetPosition(m_screenLabelCanvasWidget, new Vector2(mx, my) + vec2);
            LabelWidget labelWidget = m_screenLabelCanvasWidget.Children.Find<LabelWidget>("Content");
            //labelWidget.Text = value.Replace("[n]", "\n").Replace("\t", "");
            labelWidget.Text = entityname+":"+Text1;
            labelWidget.Color = new Color(255, 255, 255);
            labelWidget.FontScale = 1f;
            RectangleWidget rectangleWidget = m_screenLabelCanvasWidget.Children.Find<RectangleWidget>("ScreenLabelRectangle");
            rectangleWidget.FillColor = new Color(20,20,20,130);
            rectangleWidget.OutlineColor = Color.DarkRed;
            rectangleWidget.OutlineThickness = 40 / 10f;
        }
        public bool ErgodicBody(string target, Func<ComponentBody, bool> action)
        {
            target = target.ToLower();
            Vector2 playerPos = new Vector2(m_componentPlayer.ComponentBody.Position.X, m_componentPlayer.ComponentBody.Position.Z);
            DynamicArray<ComponentBody> targetArray = new DynamicArray<ComponentBody>();
            m_subsystemBodies.FindBodiesInArea(playerPos - new Vector2(64f), playerPos + new Vector2(64f), targetArray);
            foreach (ComponentBody body in targetArray)
            {
                if (target == "player")
                {
                    ComponentPlayer componentPlayer = body.Entity.FindComponent<ComponentPlayer>();
                    if (componentPlayer != null)
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == "boat")
                {
                    ComponentBoat componentBoat = body.Entity.FindComponent<ComponentBoat>();
                    if (componentBoat != null)
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == "creature")
                {
                    ComponentPlayer componentPlayer = body.Entity.FindComponent<ComponentPlayer>();
                    ComponentCreature componentCreature = body.Entity.FindComponent<ComponentCreature>();
                    if (componentPlayer == null && componentCreature != null)
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == "vehicle")
                {
                    ComponentDamage componentDamage = body.Entity.FindComponent<ComponentDamage>();
                    if (componentDamage != null)
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == "npc")
                {
                    ComponentPlayer componentPlayer = body.Entity.FindComponent<ComponentPlayer>();
                    ComponentCreature componentCreature = body.Entity.FindComponent<ComponentCreature>();
                    ComponentDamage componentDamage = body.Entity.FindComponent<ComponentDamage>();
                    if (componentPlayer == null && (componentCreature != null || componentDamage != null))
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == "all")
                {
                    ComponentPlayer componentPlayer = body.Entity.FindComponent<ComponentPlayer>();
                    ComponentCreature componentCreature = body.Entity.FindComponent<ComponentCreature>();
                    ComponentDamage componentDamage = body.Entity.FindComponent<ComponentDamage>();
                    if (componentPlayer != null || componentCreature != null || componentDamage != null)
                    {
                        bool result = action.Invoke(body);
                        if (result) return true;
                    }
                }
                else if (target == body.Entity.ValuesDictionary.DatabaseObject.Name.ToLower())
                {
                    bool result = action.Invoke(body);
                    if (result) return true;
                }
            }
            return false;
        }
    }
    #region 生物管理器
    //生物实体信息
   
    #endregion
    public class TradingWidget : CanvasWidget
    {
        public ComponentMoney m_componentInventory;

        public ComponentHealth m_componentHealth;

        public ComponentPlayer m_componentPlayer;

        public InventorySlotWidget m_craftingRemainsSlot;

        public GridPanelWidget m_inventoryGrid;

        public GridPanelWidget m_chestGrid;

        public ButtonWidget m_itemsButton;

        public ButtonWidget m_shopButton;

        public ButtonWidget m_moneyButton;

        public BlockIconWidget m_itemvalueicon;

        public ButtonWidget m_prevRecipeButton;

        public ButtonWidget m_nextRecipeButton;

        public SliderWidget m_countSlider;

        public Money m_money;

        public int itemvalue = 0;

        public int itemcount = 0;

        public TradingWidget(IInventory inventory, ComponentMoney componentInventory, ComponentPlayer componentPlayer, ComponentHealth componentHealth)
        {
            //接收玩家的物品库存 inventory、财务组件 componentInventory、玩家组件 componentPlayer 和健康组件 componentHealth 作为参数。加载一个名为 “Widgets/TradingWidget” 的XML配置文件，并初始化所有的界面控件。
            //控件包括库存格子、购物篮、购买、出售、选择物品的按钮等，并为这些控件分配相应的库存槽或值。这个方法通过循环创建并添加库存槽控件到库存网格和购物篮网格中。
            m_componentInventory = componentInventory;
            m_componentHealth = componentHealth;
            m_componentPlayer = componentPlayer;
            XElement node = ContentManager.Get<XElement>("Widgets/TradingWidget");
            LoadContents(this, node);
            m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
            m_chestGrid = Children.Find<GridPanelWidget>("ChestGrid");
            m_craftingRemainsSlot = Children.Find<InventorySlotWidget>("CraftingRemainsSlot");
            m_itemsButton = Children.Find<ButtonWidget>("ItemsButton");
            m_shopButton = Children.Find<ButtonWidget>("ShopButton");
            m_moneyButton = Children.Find<ButtonWidget>("MoneyButton");
            m_itemvalueicon = Children.Find<BlockIconWidget>("Icon");
            m_countSlider = Children.Find<SliderWidget>("CountSlider");
            m_prevRecipeButton = Children.Find<ButtonWidget>("PreviousRecipe");
            m_nextRecipeButton = Children.Find<ButtonWidget>("NextRecipe");
            m_countSlider.MinValue = 0f;
            m_countSlider.MaxValue = 40f;
            m_countSlider.Granularity = 1f;
            m_countSlider.Value = itemcount;
            int num = 0;
            for (int i = 0; i < m_chestGrid.RowsCount; i++)
            {
                for (int j = 0; j < m_chestGrid.ColumnsCount; j++)
                {
                    var inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(componentInventory, num++);
                    m_chestGrid.Children.Add(inventorySlotWidget);
                    m_chestGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
                }
            }
            num = 10;
            for (int k = 0; k < m_inventoryGrid.RowsCount; k++)
            {
                for (int l = 0; l < m_inventoryGrid.ColumnsCount; l++)
                {
                    var inventorySlotWidget2 = new InventorySlotWidget();
                    inventorySlotWidget2.AssignInventorySlot(inventory, num++);
                    m_inventoryGrid.Children.Add(inventorySlotWidget2);
                    m_inventoryGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
                }
            }
            m_craftingRemainsSlot.AssignInventorySlot(componentInventory, componentInventory.RemainsSlotIndex);
        }

        public override void Update()
        {
            //该方法首先检查玩家的健康值，如果健康值小于等于0，那么关闭交易界面。
            //然后更新物品数量的滑动条的文本，显示当前选择购买的物品数量。
            //使用 Clamp 方法确保滑块的值在0到40之间。这个值代表玩家选择购买的物品数量。
            int Count = MathUtils.Clamp((int)m_countSlider.Value, 0, 40);

            //如果玩家的健康值小于或等于0，即玩家已经死亡，则从父控件中移除交易界面，关闭交易窗口。
            if (m_componentHealth.Health <= 0f)
            {
                ParentWidget.Children.Remove(this);
            }

            //更新滑块控件的文本，显示当前选择的物品数量。
            m_countSlider.Text = "数量:" + Count;

            //如果物品库存没有添加到游戏项目中，则关闭交易界面。
            if (!m_componentInventory.IsAddedToProject)
            {
                ParentWidget.Children.Remove(this);
            }

            if (m_itemsButton.IsClicked)
            {
                //当玩家点击选择物品的按钮 m_itemsButton 时，显示一个 MoneyEditDialog 对话框，让玩家选择要购买的物品。并根据选择的物品更新物品图标和物品的库存值。
                //如果玩家点击了物品选择按钮，弹出 MoneyEditDialog 对话框，让玩家选择想要交易的物品。用户在对话框中的选择将通过委托回调设置 itemvalue。
                DialogsManager.ShowDialog(null, new MoneyEditDialog(delegate (int value)
                {
                    itemvalue = value;
                }));
            }
            m_itemvalueicon.Value = itemvalue;

            //以下代码获取和检查组件的库存槽位的值和数量，进行必要的清空或更新操作。
            int num = m_componentInventory.m_slots[0].Value;//货币位置
            int num2 = m_componentInventory.m_slots[0].Count;//货币数量
            int num3 = m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Value;//方块
            int num4 = m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Count;//数量
            //如果“剩余”槽位中物品的数量为0且值不为0，则将该槽位的值清零。
            if (num4 <= 0 && num3 != 0)
            {
                m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Value = 0;
            }

            //如果购买按钮被点击，遍历所有货币配方，检查是否可以进行交易。
            if (m_shopButton.IsClicked)
            {
                foreach (Money money in MoneyManager.m_recipes)
                {
                    if (itemvalue == money.ResultValue)
                    {
                        if (num == 987)
                        {
                            if (num3 == 0 || num3 == money.ResultValue)
                            {
                                if (num2 >= Count * money.itemMoney)//如果货币数量>=物品数量（滑块数量）x物品钱
                                {
                                    m_componentInventory.m_slots[0].Count -= money.itemMoney * Count;
                                    m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Value = money.ResultValue;
                                    m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Count += Count;
                                }
                                else
                                {
                                    m_componentPlayer.ComponentGui.DisplaySmallMessage("金额不足", Color.White, blinking: true, playNotificationSound: true);
                                }
                            }
                            else
                            {
                                m_componentPlayer.ComponentGui.DisplaySmallMessage("输出的糟出现了问题", Color.White, blinking: true, playNotificationSound: true);
                            }
                        }
                        else
                        {
                            m_componentPlayer.ComponentGui.DisplaySmallMessage("货币无效", Color.White, blinking: true, playNotificationSound: true);
                        }
                    }
                }
            }
            //如果交易成功，更新库存槽和交易栏的物品数量和值；如果失败，则显示错误信息。
            m_prevRecipeButton.IsEnabled = (m_countSlider.Value > 0);
            m_nextRecipeButton.IsEnabled = (m_countSlider.Value < 40);

            //设置前一个配方和后一个配方按钮的可用状态，基于当前滑块的值。
            if (m_prevRecipeButton.IsClicked)
            {
                m_countSlider.Value--;
            }
            if (m_nextRecipeButton.IsClicked)
            {
                m_countSlider.Value++;
            }
            if (m_moneyButton.IsClicked)
            {
                //如果出售按钮被点击，执行出售逻辑。
                foreach (Money money2 in MoneyManager.m_recipes)
                {
                    //出售逻辑会检查是否可以将物品换成货币，如果可以则进行转换；如果不可以，则显示错误信息。
                    if (num == money2.ResultValue)
                    {
                        if (num3 == 0 || num3 == 987)
                        {
                            m_componentInventory.m_slots[0].Count = 0;
                            m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Value = 987;
                            m_componentInventory.m_slots[m_componentInventory.RemainsSlotIndex].Count += num2 * money2.returnMoney;
                        }
                        else
                        {
                            m_componentPlayer.ComponentGui.DisplaySmallMessage("输出的糟出现了问题", Color.White, blinking: true, playNotificationSound: true);
                        }
                    }
                }
            }
        }
    }

    public class MoneyEditDialog : Dialog
    {
        public ButtonWidget m_okButton;

        public ListPanelWidget m_itemsList;

        public Action<int> m_handler;

        public int? value;//返回的选择商品值，不确定有没有选择，所以说打问号

        public MoneyEditDialog(Action<int> handler)
        {
            XElement node = ContentManager.Get<XElement>("Dialogs/MoneyEditDialog");
            LoadContents(this, node);
            m_okButton = Children.Find<ButtonWidget>("OkButton");
            m_itemsList = Children.Find<ListPanelWidget>("ItemsList");
            m_handler = handler;
            m_itemsList.ItemWidgetFactory = delegate (object item)
            {
                var money2 = (Money)item;
                int num = Terrain.ExtractContents(money2.ResultValue);
                Block block = BlocksManager.Blocks[num];
                XElement node2 = ContentManager.Get<XElement>("Widgets/MoneyItem");
                var obj = (ContainerWidget)LoadWidget(this, node2, null);
                obj.Children.Find<BlockIconWidget>("Icon").Value = money2.ResultValue;
                obj.Children.Find<LabelWidget>("Name").Text = "商品名称:" + block.GetDisplayName(null, money2.ResultValue);
                obj.Children.Find<LabelWidget>("Money").Text = "商品价格:" + money2.itemMoney + "   售价:" + money2.returnMoney;
                return obj;
            };
            m_itemsList.ItemClicked += delegate (object item)
            {
                var money3 = (Money)item;
                value = money3.ResultValue;
            };
            foreach (Money money in MoneyManager.m_recipes)
            {
                m_itemsList.AddItem(money);
            }
        }

        public override void Update()
        {
            if (m_okButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
                if (m_handler != null && value.HasValue)
                {
                    m_handler(value.Value);
                }
            }
        }
    }

    public class ComponentMoney : ComponentInventoryBase//展示玩家背包
    {
        public int RemainsSlotIndex => SlotsCount - 1;

        public override int GetSlotCapacity(int slotIndex, int value)
        {
            if (slotIndex == RemainsSlotIndex)
            {
                return 0;
            }
            return base.GetSlotCapacity(slotIndex, value);
        }
    }

    public class Money
    {
        public int ResultValue;//物品id

        public int itemMoney;//物品价格

        public int returnMoney;//回收价
    }

    public static class MoneyManager//这个是开局把所有商品的都计入m_recipes 中，其中列表的每一个元素都是money类
    {
        public static List<Money> m_recipes = new List<Money>();

        public static int CoinValue => 987;
        public static string CoinChar => GetBlockDisplayName(CoinValue);
        public static void Initialize()
        {
            foreach (XElement item in ContentManager.Get<XElement>("Money").Descendants("Money"))
            {
                Money craftingRecipe = new Money();
                string attributeValue = XmlUtils.GetAttributeValue<string>(item, "Result");
                craftingRecipe.ResultValue = DecodeResult(attributeValue);
                craftingRecipe.itemMoney = XmlUtils.GetAttributeValue<int>(item, "Money");
                craftingRecipe.returnMoney = XmlUtils.GetAttributeValue<int>(item, "ReturnMoney");
                m_recipes.Add(craftingRecipe);
            }
        }

        public static int DecodeResult(string result)
        {
            string[] array = result.Split((new char[1] { ':' }));
            Block block = BlocksManager.FindBlockByTypeName(array[0], true);
            return Terrain.MakeBlockValue(data: (array.Length >= 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0, contents: block.BlockIndex, light: 0);
        }

        public static int GetItemMoney(int value)
        {//获取物品购买价格
            foreach (var item in m_recipes)
            {
                if (item.ResultValue == value)
                    return item.itemMoney;
            }
            return -1;
        }
        public static int GetReturnMoney(int value)
        {//获取物品出售价格
            foreach (var item in m_recipes)
            {
                if (item.ResultValue == value)
                    return item.returnMoney;
            }
            return -1;
        }

        public static string GetBlockDisplayName(int value)
        {//获取物品显示的名称
            int id = Terrain.ExtractContents(value);
            return BlocksManager.Blocks[id].GetDisplayName(null, value);
        }

        public static int GetItemCountInInventory(int value, IInventory inventory)
        {//获取物品在背包内的总数量
            int count = 0;
            for (int i = 0; i < inventory.SlotsCount; i++)
            {
                if (inventory.GetSlotValue(i) == value)
                    count += inventory.GetSlotCount(i);
            }
            return count;
        }

        public static void AddItem(ComponentMiner componentMiner, SubsystemPickables subsystemPickables, int value, int count)
        {//向背包中添加一定数量的某种物品，若没有足够空间则以掉落物形式添加
            IInventory inventory = componentMiner.Inventory;
            for (int i = 0; i < count; i++)
            {
                if (ComponentInventoryBase.FindAcquireSlotForItem(inventory, value) >= 0)
                {
                    ComponentInventoryBase.AcquireItems(inventory, value, 1);
                }
                else
                {
                    subsystemPickables.AddPickable(value, 1, componentMiner.ComponentCreature.ComponentBody.Position + new Vector3(0, 1, 0), null, null);
                }
            }
        }

        public static void RemoveItemFromInventory(int value, int count, IInventory inventory)
        {//从背包中移除一定数量的某种物品
            for (int i = 0; i < inventory.SlotsCount; i++)
            {
                if (count <= 0)
                    break;
                if (inventory.GetSlotValue(i) == value)
                {
                    int min = MathUtils.Min(count, inventory.GetSlotCount(i));
                    count -= min;
                    inventory.RemoveSlotItems(i, min);
                }
            }
        }
    }
    #endregion
    public class GamePlayerDataInfo
    {
        public class StatAttribute : Attribute
        {
        }

        public Dictionary<string, Task> m_othertasks = new Dictionary<string, Task>();

        [Stat]
        public int Money;

        [Stat]
        public bool ShopVip;

        public IEnumerable<FieldInfo> GamePlayerDatas
        {
            get
            {
                foreach (FieldInfo item in from f in typeof(GamePlayerDataInfo).GetRuntimeFields()
                                           where f.GetCustomAttribute<StatAttribute>() != null
                                           select f)
                {
                    yield return item;
                }
            }
        }

        public void Load(ValuesDictionary valuesDictionary)
        {
            foreach (FieldInfo stat in GamePlayerDatas)
            {
                if (valuesDictionary.ContainsKey(stat.Name))
                {
                    object value = valuesDictionary.GetValue<object>(stat.Name);
                    stat.SetValue(this, value);
                }
            }
            var valuesDictionary2 = valuesDictionary.GetValue("OtherTasks", new ValuesDictionary());
            foreach (var item in valuesDictionary2)
            {
                var task = SubsystemTask.GetTask(item.Key, TaskType.Other);
                task.Conditions = (bool)item.Value;
                m_othertasks.Add(item.Key, task);
            }
        }

        public void Save(ValuesDictionary valuesDictionary)
        {
            foreach (FieldInfo stat in GamePlayerDatas)
            {
                object value = stat.GetValue(this);
                valuesDictionary.SetValue(stat.Name, value);
            }
            ValuesDictionary valuesDictionary2 = new ValuesDictionary();
            valuesDictionary.SetValue("OtherTasks", valuesDictionary2);
            foreach (var task in m_othertasks)
            {
                valuesDictionary2.SetValue(task.Key, task.Value.Conditions);
            }
        }
    }
    public class SubsystemWorldSystem : Subsystem, IDrawable
    {
        public readonly PrimitivesRenderer3D m_primitivesRenderer3D = new PrimitivesRenderer3D();

        public SubsystemBodies m_subsystemBodies;

        public SubsystemPickables m_subsystemPickables;

        public Dictionary<int, GamePlayerDataInfo> m_playerinfo = new Dictionary<int, GamePlayerDataInfo>();

        public GamePlayerDataInfo GetPlayerInfo(int playerIndex)
        {
            if (!m_playerinfo.TryGetValue(playerIndex, out GamePlayerDataInfo value))
            {
                value = new GamePlayerDataInfo();
                m_playerinfo.Add(playerIndex, value);
            }
            return value;
        }

        public static int[] m_drawOrders = new int[1]
        {
            200
        };

        public int[] DrawOrders => m_drawOrders;

        public void Draw(Camera camera, int drawOrder)
        {
            if (ModItemManger.CreaterMode)
            {
                try
                {
                    foreach (var componentBody in m_subsystemBodies.m_componentBodies)
                    {
                        FlatBatch3D flatBatch3D = m_primitivesRenderer3D.FlatBatch();
                        flatBatch3D.QueueBoundingBox(componentBody.BoundingBox, ModItemManger.MainUiColor);
                        m_primitivesRenderer3D.Flush(camera.ViewProjectionMatrix);
                    }
                }
                catch (Exception e) { Log.Information(e.ToString()); }
            }
            else
            {
                m_primitivesRenderer3D.Clear();
            }
        }

        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(throwOnError: true);
            m_subsystemPickables = Project.FindSubsystem<SubsystemPickables>(throwOnError: true);
            foreach (KeyValuePair<string, object> item in valuesDictionary.GetValue("GameInfo", new ValuesDictionary()))
            {
                var playerStats = new GamePlayerDataInfo();
                playerStats.Load((ValuesDictionary)item.Value);
                m_playerinfo.Add(int.Parse(item.Key, CultureInfo.InvariantCulture), playerStats);
            }
        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
            var valuesDictionary2 = new ValuesDictionary();
            valuesDictionary.SetValue("GameInfo", valuesDictionary2);
            foreach (KeyValuePair<int, GamePlayerDataInfo> playerStat in m_playerinfo)
            {
                var valuesDictionary3 = new ValuesDictionary();
                valuesDictionary2.SetValue(playerStat.Key.ToString(CultureInfo.InvariantCulture), valuesDictionary3);
                playerStat.Value.Save(valuesDictionary3);
            }
        }
    }

    public class ComponentPlayerSystem : ComponentInventoryBase, IInventory, IDrawable, IUpdateable
    {
        public MapViewWidget MapViewWidget;
        public CanvasWidget Mapwidget;
        public UpdateOrder UpdateOrder => UpdateOrder.Default;

        public int[] DrawOrders => new int[1]
        {
            1
        };

        public ComponentPlayer m_componentPlayer;

        public SubsystemTime m_subsystemTime;

        public SubsystemTerrain m_subsystemTerrain;
        public bool showmap=false;

        public MapCamera MapCamera;

        public double m_lastrunTime;

        private bool m_running = false;

        private bool m_swimming = false;
        public SubsystemWorldSystem m_subsystemWorldSystem;
        public bool Running => m_running;

        public bool Swimming => m_swimming;

        public bool flag;

        public Model m_handModel;

        public int SellItemSlotIndex => SlotsCount - 1;

        public static UnlitShader UnlitShader = new UnlitShader(ShaderCodeManager.GetFast("Shaders/Unlit.vsh"), ShaderCodeManager.GetFast("Shaders/Unlit.psh"), useVertexColor: true, useTexture: true, useAlphaThreshold: false);

        public static UnlitShader UnlitShaderAlphaTest = new UnlitShader(ShaderCodeManager.GetFast("Shaders/Unlit.vsh"), ShaderCodeManager.GetFast("Shaders/Unlit.psh"), useVertexColor: true, useTexture: true, useAlphaThreshold: true);

        public static LitShader LitShader = new LitShader(ShaderCodeManager.GetFast("Shaders/Lit.vsh"), ShaderCodeManager.GetFast("Shaders/Lit.psh"), 2, useEmissionColor: false, useVertexColor: false, useTexture: true, useFog: false, useAlphaThreshold: false);
        public GamePlayerDataInfo PlayerInfo
        {
            get
            {
                return m_subsystemWorldSystem.GetPlayerInfo(m_componentPlayer.PlayerData.PlayerIndex);
            }
        }
        public void Update(float dt)
        {
            if (m_componentPlayer.ComponentInput.PlayerInput.Move.Z > 0f && m_componentPlayer.ComponentVitalStats.Stamina > 0.2f)
            {
                if (m_subsystemTime.GameTime - m_lastrunTime < 0.3)
                {
                    m_running = true;
                    //HardwareManager a = new HardwareManager();
                    //a.Vibrate(2);

                }
                flag = true;
            }
            else if (m_subsystemTime.GameTime - m_lastrunTime > 0.3 && flag)
            {
                m_lastrunTime = m_subsystemTime.GameTime;
                m_running = false;
                flag = false;
            }
            else
            {
                flag = false;
            }
            if (m_running)
            {
                for (int i = 0; i < 5; i++)
                {
                    m_componentPlayer.ComponentVitalStats.UpdateStamina();
                }
                m_componentPlayer.ComponentVitalStats.UpdateFood();
            }
            if (m_componentPlayer.ComponentBody.ImmersionFactor > 0.4f)
            {
                m_swimming = m_running;
            }
            else
            {
                m_swimming = false;
            }
            if(showmap==true)
            {
               
                Vector3 position = this.m_componentPlayer.ComponentBody.Position;
                this.MapCamera.TargetPosition = position;
                this.MapCamera.CameraPosition = position + new Vector3(0f, 100f, 0f);
                this.MapCamera.ViewRange = (float)SettingsManager.VisibilityRange;
                this.MapCamera.Update(dt);
            }
            else
            {
                Mapwidget.IsVisible = false;
            }
            
        }

        public void Vibrate(long ms)
        {
        }

        public void Draw(Camera camera, int drawOrder)
        {
            /*if (m_componentPlayer.ComponentHealth.Health > 0f && camera.GameWidget.IsEntityFirstPersonTarget(Entity))
			{
				Viewport viewport = Display.Viewport;
				Viewport viewport2 = viewport;
				viewport2.MaxDepth *= 0.1f;
				Display.Viewport = viewport2;
				try
				{
					var m = Matrix.CreateFromQuaternion(m_componentPlayer.ComponentCreatureModel.EyeRotation);
					float m_handLight = LightingManager.CalculateSmoothLight(m_subsystemTerrain, m_componentPlayer.ComponentCreatureModel.EyePosition).Value;// ? LightingManager.CalculateSmoothLight(m_subsystemTerrain, m_componentPlayer.ComponentCreatureModel.EyePosition).Value : 0f;
					var position4 = new Vector3(-0.8f, -0.29f, -0.05f);
					Matrix matrix2 = Matrix.CreateScale(0.01f) * Matrix.CreateRotationX(0.8f) * Matrix.CreateRotationY(-0.60f) * Matrix.Identity * Matrix.CreateTranslation(position4) * m * camera.ViewMatrix;
					Display.DepthStencilState = DepthStencilState.Default;
					Display.RasterizerState = RasterizerState.CullCounterClockwiseScissor;
					LitShader.Texture = m_componentPlayer.ComponentCreatureModel.TextureOverride;
					LitShader.SamplerState = SamplerState.PointClamp;
					LitShader.MaterialColor = Vector4.One;
					LitShader.AmbientLightColor = new Vector3(m_handLight * LightingManager.LightAmbient);
					LitShader.DiffuseLightColor1 = new Vector3(m_handLight);
					LitShader.DiffuseLightColor2 = new Vector3(m_handLight);
					LitShader.LightDirection1 = Vector3.TransformNormal(LightingManager.DirectionToLight1, camera.ViewMatrix);
					LitShader.LightDirection2 = Vector3.TransformNormal(LightingManager.DirectionToLight2, camera.ViewMatrix);
					LitShader.Transforms.World[0] = matrix2;
					LitShader.Transforms.View = Matrix.Identity;
					LitShader.Transforms.Projection = camera.ProjectionMatrix;
					foreach (ModelMesh mesh in m_handModel.Meshes)
					{
						foreach (ModelMeshPart meshPart in mesh.MeshParts)
						{
							Display.DrawIndexed(PrimitiveType.TriangleList, LitShader, meshPart.VertexBuffer, meshPart.IndexBuffer, meshPart.StartIndex, meshPart.IndicesCount);
							Log.Information(m_handLight);
						}
					}
				}
				catch (Exception e)
				{
					Log.Warning(e.ToString());
				}
				finally
				{
					Display.Viewport = viewport;
				}
			}*/
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(throwOnError: true);
            m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            m_subsystemWorldSystem = Project.FindSubsystem<SubsystemWorldSystem>(throwOnError: true);
            m_handModel = ContentManager.Get<Model>("Models/FirstPersonHand");
            MapCamera = new MapCamera(m_componentPlayer.GameWidget)
            {
                IsOrthographic = false
            };
            
            Mapwidget = m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("MapWidget",true);
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
        }
        
    }



}
