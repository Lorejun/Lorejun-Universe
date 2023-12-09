using System.Linq;
using Game;
using Engine.Serialization;
using System.Xml.Linq;
using Engine;
using System;
using Engine.Media;

using Engine.Graphics;
using GameEntitySystem;
using System.Collections.Generic;
using System.Globalization;
using TemplatesDatabase;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using XmlUtilities;
using Engine.Input;
using System.Text;
using System.Threading.Tasks;
using Engine.Audio;
using Test1;
using System.Text.RegularExpressions;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
using static Game.TerrainContentsGenerator21;

namespace Test1 //界面测试
{
	public class Test1ButtonsPanelWidget : CanvasWidget
	{
		
		public bool YHLZ = false;
		public bool BS = false;//召唤商人标记
		public ComponentTest1 m_componentTest1;
		public SubsystemWorldSystem m_subsystemWorldSystem;
		public ComponentTask m_componentTask;
		public SubsystemTask1 subsystemTask1;
		public BevelledButtonWidget m_button1;
		public BevelledButtonWidget m_button2;
		public BevelledButtonWidget m_button3;
        public BevelledButtonWidget m_button4;
        public BevelledButtonWidget m_button5;
        public BevelledButtonWidget m_button6;
        public BevelledButtonWidget m_button7;
        public BevelledButtonWidget m_button8;
		public BevelledButtonWidget m_button9;
        public BevelledButtonWidget m_button10;
		public Test1ButtonsPanelWidget(ComponentTest1 componentTest1,SubsystemWorldSystem m_subsystemWorldSystem1)
		{
			m_subsystemWorldSystem = m_subsystemWorldSystem1;
            m_componentTest1 = componentTest1;
			subsystemTask1 = componentTest1.m_subsystemTask1;
			m_componentTask = componentTest1.m_componentTask;
			XElement node = ContentManager.Get<XElement>("Test1/Widgets/Test1ButtonsPanelWidget");
			LoadContents(this, node);
			m_button1 = Children.Find<BevelledButtonWidget>("Button1");//通过Name属性获取子界面，若xml中不存在对应的Name则会出错
			m_button2 = Children.Find<BevelledButtonWidget>("Button2");
			m_button3 = Children.Find<BevelledButtonWidget>("Button3");
            m_button4 = Children.Find<BevelledButtonWidget>("Button4");
            m_button5 = Children.Find<BevelledButtonWidget>("Button5");
            m_button6 = Children.Find<BevelledButtonWidget>("Button6");
            m_button7 = Children.Find<BevelledButtonWidget>("Button7");
            m_button8 = Children.Find<BevelledButtonWidget>("Button8");
            m_button9 = Children.Find<BevelledButtonWidget>("Button9");
            m_button10 = Children.Find<BevelledButtonWidget>("Button10");
			
		}

		public override void Update()
		{
			if (m_button1.IsClicked)
			{//按钮1被点击
				Test1TextDialog dialog = new Test1TextDialog("模组介绍", "这是一段模组介绍文字", null);//定义对话框，action传入为null则不显示确定按钮，仅显示关闭按钮
				DialogsManager.ShowDialog(m_componentTest1.m_componentPlayer.GuiWidget, dialog);//弹出对话框
			}
			if (m_button2.IsClicked)
			{//按钮2被点击
				Action action = delegate
				{//定义一个action，将此action传入对话框，则点击对话框的确定按钮时将执行这里面的代码
					ScreensManager.SwitchScreen("Help");//转到帮助
				};
				Test1TextDialog dialog = new Test1TextDialog("模组教程", "这是一段教程文字。点击下方确定按钮可跳转到游戏帮助界面", action);//定义对话框，action传入不为null则显示确定按钮
				DialogsManager.ShowDialog(m_componentTest1.m_componentPlayer.GuiWidget, dialog);//弹出对话框
			}
			if(m_button3.IsClicked)
            {
				if(YHLZ==false)
                {
					YHLZ = true;
					m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("樱花粒子已经开启！（樱花粒子暂时被移除，请期待作者的优化。）", Color.Pink, true, true);//显示通知
				}
				else
                {
					YHLZ=false;
					m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("樱花粒子已经关闭！", Color.Pink, true, true);//显示通知
				}
            }
			if(m_button4.IsClicked)
			{
				m_componentTest1.m_componentPlayer.ComponentGui.ModalPanelWidget = new XRecipeWidget();
            }
            if (m_button5.IsClicked)//召唤商人
            {
				if(m_componentTest1.BS1==false)
				{
                    m_componentTest1.BS1 = true;//加入商人
                    m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("成功召唤商人洛德！要好好相处哦", Color.DarkRed, true, true);//显示通知
                    m_componentTest1.SummonBusinessman(m_componentTest1.BS1);
                }
				else
				{
                    m_componentTest1.BS1 = false;//加入商人
                    m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("下次再见，孤独的游荡之魂", Color.DarkRed, true, true);//显示通知
                    m_componentTest1.RemoveBusinessman(m_componentTest1.BS1);
                }
				
				
                // m_componentTest1.m_componentPlayer.ComponentGui.ModalPanelWidget = new ShopWidget(m_subsystemWorldSystem, m_componentTest1.m_componentPlayer.ComponentGui.Entity.FindComponent<ComponentBody>(true));
                
            }
            if (m_button6.IsClicked)
            {
                m_componentTest1.m_componentPlayer.ComponentGui.ModalPanelWidget = new MCCGPurchaseWidget(m_componentTest1.m_componentPlayer.ComponentMiner);//显示购买界面
            }
            if (m_button7.IsClicked)
            {
                m_componentTest1.m_componentPlayer.ComponentGui.ModalPanelWidget = new MCCGSellWidget(m_componentTest1.m_componentPlayer.ComponentMiner);//显示购买界面
            }
			if(m_button8.IsClicked)
			{
				if (m_componentTest1.m_componentPlayersystem.showmap == false)
				{
					m_componentTest1.m_componentPlayersystem.showmap = true;
                    m_componentTest1.m_componentPlayersystem.Mapwidget.IsVisible = true;
                   // m_componentTest1.m_componentPlayersystem.MapViewWidget.IsVisible = true;

                }
				else
				{
                    m_componentTest1.m_componentPlayersystem.showmap = false;
                    m_componentTest1.m_componentPlayersystem.Mapwidget.IsVisible = false;
                   // m_componentTest1.m_componentPlayersystem.MapViewWidget.IsVisible = false;
                }
               
            }
            if (m_button9.IsClicked)
            {
                m_componentTest1.m_componentPlayer.ComponentGui.ModalPanelWidget = new TaskWidget(m_componentTask);//将当前界面转至AchievementsWidget
            }
            if (m_button10.IsClicked)
			{
				m_componentTest1.DeveloperModeOn = !m_componentTest1.DeveloperModeOn;//关闭或打开开发者模式
				string txt = "食品工艺：开发者模式已" + (m_componentTest1.DeveloperModeOn ? "开启" : "关闭");
				m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage(txt, Color.White, true, true);//显示通知
			}
			Color centerColor = m_componentTest1.DeveloperModeOn ? new Color(50, 150, 35) : new Color(181, 172, 154);//中心颜色
			Color bevelColor = m_componentTest1.DeveloperModeOn ? new Color(50, 150, 35) : new Color(181, 172, 154);//边框颜色
			m_button10.CenterColor = centerColor;//设定开发者模式按钮颜色，开启开发者模式时为绿色，反之则为默认颜色
			m_button10.BevelColor = bevelColor;

			

		}
	}
	public class Test1TextDialog : Dialog
	{
		
		public ButtonWidget m_okButton;
		public ButtonWidget m_cancelButton;
		public List<Widget> Extra = new List<Widget>();
		public Action OnYesClicked;
		public StackPanelWidget Stack;
		

		public Test1TextDialog(string title, string txt, Action action)
		{
			XElement node = ContentManager.Get<XElement>("Test1/Dialogs/Test1TextDialog");
			LoadContents(this, node);
			Stack = Children.Find<StackPanelWidget>("Stack");
			m_okButton = Children.Find<ButtonWidget>("OkButton");
			m_cancelButton = Children.Find<ButtonWidget>("CancelButton");
			LabelWidget GM = Children.Find<LabelWidget>("Text");
			LabelWidget Title = Children.Find<LabelWidget>("Title");
			GM.Text = txt;
			Title.Text = title;
			OnYesClicked += action;
			m_okButton.IsVisible = action != null;
		}

		public override void Update()
		{
			if (m_okButton.IsClicked)
			{
				DialogsManager.HideDialog(this);
				OnYesClicked();
			}
			if (m_cancelButton.IsClicked || base.Input.Cancel)
			{
				DialogsManager.HideDialog(this);
			}
		}
	}
	public class ComponentTest1 : ComponentInventoryBase, IUpdateable
	{//此组件用于添加打开界面的按钮，可将该组件全部功能合并至任意一个仅注册在玩家身上的组件
		bool isbuffing = false;

       
		
		public UpdateOrder UpdateOrder => UpdateOrder.Default;
		public BitmapButtonWidget m_test1Button = new BitmapButtonWidget()
		{//定义一个按钮
			Margin = new Vector2(0f, 0f),//边距
			Size = new Vector2(60f, 60f),//按钮大小
			NormalSubtexture = new Subtexture(ContentManager.Get<Texture2D>("Test1/Textures/Setting"), Vector2.Zero, Vector2.One),//按钮贴图
			ClickedSubtexture = new Subtexture(ContentManager.Get<Texture2D>("Test1/Textures/Setting_Click"), Vector2.Zero, Vector2.One)//按钮被按下时的贴图
		};
		public LabelWidget m_test1Display = new LabelWidget()
		{//定义常驻屏幕右下角显示的文字界面
			Text = "Loading",//显示的文字，这段是默认文字，因为Update一启动就会覆盖这个文字
			FontScale = 0.7f,//字体大小
			IsHitTestVisible = false,//是否能点击，为true的话可能会挡住重叠在它下方的界面，出现点不到下方界面的情况
			HorizontalAlignment = WidgetAlignment.Far,//居右对齐(由于界面对齐以左上角为原点，因此水平方向的Near就是居左，Far就是居右，Center居中)
			VerticalAlignment = WidgetAlignment.Far,//居下对齐(由于界面对齐以左上角为原点，因此垂直方向的Near就是居上，Far就是居下，Center居中)
			DropShadow = true//是否投影阴影(为true时字体会在右下方重叠显示一片黑色阴影，看着更立体、更醒目)
		};

		public LabelWidget Buffscreen = new LabelWidget()
		{//定义buff显示界面
			Text = "Loading",//显示的文字，这段是默认文字，因为Update一启动就会覆盖这个文字
			FontScale = 0.5f,//字体大小
			IsHitTestVisible = false,//是否能点击，为true的话可能会挡住重叠在它下方的界面，出现点不到下方界面的情况
			HorizontalAlignment = WidgetAlignment.Near,//居右对齐(由于界面对齐以左上角为原点，因此水平方向的Near就是居左，Far就是居右，Center居中)
			VerticalAlignment = WidgetAlignment.Center,//居下对齐(由于界面对齐以左上角为原点，因此垂直方向的Near就是居上，Far就是居下，Center居中)
			DropShadow = true//是否投影阴影(为true时字体会在右下方重叠显示一片黑色阴影，看着更立体、更醒目)
		};

		public bool DeveloperModeOn;//开发者模式是否开启
        public SubsystemGameInfo m_subsystemGameInfo;
        public SubsystemTerrain m_systemTerrain;
        public NewModLoaderShengcheng Shengcheng1;
        public FCSubsystemTown m_subsystemtown;
        public FCSubsystemTownChunk m_subsystemtownchunk;
        public ComponentPlayer m_componentPlayer;
        Game.Random random = new Game.Random();
		public SubsystemWorldDemo World;
		public SubsystemNaturallyBuildings m_subsystemNaturallyBuildings;
        public SubsystemTime m_subsystemTime;
		public SubsystemWorldSystem m_subsystemWorldsystem;
		public SubsystemTask1 m_subsystemTask1;
		public SubsystemNames m_subsystemNames;
		public ComponententityLord entitylord;
		public ComponentTask m_componentTask;
		public ComponentPlayerSystem m_componentPlayersystem;
		public SubsystemTask m_subsystemTask;
        public bool BS1=false;
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_systemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
			m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
			m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemGameInfo =Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTask1=Project.FindSubsystem<SubsystemTask1>(true);
            m_subsystemNames=Project.FindSubsystem<SubsystemNames>(true);
			m_subsystemTask=Project.FindSubsystem<SubsystemTask>(true);

            World = Project.FindSubsystem<SubsystemWorldDemo>(true);
            m_subsystemNaturallyBuildings = Project.FindSubsystem<SubsystemNaturallyBuildings>(true);
			m_subsystemWorldsystem =Project.FindSubsystem<SubsystemWorldSystem>(true);
			BS1 = valuesDictionary.GetValue<bool>("BS", false);
			

            componentNightsight = Entity.FindComponent<ComponentNightsight>(true);
			componentSpeedUP = Entity.FindComponent<ComponentSpeedUP>(true);
			componentHealBuffA = Entity.FindComponent<ComponentHealBuffA>(true);
			componentAttackUP = Entity.FindComponent<ComponentAttackUP>(true);
            componentBlind = Entity.FindComponent<ComponentBlind>(true);
            componentJump = Entity.FindComponent<ComponentJump>(true);
            componentDizzy = Entity.FindComponent<ComponentDizzy>(true);
            m_componentPC = Entity.FindComponent<FCComponentPC>(true);
            m_componentSlowdown = Entity.FindComponent<ComponentSlowDown>(true);
            


            m_componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("MoreContents", true).Children.Add(m_test1Button);//将按钮添加至玩家右上角省略号内
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Add(m_test1Display);//将文字界面添加至屏幕
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Add(Buffscreen);//将buff界面添加至屏幕
			DeveloperModeOn = valuesDictionary.GetValue("DeveloperModeOn", false);//从project.xml中获取储存的值，若获取失败则采用false
			m_componentTask=Entity.FindComponent<ComponentTask>(true);
			m_componentPlayersystem = Entity.FindComponent<ComponentPlayerSystem>(true);
			Shengcheng1 = new NewModLoaderShengcheng();

			//灵能sen值组件
            m_mp = valuesDictionary.GetValue<float>("MagicPower", 1f);
            m_sen = valuesDictionary.GetValue<float>("Sen", 100f);
			isLowSen = valuesDictionary.GetValue<bool>("IsSenLow", false);//默认返回不处于低sen状态
			m_Maxmp = valuesDictionary.GetValue<float>("MaxMagicPower", 100f);
            m_mplevel = valuesDictionary.GetValue<float>("MPLevel", 10f);
            FCMagicLevel = valuesDictionary.GetValue<int>("FCLevel", 0);

			Areaname = valuesDictionary.GetValue<string>("Areaname", "");
           

        }
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			base.Save(valuesDictionary, entityToIdMap);
			valuesDictionary.SetValue("DeveloperModeOn", DeveloperModeOn);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<float>("MagicPower", MagicPower);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<float>("Sen", Sen);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<bool>("IsSenLow", IsSenLow);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<float>("MaxMagicPower", MaxMagicPower);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<float>("MPLevel", m_mplevel);//将该变量值储存到project.xml，防止重进存档后变回默认值
            valuesDictionary.SetValue<int>("FCLevel", FCMagicLevel);//将该变量值储存到project.xml，防止重进存档后变回默认值
			valuesDictionary.SetValue<string>("Areaname", Areaname);
           // valuesDictionary.SetValue<bool>("BS", BS1);//将该变量值储存到project.xml，防止重进存档后变回默认值
        }

		public override void OnEntityRemoved()
		{
			base.OnEntityRemoved();
			m_componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("MoreContents", true).Children.Remove(m_test1Button);//当玩家实体被移除后，也要移除按钮，否则玩家复活后会出现两个按钮
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Remove(m_test1Display);//移除文字界面，原因同上
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Remove(Buffscreen);//移除文字界面，原因同上
		}

        public override void OnEntityAdded()
        {
            base.OnEntityAdded();
			if(IsSenLow==true)
			{
                base.Project.FindSubsystem<SubsystemBlocksTexture>(true).BlocksTexture = ContentManager.Get<Texture2D>("BlocksSen");
            }
        }
        /*第一级的mplevel进度值需要达到100，然后最大灵能值会对应扩充到1000，这是第一级。
			以此类推，二级的进度值需要达到5000，同时最大灵能扩充到5000
			三级的进度需要达到10000，最大灵能为10000
			四级进度需要到20000，最大灵能为50000
			五级进度需要达到50000，最大灵能为100000
			第六级需要达到100000，最大灵能为1098888*/
		public void SummonBusinessman(bool flag)
		{
            Entity entity = DatabaseManager.CreateEntity(Project, "BSLord", throwIfNotFound: true);
            if (flag==true)
			{
				
                entity.FindComponent<ComponentFrame>(throwOnError: true).Position = new Vector3(m_componentPlayer.ComponentBody.Position.X + 2, m_componentPlayer.ComponentBody.Position.Y, m_componentPlayer.ComponentBody.Position.Z);
                entity.FindComponent<ComponentFrame>(throwOnError: true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, random.Float(0f, (float)Math.PI * 2f));
                entity.FindComponent<ComponentSpawn>(throwOnError: true).SpawnDuration = 0f;
                entitylord=entity.FindComponent<ComponententityLord>();
                Project.AddEntity(entity);
            }
			
            
        }
        public void RemoveBusinessman(bool flag)
        {
			try
			{
                Entity entity = entitylord.LordEntity;
                if (flag == false)
                {
                    if (entity != null)
                    {
                        Project.RemoveEntity(entity, true);
                    }


                }
            }
			catch
			{
                Log.Error("Generation1 failed.");
            }
            
            


            


        }

        public void UpdateLevel()
        {
            m_subsystemAudio.PlaySound("Audio/LevelUp", 1f, 0, 0,0.1f);
            if ( m_Maxmp < 1000)//10%的免伤
            {
                m_Maxmp = 1000;
				
				FCMagicLevel = 1;
            }
            else if (m_Maxmp < 5000) //20%的免伤
            {
                m_Maxmp = 5000;
                FCMagicLevel = 2;

            }
            else if (m_Maxmp < 10000)//30%的免伤
            {
                m_Maxmp = 10000;
                FCMagicLevel = 3;

            }
            else if (m_Maxmp < 50000)//40%的免伤
            {
                m_Maxmp = 50000;
                FCMagicLevel = 4;

            }
            else if (m_Maxmp < 100000)//50%的免伤
            {
                m_Maxmp = 100000;
                FCMagicLevel = 5;

            }
            else if (m_Maxmp < 1098888)//70%的免伤
            {
                m_Maxmp = 1098888;
                FCMagicLevel = 6;

            }
            m_mp = m_Maxmp;
        }//升级封装方法
        public float CalculateLevelnum(int level,float levelnum)
        {
			float num1=0;
            if (level== 0)//
            {
				num1 = 100-levelnum;
				MPlevel1 = "灵感者";

            }
            else if (level ==  1)
            {
                num1 = 5000 - levelnum;
                MPlevel1 = "一级通灵师";
            }
            else if (level == 2)
            {
                num1 = 10000 - levelnum;
                MPlevel1 = "二级通灵师";
            }
            else if (level == 3)
            {
                num1 = 20000 - levelnum;
                MPlevel1 = "三级通灵师";
            }
            else if (level == 4)
            {
                num1 = 50000 - levelnum;
                MPlevel1 = "灵王";
            }
            else if (level == 5)//升到6级为封顶
            {
                num1 = 100000 - levelnum;
                MPlevel1 = "灵王";
            }
            else if (level == 6)//升到6级为封顶
            {
				num1 = 1;
               
                MPlevel1 = "通灵之神";
            }

            return num1;

        }//计算等级差封装方法
        public string GetLevelname(int level)
        {
           ;
            if (level == 0)//
            {
                
                MPlevel1 = "灵感者";

            }
            else if (level == 1)
            {
                
                MPlevel1 = "一级通灵师";
            }
            else if (level == 2)
            {
                
                MPlevel1 = "二级通灵师";
            }
            else if (level == 3)
            {
               
                MPlevel1 = "三级通灵师";
            }
            else if (level == 4)
            {
                
                MPlevel1 = "灵王";
            }
            else if (level == 5)//升到6级为封顶
            {
               
                MPlevel1 = "灵祖";
            }
            else if (level == 6)//升到6级为封顶
            {
              

                MPlevel1 = "通灵之神";
            }

            return MPlevel1;

        }//计算等级名字方法
        public static int GetTemperatureAdjustmentAtHeight(int y)
		{//该静态方法从原版复制过来，用于计算高度变化带来的温度修正
			return (int)MathUtils.Round((y > 64) ? (-0.0008f * MathUtils.Sqr(y - 64)) : (0.1f * (64 - y)));
		}
		public bool isvillageLoading = false;
		public int num_village4 = 0;
		
        public bool buffscreenIsVisible = true;



        #region Sen灵力数值读取
        public float m_mp=1f;
		public float MagicPower
		{
			get { return m_mp; }
		}

		public int FCMagicLevel = 0;//灵能等级

		public float m_mplevel;//灵能进度
        public float MPLevel
        {
            get { return m_mplevel; }
        }

        public float m_Maxmp;//最大灵力值
        public float MaxMagicPower
        {
            get { return m_Maxmp; }
        }


        public float m_sen;
        public float Sen
        {
            get { return m_sen; }
        }

        public bool isLowSen = false;//控制材质变化

		public bool IsSenLow
		{
			get { return isLowSen; }
		}
        #endregion 
        #region  更新区块
        public void UpdateAllChunks(float time, TerrainChunkState chunkState)
        {
            bool flag = time == 0f;
            if (flag)
            {
                m_systemTerrain.TerrainUpdater.DowngradeAllChunksState(chunkState, true);
            }
            Time.QueueTimeDelayedExecution(Time.RealTime + (double)time, delegate
            {
                m_systemTerrain.TerrainUpdater.DowngradeAllChunksState(chunkState, true);
            });
        }
        #endregion
		public string worldnameN;
		public string worldgravity;
		public string MPlevel1;
		public bool isinCityArea=false;
        public bool isinBloodArea = false;
        public string Areaname;
        public ComponentHealBuffA componentHealBuffA;//引入buff组件
        public ComponentAttackUP componentAttackUP;
        public ComponentSpeedUP componentSpeedUP;
        public ComponentNightsight componentNightsight;
        public ComponentBlind componentBlind;
        public ComponentJump componentJump;
        public ComponentDizzy componentDizzy;
        public FCComponentPC m_componentPC;
		public ComponentSlowDown m_componentSlowdown;
		public SubsystemAudio m_subsystemAudio;
		public Point3 point1;
        public  void Update(float dt)
		{
            WorldType m_worldname = World.worldType;//时刻获取世界信息
            point1= Terrain.ToCell(m_componentPlayer.ComponentBody.Position);//玩家所在方块坐标
			Point2 pointcoords = ((int)point1.X/16, (int)point1.Z/16);
            #region 灵能判断区域
			if(m_mp<0f) //灵力值不能超过最大灵力
			{
				m_mp = 0f;
			}
			else if(m_mp>1f)
			{
				 m_mp=1f;
			}

			if(FCMagicLevel<6)
			{
                if (m_mplevel >= 100 && FCMagicLevel == 0)//处理升级
                {
                    UpdateLevel();
					
                }
                else if (m_mplevel >= 5000 && FCMagicLevel == 1)
                {
                    UpdateLevel();
                }
                else if (m_mplevel >= 10000 && FCMagicLevel == 2)
                {
                    UpdateLevel();
                }
                else if (m_mplevel >= 20000 && FCMagicLevel == 3)
                {
                    UpdateLevel();
                }
                else if (m_mplevel >= 50000 && FCMagicLevel == 4)
                {
                    UpdateLevel();
                }
                else if (m_mplevel >= 100000 && FCMagicLevel == 5)//升到6级为封顶
                {
                    UpdateLevel();
                }
            }
			if(m_mplevel>=120000)
			{
				m_mplevel = 120000;

            }
			
            

            #endregion
            #region Sen值判断区域
            //sen的范围是0-100
            if (m_sen > 100f)
			{
				m_sen = 100f;
			}
			else if(m_sen<0)
			{
				m_sen = 0f;
			}


			if (m_sen <30&& m_sen >= 20)
			{
                bool flag = m_subsystemTime.PeriodicGameTimeEvent(3.0, 0.0);
				if(flag)
				{
                    m_componentPlayer.ComponentGui.DisplayLargeMessage("Sen值过低！请立刻恢复Sen值！", null, 1.5f, 0f);
                }
               
            }
           
            if (m_sen<20)
			{
				if(isLowSen==false)
				{
                    m_componentPlayer.ComponentGui.DisplayLargeMessage("低Sen状态！！！它看到你了……", null, 3f, 0f);
                    base.Project.FindSubsystem<SubsystemBlocksTexture>(true).BlocksTexture = ContentManager.Get<Texture2D>("BlocksSen");
                    UpdateAllChunks(0f, TerrainChunkState.InvalidLight);
					isLowSen = true;
                }
                
            }
			else
			{
				if(m_sen>=20f)//sen恢复到20以上，切换sen值状态
				{
					if(isLowSen== true)
					{
                        m_componentPlayer.ComponentGui.DisplayLargeMessage("成功脱离低Sen！", null, 1.5f, 0f);
                        base.Project.FindSubsystem<SubsystemBlocksTexture>(true).BlocksTexture = BlocksTexturesManager.DefaultBlocksTexture;
                        UpdateAllChunks(0f, TerrainChunkState.InvalidLight);
						isLowSen = false;
                    }
                   
                }
              
            }
            #endregion
            #region 村庄建筑区块提示和判断
            List<NewModLoaderShengcheng.RoadPoint> listRD = NewModLoaderShengcheng.listRD;
            List<NewModLoaderShengcheng.BuildPoint> listBD = NewModLoaderShengcheng.listBD;
            if (m_worldname == WorldType.Default)
            {
                if (listRD.Count != 0 && isvillageLoading == false)//村庄代码区块
                {
                    isvillageLoading = true;
                    if (FCSubsystemTown.Village_start.Count != 0)
                    {
                        /*if ((FCSubsystemTown.Village_start.Count - num_village4) > 0)//如果村庄生成有变化，实时提醒。
                        {
                            num_village4++;
                            string txt = $"检测到村庄已经生成，玩家可前往探索！坐标为：{FCSubsystemTown.Village_start[num_village4 - 1]}";
                            m_componentPlayer.ComponentGui.DisplaySmallMessage(txt, Color.White, true, true);//显示通知

                        }*/


                    }

                }
                if (listRD.Count == 0)
                {
                    isvillageLoading = false;
                }

                
                //副本区域判断
                foreach (BuildingInfo buildinfo in m_subsystemNaturallyBuildings.BuildingInfos)
                {
                    if (buildinfo != null)
                    {
                        if (buildinfo.CalculatPlainRange(pointcoords) == true)
                        {
							if(buildinfo.Name== "House/Supercity"&& isinCityArea == false)
							{
                                string txt = $"你已经进入了[失落城市]区域！";
                                m_componentPlayer.ComponentGui.DisplayLargeMessage(txt,null,2f,0f);//显示通知
								Areaname = "失落城市";
                                isinCityArea = true;
                            }
							else if(buildinfo.Name == "House/血泪" && isinBloodArea == false)
							{
								m_componentTask.BloodPool = true;//完成任务
                                string txt = $"你已经进入了[血泪之池]区域！小心游荡的怪物！";
                                m_componentPlayer.ComponentGui.DisplayLargeMessage(txt, null, 2f, 0f);//显示通知
                                Areaname = "血泪之池";
                                isinBloodArea = true;
                            }
                                
                        }
                        else
                        {
                            if (buildinfo.Name == "House/Supercity"&& isinCityArea == true)
							{
                                string txt = $"你已经离开了[失落城市]区域！";
                                m_componentPlayer.ComponentGui.DisplayLargeMessage(txt, null, 2f, 0.1f);//显示通知
                                Areaname = "";
                                isinCityArea = false;
                            }
                            else if(buildinfo.Name == "House/血泪" && isinBloodArea == true)
							{
                                string txt = $"你已经离开了[血泪之池]区域！";
                                m_componentPlayer.ComponentGui.DisplayLargeMessage(txt, null, 2f, 0.1f);//显示通知
                                Areaname = "";
                                isinBloodArea = false;
                            }
                        }
                    }

                }

                
            }

            #endregion
            #region 开发者模式
            if (DeveloperModeOn)
			{//开发者模式打开
				m_test1Display.IsVisible = true;//文字界面可见

				Point3 point = Terrain.ToCell(m_componentPlayer.ComponentBody.Position);//玩家所在方块坐标
				int temperature = m_systemTerrain.Terrain.GetTemperature(point.X, point.Z) + GetTemperatureAdjustmentAtHeight(point.Y);//获取玩家所在位置温度
				int humidity = m_systemTerrain.Terrain.GetHumidity(point.X, point.Z);//获取玩家所在位置湿度
				string txt1 = string.Format("温度={0},湿度={1},坐标 = {2}", temperature, humidity, point);//传入变量组成第一段文字

				int slotValue = m_componentPlayer.ComponentMiner.ActiveBlockValue;//获取玩家手中物品值
				int id = Terrain.ExtractContents(slotValue), data = Terrain.ExtractData(slotValue);//提取content和data
				string extra = (data == 0) ? string.Empty : ":" + data.ToString();//冒号后面接data值，若data为0则为空字符
				string typeName = BlocksManager.Blocks[id].GetType().Name + extra;//获取方块的类名
				string craftingId = BlocksManager.Blocks[id].CraftingId + extra;//获取方块的合成标识
				string txt2 = string.Format("完整值={0},id={1},data={2},\nTypeName={3},\nCraftingId={4}", slotValue, id, data, typeName, craftingId);//传入变量组成第二段文字

				m_test1Display.Text = txt1 + "\n" + txt2;//更新文字界面的文字
			}
			else//开发者模式关闭
			{
				m_test1Display.IsVisible = false;//文字界面不可见
			}
			if (m_test1Button.IsClicked)
			{//在Update中实时检测按钮是否被按下，若被按下则触发以下代码
				m_componentPlayer.ComponentGui.ModalPanelWidget = new Test1ButtonsPanelWidget(this, m_subsystemWorldsystem);//打开界面
			}
            
            #endregion
            if (buffscreenIsVisible == true)//时刻开启
			{
				#region 世界判断
				WorldType worldname = World.worldType;
                
                if (worldname == WorldType.Default)
				{
                    worldnameN = "主世界";
					if(point1.Y >1500f)//传送到空间站
					{
                        Vector3 v1 = m_componentPlayer.ComponentBody.Position;
                        m_componentPlayer.ComponentBody.Position = (v1.X, 1300, v1.Z);//重置位置
                        World.MajorToChildWorld("StationMoon");
					}
					worldgravity = "10";
                }
                if (worldname == WorldType.Ashes)
                {
                    worldnameN = "灰烬世界";

                }
                if (worldname == WorldType.Desert)
                {
                    worldnameN = "沙子世界";

                }
                if (worldname == WorldType.Exist)
                {
                    worldnameN = "现有世界";

                }
                if (worldname == WorldType.StationMoon)
                {
                    worldnameN = "地月空间站";
					worldgravity = m_componentPC.Gravity.ToString("f2");

                    if (point1.Y > 1500f)//传送到月球
                    {
                        Vector3 v1 = m_componentPlayer.ComponentBody.Position;
                        m_componentPlayer.ComponentBody.Position = (v1.X, 1300, v1.Z);//重置位置
                        World.ChildToChildWorld("Moon");
                    }
                    if (point1.Y < -300f)//传送到地球
                    {
                        Vector3 v1 = m_componentPlayer.ComponentBody.Position;
                        m_componentPlayer.ComponentBody.Position = (v1.X, 600, v1.Z);//重置位置
                        World.ChildToMajorWorld();
                    }

                }
                if (worldname == WorldType.Moon)
                {
                    worldnameN = "月球";
                    worldgravity = m_componentPC.Gravity.ToString("f2");
                    if (point1.Y > 1500f)//传送到空间站
                    {
                        Vector3 v1 = m_componentPlayer.ComponentBody.Position;
                        m_componentPlayer.ComponentBody.Position = (v1.X, 1300, v1.Z);//重置位置
                        World.ChildToChildWorld("StationMoon");
                    }

                }
                if (worldname == WorldType.Snowfield)
                {
                    worldnameN = "冰雪世界";

                }
                #endregion
                Buffscreen.IsVisible = true;
                string buffinfo = "状态栏";
                string txt7 = string.Format("当前世界:{0} [重力:{1}] [氧气:{2}/{3}]", worldnameN,worldgravity,(int)(m_componentPlayer.ComponentHealth.Air* m_componentPlayer.ComponentHealth.AirCapacity), (int)m_componentPlayer.ComponentHealth.AirCapacity);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt7;
                string txt9 = string.Format("当前区域:{0}", Areaname);//获取区域名字
                buffinfo = buffinfo + "\n" + txt9;
                string txt2 = string.Format("当前生命:{0}/{1}", (int)(m_componentPlayer.ComponentHealth.Health* m_componentPlayer.ComponentHealth.AttackResilience), (int)m_componentPlayer.ComponentHealth.AttackResilience);//获取最大生命值和当前生命值
                buffinfo = buffinfo + "\n" + txt2;
                string txt3 = string.Format("当前攻击力:{0}", (int)m_componentPlayer.ComponentMiner.AttackPower);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt3;
                string txt4 = string.Format("当前速度:{0}", (int)m_componentPlayer.ComponentLocomotion.WalkSpeed);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt4;
                string txt5 = string.Format("灵力值:{0}/{1}[进度：{2}]", (int)(MagicPower*MaxMagicPower),(int)MaxMagicPower,(int)MPLevel);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt5;
                string txt8 = string.Format("灵力等级:{0}||{1}[距离下一等级还差:{2}]", FCMagicLevel, GetLevelname(FCMagicLevel), (int)CalculateLevelnum(FCMagicLevel,m_mplevel));//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt8;
                string txt6 = string.Format("当前sen值:{0}", (int)Sen);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt6;


                if (componentHealBuffA.m_HealDuration > 0 || componentAttackUP.m_ATKDuration > 0 || componentSpeedUP.m_SpeedDuration > 0||componentNightsight.m_NightseeDuration>0||componentBlind.m_BlindDuration > 0|| m_componentSlowdown.m_SlowDuration>0||componentJump.m_JumpDuration>0|| componentDizzy.m_DizzyDuration>0)//是否处于buff的前置判断
                {//这里是buff的前置显示判断很重要
                    isbuffing = true;

                }
                else
                {

                    isbuffing = false;
                }
                if (isbuffing == true)
                {


                    if (componentHealBuffA.m_HealDuration > 0)
                    {
                        string txt1 = string.Format("生命恢复：{0}s,速率：{1}血每秒", (int)componentHealBuffA.m_HealDuration, (int)componentHealBuffA.m_HealRate);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentAttackUP.m_ATKDuration > 0)
                    {
                        string txt1 = string.Format("攻击力强化：{0}s,原攻击力：{1}", (int)componentAttackUP.m_ATKDuration,(int)componentAttackUP.ATK_first);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentSpeedUP.m_SpeedDuration > 0)
                    {

                        string txt1 = string.Format("速度强化：{0}s,原速度：{1}", (int)componentSpeedUP.m_SpeedDuration,(int)componentSpeedUP.Speed_first);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentNightsight.m_NightseeDuration > 0)
                    {

                        string txt1 = string.Format("夜视：{0}s", (int)componentNightsight.m_NightseeDuration);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentBlind.m_BlindDuration > 0)
                    {

                        string txt1 = string.Format("致盲：{0}s", (int)componentBlind.m_BlindDuration);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (m_componentSlowdown.m_SlowDuration > 0)
                    {

                        string txt1 = string.Format("迟缓：{0}s", (int)m_componentSlowdown.m_SlowDuration);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentDizzy.m_DizzyDuration > 0)
                    {

                        string txt1 = string.Format("眩晕：{0}s", (int)componentDizzy.m_DizzyDuration);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }
                    if (componentJump.m_JumpDuration > 0)
                    {

                        string txt1 = string.Format("跳跃强化：{0}s", (int)componentJump.m_JumpDuration);//传入变量组成第一段文字
                        buffinfo = buffinfo + "\n" + txt1;
                    }

                }
                Buffscreen.Text = buffinfo;
            }//玩家信息状态栏目


           



        }
    }

	public class FCComponentPC:Component, IUpdateable
    {
        public SubsystemTerrain m_systemTerrain;
        public SubsystemTime m_subsystemTime;
        public ComponentPlayer m_componentPlayer;
		public ComponentCreature m_componentCreature;
        public SubsystemWorldDemo m_subsystemWorldDemo;
		public SubsystemFluidBlockBehavior m_fluidBlockBehavior;
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
            m_systemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
			m_fluidBlockBehavior = Project.FindSubsystem<SubsystemFluidBlockBehavior>(true);

            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemWorldDemo = Project.FindSubsystem<SubsystemWorldDemo>(true);
            base.Load(valuesDictionary, idToEntityMap);
		}
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
           


        }
        public UpdateOrder UpdateOrder => UpdateOrder.Default;

		public float Gravity
		{
			get
			{
				return  10-g;
			}
		}
        public float g;
        public  void Update(float dt)
        {
            

            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            
            if (worldType == WorldType.Default)
            {
                //如果为主世界，则怎么样
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
                if (m_componentCreature.ComponentLocomotion.m_flying == false && m_componentCreature.ComponentLocomotion.m_climbing == false)//不在飞行
                {
					g = 10f * 5f / 6f;
                    m_componentCreature.ComponentBody.m_velocity.Y = m_componentCreature.ComponentBody.m_velocity.Y +  g * dt;
                }
                


            }
            else if (worldType == WorldType.StationMoon)
            {
				/*if(m_componentPlayer!=null)
				{
					m_componentPlayer.ComponentBody.m_velocity.Y = m_componentPlayer.ComponentBody.m_velocity.Y + 9.8f * dt;
                }*/
				//m_fluidBlockBehavior.m_toUpdate.Clear();//禁止水流动
                Point3 point1 = Terrain.ToCell(m_componentCreature.ComponentBody.Position);//玩家所在方块坐标
				
				if(point1.Y<0&&point1.Y>-300)
				{
                     g = MathUtils.Lerp(5f,0.1f, MathUtils.Abs(point1.Y/301));//太阳大小
                }
				else
				{
					 g = 9.8f;

                }
                if (m_componentCreature.ComponentLocomotion.m_flying == false&& m_componentCreature.ComponentLocomotion.m_climbing == false)//不在飞行
				{
					m_componentCreature.ComponentBody.m_velocity.Y = m_componentCreature.ComponentBody.m_velocity.Y + g * dt;
					//m_componentCreature.ComponentBody.IsGravityEnabled = false;
                }
                   

                m_componentPlayer = Entity.FindComponent<ComponentPlayer>();
				if(m_componentPlayer!=null)
				{
                    if (m_componentPlayer.ComponentInput.PlayerInput.Jump && m_componentPlayer.ComponentLocomotion.m_falling)
                    {
                        Vector3 velocity = m_componentPlayer.ComponentBody.Velocity;
                        m_componentPlayer.ComponentBody.Velocity = new Vector3(velocity.X, 3f, velocity.Z);
                    }
                    if ((m_componentPlayer.ComponentInput.PlayerInput.ToggleSneak ) && m_componentPlayer.ComponentLocomotion.m_falling)
                    {

                        Vector3 velocity = m_componentPlayer.ComponentBody.Velocity;
                        m_componentPlayer.ComponentBody.Velocity = new Vector3(velocity.X, -3f, velocity.Z);
                    }
                }

               





            }
        }

    }//玩家和生物共有组件


}



namespace Game
{
    #region 新生命组件
    public class FCComponentHealth : ComponentHealth, IUpdateable
    {
		public SubsystemWorldDemo m_subsystemWorldDemo;
        public new ComponentPlayer m_componentPlayer;
        public ComponentTest1 m_componentTest1;
		public ComponentHealth m_componentHealth = new ComponentHealth();
       
        public new void Update(float dt)
        {
			
			WorldType worldType = m_subsystemWorldDemo.worldType;
            //抗性值用来决定生物对攻击、跌落和火焰的抗性。这些值可能会因装备或其他游戏效果而变化。
            AttackResilience = m_attackResilience * AttackResilienceFactor;
            FallResilience = m_fallResilience * FallResilienceFactor;
            FireResilienceFactor = m_fireResilience * FireResilienceFactor;
            Vector3 position = m_componentCreature.ComponentBody.Position;
            if (Health > 0f && Health < 1f)
            {
                //如果生物的健康值在0到1之间（存活状态），但不是满血，它将根据不同的条件（游戏模式、睡眠状态、饥饿度）进行自然恢复。
                float num = 0f;
                if (m_componentPlayer != null)
                {
                    if (m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Harmless)
                    {
                        num = 0.016666668f;
                    }
                    else if (m_componentPlayer.ComponentSleep.SleepFactor == 1f && m_componentPlayer.ComponentVitalStats.Food > 0f)
                    {
                        num = 0.0016666667f;
                    }
                    else if (m_componentPlayer.ComponentVitalStats.Food > 0.5f)
                    {
                        num = 0.0011111111f;
                    }

                }
                else
                {
                    num = 0.0011111111f;
                }
               
                Heal(m_subsystemGameInfo.TotalElapsedGameTimeDelta * num);
                if (m_componentTest1 != null)
				{
					if(m_componentTest1.m_sen<=70&& m_componentTest1.m_sen >= 25)
					{
                        m_componentTest1.m_sen = m_componentTest1.m_sen + (0.5f * dt);//缓慢恢复san值
                    }
                   
                }
                   
            }
			else if(Health==1)
			{
                if (m_componentTest1 != null)
                {
                    if (m_componentTest1.m_sen <= 70 && m_componentTest1.m_sen >= 25)
                    {
                        m_componentTest1.m_sen = m_componentTest1.m_sen + (0.5f * dt);//缓慢恢复san值
                    }
					if(m_componentTest1.m_mp<1)
					{
                        m_componentTest1.m_mp = m_componentTest1.m_mp + (0.5f * dt)/m_componentTest1.m_Maxmp;//缓慢恢复san值
                    }

                }
            }


            if (BreathingMode == BreathingMode.Air)
            {
                //如果生物需要呼吸空气，它会检查当前位置是否在水下或者高于一定的高度（259单位），并相应地减少空气值。

                
                int cellContents = m_subsystemTerrain.Terrain.GetCellContents(Terrain.ToCell(position.X), Terrain.ToCell(m_componentCreature.ComponentCreatureModel.EyePosition.Y), Terrain.ToCell(position.Z));
                
                if (worldType == WorldType.StationMoon)//如果是主世界（地球）
				{
					Air = Air - dt / AirCapacity;

                }
				else if(worldType == WorldType.Moon)
				{
                    Air = Air - dt / AirCapacity;
                }
				else
				{
                    Air = ((BlocksManager.Blocks[cellContents] is FluidBlock || position.Y > 600f || position.Y < 0f) ? MathUtils.Saturate(Air - dt / AirCapacity) : 1f);
                }
				if(Air<0f)
				{
					Air = 0f;
				}

            }
            else if (BreathingMode == BreathingMode.Water)
            {
                ////如果生物需要呼吸水，它会检查浸水度是否超过一定的值，并相应地减少空气值。
                Air = ((m_componentCreature.ComponentBody.ImmersionFactor > 0.25f) ? 1f : MathUtils.Saturate(Air - dt / AirCapacity));
            }

            //当生物的身体一定程度上浸没在岩浆中时，将会受到伤害，并且屏幕会有红色效果表示痛苦。
            if (m_componentCreature.ComponentBody.ImmersionFactor > 0f && m_componentCreature.ComponentBody.ImmersionFluidBlock is MagmaBlock)
            {
                Injure(2f * m_componentCreature.ComponentBody.ImmersionFactor * dt, null, false, LanguageControl.Get(m_componentHealth.GetType().Name, 1));
				
                float num2 = 1.1f + 0.1f * (float)MathUtils.Sin(12.0 * m_subsystemTime.GameTime);
                m_redScreenFactor = MathUtils.Max(m_redScreenFactor, num2 * 1.5f * m_componentCreature.ComponentBody.ImmersionFactor);
            }

            //方法计算了生物因为撞击地面而产生的速度变化。如果速度变化大于生物的跌落抗性，将会造成伤害。
            float num3 = MathUtils.Abs(m_componentCreature.ComponentBody.CollisionVelocityChange.Y);
            if (!m_wasStanding && num3 > FallResilience)
            {
                float num4 = MathUtils.Sqr(MathUtils.Max(num3 - FallResilience, 0f)) / 15f;
                if (m_componentPlayer != null)
                {
                    num4 /= m_componentPlayer.ComponentLevel.ResilienceFactor;
                    //m_componentTest1.m_sen = m_componentTest1.m_sen - 1 * dt;//受伤减少sen值
                }
                Injure(num4, null, false, LanguageControl.Get(m_componentHealth.GetType().Name, 2));

            }
            m_wasStanding = (m_componentCreature.ComponentBody.StandingOnValue != null || m_componentCreature.ComponentBody.StandingOnBody != null);


            //每秒进行一次检查，用于处理窒息和着火的伤害。窒息伤害发生在没有空气时（空气值为0），着火伤害则是当生物处于火焰中或触碰到火焰时。
            bool flag = m_subsystemTime.PeriodicGameTimeEvent(1.0, 0.0);
            if (flag && Air == 0f)
            {
                float num5 = 0.12f;
                if (m_componentPlayer != null)
                {
                    num5 /= m_componentPlayer.ComponentLevel.ResilienceFactor;
                }
                Injure(num5, null, false, LanguageControl.Get(m_componentHealth.GetType().Name, 7));
                //m_componentTest1.m_sen = m_componentTest1.m_sen - 1 * dt;//受伤减少sen值
            }
            if (flag && (m_componentOnFire.IsOnFire || m_componentOnFire.TouchesFire))
            {
                float num6 = 1f / FireResilience;
                if (m_componentPlayer != null)
                {
                    num6 /= m_componentPlayer.ComponentLevel.ResilienceFactor;
                }
                Injure(num6, m_componentOnFire.Attacker, false, LanguageControl.Get(m_componentHealth.GetType().Name, 5));
                //m_componentTest1.m_sen = m_componentTest1.m_sen - 1 * dt;//受伤减少sen值
            }
            if (flag && CanStrand && m_componentCreature.ComponentBody.ImmersionFactor < 0.25f)
            {
                //如果生物可以搁浅，并且在地面上但不浸水，将会受到伤害。
                int? standingOnValue = m_componentCreature.ComponentBody.StandingOnValue;
                int num7 = 0;
                if (!(standingOnValue.GetValueOrDefault() == num7 & standingOnValue != null) || m_componentCreature.ComponentBody.StandingOnBody != null)
                {
                    Injure(0.05f, null, false, LanguageControl.Get(m_componentHealth.GetType().Name, 6));
                }
            }

            //记录上一次更新后健康值的变化。
            HealthChange = Health - m_lastHealth;
            m_lastHealth = Health;
            //如果存在红屏效果，随着时间推移减弱这个效果。
            if (m_redScreenFactor > 0.01f)
            {
                m_redScreenFactor *= MathUtils.Pow(0.2f, dt);
            }
            else
            {
                m_redScreenFactor = 0f;
            }


            //当生物受伤时（健康值减少），播放疼痛声音，增加红屏效果，如果是玩家，则让健康条组件闪烁。
            if (HealthChange < 0f)
            {
                m_componentCreature.ComponentCreatureSounds.PlayPainSound();
				
               
                m_redScreenFactor += -4f * HealthChange;
                ComponentPlayer componentPlayer2 = m_componentPlayer;
                if (componentPlayer2 != null)
                {
                    componentPlayer2.ComponentGui.HealthBarWidget.Flash(MathUtils.Clamp((int)((0f - HealthChange) * 30f), 0, 10));
                   
                    if (m_componentTest1 != null)
                    {
                        m_componentTest1.m_sen = m_componentTest1.m_sen - 1f ;//受伤减少sen值
                    }

                }
            }

            //如果生物是玩家，更新红屏效果的因子，以及玩家的健康条显示值。
            if (m_componentPlayer != null)
            {
                m_componentPlayer.ComponentScreenOverlays.RedoutFactor = MathUtils.Max(m_componentPlayer.ComponentScreenOverlays.RedoutFactor, m_redScreenFactor);
            }
            if (m_componentPlayer != null)
            {
                m_componentPlayer.ComponentGui.HealthBarWidget.Value = Health;
            }

            //当健康值为0且健康值有所下降，检查是否有Mod挂钩要求阻止后续的死亡掉落逻辑。

           // 如果没有阻止，则产生死亡粒子效果，丢弃所有物品，并记录死亡时间。
            if (Health == 0f && HealthChange < 0f)
            {
                bool pass = false;
                ModsManager.HookAction("DeadBeforeDrops", delegate (ModLoader loader)
                {
                    bool flag2;
                    loader.DeadBeforeDrops(this, out flag2);
                    pass = (pass || flag2);
                    return false;
                });
                if (!pass)
                {
                    Vector3 position2 = m_componentCreature.ComponentBody.Position + new Vector3(0f, m_componentCreature.ComponentBody.StanceBoxSize.Y / 2f, 0f);
                    float x = m_componentCreature.ComponentBody.StanceBoxSize.X;
                    m_subsystemParticles.AddParticleSystem(new KillParticleSystem(m_subsystemTerrain, position2, x));
                    Vector3 position3 = (m_componentCreature.ComponentBody.BoundingBox.Min + m_componentCreature.ComponentBody.BoundingBox.Max) / 2f;
                    foreach (IInventory inventory in base.Entity.FindComponents<IInventory>())
                    {
                        inventory.DropAllItems(position3);
                    }
                    DeathTime = new double?(m_subsystemGameInfo.TotalElapsedGameTime);
                }
            }
            if (Health <= 0f && CorpseDuration > 0f)
            {
                double? num8 = m_subsystemGameInfo.TotalElapsedGameTime - DeathTime;
                double num9 = (double)CorpseDuration;
                if (num8.GetValueOrDefault() > num9 & num8 != null)
                {
                    m_componentCreature.ComponentSpawn.Despawn();
                }
            }
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("Health", Health);
            valuesDictionary.SetValue<float>("Air", Air);
            if (DeathTime != null)
            {
                valuesDictionary.SetValue<double?>("DeathTime", DeathTime);
            }
            if (!string.IsNullOrEmpty(CauseOfDeath))
            {
                valuesDictionary.SetValue<string>("CauseOfDeath", CauseOfDeath);
            }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemTimeOfDay = base.Project.FindSubsystem<SubsystemTimeOfDay>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemPickables = base.Project.FindSubsystem<SubsystemPickables>(true);
            m_componentCreature = base.Entity.FindComponent<ComponentCreature>(true);
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>();
            m_componentOnFire = base.Entity.FindComponent<ComponentOnFire>(true);
           
            AttackResilience = valuesDictionary.GetValue<float>("AttackResilience");
            FallResilience = valuesDictionary.GetValue<float>("FallResilience");
            FireResilience = valuesDictionary.GetValue<float>("FireResilience");
            m_attackResilience = AttackResilience;
            m_fallResilience = FallResilience;
            m_fireResilience = FireResilience;
            CorpseDuration = valuesDictionary.GetValue<float>("CorpseDuration");
            BreathingMode = valuesDictionary.GetValue<BreathingMode>("BreathingMode");
            CanStrand = valuesDictionary.GetValue<bool>("CanStrand");
            Health = valuesDictionary.GetValue<float>("Health");
            Air = valuesDictionary.GetValue<float>("Air");
            AirCapacity = valuesDictionary.GetValue<float>("AirCapacity");
            double value = valuesDictionary.GetValue<double>("DeathTime");
            AttackResilienceFactor = 1f;
            FallResilienceFactor = 1f;
            FireResilienceFactor = 1f;
            HealFactor = 1f;
            DeathTime = ((value >= 0.0) ? new double?(value) : null);
            CauseOfDeath = valuesDictionary.GetValue<string>("CauseOfDeath");
            if (m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative && base.Entity.FindComponent<ComponentPlayer>() != null)
            {
                IsInvulnerable = true;
            }

            
            m_componentTest1 = m_componentPlayer?.Entity.FindComponent<ComponentTest1>();
            m_subsystemWorldDemo = Project.FindSubsystem<SubsystemWorldDemo>(true);

        }
        


    }
    #endregion

    #region 3D方块外置材质
    public abstract class FCSixFaceBlock : Block
	{
		public Texture2D m_texture;

		public string m_texturename;

		public Color m_color;

		public FCSixFaceBlock(string texturename, Color color)
		{
			m_texturename = texturename;
			m_color = color;
		}

		public override void Initialize()
		{
			m_texture = ContentManager.Get<Texture2D>(m_texturename);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).OpaqueSubsetsByFace);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, m_color, m_color, environmentData, m_texture);
		}

		public override int GetTextureSlotCount(int value)
		{
			return 3;
		}

		public override int GetFaceTextureSlot(int face, int value)
		{
			switch (face)
			{
				case 0: return 0;//  右				
				case 1: return 1; //后
				case 2: return 2; //左
				case 3: return 3;//前
				case 4: return 4;//上
				case 5: return 5;//下
			}
			return 0;
		}
	}
    public abstract class FConeFaceBlock : CubeBlock
    {
        public override void Initialize()
        {
            m_texture = ContentManager.Get<Texture2D>("Textures/FCBlock", null);
        }

		public int index1;
        public FConeFaceBlock(int index)
        {
			index1 = index;//获取贴图位置
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(this, value, x, y, z, Color.White, geometry.GetGeometry(this.m_texture).OpaqueSubsetsByFace);
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, color, environmentData, this.m_texture);
        }
        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(0, value), m_texture);
        }

        public override int GetTextureSlotCount(int value)
        {
            return 16;
        }

        public override int GetFaceTextureSlot(int face, int value)
        {
            switch (face)
            {
                case 0: return index1;//  右				
                case 1: return index1; //后
                case 2: return index1; //左
                case 3: return index1;//前
                case 4: return index1;//上
                case 5: return index1;//下
            }
            return 0;
        }



        public Texture2D m_texture;
    }
    #endregion
    #region 2D方块外置材质
    public abstract class FCTwoDBlock : Block
	{
		public Texture2D m_texture;

		public string texture;

		public FCTwoDBlock(string textureName)
		{
			texture = textureName;
		}

		public override void Initialize()
		{
			base.Initialize();
			m_texture = ContentManager.Get<Texture2D>(texture);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, m_texture, Color.White, isEmissive: true, environmentData);
		}
	}

    #endregion
   

    #region 交叉植物方块
    public abstract class FCCrossBlocks : CrossBlock
	{
		public Texture2D m_texture;
		public string m_textureRoute;

		public FCCrossBlocks(string textureRoute)
		{
			m_textureRoute = textureRoute;
		}
		public override int GetTextureSlotCount(int value)
		{
			return 1;
		}
		public override int GetFaceTextureSlot(int face, int value)
		{
			return 0;
		}
		public override void Initialize()
		{
			m_texture = ContentManager.Get<Texture2D>(m_textureRoute);
		}
		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			color *= BlockColorsMap.GrassColorsMap.Lookup(environmentData.Temperature, environmentData.Humidity);

			BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
		}

	}
    #endregion

    #region 区块（物品）

    #region 物品不动区1.01版本

   

    #region  1.06以后的食物，西瓜汁-橘子
    public class Jiangyoutong : BucketBlock
    {
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
            Model model = ContentManager.Get<Model>("Models/FullBucket", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(79, 56, 32, 255));
            this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override int GetDamageDestructionValue(int value)
        {
            return 245;
        }

        public const int Index = 928;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }//1



    public class Jiutong : BucketBlock
    {
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
            Model model = ContentManager.Get<Model>("Models/FullBucket", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 204, 0, 255));
            this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override int GetDamageDestructionValue(int value)
        {
            return 245;
        }

        public const int Index = 929;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }//1

    public class Rotjiutong : BucketBlock
    {
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
            Model model = ContentManager.Get<Model>("Models/FullBucket", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(135, 206, 235, 255));
            this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override int GetDamageDestructionValue(int value)
        {
            return 245;
        }

        public const int Index = 930;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }//1



    public class Youtong : BucketBlock  //亮黄色
    {
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
            Model model = ContentManager.Get<Model>("Models/FullBucket", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 255, 204, 255));
            this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override int GetDamageDestructionValue(int value)
        {
            return 245;
        }

        public const int Index = 931;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }//1
    public class Xiguazhi : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/FullFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 105, 180));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 954;
		}

		public const int Index = 932;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

	}//1

	public class Pingguozhi : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/FullFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 204, 102));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 954;
		}

		public const int Index = 955;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

	}//1

	public class Juzizhi : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/FullFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 165, 0));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 954;
		}

		public const int Index = 956;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

	}//1

	public class Cafe : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/FullFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(92, 49, 11));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 954;
		}

		public const int Index = 958;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

	}
	public class EmptyFCBucketBlock : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/EmptyFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public const int Index = 953;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}

	public class RottenFruitFCBucketBlock : BucketBlock
	{
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
			Model model = ContentManager.Get<Model>("Models/FullFCBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.DarkGreen);
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override IEnumerable<CraftingRecipe> GetProceduralCraftingRecipes()
		{
			CraftingRecipe craftingRecipe = new CraftingRecipe
			{
				ResultCount = 1,
				ResultValue = 953,
				RequiredHeatLevel = 0f,
				Description = "倒掉腐烂的汁液"
			};
			craftingRecipe.Ingredients[0] = BlocksManager.Blocks[954].CraftingId;
			yield return craftingRecipe;
			yield break;
		}

		public const int Index = 954;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}



	public class Juzi : FCPlatBlock
    {

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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/orange", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
           
            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size*2f , ref matrix, environmentData);
        }
        

		public const int Index = 950;

		private Texture2D m_texture;
	}

	public class Pingguo : FCPlatBlock
    {

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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/apple", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {

            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size * 2f, ref matrix, environmentData);
        }

        public const int Index = 951;

		private Texture2D m_texture;
	}

	public class Cocobean : FCPlatBlock
    {

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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/coco", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {

            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size * 2f, ref matrix, environmentData);
        }

        public const int Index = 952;

		private Texture2D m_texture;
	}

	public class Cocofeng : FCPlatBlock
    {
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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/coco2", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {

            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size * 2f, ref matrix, environmentData);
        }

        public const int Index = 957;

		private Texture2D m_texture;
	}

	public class Chocolate : FCPlatBlock
    {
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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/qiaokeli", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {

            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size * 2f, ref matrix, environmentData);
        }

        public const int Index = 959;

		private Texture2D m_texture;
	}//1

	


	public class YHBeer : FCPlatBlock
    {
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
			base.Initialize();
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/yhbeer", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {

            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_texture, color, size * 2f, ref matrix, environmentData);
        }

        public const int Index = 968;

		private Texture2D m_texture;
	}//1

	public class YHjiutong : BucketBlock //粉色
	{
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
			Model model = ContentManager.Get<Model>("Models/FullBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 192, 203, 255));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 245;
		}

		public const int Index = 966;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}//1
    public class BloodBucket : BucketBlock //粉色
    {
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
            Model model = ContentManager.Get<Model>("Models/FullBucket", null);
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
            Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.DarkRed);
            this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
            this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
            base.Initialize();
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
        }

        public override int GetDamageDestructionValue(int value)
        {
            return 245;
        }

        public const int Index = 962;

        public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    }//1
    public class RotYHjiutong : BucketBlock //淡黄色
	{
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
			Model model = ContentManager.Get<Model>("Models/FullBucket", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bucket", true).ParentBone);
			Matrix boneAbsoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Contents", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Contents", true).MeshParts[0], boneAbsoluteTransform2 * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, new Color(255, 246, 143, 255));
			this.m_standaloneBlockMesh.TransformTextureCoordinates(Matrix.CreateTranslation(0.8125f, 0.6875f, 0f), -1);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Bucket", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateRotationY(MathUtils.DegToRad(180f)) * Matrix.CreateTranslation(0f, -0.3f, 0f), false, false, false, false, Color.White);
			base.Initialize();
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetDamageDestructionValue(int value)
		{
			return 245;
		}

		public const int Index = 967;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}//1
    #endregion



    #endregion
   


    #region 西瓜 萤石 向日葵 1.05版本后新物品
    public class LightStoneBlock : FCSixFaceBlock
	{
		public LightStoneBlock()
		   : base("Textures/FCBlocks/yingshi", Color.White)
		{
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(0, value), m_texture);
		}

		public const int Index = 934;
	}
	public class LightDustBlock : FCTwoDBlock
	{
		public override int GetTextureSlotCount(int value)
		{
			return 1;
		}
		public override int GetFaceTextureSlot(int face, int value)
		{
			if (face == -1) return 0;
			return 2;
		}

		public LightDustBlock()
		   : base("Textures/amod/yingshifeng")
		{
		}

		public const int Index = 935;
	}

	public class RottenXiguaBlock : BaseXiguaBlock
	{

		public RottenXiguaBlock() : base(true)
		{
		}

		public const int Index = 936;
	}

	//植物区，id开始为937
	public class SunFlowerBlock : FlowerBlock //解决1.5外置植物贴图的模板
	{

		public Texture2D m_texture;

		public override void Initialize()
		{
			
			m_texture = ContentManager.Get<Texture2D>("Textures/FCPlants/sunflower");
		}


		public override int GetTextureSlotCount(int value)
		{
			return 1;
		}
		public override int GetFaceTextureSlot(int face, int value)
		{
			if (face == -1) return 0;
			return 2;
		}
		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);
		}
		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			

			BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
		}
		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(0, value), m_texture);
		}


		public const int Index = 937;
	}

	public class XiguaBlock : BaseXiguaBlock
	{
		public XiguaBlock() : base(false)
		{
		}



		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
			int data = Terrain.ExtractData(oldValue);
			if (BaseXiguaBlock.GetSize(data) == 7 && !BaseXiguaBlock.GetIsDead(data) && this.Random.Bool(0.5f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 248,
					Count = 1
				});
			}
		}

		public const int Index = 938;
	}
    public class MoonStoneBlock : FConeFaceBlock//月岩
    {
		public MoonStoneBlock():base(0)//0号位置
		{

		}

        public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
        {
            return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(0, value), m_texture);
        }
       /* public override int GetFaceTextureSlot(int face, int value)
        {
            switch (face)
            {
                case 0:
                    return 0;
                case 1:
                    return 0;
                case 2:
                    return 0;
                case 3:
                    return 0;
                case 4:
                    return 0;
                case 5:
                    return 0;
                default:
                    return 0;
            }
        }*/
        
            
        

        public const int Index = 960;
    }

    #region 树木区
    public class CocoWoodBlock : FCSixFaceBlock
	{
		public const int Index = 939;

		public CocoWoodBlock()
		   : base("Textures/FCBlocks/cocowood", Color.White)
		{
		}
	}
	public class OrangeWoodBlock : WoodBlock
	{
		public OrangeWoodBlock() : base(21, 20)
		{
		}

		public const int Index = 940;
	}

	public class AppleWoodBlock : WoodBlock
	{
		public AppleWoodBlock() : base(21, 20)
		{
		}

		public const int Index = 941;
	}

	public class YinghuaWoodBlock : FCSixFaceBlock
	{
		public YinghuaWoodBlock()
		   : base("Textures/FCBlocks/yhwood", Color.White)
		{
		}

		public const int Index = 943;
	}

	public class LorejunWoodBlock : FCSixFaceBlock
	{
		public LorejunWoodBlock()
		   : base("Textures/FCBlocks/lorejunwood", Color.White)
		{
		}

		public const int Index = 942;
	}

	public class CocoLeavesBlock : LeavesBlock
	{
		public CocoLeavesBlock() : base(BlockColorsMap.OakLeavesColorsMap)
		{
		}
		
		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			if (this.m_random.Bool(0.15f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 23,
					Count = 1
				});
				showDebris = true;
				return;
			}

			if (this.m_random.Bool(0.5f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 952,
					Count = 1
				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
		}

		public const int Index = 944;
	}

	public class OrangeLeavesBlock : FCSixFaceBlock
	{
		public Random m_random = new Random();
		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			if (this.m_random.Bool(0.15f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 23,
					Count = 1

				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);

			if (this.m_random.Bool(0.5f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 950,
					Count = 1
				});
				showDebris = true;
				return;
			}
		}
		public OrangeLeavesBlock()
		   : base("Textures/FCBlocks/orangeleave", Color.White)
		{
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).AlphaTestSubsetsByFace);   //渲染透明效果
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

		public const int Index = 945;
	}

	public class AppleLeavesBlock : FCSixFaceBlock
	{
		public Random m_random = new Random();
		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			if (this.m_random.Bool(0.15f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 23,
					Count = 1
				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);

			if (this.m_random.Bool(0.5f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 951,
					Count = 1
				});
				showDebris = true;
				return;
			}
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).AlphaTestSubsetsByFace);   //渲染透明效果
		}

		public AppleLeavesBlock()
		   : base("Textures/FCBlocks/appleleave", Color.White)
		{
		}

		public const int Index = 946;
	}

	public class LorejunLeavesBlock : LeavesBlock
	{
		public LorejunLeavesBlock() : base(BlockColorsMap.OakLeavesColorsMap)
		{
		}
		

		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			if (this.m_random.Bool(0.15f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 23,
					Count = 1
				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
		}

		public const int Index = 947;
	}

	public class YinghuaLeavesBlock : FCSixFaceBlock //已解决3d方块外置贴图掉落碎片外置问题。
	{
		public Random m_random = new Random();
		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			if (this.m_random.Bool(0.15f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 23,
					Count = 1
				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).AlphaTestSubsetsByFace);   //渲染透明效果
		}

		public YinghuaLeavesBlock()
		   : base("Textures/FCBlocks/yhleave", Color.White)
		{
		}

		public const int Index = 948;
	}

	public class YHPlanksBlock : FCSixFaceBlock
	{
		public YHPlanksBlock()
		   : base("Textures/FCBlocks/yhplanks", Color.White)
		{
		}
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			return ("樱花木板");
		}

		public const int Index = 961;
	}

    #endregion 

   

	public class FCZLSBlock : CubeBlock//已解决3d方块外置贴图掉落碎片外置问题。
	{
		public override int GetTextureSlotCount(int value)
		{
			return 1;
		}
		public override int GetFaceTextureSlot(int face, int value)
		{
			if (face == -1) return 0;
			return DefaultTextureSlot;
		}
		public Texture2D m_texture;

		public override void Initialize()
		{

			m_texture = ContentManager.Get<Texture2D>("Textures/FCBlocks/ZLS");
		}


		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, Color.White, geometry.GetGeometry(m_texture).OpaqueSubsetsByFace);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, color, environmentData, m_texture);
		}



		public const int Index = 980;


		/*public class FCYHGrassBlock : Block
		{
			public override int GetFaceTextureSlot(int face, int value)
			{
				if (face == 4)
				{
					return 0;
				}
				if (face == 5)
				{
					return 2;
				}
				if (Terrain.ExtractData(value) == 0)
				{
					return 3;
				}
				return 68;
			}

			public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
			{
				Color topColor = Color.Green;
				BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, topColor, environmentData);
			}

			public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
			{
				Color topColor = Color.Green;
				Color topColor2 = Color.Green;
				Color topColor3 = Color.Green;
				Color topColor4 = Color.Green;
				generator.GenerateCubeVertices(this, value, x, y, z, 1f, 1f, 1f, 1f, Color.White, topColor, topColor2, topColor3, topColor4, -1, geometry.OpaqueSubsetsByFace);
			}

			public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
			{
				return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, this.DestructionDebrisScale, Color.White, 2);
			}

			public  const int Index = 984;
		}*/

		/*public class FCRDGrassBlock : Block
		{
			public override int GetFaceTextureSlot(int face, int value)
			{
				if (face == 4)
				{
					return 0;
				}
				if (face == 5)
				{
					return 2;
				}
				if (Terrain.ExtractData(value) == 0)
				{
					return 3;
				}
				return 68;
			}

			public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
			{
				Color topColor = new Color(0, 128, 0);
				BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, topColor, environmentData);
			}

			public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
			{
				Color topColor = new Color(0, 128, 0);
				Color topColor2 = new Color(0, 128, 0);
				Color topColor3 = new Color(0, 128, 0);
				Color topColor4 = new Color(0, 128, 0);
				generator.GenerateCubeVertices(this, value, x, y, z, 1f, 1f, 1f, 1f, Color.White, topColor, topColor2, topColor3, topColor4, -1, geometry.OpaqueSubsetsByFace);
			}

			public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
			{
				return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, this.DestructionDebrisScale, Color.White, 2);
			}

			public const int Index = 985;
		}*/
	}



	#endregion
	#endregion

	#region 物品子系统BUFF
	public class ChocolateSystem : SubsystemBlockBehavior
	{
        public SubsystemAudio m_subsystemAudio;
        public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					959,//巧克力
					932,//西瓜
					955,//苹果
					956,//橘子
					958,//咖啡
					968,//樱花酒
					987,//进阶食物和药水
				};
			}
		}
	
	    
		public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
		{
			//1 heal 2.Atk 3.速度
			ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
            BuffManager buffManager = new BuffManager(componentPlayer);
            var m_componentTest1 = componentPlayer.Entity.FindComponent<ComponentTest1>();
            int F987ID = Terrain.ExtractContents(componentMiner.ActiveBlockValue);
			if (componentPlayer != null)
			{
				if(componentMiner.ActiveBlockValue==959)
                {
					componentPlayer.ComponentVitalStats.Food = 1f;
					componentPlayer.ComponentVitalStats.Stamina +=0.7f;//耐力
					
					componentPlayer.ComponentGui.DisplaySmallMessage("吃了蛋仔牌巧克力,你不再感到饥饿。你感觉浑身充满了力量！（耐力和饱食度大量恢复！）", Color.White, true, true);
                    if (m_componentTest1 != null)
					{
                        m_componentTest1.m_sen += 20f;//提升sen值！
                    }
                       

                    AudioManager.PlaySound("Audio/Creatures/HumanEat/HumanEat1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
				} //巧克力
				else if(componentMiner.ActiveBlockValue == 932)
                {
					
                    
					buffManager.AddBuff(1, 2f, 0.5f);
					//componentHealBuffA.StartHealBuff(2f, 0.5f);//否则，设置一个两秒，速率0.5的恢复效果
					
					
					componentPlayer.ComponentGui.DisplaySmallMessage("喝下了西瓜汁,你的生命略微恢复了", Color.White, true, true);
					AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
				}//西瓜
				else if (componentMiner.ActiveBlockValue == 955)
				{
					
					
					buffManager.AddBuff(1, 2f, 1f);
					//componentHealBuffA.StartHealBuff(2f, 1.5f);//否则，设置一个两秒，速率0.5的恢复效果
					
					if (componentPlayer.ComponentSickness.IsSick==true)
                    {
						componentPlayer.ComponentSickness.m_sicknessDuration -= 30f;
					}
					
					if(componentPlayer.ComponentFlu.HasFlu==true)//如果正在生病
                    {
						componentPlayer.ComponentFlu.m_fluDuration -= 30f;
					}
					
					componentPlayer.ComponentGui.DisplaySmallMessage("喝下了苹果汁,你的生命略微恢复了,你恢复了些许健康(感冒时间-30秒，生病呕吐时间-30秒））。", Color.White, true, true);
					AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
				}//苹果
				else if (componentMiner.ActiveBlockValue == 956)
				{
					
					
					buffManager.AddBuff(1, 5f, 0.25f);
					//componentHealBuffA.StartHealBuff(5f, 0.25f);//否则，设置一个5秒，速率0.25的恢复效果
					
					if (componentPlayer.ComponentFlu.HasFlu == true)//如果正在生病
					{
						componentPlayer.ComponentFlu.m_fluDuration -= 100f;
					}
					componentPlayer.ComponentGui.DisplaySmallMessage("喝下了橘子汁,你的生命略微恢复了.橘子富含大量维生素c，感冒时间-100秒", Color.White, true, true);
					AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
				}//橘子
				else if (componentMiner.ActiveBlockValue == 958)
				{

					componentPlayer.ComponentVitalStats.Sleep = 1f;
					
					componentPlayer.ComponentGui.DisplaySmallMessage("喝下了咖啡,你的疲劳一扫而空！", Color.White, true, true);
                    if (m_componentTest1 != null)
					{
                        m_componentTest1.m_sen += 20f;
                    }
                        
                    AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
				} //咖啡
				else if (componentMiner.ActiveBlockValue == 968)
				{
					
					
			
					buffManager.AddBuff(2, 8f, 5f);
					//componentAttackUP.StartATKBuff(8f, 5f);//否则，设置一个8，5%的攻击加成
					buffManager.AddBuff(3, 8f, 4f);
					//componentSpeedUP.StartSpeedBuff(8f, 4f);//否则，设置一个8秒，4%的加速效果
					

					componentPlayer.ComponentHealth.Injure(0.1f,null,false,"粗制的樱花酒似乎有些许副作用……");
                    if (m_componentTest1 != null)
					{
                        m_componentTest1.m_sen -= 1f;
                    }
                       
                    componentPlayer.ComponentGui.DisplaySmallMessage("喝下樱花酒，你的攻击力和速度略微提升！", Color.White, true, true);
					AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
                }//樱花酒
				else if(F987ID == 987)
				{
					int ID = Terrain.ExtractData(componentMiner.ActiveBlockValue);
					if(ID==0)//实体灵能
					{
						//m_componentTest1.m_mp += 10*1/ m_componentTest1.MaxMagicPower;
                        //m_componentTest1.m_Maxmp += 10;
                        m_componentTest1.m_mplevel += 0.01f;
						m_componentTest1.m_mp += 0.5f;
                        componentPlayer.ComponentGui.DisplaySmallMessage("I need More POWER!", Color.LightYellow, true, true);
                        //m_subsystemAudio.PlaySound("Audio/MagicPowerGet", 1f, 0f, 0f,0.1f);
                    }
                    else if (ID ==6)//夜视
					{
						buffManager.AddBuff(9, 20f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("喝下夜视药水，你终于摆脱了无尽的黑夜！", Color.Purple, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f,0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 7)//治疗
                    {
                        buffManager.AddBuff(1, 60f,3);
                        componentPlayer.ComponentGui.DisplaySmallMessage("痛苦很快就会消失……", Color.LightRed ,true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f,0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 8)//攻击力加成
                    {
                        buffManager.AddBuff(2, 20f,10f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("勇敢战斗吧！", Color.DarkRed, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f,0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 9)//速度加成
                    {
                        buffManager.AddBuff(3, 20f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("迅疾如风！", Color.Green, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 10)//跳跃加成
                    {
                        buffManager.AddBuff(10, 60f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("喝下这个你会跳的和兔蛋仔一样高", Color.LightGreen, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 11)//经验加成
                    {
						componentPlayer.ComponentLevel.AddExperience(10, false);
                        componentPlayer.ComponentGui.DisplaySmallMessage("一瓶经验药水提供10点经验", Color.LightYellow, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 12)//灵能药水
                    {
						m_componentTest1.m_mp += 0.2f;
                        componentPlayer.ComponentGui.DisplaySmallMessage("快速恢复枯竭的灵能（不同等级灵能药水恢复不同）", Color.LightYellow , true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 13)//剧毒
                    {
						componentPlayer.ComponentHealth.Health = 0.5f * componentPlayer.ComponentHealth.Health;
                        componentPlayer.ComponentGui.DisplaySmallMessage("最毒不过妇人心。", Color.Purple, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 14)//致盲
                    {
                        buffManager.AddBuff(4, 5f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("你感觉眼前好像蒙了一层黑障……", Color.Gray, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 15)//瘟疫
                    {
                        
                        componentPlayer.ComponentGui.DisplaySmallMessage("这个药水会让你立刻生病！", Color.DarkGreen, true, true);
						componentPlayer.ComponentSickness.StartSickness();
						componentPlayer.ComponentFlu.StartFlu();
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 16)//迟缓
                    {
                        buffManager.AddBuff(6, 10f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("你感觉你的双脚如同灌了铅一样……", new Color(60,60,60), true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 17)//眩晕
                    {
                        buffManager.AddBuff(11, 3f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("脑袋晕乎乎的……", Color.White, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 18)//降sen药水
                    {
                        m_componentTest1.m_sen -= 100f;
                        componentPlayer.ComponentGui.DisplaySmallMessage("为什么要想不开喝这个？或者说……你准备好了……", Color.DarkRed, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                    else if (ID == 19)//升sen
                    {
						m_componentTest1.m_sen += 100f;
                        componentPlayer.ComponentGui.DisplaySmallMessage("永远幸福……", Color.DarkRed, true, true);
                        m_subsystemAudio.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f, 0.1f);
                        componentMiner.RemoveActiveTool(1);
                    }
                }


            }
			
			return true;
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
	#region 进阶食物
	#region 特殊值区。fc物品987
	public class FCDVFoodBlock : FCPlatBlock
	{
		public const int Index = 987;
		private Texture2D[] m_texture = new Texture2D[20];
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
			for (int i = 0; i < 20; i++)
			{
				this.m_texture[i] = ContentManager.Get<Texture2D>(FCDVFoodBlock.m_textureNames[i], null);
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
			float num2 = (num >= 0 && num < FCDVFoodBlock.m_sizes.Length) ? (size * FCDVFoodBlock.m_sizes[num]) : size;
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
			0.5f,
			1f,
			1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f, //传动活塞
			1f, //钢锭
			1f,
			1f,
			1f,
            1f,
        };

		public override IEnumerable<int> GetCreativeValues()
		{
			foreach (int data in EnumUtils.GetEnumValues(typeof(FCDVFoodBlock.ItemType)))
			{
				yield return Terrain.MakeBlockValue(987, 0, data);
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
			100000,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
			1024,
		};
		//类名字
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			int num = Terrain.ExtractData(value);
			if (num < 0 || num >= FCDVFoodBlock.m_displayNames.Length)
			{
				return string.Empty;
			}
			return FCDVFoodBlock.m_displayNames[num];
		}

		private static string[] m_displayNames = new string[]
		{
            "实体灵能",//0
            "基础的闪烁西瓜",//1
            "基础的闪烁苹果",//2
            "基础的闪烁橘子",//3
            "基础的闪烁可可豆",//4
            "基础的闪烁可可粉",//5
            "夜视药水I",//6
            "治疗药水I",//7
            "攻击力药水I",//8
            "速度药水I",//9
            "跳跃药水I",//10
            "经验药水I",//11
            "灵能药水I",//12
            "剧毒药水I",//13
            "致盲药水I",//14
            "瘟疫药水I",//15
            "迟缓药水I",//16
            "眩晕药水I",//17
            "降sen药水I",//18
            "升sen药水I",//19


		};

		private static string[] m_textureNames = new string[]
		{
			"Textures/FCDVFood/EnMP",
			"Textures/FCDVFood/BSmelon",
			"Textures/FCDVFood/BSaplle",
			"Textures/FCDVFood/BSorange",
			"Textures/FCDVFood/BSCocobean",
			"Textures/FCDVFood/BSCoco",
            "Textures/FCDVFood/yeshi",
            "Textures/FCDVFood/Heal",
			"Textures/FCDVFood/ATKUP",
			"Textures/FCDVFood/Speed",
			"Textures/FCDVFood/Jump",//齿轮
			"Textures/FCDVFood/EXP",
			"Textures/FCDVFood/MP",
			"Textures/FCDVFood/Poison",//剧毒
			"Textures/FCDVFood/Blind",
            "Textures/FCDVFood/Disease",
			"Textures/FCDVFood/Slowdown",
			"Textures/FCDVFood/Dizzy",
			"Textures/FCDVFood/SenDown",
			"Textures/FCDVFood/Senup",

		};
		//动态材质
		private static string[] m_frostPower = new string[]
		{







		};

		private Texture2D[] m_frostPowerTexture = new Texture2D[7];

		public enum ItemType
		{
			Lingneng, //1实体灵能
			BsShiningMelon, //2基础的闪烁西瓜
			Bsapple,//3基础的闪烁苹果
			Bsorange,//4.基础的闪烁橘子
			Cocobean,//5.基础的闪烁可可豆
			Coco,//6基础的闪烁可可粉
			NightSeeI, //7夜视药水I
			HealI,//8治疗I
			ATKUPI, //9 攻击力加成1
			SpeedUPI,//10速度加成1
			JumpI,//11跳跃加成
			LevelI,//12经验药水I
			LingnengI,//13灵能药水I
			poisonI,//14剧毒药水I
			BlindI,//15致盲药水I
			DiseaseI,//16瘟疫药水I
			SlowdownI,//17迟缓药水I
			DizzyI,//18眩晕药水I
			SenDown,//19降sen药水I
			SenUp,//20升sen药水I

		}

		public override string GetDescription(int value)
		{
			int num = Terrain.ExtractData(value);
			if (num < 0 || num >= FCDVFoodBlock.m_Description.Length)
			{
				return string.Empty;
			}
			return FCDVFoodBlock.m_Description[num];

		}

		private static string[] m_Description = new string[]
		{
			"实体灵能",
            "基础的闪烁西瓜",
            "基础的闪烁苹果",
            "基础的闪烁橘子",
            "基础的闪烁可可豆",
            "基础的闪烁可可粉",
            "夜视药水I",
            "治疗药水I",
            "攻击力药水I",
            "速度药水I",
            "跳跃药水I",
            "经验药水I",
            "灵能药水I",
            "剧毒药水I",
            "致盲药水I",
            "瘟疫药水I",
            "迟缓药水I",
            "眩晕药水I",
            "降sen药水I",
            "升sen药水I",


        };
	}

    

    #endregion
    #endregion
    #region buff体系
    public  class BuffManager
    {
        public ComponentPlayer componentPlayer;

        public BuffManager(ComponentPlayer player)
        {
            componentPlayer = player;
        }
		//public ComponentHealBuffA componentHealBuffA;
        public void StopBuffs()
		{
            var componentHealBuffA = componentPlayer.Entity.FindComponent<ComponentHealBuffA>();
            var componentAttackUP = componentPlayer.Entity.FindComponent<ComponentAttackUP>();
            var componentSpeedUP = componentPlayer.Entity.FindComponent<ComponentSpeedUP>();
            var componentNightsight = componentPlayer.Entity.FindComponent<ComponentNightsight>();
            var componentBlind = componentPlayer.Entity.FindComponent<ComponentBlind>();
            var componentSlow = componentPlayer.Entity.FindComponent<ComponentSlowDown>();
            var componentJump = componentPlayer.Entity.FindComponent<ComponentJump>();
            var componentDizzy = componentPlayer.Entity.FindComponent<ComponentDizzy>();

			componentHealBuffA.StopBuff();
            componentAttackUP.StopBuff();
            componentSpeedUP.StopBuff();
            componentNightsight.StopBuff();
            componentBlind.StopBuff();
            componentSlow.StopBuff();
            componentJump.StopBuff();
            componentDizzy.StopBuff();

        }//解除所有buff。
        public void AddBuffOnUse(int buffType, int activeBlockValue, float duration, float rate)
        {
            switch (activeBlockValue)
            {
                case 932:
                    var componentHealBuffA = componentPlayer.Entity.FindComponent<ComponentHealBuffA>();
                   
                    componentHealBuffA.StartHealBuff(duration, rate);
                    break;
                case 968:
                    // Apply attack and speed buffs
                    //ApplyAttackBuff(duration, rate);
                    //ApplySpeedBuff(duration, rate);
                    break;
                
                default:
                    throw new ArgumentException("Unknown active block value");
            }
        }
        public void AddBuff(int buffType, float duration, float rate = 1f)//直接加buff，仅根据buff类别进行固定加成。
        {
            switch (buffType)
            {
                case BuffTypes.Heal://如果是治疗 一级治疗基础回复是每秒一血。不算被倍率。
                    float MaxDuration = 120f;//提供统一接口参数
					float MaxRate = 10f;
                    var componentHealBuffA = componentPlayer.Entity.FindComponent<ComponentHealBuffA>();
                    if (componentHealBuffA != null)
                    {
                        if (componentHealBuffA.IsActive==false)// 如果不在激活状态
                        {
                            componentHealBuffA.StartHealBuff(duration, rate);
                        }
						else
						{
							if(componentHealBuffA.m_HealDuration> MaxDuration)
							{
                                componentHealBuffA.m_HealDuration = MaxDuration;
                            }
							else if (componentHealBuffA.m_HealDuration < MaxDuration)//20秒封顶
							{
                                componentHealBuffA.m_HealDuration += 10f;
                                if (componentHealBuffA.m_HealDuration> MaxDuration)
								{
                                    componentHealBuffA.m_HealDuration = MaxDuration;
                                }
							}

							if(componentHealBuffA.m_HealRate> MaxRate)//倍率至多为10，每秒恢复10
							{
								componentHealBuffA.m_HealRate = MaxRate;
							}
							else if (componentHealBuffA.HealingRate< MaxRate)
							{
                                componentHealBuffA.m_HealRate = 1.01f * componentHealBuffA.m_HealRate;
								if(componentHealBuffA.m_HealRate > MaxRate)
								{
									componentHealBuffA.m_HealRate = MaxRate;
								}
                            }
                            
                        }
                        
                    }
                    break;
                case BuffTypes.AttackUp://如果是攻击加成 ，攻击加成第一次生效的时候，公式是 原始攻击力 x（1+攻击力加成倍率/100）+2
                    var componentAttackUP = componentPlayer.Entity.FindComponent<ComponentAttackUP>();
					float MaxDurationATK= 60f;    
					float MaxATK= 50f;
					float MaxATKRate = 1.02f;//延续攻击buff时候的最大的攻击力加成
                    if (componentAttackUP != null)
                    {
                        if (componentAttackUP.IsActive==false)
                        {
                            componentAttackUP.StartATKBuff(duration, rate);
                        }
						else
						{
							float originATK = componentAttackUP.ATK_first;//获取原始攻击力。
                            if (componentAttackUP.m_ATKDuration > MaxDurationATK)
                            {
                                componentAttackUP.m_ATKDuration = MaxDurationATK;
                            }
                            else if (componentAttackUP.m_ATKDuration < MaxDurationATK)//20秒封顶
                            {
                                componentAttackUP.m_ATKDuration += 10f;
                                if (componentAttackUP.m_ATKDuration > MaxDurationATK)
                                {
                                    componentAttackUP.m_ATKDuration = MaxDurationATK;
                                }
                            }

                            if (componentPlayer.ComponentMiner.AttackPower>MaxATK+originATK)//最大攻击加成不超过原始攻击力+15
                            {
								componentPlayer.ComponentMiner.AttackPower = MaxATK+originATK;
                               
                            }
                            else if (componentPlayer.ComponentMiner.AttackPower < MaxATK+ originATK)
                            {
                                componentPlayer.ComponentMiner.AttackPower = componentPlayer.ComponentMiner.AttackPower * MaxATKRate+1; //延续攻击效果的时候，按百分比加成

                                if (componentPlayer.ComponentMiner.AttackPower > MaxATK+ originATK)
                                {
                                    componentPlayer.ComponentMiner.AttackPower = MaxATK+ originATK;
                                }
                            }
                        }
                        
                    }
                    break;
                case BuffTypes.SpeedUp://如果是速度加成,公式是 原始速度 x（1+速度加成倍率/100）+2
                    var componentSpeedUP = componentPlayer.Entity.FindComponent<ComponentSpeedUP>();
                    float MaxDurationSpeed = 60f;
                    float MaxSpeed = 7f;
                    float MaxSpeedRate = 1.01f;//延续速度buff时候的最大的速度加成 目前百分之一
                    if (componentSpeedUP != null)
                    {
                        if (componentSpeedUP.IsActive==false)
                        {
                            componentSpeedUP.StartSpeedBuff(duration, rate);
                        }
                        else
                        {
                            float originSpeed = componentSpeedUP.Speed_first;//获取原始速度。
                            if (componentSpeedUP.m_SpeedDuration > MaxDurationSpeed)
                            {
                                componentSpeedUP.m_SpeedDuration = MaxDurationSpeed;
                            }
                            else if (componentSpeedUP.m_SpeedDuration < MaxDurationSpeed)//20秒封顶
                            {
                                componentSpeedUP.m_SpeedDuration += 10f;
                                if (componentSpeedUP.m_SpeedDuration > MaxDurationSpeed)
                                {
                                    componentSpeedUP.m_SpeedDuration = MaxDurationSpeed;
                                }
                            }

                            if (componentPlayer.ComponentLocomotion.WalkSpeed> MaxSpeed + originSpeed)//最大速度不超过7
                            {
                                componentPlayer.ComponentLocomotion.WalkSpeed = MaxSpeed + originSpeed;

                            }
                            else if (componentPlayer.ComponentLocomotion.WalkSpeed < MaxSpeed + originSpeed)
                            {
                                componentPlayer.ComponentLocomotion.WalkSpeed = componentPlayer.ComponentLocomotion.WalkSpeed * MaxSpeedRate; //延续攻击效果的时候，按百分比加成

                                if (componentPlayer.ComponentLocomotion.WalkSpeed > MaxSpeed + originSpeed)
                                {
                                    componentPlayer.ComponentLocomotion.WalkSpeed = MaxSpeed + originSpeed;
                                }
                            }
                        }

                    }
                    break;
                case BuffTypes.Nightsight://如果是速度加成,公式是 原始速度 x（1+速度加成倍率/100）+2
                    var componentNightsight = componentPlayer.Entity.FindComponent<ComponentNightsight>();
                    float MaxDurationNightsee = 120f;
                   
                    if (componentNightsight  != null)
                    {
                        if (componentNightsight .IsActive == false)
                        {
                            componentNightsight .StartNightsightBuff(duration);
                        }
                        else
                        {
                            
                            if (componentNightsight.m_NightseeDuration > MaxDurationNightsee)
                            {
                                componentNightsight .m_NightseeDuration = MaxDurationNightsee;
                            }
                            else if (componentNightsight .m_NightseeDuration < MaxDurationNightsee)//20秒封顶
                            {
                                componentNightsight .m_NightseeDuration += 60f;//加10秒
                                if (componentNightsight .m_NightseeDuration > MaxDurationNightsee)
                                {
                                    componentNightsight.m_NightseeDuration = MaxDurationNightsee;
                                }
                            }

                           
                        }

                    }
                    break;
                case BuffTypes.Blind://如果是速度加成,公式是 原始速度 x（1+速度加成倍率/100）+2
                    var componentBlind = componentPlayer.Entity.FindComponent<ComponentBlind>();
                    float MaxDurationBlind = 5f;

                    if (componentBlind != null)
                    {
                        if (componentBlind.IsActive == false)
                        {
                            componentBlind.StartBlindBuff(duration,rate);
                        }
                        else
                        {

                            if (componentBlind.m_BlindDuration > MaxDurationBlind)
                            {
                                componentBlind.m_BlindDuration = MaxDurationBlind;
                            }
                            else if (componentBlind.m_BlindDuration < MaxDurationBlind)//20秒封顶
                            {
                                componentBlind.m_BlindDuration += 5f;//加10秒
                                if (componentBlind.m_BlindDuration > MaxDurationBlind)
                                {
                                    componentBlind.m_BlindDuration = MaxDurationBlind;
                                }
                            }


                        }

                    }
                    break;
                case BuffTypes.SlowDown://如果是
                    var componentSlow = componentPlayer.Entity.FindComponent<ComponentSlowDown>();
                    float MaxDurationSlow = 5f;

                    if (componentSlow != null)
                    {
                        if (componentSlow.IsActive == false)
                        {
                            componentSlow.StartBuff(duration, rate);
                        }
                        else
                        {

                            if (componentSlow.m_SlowDuration > MaxDurationSlow)
                            {
                                componentSlow.m_SlowDuration = MaxDurationSlow;
                            }
                            else if (componentSlow.m_SlowDuration < MaxDurationSlow)//20秒封顶
                            {
                                componentSlow.m_SlowDuration += 5f;//加10秒
                                if (componentSlow.m_SlowDuration > MaxDurationSlow)
                                {
                                    componentSlow.m_SlowDuration = MaxDurationSlow;
                                }
                            }


                        }

                    }
                    break;
                case BuffTypes.Jump://如果是跳跃加成
                    var componentJump = componentPlayer.Entity.FindComponent<ComponentJump>();
                    float MaxDurationJump = 30f;

                    if (componentJump != null)
                    {
                        if (componentJump.IsActive == false)
                        {
                            componentJump.StartJumpBuff(duration, rate);
                        }
                        else
                        {

                            if (componentJump.m_JumpDuration > MaxDurationJump)
                            {
                                componentJump.m_JumpDuration = MaxDurationJump;
                            }
                            else if (componentJump.m_JumpDuration < MaxDurationJump)//30秒封顶
                            {
                                componentJump.m_JumpDuration += 5f;//加5秒
                                if (componentJump.m_JumpDuration > MaxDurationJump)
                                {
                                    componentJump.m_JumpDuration = MaxDurationJump;
                                }
                            }


                        }

                    }
                    break;
                case BuffTypes.Dizzy://如果是跳跃加成
                    var componentDizzy = componentPlayer.Entity.FindComponent<ComponentDizzy>();
                    float MaxDurationDizzy = 5f;

                    if (componentDizzy != null)
                    {
                        if (componentDizzy.IsActive == false)
                        {
                            componentDizzy.StartDizzyBuff(duration, rate);
                        }
                        else
                        {

                            if (componentDizzy.m_DizzyDuration > MaxDurationDizzy)
                            {
                                componentDizzy.m_DizzyDuration = MaxDurationDizzy;
                            }
                            else if (componentDizzy.m_DizzyDuration < MaxDurationDizzy)//30秒封顶
                            {
                                componentDizzy.m_DizzyDuration += 3f;//加5秒
                                if (componentDizzy.m_DizzyDuration > MaxDurationDizzy)
                                {
                                    componentDizzy.m_DizzyDuration = MaxDurationDizzy;
                                }
                            }


                        }

                    }
                    break;
                default:
                    throw new ArgumentException("Unknown buff type");
            }
        }
    }

    public static class BuffTypes
    {
        public const int Heal = 1;
        public const int AttackUp = 2;
        public const int SpeedUp = 3;
		public const int Blind = 4;
		public const int AttackDown = 5;
		public const int SlowDown = 6;
		public const int Nofire = 7;
		public const int Onfire = 8;
        public const int Nightsight = 9;
        public const int Jump = 10;
        public const int Dizzy= 11;



    }
	#region buff区域

	public class ComponentHealBuffA : Component, IUpdateable
	{
        public BuffParticleSystem onBuffParticleSystem;
        public float HealingRate
		{
			get
			{
				return m_HealRate ;
			}
		}//恢复速率
		//public bool flag_1 =false;//控制消息提示
		//public bool flag_2 =false;

		public bool IsActive 
		{
			get
			{
				return this.m_HealDuration > 0f;
			}
		}//标记buff是否已经激活

		public UpdateOrder UpdateOrder
		{
			get
			{
				return UpdateOrder.Default;
			}
		}

		
		public void StartHealBuff(float healtime,float healrate)
		{
			m_HealRate = healrate;
			m_HealDuration = healtime;
			/*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/
			
		}

		public void StopBuff()
		{
			
			if (m_componentPlayer != null)
            {
                
                m_HealDuration = 0f;
				m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已停止！", Color.Red, true, false);
				
			}
				
		}

		public  void Update(float dt)
		{
           
            if (IsActive == true)
            {
				if (m_HealDuration > 0 && HealingRate > 0)//如果持续时间大于0，说明处于生命恢复状态
				{
					if(onBuffParticleSystem==null)
					{
                        onBuffParticleSystem = new BuffParticleSystem(10)
                        {
                            Texture = ContentManager.Get<Texture2D>("Textures/Star", null)
                        };
                        m_subsystemParticles.AddParticleSystem(onBuffParticleSystem);
                    }
                    
                    BoundingBox boundingBox2 = m_componentCreature.ComponentBody.BoundingBox;
                    onBuffParticleSystem.Position = 0.5f * (boundingBox2.Min + boundingBox2.Max);
                    onBuffParticleSystem.Radius = 0.5f * MathUtils.Min(boundingBox2.Max.X - boundingBox2.Min.X, boundingBox2.Max.Z - boundingBox2.Min.Z);
                    onBuffParticleSystem.Color = Color.Lerp(Color.LightGreen, Color.Green, MathUtils.Saturate((m_HealDuration - m_componentCreature.ComponentHealth.AirCapacity) / m_HealDuration));
                    onBuffParticleSystem.Size = m_componentCreature.ComponentBody.BoxSize.XZ * 0.1f;

                    m_componentPlayer.ComponentHealth.Heal((1f / m_componentPlayer.ComponentHealth.AttackResilience) * HealingRate*dt);//生命恢复=1x恢复速率
					m_HealDuration = m_HealDuration - dt;
					if (m_HealDuration <= 0)
					{
						if(onBuffParticleSystem!= null)
						{
                            onBuffParticleSystem.IsStopped = true;
							onBuffParticleSystem = null;

                            
                        }
						StopBuff();
					}
				}
				/*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
			}

			


			
		}
        public override void OnEntityRemoved()
        {
            
                bool flag = onBuffParticleSystem != null;
                if (flag)
                {
                    onBuffParticleSystem.IsStopped = true;
                    onBuffParticleSystem = null;
                    
                  
                }
            
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
			m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
			m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentPlayer = m_componentCreature.Entity.FindComponent<ComponentPlayer>();
            m_HealDuration = valuesDictionary.GetValue<float>("HealDuration",0f);
			m_HealRate = valuesDictionary.GetValue<float>("HealRate",1f);
		}
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			valuesDictionary.SetValue<float>("HealDuration", m_HealDuration);
			valuesDictionary.SetValue<float>("HealRate", m_HealRate);
		}
		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTerrain m_subsystemTerrain;
        public ComponentCreature m_componentCreature;
        public SubsystemTime m_subsystemTime;
        
        public SubsystemAudio m_subsystemAudio;

		public SubsystemParticles m_subsystemParticles;
	    public ComponentPlayer m_componentPlayer;
	
		public float m_HealRate;//保存生命恢复速率
		public float m_HealDuration;//buff持续时间，需要在存档读取。
		public Random m_random = new Random();
		
		
	}
	public class ComponentAttackUP : Component, IUpdateable
	{
        public BuffParticleSystem onBuffParticleSystem;
        public bool Isadded1 
		{
            get
            {
                return isadded;
            }
        }
		public bool isadded=false ;
		public float ATK_first;
		public float ATKRate
		{
			get
			{
				return m_AttackRate;
			}
		}//恢复速率
		 //public bool flag_1 =false;//控制消息提示
		 //public bool flag_2 =false;

		public bool IsActive
		{
			get
			{
				return this.m_ATKDuration > 0f;
			}
		}//标记buff是否已经激活

		public UpdateOrder UpdateOrder
		{
			get
			{
				return UpdateOrder.Default;
			}
		}


		public void StartATKBuff(float healtime, float healrate)
		{
			m_AttackRate = healrate;
			m_ATKDuration = healtime;
			/*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

		}

		public void StopBuff()
		{

			if (m_componentPlayer != null)
			{
				m_ATKDuration = 0f;
				m_componentPlayer.ComponentGui.DisplaySmallMessage("攻击力强化结束！", Color.DarkRed, true, false);
				isadded = false;
				m_componentPlayer.ComponentMiner.AttackPower = ATK_first;
				ATK_first = 0f;
			}

		}

		public void Update(float dt)
		{
			if (IsActive == true)
			{
				if (m_ATKDuration > 0 && ATKRate > 0)//如果持续时间大于0，说明处于xx状态
				{
					if(Isadded1==false)
                    {
						if(ATK_first==0)
						{
                            ATK_first = m_componentPlayer.ComponentMiner.AttackPower;
                            m_componentPlayer.ComponentMiner.AttackPower = m_componentPlayer.ComponentMiner.AttackPower * (1f + ATKRate / 100) + 2;
                           
                        }
						
						
						isadded = true;
					}
                    if (onBuffParticleSystem == null)
                    {
                        onBuffParticleSystem = new BuffParticleSystem(10)
                        {
                            Texture = ContentManager.Get<Texture2D>("Textures/Star", null)
                        };
                        m_subsystemParticles.AddParticleSystem(onBuffParticleSystem);
                    }

                    BoundingBox boundingBox2 = m_componentCreature.ComponentBody.BoundingBox;
                    onBuffParticleSystem.Position = 0.5f * (boundingBox2.Min + boundingBox2.Max);
                    onBuffParticleSystem.Radius = 0.5f * MathUtils.Min(boundingBox2.Max.X - boundingBox2.Min.X, boundingBox2.Max.Z - boundingBox2.Min.Z);
                    onBuffParticleSystem.Color = Color.Lerp(Color.LightRed, Color.DarkRed, MathUtils.Saturate(m_ATKDuration /20f));
                    onBuffParticleSystem.Size = m_componentCreature.ComponentBody.BoxSize.XZ * 0.1f;

                    m_ATKDuration = m_ATKDuration - dt;
					if (m_ATKDuration <= 0)
					{
                        if (onBuffParticleSystem != null)
                        {
                            onBuffParticleSystem.IsStopped = true;
                            onBuffParticleSystem = null;


                        }
                        StopBuff();
					}
				}

				/*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
			}
			





		}
        public override void OnEntityRemoved()
        {

            bool flag = onBuffParticleSystem != null;
            if (flag)
            {
                onBuffParticleSystem.IsStopped = true;
                onBuffParticleSystem = null;


            }

        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
			m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
			m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
		
            m_componentCreature= Entity.FindComponent<ComponentCreature>(true);
            m_componentPlayer = m_componentCreature.Entity.FindComponent<ComponentPlayer>();
            m_ATKDuration = valuesDictionary.GetValue<float>("ATKDuration", 0f);
			m_AttackRate = valuesDictionary.GetValue<float>("ATKRate", 1f);
			ATK_first = valuesDictionary.GetValue<float>("ATKFirst", 0f);
        }
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			valuesDictionary.SetValue<float>("ATKDuration", m_ATKDuration);
			valuesDictionary.SetValue<float>("ATKRate", m_AttackRate);
            valuesDictionary.SetValue<float>("ATKFirst", ATK_first);

        }
		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTerrain m_subsystemTerrain;

		public SubsystemTime m_subsystemTime;

		public SubsystemAudio m_subsystemAudio;

		public SubsystemParticles m_subsystemParticles;
		public ComponentPlayer m_componentPlayer;
        public ComponentCreature m_componentCreature;

        public float m_AttackRate;//保存攻击加成速率
		public float m_ATKDuration;//buff持续时间，需要在存档读取。
		public Random m_random = new Random();


	}

	public class ComponentSpeedUP : Component, IUpdateable
	{
		
		public float Speed_first;
		public float Speed_Now;
		public float SpeedRate
		{
			get
			{
				return m_SpeedRate;
			}
		}//恢复速率
		 //public bool flag_1 =false;//控制消息提示
		 //public bool flag_2 =false;

		public bool IsActive
		{
			get
			{
				return this.m_SpeedDuration > 0f;
			}
		}//标记buff是否已经激活

		public UpdateOrder UpdateOrder
		{
			get
			{
				return UpdateOrder.Default;
			}
		}


		public void StartSpeedBuff(float healtime, float healrate)
		{
			m_SpeedRate = healrate;
			m_SpeedDuration = healtime;
			/*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

		}

		public void StopBuff()
		{

			if (m_componentPlayer != null)
			{
				m_SpeedDuration = 0f;
				m_componentPlayer.ComponentGui.DisplaySmallMessage("速度强化结束！", Color.LightGreen, true, false);
				if(Speed_first<3)
				{
					Speed_first = 3.9f;
				}
				m_componentPlayer.ComponentLocomotion.WalkSpeed= Speed_first;
				Speed_first = 0f;
				Speed_Now = 0f;
			}

		}

		public void Update(float dt)
		{
			if (IsActive == true)
			{
				if (m_SpeedDuration > 0 && SpeedRate > 0)//如果持续时间大于0，说明处于生命恢复状态
				{
					if (Speed_first == 0)
					{
						Speed_first = m_componentPlayer.ComponentLocomotion.WalkSpeed;//记录初始速度保证回档
						m_componentPlayer.ComponentLocomotion.WalkSpeed = m_componentPlayer.ComponentLocomotion.WalkSpeed * (1f + SpeedRate / 100) + 1;
						Speed_Now = m_componentPlayer.ComponentLocomotion.WalkSpeed;//时刻记录速度
						


                    }
					else
					{
						if(m_componentPlayer.ComponentLocomotion.WalkSpeed>=4f)
						{
                            Speed_Now = m_componentPlayer.ComponentLocomotion.WalkSpeed;
                        }
                       
                    }
					m_componentPlayer.ComponentLocomotion.WalkSpeed = Speed_Now; //同步速度
                    m_SpeedDuration = m_SpeedDuration - dt;
					if (m_SpeedDuration <= 0)
					{
						StopBuff();
					}
				}
				/*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
			}





		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
			m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
			m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
			
			m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_SpeedDuration = valuesDictionary.GetValue<float>("SpeedDuration", 0f);
			m_SpeedRate = valuesDictionary.GetValue<float>("SpeedRate", 1f);
            m_componentPlayer = m_componentCreature.Entity.FindComponent<ComponentPlayer>();
            Speed_first = valuesDictionary.GetValue<float>("SpeedFirst", 0f);
			Speed_Now = valuesDictionary.GetValue<float>("SpeedNow", 3f);

        }
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			valuesDictionary.SetValue<float>("SpeedDuration", m_SpeedDuration);
			valuesDictionary.SetValue<float>("SpeedRate", m_SpeedRate);
            valuesDictionary.SetValue<float>("SpeedFirst", Speed_first);
            valuesDictionary.SetValue<float>("SpeedNow", Speed_Now);
        }
		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTerrain m_subsystemTerrain;
        public ComponentCreature m_componentCreature;
        public SubsystemTime m_subsystemTime;

		public SubsystemAudio m_subsystemAudio;

		public SubsystemParticles m_subsystemParticles;
		public ComponentPlayer m_componentPlayer;

		public float m_SpeedRate;//保存速率加成
		public float m_SpeedDuration;//buff持续时间，需要在存档读取。
		public Random m_random = new Random();


	}
    public class ComponentSlowDown : Component, IUpdateable
    {
        public BuffParticleSystem onBuffParticleSystem;
       
        public float Slown_first;
        public float SlowRate
        {
            get
            {
                return m_SlowRate;
            }
        }//恢复速率
         //public bool flag_1 =false;//控制消息提示
         //public bool flag_2 =false;

        public bool IsActive
        {
            get
            {
                return m_SlowDuration > 0f;
            }
        }//标记buff是否已经激活

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }


        public void StartBuff(float time, float rate)
        {
            m_SlowRate = rate;
            m_SlowDuration = time;
            /*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

        }

        public void StopBuff()
        {
			if(m_componentCreature!=null)
			{
                m_SlowDuration = 0f;
                if(Slown_first>3.7)
				{
					Slown_first = 3.9f;
				}
                m_componentCreature.ComponentLocomotion.WalkSpeed = Slown_first;
                Slown_first = 0f;
                if (m_componentPlayer != null)
                {

                    m_componentPlayer.ComponentGui.DisplaySmallMessage("迟缓效果结束！", Color.White, true, false);


                }
            }
           
           

        }

        public void Update(float dt)
        {
            if (IsActive == true)
            {
                if (m_SlowDuration > 0 && m_SlowRate > 0)//如果持续时间大于0，说明处于xx状态
                {
                    
                    if (Slown_first == 0)
                    {
						Slown_first=m_componentCreature.ComponentLocomotion.WalkSpeed;

                        
                    }
                    m_componentCreature.ComponentLocomotion.WalkSpeed = 1f;





                    if (onBuffParticleSystem == null)
                    {
                        onBuffParticleSystem = new BuffParticleSystem(10)
                        {
                            Texture = ContentManager.Get<Texture2D>("Textures/Star", null)
                        };
                        m_subsystemParticles.AddParticleSystem(onBuffParticleSystem);
                    }
                    BoundingBox boundingBox2 = m_componentCreature.ComponentBody.BoundingBox;
                    onBuffParticleSystem.Position = 0.5f * (boundingBox2.Min + boundingBox2.Max);
                    onBuffParticleSystem.Radius = 0.5f * MathUtils.Min(boundingBox2.Max.X - boundingBox2.Min.X, boundingBox2.Max.Z - boundingBox2.Min.Z);
                    //onBuffParticleSystem.Color = Color.Lerp(Color.LightRed, Color.DarkRed, MathUtils.Saturate(m_SlowDuration / 20f));
                    onBuffParticleSystem.Color = new Color(60f,60f,60f);
                    onBuffParticleSystem.Size = m_componentCreature.ComponentBody.BoxSize.XZ * 0.1f;

                    m_SlowDuration = m_SlowDuration - dt;
                    if (m_SlowDuration <= 0)
                    {
                        if (onBuffParticleSystem != null)
                        {
                            onBuffParticleSystem.IsStopped = true;
                            onBuffParticleSystem = null;


                        }
                        StopBuff();
                    }
                }

                /*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
            }






        }
        public override void OnEntityRemoved()
        {

            bool flag = onBuffParticleSystem != null;
            if (flag)
            {
                onBuffParticleSystem.IsStopped = true;
                onBuffParticleSystem = null;


            }

        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);

            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);
            m_componentPlayer = m_componentCreature.Entity.FindComponent<ComponentPlayer>();
            m_SlowDuration = valuesDictionary.GetValue<float>("SlowDuration", 0f);
            m_SlowRate = valuesDictionary.GetValue<float>("SlowRate", 1f);
            Slown_first = valuesDictionary.GetValue<float>("SlowFirst", 0f);
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("SlowDuration", m_SlowDuration);
            valuesDictionary.SetValue<float>("SlowRate", m_SlowRate);
            valuesDictionary.SetValue<float>("SlowFirst", Slown_first);

        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;
        public ComponentCreature m_componentCreature;

        public float m_SlowRate;//保存攻击加成速率
        public float m_SlowDuration;//buff持续时间，需要在存档读取。
        public Random m_random = new Random();


    }
    public class ComponentBlind : Component, IUpdateable
    {
        public float m_BlackoutDuration;//黑屏时间
        public float m_BlackoutFactor;//黑屏因素

        public float Blind_first;
        public float BlindRate
        {
            get
            {
                return m_BlindRate;
            }
        }//恢复速率
         //public bool flag_1 =false;//控制消息提示
         //public bool flag_2 =false;

        public bool IsActive
        {
            get
            {
                return this.m_BlindDuration > 0f;
            }
        }//标记buff是否已经激活

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }


        public void StartBlindBuff(float time, float rate)
        {
            m_BlindRate = rate;
            m_BlindDuration = time;
            /*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

        }

        public void StopBuff()
        {

            if (m_componentPlayer != null)
            {
                m_BlindDuration = 0f;
                m_componentPlayer.ComponentGui.DisplaySmallMessage("致盲结束！", Color.Black, false, false);

				m_BlindRate = 0f;

                Blind_first = 0f;
            }

        }

        public void Update(float dt)
        {
            
                if (m_BlindDuration > 0 || BlindRate > 0)//如果持续时间大于0，说明处于致盲状态
                {
					if(Blind_first==0)
					{
                        Blind_first = 1f;
                        
                    }
                    
                    if (m_BlindDuration > 0f)
                    {
                        m_BlindDuration = MathUtils.Max(m_BlindDuration - dt, 0f);
                        m_BlindRate = MathUtils.Min(m_BlindRate + 0.5f * dt, 0.95f);
                    }
                    else if (m_BlindRate > 0f)
                    {
                        m_BlindRate = MathUtils.Max(BlindRate - 0.5f * dt, 0f);
                    }
                    m_componentPlayer.ComponentScreenOverlays.BlackoutFactor = MathUtils.Max(m_BlindRate, m_componentPlayer.ComponentScreenOverlays.BlackoutFactor);


                   // m_BlindDuration = m_BlindDuration - dt;
                    if (m_BlindDuration <= 0&& m_BlindRate<=0)
                    {
                        StopBuff();
                    }
                }
                /*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
            





        }
		
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);
            Blind_first = valuesDictionary.GetValue<float>("BlindFirst", 0f);
            m_BlindDuration = valuesDictionary.GetValue<float>("BlindDuration", 0f);
            m_BlindRate = valuesDictionary.GetValue<float>("BlindRate", 0f);

        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("BlindFirst", Blind_first);
            valuesDictionary.SetValue<float>("BlindDuration", m_BlindDuration);
            valuesDictionary.SetValue<float>("BlindRate", m_BlindRate);
        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;

        public float m_BlindRate;//保存致盲强度
        public float m_BlindDuration;//buff持续时间，需要在存档读取。
        public Random m_random = new Random();


    }
    public class ComponentJump : Component, IUpdateable
    {
        public float m_BlackoutDuration;//黑屏时间
        public float m_BlackoutFactor;//黑屏因素

        public float Jump_first;
        public float JumpRate
        {
            get
            {
                return m_JumpRate;
            }
        }//恢复速率
         //public bool flag_1 =false;//控制消息提示
         //public bool flag_2 =false;

        public bool IsActive
        {
            get
            {
                return this.m_JumpDuration > 0f;
            }
        }//标记buff是否已经激活

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }


        public void StartJumpBuff(float time, float rate)
        {
            m_JumpRate = rate;
            m_JumpDuration = time;
            /*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

        }

        public void StopBuff()
        {

            if (m_componentPlayer != null)
            {
                m_JumpDuration = 0f;
                m_componentPlayer.ComponentGui.DisplaySmallMessage("跳跃强化结束！", Color.Black, false, false);
				m_componentPlayer.ComponentHealth.FallResilience = Jump_first;
				m_componentPlayer.ComponentLocomotion.JumpSpeed = Jump_firstR;

                m_JumpRate = 0f;

                Jump_first = 0f;
                Jump_firstR = 0f;
            }

        }

        public void Update(float dt)
        {

            if (m_JumpDuration > 0 && JumpRate > 0)//如果持续时间大于0，说明处于致盲状态
            {
                if (Jump_first == 0)
                {
					Jump_firstR = m_componentPlayer.ComponentLocomotion.JumpSpeed;
                    Jump_first = m_componentPlayer.ComponentHealth.FallResilience;//无法摔死
                    m_componentPlayer.ComponentHealth.FallResilience = 100000f;

                }
				
				m_componentPlayer.ComponentLocomotion.JumpSpeed = 15;

				m_JumpDuration-= dt;
                // m_JumpDuration = m_JumpDuration - dt;
                if (m_JumpDuration <= 0 )
                {
                    StopBuff();
                }
            }
            /*else if(m_HealRate<=0)
            {
                m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
                m_HealDuration = 0f;
            }*/






        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);
            Jump_first = valuesDictionary.GetValue<float>("JumpFirst", 0f);

            m_JumpDuration = valuesDictionary.GetValue<float>("JumpDuration", 0f);
            m_JumpRate = valuesDictionary.GetValue<float>("JumpRate", 0f);
            Jump_firstR = valuesDictionary.GetValue<float>("JumpR", 0f);
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("JumpFirst", Jump_first);
            valuesDictionary.SetValue<float>("JumpDuration", m_JumpDuration);
            valuesDictionary.SetValue<float>("JumpRate", m_JumpRate);
            valuesDictionary.SetValue<float>("JumpR", Jump_firstR);
        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;
		public float Jump_firstR;
        public float m_JumpRate;//保存致盲强度
        public float m_JumpDuration;//buff持续时间，需要在存档读取。
        public Random m_random = new Random();


    }
    public class ComponentDizzy : Component, IUpdateable
    {
        public float m_BlackoutDuration;//黑屏时间
        public float m_BlackoutFactor;//黑屏因素

       
        public float DizzyRate
        {
            get
            {
                return m_DizzyRate;
            }
        }//恢复速率
         //public bool flag_1 =false;//控制消息提示
         //public bool flag_2 =false;

        public bool IsActive
        {
            get
            {
                return this.m_DizzyDuration > 0f;
            }
        }//标记buff是否已经激活

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }


        public void StartDizzyBuff(float time, float rate)
        {
            m_DizzyRate = rate;
            m_DizzyDuration = time;
            /*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/

        }

        public void StopBuff()
        {

            if (m_componentPlayer != null)
            {
                m_DizzyDuration = 0f;
                m_componentPlayer.ComponentGui.DisplaySmallMessage("眩晕结束！", Color.White, false, false);
				if(Dizzy_firstR>3.7||Dizzy_firstR<3)
				{
					Dizzy_firstR = 3.9f;
				}
                if (Dizzy_firstJ>4.7f || Dizzy_firstJ < 3)
                {
                    Dizzy_firstR = 4.5f;
                }
                m_componentPlayer.ComponentLocomotion.WalkSpeed = Dizzy_firstR;
				m_componentPlayer.ComponentLocomotion.FlySpeed = Dizzy_first ;
				m_componentPlayer.ComponentLocomotion.JumpSpeed= Dizzy_firstJ ;
				m_componentPlayer.ComponentLocomotion.TurnSpeed= Dizzy_firstT ;
                m_componentPlayer.ComponentLocomotion.SwimSpeed= Dizzy_firstS ;


                m_DizzyRate = 0f;
                Dizzy_first = 0f;
                Dizzy_firstR = 0f;
                Dizzy_firstJ = 0f;
                Dizzy_firstT = 0f;
                Dizzy_firstS = 0f;
            }

        }

        public void Update(float dt)
        {

            if (m_DizzyDuration > 0 && DizzyRate > 0)//如果持续时间大于0，说明处于致盲状态
            {
                if (Dizzy_firstR == 0)
                {
					Dizzy_firstR = m_componentPlayer.ComponentLocomotion.WalkSpeed;
					Dizzy_first = m_componentPlayer.ComponentLocomotion.FlySpeed;
					Dizzy_firstJ = m_componentPlayer.ComponentLocomotion.JumpSpeed;
					Dizzy_firstT = m_componentPlayer.ComponentLocomotion.TurnSpeed;
					Dizzy_firstS = m_componentPlayer.ComponentLocomotion.SwimSpeed;


                }
                m_componentPlayer.ComponentLocomotion.WalkSpeed = 0f;
                m_componentPlayer.ComponentLocomotion.TurnSpeed = 0f;
                m_componentPlayer.ComponentLocomotion.FlySpeed = 0f;
                m_componentPlayer.ComponentLocomotion.SwimSpeed = 0f;
                m_componentPlayer.ComponentLocomotion.JumpSpeed = 0f;


                m_DizzyDuration -= dt;

                // m_DizzyDuration = m_DizzyDuration - dt;
                if (m_DizzyDuration <= 0 )
                {
                    StopBuff();
                }
            }
            /*else if(m_HealRate<=0)
            {
                m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
                m_HealDuration = 0f;
            }*/






        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);
            Dizzy_first = valuesDictionary.GetValue<float>("DizzyFirst", 0f);

            m_DizzyDuration = valuesDictionary.GetValue<float>("DizzyDuration", 0f);
            m_DizzyRate = valuesDictionary.GetValue<float>("DizzyRate", 0f);
            Dizzy_firstR = valuesDictionary.GetValue<float>("DizzyR", 0f);
            Dizzy_firstJ = valuesDictionary.GetValue<float>("DizzyJ", 0f);
            Dizzy_firstT = valuesDictionary.GetValue<float>("DizzyT", 0f);
            Dizzy_firstS = valuesDictionary.GetValue<float>("DizzyS", 0f);
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("DizzyFirst", Dizzy_first);
            valuesDictionary.SetValue<float>("DizzyDuration", m_DizzyDuration);
            valuesDictionary.SetValue<float>("DizzyRate", m_DizzyRate);
            valuesDictionary.SetValue<float>("DizzyR", Dizzy_firstR);//走路速度
            valuesDictionary.SetValue<float>("DizzyJ", Dizzy_firstJ);//储存原先的速度
            valuesDictionary.SetValue<float>("DizzyT", Dizzy_firstT);//储存原先的速度
            valuesDictionary.SetValue<float>("DizzyS", Dizzy_firstS);//储存原先的速度
        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;
        public float Dizzy_first;//飞行速度
        public float Dizzy_firstJ;//跳跃速度
        public float Dizzy_firstT;//转身速度
        public float Dizzy_firstR;//走路速度
        public float Dizzy_firstS;//游泳速度
        public float m_DizzyRate;
        public float m_DizzyDuration;//buff持续时间，需要在存档读取。
        public Random m_random = new Random();


    }
    public class ComponentNightsight : Component, IUpdateable
    {
       
        
        public bool IsActive
        {
            get
            {
                return this.m_NightseeDuration > 0f;
            }
        }//标记buff是否已经激活

        public UpdateOrder UpdateOrder
        {
            get
            {
                return UpdateOrder.Default;
            }
        }


        public void StartNightsightBuff(float time)
        {

            m_NightseeDuration = time;
			/*if(componentPlayer != null)
            {
				componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复效果已经激活！", Color.Red, true, false);
				
			}*/
			float a = 5;
            UpdateBrightness(a);
           
        }

        public void StopBuff()
        {

            if (m_componentPlayer != null)
            {
                m_NightseeDuration = 0f;
                m_componentPlayer.ComponentGui.DisplaySmallMessage("夜视结束！", Color.Purple, true, false);


               
                float a = SettingsManager.Brightness;
				UpdateBrightness(a);

            }

        }
        public void UpdateBrightness(float blockLight)
        {
            float x = MathUtils.Lerp(0f, 0.1f, blockLight);
            for (int i = 0; i < 16; i++)
            {
                LightingManager.LightIntensityByLightValue[i] = MathUtils.Saturate(MathUtils.Lerp(x, 1f, MathUtils.Pow(i / 15f, 1.25f)));
            }
            for (int j = 0; j < 6; j++)
            {
                float num = LightingManager.CalculateLighting(CellFace.FaceToVector3(j));
                for (int k = 0; k < 16; k++)
                {
                    LightingManager.LightIntensityByLightValueAndFace[k + j * 16] = LightingManager.LightIntensityByLightValue[k] * num;
                }
            }
            m_subsystemTerrain.TerrainUpdater.DowngradeAllChunksState(TerrainChunkState.InvalidPropagatedLight, true);

        }

        public void Update(float dt)
        {
            if (IsActive == true)
            {
                if (m_NightseeDuration > 0 )//如果持续时间大于0，说明处于生命恢复状态
                {
                    

                    m_NightseeDuration = m_NightseeDuration - dt;
                    
                }
                if (m_NightseeDuration <= 0)
                {
                    StopBuff();
                }
                /*else if(m_HealRate<=0)
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage("生命恢复速率数值不应该小于等于0！该操作将强行打断恢复！", Color.Red, true, false);
					m_HealDuration = 0f;
				}*/
            }





        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
            m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);

            m_NightseeDuration = valuesDictionary.GetValue<float>("NightDuration", 0f);
            

        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("NightDuration", m_NightseeDuration);
           
        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;
		public NewModLoaderShengcheng m_modloader = new NewModLoaderShengcheng();
        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;

        
        public float m_NightseeDuration;//夜视buff持续时间，需要在存档读取。
        public Random m_random = new Random();


    }
	#endregion
	#endregion
	#endregion
    #region 超巨型建筑生成
	public class BuildingInfo
    {
        /// <summary>
        /// 建筑名称
        /// </summary>
        public string Name;//这是用来存储建筑名称的字符串。
        /// <summary>
        /// 平原起始区块起始坐标
        /// </summary>
        public Point2 PlainAreaMinCoord;//这两个是用来定义生成建筑的平原范围的坐标。PlainAreaMinCoord定义了平原的最小坐标，PlainAreaMaxCoord定义了平原的最大坐标。
        /// <summary>
        /// 平原起始区块结束坐标
        /// </summary>
        public Point2 PlainAreaMaxCoord;
        /// <summary>
        /// 平原中心位置
        /// </summary>
        public Point2 AreaCenterPoint;//这个是用来存储平原中心位置的坐标。
        /// <summary>
        /// 平原半径
        /// </summary>
        public float Radius;//这个是用来存储平原半径的浮点数。
        /// <summary>
        /// 建筑生成起始坐标
        /// </summary>
        public Point3? OriginatePoint;//这个可空类型是用来存储建筑生成的起始坐标。如果这个值为null，那么说明还没有为这个建筑分配起始位置。
        /// <summary>
        /// 建筑生成拟定区块坐标
        /// </summary>
        public Point2 OriginateCoord;//这个是用来存储建筑生成拟定区块的坐标。
        /// <summary>
        /// 区块偏移数
        /// </summary>
        public int ChunkShiftCount;//这两个是用来存储区块偏移数和建筑高度偏移数的整数。它们是用来调整建筑的位置和高度的。

		public int Long;
		public int Wide;
        /// <summary>
        /// 建筑高度偏移数
        /// </summary>
        public int HightShiftCount;

        public BuildingInfo(string name, Point2 originateCoord, int hightShiftCount, Point3 originatePoint, int longnum ,int widenum)
        {
            //这个是BuildingInfo类的构造函数，它根据提供的参数初始化类的实例。
            Name = name;
			Long= longnum;
			Wide= widenum;
			OriginatePoint = originatePoint;
            OriginateCoord = originateCoord;
            HightShiftCount = hightShiftCount;
            ChunkShiftCount = 1;
			int num2 = (int)MathUtils.Sqrt(Long*Long+Wide*Wide);//计算建筑区域的平原直径
            PlainAreaMinCoord = OriginateCoord - new Point2(ChunkShiftCount);
            PlainAreaMaxCoord = OriginateCoord + new Point2(ChunkShiftCount) + new Point2((int)Long/16, (int)Long/16);
            int px = (PlainAreaMaxCoord.X * 16 + PlainAreaMinCoord.X * 16 + 15) / 2;//中心点的x坐标
            int pz = (PlainAreaMaxCoord.Y * 16 + PlainAreaMinCoord.Y * 16 + 15) / 2;//

            Radius = MathUtils.Min(px - PlainAreaMinCoord.X * 16, pz - PlainAreaMinCoord.Y * 16);
            AreaCenterPoint = new Point2(originatePoint.X+(int)Long/2, originatePoint.Z+(int)Wide/2);
        }

        /// <summary>
        /// 平原范围
        /// </summary>
        public bool CalculatPlainRange(Point2 coord)//这个函数用来判断给定的坐标是否在定义的平原范围内。如果在范围内，返回true；否则返回false。
        {
            if (coord.X >= PlainAreaMinCoord.X && coord.Y >= PlainAreaMinCoord.Y)
            {
                if (coord.X <= PlainAreaMaxCoord.X && coord.Y <= PlainAreaMaxCoord.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class AreaInfo//特殊区域生成专用
    {
        /// <summary>
        /// 建筑名称
        /// </summary>
        public string Name;//这是用来存储建筑名称的字符串。
        /// <summary>
        /// 平原起始区块起始坐标
        /// </summary>
        public Point2 PlainAreaMinCoord;//这两个是用来定义生成建筑的平原范围的坐标。PlainAreaMinCoord定义了平原的最小坐标，PlainAreaMaxCoord定义了平原的最大坐标。
        /// <summary>
        /// 平原起始区块结束坐标
        /// </summary>
        public Point2 PlainAreaMaxCoord;
        /// <summary>
        /// 平原中心位置
        /// </summary>
        public Point2 AreaCenterPoint;//这个是用来存储平原中心位置的坐标。
        /// <summary>
        /// 平原半径
        /// </summary>
        public float Radius;//这个是用来存储平原半径的浮点数。
        /// <summary>
        /// 建筑生成起始坐标
        /// </summary>
        public Point3? OriginatePoint;//这个可空类型是用来存储建筑生成的起始坐标。如果这个值为null，那么说明还没有为这个建筑分配起始位置。
        /// <summary>
        /// 建筑生成拟定区块坐标
        /// </summary>
        public Point2 OriginateCoord;//这个是用来存储建筑生成拟定区块的坐标。
        /// <summary>
        /// 区块偏移数
        /// </summary>
        public int ChunkShiftCount;//这两个是用来存储区块偏移数和建筑高度偏移数的整数。它们是用来调整建筑的位置和高度的。

        public int Long;
        public int Wide;
        /// <summary>
        /// 建筑高度偏移数
        /// </summary>
        public int HightShiftCount;

        public AreaInfo(string name, Point2 originateCoord, int hightShiftCount, Point3 originatePoint, int longnum, int widenum)
        {
            //这个是BuildingInfo类的构造函数，它根据提供的参数初始化类的实例。
            Name = name;
            Long = longnum;
            Wide = widenum;
            OriginatePoint = originatePoint;
            OriginateCoord = originateCoord;
            HightShiftCount = hightShiftCount;
            ChunkShiftCount = 1;
            int num2 = (int)MathUtils.Sqrt(Long * Long + Wide * Wide);//计算建筑区域的平原直径
            PlainAreaMinCoord = OriginateCoord - new Point2(ChunkShiftCount);
            PlainAreaMaxCoord = OriginateCoord + new Point2(ChunkShiftCount) + new Point2((int)Long / 16, (int)Long / 16);
            int px = (PlainAreaMaxCoord.X * 16 + PlainAreaMinCoord.X * 16 + 15) / 2;//中心点的x坐标
            int pz = (PlainAreaMaxCoord.Y * 16 + PlainAreaMinCoord.Y * 16 + 15) / 2;//

            Radius = MathUtils.Min(px - PlainAreaMinCoord.X * 16, pz - PlainAreaMinCoord.Y * 16);
            AreaCenterPoint = new Point2(originatePoint.X + (int)Long / 2, originatePoint.Z + (int)Wide / 2);
        }

        /// <summary>
        /// 平原范围
        /// </summary>
        public bool CalculatPlainRange(Point2 coord)//这个函数用来判断给定的坐标是否在定义的平原范围内。如果在范围内，返回true；否则返回false。
        {
            if (coord.X >= PlainAreaMinCoord.X && coord.Y >= PlainAreaMinCoord.Y)
            {
                if (coord.X <= PlainAreaMaxCoord.X && coord.Y <= PlainAreaMaxCoord.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class SubsystemNaturallyBuildings : Subsystem
    {
        public static Dictionary<string, Dictionary<Point2, Dictionary<Point3, int>>> Buildings = new Dictionary<string, Dictionary<Point2, Dictionary<Point3, int>>>();

        public SubsystemTerrain SubsystemTerrain;

        public List<BuildingInfo> BuildingInfos = new List<BuildingInfo>();//建筑信息列表
        public List<AreaInfo> AreaInfos = new List<AreaInfo>();//特殊区域列表
        public SubsystemWorldDemo m_subsystemWorldDemo;
        
        public override void Load(ValuesDictionary valuesDictionary)
        {
            SubsystemTerrain = Project.FindSubsystem<SubsystemTerrain>();
            m_subsystemWorldDemo = Project.FindSubsystem<SubsystemWorldDemo>();
            BuildingInfos.Add(new BuildingInfo("House/Supercity",(192,192) , 0, (3072,66,3072), 550, 550));
            BuildingInfos.Add(new BuildingInfo("House/血泪", (64, 64), 0, (1024, 66, 1024), 550, 550));
            //创建一个新的建筑信息实例，该实例表示“悦灵城”，并将其添加到BuildingInfos列表中。该建筑物的生成点是由GetRandomPoint方法获取的随机点，高度偏移数为9。
            //BuildingInfos.Add(new BuildingInfo("建筑物/天空城", GetRandomPoint(SubsystemTerrain.SubsystemGameInfo.WorldSeed, BuildingInfos), -70));
            //BuildingInfos.Add(new BuildingInfo("建筑物/小镇", GetRandomPoint(SubsystemTerrain.SubsystemGameInfo.WorldSeed, BuildingInfos), 2));
            try
            {
                string line = valuesDictionary.GetValue<string>("OriginatePoints");//字典中获取键为"OriginatePoints"的值，并将其存储在line字符串中。
                  //对line进行分割，使用分号’;’作为分隔符，得到多个字符串，每个字符串表示一个建筑的起始点信息，然后遍历这些字符串。
                foreach (string str in line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //对每一个字符串用’@’进行分割，取出第一部分作为建筑信息在BuildingInfos列表中的索引。
                    int i = int.Parse(str.Split(new char[] { '@' })[0]);
                    //string[] points = (str.Split(new char[] { '@' })[1]).Split(new char[] { ',' }); 取出字符串的第二部分，
					//表示建筑的起始点坐标，然后用逗号’,’进行分割，得到坐标的x、y、z三个部分。
                    string[] points = (str.Split(new char[] { '@' })[1]).Split(new char[] { ',' });
                    
					//将坐标的x、y、z部分转化为整数，然后创建一个新的Point3实例，将其设为对应的建筑信息的起始点。
                    BuildingInfos[i].OriginatePoint = new Point3(int.Parse(points[0]), int.Parse(points[1]), int.Parse(points[2]));
                }
            }
            catch (Exception e)
            {
                Log.Warning("SubsystemNaturallyBuildings-Load:" + e.Message);
            }
            try
            {
                string line = valuesDictionary.GetValue<string>("AreaOriginatePoints");//字典中获取键为"OriginatePoints"的值，并将其存储在line字符串中。
                                                                                   //对line进行分割，使用分号’;’作为分隔符，得到多个字符串，每个字符串表示一个建筑的起始点信息，然后遍历这些字符串。
                foreach (string str in line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //对每一个字符串用’@’进行分割，取出第一部分作为建筑信息在BuildingInfos列表中的索引。
                    int i = int.Parse(str.Split(new char[] { '@' })[0]);
                    //string[] points = (str.Split(new char[] { '@' })[1]).Split(new char[] { ',' }); 取出字符串的第二部分，
                    //表示建筑的起始点坐标，然后用逗号’,’进行分割，得到坐标的x、y、z三个部分。
                    string[] points = (str.Split(new char[] { '@' })[1]).Split(new char[] { ',' });

                    //将坐标的x、y、z部分转化为整数，然后创建一个新的Point3实例，将其设为对应的建筑信息的起始点。
                    AreaInfos[i].OriginatePoint = new Point3(int.Parse(points[0]), int.Parse(points[1]), int.Parse(points[2]));
                }
            }
            catch (Exception e)
            {
                Log.Warning("SubsystemNaturallyBuildings-Load:" + e.Message);
            }

        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
            int i = 0;//用于在遍历BuildingInfos列表时记录当前的索引。
            string line = "";//初始化一个空字符串line，用于存储所有建筑的起始点信息。
            string line1 = "";//初始化一个空字符串line，用于存储所有区域的起始点信息。
            foreach (BuildingInfo buildingInfo in BuildingInfos)//遍历BuildingInfos列表中的每一个建筑信息。
            {
                //判断当前的建筑信息的起始点是否已经被设置。如果已经被设置（HasValue为true），则执行括号中的代码。
                if (buildingInfo.OriginatePoint.HasValue)
                {
                    //将当前的索引i、’@’符号、当前的建筑信息的起始点转化为字符串、分号’;’连接起来，然后添加到line字符串的末尾。
                    line += i + "@" + buildingInfo.OriginatePoint.Value.ToString() + ";";
                }
                i++;
            }
            valuesDictionary.SetValue("OriginatePoints", line);
            foreach (AreaInfo areaInfo in AreaInfos)//遍历BuildingInfos列表中的每一个建筑信息。
            {
                //判断当前的建筑信息的起始点是否已经被设置。如果已经被设置（HasValue为true），则执行括号中的代码。
                if (areaInfo.OriginatePoint.HasValue)
                {
                    //将当前的索引i、’@’符号、当前的建筑信息的起始点转化为字符串、分号’;’连接起来，然后添加到line字符串的末尾。
                    line1 += i + "@" + areaInfo.OriginatePoint.Value.ToString() + ";";
                }
                i++;
            }
            
            valuesDictionary.SetValue("AreaOriginatePoints", line1);
            //这个方法的结果是，valuesDictionary字典中将含有一个键为"OriginatePoints"的项，其值是一个字符串，包含了所有已设置起始点的建筑信息的索引和起始点坐标。
        }

        /// <summary>
        /// 建筑文件在进入游戏时加载
        /// </summary>
        public static void Initialize()
        {
			
            LoadBuilding("House/Supercity");
            LoadBuilding("House/血泪");
            //LoadBuilding("建筑物/天空城");
            //LoadBuilding("建筑物/小镇");
        }

        /// <summary>
        /// 初始化家具包，在第一次进入存档时加载
        /// </summary>
        public static void InitFurnitureDesign(SubsystemFurnitureBlockBehavior furnitureBlockBehavior)
        {
            try
            {
                var overridesNode = ContentManager.Get<XElement>("建筑物/完美世界家具");
                var valuesDictionary = new ValuesDictionary();
                valuesDictionary.ApplyOverrides(overridesNode);
                List<FurnitureDesign> designs = SubsystemFurnitureBlockBehavior.LoadFurnitureDesigns(furnitureBlockBehavior.SubsystemTerrain, valuesDictionary);
                foreach (FurnitureDesign design in designs)
                {
                    furnitureBlockBehavior.m_furnitureDesigns[design.Index] = design;
                }
            }
            catch (Exception e)
            {
                Log.Warning("SubsystemNaturallyBuildings--InitFurnitureDesign:" + e.Message);
            }
        }
        /// <summary>
        /// 更改山脉系数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static float ChangeMountainRangeFactor(float x, float z, float factor, SubsystemNaturallyBuildings SNBuildings)
        {
            //遍历SubsystemNaturallyBuildings实例中的每一个BuildingInfo对象，即每一个建筑。
            foreach (BuildingInfo buildingInfo in SNBuildings.BuildingInfos)
            {
                //使用CalculatPlainRange方法判断当前坐标(x, z)是否在当前建筑的平原范围内。如果不在范围内，遍历下一个建筑。
                if (buildingInfo.CalculatPlainRange(new Point2((int)(x / 16), (int)(z / 16))))
                {
                    //如果当前坐标在平原范围内，计算当前坐标与平原中心点的距离，并除以平原半径，得到一个比例值k。这个比例值表示当前坐标距离平原中心的相对位置。
                    float dis = Vector2.Distance(new Vector2(x, z), new Vector2(buildingInfo.AreaCenterPoint));
                    float k = dis / buildingInfo.Radius;
                    //使用MathUtils.Clamp方法确保比例值k在0和1之间。这是因为如果k小于0，那么它将被设置为0；如果k大于1，那么它将被设置为1。
                    //将初始山脉因子乘以这个比例值k，得到新的山脉因子。
                    factor = factor * MathUtils.Clamp(k, 0, 1f);
                    return factor;
                }
                //如果遍历所有建筑后都没有找到在平原范围内的坐标，那么方法将直接返回初始的山脉因子。
            }
            return factor;
            //这个方法将山脉因子与当前坐标距离平原中心的距离关联起来，距离中心越近，
			//返回的山脉因子越小；距离中心越远，返回的山脉因子越接近初始值。
			//这样可以在平原中心附近生成平坦的地形，而在远离中心的地方生成山脉。
        }
        /// <summary>
        /// 生成建筑准备
        /// </summary>       
        public static void GenerateBuildingPrepare(TerrainChunk chunk, SubsystemNaturallyBuildings SNBuildings)
        {
			if(SNBuildings!=null)
			{
                foreach (BuildingInfo buildingInfo in SNBuildings.BuildingInfos)
                {
                    //它首先遍历所有的建筑信息（BuildingInfo）。
                    if (buildingInfo.CalculatPlainRange(chunk.Coords))
                    {
                        //对于每个建筑信息，它检查当前处理的地形块坐标是否在此建筑的平原范围内。
                        for (int i = 0; i < 16; i++)
                        //如果在平原范围内，它开始遍历这个地形块的每一个单元格。
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                //对于每个单元格，它计算单元格的x和y坐标与建筑平原中心的距离，然后检查这个距离是否小于平原的半径。
                                int dx = chunk.Origin.X + i - buildingInfo.AreaCenterPoint.X;
                                int dz = chunk.Origin.Y + j - buildingInfo.AreaCenterPoint.Y;
                                int r = (int)buildingInfo.Radius  ;
                                //如果距离小于半径，
                                if (dx * dx + dz * dz < r * r)
                                {
                                    //那么它检查这个建筑是否已经有一个起始点。
                                    if (!buildingInfo.OriginatePoint.HasValue)
                                    {
                                        //如果没有，那么它就遍历这个地形块的所有高度，找到第一个不为空的单元格，然后把它的高度设置为这个建筑的起始点高度。
                                        for (int t = 254; t > 0; t--)
                                        {
                                            int id = chunk.GetCellContentsFast(3, t, 3);
                                            if (id != 0)
                                            {
                                                buildingInfo.OriginatePoint = new Point3(buildingInfo.OriginateCoord.X * 16, t, buildingInfo.OriginateCoord.Y * 16);
                                                break;
                                            }
                                        }
                                    }
                                    //它接着找到当前处理的单元格的高度，然后比较这个高度和建筑的起始点高度。
                                    int h = buildingInfo.OriginatePoint.Value.Y;
                                    int id2 = 0;
                                    for (int k = 254; k > 0; k--)
                                    {

                                        id2 = chunk.GetCellContentsFast(i, k, j);
                                        if (id2 != 0)
                                        {
                                            h = k;
                                            break;
                                        }
                                    }
                                    if (h > buildingInfo.OriginatePoint.Value.Y)
                                    {
                                        // //如果单元格的高度大于起始点的高度，那么它就把这个单元格以及它上面的所有单元格都设为空。
                                        for (int k = h; k > buildingInfo.OriginatePoint.Value.Y; k--)
                                        {
                                            chunk.SetCellValueFast(i, k, j, 0);
                                        }
                                    }
                                    else if (h < buildingInfo.OriginatePoint.Value.Y)
                                    {
										if(id2==3|| id2 == 4 || id2 == 2 || id2 == 8 || id2 == 6 || id2 == 7 )
										{
                                            //如果单元格的高度小于起始点的高度，那么它就把这个单元格以及它下面到起始点高度的所有单元格都设为当前单元格的类型。
                                            for (int k = h; k <= buildingInfo.OriginatePoint.Value.Y; k++)
                                            {
                                                chunk.SetCellValueFast(i, k, j, id2);
                                            }
                                        }
										else
										{
                                            for (int k = h; k <= buildingInfo.OriginatePoint.Value.Y; k++)
                                            {
                                                chunk.SetCellValueFast(i, k, j, 8);
                                            }
                                        }
                                        
                                        
                                    }
                                }
                            }
                            //这个过程会在所有在平原范围内的地形块上进行，为接下来在这些地形块上生成建筑做好准备。

                            //总的来说，这个方法的作用是为建筑的生成做地形处理，确保建筑的底部是平坦的，并且它的高度是正确的。
                        }
                    }
                }
            }
           
        }
        /// <summary>
        /// 生成建筑
        /// </summary>       
        public static void GenerateBuilding(TerrainChunk chunk, SubsystemNaturallyBuildings FCBuildings)
        {
			if(FCBuildings!=null)
			{
                foreach (BuildingInfo buildingInfo in FCBuildings.BuildingInfos)
                {
                    //对于每个建筑信息，它检查建筑是否有一个已经设定的起始点，这是通过判断BuildingInfo中的OriginatePoint属性是否有值来完成的。
                    if (buildingInfo.OriginatePoint.HasValue)
                    {
                        //如果起始点存在，它首先计算新的起始点。新的起始点是原始起始点的高度减去建筑信息中的高度偏移数（HightShiftCount）。
                        Point3 point = buildingInfo.OriginatePoint.Value - new Point3(0, buildingInfo.HightShiftCount, 0);
                        //最后，它通过调用SubsystemNaturallyBuildings.GenerateBuildings方法来在计算出的新起始点上生成建筑。这个方法需要三个参数：建筑信息，新的起始点和地形块。
                        //总的来说，这个方法对所有的建筑进行遍历，在每个建筑的起始点生成建筑。如果某个建筑的起始点没有被设定，那么这个建筑就不会被生成。
                        SubsystemNaturallyBuildings.GenerateBuildings(buildingInfo, point, chunk);
                    }
                }
            }
            
        }
        /// <summary>
        /// 生成特殊区域
        /// </summary>       
        public static void GenerateArea(TerrainChunk chunk, SubsystemNaturallyBuildings FCBuildings)
        {
            if (FCBuildings != null)
            {
                foreach (AreaInfo areaInfo in FCBuildings.AreaInfos)
                {
                    //对于每个建筑信息，它检查建筑是否有一个已经设定的起始点，这是通过判断BuildingInfo中的OriginatePoint属性是否有值来完成的。
                    if (areaInfo.OriginatePoint.HasValue)
                    {
                        //如果起始点存在，它首先计算新的起始点。新的起始点是原始起始点的高度减去建筑信息中的高度偏移数（HightShiftCount）。
                        Point3 point = areaInfo.OriginatePoint.Value - new Point3(0, areaInfo.HightShiftCount, 0);
                        //最后，它通过调用SubsystemNaturallyBuildings.GenerateBuildings方法来在计算出的新起始点上生成建筑。这个方法需要三个参数：建筑信息，新的起始点和地形块。
                        //总的来说，这个方法对所有的建筑进行遍历，在每个建筑的起始点生成建筑。如果某个建筑的起始点没有被设定，那么这个建筑就不会被生成。
                        SubsystemNaturallyBuildings.GenerateAreas(areaInfo, point, chunk);
                    }
                }
            }

        }

        /// <summary>
        /// 生成建筑
        /// </summary>
        /// <param name="name"></param>
        /// <param name="originatePoint"></param>
        /// <param name="chunk"></param>
        public static void GenerateAreas(AreaInfo areaInfo, Point3 originatePoint, TerrainChunk chunk)
        {
            int dchunkX = chunk.Coords.X - (int)(originatePoint.X / 16);
            int dchunkY = chunk.Coords.Y - (int)(originatePoint.Z / 16);
            //计算地形块的坐标和建筑起始点的距离，分别在x轴和z轴上。
            
            
                //判断建筑信息中是否有对应于当前地形块的建筑单元格信息，如果有，则获取这些单元格信息。
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        //对于地形块内的每一个单元格，如果该单元格的高度大于建筑起始点的高度，那么就将这个单元格设为空。这是为了清除可能存在的原始地形。
                        for (int k = originatePoint.Y + areaInfo.HightShiftCount + 1; k < 254; k++)
                        {
                            chunk.SetCellValueFast(i, k, j, 0);
                        }
                    }
                }
                //接着遍历所有的建筑单元格，对于每一个单元格，首先获取它的值。
                
            
        }
        /// <summary>
        /// 获取随机生成点
        /// </summary>
		/// 
        public  Point2 GetRandomPoint(int seed, List<BuildingInfo> buildingInfos)
        {
            
            int id = buildingInfos.Count;
            Point2 point = new Point2(-1000, -1000);
            Random random = new Random(seed + id * 3);
            if (id == 0)//第一个建筑
            {
                point = new Point2(random.Int(0, 50), random.Int(0, 50));
            }
            else
            {
                bool pass;
                int c = 0;
                int cx = 0;
                int cz = 0;
                //do while语句格式： do{循环体}while（条件语句）；
                //不管满不满足条件先执行一次，
                //然后在看条件语句，如果满足那就继续执行，如果不满足则不再执行循环体，注意语法格式，
                //while后面必须要加上分号哦，而while语句就不需要分号，要注意一些细节
                do
                {
                    pass = true;
                    c++;
                    cx = random.Int(-50, 50);
                    cz = random.Int(-50, 50);
                    foreach (BuildingInfo buildingInfo in buildingInfos)
                    {
                        if (MathUtils.Abs(buildingInfo.OriginateCoord.X - cx) < 50 && MathUtils.Abs(buildingInfo.OriginateCoord.Y - cz) < 50)
                        {
                            pass = false;//重新执行
                            break;
                        }
                    }
                }
                while (!pass && c < 10000);
                point = new Point2(cx, cz);
            }
            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            if (worldType == WorldType.Default)//如果是主世界（地球）
            {
                string name = id == 0 ? "悦灵城" : id == 1 ? "天空城" : id == 2 ? "小镇" : "";
                Log.Warning($"{name}坐标：[x:{point.X * 16},y:?,z:{point.Y * 16}]");
                //System.Diagnostics.Debug.WriteLine($"{name}:建筑坐标：[x:{point.X * 16},y:生成高度随机,z:{point.Y * 16}]");
            }
            return point;
        }

        /// <summary>
        /// 生成建筑
        /// </summary>
        /// <param name="name"></param>
        /// <param name="originatePoint"></param>
        /// <param name="chunk"></param>
        public static void GenerateBuildings(BuildingInfo buildingInfo, Point3 originatePoint, TerrainChunk chunk)
        {
            int dchunkX = chunk.Coords.X - (int)(originatePoint.X / 16);
            int dchunkY = chunk.Coords.Y - (int)(originatePoint.Z / 16);
            //计算地形块的坐标和建筑起始点的距离，分别在x轴和z轴上。
            if (Buildings[buildingInfo.Name].TryGetValue(new Point2(dchunkX, dchunkY), out Dictionary<Point3, int> cells))
            {
                //判断建筑信息中是否有对应于当前地形块的建筑单元格信息，如果有，则获取这些单元格信息。
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        //对于地形块内的每一个单元格，如果该单元格的高度大于建筑起始点的高度，那么就将这个单元格设为空。这是为了清除可能存在的原始地形。
                        for (int k = originatePoint.Y + buildingInfo.HightShiftCount + 1; k < 254; k++)
                        {
                            chunk.SetCellValueFast(i, k, j, 0);
                        }
                    }
                }
                //接着遍历所有的建筑单元格，对于每一个单元格，首先获取它的值。
                foreach (Point3 point in cells.Keys)
                {
                    int value = cells[point];
                    //如果这个单元格的高度小于或等于建筑信息中的高度偏移量，并且这个单元格的类型是玻璃（ID为15），那么将这个单元格设为空。这可能是为了清除建筑的底部的玻璃。
                    if (point.Y <= buildingInfo.HightShiftCount && Terrain.ExtractContents(value) == 15)//15为玻璃
                    {
                        value = 0;
                    }
                    //最后，根据建筑单元格的值，设置地形块中对应位置的单元格。
                    chunk.SetCellValueFast(point.X, point.Y + originatePoint.Y, point.Z, value);
                }
            }
        }

        /// <summary>
        /// 装载建筑数据
        /// </summary>
        /// <param name="name"></param>
        public static void LoadBuilding(string name)
        {
            //定义一个l变量用于记录处理的行数。
            int l = 0;
            try
            {
                Log.Information("加载了1");

                //从内容管理器中获取名字为name的字符串，这个字符串是以文本形式存储的建筑数据。然后将这个字符串中的回车换行符"\r"替换为空字符串。
                string buildingText = ContentManager.Get<string>(name).Replace("\r", string.Empty);
                //将处理后的字符串按照换行符"\n"分割为多行。
                string[] lines = buildingText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
				buildingText = string.Empty;
                Log.Information("加载了2");
                foreach (string line in lines)
                {
                    
                    //对于每一行，首先增加l变量的值。然后将这一行按照逗号","分割为多个参数。
                    l++;
                    
                    string[] parameters = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //如果参数的数量大于3，那么将这些参数转化为单元格的坐标和值。
                    if (parameters.Length > 3)
                    {
                        
                        int x = int.Parse(parameters[0]);
                        int y = int.Parse(parameters[1]);
                        int z = int.Parse(parameters[2]);
                        int value = int.Parse(parameters[3]);
                        //计算这个单元格所在的地形块的坐标，以及它在地形块中的位置。
                        Point2 coords = new Point2((int)(x / 16), (int)(z / 16));
                        Point3 point = new Point3(x % 16, y, z % 16);
                        //检查Buildings字典中是否包含名字为name的建筑数据，如果不包含，那么就添加一个新的字典。
                        if (!Buildings.ContainsKey(name))
                        {
                            Buildings[name] = new Dictionary<Point2, Dictionary<Point3, int>>();
                        }
                        //检查这个建筑的字典中是否包含当前地形块的坐标，如果不包含，那么就添加一个新的字典。
                        if (!Buildings[name].ContainsKey(coords))
                        {
                            Buildings[name][coords] = new Dictionary<Point3, int>();
                        }

                        Buildings[name][coords][point] = value;
                    }
                }
                
                Log.Information("加载了2");
            }
            catch (Exception e)
            {
                //如果出现任何异常，那么就从Buildings字典中移除这个建筑的数据，然后将错误信息输出到日志中。
                if (Buildings.ContainsKey(name)) Buildings.Remove(name);
                Log.Warning($"LoadBuilding-{name}-Line-{l}:{e.Message}");
                //总的来说，这个方法的作用是加载一个建筑的数据，然后将这些数据存储到Buildings字典中，以便之后生成建筑时使用。
            }
        }
    }
    #endregion
   

    #region 区块（植物）
    #region 草皮方块子系统（已经废弃）
    /*public class FCSubsystemGrassBlockBehavior : SubsystemPollableBlockBehavior, IUpdateable
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					8,
					984,
					985,
					986,
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

		public override void OnPoll(int value, int x, int y, int z, int pollPass)
		{
			if (Terrain.ExtractData(value) != 0 || this.m_subsystemGameInfo.WorldSettings.EnvironmentBehaviorMode != EnvironmentBehaviorMode.Living)
			{
				return;
			}
			int num = Terrain.ExtractLight(base.SubsystemTerrain.Terrain.GetCellValue(x, y + 1, z));
			if (num == 0)
			{
				this.m_toUpdate[new Point3(x, y, z)] = Terrain.ReplaceContents(value, 8);
			}
			if (num < 13)
			{
				return;
			}
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = z - 1; j <= z + 1; j++)
				{
					for (int k = y - 2; k <= y + 1; k++)
					{
						int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(i, k, j);
						if (Terrain.ExtractContents(cellValue) == 2)
						{
							int cellValue2 = base.SubsystemTerrain.Terrain.GetCellValue(i, k + 1, j);
							if (!this.KillsGrassIfOnTopOfIt(cellValue2) && Terrain.ExtractLight(cellValue2) >= 13 && this.m_random.Float(0f, 1f) < 0.1f)
							{
								int num2 = Terrain.ReplaceContents(cellValue, 8);
								this.m_toUpdate[new Point3(i, k, j)] = num2;
								if (Terrain.ExtractContents(cellValue2) == 0)
								{
									int temperature = base.SubsystemTerrain.Terrain.GetTemperature(i, j);
									int humidity = base.SubsystemTerrain.Terrain.GetHumidity(i, j);
									int num3 = PlantsManager.GenerateRandomPlantValue(this.m_random, num2, temperature, humidity, k + 1);
									if (num3 != 0)
									{
										this.m_toUpdate[new Point3(i, k + 1, j)] = num3;
									}
								}
							}
						}
					}
				}
			}
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(x, y + 1, z);
			if (Terrain.ExtractContents(cellValue) == 61)
			{
				int value = base.SubsystemTerrain.Terrain.GetCellValueFast(x, y, z);
				value = Terrain.ReplaceData(value, 1);
				base.SubsystemTerrain.ChangeCell(x, y, z, value, true);
			}
			else
			{
				int value2 = base.SubsystemTerrain.Terrain.GetCellValueFast(x, y, z);
				value2 = Terrain.ReplaceData(value2, 0);
				base.SubsystemTerrain.ChangeCell(x, y, z, value2, true);
			}
			if (this.KillsGrassIfOnTopOfIt(cellValue))
			{
				base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(2, 0, 0), true);
			}
		}

		public override void OnExplosion(int value, int x, int y, int z, float damage)
		{
			if (damage > BlocksManager.Blocks[8].ExplosionResilience * this.m_random.Float(0f, 1f))
			{
				base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(2, 0, 0), true);
			}
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			this.m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			base.Load(valuesDictionary);
		}

		public void Update(float dt)
		{
			if (this.m_subsystemTime.PeriodicGameTimeEvent(60.0, 0.0))
			{
				foreach (KeyValuePair<Point3, int> keyValuePair in this.m_toUpdate)
				{
					if (Terrain.ExtractContents(keyValuePair.Value) == 8)
					{
						if (base.SubsystemTerrain.Terrain.GetCellContents(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z) != 2)
						{
							continue;
						}
					}
					else
					{
						int cellContents = base.SubsystemTerrain.Terrain.GetCellContents(keyValuePair.Key.X, keyValuePair.Key.Y - 1, keyValuePair.Key.Z);
						if ((cellContents != 8 && cellContents != 2) || base.SubsystemTerrain.Terrain.GetCellContents(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z) != 0)
						{
							continue;
						}
					}
					base.SubsystemTerrain.ChangeCell(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z, keyValuePair.Value, true);
				}
				this.m_toUpdate.Clear();
			}
		}

		public bool KillsGrassIfOnTopOfIt(int value)
		{
			int num = Terrain.ExtractContents(value);
			Block block = BlocksManager.Blocks[num];
			return block is FluidBlock || (!block.IsFaceTransparent(base.SubsystemTerrain, 5, value) && block.IsCollidable_(value));
		}

		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTime m_subsystemTime;

		public Dictionary<Point3, int> m_toUpdate = new Dictionary<Point3, int>();

		public Random m_random = new Random();
	}*/
	#endregion

	#region 摔落方块子系统未使用
	/*public class FCSubsystemCollapsingBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks
		{
			get
			{
				return FCSubsystemCollapsingBlockBehavior.m_handledBlocks;
			}
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			if (this.m_subsystemGameInfo.WorldSettings.EnvironmentBehaviorMode == EnvironmentBehaviorMode.Living)
			{
				this.TryCollapseColumn(new Point3(x, y, z));
			}
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			this.m_subsystemSoundMaterials = base.Project.FindSubsystem<SubsystemSoundMaterials>(true);
			this.m_subsystemMovingBlocks = base.Project.FindSubsystem<SubsystemMovingBlocks>(true);
			this.m_subsystemMovingBlocks.Stopped += this.MovingBlocksStopped;
			this.m_subsystemMovingBlocks.CollidedWithTerrain += this.MovingBlocksCollidedWithTerrain;
		}

		public void MovingBlocksCollidedWithTerrain(IMovingBlockSet movingBlockSet, Point3 p)
		{
			if (movingBlockSet.Id == "CollapsingBlock")
			{
				int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(p.X, p.Y, p.Z);
				if (this.IsCollapseSupportBlock(cellValue))
				{
					movingBlockSet.Stop();
					return;
				}
				if (FCSubsystemCollapsingBlockBehavior.IsCollapseDestructibleBlock(cellValue))
				{
					base.SubsystemTerrain.DestroyCell(0, p.X, p.Y, p.Z, 0, false, false);
				}
			}
		}

		public void MovingBlocksStopped(IMovingBlockSet movingBlockSet)
		{
			if (movingBlockSet.Id == "CollapsingBlock")
			{
				Point3 p = Terrain.ToCell(MathUtils.Round(movingBlockSet.Position.X), MathUtils.Round(movingBlockSet.Position.Y), MathUtils.Round(movingBlockSet.Position.Z));
				foreach (MovingBlock movingBlock in movingBlockSet.Blocks)
				{
					Point3 point = p + movingBlock.Offset;
					base.SubsystemTerrain.DestroyCell(0, point.X, point.Y, point.Z, movingBlock.Value, false, false);
				}
				this.m_subsystemMovingBlocks.RemoveMovingBlockSet(movingBlockSet);
				if (movingBlockSet.Blocks.Count > 0)
				{
					this.m_subsystemSoundMaterials.PlayImpactSound(movingBlockSet.Blocks[0].Value, movingBlockSet.Position, 1f);
				}
			}
		}

		public void TryCollapseColumn(Point3 p)
		{
			if (p.Y <= 0)
			{
				return;
			}
			int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(p.X, p.Y - 1, p.Z);
			if (this.IsCollapseSupportBlock(cellValue))
			{
				return;
			}
			List<MovingBlock> list = new List<MovingBlock>();
			for (int i = p.Y; i < 256; i++)
			{
				int cellValue2 = base.SubsystemTerrain.Terrain.GetCellValue(p.X, i, p.Z);
				if (!FCSubsystemCollapsingBlockBehavior.IsCollapsibleBlock(cellValue2))
				{
					break;
				}
				list.Add(new MovingBlock
				{
					Value = cellValue2,
					Offset = new Point3(0, i - p.Y, 0)
				});
			}
			if (list.Count != 0 && this.m_subsystemMovingBlocks.AddMovingBlockSet(new Vector3(p), new Vector3((float)p.X, (float)(-(float)list.Count - 1), (float)p.Z), 0f, 10f, 0.7f, new Vector2(0f), list, "CollapsingBlock", null, true) != null)
			{
				foreach (MovingBlock movingBlock in list)
				{
					Point3 point = p + movingBlock.Offset;
					base.SubsystemTerrain.ChangeCell(point.X, point.Y, point.Z, 0, true);
				}
			}
		}

		public static bool IsCollapsibleBlock(int value)
		{
			return Enumerable.Contains<int>(FCSubsystemCollapsingBlockBehavior.m_handledBlocks, Terrain.ExtractContents(value));
		}

		public bool IsCollapseSupportBlock(int value)
		{
			int num = Terrain.ExtractContents(value);
			if (num == 0)
			{
				return false;
			}
			int data = Terrain.ExtractData(value);
			Block block = BlocksManager.Blocks[num];
			if (block is TrapdoorBlock)
			{
				return TrapdoorBlock.GetUpsideDown(data) && !TrapdoorBlock.GetOpen(data);
			}
			return block.BlockIndex == 238 || !block.IsFaceTransparent(base.SubsystemTerrain, 4, value) || block is SoilBlock;
		}

		public static bool IsCollapseDestructibleBlock(int value)
		{
			int num = Terrain.ExtractContents(value);
			Block block = BlocksManager.Blocks[num];
			if (block is TrapdoorBlock)
			{
				int data = Terrain.ExtractData(value);
				if (TrapdoorBlock.GetUpsideDown(data) && TrapdoorBlock.GetOpen(data))
				{
					return false;
				}
			}
			else if (block is FluidBlock)
			{
				return false;
			}
			return true;
		}

		public const string IdString = "CollapsingBlock";

		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemSoundMaterials m_subsystemSoundMaterials;

		public SubsystemMovingBlocks m_subsystemMovingBlocks;

		public static int[] m_handledBlocks = new int[]
		{
			980
		};
	}*/
	#endregion

	#region 西瓜父类
	//西瓜父类 id938
	public abstract class BaseXiguaBlock : Block
	{


		public BaseXiguaBlock(bool isRotten)
		{
			this.m_isRotten = isRotten;
		}
		public Texture2D m_texture;
		public override void Initialize()
		{


			Model model = ContentManager.Get<Model>("Models/Pumpkins1", null);
			m_texture = ContentManager.Get<Texture2D>("Textures/amod/xigua", null);//西瓜外置材质是通过csv里的默认材质和整张外置贴图设置的。
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Pumpkin", true).ParentBone);
			for (int i = 0; i < 8; i++)
			{
				float num = MathUtils.Lerp(0.2f, 1f, (float)i / 7f);
				float num2 = MathUtils.Min(0.3f * num, 0.7f * (1f - num));
				Color color;
				if (this.m_isRotten)
				{
					color = Color.White;
				}
				else
				{
					color = Color.Lerp(new Color(0, 128, 128), new Color(80, 255, 255), (float)i / 7f);
					if (i == 7)
					{
						color.R = byte.MaxValue;
					}
				}
				this.m_blockMeshesBySize[i] = new BlockMesh();
				if (i >= 1)
				{
					this.m_blockMeshesBySize[i].AppendModelMeshPart(model.FindMesh("Pumpkin", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateScale(num) * Matrix.CreateTranslation(0.5f + num2, 0f, 0.5f + num2), false, false, false, false, color);
				}
				if (this.m_isRotten)
				{
					this.m_blockMeshesBySize[i].TransformTextureCoordinates(Matrix.CreateTranslation(-0.375f, 0.25f, 0f), -1);
				}
				this.m_standaloneBlockMeshesBySize[i] = new BlockMesh();
				this.m_standaloneBlockMeshesBySize[i].AppendModelMeshPart(model.FindMesh("Pumpkin", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateScale(num) * Matrix.CreateTranslation(0f, -0.23f, 0f), false, false, false, false, color);
				if (this.m_isRotten)
				{
					this.m_standaloneBlockMeshesBySize[i].TransformTextureCoordinates(Matrix.CreateTranslation(-0.375f, 0.25f, 0f), -1);
				}
			}
			for (int j = 0; j < 8; j++)
			{
				BoundingBox boundingBox = (this.m_blockMeshesBySize[j].Vertices.Count > 0) ? this.m_blockMeshesBySize[j].CalculateBoundingBox() : new BoundingBox(new Vector3(0.5f, 0f, 0.5f), new Vector3(0.5f, 0f, 0.5f));
				float num3 = boundingBox.Max.X - boundingBox.Min.X;
				if (num3 < 0.8f)
				{
					float num4 = (0.8f - num3) / 2f;
					boundingBox.Min.X = boundingBox.Min.X - num4;
					boundingBox.Min.Z = boundingBox.Min.Z - num4;
					boundingBox.Max.X = boundingBox.Max.X + num4;
					boundingBox.Max.Y = 0.4f;
					boundingBox.Max.Z = boundingBox.Max.Z + num4;
				}
				this.m_collisionBoxesBySize[j] = new BoundingBox[]
				{
					boundingBox
				};
			}
			base.Initialize();

		}



		public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
		{
			int size = BaseXiguaBlock.GetSize(Terrain.ExtractData(value));
			return this.m_collisionBoxesBySize[size];
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			int data = Terrain.ExtractData(value);
			int size = BaseXiguaBlock.GetSize(data);
			bool isDead = BaseXiguaBlock.GetIsDead(data);
			bool flag = size >= 1;
			if (flag)
			{
				generator.GenerateMeshVertices(this, x, y, z, this.m_blockMeshesBySize[size], Color.White, null, geometry.GetGeometry(m_texture).SubsetAlphaTest);
			}
			bool flag2 = size == 0;
			if (flag2)
			{
				generator.GenerateCrossfaceVertices(this, value, x, y, z, new Color(160, 160, 160), 11, geometry.SubsetAlphaTest);
			}
			else
			{
				bool flag3 = size < 7 && !isDead;
				if (flag3)
				{
					generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, 28, geometry.GetGeometry(m_texture).SubsetAlphaTest);
				}
			}
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			int size2 = BaseXiguaBlock.GetSize(Terrain.ExtractData(value));
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMeshesBySize[size2], m_texture, color, 2f * size, ref matrix, environmentData);
		}

		public override int GetShadowStrength(int value)
		{
			return BaseXiguaBlock.GetSize(Terrain.ExtractData(value));
		}

		public override float GetNutritionalValue(int value)
		{
			if (BaseXiguaBlock.GetSize(Terrain.ExtractData(value)) != 7)
			{
				return 0f;
			}
			return base.GetNutritionalValue(value);
		}

		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{
			int size = BaseXiguaBlock.GetSize(Terrain.ExtractData(oldValue));
			if (size >= 1)
			{
				int value = this.SetDamage(Terrain.MakeBlockValue(this.BlockIndex, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, true), size)), this.GetDamage(oldValue));
				dropValues.Add(new BlockDropValue
				{
					Value = value,
					Count = 1
				});
			}
			showDebris = true;
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{         //方块碎片效果
			int size = BaseXiguaBlock.GetSize(Terrain.ExtractData(value));
			float num = MathUtils.Lerp(0.2f, 1f, (float)size / 7f);
			Color color = (size == 7) ? Color.White : new Color(0, 128, 128);
			return new BlockDebrisParticleSystem(subsystemTerrain, position, 1.5f * strength, this.DestructionDebrisScale * num, color, GetFaceTextureSlot(1, value), m_texture);
		}
		
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			int size = BaseXiguaBlock.GetSize(Terrain.ExtractData(value));
			if (this.m_isRotten)
			{
				if (size >= 7)
				{
					return "腐烂的西瓜";
				}
				return "腐烂未成熟的西瓜";
			}
			else
			{
				if (size >= 7)
				{
					return "西瓜";
				}
				return "未成熟的西瓜";
			}
		}

		public override IEnumerable<int> GetCreativeValues()
		{
			yield return Terrain.MakeBlockValue(this.BlockIndex, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, true), 1));
			yield return Terrain.MakeBlockValue(this.BlockIndex, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, true), 3));
			yield return Terrain.MakeBlockValue(this.BlockIndex, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, true), 5));
			yield return Terrain.MakeBlockValue(this.BlockIndex, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, true), 7));
			yield break;
		}

		public static int GetSize(int data)
		{
			return 7 - (data & 7);
		}

		public static int SetSize(int data, int size)
		{
			return (data & -8) | 7 - (size & 7);
		}

		public static bool GetIsDead(int data)
		{
			return (data & 8) != 0;
		}

		public static int SetIsDead(int data, bool isDead)
		{
			if (!isDead)
			{
				return data & -9;
			}
			return data | 8;
		}

		public override int GetDamage(int value)
		{
			return (Terrain.ExtractData(value) & 16) >> 4;
		}

		public override int SetDamage(int value, int damage)
		{
			int num = Terrain.ExtractData(value);
			return Terrain.ReplaceData(value, (num & -17) | (damage & 1) << 4);
		}

		public override int GetDamageDestructionValue(int value)
		{
			if (this.m_isRotten)
			{
				return 0;
			}
			int data = Terrain.ExtractData(value);
			return this.SetDamage(Terrain.MakeBlockValue(936, 0, data), 0);
		}

		public override int GetRotPeriod(int value)
		{
			if (!BaseXiguaBlock.GetIsDead(Terrain.ExtractData(value)))
			{
				return 0;
			}
			return this.DefaultRotPeriod;
		}

		public BlockMesh[] m_blockMeshesBySize = new BlockMesh[8];

		public BlockMesh[] m_standaloneBlockMeshesBySize = new BlockMesh[8];

		public BoundingBox[][] m_collisionBoxesBySize = new BoundingBox[8][];

		public bool m_isRotten;
	}
	#endregion
	#region  种子区域
	public class FCSeedBlock : FlatBlock
	{


		public override IEnumerable<int> GetCreativeValues()
		{
			List<int> list = new List<int>();
			foreach (int data in EnumUtils.GetEnumValues(typeof(FCSeedBlock.SeedType)))
			{
				list.Add(Terrain.MakeBlockValue(933, 0, data));
			}
			return list;
		}

		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			switch (Terrain.ExtractData(value))
			{
				case 0:
					return "西瓜种子";
				case 1:
					return "向日葵种子";

				default:
					return string.Empty;
			}

		}

		public override int GetFaceTextureSlot(int face, int value)
		{
			int num = Terrain.ExtractData(value);
			bool flag = num == 5 || num == 4;
			int result;
			if (flag)
			{
				result = 74;
			}
			else
			{
				result = 75;
			}
			return result;
		}

		public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
		{
			BlockPlacementData result = default(BlockPlacementData);
			result.CellFace = raycastResult.CellFace;
			bool flag = raycastResult.CellFace.Face == 4;
			if (flag)
			{
				switch (Terrain.ExtractData(value))
				{
					case 0:
						result.Value = Terrain.MakeBlockValue(938, 0, BaseXiguaBlock.SetSize(BaseXiguaBlock.SetIsDead(0, false), 0));
						break;
					case 1:
						result.Value = Terrain.MakeBlockValue(937, 0, FlowerBlock.SetIsSmall(0, true));
						break;

				}
			}
			return result;
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			switch (Terrain.ExtractData(value))
			{
				case 0:
					color *= new Color(2, 21, 1);//黑色西瓜种子
					break;
				case 1:
					color *= new Color(102, 51, 0);//棕色的向日葵种子
					break;

			}
			BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, null, color, false, environmentData);


		}

		public const int Index = 933;

		public enum SeedType
		{
			Xigua,
			SunFlower
		}
	}

	#endregion

	#region 植物生长行为区块
	//植物生长

	public class FoodcraftPlantBlockBehavior : SubsystemPollableBlockBehavior, IUpdateable
	{

		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					938,//西瓜
					936,//腐烂西瓜
					937,//sunflower
				};
			}
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			int num = Terrain.ExtractContents(base.SubsystemTerrain.Terrain.GetCellValue(x, y, z));
			int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 1, z);
			int num2 = Terrain.ExtractContents(cellValue);
			if (num != 131)//南瓜
			{
				if (num != 132)//南瓜灯
				{
					if (num != 244)//烂南瓜
					{

						if (num2 != 8 && num2 != 2 && num2 != 7 && num2 != 168 )//草地，泥土，沙子，土壤
						{
							base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
							return;
						}
						return;
					}
				}
				else
				{
					Block block = BlocksManager.Blocks[num2];
					if (block.IsFaceTransparent(base.SubsystemTerrain, 4, cellValue) && !(block is FenceBlock))
					{
						base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
						return;
					}
					return;
				}
			}
			if (num2 != 8 && num2 != 2 && num2 != 168 )
			{
				base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
				return;
			}
		}

		public UpdateOrder UpdateOrder
		{
			get
			{
				return UpdateOrder.Default;
			}
		}

		public void Update(float dt)
		{
			if (this.m_subsystemTime.PeriodicGameTimeEvent(30.0, 0.0))
			{
				foreach (KeyValuePair<Point3, FoodcraftPlantBlockBehavior.Replacement> keyValuePair in this.m_toReplace)
				{
					Point3 key = keyValuePair.Key;
					if (Terrain.ReplaceLight(base.SubsystemTerrain.Terrain.GetCellValue(key.X, key.Y, key.Z), 0) == Terrain.ReplaceLight(keyValuePair.Value.RequiredValue, 0))
					{
						base.SubsystemTerrain.ChangeCell(key.X, key.Y, key.Z, keyValuePair.Value.Value, true);
					}
				}
				this.m_toReplace.Clear();
			}
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
		}

		public override void OnPoll(int value, int x, int y, int z, int pollPass)
		{
			if (this.m_subsystemGameInfo.WorldSettings.EnvironmentBehaviorMode != EnvironmentBehaviorMode.Living || y <= 0 || y >= 255)
			{
				return;
			}
			int num = Terrain.ExtractContents(value);
			Block block = BlocksManager.Blocks[num];
			
			if (block is FlowerBlock)
			{
				this.GrowFlower(value, x, y, z, pollPass);
				return;
			}
			
			if (num == 938)
			{
				this.GrowXigua(value, x, y, z, pollPass);
				return;
			}

			if (num == 937)
			{
				this.GrowFlower(value, x, y, z, pollPass);
				return;
			}


		}

		

		public void GrowFlower(int value, int x, int y, int z, int pollPass)
		{
			int data = Terrain.ExtractData(value);
			if (FlowerBlock.GetIsSmall(data) && Terrain.ExtractLight(base.SubsystemTerrain.Terrain.GetCellValueFast(x, y + 1, z)) >= 9)
			{
				int data2 = FlowerBlock.SetIsSmall(data, false);
				int value2 = Terrain.ReplaceData(value, data2);
				this.m_toReplace[new Point3(x, y, z)] = new FoodcraftPlantBlockBehavior.Replacement
				{
					Value = value2,
					RequiredValue = value
				};
			}
		}

		

		

		
		public void GrowXigua(int value, int x, int y, int z, int pollPass)
		{
			bool flag = Terrain.ExtractLight(base.SubsystemTerrain.Terrain.GetCellValueFast(x, y + 1, z)) < 9;
			if (!flag)
			{
				int data = Terrain.ExtractData(value);
				int size = BaseXiguaBlock.GetSize(data);
				bool flag2 = BaseXiguaBlock.GetIsDead(data) || size >= 7;
				if (!flag2)
				{
					int cellValueFast = base.SubsystemTerrain.Terrain.GetCellValueFast(x, y - 1, z);
					int num = Terrain.ExtractContents(cellValueFast);
					int data2 = Terrain.ExtractData(cellValueFast);
					bool flag3 = num == 168 && SoilBlock.GetHydration(data2);
					int num2 = (num == 168) ? SoilBlock.GetNitrogen(data2) : 0;
					int num3 = 4;
					float num4 = 0.15f;
					bool flag4 = num == 168;
					if (flag4)
					{
						num3--;
						num4 -= 0.05f;
					}
					bool flag5 = num2 > 0;
					if (flag5)
					{
						num3--;
						num4 -= 0.05f;
					}
					bool flag6 = flag3;
					if (flag6)
					{
						num3--;
						num4 -= 0.05f;
					}
					bool flag7 = pollPass % MathUtils.Max(num3, 1) == 0;
					if (flag7)
					{
						int data3 = BaseXiguaBlock.SetSize(data, MathUtils.Min(size + 1, 7));
						bool flag8 = this.m_random.Float(0f, 1f) < num4;
						if (flag8)
						{
							data3 = BaseXiguaBlock.SetIsDead(data3, true);
						}
						int value2 = Terrain.ReplaceData(value, data3);
						FoodcraftPlantBlockBehavior.Replacement replacement = this.m_toReplace[new Point3(x, y, z)] = new FoodcraftPlantBlockBehavior.Replacement
						{
							Value = value2,
							RequiredValue = value
						};
						bool flag9 = num == 168 && size + 1 == 7;
						if (flag9)
						{
							int data4 = SoilBlock.SetNitrogen(data2, MathUtils.Max(num2 - 3, 0));
							int value3 = Terrain.ReplaceData(cellValueFast, data4);
							Dictionary<Point3, FoodcraftPlantBlockBehavior.Replacement> toReplace = this.m_toReplace;
							Point3 key = new Point3(x, y - 1, z);
							FoodcraftPlantBlockBehavior.Replacement value4 = new FoodcraftPlantBlockBehavior.Replacement
							{
								Value = value3,
								RequiredValue = cellValueFast
							};
							toReplace[key] = value4;
						}
					}
				}
			}
		}

		public Random m_random = new Random();
		public SubsystemTime m_subsystemTime;
		public Dictionary<Point3, FoodcraftPlantBlockBehavior.Replacement> m_toReplace = new Dictionary<Point3, FoodcraftPlantBlockBehavior.Replacement>();
		public SubsystemGameInfo m_subsystemGameInfo;
		public struct Replacement
		{
			public int RequiredValue;

			public int Value;
		}
	}
    
    #endregion

    #region 树木种类
    public enum FCTreeType
	{
		Coco,
		Orange,
		Apple,
		Yinghua,
		Lorejun,

	}
	#endregion

	#region 新植物管理器
	public static class FCPlantManager
	{
		static FCPlantManager()
		{
			Random random = new Random(33);
			FCPlantManager.m_treeBrushesByType[0] = new List<TerrainBrush>(); //coco  橡木 位置0
			for (int i = 0; i < 16; i++)
			{
				int[] array = new int[]
				{
					5,
					6,
					7,
					7,
					8,
					8,
					9,
					9,
					9,
					10,
					10,
					11,
					12,
					13,
					14,
					16
				};
				int height4 = array[i];
				int branchesCount = (int)MathUtils.Lerp(10f, 20f, (float)i / 16f);
				TerrainBrush item = FCPlantManager.CreateTreeBrush(random, FCPlantManager.GetTreeTrunkValue(FCTreeType.Coco), FCPlantManager.GetTreeLeavesValue(FCTreeType.Coco), height4, branchesCount, delegate (int y)
				{
					float num = 0.4f;
					if ((float)y < 0.2f * (float)height4)
					{
						num = 0f;
					}
					else if ((float)y >= 0.2f * (float)height4 && y <= height4)
					{
						num *= 1.5f;
					}
					return num;
				}, delegate (int y)
				{
					if ((float)y < (float)height4 * 0.3f || (float)y > (float)height4 * 0.9f)
					{
						return 0f;
					}
					float num = ((float)y < (float)height4 * 0.7f) ? (0.5f * (float)height4) : (0.35f * (float)height4);
					return random.Float(0.33f, 1f) * num;
				});
				FCPlantManager.m_treeBrushesByType[0].Add(item);
			}
			FCPlantManager.m_treeBrushesByType[1] = new List<TerrainBrush>();   //橘子树 桦木 位置2
			for (int j = 0; j < 16; j++)
			{
				int[] array2 = new int[]
				{
					4,
					5,
					6,
					6,
					7,
					7,
					7,
					8,
					8,
					8,
					9,
					9,
					9,
					10,
					10,
					11
				};
				int height3 = array2[j];
				int branchesCount2 = (int)MathUtils.Lerp(0f, 20f, (float)j / 16f);
				TerrainBrush item2 = FCPlantManager.CreateTreeBrush(random, FCPlantManager.GetTreeTrunkValue(FCTreeType.Orange), FCPlantManager.GetTreeLeavesValue(FCTreeType.Orange), height3, branchesCount2, delegate (int y)
				{
					float num = 0.66f;
					if (y < height3 / 2 - 1)
					{
						num = 0f;
					}
					else if (y > height3 / 2 && y <= height3)
					{
						num *= 1.5f;
					}
					return num;
				}, delegate (int y)
				{
					if ((float)y >= (float)height3 * 0.35f && (float)y <= (float)height3 * 0.75f)
					{
						return random.Float(0f, 0.33f * (float)height3);
					}
					return 0f;
				});
				FCPlantManager.m_treeBrushesByType[1].Add(item2);
			}
			FCPlantManager.m_treeBrushesByType[2] = new List<TerrainBrush>();//苹果，云杉木 位置3
			for (int k = 0; k < 16; k++)
			{
				int[] array3 = new int[]
				{
					7,
					8,
					9,
					10,
					10,
					11,
					11,
					12,
					12,
					13,
					13,
					14,
					14,
					15,
					16,
					17
				};
				int height2 = array3[k];
				int branchesCount3 = height2 * 3;
				TerrainBrush item3 = FCPlantManager.CreateTreeBrush(random, FCPlantManager.GetTreeTrunkValue(FCTreeType.Apple), FCPlantManager.GetTreeLeavesValue(FCTreeType.Apple), height2, branchesCount3, delegate (int y)
				{
					float num = MathUtils.Lerp(1.4f, 0.3f, (float)y / (float)height2);
					if (y < 3)
					{
						num = 0f;
					}
					if (y % 2 == 0)
					{
						num *= 0.3f;
					}
					return num;
				}, delegate (int y)
				{
					if (y < 3 || (float)y > (float)height2 * 0.8f)
					{
						return 0f;
					}
					if (y % 2 != 0)
					{
						return MathUtils.Lerp(0.3f * (float)height2, 0f, MathUtils.Saturate((float)y / (float)height2));
					}
					return 0f;
				});
				FCPlantManager.m_treeBrushesByType[2].Add(item3);
			}

			FCPlantManager.m_treeBrushesByType[3] = new List<TerrainBrush>();//樱花，云杉 位置4
			for (int m = 0; m < 16; m++)
			{
				FCPlantManager.m_treeBrushesByType[3].Add(FCPlantManager.CreateCherryBlossomTreeBrush(random, MathUtils.Lerp(6f, 9f, (float)m / 15f)));
			}

			FCPlantManager.m_treeBrushesByType[4] = new List<TerrainBrush>();// 老君树 高云杉 位置5
			for (int l = 0; l < 16; l++)
			{
				int[] array5 = new int[]
				{
					30,
					31,
					22,
					33,
					34,
					34,
					35,
					35,
					36,
					36,
					47,
					47,
					58,
					58,
					59,
					59,
					60,
					60
				};
				int height = array5[l];
				int branchesCount4 = height * 3;
				float startHeight = (0.3f + (float)(l % 4) * 0.05f) * (float)height;
				TerrainBrush item5 = FCPlantManager.CreateLorejunTreeBrush(random, FCPlantManager.GetTreeTrunkValue(FCTreeType.Lorejun), FCPlantManager.GetTreeLeavesValue(FCTreeType.Lorejun), height, branchesCount4, delegate (int y)
				{
					float num = MathUtils.Saturate((float)y / (float)height);
					float num2 = MathUtils.Lerp(1.5f, 0f, MathUtils.Saturate((num - 0.6f) / 0.4f));
					if ((float)y < startHeight)
					{
						num2 = 0f;
					}
					if (y % 3 != 0 && y < height - 4)
					{
						num2 *= 0.2f;
					}
					return num2;
				}, delegate (int y)
				{
					float num = MathUtils.Saturate((float)y / (float)height);
					if (y % 3 != 0)
					{
						return 0f;
					}
					if ((float)y >= startHeight)
					{
						return MathUtils.Lerp(0.18f * (float)height, 0f, MathUtils.Saturate((num - 0.6f) / 0.4f));
					}
					if ((float)y < startHeight - 4f)
					{
						return 0f;
					}
					return 0.1f * (float)height;
				});
				FCPlantManager.m_treeBrushesByType[4].Add(item5);
			}

		}

		public static int GetTreeTrunkValue(FCTreeType treeType)
		{
			return FCPlantManager.m_treeTrunksByType[(int)treeType];
		}

		public static int GetTreeLeavesValue(FCTreeType treeType)
		{
			return FCPlantManager.m_treeLeavesByType[(int)treeType];
		}

		public static ReadOnlyList<TerrainBrush> GetTreeBrushes(FCTreeType treeType)
		{
			return new ReadOnlyList<TerrainBrush>(FCPlantManager.m_treeBrushesByType[(int)treeType]);
		}

		public static int GenerateRandomPlantValue(Random random, int groundValue, int temperature, int humidity, int y)
		{
			int num = Terrain.ExtractContents(groundValue);
			if (num != 2)//2泥土
			{
				if (num != 7)//7沙子
				{
					if (num != 8) //8草地
					{
						return 0;
					}
				}
				else
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
			if (humidity >= 6)
			{
				if (random.Float(0f, 1f) < (float)humidity / 60f)
				{
					int result = Terrain.MakeBlockValue(19, 0, TallGrassBlock.SetIsSmall(0, false));//19高草
					if (!SubsystemWeather.IsPlaceFrozen(temperature, y))
					{
						float num2 = random.Float(0f, 1f);
						if (num2 < 0.04f)
						{
							result = Terrain.MakeBlockValue(20); //红花
						}
						else if (num2 < 0.07f)
						{
							result = Terrain.MakeBlockValue(24); //紫花
						}
						else if (num2 < 0.075f)
						{
							result = Terrain.MakeBlockValue(937); //向日葵
						}
						else if (num2 < 0.09f)
						{
							result = Terrain.MakeBlockValue(25); //白花
						}
						else if (num2 < 0.17f)
						{
							result = Terrain.MakeBlockValue(174, 0, RyeBlock.SetIsWild(RyeBlock.SetSize(0, 7), true));//174 rye 黑麦
						}
						else if (num2 < 0.19f)
						{
							result = Terrain.MakeBlockValue(204, 0, CottonBlock.SetIsWild(CottonBlock.SetSize(0, 2), true));//204 棉花
						}
					}
					return result;
				}
			}
			else if (random.Float(0f, 1f) < 0.025f)
			{
				if (random.Float(0f, 1f) < 0.2f)
				{
					return Terrain.MakeBlockValue(99, 0, 0); //大干灌木
				}
				return Terrain.MakeBlockValue(28, 0, 0);//干枯木
			}
			return 0;
		}

		public static FCTreeType? GenerateRandomTreeType(Random random, int temperature, int humidity, int y, float densityMultiplier = 1f)
		{
			FCTreeType? result = null;
			float num = random.Float() * FCPlantManager.CalculateTreeProbability(FCTreeType.Coco, temperature, humidity, y);
			float num2 = random.Float() * FCPlantManager.CalculateTreeProbability(FCTreeType.Orange, temperature, humidity, y);
			float num3 = random.Float() * FCPlantManager.CalculateTreeProbability(FCTreeType.Apple, temperature, humidity, y);
			float num4 = random.Float() * FCPlantManager.CalculateTreeProbability(FCTreeType.Yinghua, temperature, humidity, y);
			float num5 = random.Float() * FCPlantManager.CalculateTreeProbability(FCTreeType.Lorejun, temperature, humidity, y);
			float num6 = MathUtils.Max(MathUtils.Max(num, num2, num3, num4), num5);
			if (num6 > 0f)
			{
				if (num6 == num)
				{
					result = new FCTreeType?(FCTreeType.Coco);
				}
				if (num6 == num2)
				{
					result = new FCTreeType?(FCTreeType.Orange);
				}
				if (num6 == num3)
				{
					result = new FCTreeType?(FCTreeType.Apple);
				}
				if (num6 == num4)
				{
					result = new FCTreeType?(FCTreeType.Yinghua);
				}
				if (num6 == num5)
				{
					result = new FCTreeType?(FCTreeType.Lorejun);
				}
			}
			if (result != null && random.Bool(densityMultiplier * FCPlantManager.CalculateTreeDensity(result.Value, temperature, humidity, y)))
			{
				return result;
			}
			return null;
		}

		public static float CalculateTreeDensity(FCTreeType treeType, int temperature, int humidity, int y)
		{
			switch (treeType)
			{
				case FCTreeType.Coco:
					return FCPlantManager.RangeProbability((float)humidity, 4f, 15f, 15f, 15f);
				case FCTreeType.Orange:
					return FCPlantManager.RangeProbability((float)humidity, 4f, 15f, 15f, 15f);
				case FCTreeType.Apple:
					return FCPlantManager.RangeProbability((float)humidity, 4f, 15f, 15f, 15f);
				case FCTreeType.Yinghua:
					return FCPlantManager.RangeProbability((float)humidity, 4f, 15f, 15f, 15f);
				case FCTreeType.Lorejun:
					return 0.03f;
				default:
					return 0f;
			}
		}

		public static float CalculateTreeProbability(FCTreeType treeType, int temperature, int humidity, int y)
		{
			switch (treeType)
			{
				case FCTreeType.Coco:
					return FCPlantManager.RangeProbability((float)temperature, 4f, 10f, 15f, 15f) * FCPlantManager.RangeProbability((float)humidity, 6f, 8f, 15f, 15f);
				case FCTreeType.Orange:
					return FCPlantManager.RangeProbability((float)temperature, 4f, 10f, 15f, 15f) * FCPlantManager.RangeProbability((float)humidity, 6f, 8f, 15f, 15f);
				case FCTreeType.Apple:
					return FCPlantManager.RangeProbability((float)temperature, 4f, 10f, 15f, 15f) * FCPlantManager.RangeProbability((float)humidity, 6f, 8f, 15f, 15f);
				case FCTreeType.Lorejun:
					return FCPlantManager.RangeProbability((float)temperature, 4f, 10f, 15f, 15f) * FCPlantManager.RangeProbability((float)humidity, 6f, 8f, 15f, 15f);
				case FCTreeType.Yinghua:
					return FCPlantManager.RangeProbability((float)temperature, 4f, 10f, 15f, 15f) * FCPlantManager.RangeProbability((float)humidity, 6f, 8f, 15f, 15f);
				default:
					return 0f;
			}
		}

		public static float RangeProbability(float v, float a, float b, float c, float d)
		{
			if (v < a)
			{
				return 0f;
			}
			if (v < b)
			{
				return (v - a) / (b - a);
			}
			if (v <= c)
			{
				return 1f;
			}
			if (v <= d)
			{
				return 1f - (v - c) / (d - c);
			}
			return 0f;
		}

		public static TerrainBrush CreateTreeBrush(Random random, int woodIndex, int leavesIndex, int height, int branchesCount, Func<int, float> leavesProbabilityByHeight, Func<int, float> branchesLengthByHeight)
		{
			TerrainBrush terrainBrush = new TerrainBrush();
			terrainBrush.AddRay(0, -1, 0, 0, height, 0, 1, 1, 1, woodIndex);
			for (int i = 0; i < branchesCount; i++)
			{
				int x = 0;
				int num = random.Int(0, height);
				int z = 0;
				float s = branchesLengthByHeight(num);
				Vector3 vector = Vector3.Normalize(new Vector3(random.Float(-1f, 1f), random.Float(0f, 0.33f), random.Float(-1f, 1f))) * s;
				int x2 = (int)MathUtils.Round(vector.X);
				int y = num + (int)MathUtils.Round(vector.Y);
				int z2 = (int)MathUtils.Round(vector.Z);
				int cutFace = 0;
				if (MathUtils.Abs(vector.X) == MathUtils.Max(MathUtils.Abs(vector.X), MathUtils.Abs(vector.Y), MathUtils.Abs(vector.Z)))
				{
					cutFace = 1;
				}
				else if (MathUtils.Abs(vector.Y) == MathUtils.Max(MathUtils.Abs(vector.X), MathUtils.Abs(vector.Y), MathUtils.Abs(vector.Z)))
				{
					cutFace = 4;
				}
				terrainBrush.AddRay(x, num, z, x2, y, z2, 1, 1, 1, (Func<int?, int?>)((int? v) => v.HasValue ? null : new int?(Terrain.MakeBlockValue(woodIndex, 0, WoodBlock.SetCutFace(0, cutFace)))));
			}
			for (int j = 0; j < 3; j++)
			{
				terrainBrush.CalculateBounds(out Point3 min, out Point3 max);
				for (int k = min.X - 1; k <= max.X + 1; k++)
				{
					for (int l = min.Z - 1; l <= max.Z + 1; l++)
					{
						for (int m = 1; m <= max.Y + 1; m++)
						{
							float num2 = leavesProbabilityByHeight(m);
							if (random.Float(0f, 1f) < num2 && !terrainBrush.GetValue(k, m, l).HasValue && (terrainBrush.CountNonDiagonalNeighbors(k, m, l, leavesIndex) != 0 || terrainBrush.CountNonDiagonalNeighbors(k, m, l, (Func<int?, int>)((int? v) => (v.HasValue && Terrain.ExtractContents(v.Value) == woodIndex) ? 1 : 0)) != 0))
							{
								terrainBrush.AddCell(k, m, l, 0);
							}
						}
					}
				}
				terrainBrush.Replace(0, leavesIndex);
			}
			terrainBrush.AddCell(0, height, 0, leavesIndex);
			terrainBrush.Compile();
			return terrainBrush;
		}

		#region 樱花树刷子
		public static TerrainBrush CreateCherryBlossomTreeBrush(Random random, float size)
		{
			int height = 8;//树干高度
			TerrainBrush terrainBrush = new TerrainBrush(); //创建一个新的地形刷子对象。
			int value = FCPlantManager.m_treeTrunksByType[3]; //从樱花树木类型数组中获取指定索引的树干方块类型。
			int value2 = FCPlantManager.m_treeLeavesByType[3]; //从樱花树叶类型数组中获取指定索引的树叶方块类型。
			//向地形刷子添加一条射线，用于生成樱花树的树干。射线的起点是(0, -1, 0)，终点是(0, 8, 0)，宽度为2，高度为2，深度为2，方块类型为树干方块类型。
			terrainBrush.AddRay(0, -1, 0, 0, height, 0, 1, 1, 1, value);
			List<Point3> list = new List<Point3>(); //创建一个列表，用于存储树枝生成的位置信息。
			float num = random.Float(0f, 6.2831855f); //生成一个随机的浮点数作为角度值，用于计算树枝的方向。
			for (int i = 0; i < 6; i++) //迭代3次，生成3个树枝
			{
				float radians = num + (float)i * MathUtils.DegToRad(120f); //计算当前树枝的角度，根据角度的增量120°来确定每个树枝之间的角度差。
				Vector3 v = Vector3.Transform(Vector3.Normalize(new Vector3(1f, random.Float(1f, 1.5f), 0f)), Matrix.CreateRotationY(radians)); //根据角度和随机生成的长度因子，计算树枝的方向向量。
				int num2 = random.Int((int)(0.7 * size), (int)(1.2 * size));  //树枝的长度由(int)(0.7f * size)和(int)size之间的随机整数决定。
				Point3 point = new Point3(0, height-1 , 0);   //树枝生成的起始位置
				Point3 point2 = new Point3(Vector3.Round(new Vector3(point) + v * (float)num2)); //根据起始位置、方向向量和长度计算树枝的结束位置。
				terrainBrush.AddRay(point.X, point.Y, point.Z, point2.X, point2.Y, point2.Z, 1, 1, 1, value); //向地形刷子添加一条射线，用于生成树枝。射线的起点是起始位置，终点是结束位置，宽度为1，高度为1，深度为1，方块类型为树干方块类型。
				list.Add(point2); //将树枝的结束位置添加到列表中，用于后续生成树枝周围的树叶。
			}
			foreach (Point3 point3 in list) //遍历存储树枝位置的列表。
			{
				float num3 = random.Float(0.45f * size, 0.6f * size); // 随机生成树叶的大小，大小范围为0.45倍到0.6倍的size值。
				int num4 = (int)MathUtils.Ceiling(num3); // 向上取整树叶的大小，得到树叶的半径。
				int trunkHeight = height; // 树枝的高度
				int maxHeight = point3.Y + trunkHeight + num4; // 树叶生成的最大高度限制为树枝高度加树叶半径

				for (int j = point3.X - num4; j <= point3.X + num4; j++) // 遍历树叶周围的X坐标范围。
				{
					for (int k = point3.Y; k <= maxHeight; k++) // 遍历树叶的高度范围，从树枝顶部到最大高度。
					{
						for (int l = point3.Z - num4; l <= point3.Z + num4; l++) // 遍历树叶周围的Z坐标范围。
						{
							int num5 = MathUtils.Abs(j - point3.X) + MathUtils.Abs(l - point3.Z); // 计算当前位置与树叶起始位置的Manhattan距离。
																								  // 计算当前位置与树叶起始位置的欧几里得距离，使用缩放因子(1, 1.7, 1)来调整距离。
							float num6 = ((new Vector3((float)j, (float)k, (float)l) - new Vector3(point3)) * new Vector3(1f, 1.7f, 1f)).Length();
							// 判断当前位置是否满足生成树叶的条件：距离树叶起始位置距离小于等于树叶大小、距离树叶起始位置的Manhattan距离小于等于树叶半径。
							if (num6 <= num3 && num5 <= num4 && terrainBrush.GetValue(j, k, l) == null)
							{
								terrainBrush.AddCell(j, k, l, value2); // 生成树叶
							}
						}
					}
				}
			}
			terrainBrush.Compile();
			return terrainBrush;
		}
		#endregion

		#region 老君树刷子
		public static TerrainBrush CreateLorejunTreeBrush(Random random, int woodIndex, int leavesIndex, int height, int branchesCount, Func<int, float> leavesProbabilityByHeight, Func<int, float> branchesLengthByHeight)
		{
			int m1 = random.Int(1, 17);
			float size  = MathUtils.Lerp(6f, 9f, (float)m1 / 15f);

			TerrainBrush terrainBrush = new TerrainBrush();
			terrainBrush.AddRay(0, -1, 0, 0, height, 0, 3, 3, 3, woodIndex);
			for (int i = 0; i < branchesCount; i++)
			{
				int x = 0;
				int num = random.Int(0, height);
				int z = 0;
				float s = branchesLengthByHeight(num);
				Vector3 vector = Vector3.Normalize(new Vector3(random.Float(-1f, 1f), random.Float(0f, 0.33f), random.Float(-1f, 1f))) * s;
				int x2 = (int)MathUtils.Round(vector.X);
				int y = num + (int)MathUtils.Round(vector.Y);
				int z2 = (int)MathUtils.Round(vector.Z);
				int cutFace = 0;
				if (MathUtils.Abs(vector.X) == MathUtils.Max(MathUtils.Abs(vector.X), MathUtils.Abs(vector.Y), MathUtils.Abs(vector.Z)))
				{
					cutFace = 1;
				}
				else if (MathUtils.Abs(vector.Y) == MathUtils.Max(MathUtils.Abs(vector.X), MathUtils.Abs(vector.Y), MathUtils.Abs(vector.Z)))
				{
					cutFace = 4;
				}
				terrainBrush.AddRay(x, num, z, x2, y, z2, 1, 1, 1, (Func<int?, int?>)((int? v) => v.HasValue ? null : new int?(Terrain.MakeBlockValue(woodIndex, 0, WoodBlock.SetCutFace(0, cutFace)))));
			}
			for (int j = 0; j < 3; j++)
			{
				terrainBrush.CalculateBounds(out Point3 min, out Point3 max);
				for (int k = min.X - 1; k <= max.X + 1; k++)
				{
					for (int l = min.Z - 1; l <= max.Z + 1; l++)
					{
						for (int m = 1; m <= max.Y + 1; m++)
						{
							float num2 = leavesProbabilityByHeight(m);
							if (random.Float(0f, 1f) < num2 && !terrainBrush.GetValue(k, m, l).HasValue && (terrainBrush.CountNonDiagonalNeighbors(k, m, l, leavesIndex) != 0 || terrainBrush.CountNonDiagonalNeighbors(k, m, l, (Func<int?, int>)((int? v) => (v.HasValue && Terrain.ExtractContents(v.Value) == woodIndex) ? 1 : 0)) != 0))
							{
								terrainBrush.AddCell(k, m, l, 0);
							}
						}
					}
				}
				terrainBrush.Replace(0, leavesIndex);
			}
			terrainBrush.AddCell(0, height, 0, leavesIndex);

			List<Point3> list = new List<Point3>(); //创建一个列表，用于存储树枝生成的位置信息。
			float num1 = random.Float(0f, 6.2831855f); //生成一个随机的浮点数作为角度值，用于计算树枝的方向。
			for (int i = 0; i < 6; i++) //迭代3次，生成3个树枝
			{
				float radians = num1 + (float)i * MathUtils.DegToRad(120f); //计算当前树枝的角度，根据角度的增量120°来确定每个树枝之间的角度差。
				Vector3 v = Vector3.Transform(Vector3.Normalize(new Vector3(1f, random.Float(1f, 1.5f), 0f)), Matrix.CreateRotationY(radians)); //根据角度和随机生成的长度因子，计算树枝的方向向量。
				int num2 = random.Int((int)(0.7 * size), (int)(1.2 * size));  //树枝的长度由(int)(0.7f * size)和(int)size之间的随机整数决定。
				Point3 point = new Point3(0, height - 1, 0);   //树枝生成的起始位置
				Point3 point2 = new Point3(Vector3.Round(new Vector3(point) + v * (float)num2)); //根据起始位置、方向向量和长度计算树枝的结束位置。
				terrainBrush.AddRay(point.X, point.Y, point.Z, point2.X, point2.Y, point2.Z, 1, 1, 1, woodIndex); //向地形刷子添加一条射线，用于生成树枝。射线的起点是起始位置，终点是结束位置，宽度为1，高度为1，深度为1，方块类型为树干方块类型。
				list.Add(point2); //将树枝的结束位置添加到列表中，用于后续生成树枝周围的树叶。
			}
			foreach (Point3 point3 in list) //遍历存储树枝位置的列表。
			{
				float num3 = random.Float(0.45f * size, 0.6f * size); // 随机生成树叶的大小，大小范围为0.45倍到0.6倍的size值。
				int num4 = (int)MathUtils.Ceiling(num3); // 向上取整树叶的大小，得到树叶的半径。
				int trunkHeight = height; // 树枝的高度
				int maxHeight = point3.Y + trunkHeight + num4; // 树叶生成的最大高度限制为树枝高度加树叶半径

				for (int j = point3.X - num4; j <= point3.X + num4; j++) // 遍历树叶周围的X坐标范围。
				{
					for (int k = point3.Y; k <= maxHeight; k++) // 遍历树叶的高度范围，从树枝顶部到最大高度。
					{
						for (int l = point3.Z - num4; l <= point3.Z + num4; l++) // 遍历树叶周围的Z坐标范围。
						{
							int num5 = MathUtils.Abs(j - point3.X) + MathUtils.Abs(l - point3.Z); // 计算当前位置与树叶起始位置的Manhattan距离。
																								  // 计算当前位置与树叶起始位置的欧几里得距离，使用缩放因子(1, 1.7, 1)来调整距离。
							float num6 = ((new Vector3((float)j, (float)k, (float)l) - new Vector3(point3)) * new Vector3(1f, 1.7f, 1f)).Length();
							// 判断当前位置是否满足生成树叶的条件：距离树叶起始位置距离小于等于树叶大小、距离树叶起始位置的Manhattan距离小于等于树叶半径。
							if (num6 <= num3 && num5 <= num4 && terrainBrush.GetValue(j, k, l) == null)
							{
								terrainBrush.AddCell(j, k, l, leavesIndex); // 生成树叶
							}
						}
					}
				}
			}
			terrainBrush.Compile();
			return terrainBrush;
		}
		#endregion



		public static List<TerrainBrush>[] m_treeBrushesByType = new List<TerrainBrush>[Enumerable.Max(EnumUtils.GetEnumValues(typeof(FCTreeType))) + 1];

		public static int[] m_treeTrunksByType = new int[]
		{
			939,
			940,
			941,
			943,
			942
		};

		public static int[] m_treeLeavesByType = new int[]
		{
			944,
			945,
			946,
			948,
			947
		};
	}
	#endregion

	#region 树苗 （特殊值方块外置材质解决模板）

	public class FCSaplingBlock : FCCrossBlocks //id为949的新树苗 
	{

		public FCSaplingBlock()
		   : base("Textures/FCPlants/fcsapling")
		{
		}

		public override int GetTextureSlotCount(int value) //9宫格
		{
			return 3;
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, new Color(144, 238, 144), GetFaceTextureSlot(0, value));
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{


			BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);
		}
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			switch (Terrain.ExtractData(value))
			{
				case 0:
					return "可可树树苗";
				case 1:
					return "橘子树苗";
				case 2:
					return "苹果树苗";
				case 3:
					return "樱花树幼苗";
				case 4:
					return "老君树幼苗";
				default:
					return "树苗";
			}
		}



		public override int GetFaceTextureSlot(int face, int value)//按data值返回对应的槽位的贴图
		{
			switch (Terrain.ExtractData(value))
			{
				case 0:
					return 0;
				case 1:
					return 1;
				case 2:
					return 2;
				case 3:
					return 3;
				case 4:
					return 4;
				default:
					return 0;
			}
		}

		public override IEnumerable<int> GetCreativeValues()
		{
			yield return Terrain.MakeBlockValue(949, 0, 0);
			yield return Terrain.MakeBlockValue(949, 0, 1);
			yield return Terrain.MakeBlockValue(949, 0, 2);
			yield return Terrain.MakeBlockValue(949, 0, 3);
			yield return Terrain.MakeBlockValue(949, 0, 4);
			yield break;
		}

		public const int Index = 949;
	}
	#endregion


	#region 树苗生长行为子系统
	public class NewSubsystemSaplingBlockBehavior : SubsystemBlockBehavior, IUpdateable
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					949
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

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 1, z);
			if (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)].IsTransparent_(cellValue))
			{
				base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
			}
		}

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			float num = (this.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative) ? this.m_random.Float(8f, 12f) : this.m_random.Float(480f, 600f);
			AddSapling(new SaplingData
			{
				Point = new Point3(x, y, z),
				Type = (FCTreeType)Terrain.ExtractData(value),
				MatureTime = this.m_subsystemGameInfo.TotalElapsedGameTime + (double)num
			});
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			this.RemoveSapling(new Point3(x, y, z));
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
			this.m_subsystemTimeOfDay = base.Project.FindSubsystem<SubsystemTimeOfDay>(true);
			this.m_enumerator = this.m_saplings.Values.GetEnumerator();
			foreach (object obj in valuesDictionary.GetValue<ValuesDictionary>("Saplings").Values)
			{
				string data = (string)obj;
				this.AddSapling(this.LoadSaplingData(data));
			}
		}

		public override void Save(ValuesDictionary valuesDictionary)
		{
			ValuesDictionary valuesDictionary2 = new ValuesDictionary();
			valuesDictionary.SetValue<ValuesDictionary>("Saplings", valuesDictionary2);
			int num = 0;
			foreach (NewSubsystemSaplingBlockBehavior.SaplingData saplingData in this.m_saplings.Values)
			{
				valuesDictionary2.SetValue<string>(num++.ToString(CultureInfo.InvariantCulture), this.SaveSaplingData(saplingData));
			}
		}

		public void Update(float dt)
		{
			for (int i = 0; i < 10; i++)
			{
				if (!this.m_enumerator.MoveNext())
				{
					this.m_enumerator = this.m_saplings.Values.GetEnumerator();
					return;
				}
				this.MatureSapling(this.m_enumerator.Current);
			}
		}

		public NewSubsystemSaplingBlockBehavior.SaplingData LoadSaplingData(string data)
		{
			string[] array = data.Split(new string[]
			{
				";"
			}, StringSplitOptions.None);
			if (array.Length != 3)
			{
				throw new InvalidOperationException("Invalid sapling data string.");
			}
			return new NewSubsystemSaplingBlockBehavior.SaplingData
			{
				Point = HumanReadableConverter.ConvertFromString<Point3>(array[0]),
				Type = HumanReadableConverter.ConvertFromString<FCTreeType>(array[1]),
				MatureTime = HumanReadableConverter.ConvertFromString<double>(array[2])
			};
		}

		public string SaveSaplingData(NewSubsystemSaplingBlockBehavior.SaplingData saplingData)
		{
			this.m_stringBuilder.Length = 0;
			this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Point));
			this.m_stringBuilder.Append(';');
			this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.Type));
			this.m_stringBuilder.Append(';');
			this.m_stringBuilder.Append(HumanReadableConverter.ConvertToString(saplingData.MatureTime));
			return this.m_stringBuilder.ToString();
		}

		public void MatureSapling(NewSubsystemSaplingBlockBehavior.SaplingData saplingData)
		{
			if (this.m_subsystemGameInfo.TotalElapsedGameTime < saplingData.MatureTime)
			{
				return;
			}
			int x = saplingData.Point.X;
			int y = saplingData.Point.Y;
			int z = saplingData.Point.Z;
			TerrainChunk chunkAtCell = base.SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z - 6);
			TerrainChunk chunkAtCell2 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x - 6, z + 6);
			TerrainChunk chunkAtCell3 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z - 6);
			TerrainChunk chunkAtCell4 = base.SubsystemTerrain.Terrain.GetChunkAtCell(x + 6, z + 6);
			if (chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid && chunkAtCell2 != null && chunkAtCell2.State == TerrainChunkState.Valid && chunkAtCell3 != null && chunkAtCell3.State == TerrainChunkState.Valid && chunkAtCell4 != null && chunkAtCell4.State == TerrainChunkState.Valid)
			{
				int cellContents = base.SubsystemTerrain.Terrain.GetCellContents(x, y - 1, z);
				if (cellContents != 2 && cellContents != 8)
				{
					base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(28, 0, 0), true);
					return;
				}
				if (base.SubsystemTerrain.Terrain.GetCellLight(x, y + 1, z) >= 9)//光照
				{
					bool flag = false;
					for (int i = x - 1; i <= x + 1; i++)
					{
						for (int j = z - 1; j <= z + 1; j++)
						{
							int cellContents2 = base.SubsystemTerrain.Terrain.GetCellContents(i, y - 1, j);
							if (BlocksManager.Blocks[cellContents2] is WaterBlock)
							{
								flag = true;
								break;
							}
						}
					}
					float probability;
					if (this.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative)
					{
						probability = 1f;
					}
					else
					{
						int num = base.SubsystemTerrain.Terrain.GetTemperature(x, z) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
						int num2 = base.SubsystemTerrain.Terrain.GetHumidity(x, z);
						if (flag)
						{
							num = (num + 10) / 2;
							num2 = MathUtils.Max(num2, 12);
						}
						probability = 2f * FCPlantManager.CalculateTreeProbability(saplingData.Type, num, num2, y);
					}
					if (!this.m_random.Bool(probability))
					{
						base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(28, 0, 0), true);
						return;
					}
					base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(0, 0, 0), true);
					if (!this.GrowTree(x, y, z, saplingData.Type))
					{
						base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(28, 0, 0), true);
						return;
					}
				}
				else if (this.m_subsystemGameInfo.TotalElapsedGameTime > saplingData.MatureTime + (double)this.m_subsystemTimeOfDay.DayDuration)
				{
					base.SubsystemTerrain.ChangeCell(x, y, z, Terrain.MakeBlockValue(28, 0, 0), true);
					return;
				}
			}
			else
			{
				saplingData.MatureTime = this.m_subsystemGameInfo.TotalElapsedGameTime;
			}
		}

		public bool GrowTree(int x, int y, int z, FCTreeType treeType)
		{
			ReadOnlyList<TerrainBrush> treeBrushes = FCPlantManager.GetTreeBrushes(treeType);
			for (int i = 0; i < 20; i++)
			{
				TerrainBrush terrainBrush = treeBrushes[this.m_random.Int(0, treeBrushes.Count - 1)];
				bool flag = true;
				foreach (TerrainBrush.Cell cell in terrainBrush.Cells)
				{
					if (cell.Y >= 0 && (cell.X != 0 || cell.Y != 0 || cell.Z != 0))
					{
						int cellContents = base.SubsystemTerrain.Terrain.GetCellContents((int)cell.X + x, (int)cell.Y + y, (int)cell.Z + z);
						if (cellContents != 0 && !(BlocksManager.Blocks[cellContents] is LeavesBlock))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					terrainBrush.Paint(base.SubsystemTerrain, x, y, z);
					return true;
				}
			}
			return false;
		}

		public void AddSapling(NewSubsystemSaplingBlockBehavior.SaplingData saplingData)
		{
			this.m_saplings[saplingData.Point] = saplingData;
			this.m_enumerator = this.m_saplings.Values.GetEnumerator();
		}

		public void RemoveSapling(Point3 point)
		{
			this.m_saplings.Remove(point);
			this.m_enumerator = this.m_saplings.Values.GetEnumerator();
		}

		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTimeOfDay m_subsystemTimeOfDay;

		public Dictionary<Point3, NewSubsystemSaplingBlockBehavior.SaplingData> m_saplings = new Dictionary<Point3, NewSubsystemSaplingBlockBehavior.SaplingData>();

		public Dictionary<Point3, NewSubsystemSaplingBlockBehavior.SaplingData>.ValueCollection.Enumerator m_enumerator;

		public Random m_random = new Random();

		public StringBuilder m_stringBuilder = new StringBuilder();



		public class SaplingData
		{
			public Point3 Point;

			public FCTreeType Type;

			public double MatureTime;
		}
	}

    #endregion

    #endregion

    #region 区块（粒子系统渲染区块）

    #region 新颜色渲染
    public class NewBlockColorsMap
	{
		public Color[] m_map = new Color[256];

		public static NewBlockColorsMap RedTreePlantsBlocks = new NewBlockColorsMap(new Color(255, 0, 0), new Color(225, 69, 0), new Color(255, 20, 147), new Color(255, 20, 180));//30的差值
		public static NewBlockColorsMap BlueTreePlantsBlocks = new NewBlockColorsMap(new Color(0, 0, 255), new Color(0, 69, 230), new Color(0, 20, 220), new Color(0, 0, 180));//30的差值
		public static NewBlockColorsMap YellowTreePlantsBlocks = new NewBlockColorsMap(new Color(255, 255, 0), new Color(205, 205, 0), new Color(238, 173, 14), new Color(255, 215, 0));//30的差值
		public static NewBlockColorsMap OrangeTreePlantsBlocks = new NewBlockColorsMap(new Color(255, 69, 0), new Color(225, 69, 0), new Color(238, 118, 33), new Color(200, 118, 45));//30的差值
		public NewBlockColorsMap(Color th11, Color th21, Color th12, Color th22)
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					float f = MathUtils.Saturate(i / 8f);//2
					float f2 = MathUtils.Saturate((j - 4) / 10f);//1.2
					var c = Color.Lerp(th11, th21, f);//50,120,8
					var c2 = Color.Lerp(th12, th22, f);//
					var color = Color.Lerp(c, c2, f2);
					int num = i + j * 16;
					m_map[num] = color;
				}
			}
		}

		public Color Lookup(int temperature, int humidity)
		{
			int num = MathUtils.Clamp(temperature, 0, 15) + 16 * MathUtils.Clamp(humidity, 0, 15);
			return m_map[num];//255
		}

		public Color Lookup(Terrain terrain, int x, int y, int z)
		{
			int shaftValue = terrain.GetShaftValue(x, z);
			int temperature = terrain.GetSeasonalTemperature(shaftValue) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
			int seasonalHumidity = terrain.GetSeasonalHumidity(shaftValue);
			return Lookup(temperature, seasonalHumidity);
		}
	}
    #endregion

    #region FC子系统天空
	public class FCSubsystemSky:SubsystemSky, IDrawable, IUpdateable
	{
		public SubsystemPlayers m_subsystemPlayer;
		public ComponentPlayer m_componentPlayer;
		public ComponentTest1 m_componentTest1;
		public SubsystemWorldDemo m_subsystemWorldDemo;

        public override void OnEntityAdded(Entity entity)
        {
            ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
			{
                m_componentPlayer = componentPlayer;
                m_componentTest1 = m_componentPlayer.Entity.FindComponent<ComponentTest1>();
            }
                

            
            
            
        }

        public float FCCalculateLightIntensity(float timeOfDay, Camera camera)
        {
            float num = 1f;
            bool flag = camera.ViewPosition.Y > 1000f;
            if (flag)
            {
                num = 1000f / camera.ViewPosition.Y * (1000f / camera.ViewPosition.Y) * (1000f / camera.ViewPosition.Y);
            }
            bool flag2 = camera.ViewPosition.Y > 2000f;
            if (flag2)
            {
                num = 0f;
            }
            bool flag3 = timeOfDay <= 0.2f || timeOfDay > 0.8f;
            float result;
            if (flag3)
            {
                result = 0f;
            }
            else
            {
                bool flag4 = timeOfDay > 0.2f && timeOfDay <= 0.3f;
                if (flag4)
                {
                    result = (timeOfDay - 0.2f) / 0.10000001f * num;
                }
                else
                {
                    bool flag5 = timeOfDay > 0.3f && timeOfDay <= 0.7f;
                    if (flag5)
                    {
                        result = 1f * num;
                    }
                    else
                    {
                        result = (1f - (timeOfDay - 0.7f) / 0.100000024f) * num;
                    }
                }
            }
            return result;
        }
        public void FCFillSkyVertexBuffer(SubsystemSky.SkyDome skyDome, float timeOfDay, float precipitationIntensity, int temperature,Camera camera)
        {
            for (int i = 0; i < 8; i++)
            {
                float x = 1.5707964f * MathUtils.Sqr((float)i / 7f);
                for (int j = 0; j < 10; j++)
                {
                    int num = j + i * 10;
                    float x2 = 6.2831855f * (float)j / 10f;
                    float num2 = 1800f * MathUtils.Cos(x);
                    skyDome.Vertices[num].Position.X = num2 * MathUtils.Sin(x2);
                    skyDome.Vertices[num].Position.Z = num2 * MathUtils.Cos(x2);
                    skyDome.Vertices[num].Position.Y = 1800f * MathUtils.Sin(x) - ((i == 0) ? 450f : 0f);
                    skyDome.Vertices[num].Color = this.FCCalculateSkyColor(skyDome.Vertices[num].Position, timeOfDay, precipitationIntensity, temperature,camera);
                }
            }
            skyDome.VertexBuffer.SetData<SubsystemSky.SkyVertex>(skyDome.Vertices, 0, skyDome.Vertices.Length, 0);
        }
        public Color FCCalculateSkyColor(Vector3 direction, float timeOfDay, float precipitationIntensity, int temperature, Camera camera)
        {
            WorldType worldType = m_subsystemWorldDemo.worldType;
           // if (worldType == WorldType.Default)
            direction = Vector3.Normalize(direction);
            Vector2 vector = Vector2.Normalize(new Vector2(direction.X, direction.Z));
            float s = this.FCCalculateLightIntensity(timeOfDay, camera);
            float f = (float)temperature / 15f;
            Vector3 v = new Vector3(0.65f, 0.68f, 0.7f) * s;
            Vector3 v2 = Vector3.Lerp(new Vector3(0.28f, 0.38f, 0.52f), new Vector3(0.15f, 0.3f, 0.56f), f);
            Vector3 v3 = Vector3.Lerp(new Vector3(0.7f, 0.79f, 0.88f), new Vector3(0.64f, 0.77f, 0.91f), f);
            Vector3 v4 = Vector3.Lerp(v2, v, precipitationIntensity) * s;
            Vector3 v5 = Vector3.Lerp(v3, v, precipitationIntensity) * s;
            Vector3 v6 = new Vector3(1f, 0.3f, -0.2f);
            Vector3 v7 = new Vector3(1f, 0.3f, -0.2f);
            bool flag = this.m_lightningStrikePosition != null;
            if (flag)
            {
                v4 = Vector3.Max(new Vector3(this.m_lightningStrikeBrightness), v4);
            }
            float num = MathUtils.Lerp(SubsystemSky.CalculateDawnGlowIntensity(timeOfDay) * MathUtils.Clamp(1000f / camera.ViewPosition.Y, 0f, 1f), 0f, precipitationIntensity);
            float num2 = MathUtils.Lerp(SubsystemSky.CalculateDuskGlowIntensity(timeOfDay) * MathUtils.Clamp(1000f / camera.ViewPosition.Y, 0f, 1f), 0f, precipitationIntensity);
            float f2 = MathUtils.Saturate((direction.Y - 0.1f) / 0.4f);
            float s2 = num * MathUtils.Sqr(MathUtils.Saturate(0f - vector.X));
            float s3 = num2 * MathUtils.Sqr(MathUtils.Saturate(vector.X));
            if (m_componentTest1 != null)
            {
                if (worldType == WorldType.Default)//如果是主世界（地球）
				{
					
                    if (m_componentTest1.Sen < 20)
                    {
                        return new Color(0.2f, 0f, 0f);
                    }
					else if(m_componentTest1.Areaname == "血泪之池")

                    {
                        return Color.DarkRed;
                    }
                    else
                    {
                        return new Color(Vector3.Lerp(v5 + v6 * s2 + v7 * s3, v4, f2));
                    }
                }
				else if (worldType == WorldType.Moon|| worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球，渲染黑色
				{
                    if (m_componentTest1.Sen < 20)//sen值为底层渲染决定因素
                    {
                        return new Color(0.2f, 0f, 0f);
                    }
                    else
                    {
                        return new Color(0f,0f,0f);
                    }
                }


            }
            return new Color(Vector3.Lerp(v5 + v6 * s2 + v7 * s3, v4, f2));
        }
        public  void FillSkyVertexBuffer(SubsystemSky.SkyDome skyDome, float timeOfDay, float precipitationIntensity, int temperature,Camera camera)
        {
            for (int i = 0; i < 8; i++)
            {
                float x = 1.5707964f * MathUtils.Sqr((float)i / 7f);
                for (int j = 0; j < 10; j++)
                {
                    int num = j + i * 10;
                    float x2 = 6.2831855f * (float)j / 10f;
                    float num2 = 1800f * MathUtils.Cos(x);
                    skyDome.Vertices[num].Position.X = num2 * MathUtils.Sin(x2);
                    skyDome.Vertices[num].Position.Z = num2 * MathUtils.Cos(x2);
                    skyDome.Vertices[num].Position.Y = 1800f * MathUtils.Sin(x) - ((i == 0) ? 450f : 0f);
                    skyDome.Vertices[num].Color = this.FCCalculateSkyColor(skyDome.Vertices[num].Position, timeOfDay, precipitationIntensity, temperature,camera);
                }
            }
            skyDome.VertexBuffer.SetData<SubsystemSky.SkyVertex>(skyDome.Vertices, 0, skyDome.Vertices.Length, 0);
        }
        public new void Draw(Camera camera, int drawOrder)
        {
            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            //第一个和雾有关
            if (drawOrder == this.m_drawOrders[0])
            {
                this.ViewUnderWaterDepth = 0f;
                this.ViewUnderMagmaDepth = 0f;
                Vector3 viewPosition = camera.ViewPosition;
                int x = Terrain.ToCell(viewPosition.X);
                int y = Terrain.ToCell(viewPosition.Y);
                int z = Terrain.ToCell(viewPosition.Z);
                FluidBlock fluidBlock;
                float? surfaceHeight = this.m_subsystemFluidBlockBehavior.GetSurfaceHeight(x, y, z, out fluidBlock);
                if (surfaceHeight != null)
                {
                    if (fluidBlock is WaterBlock)
                    {
                        this.ViewUnderWaterDepth = surfaceHeight.Value + 0.1f - viewPosition.Y;
                    }
                    else if (fluidBlock is MagmaBlock)
                    {
                        this.ViewUnderMagmaDepth = surfaceHeight.Value + 1f - viewPosition.Y;
                    }
					else if(fluidBlock is BloodBlock)
					{
                        this.ViewUnderWaterDepth = surfaceHeight.Value + 0.1f - viewPosition.Y;
                    }
                }
                if (this.ViewUnderWaterDepth > 0f)//水下
                {
					if(fluidBlock is BloodBlock)
					{
                        int seasonalHumidity = this.m_subsystemTerrain.Terrain.GetSeasonalHumidity(x, z);
                        int temperature = this.m_subsystemTerrain.Terrain.GetSeasonalTemperature(x, z) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
                        Color c = BlockColorsMap.WaterColorsMap.Lookup(temperature, seasonalHumidity);
                        float num = MathUtils.Lerp(1f, 0.5f, (float)seasonalHumidity / 15f);
                        float num2 = MathUtils.Lerp(1f, 0.2f, MathUtils.Saturate(0.075f * (this.ViewUnderWaterDepth - 2f)));
                        float num3 = MathUtils.Lerp(0.33f, 1f, this.SkyLightIntensity);
                        this.m_viewFogRange.X = 0f;
                        this.m_viewFogRange.Y = MathUtils.Lerp(4f, 10f, num * num2 * num3);
						this.m_viewFogColor = Color.DarkRed;
                        this.VisibilityRangeYMultiplier = 1f;
                        
                    }
					else
					{
                        int seasonalHumidity = this.m_subsystemTerrain.Terrain.GetSeasonalHumidity(x, z);
                        int temperature = this.m_subsystemTerrain.Terrain.GetSeasonalTemperature(x, z) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
                        Color c = BlockColorsMap.WaterColorsMap.Lookup(temperature, seasonalHumidity);
                        float num = MathUtils.Lerp(1f, 0.5f, (float)seasonalHumidity / 15f);
                        float num2 = MathUtils.Lerp(1f, 0.2f, MathUtils.Saturate(0.075f * (this.ViewUnderWaterDepth - 2f)));
                        float num3 = MathUtils.Lerp(0.33f, 1f, this.SkyLightIntensity);
                        this.m_viewFogRange.X = 50f;
                        this.m_viewFogRange.Y = MathUtils.Lerp(50f, 200f, num * num2 * num3);
                        this.m_viewFogColor = Color.MultiplyColorOnly(c, 0.66f * num2 * num3);
                        this.VisibilityRangeYMultiplier = 1f;
                        this.m_viewIsSkyVisible = false;
                    }
                    
                }
                else if (this.ViewUnderMagmaDepth > 0f)//岩浆
                {
                    this.m_viewFogRange.X = 0f;
                    this.m_viewFogRange.Y = 0.1f;
                    this.m_viewFogColor = new Color(255, 80, 0);//雾气颜色
                    this.VisibilityRangeYMultiplier = 1f;
                    this.m_viewIsSkyVisible = false;
                }
                else
                {
                    float num4 = 1024f;
                    float num5 = 128f;
                    int seasonalTemperature = this.m_subsystemTerrain.Terrain.GetSeasonalTemperature(Terrain.ToCell(viewPosition.X), Terrain.ToCell(viewPosition.Z));
                    float num6 = MathUtils.Lerp(0.5f, 0f, this.m_subsystemWeather.GlobalPrecipitationIntensity);
                    float num7 = MathUtils.Lerp(1f, 0.8f, this.m_subsystemWeather.GlobalPrecipitationIntensity);
                    this.m_viewFogRange.X = this.VisibilityRange * num6;
                    this.m_viewFogRange.Y = this.VisibilityRange * num7;
                    this.m_viewFogColor = this.CalculateSkyColor(new Vector3(camera.ViewDirection.X, 0f, camera.ViewDirection.Z), this.m_subsystemTimeOfDay.TimeOfDay, this.m_subsystemWeather.GlobalPrecipitationIntensity, seasonalTemperature);
                    this.VisibilityRangeYMultiplier = MathUtils.Lerp(this.VisibilityRange / num4, this.VisibilityRange / num5, MathUtils.Pow(this.m_subsystemWeather.GlobalPrecipitationIntensity, 4f));
                    this.m_viewIsSkyVisible = true;
                }
                if (!this.FogEnabled)
                {
                    this.m_viewFogRange = new Vector2(100000f, 100000f);
                }

				//处理无大气的星球雾气渲染
                if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球，渲染黑色
				{
                    this.m_viewFogColor = FCCalculateSkyColor(new Vector3(camera.ViewDirection.X, 0f, camera.ViewDirection.Z), this.m_subsystemTimeOfDay.TimeOfDay, this.m_subsystemWeather.GlobalPrecipitationIntensity, 0,camera);

                }
				else if(worldType == WorldType.Default)
				{
					if(m_componentTest1.Areaname=="血泪之池")
					{
                        float RangeFactor = m_componentTest1.Sen / 100f;
                        m_viewFogColor = Color.DarkRed;
                        m_viewFogRange.X = MathUtils.Lerp(3f, 50f, RangeFactor);//表示雾气的起始距离，也就是从摄像机多远的地方开始雾化的效果。
                        m_viewFogRange.Y = MathUtils.Lerp(50f, 60f, RangeFactor);//表示雾气的结束距离，即雾气效果完全覆盖视野的最大距离,sen越低。离人越近。
                    }
				}

                if (m_componentTest1.Sen < 20)//如果sen小于20
                {
                    

                    float RangeFactor = m_componentTest1.Sen / 20f;
                    m_viewFogColor = Color.DarkRed;
					
                    m_viewFogRange.X = MathUtils.Lerp(3f, 15f, RangeFactor);//表示雾气的起始距离，也就是从摄像机多远的地方开始雾化的效果。
                    m_viewFogRange.Y = MathUtils.Lerp(16f, 30f,RangeFactor);//表示雾气的结束距离，即雾气效果完全覆盖视野的最大距离,sen越低。离人越近。
                    FlatBatch2D flatBatch2D = this.m_primitivesRenderer2d.FlatBatch(-1, DepthStencilState.None, RasterizerState.CullNoneScissor, BlendState.Opaque);
                    int count = flatBatch2D.TriangleVertices.Count;
                   

                    flatBatch2D.QueueQuad(Vector2.Zero, camera.ViewportSize, 0f, this.m_viewFogColor);
                    flatBatch2D.TransformTriangles(camera.ViewportMatrix, count, -1);
                    this.m_primitivesRenderer2d.Flush(true, int.MaxValue);
                    return;

                }
                else if (m_componentTest1.Sen <= 10)//如果sen小于10
                {


                    float RangeFactor = m_componentTest1.Sen / 20f;
                    m_viewFogColor = Color.DarkRed;
                    
                    m_viewFogRange.X = MathUtils.Lerp(3f, 15f, RangeFactor);//表示雾气的起始距离，也就是从摄像机多远的地方开始雾化的效果。
                    m_viewFogRange.Y = MathUtils.Lerp(16f, 30f, RangeFactor);//表示雾气的结束距离，即雾气效果完全覆盖视野的最大距离,sen越低。离人越近。
                    FlatBatch2D flatBatch2D = this.m_primitivesRenderer2d.FlatBatch(-1, DepthStencilState.None, RasterizerState.CullNoneScissor, BlendState.Opaque);
                    int count = flatBatch2D.TriangleVertices.Count;
                   

                    flatBatch2D.QueueQuad(Vector2.Zero, camera.ViewportSize, 0f, this.m_viewFogColor);
                    flatBatch2D.TransformTriangles(camera.ViewportMatrix, count, -1);
                    this.m_primitivesRenderer2d.Flush(true, int.MaxValue);
                    return;

                }
                else if (!this.DrawSkyEnabled || !this.m_viewIsSkyVisible || SettingsManager.SkyRenderingMode == SkyRenderingMode.Disabled)
                {
                    FlatBatch2D flatBatch2D = this.m_primitivesRenderer2d.FlatBatch(-1, DepthStencilState.None, RasterizerState.CullNoneScissor, BlendState.Opaque);
                    int count = flatBatch2D.TriangleVertices.Count;
                   

                    flatBatch2D.QueueQuad(Vector2.Zero, camera.ViewportSize, 0f, this.m_viewFogColor);
                    flatBatch2D.TransformTriangles(camera.ViewportMatrix, count, -1);
                    this.m_primitivesRenderer2d.Flush(true, int.MaxValue);
                    return;

                }
            }
            else if (drawOrder == this.m_drawOrders[1])
            {
                if (this.DrawSkyEnabled && this.m_viewIsSkyVisible && SettingsManager.SkyRenderingMode != SkyRenderingMode.Disabled)
                {
                    this.DrawSkydome(camera);
                    if (SubsystemSky.DrawGalaxyEnabled)
                    {
                        if (worldType == WorldType.Default)//如果是主世界（地球）
						{
                            DrawStars(camera);
                            DrawSunAndMoon(camera);
                            DrawClouds(camera);
                            DrawEarth(camera);
                        }
                        else if (worldType == WorldType.Moon )//如果是无大气层星球
						{
                            DrawStars(camera);
                            DrawSunAndMoon(camera);
                            DrawClouds(camera);
							DrawPlanetSurface(camera);
                        }
                        else if (worldType == WorldType.StationMoon)//如果是地月空间站，或者空间站类型
						{
                            DrawStars(camera);
                            DrawSunAndMoon(camera);
                            DrawClouds(camera);
                        }
                        else//默认
						{
                            DrawStars(camera);
                            DrawSunAndMoon(camera);
                            DrawClouds(camera);
                            DrawEarth(camera);
                        }



                    }
                    
                   
                    if (SubsystemSky.Shader != null && SubsystemSky.ShaderAlphaTest != null)
                    {
                        if (this.m_primitiveRender.Shader == null && this.m_primitiveRender.ShaderAlphaTest == null)
                        {
                            this.m_primitiveRender.Shader = SubsystemSky.Shader;
                            this.m_primitiveRender.ShaderAlphaTest = SubsystemSky.ShaderAlphaTest;
                            this.m_primitiveRender.Camera = camera;
                        }
                        this.m_primitiveRender.Flush(this.m_primitivesRenderer3d, camera.ViewProjectionMatrix, true, int.MaxValue);
                        return;
                    }
                    this.m_primitivesRenderer3d.Flush(camera.ViewProjectionMatrix, true, int.MaxValue);
                    return;
                }
            }
            else
            {
                this.DrawLightning(camera);
                this.m_primitivesRenderer3d.Flush(camera.ViewProjectionMatrix, true, int.MaxValue);
            }
        }
        public void DrawEarth(Camera camera)
        {
            float f = 0f;
            float num = 12f;
            float f2 = MathUtils.Max(SubsystemSky.CalculateDawnGlowIntensity(num), SubsystemSky.CalculateDuskGlowIntensity(num));
            float num2 = 2f * num * 3.1415927f;
            float num3 = num2;
            float num4 = MathUtils.Lerp(90f, 160f, f2);
            float num5 = MathUtils.Lerp(60f, 80f, f2);
            Color c = Color.Lerp(new Color(255, 255, 255), new Color(255, 255, 160), f2);
            Color color = new Color(255, 255, 255);
            color *= 1f;
            c *= MathUtils.Lerp(1f, 0f, f);
            color *= MathUtils.Lerp(1f, 0f, f);
            TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_EarthTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
            float num6 = Vector2.Angle(new Vector2(1f, 1f), new Vector2(camera.ViewPosition.X, camera.ViewPosition.Z)) / camera.ViewPosition.Y;
            bool flag = camera.ViewPosition.Y <= 600 && camera.ViewPosition.Y > 360f;
            if (flag)
            {
                base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 100f + 150f, num5 * 100f / camera.ViewPosition.Y * 4f * 10f, num3 - num6);
            }
            bool flag2 = camera.ViewPosition.Y > 600f;
            if (flag2)
            {
                base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 400f + 100f, num5 * 10f / (camera.ViewPosition.Y / 1000f) * 4f, num3 - num6);
            }
            /*bool flag3 = camera.ViewPosition.Y < -120f;
            if (flag2)
            {
                base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 400f + 100f, num5 * 10f / (camera.ViewPosition.Y / 1000f) * 4f, num3 - num6);
            }*/
        }
        public void DrawPlanetSurface(Camera camera)
        {
            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            float f = 0f;
            float num = 12f;
            float f2 = MathUtils.Max(SubsystemSky.CalculateDawnGlowIntensity(num), SubsystemSky.CalculateDuskGlowIntensity(num));
            float num2 = 2f * num * 3.1415927f;
            float num3 = num2;
            float num4 = MathUtils.Lerp(90f, 160f, f2);
            float num5 = MathUtils.Lerp(60f, 80f, f2);
            Color c = Color.Lerp(new Color(255, 255, 255), new Color(255, 255, 160), f2);
            Color color = new Color(255, 255, 255);
            color *= 1f;
            c *= MathUtils.Lerp(1f, 0f, f);
            color *= MathUtils.Lerp(1f, 0f, f);
            TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_EarthTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);

            if (worldType == WorldType.Moon )//如果是月球，贴图置换
            {
                 batch = this.m_primitivesRenderer3d.TexturedBatch(m_moon, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);

            }

            if (worldType == WorldType.Default)//如果是月球，贴图置换
			{
                float num6 = Vector2.Angle(new Vector2(1f, 1f), new Vector2(camera.ViewPosition.X, camera.ViewPosition.Z)) / camera.ViewPosition.Y;
                bool flag = camera.ViewPosition.Y <= 600 && camera.ViewPosition.Y > 360f;
                if (flag)
                {
                    base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 100f + 150f, num5 * 100f / camera.ViewPosition.Y * 4f * 10f, num3 - num6);
                }
                bool flag2 = camera.ViewPosition.Y > 600f;
                if (flag2)
                {
                    base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 400f + 100f, num5 * 10f / (camera.ViewPosition.Y / 1000f) * 4f, num3 - num6);
                }
            }
			else if (worldType == WorldType.Moon )
			{
                float num6 = Vector2.Angle(new Vector2(1f, 1f), new Vector2(camera.ViewPosition.X, camera.ViewPosition.Z)) / camera.ViewPosition.Y;
                bool flag =   camera.ViewPosition.Y > 360f;
                if (flag)
                {
                    base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 100f + 150f, num5 * 100f / camera.ViewPosition.Y * 4f * 10f, num3 - num6);
                }
            }
			

            /*bool flag3 = camera.ViewPosition.Y < -120f;
            if (flag2)
            {
                base.QueueCelestialBody(batch, camera.ViewPosition, color, camera.ViewPosition.Y / 400f + 100f, num5 * 10f / (camera.ViewPosition.Y / 1000f) * 4f, num3 - num6);
            }*/
        }
        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            this.m_EarthTexture = ContentManager.Get<Texture2D>("Textures/Earth");
			m_subsystemPlayer = Project.FindSubsystem<SubsystemPlayers>();
			m_subsystemWorldDemo = Project.FindSubsystem<SubsystemWorldDemo>();
            m_earth = ContentManager.Get<Texture2D>("Textures/Earth");
            m_moon = ContentManager.Get<Texture2D>("Textures/MoonShadow");
        }
        public Texture2D m_EarthTexture;
		public float f1;//渲染星球的明度
        public Color color;//渲染辉光
        public new void DrawSunAndMoon(Camera camera)
        {
            float num8 = (camera.ViewPosition.Y > 1000f) ? 0f : this.m_subsystemWeather.GlobalPrecipitationIntensity;
            float timeOfDay = this.m_subsystemTimeOfDay.TimeOfDay;
            bool flag = this.m_starsVertexBuffer == null || this.m_starsIndexBuffer == null;
            if (flag)
            {
                Utilities.Dispose<VertexBuffer>(ref this.m_starsVertexBuffer);
                Utilities.Dispose<IndexBuffer>(ref this.m_starsIndexBuffer);
                this.m_starsVertexBuffer = new VertexBuffer(this.m_starsVertexDeclaration, 200);
                this.m_starsIndexBuffer = new IndexBuffer(IndexFormat.SixteenBits, 500);
                base.FillStarsBuffers();
            }
            Display.DepthStencilState = DepthStencilState.DepthRead;
            Display.RasterizerState = RasterizerState.CullNoneScissor;


            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            if (worldType == WorldType.Default)//如果是主世界（地球）
			{
                 f1 = (camera.ViewPosition.Y > 1000f) ? 0f : this.m_subsystemWeather.GlobalPrecipitationIntensity;
            }
            else if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球，渲染黑色
			{
				 f1 = 0;
			}
			else
			{
                f1 = (camera.ViewPosition.Y > 1000f) ? 0f : this.m_subsystemWeather.GlobalPrecipitationIntensity;
            }

           
            float f2 = MathUtils.Max(SubsystemSky.CalculateDawnGlowIntensity(timeOfDay), SubsystemSky.CalculateDuskGlowIntensity(timeOfDay));
            float num = 2f * timeOfDay * 3.1415927f;
            float angle = num + 3.1415927f;
			//默认
            float num2 = MathUtils.Lerp(90f, 160f, f2);//太阳大小
            float num3 = MathUtils.Lerp(60f, 80f, f2);//月球大小

            if (worldType == WorldType.Default)//如果是主世界（地球）
            {
                num2 = MathUtils.Lerp(90f, 160f, f2);//太阳大小
                num3 = MathUtils.Lerp(60f, 80f, f2);//月球大小
            }
            else if (worldType == WorldType.Moon )//如果是月球
            {
                num2 = MathUtils.Lerp(90f, 160f, f2);//太阳大小
                num3 = MathUtils.Lerp(180f, 240f, f2);//地球大小
            }
			else if( worldType == WorldType.StationMoon)//如果是地月空间站
			{
                num2 = MathUtils.Lerp(90f, 160f, f2);//太阳大小
                num3 = MathUtils.Lerp(1600f, 1800f, f2);//地球大小
            }
            
           

			//调节辉光color
            if (worldType == WorldType.Default)//如果是主世界（地球）
            {
                color = Color.Lerp(new Color(255, 255, 255), new Color(255, 255, 160), f2);//辉光
            }
            else if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球
            {
                color = new Color(255, 255, 255);//无辉光
            }
			else
			{
                color = Color.Lerp(new Color(255, 255, 255), new Color(255, 255, 160), f2);//辉光
            }


           //调节星球可见度
            Color color2 = Color.White;
            if (worldType == WorldType.Default)//如果是主世界（地球）
            {
                color2 *= 1f - base.SkyLightIntensity;//月球在背面可见度
            }
            else if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球
            {
				color2 *= 1f;//无论如何都可见
            }
			else
			{
                color2 *= 1f - base.SkyLightIntensity;//月球可见度
            }
            



            color *= MathUtils.Lerp(1f, 0f, f1);
            color2 *= MathUtils.Lerp(1f, 0f, f1);
            Color color3 = color * 0.6f * MathUtils.Lerp(1f, 0f, f1);
            Color color4 = color * 0.2f * MathUtils.Lerp(1f, 0f, f1);
            if (worldType == WorldType.Default)//如果是主世界（地球）
			{
                TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_glowTexture, false, 0, DepthStencilState.DepthRead, null, BlendState.Additive, null);
                TexturedBatch3D batch2 = this.m_primitivesRenderer3d.TexturedBatch(this.m_sunTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                TexturedBatch3D batch3 = this.m_primitivesRenderer3d.TexturedBatch(this.m_moonTextures[base.MoonPhase], false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                base.QueueCelestialBody(batch, camera.ViewPosition, color3, 900f, 3.5f * num2, num);
                base.QueueCelestialBody(batch, camera.ViewPosition, color4, 900f, 3.5f * num3, angle);
                base.QueueCelestialBody(batch2, camera.ViewPosition, color, 900f, num2, num);
                base.QueueCelestialBody(batch3, camera.ViewPosition, color2, 900f, num3, angle);
            }
            else if (worldType == WorldType.Moon )//如果是月球的星空
			{
                TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_glowTexture, false, 0, DepthStencilState.DepthRead, null, BlendState.Additive, null);
                TexturedBatch3D batch2 = this.m_primitivesRenderer3d.TexturedBatch(this.m_sunTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                TexturedBatch3D batch3 = this.m_primitivesRenderer3d.TexturedBatch(m_earth, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);

                base.QueueCelestialBody(batch, camera.ViewPosition, color3, 900f, 3.5f * num2, num);
                base.QueueCelestialBody(batch, camera.ViewPosition, color4, 900f, 3.5f * num3, angle);
                base.QueueCelestialBody(batch2, camera.ViewPosition, color, 900f, num2, num);
                base.QueueCelestialBody(batch3, camera.ViewPosition, color2, 900f, num3, angle);
            }
            else if ( worldType == WorldType.StationMoon)//地月空间站
			{
                float num7 = MathUtils.Sqr((1f - this.FCCalculateLightIntensity(timeOfDay, camera)) * (1f -num8));
                //这是着色器绘制星空
                Display.BlendState = BlendState.Additive;
                SubsystemSky.m_shaderTextured.Transforms.World[0] = CreateEarthRotationMatrix(timeOfDay,3f) * Matrix.CreateTranslation(camera.ViewPosition) * camera.ViewProjectionMatrix;
                SubsystemSky.m_shaderTextured.Color = new Vector4(3.4f, 4f, 4f, 255f);
                SubsystemSky.m_shaderTextured.Texture = ContentManager.Get<Texture2D>("Textures/Stone");
                SubsystemSky.m_shaderTextured.SamplerState = SamplerState.LinearClamp;
                Display.DrawIndexed(PrimitiveType.TriangleList, SubsystemSky.m_shaderTextured, this.m_starsVertexBuffer, this.m_starsIndexBuffer, 0, this.m_starsIndexBuffer.IndicesCount);

				
                TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_glowTexture, false, 0, DepthStencilState.DepthRead, null, BlendState.Additive, null);
                TexturedBatch3D batch2 = this.m_primitivesRenderer3d.TexturedBatch(m_moon, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                TexturedBatch3D batch3 = this.m_primitivesRenderer3d.TexturedBatch(m_earth, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                TexturedBatch3D batch4 = this.m_primitivesRenderer3d.TexturedBatch(m_sunTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);

				//float earthRotationAngle = 2f * timeOfDay * 3.1415927f; // 假设地球每天旋转一圈
				if(timeOfDay<0.22||timeOfDay>0.77)
				{
                    color *= SkyLightIntensity*MathUtils.Lerp(1f,0f,timeOfDay);//线性变化，1是深夜，随着夜深，逐渐不见
                }
				
                //base.QueueCelestialBody(batch, camera.ViewPosition, color3, 900f, 3.5f * num2, num);
                //base.QueueCelestialBody(batch, camera.ViewPosition, color4, 900f, 3.5f * num3, angle);
                base.QueueCelestialBody(batch2, camera.ViewPosition, color2, 900f, num2, angle);//月球
                base.QueueCelestialBody(batch3, camera.ViewPosition, color2, 800f, num3, num);//地球
                base.QueueCelestialBody(batch4, camera.ViewPosition, color, 900f, num2, num);//太阳
                batch3.TransformTriangles(Matrix.CreateRotationZ(2f * timeOfDay * 3.1415927f));//这个貌似决定了地球的位置会在一圈的哪里，0会和太阳一样转，2固定在脚下
            }
			else//默认
			{
                TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_glowTexture, false, 0, DepthStencilState.DepthRead, null, BlendState.Additive, null);
                TexturedBatch3D batch2 = this.m_primitivesRenderer3d.TexturedBatch(this.m_sunTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                TexturedBatch3D batch3 = this.m_primitivesRenderer3d.TexturedBatch(this.m_moonTextures[base.MoonPhase], false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
                base.QueueCelestialBody(batch, camera.ViewPosition, color3, 900f, 3.5f * num2, num);
                base.QueueCelestialBody(batch, camera.ViewPosition, color4, 900f, 3.5f * num3, angle);
                base.QueueCelestialBody(batch2, camera.ViewPosition, color, 900f, num2, num);
                base.QueueCelestialBody(batch3, camera.ViewPosition, color2, 900f, num3, angle);
            }


        }
        public Texture2D m_earth;//地球渲染
        public Texture2D m_moon;//月球渲染
        public Matrix CreateEarthRotationMatrix(float timeOfDay, float earthRotationSpeed = 1f)
        {
            // 地球每天旋转360度，
            float earthRotationAngle = timeOfDay * earthRotationSpeed * 3.1415927f;

            // 地球自转的轴线倾角约为23.5度，这里将其转换为弧度
            float earthTiltAngle = 23.5f;

            // 创建绕地球自转轴进行旋转的变换矩阵，先绕Z轴旋转倾角，再绕变换后的Y轴旋转角度
            Matrix earthTilt = Matrix.CreateRotationZ(earthTiltAngle); // 倾角
            Matrix earthTilt2 = Matrix.CreateRotationZ(earthRotationAngle); // 倾角
            Matrix earthRotation = Matrix.CreateRotationY(earthRotationAngle); // 自转

			// 将地球的倾角旋转和自转旋转组合起来
			Matrix finalEarthRotation = earthTilt*earthRotation;

            return finalEarthRotation;
        }
        public new void DrawClouds(Camera camera)
        {
            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            if (SettingsManager.SkyRenderingMode == SkyRenderingMode.NoClouds || camera.ViewPosition.Y > 1000f)
            {
                return;
            }
			if(m_componentTest1!= null)
			{
                if (m_componentTest1.Sen < 30)
                {
                    m_cloudsTexture = ContentManager.Get<Texture2D>("Textures/CloudsSen", null);
                }
                else
                {
                    m_cloudsTexture = ContentManager.Get<Texture2D>("Textures/Clouds", null);
                    if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球，不渲染云
					{
						return;
					}

                }
            }
			
            
            float globalPrecipitationIntensity = this.m_subsystemWeather.GlobalPrecipitationIntensity;
            float num = MathUtils.Lerp(0.03f, 1f, MathUtils.Sqr(this.SkyLightIntensity)) * MathUtils.Lerp(1f, 0.2f, globalPrecipitationIntensity);
            this.m_cloudsLayerColors[0] = Color.White * (num * 0.75f);
            this.m_cloudsLayerColors[1] = Color.White * (num * 0.66f);
            this.m_cloudsLayerColors[2] = this.ViewFogColor;
            this.m_cloudsLayerColors[3] = Color.Transparent;
            double gameTime = this.m_subsystemTime.GameTime;
            Vector3 viewPosition = camera.ViewPosition;
            Vector2 v = new Vector2((float)MathUtils.Remainder(0.0020000000949949026 * gameTime - (double)(viewPosition.X / 1900f * 1.75f), 1.0) + viewPosition.X / 1900f * 1.75f, (float)MathUtils.Remainder(0.0020000000949949026 * gameTime - (double)(viewPosition.Z / 1900f * 1.75f), 1.0) + viewPosition.Z / 1900f * 1.75f);
            TexturedBatch3D texturedBatch3D = this.m_primitivesRenderer3d.TexturedBatch(m_cloudsTexture, false, 2, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, SamplerState.LinearWrap);
            DynamicArray<VertexPositionColorTexture> triangleVertices = texturedBatch3D.TriangleVertices;
            DynamicArray<int> triangleIndices = texturedBatch3D.TriangleIndices;
            int count = triangleVertices.Count;
            int count2 = triangleVertices.Count;
            int count3 = triangleIndices.Count;
            triangleVertices.Count += 49;
            triangleIndices.Count += 216;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int num2 = j - 3;
                    int num3 = i - 3;
                    int num4 = MathUtils.Max(MathUtils.Abs(num2), MathUtils.Abs(num3));
                    float num5 = this.m_cloudsLayerRadii[num4];
                    float num6 = (num4 > 0) ? (num5 / MathUtils.Sqrt((float)(num2 * num2 + num3 * num3))) : 0f;
                    float num7 = (float)num2 * num6;
                    float num8 = (float)num3 * num6;
                    float y = MathUtils.Lerp(600f, 60f, num5 * num5);
                    Vector3 vector = new Vector3(viewPosition.X + num7 * 1900f, y, viewPosition.Z + num8 * 1900f);
                    Vector2 texCoord = new Vector2(vector.X, vector.Z) / 1900f * 1.75f - v;
                    Color color = this.m_cloudsLayerColors[num4];
                    texturedBatch3D.TriangleVertices.Array[count2++] = new VertexPositionColorTexture(vector, color, texCoord);
                    if (j > 0 && i > 0)
                    {
                        ushort num9 = (ushort)(count + j + i * 7);
                        ushort num10 = (ushort)(count + (j - 1) + i * 7);
                        ushort num11 = (ushort)(count + (j - 1) + (i - 1) * 7);
                        ushort num12 = (ushort)(count + j + (i - 1) * 7);
                        if ((num2 <= 0 && num3 <= 0) || (num2 > 0 && num3 > 0))
                        {
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num9;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num10;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num11;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num11;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num12;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num9;
                        }
                        else
                        {
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num9;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num10;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num12;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num10;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num11;
                            texturedBatch3D.TriangleIndices.Array[count3++] = (int)num12;
                        }
                    }
                }
            }
            bool drawCloudsWireframe = this.DrawCloudsWireframe;
        }

        public new void DrawSkydome(Camera camera)
        {
            SubsystemSky.SkyDome skyDome;
            bool flag = !this.m_skyDomes.TryGetValue(camera.GameWidget, out skyDome);
            if (flag)
            {
                skyDome = new SubsystemSky.SkyDome();
                this.m_skyDomes.Add(camera.GameWidget, skyDome);
            }
            bool flag2 = skyDome.VertexBuffer == null || skyDome.IndexBuffer == null;
            if (flag2)
            {
                Utilities.Dispose<VertexBuffer>(ref skyDome.VertexBuffer);
                Utilities.Dispose<IndexBuffer>(ref skyDome.IndexBuffer);
                skyDome.VertexBuffer = new VertexBuffer(this.m_skyVertexDeclaration, skyDome.Vertices.Length);
                skyDome.IndexBuffer = new IndexBuffer(IndexFormat.SixteenBits, skyDome.Indices.Length);
                base.FillSkyIndexBuffer(skyDome);
                skyDome.LastUpdateTimeOfDay = null;
            }
            int x = Terrain.ToCell(camera.ViewPosition.X);
            int z = Terrain.ToCell(camera.ViewPosition.Z);
            float globalPrecipitationIntensity = this.m_subsystemWeather.GlobalPrecipitationIntensity;
            float timeOfDay = this.m_subsystemTimeOfDay.TimeOfDay;
            int temperature = this.m_subsystemTerrain.Terrain.GetTemperature(x, z);
            bool flag3;
            if (skyDome.LastUpdateTimeOfDay != null && MathUtils.Abs(timeOfDay - skyDome.LastUpdateTimeOfDay.Value) <= 0.001f && skyDome.LastUpdatePrecipitationIntensity != null && MathUtils.Abs(globalPrecipitationIntensity - skyDome.LastUpdatePrecipitationIntensity.Value) <= 0.02f && ((globalPrecipitationIntensity != 0f && globalPrecipitationIntensity != 1f) || skyDome.LastUpdatePrecipitationIntensity.Value == globalPrecipitationIntensity) && this.m_lightningStrikeBrightness == skyDome.LastUpdateLightningStrikeBrightness && skyDome.LastUpdateTemperature != null)
            {
                int num = temperature;
                int? lastUpdateTemperature = skyDome.LastUpdateTemperature;
                flag3 = !(num == lastUpdateTemperature.GetValueOrDefault() & lastUpdateTemperature != null);
            }
            else
            {
                flag3 = true;
            }
            bool flag4 = flag3;
            if (flag4)
            {
                skyDome.LastUpdateTimeOfDay = new float?(timeOfDay);
                skyDome.LastUpdatePrecipitationIntensity = new float?(globalPrecipitationIntensity);
                skyDome.LastUpdateLightningStrikeBrightness = this.m_lightningStrikeBrightness;
                skyDome.LastUpdateTemperature = new int?(temperature);
                this.FCFillSkyVertexBuffer(skyDome, timeOfDay, globalPrecipitationIntensity, temperature, camera);
            }
            Display.DepthStencilState = DepthStencilState.DepthRead;
            Display.RasterizerState = RasterizerState.CullNoneScissor;
            Display.BlendState = BlendState.Opaque;
            SubsystemSky.m_shaderFlat.Transforms.World[0] = Matrix.CreateTranslation(camera.ViewPosition) * camera.ViewProjectionMatrix;
            SubsystemSky.m_shaderFlat.Color = Vector4.One;
            Display.DrawIndexed(PrimitiveType.TriangleList, SubsystemSky.m_shaderFlat, skyDome.VertexBuffer, skyDome.IndexBuffer, 0, skyDome.IndexBuffer.IndicesCount);
        }
        public new void  DrawStars(Camera camera)
        {
            WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
            float num = (camera.ViewPosition.Y > 1000f) ? 0f : this.m_subsystemWeather.GlobalPrecipitationIntensity;
            float timeOfDay = this.m_subsystemTimeOfDay.TimeOfDay;
            bool flag = this.m_starsVertexBuffer == null || this.m_starsIndexBuffer == null;
            if (flag)
            {
                Utilities.Dispose<VertexBuffer>(ref this.m_starsVertexBuffer);
                Utilities.Dispose<IndexBuffer>(ref this.m_starsIndexBuffer);
                this.m_starsVertexBuffer = new VertexBuffer(this.m_starsVertexDeclaration, 600);
                this.m_starsIndexBuffer = new IndexBuffer(IndexFormat.SixteenBits, 900);
                base.FillStarsBuffers();
            }
            Display.DepthStencilState = DepthStencilState.DepthRead;
            Display.RasterizerState = RasterizerState.CullNoneScissor;
            if (worldType == WorldType.Default)//如果是主世界（地球）
			{
                float num2 = MathUtils.Sqr((1f - this.FCCalculateLightIntensity(timeOfDay, camera)) * (1f - num));
                bool flag2 = num2 > 0.01f;
                if (flag2)
                {
                    Display.BlendState = BlendState.Additive;
                    SubsystemSky.m_shaderTextured.Transforms.World[0] = Matrix.CreateRotationZ(-2f * timeOfDay * 3.1415927f) * Matrix.CreateTranslation(camera.ViewPosition) * camera.ViewProjectionMatrix;
                    SubsystemSky.m_shaderTextured.Color = new Vector4(1f, 1f, 1f, num2);
                    SubsystemSky.m_shaderTextured.Texture = ContentManager.Get<Texture2D>("Textures/Star");
                    SubsystemSky.m_shaderTextured.SamplerState = SamplerState.LinearClamp;
                    Display.DrawIndexed(PrimitiveType.TriangleList, SubsystemSky.m_shaderTextured, this.m_starsVertexBuffer, this.m_starsIndexBuffer, 0, this.m_starsIndexBuffer.IndicesCount);
                }
            }
            else if (worldType == WorldType.Moon || worldType == WorldType.StationMoon)//如果是宇宙空间站或者无大气层星球，
			{
                Display.BlendState = BlendState.Additive;
                SubsystemSky.m_shaderTextured.Transforms.World[0] = Matrix.CreateRotationZ(-2f * timeOfDay * 3.1415927f) * Matrix.CreateTranslation(camera.ViewPosition) * camera.ViewProjectionMatrix;
                SubsystemSky.m_shaderTextured.Color = new Vector4(1f, 1f, 1f, 255f);
                SubsystemSky.m_shaderTextured.Texture = ContentManager.Get<Texture2D>("Textures/Star");
                SubsystemSky.m_shaderTextured.SamplerState = SamplerState.LinearClamp;
                Display.DrawIndexed(PrimitiveType.TriangleList, SubsystemSky.m_shaderTextured, this.m_starsVertexBuffer, this.m_starsIndexBuffer, 0, this.m_starsIndexBuffer.IndicesCount);
            }
			else//默认
			{
                float num2 = MathUtils.Sqr((1f - this.FCCalculateLightIntensity(timeOfDay, camera)) * (1f - num));
                bool flag2 = num2 > 0.01f;
                if (flag2)
                {
                    Display.BlendState = BlendState.Additive;
                    SubsystemSky.m_shaderTextured.Transforms.World[0] = Matrix.CreateRotationZ(-2f * timeOfDay * 3.1415927f) * Matrix.CreateTranslation(camera.ViewPosition) * camera.ViewProjectionMatrix;
                    SubsystemSky.m_shaderTextured.Color = new Vector4(1f, 1f, 1f, num2);
                    SubsystemSky.m_shaderTextured.Texture = ContentManager.Get<Texture2D>("Textures/Star");
                    SubsystemSky.m_shaderTextured.SamplerState = SamplerState.LinearClamp;
                    Display.DrawIndexed(PrimitiveType.TriangleList, SubsystemSky.m_shaderTextured, this.m_starsVertexBuffer, this.m_starsIndexBuffer, 0, this.m_starsIndexBuffer.IndicesCount);
                }
            }


        }
    }
    #endregion


    #region 樱花粒子效果
    public class BlossomParticleSystem : ParticleSystem<Particle>
	{
		
		public class Particle1 : Particle
		{
		
			public float Time;
			public Vector3 Velocity; //y轴
			public float TimeToLive; //持续时间
		}

		
		private float m_size;
		public float m_timeToLive;
		public Vector3 m_position;
		public Random m_random = new Random();
		public float Age { get; set; }
		public BlossomParticleSystem(SubsystemTerrain terrain, Vector3 position, float size, float time, string Path, int TextureCount)
			: base(1)
		{
			
			m_size = size;    //粒子尺寸
			m_position = position; //初始位置
			m_timeToLive = time;

			Texture = ContentManager.Get<Texture2D>(Path);
			int num = Terrain.ToCell(position.X);
			int num2 = Terrain.ToCell(position.Y);
			int num3 = Terrain.ToCell(position.Z);
			int x = 0;
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num + 1, num2, num3));
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num - 1, num2, num3));
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num, num2 + 1, num3));
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num, num2 - 1, num3));
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num, num2, num3 + 1));
			x = MathUtils.Max(x, terrain.Terrain.GetCellLight(num, num2, num3 - 1));
			TextureSlotsCount = TextureCount;
			Color white = Color.White;
			float num4 = LightingManager.LightIntensityByLightValue[x];
			white *= num4;
			white.A = 255;
			for (int i = 0; i < Particles.Length; i++)
			{
                Particle1 obj =(Particle1)Particles[i];
				obj.IsActive = true;
				obj.Position = position + 0.4f * size * new Vector3(m_random.Float(-1f, 1f), m_random.Float(-1f, 1f), m_random.Float(-1f, 1f));
				obj.Color = white;
				obj.Size = new Vector2(0.3f * size);
				obj.TimeToLive = m_timeToLive;
				obj.Velocity = 1.2f * size * new Vector3(m_random.Float(-1f, 1f), m_random.Float(-1f, 1f), m_random.Float(-1f, 1f));
				obj.FlipX = m_random.Bool();
				obj.FlipY = m_random.Bool();
				
			}
		}

		public override bool Simulate(float dt)
		{
			dt = MathUtils.Clamp(dt, 0f, 0.1f);
			float num = MathUtils.Pow(0.1f, dt);
			float driftStrength = 0.085f; // 飘动强度，用于控制粒子飘动的幅度
			//float driftSpeed = 0.1f; //飘速度
			bool flag = false;
			
			


			for (int i = 0; i < Particles.Length; i++)
			{
                Particle1 particle = (Particle1)Particles[i];
				if (particle.IsActive)
				{
					flag = true;
					particle.TimeToLive -= dt;
					if (particle.TimeToLive > 0f)
					{


						Vector3 vector = particle.Position += particle.Velocity * dt;
						particle.Velocity.Y -= MathUtils.Tan(3.5f * dt);
						particle.Velocity.X -= (MathUtils.Sin(1f * MathUtils.PI * dt) + m_random.Float(-1f, 1f)) * driftStrength;
						particle.Velocity.Z -= (MathUtils.Sin(1f * MathUtils.PI * dt) + m_random.Float(-1f, 1f)) * driftStrength;
						particle.Velocity *= num;
						
						

					}
					else
					{
						
						particle.Position = m_position + 0.4f * m_size * new Vector3(m_random.Float(-1f, 1f), m_random.Float(-1f, 1f), m_random.Float(-1f, 1f)); // 复位
						particle.TimeToLive = m_timeToLive;
						
					}
				}
			}

			return !flag;
		}


	}


	#endregion


	#region 新粒子子系统，方块追踪
	/*public class FCSubsystemPlantataBlockBehavior : SubsystemBlockBehavior, IUpdateable
	{
		
		Random randomfc = new Random();
		public SubsystemTerrain subsystemTerrain;
		public SubsystemTime m_subsystemTime;
		public SubsystemParticles m_subsystemParticles;
		public SubsystemTerrain m_subsystemTerrain;
		public Terrain m_Terrain;
		public Dictionary<Point3, Vector3>.KeyCollection PlantData => Plantdatas.Keys;
		public Dictionary<Point3, Vector3> Plantdatas = new Dictionary<Point3, Vector3>();
		public List<Point3> m_toReduce = new List<Point3>();
		public Random m_random = new Random();
		public int m_updateIndex;
		public UpdateOrder UpdateOrder => UpdateOrder.Default;
		public override int[] HandledBlocks => new int[1]
		{
			948,//樱花树叶的ID
		};
		public void Update(float dt)
		{
			if (m_subsystemTime.PeriodicGameTimeEvent(30.0, 0.0))//每60秒遍历一次  少了会卡跟粒子的时间差不多就行
			{
				m_updateIndex++;
				foreach (Point3 key in Plantdatas.Keys)
				{
					m_toReduce.Add(key);

				}
				m_toReduce.Clear();
			}




		}
		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			//AddPlantData(value, x, y, z);
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			RemovePlantData(x, y, z);
			if (Terrain.ExtractContents(value) == 948)
			{
				this.RemovePlantData(x, y, z);
			}
		}


		public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
		{
			//AddPlantData(value, x, y, z);

			

		}

		public override void OnChunkDiscarding(TerrainChunk chunk)
		{
			var list = new List<Point3>();
			foreach (Point3 key in Plantdatas.Keys)
			{
				if (key.X >= chunk.Origin.X && key.X < chunk.Origin.X + 16 && key.Z >= chunk.Origin.Y && key.Z < chunk.Origin.Y + 16)
				{
					list.Add(key);
				}
			}
			foreach (Point3 point2 in list)
			{
				this.RemovePlantData(point2.X, point2.Y, point2.Z);
			}
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			m_subsystemTime = Project.FindSubsystem<SubsystemTime>(throwOnError: true);
			subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
			m_subsystemParticles = Project.FindSubsystem<SubsystemParticles>(throwOnError: true);
			m_subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);

		}

		public void AddPlantData(int value, int x, int y, int z)
		{
			
		
			var v = new Vector3(0.5f, 0f, 0.5f);
			var position = new Point3(x, y, z);
			Plantdatas[new Point3(x, y, z)] = new Vector3(x, y, z) + v;
			if (randomfc.Bool(0.2f)&& (Terrain.ExtractContents(SubsystemTerrain.Terrain.GetCellValue(x, y - 1, z)) == 0))
            {
				var cherryParticleSystem = new BlossomParticleSystem(m_subsystemTerrain, new Vector3(x, y - 1, z) + v, 1f, 10.5f, "Textures/YHParticle", 3);
				ParticleSystems[position] = cherryParticleSystem;
				m_subsystemParticles.AddParticleSystem(cherryParticleSystem);
			}
				
			

			


		}
		public void RemovePlantData(int x, int y, int z)
		{
			Point3 position = new Point3(x, y, z);
			if (ParticleSystems.TryGetValue(position, out BlossomParticleSystem particleSystem))
			{
				m_subsystemParticles.RemoveParticleSystem(particleSystem);
				ParticleSystems.Remove(position);
			}
		}
		public Dictionary<Point3, BlossomParticleSystem> ParticleSystems = new Dictionary<Point3, BlossomParticleSystem>();
	}*/
	#endregion

	#region 树叶地毯系统
	public class YHcarpetBlock : FCSixFaceBlock  //地毯类模板
	{

		public YHcarpetBlock()
		   : base("Textures/FCBlocks/yhcarpet", Color.White)
		{
		}



		public Random m_random = new Random();
		

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			
			
			generator.GenerateCubeVertices(this, value, x, y, z, 0.0625f, 0.0625f, 0.0625f, 0.0625f, m_color, m_color, m_color, m_color, m_color, -1, geometry.GetGeometry(m_texture).AlphaTestSubsetsByFace);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			
			BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size, 0.0625f * size, size), ref matrix, m_color, m_color, environmentData, m_texture);
		}

		

		public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
		{

			showDebris = true;

			if (this.m_random.Bool(0.5f))
			{
				dropValues.Add(new BlockDropValue
				{
					Value = 964,
					Count = 4
				});
				showDebris = true;
				return;
			}
			base.GetDropValues(subsystemTerrain, oldValue, newValue, toolLevel, dropValues, out showDebris);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(4, value), m_texture);
		}



		public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
		{
			return this.m_collisionBoxes;
		}


		

		public const int Index = 963;

		public BoundingBox[] m_collisionBoxes = new BoundingBox[] //碰撞箱
		{
			new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(1f, 0.0625f, 1f))
		};
	}

	public class CherryflowerBlock : FCTwoDBlock
	{
		public override int GetTextureSlotCount(int value)
		{
			return 1;
		}
		public override int GetFaceTextureSlot(int face, int value)
		{
			if (face == -1) return 0;
			return DefaultTextureSlot;
		}

		public CherryflowerBlock()
		   : base("Textures/amod/yinghua")
		{
		}

		public const int Index = 964;
	}

	#endregion

	#region 樱花地毯生成子系统
	public class SubsystemYHleaveBlockBehavior : SubsystemPollableBlockBehavior, IUpdateable
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					963,
					948
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

		

		public override void OnPoll(int value, int x, int y, int z, int pollPass)
		{
			if (this.m_subsystemGameInfo.WorldSettings.EnvironmentBehaviorMode != EnvironmentBehaviorMode.Living || y <= 0 || y >= 255)
			{
				return;
			}
			int num = Terrain.ExtractContents(value);
			Block block = BlocksManager.Blocks[num];
			if (num == 948)
			{
				GrowYHCarpet(value, x, y, z, pollPass);
				return;
			}
				
		}
		public void GrowYHCarpet(int value, int x, int y, int z, int pollPass)
		{
			
			int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 1, z);//第一个
			int cellValue1 = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 14, z);//第四格
			int cellValue2 = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 13, z);//第三格子
			int cellValue3 = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 15, z);//第五格
			int cellValue4 = base.SubsystemTerrain.Terrain.GetCellValue(x, y - 12, z);//第五格

			if (cellValue != 0) // 检查樱花树叶下方块是否为空
			{
				return; // 如果不为空，则不生成樱花地毯
			}

			else if ((Terrain.ExtractContents(cellValue1) == 8|| Terrain.ExtractContents(cellValue1) == 2 || Terrain.ExtractContents(cellValue1) == 3 || Terrain.ExtractContents(cellValue1) == 4 || Terrain.ExtractContents(cellValue1) == 5 || Terrain.ExtractContents(cellValue1) == 6 || Terrain.ExtractContents(cellValue1) == 7 || Terrain.ExtractContents(cellValue1) == 66 || Terrain.ExtractContents(cellValue1) == 67) && (Terrain.ExtractContents(cellValue2) == 0) && (Terrain.ExtractContents(cellValue2) != 963)&&m_random.Bool(0.8f)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
                this.m_toUpdate[new Point3(x, y -13 , z)] = Terrain.MakeBlockValue(carpetValue);
               // base.SubsystemTerrain.ChangeCell(x, y - 13, z, carpetValue, true);
			}
			else if ((Terrain.ExtractContents(cellValue3) == 8|| Terrain.ExtractContents(cellValue3) == 2 || Terrain.ExtractContents(cellValue3) == 3 || Terrain.ExtractContents(cellValue3) == 4 || Terrain.ExtractContents(cellValue3) == 5 || Terrain.ExtractContents(cellValue3) == 6 || Terrain.ExtractContents(cellValue3) == 7 || Terrain.ExtractContents(cellValue3) == 66 || Terrain.ExtractContents(cellValue3) == 67) && (Terrain.ExtractContents(cellValue1) == 0) && (Terrain.ExtractContents(cellValue1) != 963) && m_random.Bool(0.7f)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
                this.m_toUpdate[new Point3(x, y - 14, z)] = Terrain.MakeBlockValue(carpetValue);
                //base.SubsystemTerrain.ChangeCell(x, y - 14, z, carpetValue, true);
			}
			else if ((Terrain.ExtractContents(cellValue2) == 8 || Terrain.ExtractContents(cellValue2) == 2 || Terrain.ExtractContents(cellValue2) == 3 || Terrain.ExtractContents(cellValue2) == 4 || Terrain.ExtractContents(cellValue2) == 5 || Terrain.ExtractContents(cellValue2) == 6 || Terrain.ExtractContents(cellValue2) == 7 || Terrain.ExtractContents(cellValue2) == 66 || Terrain.ExtractContents(cellValue2) == 67) && (Terrain.ExtractContents(cellValue4) == 0) && (Terrain.ExtractContents(cellValue4) != 963) && m_random.Bool(0.9f)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
                this.m_toUpdate[new Point3(x, y - 12, z)] = Terrain.MakeBlockValue(carpetValue);
                
			}

		}


		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemTime = base.Project.FindSubsystem<SubsystemTime>(true);
			this.m_subsystemGameInfo = base.Project.FindSubsystem<SubsystemGameInfo>(true);
		}

		public void Update(float dt)
		{
			if (this.m_subsystemTime.PeriodicGameTimeEvent(30.0, 0.0))
			{
                foreach (KeyValuePair<Point3, int> keyValuePair in this.m_toUpdate)
                {
                    if (base.SubsystemTerrain.Terrain.GetCellContents(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z) == 0)
                    {
                        base.SubsystemTerrain.ChangeCell(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z, keyValuePair.Value, true);
                    }
                }
                this.m_toUpdate.Clear();
            }
		}

		

		

		

		

		
		

		public SubsystemTime m_subsystemTime;

		public SubsystemGameInfo m_subsystemGameInfo;

		public Random m_random = new Random();

        //public Dictionary<Point3, SubsystemPlantBlockBehavior.Replacement> m_toReplace = new Dictionary<Point3, SubsystemPlantBlockBehavior.Replacement>();
        public Dictionary<Point3, int> m_toUpdate = new Dictionary<Point3, int>();
        public struct Replacement
		{
			public int RequiredValue;

			public int Value;
		}
	}



    #endregion
    #endregion

    #region 区块（工业）

    #region 区块探针藤曼
    public class Dongtai : Block
	  {
		public ComponentGui m_com;

		   
		public const int Index = 973;
		public List<Texture2D> textures = new System.Collections.Generic.List<Texture2D>();
		public override void Initialize()
		{
			ReadOnlyList<ContentInfo> contentInfos = ContentManager.List("DT/images");
			foreach (var c in contentInfos)
			{
				textures.Add(ContentManager.Get<Texture2D>(c.ContentPath));
			}

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
		int textureIndex1 = 0;
		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			    
			textureIndex1 = (textureIndex1 + 1) % textures.Count;
			generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(textures[textureIndex1]).SubsetAlphaTest);
			

		}

		
		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size, ref matrix, textures[Time.FrameIndex/10 % textures.Count], Color.White, false, environmentData);
		}
	  }
	#endregion

	#region 模型类2d物品特殊值
	public abstract class FCPlatBlock : FlatBlock
	{
		public override void Initialize()
		{
			Model model = ContentManager.Get<Model>("Models/Plate", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Plate", true).ParentBone);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Plate", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.1f, 0f) * Matrix.CreateRotationX(1.5707f), false, false, true, false, Color.White);
			base.Initialize();
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}
	#endregion
	#region 特殊值区。fc物品972
	public class FCItemBlock : FCPlatBlock
    {
		public const int Index = 972;
		private Texture2D[] m_texture = new Texture2D[20];
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
			for (int i = 0; i < 20; i++)
			{
				this.m_texture[i] = ContentManager.Get<Texture2D>(FCItemBlock.m_textureNames[i], null);
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
			float num2 = (num >= 0 && num < FCItemBlock.m_sizes.Length) ? (size * FCItemBlock.m_sizes[num]) : size;
			/*switch (num)
			{
				case 14:
					BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, this.m_frostPowerTexture[Time.FrameIndex / 12 % 7], color, 1.5f * num2, ref matrix, environmentData);
					return;
				
			}*/
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, this.m_texture[num], color, 2f * num2, ref matrix, environmentData);
		}

		public override float GetFuelHeatLevel(int value)
		{
			int num = Terrain.ExtractData(value);
			switch (num)
			{
				case 7:
					return 1000f;
					
			}
			return this.FuelHeatLevel;
		}

		public override float GetFuelFireDuration(int value)
		{
			return this.firetime[Terrain.ExtractData(value)];
		}

		private float[] firetime = new float[]
		{
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			20f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,

		};

		private static float[] m_sizes = new float[]
		{
			0.7f,
			0.7f,
			0.7f,
			0.6f,
			0.5f,
			0.5f,
			0.5f,
			0.5f,
			0.5f,
			1f,
			0.5f,
			0.5f,
			0.5f,
			1f,
			0.5f, //传动活塞
			0.6f, //钢锭
			1f,
			1f,
			1f,
			0.5f,
		};

		public override IEnumerable<int> GetCreativeValues()
		{
			foreach (int data in EnumUtils.GetEnumValues(typeof(FCItemBlock.ItemType)))
			{
				yield return Terrain.MakeBlockValue(972, 0, data);
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
			100,
		};
		//类名字
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			int num = Terrain.ExtractData(value);
			if (num < 0 || num >= FCItemBlock.m_displayNames.Length)
			{
				return string.Empty;
			}
			return FCItemBlock.m_displayNames[num];
		}

		private static string[] m_displayNames = new string[]
		{
			"铜板Copper plate",//0
			"铁板iron plate",//1
			"钢板steel plate",//2
			"铜线圈copper coil",//3
			"硅silicon",//4
			"线路板circuit board",//5
			"集成芯片integrated chip",//6
			"酵素粉enzyme powder",//7
			"硅板silicon plate",//8
			"精制淀粉refined starch",//9
			"偏导芯片bias chip",//10
			"齿轮gear",//11
			"钢棒steel rod",//12
			"酵母菌yeast",//13
			"活塞universal piston",//14
			"钢锭steelIngot",//15
			"碳粉",//16
			"铁粉",//17
			"磁化铁棒",//18
			"基础马达",//19


		};

		private static string[] m_textureNames = new string[]
		{
			"Textures/FCgongye/tongban",
			"Textures/FCgongye/tieban",
			"Textures/FCgongye/gangban",
			"Textures/FCgongye/tongxianquan",
			"Textures/FCgongye/gui",
			"Textures/FCgongye/xianluban",
			"Textures/FCgongye/ICU",
			"Textures/FCgongye/jiaosufeng",
			"Textures/FCgongye/guiban",
			"Textures/FCgongye/DVfeng",
			"Textures/FCgongye/PDU",
			"Textures/FCgongye/chilun",//齿轮
			"Textures/FCgongye/gangbang",
			"Textures/FCgongye/jiaomujun",
			"Textures/FCgongye/huosai",//活塞
			"Textures/FCgongye/gangding",
			"Textures/FCgongye/tanfeng",
			"Textures/FCgongye/tiefeng",
			"Textures/FCgongye/cihuatiebang",
			"Textures/FCgongye/mada",

		};
		//动态材质
		private static string[] m_frostPower = new string[]
		{
			
			
		




		};

		private Texture2D[] m_frostPowerTexture = new Texture2D[7];

		public enum ItemType
		{
			Tongban, //1铜板
			Tieban, //2铁板
			gangban,//3钢板
			Tongxianquan,//铜线圈4
			SI,//5硅
			Xianluban,//6线路板
			ICU, //集成芯片7 
			jiaosufeng,//8酵素粉
			SIboard, //9 硅板
			DVfeng,//10精制淀粉
			Piandaoxingpian,//11偏导芯片
			chilun,//12齿轮
			gangbang,//13钢棒
			jiaomu,//酵母菌14
			Universalpiston,//15活塞
			gangding,//16钢锭
			tanfeng,//17碳粉
			tiefeng,//18铁粉
			cihuatiebang,//19磁化铁棒
			mada,//20马达

		}

		public override string GetDescription(int value)
		{
			int num = Terrain.ExtractData(value);
			if (num < 0 || num >= FCItemBlock.m_Description.Length)
			{
				return string.Empty;
			}
			return FCItemBlock.m_Description[num];
			
		}

		private static string[] m_Description = new string[]
		{
			"铜板Copper plate,用铜打造，用于合成工业物品（Made of copper, used for synthesizing industrial items）",
			"铁板iron plate，用于合成工业物品（ used for synthesizing industrial items）",
			"钢板steel plate 用于合成工业物品（ used for synthesizing industrial items）",
			"铜线圈copper coil由铜合成，用于合成工业物品",
			"硅silicon由沙子用高炉烧成高纯度硅，用于合成工业物品",
			"线路板circuit board",
			"集成芯片integrated chip",
			"酵素粉enzyme powder",
			"硅板silicon plate",
			"精制淀粉refined starch由研磨机磨制，可以和酵母菌合成酵素粉",
			"偏导芯片bias chip",
			"齿轮gear",
			"钢棒steel rod",
			"酵母菌yeast可以和精制淀粉合成酵素粉发酵，由微观提取机器提取",
			"通用活塞universal piston",
			"钢锭steelIngot",
			"碳粉Carbon powder",
			"铁粉Iron powder",
			"磁化铁棒Magnetized iron rod",
			"基础马达，用于制造各种机器。",

		};
	}

	#endregion

	#region 无缝玻璃
	public class FCGlassBlock : FCSixFaceBlock
	{
		public FCGlassBlock()
		   : base("Textures/FCBlocks/glass", Color.White)
		{
		}
		public override bool ShouldGenerateFace(SubsystemTerrain subsystemTerrain, int face, int value, int neighborValue)
		{
			return Terrain.ExtractContents(neighborValue) != 974 && base.ShouldGenerateFace(subsystemTerrain, face, value, neighborValue);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, Color.White, geometry.GetGeometry(m_texture).AlphaTestSubsetsByFace);
		}

		public const int Index = 974;
	}
	#endregion

	#region 发酵桶
	public class FajiaotongBlock2 : FajiaotongBlock //正在发酵的发酵桶
	{
		public new const int Index = 971;
	}
	public class FajiaotongBlock : Block  //发酵桶方块
	{
		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

		public BlockMesh[] m_blockMeshesByData = new BlockMesh[4];

		private Texture2D m_texture;

		public override void Initialize()
		{
			Model model = ContentManager.Get<Model>("Models/Furnace1", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Furnace", true).ParentBone);
			for (int i = 0; i < 4; i++)
			{
				this.m_blockMeshesByData[i] = new BlockMesh();
				Matrix matrix = Matrix.Identity;
				matrix *= Matrix.CreateRotationY((float)i * 3.1415927f / 2f) * Matrix.CreateTranslation(0.5f, 0f, 0.5f);
				this.m_blockMeshesByData[i].AppendModelMeshPart(model.FindMesh("Furnace", true).MeshParts[0], boneAbsoluteTransform * matrix, false, false, false, false, Color.White);
			}
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Furnace", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.5f, 0f), false, false, false, false, Color.White);
			m_texture = ContentManager.Get<Texture2D>("Textures/FCBlocks/fajiaotong", null);  //外置材质
			base.Initialize();
		}

		public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
		{
			return false;
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			int num = Terrain.ExtractData(value);
			if (num < this.m_blockMeshesByData.Length)
			{
				generator.GenerateShadedMeshVertices(this, x, y, z, this.m_blockMeshesByData[num], Color.White, null, null, geometry.GetGeometry(m_texture).SubsetAlphaTest);
			}
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, size, ref matrix, environmentData);
		}

		public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
		{
			Vector3 forward = Matrix.CreateFromQuaternion(componentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation).Forward;
			float num = Vector3.Dot(forward, Vector3.UnitZ);
			float num2 = Vector3.Dot(forward, Vector3.UnitX);
			float num3 = Vector3.Dot(forward, -Vector3.UnitZ);
			float num4 = Vector3.Dot(forward, -Vector3.UnitX);
			int data = 0;
			if (num == MathUtils.Max(num, num2, num3, num4))
			{
				data = 2;
			}
			else if (num2 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 3;
			}
			else if (num3 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 0;
			}
			else if (num4 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 1;
			}
			return new BlockPlacementData
			{
				Value = Terrain.ReplaceData(Terrain.ReplaceContents(0, 970), data),
				CellFace = raycastResult.CellFace
			};
		}

		public const int Index = 970;
	}

	#region 发酵桶子系统
	public class FCSubsystemFurnaceBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					970, //64
					971  //65
				};
			}
		}

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(oldValue) != 970 && Terrain.ExtractContents(oldValue) != 971)
			{
				DatabaseObject databaseObject = base.SubsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("FajiaotongBlock", base.SubsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
				ValuesDictionary valuesDictionary = new ValuesDictionary();
				valuesDictionary.PopulateFromDatabaseObject(databaseObject);
				valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x, y, z));
				Entity entity = base.SubsystemTerrain.Project.CreateEntity(valuesDictionary);
				base.SubsystemTerrain.Project.AddEntity(entity);
			}
			if (Terrain.ExtractContents(value) == 971) //如果是发光发酵桶
			{
				this.AddFire(value, x, y, z);
			}
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(newValue) != 970 && Terrain.ExtractContents(newValue) != 971)
			{
				ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(x, y, z);
				if (blockEntity != null)
				{
					Vector3 position = new Vector3((float)x, (float)y, (float)z) + new Vector3(0.5f);
					foreach (IInventory inventory in blockEntity.Entity.FindComponents<IInventory>())
					{
						inventory.DropAllItems(position);
					}
					base.SubsystemTerrain.Project.RemoveEntity(blockEntity.Entity, true);
				}
			}
			if (Terrain.ExtractContents(value) == 971)
			{
				this.RemoveFire(x, y, z);
			}
		}

		public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
		{
			if (Terrain.ExtractContents(value) == 971)
			{
				this.AddFire(value, x, y, z);
			}
		}

		public override void OnChunkDiscarding(TerrainChunk chunk) //区块卸载时，移除粒子效果
		{
			List<Point3> list = new List<Point3>();
			foreach (Point3 point in this.m_particleSystemsByCell.Keys)
			{
				if (point.X >= chunk.Origin.X && point.X < chunk.Origin.X + 16 && point.Z >= chunk.Origin.Y && point.Z < chunk.Origin.Y + 16)
				{
					list.Add(point);
				}
			}
			foreach (Point3 point2 in list)
			{
				this.RemoveFire(point2.X, point2.Y, point2.Z);
			}
		}

		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				FCComponentFajiaotong componentFajiaotong = blockEntity.Entity.FindComponent<FCComponentFajiaotong>(true);
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new FajiaotongWidget(componentMiner.Inventory, componentFajiaotong);
				AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
				return true;
			}
			return false;
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			base.OnNeighborBlockChanged(x, y, z, neighborX, neighborY, neighborZ);
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
		}

		public void AddFire(int value, int x, int y, int z)
		{
			Vector3 v = new Vector3(0.5f, 0.2f, 0.5f);
			float size = 0.15f;
			FireParticleSystem fireParticleSystem = new FireParticleSystem(new Vector3((float)x, (float)y, (float)z) + v, size, 16f);
			this.m_subsystemParticles.AddParticleSystem(fireParticleSystem);
			this.m_particleSystemsByCell[new Point3(x, y, z)] = fireParticleSystem;
		}

		public void RemoveFire(int x, int y, int z)
		{
			Point3 key = new Point3(x, y, z);
			FireParticleSystem particleSystem = this.m_particleSystemsByCell[key];
			this.m_subsystemParticles.RemoveParticleSystem(particleSystem);
			this.m_particleSystemsByCell.Remove(key);
		}

		public SubsystemParticles m_subsystemParticles;

		public Dictionary<Point3, FireParticleSystem> m_particleSystemsByCell = new Dictionary<Point3, FireParticleSystem>();
	}
	#endregion

	#region 发酵桶组件
	public class FCComponentFajiaotong : ComponentInventoryBase, IUpdateable
	{
		public int RemainsSlotIndex
		{
			get
			{
				return this.SlotsCount - 1;
			}
		}

		public int ResultSlotIndex
		{
			get
			{
				return this.SlotsCount - 2;
			}
		}

		public int FuelSlotIndex
		{
			get
			{
				return this.SlotsCount - 3;
			}
		}

		public float HeatLevel
		{
			get
			{
				return this.m_heatLevel;
			}
		}

		public float SmeltingProgress
		{
			get
			{
				return this.m_smeltingProgress;
			}
		}

		public UpdateOrder UpdateOrder
		{
			get
			{
				return UpdateOrder.Default;
			}
		}

		public override int GetSlotCapacity(int slotIndex, int value)
		{
			if (slotIndex != this.FuelSlotIndex)
			{
				return base.GetSlotCapacity(slotIndex, value);
			}
			if (BlocksManager.Blocks[Terrain.ExtractContents(value)].GetFuelHeatLevel(value) > 0f)
			{
				return base.GetSlotCapacity(slotIndex, value);
			}
			return 0;
		}

		public override void AddSlotItems(int slotIndex, int value, int count)
		{
			this.m_updateSmeltingRecipe = true;
			base.AddSlotItems(slotIndex, value, count);
		}

		public override int RemoveSlotItems(int slotIndex, int count)
		{
			this.m_updateSmeltingRecipe = true;
			return base.RemoveSlotItems(slotIndex, count);
		}

		public void Update(float dt)
		{
			Point3 coordinates = this.m_componentBlockEntity.Coordinates;  //获取熔炉所在方块实体的坐标。

			if (this.m_heatLevel == 1000f)//如果熔炉的热量级别=1000，开始烧

			{
				this.m_fireTimeRemaining = MathUtils.Max(0f, this.m_fireTimeRemaining - dt); //那么减少火焰剩余时间（m_fireTimeRemaining）的值，确保其不小于0。
				if (this.m_fireTimeRemaining == 0f)
				{
					this.m_heatLevel = 0f;  //如果火焰剩余时间为0，则将熔炉的热量级别（m_heatLevel）设置为0。
				}
			}
			if (this.m_updateSmeltingRecipe)  //如果需要更新熔炼配方（m_updateSmeltingRecipe为true），则进行以下操作：
			{
				this.m_updateSmeltingRecipe = false; //获取燃料槽（FuelSlotIndex）的物品热量级别（heatLevel）。
				float heatLevel = 0f;
				if (this.m_heatLevel > 0f)       //如果熔炉的热量级别大于0，则使用当前热量级别作为heatLevel。
				{
					heatLevel = this.m_heatLevel;
				}
				else
				{
					ComponentInventoryBase.Slot slot = this.m_slots[this.FuelSlotIndex];     //否则，获取燃料槽中物品的热量级别，并将其赋值给heatLevel。
					if (slot.Count > 0)
					{
						int num = Terrain.ExtractContents(slot.Value);
						heatLevel = BlocksManager.Blocks[num].GetFuelHeatLevel(slot.Value);
					}
				}
				CraftingRecipe craftingRecipe = this.FindFCSmeltingRecipe(heatLevel);    //查找匹配heatLevel的熔炼配方（FindSmeltingRecipe方法），并将其设置为当前的熔炼配方（m_smeltingRecipe）。
				if (craftingRecipe != this.m_smeltingRecipe)
				{
					this.m_smeltingRecipe = ((craftingRecipe != null && craftingRecipe.ResultValue != 0) ? craftingRecipe : null);
					this.m_smeltingProgress = 0f;  //如果找到了新的熔炼配方，将熔炼进度（m_smeltingProgress）重置为0。
				}
			}
			if (this.m_smeltingRecipe == null)   //如果当前没有熔炼配方（m_smeltingRecipe为null），则将熔炉的热量级别和火焰剩余时间都设置为0。
			{
				this.m_heatLevel = 0f;
				this.m_fireTimeRemaining = 0f;
			}
			if (this.m_smeltingRecipe != null && this.m_fireTimeRemaining <= 0f)   //如果当前有熔炼配方，并且火焰剩余时间小于等于0，则进行以下操作：
			{
				ComponentInventoryBase.Slot slot2 = this.m_slots[this.FuelSlotIndex];    //1.获取燃料槽（FuelSlotIndex）的物品热量级别。
				if (slot2.Count > 0)
				{
					int num2 = Terrain.ExtractContents(slot2.Value);   //2.如果燃料物品的爆炸压力大于0，则将燃料槽的物品数量设置为0，并尝试引爆相应方块。
					Block block = BlocksManager.Blocks[num2];
					if (block.GetExplosionPressure(slot2.Value) > 0f)
					{
						slot2.Count = 0;
						this.m_subsystemExplosions.TryExplodeBlock(coordinates.X, coordinates.Y, coordinates.Z, slot2.Value);
					}
					else if (block.GetFuelHeatLevel(slot2.Value) > 0f)   //3.否则，如果燃料物品的热量级别大于0，则减少燃料槽的物品数量，并根据燃料物品的热量级别设置火焰剩余时间和熔炉的热量级别。
					{
						slot2.Count--;
						this.m_fireTimeRemaining = block.GetFuelFireDuration(slot2.Value);
						this.m_heatLevel = block.GetFuelHeatLevel(slot2.Value);
					}
				}
			}
			if (this.m_fireTimeRemaining <= 0f)    //如果火焰剩余时间小于等于0，则将当前的熔炼配方和熔炼进度重置为null和0。
			{
				this.m_smeltingRecipe = null;
				this.m_smeltingProgress = 0f;
			}
			if (this.m_smeltingRecipe != null) //如果当前有熔炼配方，则逐渐增加熔炼进度，直到达到1。
			{
				this.m_smeltingProgress = MathUtils.Min(this.m_smeltingProgress + 0.02f * dt, 1f);
				if (this.m_smeltingProgress >= 1f)                //当熔炼进度达到1时，进行以下操作：
				{
					for (int i = 0; i < this.m_furnaceSize; i++)  //1.遍历熔炉中的槽，减少非空槽的物品数量。
					{
						if (this.m_slots[i].Count > 0)
						{
							this.m_slots[i].Count--;
						}
					}   //2.将熔炉中结果物品槽（ResultSlotIndex）的值设置为熔炼配方的结果物品值，并增加对应的数量。
					this.m_slots[this.ResultSlotIndex].Value = this.m_smeltingRecipe.ResultValue;
					this.m_slots[this.ResultSlotIndex].Count += this.m_smeltingRecipe.ResultCount;
					if (this.m_smeltingRecipe.RemainsValue != 0 && this.m_smeltingRecipe.RemainsCount > 0)//3.如果熔炼配方还有剩余物品，将熔炉中剩余物品槽（RemainsSlotIndex）的值设置为剩余物品的值，并增加对应的数量。
					{
						this.m_slots[this.RemainsSlotIndex].Value = this.m_smeltingRecipe.RemainsValue;
						this.m_slots[this.RemainsSlotIndex].Count += this.m_smeltingRecipe.RemainsCount;
					}
					this.m_smeltingRecipe = null;
					this.m_smeltingProgress = 0f;    //4.将熔炼配方和熔炼进度重置为null和0，并设置需要更新熔炼配方的标志为true。
					this.m_updateSmeltingRecipe = true;
				}
			}//获取熔炉所在区块，并检查该区块是否处于有效状态。如果是有效状态，则根据熔炉的热量级别，将熔炉所在方块的内容值替换为对应的熔炉方块。
			TerrainChunk chunkAtCell = this.m_subsystemTerrain.Terrain.GetChunkAtCell(coordinates.X, coordinates.Z);
			if (chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid)
			{
				int cellValue = this.m_subsystemTerrain.Terrain.GetCellValue(coordinates.X, coordinates.Y, coordinates.Z);
				this.m_subsystemTerrain.ChangeCell(coordinates.X, coordinates.Y, coordinates.Z, Terrain.ReplaceContents(cellValue, (this.m_heatLevel > 0f) ? 971 : 970), true);
			}
		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{


			base.Load(valuesDictionary, idToEntityMap);
			this.m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
			this.m_subsystemExplosions = base.Project.FindSubsystem<SubsystemExplosions>(true);
			this.m_componentBlockEntity = base.Entity.FindComponent<ComponentBlockEntity>(true);
			this.m_furnaceSize = this.SlotsCount - 3;
			if (this.m_furnaceSize < 1 || this.m_furnaceSize > 3)
			{
				throw new InvalidOperationException("Invalid furnace size.");
			}
			this.m_fireTimeRemaining = valuesDictionary.GetValue<float>("FireTimeRemaining");
			this.m_heatLevel = valuesDictionary.GetValue<float>("HeatLevel");
			this.m_updateSmeltingRecipe = true;





		}

		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{

			base.Save(valuesDictionary, entityToIdMap);
			valuesDictionary.SetValue<float>("FireTimeRemaining", this.m_fireTimeRemaining);
			valuesDictionary.SetValue<float>("HeatLevel", this.m_heatLevel);




		}

		public virtual CraftingRecipe FindFCSmeltingRecipe(float heatLevel)
		{
			if (heatLevel == 1000f)  //首先，检查热量级别是否大于0。如果不大于0，则直接返回null，表示没有匹配的熔炼配方。
			{
				for (int i = 0; i < this.m_furnaceSize; i++)    //如果热量级别大于0，那么对熔炉中的每个槽进行遍历，获取每个槽中的物品的内容值（slotValue）。
				{
					int slotValue = this.GetSlotValue(i);
					int num = Terrain.ExtractContents(slotValue);//根据内容值获取所在方块的类型（num）和数据值（num2）。
					int num2 = Terrain.ExtractData(slotValue);
					if (this.GetSlotCount(i) > 0)//如果该槽中有物品（GetSlotCount(i) > 0），则获取该物品所对应的方块类型（block）的配方ID，并将其和数据值拼接成一个字符串，存储在m_matchedIngredients数组中的对应位置。
					{
						Block block = BlocksManager.Blocks[num];
						this.m_matchedIngredients[i] = block.GetCraftingId(slotValue) + ":" + num2.ToString(CultureInfo.InvariantCulture);
					}
					else //如果该槽中没有物品，将对应位置的m_matchedIngredients数组元素设置为null。
					{
						this.m_matchedIngredients[i] = null;
					}
				}
				ComponentPlayer componentPlayer = base.FindInteractingPlayer(); //获取当前与熔炉交互的玩家（componentPlayer），并获取玩家的等级（playerLevel）。
																				//调用CraftingRecipesManager.FindMatchingRecipe方法，
																				//通过给定的热量级别、熔炼配方的材料和玩家的等级，查找匹配的熔炼配方（craftingRecipe）。
				float playerLevel = (componentPlayer != null) ? componentPlayer.PlayerData.Level : 1f;
				CraftingRecipe craftingRecipe = XCraftingRecipesManager.FindMatchingRecipe(this.m_subsystemTerrain, this.m_matchedIngredients, heatLevel, playerLevel);
				if (craftingRecipe != null && craftingRecipe.ResultValue != 0) //如果找到了匹配的熔炼配方，并且配方的结果物品不为0，继续执行以下判断：
				{
					if (craftingRecipe.RequiredHeatLevel != 1000f) //1.如果配方要求的热量级别小于等于0，则将熔炼配方设置为null，表示没有匹配的熔炼配方。
					{
						craftingRecipe = null;
					}

					if (craftingRecipe != null) //2.如果配方要求的热量级别大于0，继续执行以下判断：
					{
						ComponentInventoryBase.Slot slot = this.m_slots[this.ResultSlotIndex]; //获取结果物品槽（ResultSlotIndex）中的物品槽对象（slot）
						int num3 = Terrain.ExtractContents(craftingRecipe.ResultValue); //获取熔炼配方的结果物品类型（num3）

						//如果结果物品槽中已经有物品，并且该物品的类型与熔炼配方的结果物品类型不一致，
						//或者结果物品数量加上结果配方的数量大于该方块类型的最大堆叠数量，
						//则将熔炼配方设置为null，表示没有匹配的熔炼配方。
						if (slot.Count != 0 && (craftingRecipe.ResultValue != slot.Value || craftingRecipe.ResultCount + slot.Count > BlocksManager.Blocks[num3].GetMaxStacking(craftingRecipe.ResultValue)))
						{
							craftingRecipe = null;
						}
					}


					if (craftingRecipe != null && craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > 0)
					{
						//如果熔炼配方还有剩余物品，并且剩余物品槽（RemainsSlotIndex）中没有物品，或者剩余物品槽中的物品类型与剩余物品的类型一致，继续执行以下判断：
						if (this.m_slots[this.RemainsSlotIndex].Count == 0 || this.m_slots[this.RemainsSlotIndex].Value == craftingRecipe.RemainsValue)
						{


							//如果该方块类型的最大堆叠数量减去剩余物品槽中物品的数量小于剩余物品的数量，则将熔炼配方设置为null，表示没有匹配的熔炼配方。
							if (BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue) - this.m_slots[this.RemainsSlotIndex].Count < craftingRecipe.RemainsCount)
							{
								craftingRecipe = null;
							}
						}//否则，继续执行下一步
						else//如果剩余物品槽中的物品类型与剩余物品的类型不一致，则将熔炼配方设置为null，表示没有匹配的熔炼配方。
						{
							craftingRecipe = null;
						}
					}
				}

				//如果找到了匹配的熔炼配方，并且配方有提示信息，并且当前与熔炉交互的玩家不为空，就将提示信息显示在玩家的界面上。
				if (craftingRecipe != null && !string.IsNullOrEmpty(craftingRecipe.Message) && componentPlayer != null)
				{
					componentPlayer.ComponentGui.DisplaySmallMessage(craftingRecipe.Message, Color.White, true, true);
				}
				return craftingRecipe; //返回找到的匹配的熔炼配方。
			}
			return null;
		}

		public SubsystemTerrain m_subsystemTerrain;

		public SubsystemExplosions m_subsystemExplosions;

		public ComponentBlockEntity m_componentBlockEntity;

		public int m_furnaceSize;

		public string[] m_matchedIngredients = new string[9];

		public float m_fireTimeRemaining;

		public float m_heatLevel;

		public bool m_updateSmeltingRecipe;

		public CraftingRecipe m_smeltingRecipe;

		public float m_smeltingProgress;


	}

	#endregion



	#region  发酵桶gui
	public class FajiaotongWidget : CanvasWidget
	{
		public FajiaotongWidget(IInventory inventory, FCComponentFajiaotong componentFajiaotong)
		{
			this.m_componentFajiaotong = componentFajiaotong;
			XElement node = ContentManager.Get<XElement>("Widgets/FajiaotongWidget", null);
			base.LoadContents(this, node);
			this.m_inventoryGrid = this.Children.Find<GridPanelWidget>("InventoryGrid", true);
			this.m_furnaceGrid = this.Children.Find<GridPanelWidget>("FurnaceGrid", true);
			this.m_fire = this.Children.Find<FireWidget>("Fire", true);
			this.m_progress = this.Children.Find<ValueBarWidget>("Progress", true);
			this.m_resultSlot = this.Children.Find<InventorySlotWidget>("ResultSlot", true);
			this.m_remainsSlot = this.Children.Find<InventorySlotWidget>("RemainsSlot", true);
			this.m_fuelSlot = this.Children.Find<InventorySlotWidget>("FuelSlot", true);
			int num = 10;
			for (int i = 0; i < this.m_inventoryGrid.RowsCount; i++)
			{
				for (int j = 0; j < this.m_inventoryGrid.ColumnsCount; j++)
				{
					InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
					inventorySlotWidget.AssignInventorySlot(inventory, num++);
					this.m_inventoryGrid.Children.Add(inventorySlotWidget);
					this.m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
				}
			}
			num = 0;
			for (int k = 0; k < this.m_furnaceGrid.RowsCount; k++)
			{
				for (int l = 0; l < this.m_furnaceGrid.ColumnsCount; l++)
				{
					InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(componentFajiaotong, num++);
					this.m_furnaceGrid.Children.Add(inventorySlotWidget2);
					this.m_furnaceGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			this.m_fuelSlot.AssignInventorySlot(componentFajiaotong, componentFajiaotong.FuelSlotIndex);
			this.m_resultSlot.AssignInventorySlot(componentFajiaotong, componentFajiaotong.ResultSlotIndex);
			this.m_remainsSlot.AssignInventorySlot(componentFajiaotong, componentFajiaotong.RemainsSlotIndex);
		}

		public override void Update()
		{
			this.m_fire.ParticlesPerSecond = (float)((this.m_componentFajiaotong.HeatLevel > 0f) ? 24 : 0);
			this.m_progress.Value = this.m_componentFajiaotong.SmeltingProgress;
			if (!this.m_componentFajiaotong.IsAddedToProject)
			{
				base.ParentWidget.Children.Remove(this);
			}
		}

		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_furnaceGrid;

		public InventorySlotWidget m_fuelSlot;

		public InventorySlotWidget m_resultSlot;

		public InventorySlotWidget m_remainsSlot;

		public FireWidget m_fire;

		public ValueBarWidget m_progress;

		public FCComponentFajiaotong m_componentFajiaotong;
	}

	#endregion

	#endregion

	#region 硝石转换器
	#region 方块__硝石转换器
	public class FCFermentation : FCSixFaceBlock
	{
		public const int Index = 969;
		public FCFermentation()
			: base("Textures/FCBlocks/Fermentation", Color.White)
		{
			

		}
		public override string GetDescription(int value)
		{
			return "硝石转换器。该机制方块主要由slice提供，在此鸣谢。";
		}
		

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).OpaqueSubsetsByFace);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

	}
	public class FCSubsystemFermentationBlockBehavior : SubsystemBlockBehavior
	{
		public SubsystemBlockEntities m_subsystemBlockEntities;
		public SubsystemAudio m_subsystemAudio;
		// 引用其他子系统和组件
		public override int[] HandledBlocks => new int[1]
		{
			969
		};

		// 在加载时获取其他子系统的实例
		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			m_subsystemBlockEntities = Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true);//方块实体
			m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(throwOnError: true);  //声音
		}

		// 当方块被放置时创建一个发酵桶实体并添加到项目中
		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			DatabaseObject databaseObject = Project.GameDatabase.Database.FindDatabaseObject("FCFermentation", Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
			var valuesDictionary = new ValuesDictionary();
			valuesDictionary.PopulateFromDatabaseObject(databaseObject);
			valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", new Point3(x, y, z));
			Entity entity = Project.CreateEntity(valuesDictionary);
			Project.AddEntity(entity);
		}

		// 当方块被移除时，移除发酵桶实体并将其中的物品掉落到方块所在位置
		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			ComponentBlockEntity blockEntity = m_subsystemBlockEntities.GetBlockEntity(x, y, z);
			if (blockEntity != null)
			{
				Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
				foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
				{
					item.DropAllItems(position);
				}
				Project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
			}
		}

		// 当与方块交互时打开发酵桶界面
		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = m_subsystemBlockEntities.GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				FCComponentFermentation dissolvestove = blockEntity.Entity.FindComponent<FCComponentFermentation>(throwOnError: true);
				// 创建发酵桶界面，并传入玩家的背包和发酵组件
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new FermentationWidget(componentMiner.Inventory, dissolvestove);
				AudioManager.PlaySound("Audio/UI/start", 1f, 0f, 0f);				         
				return true;
			}
			return false;


		}
	}
	public class FCComponentFermentation : ComponentInventoryBase, IUpdateable
	{
		public double m_nextupdatetime = 0;

		// 在加载时获取其他子系统的实例
		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_componentBlockEntity = Entity.FindComponent<ComponentBlockEntity>(throwOnError: true);
			subsystemModelsRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true);

		}
		public SubsystemTerrain m_subsystemTerrain;
		public SubsystemModelsRenderer subsystemModelsRenderer;
		public PrimitivesRenderer3D m_primitivesRenderer3D = new PrimitivesRenderer3D();
		public ComponentBlockEntity m_componentBlockEntity;

		// 获取输出槽位的索引
		public int ResultSlotIndex => SlotsCount - 1;//3-1

		// 获取燃料槽位的索引
		public int FuelSlotIndex => SlotsCount - 2;//3-2


		// 获取熔炼进度
		public float m_smeltingProgress;
		public float SmeltingProgress => m_smeltingProgress;

		// 实现接口方法
		public UpdateOrder UpdateOrder => UpdateOrder.Default;


		// 根据槽位和值获取容量大小
		public override int GetSlotCapacity(int slotIndex, int value)//处理三个格子的值
		{
			if (slotIndex == FuelSlotIndex)
			{
				

				if (value == Terrain.MakeBlockValue(22, 0, 0))//燃料端的格子容量
				{
					return base.GetSlotCapacity(slotIndex, value);
				}
				return 0;
			}
			if (slotIndex == ResultSlotIndex) //输出端的格子容量
			{
				if (value == Terrain.MakeBlockValue(102, 0, 0))//输入端口
				{
					return base.GetSlotCapacity(slotIndex, value);
				}
				return 0;
			}
			return base.GetSlotCapacity(slotIndex, value);
		}

		// 判断是否完成发酵
		public bool ProcessOver()   //筛选食品工艺方块
		{
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					if ((item.GetCategory(creativeValue) == "Plants" || (item.GetCategory(creativeValue) == "Food") && item.GetSicknessProbability(creativeValue) > 0f && m_slots[0].Value == creativeValue))
					{
						return true;
					}
				}
			}
			return false;
		}

		// 判断是否满足开始发酵的条件
		public bool ProcessBegin()             //筛选器功能
		{
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					//方块必须属于植物类别或食物类别，并且能引起疾病，输入槽位的物品必须是当前方块，燃料槽位必须有足够的燃料，
					//同时输出槽位必须有足够的空间。只有当所有这些条件都满足时，条件判断才为真，表示可以开始发酵。
					if ((item.GetCategory(creativeValue) == "Plants" || (item.GetCategory(creativeValue) == "Food") && item.GetSicknessProbability(creativeValue) > 0f  && m_slots[FuelSlotIndex].Count > 0 && m_slots[ResultSlotIndex].Count < 40))
					{

						return true;
					}
				}
			}
			return false;
		}

		// 更新发酵过程
		public void Update(float dt)//WoodWaterBucketBlock 301    //XWaterBottleBlock 301
		{
			// 判断是否满足开始发酵的条件
			if (ProcessBegin())
			{
				// 增加发酵进度，最大不超过 1
				m_smeltingProgress = MathUtils.Min(m_smeltingProgress + 0.05f * dt, 1f);
			}
			// 如果发酵进度达到了 1
			if (m_smeltingProgress >= 1f)
			{
				// 如果发酵已经完成
				if (ProcessOver())
				{
					// 生成输出物品（无机盐）
					m_slots[ResultSlotIndex].Value = Terrain.MakeBlockValue(102, 0, 0);//无机盐
					m_slots[ResultSlotIndex].Count++;
				}
				// 重置发酵进度为 0
				m_smeltingProgress = 0f;
				m_slots[0].Count--;//输入端减少
				m_slots[FuelSlotIndex].Count--;//减少燃料
			}
			//什么情况下停止
			// 如果输入物品的数量为 0 或者燃料物品的数量为 0，则重置发酵进度为 0
			if (m_slots[0].Count == 0 || m_slots[FuelSlotIndex].Count == 0)
			{
				m_smeltingProgress = 0f;
			}
		}
	}
	public class FermentationWidget : CanvasWidget
	{
		internal class Order
		{
			public Block block;
			public int order;
			public int value;
			public Order(Block b, int o, int v) { block = b; order = o; value = v; }
		}
		public List<string> m_categories = new List<string>();
		public int m_categoryIndex;
		public int m_listCategoryIndex = -1;

		public GridPanelWidget m_waterpurifierGrid;

		public InventorySlotWidget m_fuelSlot;

		public InventorySlotWidget m_resultSlot;
		public ListPanelWidget m_blocksList;
		public ValueBarWidget m_progress;
		public ButtonWidget m_nextCategoryButton;
		public ButtonWidget m_prevCategoryButton;
		public FCComponentFermentation m_fermentation;







		public FermentationWidget(IInventory inventory, FCComponentFermentation componentDissolveStove)
		{
			m_fermentation = componentDissolveStove;
			XElement node = ContentManager.Get<XElement>("Widgets/FCfajiaotong/FermentationWidget");
			LoadContents(this, node);
			m_waterpurifierGrid = Children.Find<GridPanelWidget>("WaterPurifierGrid");
			m_progress = Children.Find<ValueBarWidget>("Progress");
			m_resultSlot = Children.Find<InventorySlotWidget>("ResultSlot");
			//m_result2Slot = Children.Find<InventorySlotWidget>("Result2Slot");
			m_fuelSlot = Children.Find<InventorySlotWidget>("FuelSlot");
			
			


			


			int num = 0;
			for (int k = 0; k < m_waterpurifierGrid.RowsCount; k++)
			{
				for (int l = 0; l < m_waterpurifierGrid.ColumnsCount; l++)
				{
					var inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(componentDissolveStove, num++);
					m_waterpurifierGrid.Children.Add(inventorySlotWidget2);
					m_waterpurifierGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			m_fuelSlot.AssignInventorySlot(componentDissolveStove, componentDissolveStove.FuelSlotIndex);
			m_resultSlot.AssignInventorySlot(componentDissolveStove, componentDissolveStove.ResultSlotIndex);
		}
		

		public override void Update()
		{
			// 检查发酵桶是否已经添加到项目中，如果没有则从父控件中移除自身
			if (!m_fermentation.IsAddedToProject)
			{
				ParentWidget.Children.Remove(this);
			}
			// 检查当前选中的分类索引是否与列表分类索引不匹配，如果不匹配则重新填充方块列表
			

			
			
			// 设置进度条的值为当前发酵进度
			m_progress.Value = m_fermentation.SmeltingProgress;
		}
	}
    #endregion


    #endregion

    #region 高炉区
    #region 高炉
    public class GaoluBlock2 : GaoluBlock //正在发酵的发酵桶
	{
		public new const int Index = 976;
	}
	public class GaoluBlock : Block  //发酵桶方块
	{
		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

		public BlockMesh[] m_blockMeshesByData = new BlockMesh[4];

		private Texture2D m_texture;

		public override void Initialize()
		{
			Model model = ContentManager.Get<Model>("Models/Furnace1", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Furnace", true).ParentBone);
			for (int i = 0; i < 4; i++)
			{
				this.m_blockMeshesByData[i] = new BlockMesh();
				Matrix matrix = Matrix.Identity;
				matrix *= Matrix.CreateRotationY((float)i * 3.1415927f / 2f) * Matrix.CreateTranslation(0.5f, 0f, 0.5f);
				this.m_blockMeshesByData[i].AppendModelMeshPart(model.FindMesh("Furnace", true).MeshParts[0], boneAbsoluteTransform * matrix, false, false, false, false, Color.White);
			}
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Furnace", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.5f, 0f), false, false, false, false, Color.White);
			m_texture = ContentManager.Get<Texture2D>("Textures/FCBlocks/gaolu", null);  //外置材质
			base.Initialize();
		}

		public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
		{
			return false;
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			int num = Terrain.ExtractData(value);
			if (num < this.m_blockMeshesByData.Length)
			{
				generator.GenerateShadedMeshVertices(this, x, y, z, this.m_blockMeshesByData[num], Color.White, null, null, geometry.GetGeometry(m_texture).SubsetAlphaTest);
			}
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, size, ref matrix, environmentData);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(0, value), m_texture);
		}

		public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult)
		{
			Vector3 forward = Matrix.CreateFromQuaternion(componentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation).Forward;
			float num = Vector3.Dot(forward, Vector3.UnitZ);
			float num2 = Vector3.Dot(forward, Vector3.UnitX);
			float num3 = Vector3.Dot(forward, -Vector3.UnitZ);
			float num4 = Vector3.Dot(forward, -Vector3.UnitX);
			int data = 0;
			if (num == MathUtils.Max(num, num2, num3, num4))
			{
				data = 2;
			}
			else if (num2 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 3;
			}
			else if (num3 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 0;
			}
			else if (num4 == MathUtils.Max(num, num2, num3, num4))
			{
				data = 1;
			}
			return new BlockPlacementData
			{
				Value = Terrain.ReplaceData(Terrain.ReplaceContents(0, 975), data),
				CellFace = raycastResult.CellFace
			};
		}

		public const int Index = 975;
	}

	public class FCSubsystemGaoluBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					975, //64
					976  //65
				};
			}
		}

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(oldValue) != 975 && Terrain.ExtractContents(oldValue) != 976)
			{
				DatabaseObject databaseObject = base.SubsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("GaoluBlock", base.SubsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
				ValuesDictionary valuesDictionary = new ValuesDictionary();
				valuesDictionary.PopulateFromDatabaseObject(databaseObject);
				valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x, y, z));
				Entity entity = base.SubsystemTerrain.Project.CreateEntity(valuesDictionary);
				base.SubsystemTerrain.Project.AddEntity(entity);
			}
			if (Terrain.ExtractContents(value) == 976) //如果是发光发酵桶
			{
				this.AddFire(value, x, y, z);
			}
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(newValue) != 975 && Terrain.ExtractContents(newValue) != 976)
			{
				ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(x, y, z);
				if (blockEntity != null)
				{
					Vector3 position = new Vector3((float)x, (float)y, (float)z) + new Vector3(0.5f);
					foreach (IInventory inventory in blockEntity.Entity.FindComponents<IInventory>())
					{
						inventory.DropAllItems(position);
					}
					base.SubsystemTerrain.Project.RemoveEntity(blockEntity.Entity, true);
				}
			}
			if (Terrain.ExtractContents(value) == 976)
			{
				this.RemoveFire(x, y, z);
			}
		}

		public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
		{
			if (Terrain.ExtractContents(value) == 976)
			{
				this.AddFire(value, x, y, z);
			}
		}

		public override void OnChunkDiscarding(TerrainChunk chunk) //区块卸载时，移除粒子效果
		{
			List<Point3> list = new List<Point3>();
			foreach (Point3 point in this.m_particleSystemsByCell.Keys)
			{
				if (point.X >= chunk.Origin.X && point.X < chunk.Origin.X + 16 && point.Z >= chunk.Origin.Y && point.Z < chunk.Origin.Y + 16)
				{
					list.Add(point);
				}
			}
			foreach (Point3 point2 in list)
			{
				this.RemoveFire(point2.X, point2.Y, point2.Z);
			}
		}

		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				FCComponentGaolu componentGaolu = blockEntity.Entity.FindComponent<FCComponentGaolu>(true);
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new GaoluWidget(componentMiner.Inventory, componentGaolu);
				AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
				return true;
			}
			return false;
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			base.OnNeighborBlockChanged(x, y, z, neighborX, neighborY, neighborZ);
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
		}

		public void AddFire(int value, int x, int y, int z)
		{
			Vector3 v = new Vector3(0.5f, 0.2f, 0.5f);
			float size = 0.15f;
			FireParticleSystem fireParticleSystem = new FireParticleSystem(new Vector3((float)x, (float)y, (float)z) + v, size, 16f);
			this.m_subsystemParticles.AddParticleSystem(fireParticleSystem);
			this.m_particleSystemsByCell[new Point3(x, y, z)] = fireParticleSystem;
		}

		public void RemoveFire(int x, int y, int z)
		{
			Point3 key = new Point3(x, y, z);
			FireParticleSystem particleSystem = this.m_particleSystemsByCell[key];
			this.m_subsystemParticles.RemoveParticleSystem(particleSystem);
			this.m_particleSystemsByCell.Remove(key);
		}

		public SubsystemParticles m_subsystemParticles;

		public Dictionary<Point3, FireParticleSystem> m_particleSystemsByCell = new Dictionary<Point3, FireParticleSystem>();
	}
	#endregion

	#region 高炉组件
	public class FCComponentGaolu : ComponentInventoryBase, IUpdateable
	{
        private SubsystemTerrain m_subsystemTerrain;

        private SubsystemExplosions m_subsystemExplosions;

        private ComponentBlockEntity m_componentBlockEntity;

        private int m_furnaceSize;

        private string[] m_matchedIngredients = new string[16];

        private float m_fireTimeRemaining;

        private float m_heatLevel;

        private bool m_updateSmeltingRecipe;

        private CraftingRecipe m_smeltingRecipe;

        private float m_smeltingProgress;

        public int RemainsSlotIndex => SlotsCount - 1;

        public int ResultSlotIndex => SlotsCount - 2;

        public int FuelSlotIndex => SlotsCount - 3;

        public float HeatLevel => m_heatLevel;

        public float SmeltingProgress => m_smeltingProgress;

        public UpdateOrder UpdateOrder => UpdateOrder.Default;

        public override int GetSlotCapacity(int slotIndex, int value)
        {
            if (slotIndex == FuelSlotIndex)
            {
                if (BlocksManager.Blocks[Terrain.ExtractContents(value)].FuelHeatLevel > 0f)
                {
                    return base.GetSlotCapacity(slotIndex, value);
                }
                return 0;
            }
            return base.GetSlotCapacity(slotIndex, value);
        }

        public override void AddSlotItems(int slotIndex, int value, int count)
        {
            base.AddSlotItems(slotIndex, value, count);
            m_updateSmeltingRecipe = true;
        }

        public override int RemoveSlotItems(int slotIndex, int count)
        {
            m_updateSmeltingRecipe = true;
            return base.RemoveSlotItems(slotIndex, count);
        }

        public void Update(float dt)
        {
            Point3 coordinates = m_componentBlockEntity.Coordinates;
            if (m_heatLevel == 2f)//如果热值是11
            {
                m_fireTimeRemaining = MathUtils.Max(0f, m_fireTimeRemaining - dt);
                if (m_fireTimeRemaining == 0f)
                {
                    m_heatLevel = 0f;
                }
            }
            if (m_updateSmeltingRecipe)
            {
                m_updateSmeltingRecipe = false;
                float heatLevel = 0f;
                if (m_heatLevel > 0f)
                {
                    heatLevel = m_heatLevel;
                }
                else
                {
                    Slot slot = m_slots[FuelSlotIndex];
                    if (slot.Count > 0)
                    {
                        int num = Terrain.ExtractContents(slot.Value);
                        heatLevel = BlocksManager.Blocks[num].FuelHeatLevel;
                    }
                }
                CraftingRecipe craftingRecipe = FindSmeltingRecipe(heatLevel);
                if (craftingRecipe != m_smeltingRecipe)
                {
                    m_smeltingRecipe = craftingRecipe;
                    m_smeltingProgress = 0f;
                }
            }
            if (m_smeltingRecipe == null)
            {
                m_heatLevel = 0f;
                m_fireTimeRemaining = 0f;
            }
            if (m_smeltingRecipe != null && m_fireTimeRemaining <= 0f)
            {
                Slot slot2 = m_slots[FuelSlotIndex];
                if (slot2.Count > 0)
                {
                    int num2 = Terrain.ExtractContents(slot2.Value);
                    Block block = BlocksManager.Blocks[num2];
                    if (block.GetExplosionPressure(slot2.Value) > 0f)
                    {
                        slot2.Count = 0;
                        m_subsystemExplosions.TryExplodeBlock(coordinates.X, coordinates.Y, coordinates.Z, slot2.Value);
                    }
                    else if (block.FuelHeatLevel > 0f)
                    {
                        slot2.Count--;
                        m_fireTimeRemaining = block.FuelFireDuration;
                        m_heatLevel = block.FuelHeatLevel;
                    }
                    
                }
            }
            if (m_fireTimeRemaining <= 0f)
            {
                m_smeltingRecipe = null;
                m_smeltingProgress = 0f;
            }
            if (m_smeltingRecipe != null)
            {
                m_smeltingProgress = MathUtils.Min(m_smeltingProgress + 0.2f * dt, 1f);
                if (m_smeltingProgress >= 1f)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (m_slots[i].Count > 0)
                        {
                            m_slots[i].Count--;
                        }
                    }
                    m_slots[ResultSlotIndex].Value = m_smeltingRecipe.ResultValue;
                    m_slots[ResultSlotIndex].Count += m_smeltingRecipe.ResultCount;
                    if (m_smeltingRecipe.RemainsValue != 0 && m_smeltingRecipe.RemainsCount > 0)
                    {
                        m_slots[RemainsSlotIndex].Value = m_smeltingRecipe.RemainsValue;
                        m_slots[RemainsSlotIndex].Count += m_smeltingRecipe.RemainsCount;
                    }
                    m_smeltingRecipe = null;
                    m_smeltingProgress = 0f;
                    m_updateSmeltingRecipe = true;
                }
            }
            TerrainChunk chunkAtCell = m_subsystemTerrain.Terrain.GetChunkAtCell(coordinates.X, coordinates.Z);
            if (chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid)
            {
                int cellValue = m_subsystemTerrain.Terrain.GetCellValue(coordinates.X, coordinates.Y, coordinates.Z);
                m_subsystemTerrain.ChangeCell(coordinates.X, coordinates.Y, coordinates.Z, Terrain.ReplaceContents(cellValue, (m_heatLevel > 0f) ? 976 : 975));
            }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            m_subsystemExplosions = base.Project.FindSubsystem<SubsystemExplosions>(true);
            m_componentBlockEntity = base.Entity.FindComponent<ComponentBlockEntity>(true);
            m_furnaceSize = SlotsCount - 3;
            if (m_furnaceSize < 1 || m_furnaceSize > 16)
            {
                throw new InvalidOperationException("Invalid furnace size.");
            }
            m_fireTimeRemaining = valuesDictionary.GetValue<float>("FireTimeRemaining");
            m_heatLevel = valuesDictionary.GetValue<float>("HeatLevel");
            m_updateSmeltingRecipe = true;
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
            valuesDictionary.SetValue("FireTimeRemaining", m_fireTimeRemaining);
            valuesDictionary.SetValue("HeatLevel", m_heatLevel);
        }

        private CraftingRecipe FindSmeltingRecipe(float heatLevel)
        {
            if (heatLevel ==2f)
            {
                for (int i = 0; i < this.m_furnaceSize; i++)
                {
                    int slotValue = this.GetSlotValue(i);
                    int num = Terrain.ExtractContents(slotValue);
                    int num2 = Terrain.ExtractData(slotValue);
                    if (this.GetSlotCount(i) > 0)
                    {
                        Block block = BlocksManager.Blocks[num];
                        this.m_matchedIngredients[i] = block.GetCraftingId(slotValue) + ":" + num2.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.m_matchedIngredients[i] = null;
                    }
                }
                ComponentPlayer componentPlayer = base.FindInteractingPlayer();
                float playerLevel = (componentPlayer != null) ? componentPlayer.PlayerData.Level : 1f;
                CraftingRecipe craftingRecipe = XCraftingRecipesManager.FindMatchingRecipe(this.m_subsystemTerrain, this.m_matchedIngredients, heatLevel, playerLevel);
                if (craftingRecipe != null && craftingRecipe.ResultValue != 0)
                {
                    if (craftingRecipe.RequiredHeatLevel != 2f)//如果配方的要求热度小于1，则为null
                    {
                        craftingRecipe = null;
                    }
                    if (craftingRecipe != null)
                    {
                        ComponentInventoryBase.Slot slot = this.m_slots[this.ResultSlotIndex];
                        int num3 = Terrain.ExtractContents(craftingRecipe.ResultValue);
                        if (slot.Count != 0 && (craftingRecipe.ResultValue != slot.Value || craftingRecipe.ResultCount + slot.Count > BlocksManager.Blocks[num3].GetMaxStacking(craftingRecipe.ResultValue)))
                        {
                            craftingRecipe = null;
                        }
                    }
                    if (craftingRecipe != null && craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > 0)
                    {
                        if (this.m_slots[this.RemainsSlotIndex].Count == 0 || this.m_slots[this.RemainsSlotIndex].Value == craftingRecipe.RemainsValue)
                        {
                            if (BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue) - this.m_slots[this.RemainsSlotIndex].Count < craftingRecipe.RemainsCount)
                            {
                                craftingRecipe = null;
                            }
                        }
                        else
                        {
                            craftingRecipe = null;
                        }
                    }
                }
                if (craftingRecipe != null && !string.IsNullOrEmpty(craftingRecipe.Message) && componentPlayer != null)
                {
                    componentPlayer.ComponentGui.DisplaySmallMessage(craftingRecipe.Message, Color.White, true, true);
                }
                return craftingRecipe;
            }
            return null;

        }


    }

	#endregion

	#region  高炉gui
	public class GaoluWidget : CanvasWidget
	{
		public GaoluWidget(IInventory inventory, FCComponentGaolu componentGaolu)
		{
			this.m_componentGaolu = componentGaolu;
			XElement node = ContentManager.Get<XElement>("Widgets/GaoluWidget", null);
			base.LoadContents(this, node);
			this.m_inventoryGrid = this.Children.Find<GridPanelWidget>("InventoryGrid", true);
			this.m_furnaceGrid = this.Children.Find<GridPanelWidget>("FurnaceGrid", true);
			this.m_fire = this.Children.Find<FireWidget>("Fire", true);
			this.m_progress = this.Children.Find<ValueBarWidget>("Progress", true);
			this.m_resultSlot = this.Children.Find<InventorySlotWidget>("ResultSlot", true);
			this.m_remainsSlot = this.Children.Find<InventorySlotWidget>("RemainsSlot", true);
			this.m_fuelSlot = this.Children.Find<InventorySlotWidget>("FuelSlot", true);
			int num = 10;
			for (int i = 0; i < this.m_inventoryGrid.RowsCount; i++)
			{
				for (int j = 0; j < this.m_inventoryGrid.ColumnsCount; j++)
				{
					InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
					inventorySlotWidget.AssignInventorySlot(inventory, num++);
					this.m_inventoryGrid.Children.Add(inventorySlotWidget);
					this.m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
				}
			}
			num = 0;
			for (int k = 0; k < this.m_furnaceGrid.RowsCount; k++)
			{
				for (int l = 0; l < this.m_furnaceGrid.ColumnsCount; l++)
				{
					InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget { Size = new Vector2(40f,40f)};
					inventorySlotWidget2.AssignInventorySlot(componentGaolu, num++);
					this.m_furnaceGrid.Children.Add(inventorySlotWidget2);
					this.m_furnaceGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			this.m_fuelSlot.AssignInventorySlot(componentGaolu, componentGaolu.FuelSlotIndex);
			this.m_resultSlot.AssignInventorySlot(componentGaolu, componentGaolu.ResultSlotIndex);
			this.m_remainsSlot.AssignInventorySlot(componentGaolu, componentGaolu.RemainsSlotIndex);
		}

		public override void Update()
		{
			this.m_fire.ParticlesPerSecond = (float)((this.m_componentGaolu.HeatLevel > 0f) ? 24 : 0);
			this.m_progress.Value = this.m_componentGaolu.SmeltingProgress;
			if (!this.m_componentGaolu.IsAddedToProject)
			{
				base.ParentWidget.Children.Remove(this);
			}
		}

		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_furnaceGrid;

		public InventorySlotWidget m_fuelSlot;

		public InventorySlotWidget m_resultSlot;

		public InventorySlotWidget m_remainsSlot;

		public FireWidget m_fire;

		public ValueBarWidget m_progress;

		public FCComponentGaolu m_componentGaolu;
	}

    #endregion

    #endregion

    #region 机床区
    public class JichuangBlock : Block
	{
		public Texture2D m_texture;
		

		public BlockMesh m_blockMesh = new BlockMesh();

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

		

		public override void Initialize()
		{
			Model model = ContentManager.Get<Model>("Models/CraftingTable1", null);
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Tool", true).ParentBone);
			this.m_blockMesh.AppendModelMeshPart(model.FindMesh("Tool", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0.5f, 0.7f, -1f), false, false, false, false, Color.White);
			this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Tool", true).MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0.5f, 0.7f, -1f), false, false, false, false, Color.White);
			m_texture = ContentManager.Get<Texture2D>("Textures/FCBlocks/Tool", null);  //外置材质
			base.Initialize();
			
		}

		

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateShadedMeshVertices(this, x, y, z, this.m_blockMesh, Color.White, null, null, geometry.GetGeometry(m_texture).SubsetOpaque);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, 0.5f, ref matrix, environmentData);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

		public const int Index = 977;

	
	}

	public class FCSubsystemJichuangBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					977
				};
			}
		}

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			DatabaseObject databaseObject = base.SubsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("JichuangBlock", base.SubsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
			ValuesDictionary valuesDictionary = new ValuesDictionary();
			valuesDictionary.PopulateFromDatabaseObject(databaseObject);
			valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x, y, z));
			Entity entity = base.SubsystemTerrain.Project.CreateEntity(valuesDictionary);
			base.SubsystemTerrain.Project.AddEntity(entity);
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(x, y, z);
			if (blockEntity != null)
			{
				Vector3 position = new Vector3((float)x, (float)y, (float)z) + new Vector3(0.5f);
				foreach (IInventory inventory in blockEntity.Entity.FindComponents<IInventory>())
				{
					inventory.DropAllItems(position);
				}
				base.SubsystemTerrain.Project.RemoveEntity(blockEntity.Entity, true);
			}
		}

		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				FCComponentJichuang componentJichuang = blockEntity.Entity.FindComponent<FCComponentJichuang>(true);
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new JichuangWidget(componentMiner.Inventory, componentJichuang);
				AudioManager.PlaySound("Audio/UI/jichuang", 1f, 0f, 0f);
				return true;
			}
			return false;
		}
	}

	public class FCComponentJichuang : ComponentInventoryBase
	{
		public int RemainsSlotIndex
		{
			get
			{
				return this.SlotsCount - 1;
			}
		}

		public int ResultSlotIndex
		{
			get
			{
				return this.SlotsCount - 2;
			}
		}

		public override int GetSlotCapacity(int slotIndex, int value)
		{
			if (slotIndex < this.SlotsCount - 2)
			{
				return base.GetSlotCapacity(slotIndex, value);
			}
			return 0;
		}

		public override void AddSlotItems(int slotIndex, int value, int count)
		{
			base.AddSlotItems(slotIndex, value, count);
			this.UpdateCraftingResult();
		}

		public override int RemoveSlotItems(int slotIndex, int count)
		{
			int num = 0;
			if (slotIndex == this.ResultSlotIndex)
			{
				if (this.m_matchedRecipe != null)
				{
					if (this.m_matchedRecipe.RemainsValue != 0 && this.m_matchedRecipe.RemainsCount > 0)
					{
						if (this.m_slots[this.RemainsSlotIndex].Count == 0 || this.m_slots[this.RemainsSlotIndex].Value == this.m_matchedRecipe.RemainsValue)
						{
							int num2 = BlocksManager.Blocks[Terrain.ExtractContents(this.m_matchedRecipe.RemainsValue)].GetMaxStacking(this.m_matchedRecipe.RemainsValue) - this.m_slots[this.RemainsSlotIndex].Count;
							count = MathUtils.Min(count, num2 / this.m_matchedRecipe.RemainsCount * this.m_matchedRecipe.ResultCount);
						}
						else
						{
							count = 0;
						}
					}
					count = count / this.m_matchedRecipe.ResultCount * this.m_matchedRecipe.ResultCount;
					num = base.RemoveSlotItems(slotIndex, count);
					if (num > 0)
					{
						for (int i = 0; i < 9; i++)
						{
							if (!string.IsNullOrEmpty(this.m_matchedIngredients[i]))
							{
								int index = i % 3 + this.m_craftingGridSize * (i / 3);
								this.m_slots[index].Count = MathUtils.Max(this.m_slots[index].Count - num / this.m_matchedRecipe.ResultCount, 0);
							}
						}
						if (this.m_matchedRecipe.RemainsValue != 0 && this.m_matchedRecipe.RemainsCount > 0)
						{
							this.m_slots[this.RemainsSlotIndex].Value = this.m_matchedRecipe.RemainsValue;
							this.m_slots[this.RemainsSlotIndex].Count += num / this.m_matchedRecipe.ResultCount * this.m_matchedRecipe.RemainsCount;
						}
						ComponentPlayer componentPlayer = base.FindInteractingPlayer();
						if (componentPlayer != null && componentPlayer.PlayerStats != null)
						{
							componentPlayer.PlayerStats.ItemsCrafted += (long)num;
						}
					}
				}
			}
			else
			{
				num = base.RemoveSlotItems(slotIndex, count);
			}
			this.UpdateCraftingResult();
			return num;
		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			this.m_craftingGridSize = (int)MathUtils.Sqrt((float)(this.SlotsCount - 2));
			this.UpdateCraftingResult();
		}

		public virtual void UpdateCraftingResult()
		{
			int num = int.MaxValue;
			for (int i = 0; i < this.m_craftingGridSize; i++)
			{
				for (int j = 0; j < this.m_craftingGridSize; j++)
				{
					int num2 = i + j * 3;
					int slotIndex = i + j * this.m_craftingGridSize;
					int slotValue = this.GetSlotValue(slotIndex);
					int num3 = Terrain.ExtractContents(slotValue);
					int num4 = Terrain.ExtractData(slotValue);
					int slotCount = this.GetSlotCount(slotIndex);
					if (slotCount > 0)
					{
						Block block = BlocksManager.Blocks[num3];
						this.m_matchedIngredients[num2] = block.GetCraftingId(slotValue) + ":" + num4.ToString(CultureInfo.InvariantCulture);
						num = MathUtils.Min(num, slotCount);
					}
					else
					{
						this.m_matchedIngredients[num2] = null;
					}
				}
			}
			ComponentPlayer componentPlayer = base.FindInteractingPlayer();
			float playerLevel = (componentPlayer != null) ? componentPlayer.PlayerData.Level : 1f;
			CraftingRecipe craftingRecipe = CraftingRecipesManager.FindMatchingRecipe(base.Project.FindSubsystem<SubsystemTerrain>(true), this.m_matchedIngredients, 10f, playerLevel);
			if (craftingRecipe != null && craftingRecipe.ResultValue != 0)
			{
				this.m_matchedRecipe = craftingRecipe;
				this.m_slots[this.ResultSlotIndex].Value = craftingRecipe.ResultValue;
				this.m_slots[this.ResultSlotIndex].Count = craftingRecipe.ResultCount * num;
			}
			else
			{
				this.m_matchedRecipe = null;
				this.m_slots[this.ResultSlotIndex].Value = 0;
				this.m_slots[this.ResultSlotIndex].Count = 0;
			}
			if (craftingRecipe != null && !string.IsNullOrEmpty(craftingRecipe.Message))
			{
				string text = craftingRecipe.Message;
				if (text.StartsWith("[") && text.EndsWith("]"))
				{
					text = LanguageControl.Get(new string[]
					{
						"CRMessage",
						text.Substring(1, text.Length - 2)
					});
				}
				if (componentPlayer != null)
				{
					componentPlayer.ComponentGui.DisplaySmallMessage(text, Color.White, true, true);
				}
			}
		}

		public int m_craftingGridSize;

		public string[] m_matchedIngredients = new string[9];

		public CraftingRecipe m_matchedRecipe;
	}

	public class JichuangWidget : CanvasWidget
	{
		public JichuangWidget(IInventory inventory, FCComponentJichuang componentJichuang)
		{
			this.m_componentJichuang = componentJichuang;
			XElement node = ContentManager.Get<XElement>("Widgets/JichuangWidget", null);
			base.LoadContents(this, node);
			this.m_inventoryGrid = this.Children.Find<GridPanelWidget>("InventoryGrid", true);
			this.m_craftingGrid = this.Children.Find<GridPanelWidget>("CraftingGrid", true);
			this.m_craftingResultSlot = this.Children.Find<InventorySlotWidget>("CraftingResultSlot", true);
			this.m_craftingRemainsSlot = this.Children.Find<InventorySlotWidget>("CraftingRemainsSlot", true);
			int num = 10;
			for (int i = 0; i < this.m_inventoryGrid.RowsCount; i++)
			{
				for (int j = 0; j < this.m_inventoryGrid.ColumnsCount; j++)
				{
					InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
					inventorySlotWidget.AssignInventorySlot(inventory, num++);
					this.m_inventoryGrid.Children.Add(inventorySlotWidget);
					this.m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
				}
			}
			num = 0;
			for (int k = 0; k < this.m_craftingGrid.RowsCount; k++)
			{
				for (int l = 0; l < this.m_craftingGrid.ColumnsCount; l++)
				{
					InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(this.m_componentJichuang, num++);
					this.m_craftingGrid.Children.Add(inventorySlotWidget2);
					this.m_craftingGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			this.m_craftingResultSlot.AssignInventorySlot(this.m_componentJichuang, this.m_componentJichuang.ResultSlotIndex);
			this.m_craftingRemainsSlot.AssignInventorySlot(this.m_componentJichuang, this.m_componentJichuang.RemainsSlotIndex);
		}

		public override void Update()
		{
			if (!this.m_componentJichuang.IsAddedToProject)
			{
				base.ParentWidget.Children.Remove(this);
			}
		}

		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_craftingGrid;

		public InventorySlotWidget m_craftingResultSlot;

		public InventorySlotWidget m_craftingRemainsSlot;

		public FCComponentJichuang m_componentJichuang;
	}

	#endregion

	#region 微观提取器
	public class FCMicro : FCSixFaceBlock
	{
		public const int Index = 978;
		public FCMicro()
			: base("Textures/FCBlocks/micro", Color.White)
		{


		}
		


		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).OpaqueSubsetsByFace);
		}

		public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
		{
			return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, Color.White, GetFaceTextureSlot(1, value), m_texture);
		}

	}
	public class FCSubsystemFCMicroBehavior : SubsystemBlockBehavior
	{
		public SubsystemBlockEntities m_subsystemBlockEntities;
		public SubsystemAudio m_subsystemAudio;
		// 引用其他子系统和组件
		public override int[] HandledBlocks => new int[1]
		{
			978
		};

		// 在加载时获取其他子系统的实例
		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			m_subsystemBlockEntities = Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true);//方块实体
			m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(throwOnError: true);  //声音
		}

		// 当方块被放置时创建一个发酵桶实体并添加到项目中
		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			DatabaseObject databaseObject = Project.GameDatabase.Database.FindDatabaseObject("FCMicro", Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
			var valuesDictionary = new ValuesDictionary();
			valuesDictionary.PopulateFromDatabaseObject(databaseObject);
			valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", new Point3(x, y, z));
			Entity entity = Project.CreateEntity(valuesDictionary);
			Project.AddEntity(entity);
		}

		// 当方块被移除时，移除发酵桶实体并将其中的物品掉落到方块所在位置
		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			ComponentBlockEntity blockEntity = m_subsystemBlockEntities.GetBlockEntity(x, y, z);
			if (blockEntity != null)
			{
				Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
				foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
				{
					item.DropAllItems(position);
				}
				Project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
			}
		}

		// 当与方块交互时打开发酵桶界面
		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = m_subsystemBlockEntities.GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				FCComponentMicro dissolvestove = blockEntity.Entity.FindComponent<FCComponentMicro>(throwOnError: true);
				// 创建发酵桶界面，并传入玩家的背包和发酵组件
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MicroWidget(componentMiner.Inventory, dissolvestove);
				AudioManager.PlaySound("Audio/UI/start", 1f, 0f, 0f);
				return true;
			}
			return false;


		}
	}
	public class FCComponentMicro : ComponentInventoryBase, IUpdateable
	{
		public double m_nextupdatetime = 0;

		// 在加载时获取其他子系统的实例
		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_componentBlockEntity = Entity.FindComponent<ComponentBlockEntity>(throwOnError: true);
			subsystemModelsRenderer = Project.FindSubsystem<SubsystemModelsRenderer>(true);

		}
		public SubsystemTerrain m_subsystemTerrain;
		public SubsystemModelsRenderer subsystemModelsRenderer;
		public PrimitivesRenderer3D m_primitivesRenderer3D = new PrimitivesRenderer3D();
		public ComponentBlockEntity m_componentBlockEntity;

		// 获取输出槽位的索引
		public int ResultSlotIndex => SlotsCount - 1;//3-1

		// 获取燃料槽位的索引
		public int FuelSlotIndex => SlotsCount - 2;//3-2


		// 获取熔炼进度
		public float m_smeltingProgress;
		public float SmeltingProgress => m_smeltingProgress;

		// 实现接口方法
		public UpdateOrder UpdateOrder => UpdateOrder.Default;


		// 根据槽位和值获取容量大小
		public override int GetSlotCapacity(int slotIndex, int value)//处理三个格子的值
		{
			if (slotIndex == FuelSlotIndex)
			{
				
				if (value == Terrain.MakeBlockValue(22, 0, 0))//燃料端的格子容量
				{
					return base.GetSlotCapacity(slotIndex, value);
				}
				return 0;
			}
			if (slotIndex == ResultSlotIndex) //输出端的格子容量
			{
				if (value == Terrain.MakeBlockValue(972, 0, 13))//输入端口
				{
					return base.GetSlotCapacity(slotIndex, value);
				}
				return 0;
			}
			return base.GetSlotCapacity(slotIndex, value);
		}

		// 判断是否完成发酵
		public bool ProcessOver()   //筛选食品工艺方块
		{
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					if ((item.GetCategory(creativeValue) == "Plants" || (item.GetCategory(creativeValue) == "Food") && item.GetSicknessProbability(creativeValue) > 0f && m_slots[0].Value == creativeValue))
					{
						return true;
					}
				}
			}
			return false;
		}

		// 判断是否满足开始发酵的条件
		public bool ProcessBegin()             //筛选器功能
		{
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					//方块必须属于植物类别或食物类别，并且能引起疾病，输入槽位的物品必须是当前方块，燃料槽位必须有足够的燃料，
					//同时输出槽位必须有足够的空间。只有当所有这些条件都满足时，条件判断才为真，表示可以开始发酵。
					if (m_slots[0].Value == Terrain.MakeBlockValue(2,0,0) && m_slots[FuelSlotIndex].Count > 0 && m_slots[ResultSlotIndex].Count < 100)
					{

						return true;
					}
				}
			}
			return false;
		}

		// 更新发酵过程
		public void Update(float dt)//WoodWaterBucketBlock 301    //XWaterBottleBlock 301
		{
			// 判断是否满足开始发酵的条件
			if (ProcessBegin())
			{
				// 增加发酵进度，最大不超过 1
				m_smeltingProgress = MathUtils.Min(m_smeltingProgress + 0.15f * dt, 1f);
			}
			// 如果发酵进度达到了 1
			if (m_smeltingProgress >= 1f)
			{
				// 如果发酵已经完成
				if (ProcessOver())
				{
					// 生成输出物品（无机盐）
					m_slots[ResultSlotIndex].Value = Terrain.MakeBlockValue(972, 0, 13);//酵母菌
					m_slots[ResultSlotIndex].Count++;
				}
				// 重置发酵进度为 0
				m_smeltingProgress = 0f;
				m_slots[0].Count--;//输入端减少
				m_slots[FuelSlotIndex].Count--;//减少燃料
			}
			//什么情况下停止
			// 如果输入物品的数量为 0 或者燃料物品的数量为 0，则重置发酵进度为 0
			if (m_slots[0].Count == 0 || m_slots[FuelSlotIndex].Count == 0)
			{
				m_smeltingProgress = 0f;
			}
		}
	}

	public class MicroWidget : CanvasWidget
	{
		internal class Order
		{
			public Block block;
			public int order;
			public int value;
			public Order(Block b, int o, int v) { block = b; order = o; value = v; }
		}
		public List<string> m_categories = new List<string>();
		public int m_categoryIndex;
		public int m_listCategoryIndex = -1;

		public GridPanelWidget m_waterpurifierGrid;

		public InventorySlotWidget m_fuelSlot;

		public InventorySlotWidget m_resultSlot;
		public ListPanelWidget m_blocksList;
		public ValueBarWidget m_progress;
		public ButtonWidget m_nextCategoryButton;
		public ButtonWidget m_prevCategoryButton;
		public FCComponentMicro m_fermentation;







		public MicroWidget(IInventory inventory, FCComponentMicro componentMicro)
		{
			m_fermentation = componentMicro;
			XElement node = ContentManager.Get<XElement>("Widgets/MicroWidget");
			LoadContents(this, node);
			m_waterpurifierGrid = Children.Find<GridPanelWidget>("WaterPurifierGrid");
			m_progress = Children.Find<ValueBarWidget>("Progress");
			m_resultSlot = Children.Find<InventorySlotWidget>("ResultSlot");
			//m_result2Slot = Children.Find<InventorySlotWidget>("Result2Slot");
			m_fuelSlot = Children.Find<InventorySlotWidget>("FuelSlot");







			int num = 0;
			for (int k = 0; k < m_waterpurifierGrid.RowsCount; k++)
			{
				for (int l = 0; l < m_waterpurifierGrid.ColumnsCount; l++)
				{
					var inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(componentMicro, num++);
					m_waterpurifierGrid.Children.Add(inventorySlotWidget2);
					m_waterpurifierGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			m_fuelSlot.AssignInventorySlot(componentMicro, componentMicro.FuelSlotIndex);
			m_resultSlot.AssignInventorySlot(componentMicro, componentMicro.ResultSlotIndex);
		}


		public override void Update()
		{
			// 检查发酵桶是否已经添加到项目中，如果没有则从父控件中移除自身
			if (!m_fermentation.IsAddedToProject)
			{
				ParentWidget.Children.Remove(this);
			}
			// 检查当前选中的分类索引是否与列表分类索引不匹配，如果不匹配则重新填充方块列表




			// 设置进度条的值为当前发酵进度
			m_progress.Value = m_fermentation.SmeltingProgress;
		}
	}
	#endregion

	#region 工业配方管理器
	public static class FCCraftingRecipesManager
	{
		public static ReadOnlyList<CraftingRecipe> Recipes
		{
			get
			{
				return new ReadOnlyList<CraftingRecipe>(CraftingRecipesManager.m_recipes);
			}
		}

		public static void Initialize()
		{
			CraftingRecipesManager.m_recipes.Clear();
			XElement item = null;
			foreach (ModEntity modEntity in ModsManager.ModList)
			{
				modEntity.LoadCr(ref item);
			}
			CraftingRecipesManager.LoadData(item);
			foreach (Block block in BlocksManager.Blocks)
			{
				CraftingRecipesManager.m_recipes.AddRange(block.GetProceduralCraftingRecipes());
			}
			CraftingRecipesManager.m_recipes.Sort(delegate (CraftingRecipe r1, CraftingRecipe r2)
			{
				int y = Enumerable.Count<string>(r1.Ingredients, (string s) => !string.IsNullOrEmpty(s));
				int x = Enumerable.Count<string>(r2.Ingredients, (string s) => !string.IsNullOrEmpty(s));
				return Comparer<int>.Default.Compare(x, y);
			});
		}

		public static void LoadData(XElement item)
		{
			XAttribute xattribute;
			if (!ModsManager.HasAttribute(item, (string name) => name == "Result", out xattribute))
			{
				foreach (XElement item2 in item.Elements())
				{
					CraftingRecipesManager.LoadData(item2);
				}
				return;
			}
			bool flag = false;
			ModsManager.HookAction("OnCraftingRecipeDecode", delegate (ModLoader modLoader)
			{
				modLoader.OnCraftingRecipeDecode(CraftingRecipesManager.m_recipes, item, out flag);
				return flag;
			});
			if (!flag)
			{
				CraftingRecipe item3 = CraftingRecipesManager.DecodeElementToCraftingRecipe(item, 3);
				CraftingRecipesManager.m_recipes.Add(item3);
			}
		}

		public static CraftingRecipe DecodeElementToCraftingRecipe(XElement item, int HorizontalLen = 3)
		{
			CraftingRecipe craftingRecipe = new CraftingRecipe();
			string attributeValue = XmlUtils.GetAttributeValue<string>(item, "Result");
			string text = XmlUtils.GetAttributeValue<string>(item, "Description");
			string text2;
			if (text.StartsWith("[") && text.EndsWith("]") && LanguageControl.TryGetBlock(attributeValue, "CRDescription:" + text.Substring(1, text.Length - 2), out text2))
			{
				text = text2;
			}
			craftingRecipe.ResultValue = CraftingRecipesManager.DecodeResult(attributeValue);
			craftingRecipe.ResultCount = XmlUtils.GetAttributeValue<int>(item, "ResultCount");
			string attributeValue2 = XmlUtils.GetAttributeValue<string>(item, "Remains", string.Empty);
			if (!string.IsNullOrEmpty(attributeValue2))
			{
				craftingRecipe.RemainsValue = CraftingRecipesManager.DecodeResult(attributeValue2);
				craftingRecipe.RemainsCount = XmlUtils.GetAttributeValue<int>(item, "RemainsCount");
			}
			craftingRecipe.RequiredHeatLevel = XmlUtils.GetAttributeValue<float>(item, "RequiredHeatLevel");
			craftingRecipe.RequiredPlayerLevel = XmlUtils.GetAttributeValue<float>(item, "RequiredPlayerLevel", 1f);
			craftingRecipe.Description = text;
			craftingRecipe.Message = XmlUtils.GetAttributeValue<string>(item, "Message", null);
			if (craftingRecipe.ResultCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.ResultValue)].GetMaxStacking(craftingRecipe.ResultValue))
			{
				throw new InvalidOperationException("In recipe for \"" + attributeValue + "\" ResultCount is larger than max stacking of result block.");
			}
			if (craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue))
			{
				throw new InvalidOperationException("In Recipe for \"" + attributeValue2 + "\" RemainsCount is larger than max stacking of remains block.");
			}
			Dictionary<char, string> dictionary = new Dictionary<char, string>();
			foreach (XAttribute xattribute in Enumerable.Where<XAttribute>(item.Attributes(), (XAttribute a) => a.Name.LocalName.Length == 1 && char.IsLower(a.Name.LocalName[0])))
			{
				string craftingId;
				int? num;
				CraftingRecipesManager.DecodeIngredient(xattribute.Value, out craftingId, out num);
				if (BlocksManager.FindBlocksByCraftingId(craftingId).Length == 0)
				{
					throw new InvalidOperationException("Block with craftingId \"" + xattribute.Value + "\" not found.");
				}
				if (num != null && (num.Value < 0 || num.Value > 262143))
				{
					throw new InvalidOperationException("Data in recipe ingredient \"" + xattribute.Value + "\" must be between 0 and 0x3FFFF.");
				}
				dictionary.Add(xattribute.Name.LocalName[0], xattribute.Value);
			}
			string[] array = item.Value.Trim().Split(new string[]
			{
				"\n"
			}, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = array[i].IndexOf('"');
				int num3 = array[i].LastIndexOf('"');
				if (num2 < 0 || num3 < 0 || num3 <= num2)
				{
					throw new InvalidOperationException("Invalid recipe line.");
				}
				string text3 = array[i].Substring(num2 + 1, num3 - num2 - 1);
				for (int j = 0; j < text3.Length; j++)
				{
					char c = text3[j];
					if (char.IsLower(c))
					{
						string text4 = dictionary[c];
						craftingRecipe.Ingredients[j + i * HorizontalLen] = text4;
					}
				}
			}
			return craftingRecipe;
		}

		public static CraftingRecipe FindMatchingRecipe(SubsystemTerrain terrain, string[] ingredients, float heatLevel, float playerLevel)
		{
			CraftingRecipe craftingRecipe = null;
			Block[] blocks = BlocksManager.Blocks;
			for (int i = 0; i < blocks.Length; i++)
			{
				CraftingRecipe adHocCraftingRecipe = blocks[i].GetAdHocCraftingRecipe(terrain, ingredients, heatLevel, playerLevel);
				if (adHocCraftingRecipe != null && CraftingRecipesManager.MatchRecipe(adHocCraftingRecipe.Ingredients, ingredients) )
				{
					if (adHocCraftingRecipe.RequiredHeatLevel == 10f)
                    {
						craftingRecipe = adHocCraftingRecipe;
						break;
					}
						
				}
			}
			if (craftingRecipe == null)
			{
				foreach (CraftingRecipe craftingRecipe2 in CraftingRecipesManager.Recipes)
				{
					if (CraftingRecipesManager.MatchRecipe(craftingRecipe2.Ingredients, ingredients))
					{
						if (craftingRecipe2.RequiredHeatLevel == 10f) // 只有热值为 -2 的配方才能制作
						{
							craftingRecipe = craftingRecipe2;
							break;
						}
					}
				}
			}
			if (craftingRecipe != null)
			{
				if (heatLevel < craftingRecipe.RequiredHeatLevel)
				{
					CraftingRecipe craftingRecipe3;
					if (heatLevel > 0f)
					{
						(craftingRecipe3 = new CraftingRecipe()).Message = LanguageControl.Get(CraftingRecipesManager.fName, 1);
					}
					else
					{
						(craftingRecipe3 = new CraftingRecipe()).Message = LanguageControl.Get(CraftingRecipesManager.fName, 0);
					}
					craftingRecipe = craftingRecipe3;
				}
				else if (playerLevel < craftingRecipe.RequiredPlayerLevel)
				{
					CraftingRecipe craftingRecipe4;
					if (craftingRecipe.RequiredHeatLevel > 0f)
					{
						(craftingRecipe4 = new CraftingRecipe()).Message = string.Format(LanguageControl.Get(CraftingRecipesManager.fName, 3), craftingRecipe.RequiredPlayerLevel);
					}
					else
					{
						(craftingRecipe4 = new CraftingRecipe()).Message = string.Format(LanguageControl.Get(CraftingRecipesManager.fName, 2), craftingRecipe.RequiredPlayerLevel);
					}
					craftingRecipe = craftingRecipe4;
				}
			}
			return craftingRecipe;
		}

		public static int DecodeResult(string result)
		{
			bool flag2 = false;
			int result2 = 0;
			ModsManager.HookAction("DecodeResult", delegate (ModLoader modLoader)
			{
				result2 = modLoader.DecodeResult(result, out flag2);
				return flag2;
			});
			if (flag2)
			{
				return result2;
			}
			if (!string.IsNullOrEmpty(result))
			{
				string[] array = result.Split(new char[]
				{
					':'
				}, StringSplitOptions.None);
				return Terrain.MakeBlockValue(BlocksManager.FindBlockByTypeName(array[0], true).BlockIndex, 0, (array.Length == 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0);
			}
			return 0;
		}

		public static void DecodeIngredient(string ingredient, out string craftingId, out int? data)
		{
			bool flag2 = false;
			string craftingId_R = string.Empty;
			int? data_R = null;
			ModsManager.HookAction("DecodeIngredient", delegate (ModLoader modLoader)
			{
				modLoader.DecodeIngredient(ingredient, out craftingId_R, out data_R, out flag2);
				return flag2;
			});
			if (flag2)
			{
				craftingId = craftingId_R;
				data = data_R;
				return;
			}
			string[] array = ingredient.Split(new char[]
			{
				':'
			}, StringSplitOptions.None);
			craftingId = array[0];
			data = ((array.Length >= 2) ? new int?(int.Parse(array[1], CultureInfo.InvariantCulture)) : null);
		}

		public static bool MatchRecipe(string[] requiredIngredients, string[] actualIngredients)
		{
			bool flag2 = false;
			bool result = false;
			ModsManager.HookAction("MatchRecipe", delegate (ModLoader modLoader)
			{
				result = modLoader.MatchRecipe(requiredIngredients, actualIngredients, out flag2);
				return flag2;
			});
			if (flag2)
			{
				return result;
			}
			if (actualIngredients.Length > 9)
			{
				return false;
			}
			string[] array = new string[9];
			for (int i = 0; i < 2; i++)
			{
				for (int j = -3; j <= 3; j++)
				{
					for (int k = -3; k <= 3; k++)
					{
						bool flip = i != 0;
						if (CraftingRecipesManager.TransformRecipe(array, requiredIngredients, k, j, flip))
						{
							bool flag = true;
							for (int l = 0; l < 9; l++)
							{
								if (l == actualIngredients.Length || !CraftingRecipesManager.CompareIngredients(array[l], actualIngredients[l]))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static bool TransformRecipe(string[] transformedIngredients, string[] ingredients, int shiftX, int shiftY, bool flip)
		{
			for (int i = 0; i < 9; i++)
			{
				transformedIngredients[i] = null;
			}
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					int num = (flip ? (3 - k - 1) : k) + shiftX;
					int num2 = j + shiftY;
					string text = ingredients[k + j * 3];
					if (num >= 0 && num2 >= 0 && num < 3 && num2 < 3)
					{
						transformedIngredients[num + num2 * 3] = text;
					}
					else if (!string.IsNullOrEmpty(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool CompareIngredients(string requiredIngredient, string actualIngredient)
		{
			if (requiredIngredient == null)
			{
				return actualIngredient == null;
			}
			if (actualIngredient == null)
			{
				return requiredIngredient == null;
			}
			string a;
			int? num;
			CraftingRecipesManager.DecodeIngredient(requiredIngredient, out a, out num);
			string b;
			int? num2;
			CraftingRecipesManager.DecodeIngredient(actualIngredient, out b, out num2);
			if (num2 == null)
			{
				throw new InvalidOperationException("Actual ingredient data not specified.");
			}
			return a == b && (num == null || num.Value == num2.Value);
		}

		public static List<CraftingRecipe> m_recipes = new List<CraftingRecipe>();

		public static string fName = "CraftingRecipesManager";
	}

    #endregion

    #region 配方界面显示

    #endregion
    #region 樱花酒等液体测试
    public class BloodBlock : FluidBlock
    {
        public BloodBlock() : base(BloodBlock.MaxLevel)
        {
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            Color sideColor;
            Color color = sideColor = Color.DarkRed;
            sideColor.A = byte.MaxValue;
            Color topColor = color;
            topColor.A = 0;
            base.GenerateFluidTerrainVertices(generator, value, x, y, z, sideColor, topColor, geometry.TransparentSubsetsByFace);
        }

        public const int Index = 984;

        public new static int MaxLevel = 7;
    }
    public class YHWaterBlock : FluidBlock
	{
		public YHWaterBlock() : base(YHWaterBlock.MaxLevel)
		{
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			Color sideColor;
			Color color = sideColor = new Color(255, 192, 203, 255);
			sideColor.A = byte.MaxValue;
			Color topColor = color;
			topColor.A = 0;
			base.GenerateFluidTerrainVertices(generator, value, x, y, z, sideColor, topColor, geometry.TransparentSubsetsByFace);
		}

		public const int Index = 979;

		public new static int MaxLevel = 7;
	}

	public class FCSubsystemYHWaterBlockBehavior : SubsystemFluidBlockBehavior, IUpdateable
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					979
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

		public FCSubsystemYHWaterBlockBehavior() : base(BlocksManager.FluidBlocks[979], true)
		{
		}

		public void Update(float dt)
		{
            if (base.SubsystemTime.PeriodicGameTimeEvent(0.20, 0.0))
            {
               // base.SpreadFluid();
            }
            if (base.SubsystemTime.PeriodicGameTimeEvent(1.0, 0.25))
			{
				float num = float.MaxValue;
				foreach (Vector3 p in base.SubsystemAudio.ListenerPositions)
				{
					float? num2 = base.CalculateDistanceToFluid(p, 8, true);
					if (num2 != null && num2.Value < num)
					{
						num = num2.Value;
					}
				}
				this.m_soundVolume = 0.5f * base.SubsystemAudio.CalculateVolume(num, 2f, 3.5f);
			}
			base.SubsystemAmbientSounds.WaterSoundVolume = MathUtils.Max(base.SubsystemAmbientSounds.WaterSoundVolume, this.m_soundVolume);
		}

		public override bool OnFluidInteract(int interactValue, int x, int y, int z, int fluidValue)
		{
			if (BlocksManager.Blocks[Terrain.ExtractContents(interactValue)] is MagmaBlock)
			{
				base.SubsystemAudio.PlayRandomSound("Audio/Sizzles", 1f, this.m_random.Float(-0.1f, 0.1f), new Vector3((float)x, (float)y, (float)z), 5f, true);
				base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
				base.Set(x, y, z, 3);
				return true;
			}
			return base.OnFluidInteract(interactValue, x, y, z, fluidValue);
		}

		public override void OnItemHarvested(int x, int y, int z, int blockValue, ref BlockDropValue dropValue, ref int newBlockValue)
		{
			if (y > 80 && SubsystemWeather.IsPlaceFrozen(base.SubsystemTerrain.Terrain.GetSeasonalTemperature(x, z), y))
			{
				dropValue.Value = Terrain.MakeBlockValue(62);
				return;
			}
			base.OnItemHarvested(x, y, z, blockValue, ref dropValue, ref newBlockValue);
		}

		public Random m_random = new Random();

		public float m_soundVolume;
	}
    public class FCSubsystemBloodBlockBehavior : SubsystemFluidBlockBehavior, IUpdateable
    {
        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    984
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

        public FCSubsystemBloodBlockBehavior() : base(BlocksManager.FluidBlocks[984], true)
        {
        }

        public void Update(float dt)
        {
            if (base.SubsystemTime.PeriodicGameTimeEvent(0.3, 0.0))
            {
                base.SpreadFluid();
            }
            if (base.SubsystemTime.PeriodicGameTimeEvent(1.0, 0.25))
            {
                float num = float.MaxValue;
                foreach (Vector3 p in base.SubsystemAudio.ListenerPositions)
                {
                    float? num2 = base.CalculateDistanceToFluid(p, 8, true);
                    if (num2 != null && num2.Value < num)
                    {
                        num = num2.Value;
                    }
                }
                this.m_soundVolume = 0.5f * base.SubsystemAudio.CalculateVolume(num, 2f, 3.5f);
            }
            base.SubsystemAmbientSounds.WaterSoundVolume = MathUtils.Max(base.SubsystemAmbientSounds.WaterSoundVolume, this.m_soundVolume);
        }

        public override bool OnFluidInteract(int interactValue, int x, int y, int z, int fluidValue)
        {
            if (BlocksManager.Blocks[Terrain.ExtractContents(interactValue)] is MagmaBlock)
            {
                base.SubsystemAudio.PlayRandomSound("Audio/Sizzles", 1f, this.m_random.Float(-0.1f, 0.1f), new Vector3((float)x, (float)y, (float)z), 5f, true);
                base.SubsystemTerrain.DestroyCell(0, x, y, z, 0, false, false);
                base.Set(x, y, z, 0);
                return true;
            }
            return base.OnFluidInteract(interactValue, x, y, z, fluidValue);
        }

       

        public Random m_random = new Random();

        public float m_soundVolume;
    }
	public class FCComponentBody : ComponentBody, IUpdateable
	{
       
        public new void Update(float dt)
        {
            this.CollisionVelocityChange = Vector3.Zero;
            this.Velocity += this.m_totalImpulse;
            this.m_totalImpulse = Vector3.Zero;
            if (this.m_parentBody != null || this.m_velocity.LengthSquared() > 9.9999994E-11f || this.m_directMove != Vector3.Zero || this.m_targetCrouchFactor != this.m_crouchFactor)
            {
                this.m_stoppedTime = 0f;
            }
            else
            {
                this.m_stoppedTime += dt;
                if (this.m_stoppedTime > 0.5f && !Time.PeriodicEvent(0.25, 0.0))
                {
                    return;
                }
            }
            if (this.m_targetCrouchFactor > this.m_crouchFactor)
            {
                this.m_crouchFactor = MathUtils.Min(this.m_crouchFactor + 2f * dt, this.m_targetCrouchFactor);
            }
            if (this.m_targetCrouchFactor < this.m_crouchFactor && base.Entity.FindComponent<ComponentRider>().Mount == null)
            {
                this.m_crouchFactor = MathUtils.Max(this.m_crouchFactor - 2f * dt, this.m_targetCrouchFactor);
            }
            Vector3 position = base.Position;
            TerrainChunk chunkAtCell = this.m_subsystemTerrain.Terrain.GetChunkAtCell(Terrain.ToCell(position.X), Terrain.ToCell(position.Z));
            if (chunkAtCell == null || chunkAtCell.State <= TerrainChunkState.InvalidContents4)
            {
                this.Velocity = Vector3.Zero;
                return;
            }
            this.m_bodiesCollisionBoxes.Clear();
            this.FindBodiesCollisionBoxes(position, this.m_bodiesCollisionBoxes);
            this.m_movingBlocksCollisionBoxes.Clear();
            this.FindMovingBlocksCollisionBoxes(position, this.m_movingBlocksCollisionBoxes);
            if (!this.MoveToFreeSpace(0.56f))
            {
                this.m_crouchFactor = (this.CanCrouch ? 1f : 0f);
                this.m_targetCrouchFactor = (this.CanCrouch ? 1f : 0f);
                if (!this.MoveToFreeSpace(float.PositiveInfinity))
                {
                    ComponentHealth componentHealth = base.Entity.FindComponent<ComponentHealth>();
                    if (componentHealth != null)
                    {
                        if (this.m_crushInjureTime >= 1f)
                        {
                            componentHealth.Injure(0.15f, null, true, "Crushed");
                            this.m_crushInjureTime = 0f;
                        }
                        componentHealth.m_redScreenFactor = 1f;
                        this.m_crushInjureTime += dt;
                        return;
                    }
                    base.Project.RemoveEntity(base.Entity, true);
                    return;
                }
                else
                {
                    this.m_crushInjureTime = 1f;
                }
            }
            if (this.IsGravityEnabled)
            {
                this.m_velocity.Y = this.m_velocity.Y - 10f * dt;
                if (this.ImmersionFactor > 0f)
                {
                    float num = this.ImmersionFactor * (1f + 0.03f * MathUtils.Sin((float)MathUtils.Remainder(2.0 * this.m_subsystemTime.GameTime, 6.2831854820251465)));
                    this.m_velocity.Y = this.m_velocity.Y + 10f * (1f / this.Density * num) * dt;
                }
            }
            float num2 = MathUtils.Saturate(this.AirDrag.X * dt);
            float num3 = MathUtils.Saturate(this.AirDrag.Y * dt);
            this.m_velocity.X = this.m_velocity.X * (1f - num2);
            this.m_velocity.Y = this.m_velocity.Y * (1f - num3);
            this.m_velocity.Z = this.m_velocity.Z * (1f - num2);
            if (this.IsWaterDragEnabled && this.ImmersionFactor > 0f && this.ImmersionFluidBlock != null)
            {
                Vector2? vector = this.m_subsystemFluidBlockBehavior.CalculateFlowSpeed(Terrain.ToCell(position.X), Terrain.ToCell(position.Y), Terrain.ToCell(position.Z));
                Vector3 vector2 = (vector != null) ? new Vector3(vector.Value.X, 0f, vector.Value.Y) : Vector3.Zero;
                float num4 = 1f;
                if (this.ImmersionFluidBlock.FrictionFactor != 1f)
                {
                    num4 = ((SimplexNoise.Noise((float)MathUtils.Remainder(6.0 * Time.FrameStartTime + (double)(this.GetHashCode() % 1000), 1000.0)) > 0.5f) ? this.ImmersionFluidBlock.FrictionFactor : 1f);
                }
                float f = MathUtils.Saturate(this.WaterDrag.X * num4 * this.ImmersionFactor * dt);
                float f2 = MathUtils.Saturate(this.WaterDrag.Y * num4 * dt);
                this.m_velocity.X = MathUtils.Lerp(this.m_velocity.X, vector2.X, f);
                this.m_velocity.Y = MathUtils.Lerp(this.m_velocity.Y, vector2.Y, f2);
                this.m_velocity.Z = MathUtils.Lerp(this.m_velocity.Z, vector2.Z, f);
                if (this.m_parentBody == null && vector != null && this.StandingOnValue == null)
                {
                    if (this.WaterTurnSpeed > 0f)
                    {
                        float s = MathUtils.Saturate(MathUtils.Lerp(1f, 0f, this.m_velocity.Length()));
                        Vector2 vector3 = Vector2.Normalize(vector.Value) * s;
                        base.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.WaterTurnSpeed * (-1f * vector3.X + 0.71f * vector3.Y) * dt);
                    }
                    if (this.WaterSwayAngle > 0f)
                    {
                        base.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, this.WaterSwayAngle * (float)MathUtils.Sin((double)(200f / this.Mass) * this.m_subsystemTime.GameTime));
                    }
                }
            }
            if (this.m_parentBody != null)
            {
                Vector3 v = Vector3.Transform(this.ParentBodyPositionOffset, this.m_parentBody.Rotation) + this.m_parentBody.Position - position;
                this.m_velocity = ((dt > 0f) ? (v / dt) : Vector3.Zero);
                base.Rotation = this.ParentBodyRotationOffset * this.m_parentBody.Rotation;
            }
            this.StandingOnValue = null;
            this.StandingOnBody = null;
            this.StandingOnVelocity = Vector3.Zero;
            Vector3 velocity = this.m_velocity;
            float num5 = this.m_velocity.Length();
            if (num5 > 0f)
            {
                Vector3 stanceBoxSize = this.StanceBoxSize;
                float x = 0.45f * MathUtils.Min(stanceBoxSize.X, stanceBoxSize.Y, stanceBoxSize.Z) / num5;
                float num7;
                for (float num6 = dt; num6 > 0f; num6 -= num7)
                {
                    num7 = MathUtils.Min(num6, x);
                    this.MoveWithCollision(num7, this.m_velocity * num7 + this.m_directMove);
                    this.m_directMove = Vector3.Zero;
                }
            }
            this.CollisionVelocityChange = this.m_velocity - velocity;
            if (this.IsGroundDragEnabled && this.StandingOnValue != null)
            {
                this.m_velocity = Vector3.Lerp(this.m_velocity, this.StandingOnVelocity, 6f * dt);
            }
            if (this.StandingOnValue == null && dt != 0f)
            {
                this.TargetCrouchFactor = 0f;
            }
            this.UpdateImmersionData();
            if (this.ImmersionFluidBlock is BloodBlock && this.ImmersionDepth > 0.3f && !this.m_fluidEffectsPlayed)
            {
                this.m_fluidEffectsPlayed = true;
                this.m_subsystemAudio.PlayRandomSound("Audio/WaterFallIn", this.m_random.Float(0.75f, 1f), this.m_random.Float(-0.3f, 0f), position, 4f, true);
                this.m_subsystemParticles.AddParticleSystem(new WaterSplashParticleSystem(this.m_subsystemTerrain, position, (this.BoundingBox.Max - this.BoundingBox.Min).Length() > 0.8f));
                return;
            }
            if (this.ImmersionFluidBlock is WaterBlock && this.ImmersionDepth > 0.3f && !this.m_fluidEffectsPlayed)
            {
                this.m_fluidEffectsPlayed = true;
                this.m_subsystemAudio.PlayRandomSound("Audio/WaterFallIn", this.m_random.Float(0.75f, 1f), this.m_random.Float(-0.3f, 0f), position, 4f, true);
                this.m_subsystemParticles.AddParticleSystem(new WaterSplashParticleSystem(this.m_subsystemTerrain, position, (this.BoundingBox.Max - this.BoundingBox.Min).Length() > 0.8f));
                return;
            }
            if (this.ImmersionFluidBlock is MagmaBlock && this.ImmersionDepth > 0f && !this.m_fluidEffectsPlayed)
            {
                this.m_fluidEffectsPlayed = true;
                this.m_subsystemAudio.PlaySound("Audio/SizzleLong", 1f, 0f, position, 4f, true);
                this.m_subsystemParticles.AddParticleSystem(new MagmaSplashParticleSystem(this.m_subsystemTerrain, position, (this.BoundingBox.Max - this.BoundingBox.Min).Length() > 0.8f));
                return;
            }
            if (this.ImmersionFluidBlock == null)
            {
                this.m_fluidEffectsPlayed = false;
            }
        }


        public new void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
            
        }
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
            base.Save(valuesDictionary, entityToIdMap);
           
        }
    }
    #endregion
    #region 桶行为
	public class FCSubsystemBucketBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks
		{
			get
			{
				return new int[]
				{
					90,
					966,//樱花酒桶 979液体
					962,//血桶  984液体
				};
			}
		}

		public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
		{
			IInventory inventory = componentMiner.Inventory;
			int activeBlockValue = componentMiner.ActiveBlockValue;  //检测手上的方块，是什么。可能是空桶水桶等……
			int num = Terrain.ExtractContents(activeBlockValue);
			if (num == 90)  //如果当前活动的方块是空桶（方块索引为90），则执行以下逻辑。这里是空桶接取液体的判断
			{
				object obj = componentMiner.Raycast(ray, RaycastMode.Gathering, true, true, true); //使用Raycast方法进行射线检测，检测玩家是否与地形或实体交互，并将结果存储在obj变量中。
				if (obj is TerrainRaycastResult) //如果射线检测结果是地形交互（TerrainRaycastResult类型）。
				{
					CellFace cellFace = ((TerrainRaycastResult)obj).CellFace; //获取射线检测结果的单元格面信息，存储在cellFace变量中。
					int cellValue = base.SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z); //获取单元格的方块值，存储在cellValue变量中。
					int num2 = Terrain.ExtractContents(cellValue); //从方块值中提取出方块的内容索引，存储在num2变量中。
					int data = Terrain.ExtractData(cellValue); //从方块值中提取出方块的数据部分，存储在data变量中。
					Block block = BlocksManager.Blocks[num2]; //根据方块的内容索引获取对应的方块对象，存储在block变量中。意思就是看你点的方块到底是什么。

                    

					if (block is YHWaterBlock && FluidBlock.GetLevel(data) == 0) //樱花酒液体
					{
						int value2 = Terrain.ReplaceContents(activeBlockValue, 966);
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, inventory.GetSlotCount(inventory.ActiveSlotIndex));
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
						{
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value2, 1);
						}
						base.SubsystemTerrain.DestroyCell(0, cellFace.X, cellFace.Y, cellFace.Z, 0, false, false);
						return true;
					}
					if (block is BloodBlock && FluidBlock.GetLevel(data) == 0) //血液
					{
						int value2 = Terrain.ReplaceContents(activeBlockValue, 962);
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, inventory.GetSlotCount(inventory.ActiveSlotIndex));
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
						{
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value2, 1);
						}
						base.SubsystemTerrain.DestroyCell(0, cellFace.X, cellFace.Y, cellFace.Z, 0, false, false);
						return true;
					}
				}
                
			}

			if (num == 966) //如果是樱花酒桶
			{
				TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Interaction, true, true, true);
				if (terrainRaycastResult != null && componentMiner.Place(terrainRaycastResult.Value, Terrain.MakeBlockValue(979)))
				{
					inventory.RemoveSlotItems(inventory.ActiveSlotIndex, 1);
					if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
					{
						int value4 = Terrain.ReplaceContents(activeBlockValue, 90); //换成空桶
						inventory.AddSlotItems(inventory.ActiveSlotIndex, value4, 1);
					}
					return true;
				}
			}
			if (num == 962) //如果是樱花酒桶
			{
				TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Interaction, true, true, true);
				if (terrainRaycastResult != null && componentMiner.Place(terrainRaycastResult.Value, Terrain.MakeBlockValue(984)))
				{
					inventory.RemoveSlotItems(inventory.ActiveSlotIndex, 1);
					if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
					{
						int value4 = Terrain.ReplaceContents(activeBlockValue, 90); //换成空桶
						inventory.AddSlotItems(inventory.ActiveSlotIndex, value4, 1);
					}
					return true;
				}
			}

			return true;
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.m_subsystemAudio = base.Project.FindSubsystem<SubsystemAudio>(true);
			this.m_subsystemParticles = base.Project.FindSubsystem<SubsystemParticles>(true);
		}

		public SubsystemAudio m_subsystemAudio;

		public SubsystemParticles m_subsystemParticles;

		public Random m_random = new Random();
	}
    #endregion

    #region FC配方管理器
    /*public class ComponentGameSystem1 : ComponentInventoryBase, IUpdateable
	{
		UpdateOrder IUpdateable.UpdateOrder => ((IUpdateable)componentPlayer).UpdateOrder;

		public ComponentPlayer Player;

		public ButtonWidget AButton;//获取按钮

		public SubsystemSky subsystemSky;

		public SubsystemBodies m_subsystemBodies;

		public DynamicArray<ComponentBody> m_componentBodies = new DynamicArray<ComponentBody>();

		public ComponentPlayer componentPlayer;

		public SubsystemBlockEntities Entities;

		public ComponentOnFire componentOnFire;

		public ComponentMiner componentMiner;

		public bool flag = false;

		public bool flag2 = false;

		public bool flag3 = false;

		public bool flag4 = false;

		public SubsystemTime subsystemTime;

		public ComponentHealth componentHealth;

		public ComponentLocomotion componentLocomotion;

		public ComponentBody componentBody;

		public float m_frequency = 0.5f;

		public SubsystemGameInfo m_subsystemGameInfo;

		public ComponentCreature m_componentCreature;

		public SubsystemPlayers m_subsystemPlayers;

		public void Update(float dt)
		{
			Vector3 playerPosition = componentPlayer.ComponentBody.Position;

			float Velocity2 = (float)componentLocomotion.WalkSpeed;
			if ((AButton.IsClicked || Keyboard.IsKeyDownOnce(Key.O)))//通过该参数启动按钮
			{
				componentPlayer.ComponentGui.ModalPanelWidget = new FCRecipeWidget();//打开一个界面
			}
		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			Player = Entity.FindComponent<ComponentPlayer>();
			componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
			componentMiner = Entity.FindComponent<ComponentMiner>(true);
			componentHealth = Entity.FindComponent<ComponentHealth>(true);
			componentLocomotion = Entity.FindComponent<ComponentLocomotion>(true);
			subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
			m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
			m_subsystemPlayers = Project.FindSubsystem<SubsystemPlayers>(true);
			componentBody = Entity.FindComponent<ComponentBody>(true);
			m_subsystemBodies = Project.FindSubsystem<SubsystemBodies>(true);
			subsystemSky = Project.FindSubsystem<SubsystemSky>(true);
			base.Load(valuesDictionary, idToEntityMap);
			try
			{
				AButton = Player.ViewWidget.GameWidget.Children.Find<ButtonWidget>("AButton", true);//注册
			}
			catch
			{
				AButton = new BevelledButtonWidget
				{
					Name = "AButton",
					Text = "食品工艺",//游戏中显示
					Font = ContentManager.Get<BitmapFont>("Fonts/Pericles"),//获取字体
					Size = new Vector2(110f, 60f),//尺码
					IsEnabled = true
				};
				Player.ViewWidget.GameWidget.Children.Find<StackPanelWidget>("RightControlsContainer", true).Children.Add(AButton);
				//RightControlsContainer为在gui中的位置
				//分别还有三个位置 如下
				//MoreContents为…按钮弹出按钮键的位置
				//RightControlsContainer为gui右下角                
				//LeftControlsContainer为gui左下角
			}
		}
	}*/

    public static class FCNewCraftingRecipesManager
    {
        public static List<CraftingRecipe> m_recipes = new List<CraftingRecipe>();

        public static ReadOnlyList<CraftingRecipe> Recipes => new ReadOnlyList<CraftingRecipe>(m_recipes);

        public static void Initialize()
        {
            foreach (XElement item in ContentManager.Get<XElement>("FCNewCraftingRecipes").Descendants("Recipe"))
            {
                var craftingRecipe = new CraftingRecipe();
                craftingRecipe.Ingredients = new string[16];
                string attributeValue = XmlUtils.GetAttributeValue<string>(item, "Result");
                string desc = XmlUtils.GetAttributeValue<string>(item, "Description");
                if (desc.StartsWith("[") && desc.EndsWith("]") && LanguageControl.TryGetBlock(attributeValue, "CRDescription:" + desc.Substring(1, desc.Length - 2), out var r)) desc = r;
                craftingRecipe.ResultValue = DecodeResult(attributeValue);
                craftingRecipe.ResultCount = XmlUtils.GetAttributeValue<int>(item, "ResultCount");
                string attributeValue2 = XmlUtils.GetAttributeValue(item, "Remains", string.Empty);
                if (!string.IsNullOrEmpty(attributeValue2))
                {
                    craftingRecipe.RemainsValue = DecodeResult(attributeValue2);
                    craftingRecipe.RemainsCount = XmlUtils.GetAttributeValue<int>(item, "RemainsCount");
                }
                craftingRecipe.RequiredHeatLevel = XmlUtils.GetAttributeValue<float>(item, "RequiredHeatLevel");
                craftingRecipe.RequiredPlayerLevel = XmlUtils.GetAttributeValue(item, "RequiredPlayerLevel", 1f);
                craftingRecipe.Description = desc;
                craftingRecipe.Message = XmlUtils.GetAttributeValue<string>(item, "Message", null);
                if (craftingRecipe.ResultCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.ResultValue)].GetMaxStacking(craftingRecipe.ResultValue))
                {
                    throw new InvalidOperationException($"In recipe for \"{attributeValue}\" ResultCount is larger than max stacking of result block.");
                }
                if (craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue))
                {
                    throw new InvalidOperationException($"In Recipe for \"{attributeValue2}\" RemainsCount is larger than max stacking of remains block.");
                }
                var dictionary = new Dictionary<char, string>();
                foreach (XAttribute item2 in from a in item.Attributes()
                                             where a.Name.LocalName.Length == 1 && char.IsLower(a.Name.LocalName[0])
                                             select a)
                {
                    DecodeIngredient(item2.Value, out string craftingId, out int? data);
                    if (BlocksManager.FindBlocksByCraftingId(craftingId).Length == 0)
                    {
                        throw new InvalidOperationException($"Block with craftingId \"{item2.Value}\" not found.");
                    }
                    if (data.HasValue && (data.Value < 0 || data.Value > 262143))
                    {
                        throw new InvalidOperationException($"Data in recipe ingredient \"{item2.Value}\" must be between 0 and 0x3FFFF.");
                    }
                    dictionary.Add(item2.Name.LocalName[0], item2.Value);
                }
                string[] array = item.Value.Trim().Split(new string[] { "\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    int num = array[i].IndexOf('"');
                    int num2 = array[i].LastIndexOf('"');
                    if (num < 0 || num2 < 0 || num2 <= num)
                    {
                        throw new InvalidOperationException("Invalid recipe line.");
                    }
                    string text = array[i].Substring(num + 1, num2 - num - 1);
                    for (int j = 0; j < text.Length; j++)
                    {
                        char c = text[j];
                        if (char.IsLower(c))
                        {
                            string text2 = dictionary[c];
                            craftingRecipe.Ingredients[j + i * 4] = text2;
                        }
                    }
                }

                m_recipes.Add(craftingRecipe);
                /*
				Block[] blocks = BlocksManager.Blocks;
				foreach (Block block in blocks)
				{
					m_recipes.AddRange(block.GetProceduralCraftingRecipes());
				}
				*/
                m_recipes.Sort(delegate (CraftingRecipe r1, CraftingRecipe r2)
                {
                    int y = r1.Ingredients.Count((string s) => !string.IsNullOrEmpty(s));
                    int x = r2.Ingredients.Count((string s) => !string.IsNullOrEmpty(s));
                    return Comparer<int>.Default.Compare(x, y);
                });
            }
        }

        public static CraftingRecipe FindMatchingRecipe(SubsystemTerrain terrain, string[] ingredients, float heatLevel, float playerLevel)
        {
            CraftingRecipe craftingRecipe = null;
            if (craftingRecipe != null)
            {
                if (heatLevel < craftingRecipe.RequiredHeatLevel)
                {
                    craftingRecipe = null;
                }

            }
            /*
            Block[] blocks = BlocksManager.Blocks;
            for (int i = 0; i < blocks.Length; i++)
            {
                CraftingRecipe adHocCraftingRecipe = blocks[i].GetAdHocCraftingRecipe(terrain, ingredients, heatLevel, playerLevel);
                if (adHocCraftingRecipe != null && MatchRecipe(adHocCraftingRecipe.Ingredients, ingredients))
                {
                    craftingRecipe = adHocCraftingRecipe;
                    break;
                }
            }
            */
            if (craftingRecipe == null)
            {
                foreach (CraftingRecipe recipe in Recipes)
                {
                    if (MatchRecipe(recipe.Ingredients, ingredients))
                    {
                        craftingRecipe = recipe;
                        break;
                    }
                }
            }


                /*if (heatLevel < craftingRecipe.RequiredHeatLevel)
				 {
					 craftingRecipe = ((!(heatLevel > 0f)) ? new CraftingRecipe
					 {
						 Message = "使用熔炉来熔炼这个物品"
					 } : new CraftingRecipe
					 {
						 Message = "需要更高温度来熔炼这个物品，可通过寻找更好的燃料解决"
					 });
				 }
				 else if (playerLevel < craftingRecipe.RequiredPlayerLevel)
				 {
					 craftingRecipe = ((!(craftingRecipe.RequiredHeatLevel > 0f)) ? new CraftingRecipe
					 {
						 Message = String.Format("你需要达到等级{0}才能制作这个物品", craftingRecipe.RequiredPlayerLevel)
					 } : new CraftingRecipe
					 {
						 Message = String.Format("你需要达到等级{0}才能熔炼这个物品", craftingRecipe.RequiredPlayerLevel)
					 });
				 }*/
				 
            return craftingRecipe;
        }

        public static bool MatchRecipe(string[] requiredIngredients, string[] actualIngredient)
        {
            if (requiredIngredients.Length <= 9 || actualIngredient.Length <= 9)
            {
                return BaseMatchRecipe(requiredIngredients, actualIngredient);
            }
            string[] array = new string[16];
            for (int i = 0; i < 2; i++)
            {
                for (int j = -4; j <= 4; j++)
                {
                    for (int k = -4; k <= 4; k++)
                    {
                        bool flip = (i != 0) ? true : false;
                        if (!TransformRecipe(array, requiredIngredients, k, j, flip))
                        {
                            continue;
                        }
                        bool flag = true;
                        for (int l = 0; l < 4 * 4; l++)
                        {
                            if (l == actualIngredient.Length || !CompareIngredients(array[l], actualIngredient[l]))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool BaseMatchRecipe(string[] requiredIngredients, string[] actualIngredients)
        {
            string[] array = new string[9];
            for (int i = 0; i < 2; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    for (int k = -3; k <= 3; k++)
                    {
                        bool flip = (i != 0) ? true : false;
                        if (!TransformRecipe(array, requiredIngredients, k, j, flip))
                        {
                            continue;
                        }
                        bool flag = true;
                        for (int l = 0; l < 9; l++)
                        {
                            if (l == actualIngredients.Length || !CompareIngredients(array[l], actualIngredients[l]))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static int DecodeResult(string result)
        {
            string[] array = result.Split((new char[1] { ':' }));
            Block block = BlocksManager.FindBlockByTypeName(array[0], true);
            return Terrain.MakeBlockValue(data: (array.Length >= 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0, contents: block.BlockIndex, light: 0);
        }

        public static void DecodeIngredient(string ingredient, out string craftingId, out int? data)
        {
            string[] array = ingredient.Split((new char[1] { ':' }));
            craftingId = array[0];
            data = ((array.Length >= 2) ? new int?(int.Parse(array[1], CultureInfo.InvariantCulture)) : null);
        }

        public static bool TransformRecipe(string[] transformedIngredients, string[] ingredients, int shiftX, int shiftY, bool flip)
        {
            for (int i = 0; i < 16; i++)
            {
                transformedIngredients[i] = null;
            }
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int num = (flip ? (4 - k - 1) : k) + shiftX;
                    int num2 = j + shiftY;
                    string text = ingredients[k + j * 4];
                    if (num >= 0 && num2 >= 0 && num < 4 && num2 < 4)
                    {
                        transformedIngredients[num + num2 * 4] = text;
                    }
                    else if (!string.IsNullOrEmpty(text))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool CompareIngredients(string requiredIngredient, string actualIngredient)
        {
            if (requiredIngredient == null)
            {
                return actualIngredient == null;
            }
            if (actualIngredient == null)
            {
                return requiredIngredient == null;
            }
            DecodeIngredient(requiredIngredient, out string craftingId, out int? data);
            DecodeIngredient(actualIngredient, out string craftingId2, out int? data2);
            if (!data2.HasValue)
            {
                throw new InvalidOperationException("Actual ingredient data not specified.");
            }
            if (craftingId == craftingId2)
            {
                if (!data.HasValue)
                {
                    return true;
                }
                return data.Value == data2.Value;
            }
            return false;
        }
    }
    #endregion
    #region 合成表

    public class FCRecipeWidget : CanvasWidget//打开配方表
	{
		internal class Order
		{
			public Block block;
			public int order;
			public int value;
			public Order(Block b, int o, int v) { block = b; order = o; value = v; }
		}
		public ListPanelWidget m_blocksList;

		public Screen m_previousScreen;

		public XSmeltingRecipeWidget m_smeltingRecipeWidget;

		public XCraftingRecipeWidget m_craftingRecipeWidget;

		public List<string> m_categories = new List<string>();

		public int m_categoryIndex;

		public int m_listCategoryIndex = -1;

		public ButtonWidget m_prevCategoryButton;

		public ButtonWidget m_nextCategoryButton;

		public ButtonWidget m_prevRecipeButton;

		public ButtonWidget m_nextRecipeButton;

		public int m_recipeIndex;

		public List<CraftingRecipe> m_craftingRecipes = new List<CraftingRecipe>();

		public FCRecipeWidget()
		{
			XElement node = ContentManager.Get<XElement>("Widgets/FCRecipeWidget");
			LoadContents(this, node);
			m_blocksList = Children.Find<ListPanelWidget>("BlocksList");
			m_smeltingRecipeWidget = Children.Find<XSmeltingRecipeWidget>("SmeltingRecipe");//这是熔融配方的界面
            m_craftingRecipeWidget = Children.Find<XCraftingRecipeWidget>("CraftingRecipe");//这是总合成表里面的子合成表组件界面
            m_prevRecipeButton = Children.Find<ButtonWidget>("PreviousRecipe");
			m_nextRecipeButton = Children.Find<ButtonWidget>("NextRecipe");
			m_prevCategoryButton = Children.Find<ButtonWidget>("PreviousCategory");
			m_nextCategoryButton = Children.Find<ButtonWidget>("NextCategory");
			m_categories.Add(null);
			m_categories.AddRange(BlocksManager.Categories);
			m_blocksList.ItemWidgetFactory = delegate (object item)
			{
				int value = (int)item;
				int num = Terrain.ExtractContents(value);
				Block block = BlocksManager.Blocks[num];
				XElement node2 = ContentManager.Get<XElement>("Widgets/RecipaediaItem");
				var obj = (ContainerWidget)LoadWidget(this, node2, null);
				obj.Children.Find<BlockIconWidget>("RecipaediaItem.Icon").Value = value;
				obj.Children.Find<LabelWidget>("RecipaediaItem.Text").Text = block.GetDisplayName(null, value);
				return obj;
			};
			m_blocksList.ItemClicked += delegate (object item)
			{
				int value = (int)item;
				m_recipeIndex = 0;
				m_craftingRecipes.Clear();
				m_craftingRecipes.AddRange(FCNewCraftingRecipesManager.Recipes.Where((CraftingRecipe r) => r.ResultValue == value && r.ResultValue != 0));
			};
		}

		public override void Update()
		{
			if (m_listCategoryIndex != m_categoryIndex)
			{
				PopulateBlocksList();
			}
			m_prevCategoryButton.IsEnabled = (m_categoryIndex > 0);
			m_nextCategoryButton.IsEnabled = (m_categoryIndex < m_categories.Count - 1);
			int? value = null;
			int num = 0;
			if (m_blocksList.SelectedItem is int)
			{
				value = (int)m_blocksList.SelectedItem;
				num = FCNewCraftingRecipesManager.Recipes.Count((CraftingRecipe r) => r.ResultValue == value);
			}
			if (m_prevCategoryButton.IsClicked || Input.Left)
			{
				m_categoryIndex = MathUtils.Max(m_categoryIndex - 1, 0);
			}
			if (m_nextCategoryButton.IsClicked || Input.Right)
			{
				m_categoryIndex = MathUtils.Min(m_categoryIndex + 1, m_categories.Count - 1);
			}
			m_prevRecipeButton.IsEnabled = (m_recipeIndex > 0);
			m_nextRecipeButton.IsEnabled = (m_recipeIndex < m_craftingRecipes.Count - 1);
			if (m_prevRecipeButton.IsClicked)
			{
				m_recipeIndex = MathUtils.Max(m_recipeIndex - 1, 0);
			}
			if (m_nextRecipeButton.IsClicked)
			{
				m_recipeIndex = MathUtils.Min(m_recipeIndex + 1, m_craftingRecipes.Count - 1);
			}
			if (m_recipeIndex < m_craftingRecipes.Count)
			{
				CraftingRecipe craftingRecipe = m_craftingRecipes[m_recipeIndex];
				if (craftingRecipe.RequiredHeatLevel == 0f)
				{
					m_craftingRecipeWidget.Recipe = craftingRecipe;
					m_craftingRecipeWidget.NameSuffix = string.Format(LanguageControl.GetContentWidgets(GetType().Name, 1), m_recipeIndex + 1);
					m_craftingRecipeWidget.IsVisible = true;
					m_smeltingRecipeWidget.IsVisible = false;
				}
				else
				{
					m_smeltingRecipeWidget.Recipe = craftingRecipe;
					m_smeltingRecipeWidget.NameSuffix = string.Format(LanguageControl.GetContentWidgets(GetType().Name, 1), m_recipeIndex + 1);
					m_smeltingRecipeWidget.IsVisible = true;
					m_craftingRecipeWidget.IsVisible = false;
				}
			}
		}

		public void PopulateBlocksList()
		{
			m_listCategoryIndex = m_categoryIndex;
			string text = m_categories[m_categoryIndex];
			m_blocksList.ScrollPosition = 0f;
			m_blocksList.ClearItems();
			List<Order> orders = new List<Order>();
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					if (string.IsNullOrEmpty(text) || item.GetCategory(creativeValue) == text) orders.Add(new Order(item, item.GetDisplayOrder(creativeValue), creativeValue));
				}
			}
			var orderList = orders.OrderBy(o => o.order);
			foreach (var c in orderList)
			{
				m_blocksList.AddItem(c.value);
			}
		}
	}
    #endregion
    #endregion
}//原始命名空间












namespace Game
{
	#region 附魔台炼药锅
	#region ModLoader
	
	#endregion
	public class XClothingWidget : CanvasWidget
	{
		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_handGrid;

		public StackPanelWidget m_clothingStack;

		public ButtonWidget m_vitalStatsButton;

		public ButtonWidget m_sleepButton;

		public ButtonWidget m_recipesButton;

		public PlayerModelWidget m_innerClothingModelWidget;

		public PlayerModelWidget m_outerClothingModelWidget;

		public ComponentPlayer m_componentPlayer;

		public XClothingWidget(ComponentPlayer componentPlayer)
		{
			m_componentPlayer = componentPlayer;
			XElement node = ContentManager.Get<XElement>("Widgets/XClothingWidget");
			LoadContents(this, node);
			m_clothingStack = Children.Find<StackPanelWidget>("ClothingStack");
			m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
			m_handGrid = Children.Find<GridPanelWidget>("HandGrid");
			m_vitalStatsButton = Children.Find<ButtonWidget>("VitalStatsButton");
			m_sleepButton = Children.Find<ButtonWidget>("SleepButton");
			m_recipesButton = Children.Find<ButtonWidget>("RecipesButton");
			m_innerClothingModelWidget = Children.Find<PlayerModelWidget>("InnerClothingModel");
			m_outerClothingModelWidget = Children.Find<PlayerModelWidget>("OuterClothingModel");
			for (int i = 0; i < 4; i++)
			{
				var inventorySlotWidget = new InventorySlotWidget();
				float y = float.PositiveInfinity;
				if (i == 0)
				{
					y = 68f;
				}
				if (i == 3)
				{
					y = 54f;
				}
				inventorySlotWidget.Size = new Vector2(float.PositiveInfinity, y);
				inventorySlotWidget.BevelColor = Color.Transparent;
				inventorySlotWidget.CenterColor = Color.Transparent;
				inventorySlotWidget.AssignInventorySlot(m_componentPlayer.ComponentClothing, i);
				inventorySlotWidget.HideEditOverlay = true;
				inventorySlotWidget.HideInteractiveOverlay = true;
				inventorySlotWidget.HideFoodOverlay = true;
				inventorySlotWidget.HideHighlightRectangle = true;
				inventorySlotWidget.HideBlockIcon = true;
				inventorySlotWidget.HideHealthBar = (m_componentPlayer.Project.FindSubsystem<SubsystemGameInfo>(throwOnError: true).WorldSettings.GameMode == GameMode.Creative);
				m_clothingStack.Children.Add(inventorySlotWidget);
			}
			int num = 10;
			for (int j = 0; j < m_inventoryGrid.RowsCount; j++)
			{
				for (int k = 0; k < m_inventoryGrid.ColumnsCount; k++)
				{
					var inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(componentPlayer.ComponentMiner.Inventory, num++);
					m_inventoryGrid.Children.Add(inventorySlotWidget2);
					m_inventoryGrid.SetWidgetCell(inventorySlotWidget2, new Point2(k, j));
				}
			}
			m_innerClothingModelWidget.PlayerClass = componentPlayer.PlayerData.PlayerClass;
			m_innerClothingModelWidget.CharacterSkinTexture = m_componentPlayer.ComponentClothing.InnerClothedTexture;
			m_outerClothingModelWidget.PlayerClass = componentPlayer.PlayerData.PlayerClass;
			m_outerClothingModelWidget.OuterClothingTexture = m_componentPlayer.ComponentClothing.OuterClothedTexture;
		}

		public override void Update()
		{

			if (m_recipesButton.IsClicked)
			{
				m_componentPlayer.ComponentGui.ModalPanelWidget = new XRecipeWidget();
			}
			if (m_sleepButton.IsClicked && m_componentPlayer != null)
			{
				if (!m_componentPlayer.ComponentSleep.CanSleep(out string reason))
				{
					m_componentPlayer.ComponentGui.DisplaySmallMessage(reason, Color.White, blinking: false, playNotificationSound: false);
				}
				else
				{
					m_componentPlayer.ComponentSleep.Sleep(allowManualWakeup: true);
				}
			}
		}
	}
	#region 合成展示
	public class XRecipeWidget : CanvasWidget
	{
		internal class Order
		{
			public Block block;
			public int order;
			public int value;
			public Order(Block b, int o, int v) { block = b; order = o; value = v; }
		}
		public ListPanelWidget m_blocksList;

		public Screen m_previousScreen;

		public XSmeltingRecipeWidget m_smeltingRecipeWidget;

		public XCraftingRecipeWidget m_craftingRecipeWidget;

		public List<string> m_categories = new List<string>();

		public int m_categoryIndex;

		public int m_listCategoryIndex = -1;

		public ButtonWidget m_prevCategoryButton;

		public ButtonWidget m_nextCategoryButton;

		public ButtonWidget m_prevRecipeButton;

		public ButtonWidget m_nextRecipeButton;

		public int m_recipeIndex;

		public List<CraftingRecipe> m_craftingRecipes = new List<CraftingRecipe>();

		public XRecipeWidget()
		{
			XElement node = ContentManager.Get<XElement>("Widgets/XRecipeWidget");
			LoadContents(this, node);
			m_blocksList = Children.Find<ListPanelWidget>("BlocksList");
			m_smeltingRecipeWidget = Children.Find<XSmeltingRecipeWidget>("SmeltingRecipe");
			m_craftingRecipeWidget = Children.Find<XCraftingRecipeWidget>("CraftingRecipe");
			m_prevRecipeButton = Children.Find<ButtonWidget>("PreviousRecipe");
			m_nextRecipeButton = Children.Find<ButtonWidget>("NextRecipe");
			m_prevCategoryButton = Children.Find<ButtonWidget>("PreviousCategory");
			m_nextCategoryButton = Children.Find<ButtonWidget>("NextCategory");
			m_categories.Add(null);
			m_categories.AddRange(BlocksManager.Categories);
			m_blocksList.ItemWidgetFactory = delegate (object item)
			{
				int value = (int)item;
				int num = Terrain.ExtractContents(value);
				Block block = BlocksManager.Blocks[num];
				XElement node2 = ContentManager.Get<XElement>("Widgets/RecipaediaItem");
				var obj = (ContainerWidget)LoadWidget(this, node2, null);
				obj.Children.Find<BlockIconWidget>("RecipaediaItem.Icon").Value = value;
				obj.Children.Find<LabelWidget>("RecipaediaItem.Text").Text = block.GetDisplayName(null, value);
				return obj;
			};
			m_blocksList.ItemClicked += delegate (object item)
			{
				int value = (int)item;
				m_recipeIndex = 0;
				m_craftingRecipes.Clear();
				m_craftingRecipes.AddRange(XCraftingRecipesManager.Recipes.Where((CraftingRecipe r) => r.ResultValue == value && r.ResultValue != 0));
			};
		}

		public override void Update()
		{
			if (m_listCategoryIndex != m_categoryIndex)
			{
				PopulateBlocksList();
			}
			m_prevCategoryButton.IsEnabled = (m_categoryIndex > 0);
			m_nextCategoryButton.IsEnabled = (m_categoryIndex < m_categories.Count - 1);
			int? value = null;
			int num = 0;
			if (m_blocksList.SelectedItem is int)
			{
				value = (int)m_blocksList.SelectedItem;
				num = XCraftingRecipesManager.Recipes.Count((CraftingRecipe r) => r.ResultValue == value);
			}
			if (m_prevCategoryButton.IsClicked || Input.Left)
			{
				m_categoryIndex = MathUtils.Max(m_categoryIndex - 1, 0);
			}
			if (m_nextCategoryButton.IsClicked || Input.Right)
			{
				m_categoryIndex = MathUtils.Min(m_categoryIndex + 1, m_categories.Count - 1);
			}
			m_prevRecipeButton.IsEnabled = (m_recipeIndex > 0);
			m_nextRecipeButton.IsEnabled = (m_recipeIndex < m_craftingRecipes.Count - 1);
			if (m_prevRecipeButton.IsClicked)
			{
				m_recipeIndex = MathUtils.Max(m_recipeIndex - 1, 0);
			}
			if (m_nextRecipeButton.IsClicked)
			{
				m_recipeIndex = MathUtils.Min(m_recipeIndex + 1, m_craftingRecipes.Count - 1);
			}
			if (m_recipeIndex < m_craftingRecipes.Count)
			{
				CraftingRecipe craftingRecipe = m_craftingRecipes[m_recipeIndex];
				if (craftingRecipe.RequiredHeatLevel == 0f)
				{
					m_craftingRecipeWidget.Recipe = craftingRecipe;
					m_craftingRecipeWidget.NameSuffix = string.Format(LanguageControl.GetContentWidgets(GetType().Name, 1), m_recipeIndex + 1);
					m_craftingRecipeWidget.IsVisible = true;
					m_smeltingRecipeWidget.IsVisible = false;
				}
				else
				{
					m_smeltingRecipeWidget.Recipe = craftingRecipe;
					m_smeltingRecipeWidget.NameSuffix = string.Format(LanguageControl.GetContentWidgets(GetType().Name, 1), m_recipeIndex + 1);
					m_smeltingRecipeWidget.IsVisible = true;
					m_craftingRecipeWidget.IsVisible = false;
				}
			}
		}

		public void PopulateBlocksList()
		{
			m_listCategoryIndex = m_categoryIndex;
			string text = m_categories[m_categoryIndex];
			m_blocksList.ScrollPosition = 0f;
			m_blocksList.ClearItems();
			List<Order> orders = new List<Order>();
			foreach (Block item in BlocksManager.Blocks)
			{
				foreach (int creativeValue in item.GetCreativeValues())
				{
					if (string.IsNullOrEmpty(text) || item.GetCategory(creativeValue) == text) orders.Add(new Order(item, item.GetDisplayOrder(creativeValue), creativeValue));
				}
			}
			var orderList = orders.OrderBy(o => o.order);
			foreach (var c in orderList)
			{
				m_blocksList.AddItem(c.value);
			}
		}
	}
	#endregion
	#region 大工作台 981
	public class XCraftingTableBlock : Block
	{
		public const int Index = 981;

		public BlockMesh m_blockMesh = new BlockMesh();

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();

		private Texture2D m_texture;
		public override void Initialize()
		{
			Model model = ContentManager.Get<Model>("Models/XCraftingTable");
			Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("CraftingTable").ParentBone);
			m_blockMesh.AppendModelMeshPart(model.FindMesh("CraftingTable").MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(0.5f, 0.7f, -0.245f), makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, Color.White);
			m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("CraftingTable").MeshParts[0], boneAbsoluteTransform * Matrix.CreateTranslation(-0.4f, 0.4f, -0.6f), makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, Color.White);
			m_texture = ContentManager.Get<Texture2D>("Textures/FCBlocks/fumotai", null);  //外置材质
			base.Initialize();
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateShadedMeshVertices(this, x, y, z, this.m_blockMesh, Color.White, null, null, geometry.GetGeometry(m_texture).SubsetOpaque);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, m_texture, color, size, ref matrix, environmentData);
		}
	}

	public class SubsystemXCraftingTableBlockBehavior : SubsystemBlockBehavior
	{
		public override int[] HandledBlocks => new int[1]
		{
			981
		};

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			DatabaseObject databaseObject = base.SubsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("XCraftingTable", base.SubsystemTerrain.Project.GameDatabase.EntityTemplateType, true);
			ValuesDictionary valuesDictionary = new ValuesDictionary();
			valuesDictionary.PopulateFromDatabaseObject(databaseObject);
			valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", new Point3(x, y, z));
			Entity entity = base.SubsystemTerrain.Project.CreateEntity(valuesDictionary);
			base.SubsystemTerrain.Project.AddEntity(entity);
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(x, y, z);
			if (blockEntity != null)
			{
				Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
				foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
				{
					item.DropAllItems(position);
				}
				base.SubsystemTerrain.Project.RemoveEntity(blockEntity.Entity, true);
			}
		}

		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(true).GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				ComponentXCraftingTable componentCraftingTable = blockEntity.Entity.FindComponent<ComponentXCraftingTable>(true);
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new XCraftingTableWidget(componentMiner.Inventory, componentCraftingTable, componentMiner.ComponentPlayer);
				AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
				return true;
			}
			return false;
		}
	}
	public class XCraftingTableWidget : CanvasWidget
	{
		private GridPanelWidget m_inventoryGrid;

		public ButtonWidget m_recipesButton;

		private GridPanelWidget m_craftingGrid;

		private InventorySlotWidget m_craftingResultSlot;

		private InventorySlotWidget m_craftingRemainsSlot;

		private ComponentXCraftingTable m_componentCraftingTable;

		public ComponentPlayer m_componentPlayer;



		public XCraftingTableWidget(IInventory inventory, ComponentXCraftingTable componentCraftingTable, ComponentPlayer componentPlayer)
		{

			m_componentPlayer = componentPlayer;
			m_componentCraftingTable = componentCraftingTable;
			XElement node = ContentManager.Get<XElement>("Widgets/XCraftingTableWidget");
			LoadContents(this, node);
			m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
			m_craftingGrid = Children.Find<GridPanelWidget>("CraftingGrid");
			m_craftingResultSlot = Children.Find<InventorySlotWidget>("CraftingResultSlot");
			m_craftingRemainsSlot = Children.Find<InventorySlotWidget>("CraftingRemainsSlot");
			m_recipesButton = Children.Find<ButtonWidget>("RecipesButton");
			
			int num = 10;
			for (int i = 0; i < m_inventoryGrid.RowsCount; i++)
			{
				for (int j = 0; j < m_inventoryGrid.ColumnsCount; j++)
				{
					InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
					inventorySlotWidget.AssignInventorySlot(inventory, num++);
					m_inventoryGrid.Children.Add(inventorySlotWidget);
					m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
				}
			}
			num = 0;
			for (int k = 0; k < m_craftingGrid.RowsCount; k++)
			{
				for (int l = 0; l < m_craftingGrid.ColumnsCount; l++)
				{
					InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget();
					inventorySlotWidget2.AssignInventorySlot(m_componentCraftingTable, num++);
					inventorySlotWidget2.Size = new Vector2(50f, 50f);
					m_craftingGrid.Children.Add(inventorySlotWidget2);
					m_craftingGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			m_craftingResultSlot.AssignInventorySlot(m_componentCraftingTable, m_componentCraftingTable.ResultSlotIndex);
			m_craftingRemainsSlot.AssignInventorySlot(m_componentCraftingTable, m_componentCraftingTable.RemainsSlotIndex);
		}

		public override void Update()
		{
			
			if (!m_componentCraftingTable.IsAddedToProject)
			{
				base.ParentWidget.Children.Remove(this);
			}
			if (m_recipesButton.IsClicked)
			{
				m_componentPlayer.ComponentGui.ModalPanelWidget = new XRecipeWidget();
			}

		}
	}

	public class ComponentXCraftingTable : ComponentInventoryBase
	{
		private int m_craftingGridSize;

		private string[] m_matchedIngredients = new string[16];

		private CraftingRecipe m_matchedRecipe;

		public int RemainsSlotIndex => SlotsCount - 1;

		public int ResultSlotIndex => SlotsCount - 2;

		public override int GetSlotCapacity(int slotIndex, int value)
		{
			if (slotIndex < SlotsCount - 2)
			{
				return base.GetSlotCapacity(slotIndex, value);
			}
			return 0;
		}

		public override void AddSlotItems(int slotIndex, int value, int count)
		{
			base.AddSlotItems(slotIndex, value, count);
			UpdateCraftingResult();
		}

		public override int RemoveSlotItems(int slotIndex, int count)
		{
			int num = 0;
			if (slotIndex == ResultSlotIndex)
			{
				if (m_matchedRecipe != null)
				{
					if (m_matchedRecipe.RemainsValue != 0 && m_matchedRecipe.RemainsCount > 0)
					{
						if (m_slots[RemainsSlotIndex].Count == 0 || m_slots[RemainsSlotIndex].Value == m_matchedRecipe.RemainsValue)
						{
							int num2 = BlocksManager.Blocks[Terrain.ExtractContents(m_matchedRecipe.RemainsValue)].MaxStacking - m_slots[RemainsSlotIndex].Count;
							count = MathUtils.Min(count, num2 / m_matchedRecipe.RemainsCount);
						}
						else
						{
							count = 0;
						}
					}
					count = count / m_matchedRecipe.ResultCount * m_matchedRecipe.ResultCount;
					num = base.RemoveSlotItems(slotIndex, count);
					if (num > 0)
					{
						for (int i = 0; i < 16; i++)
						{
							if (!string.IsNullOrEmpty(m_matchedIngredients[i]))
							{
								int index = i % 4 + m_craftingGridSize * (i / 4);
								m_slots[index].Count = MathUtils.Max(m_slots[index].Count - num / m_matchedRecipe.ResultCount, 0);
							}
						}
						if (m_matchedRecipe.RemainsValue != 0 && m_matchedRecipe.RemainsCount > 0)
						{
							m_slots[RemainsSlotIndex].Value = m_matchedRecipe.RemainsValue;
							m_slots[RemainsSlotIndex].Count += num / m_matchedRecipe.ResultCount * m_matchedRecipe.RemainsCount;
						}
						ComponentPlayer componentPlayer = base.Entity.FindComponent<ComponentPlayer>();
						if (componentPlayer == null)
						{
							ComponentBlockEntity componentBlockEntity = base.Entity.FindComponent<ComponentBlockEntity>();
							if (componentBlockEntity != null)
							{
								Vector3 position = new Vector3(componentBlockEntity.Coordinates);
								componentPlayer = base.Project.FindSubsystem<SubsystemPlayers>(true).FindNearestPlayer(position);
							}
						}
						if (componentPlayer != null && componentPlayer.PlayerStats != null)
						{
							componentPlayer.PlayerStats.ItemsCrafted += num;
						}
					}
				}
			}
			else
			{
				num = base.RemoveSlotItems(slotIndex, count);
			}
			UpdateCraftingResult();
			return num;
		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_craftingGridSize = (int)MathUtils.Sqrt(SlotsCount - 2);
			if (m_craftingGridSize < 1 || m_craftingGridSize > 4)
			{
				throw new InvalidOperationException("Invalid crafting grid size.");
			}
		}

		private void UpdateCraftingResult()
		{
			int num = int.MaxValue;
			for (int i = 0; i < m_craftingGridSize; i++)
			{
				for (int j = 0; j < m_craftingGridSize; j++)
				{
					int num2 = i + j * 4;
					int slotIndex = i + j * m_craftingGridSize;
					int slotValue = GetSlotValue(slotIndex);
					int num3 = Terrain.ExtractContents(slotValue);
					int num4 = Terrain.ExtractData(slotValue);
					int slotCount = GetSlotCount(slotIndex);
					if (slotCount > 0)
					{
						Block block = BlocksManager.Blocks[num3];
						m_matchedIngredients[num2] = block.CraftingId + ":" + num4.ToString(CultureInfo.InvariantCulture);
						num = MathUtils.Min(num, slotCount);
					}
					else
					{
						m_matchedIngredients[num2] = null;
					}
				}
			}
			ComponentPlayer componentPlayer = FindInteractingPlayer();
			float playerLevel = componentPlayer?.PlayerData.Level ?? 1f;
			CraftingRecipe craftingRecipe = XCraftingRecipesManager.FindMatchingRecipe(base.Project.FindSubsystem<SubsystemTerrain>(true), m_matchedIngredients, 0f, playerLevel);
			if (craftingRecipe != null)
			{
				m_matchedRecipe = craftingRecipe;
				m_slots[ResultSlotIndex].Value = craftingRecipe.ResultValue;
				m_slots[ResultSlotIndex].Count = craftingRecipe.ResultCount * num;
			}
			else
			{
				m_matchedRecipe = null;
				m_slots[ResultSlotIndex].Value = 0;
				m_slots[ResultSlotIndex].Count = 0;
			}
		}
	}
	#endregion
	public static class XCraftingRecipesManager
	{
		public static List<CraftingRecipe> m_recipes = new List<CraftingRecipe>();

		public static ReadOnlyList<CraftingRecipe> Recipes => new ReadOnlyList<CraftingRecipe>(m_recipes);

		public static void Initialize()
		{
			foreach (XElement item in ContentManager.Get<XElement>("FCCraftingRecipes").Descendants("Recipe"))
			{
				var craftingRecipe = new CraftingRecipe();
				craftingRecipe.Ingredients = new string[16];
				string attributeValue = XmlUtils.GetAttributeValue<string>(item, "Result");
				string desc = XmlUtils.GetAttributeValue<string>(item, "Description");
				if (desc.StartsWith("[") && desc.EndsWith("]") && LanguageControl.TryGetBlock(attributeValue, "CRDescription:" + desc.Substring(1, desc.Length - 2), out var r)) desc = r;
				craftingRecipe.ResultValue = DecodeResult(attributeValue);
				craftingRecipe.ResultCount = XmlUtils.GetAttributeValue<int>(item, "ResultCount");
				string attributeValue2 = XmlUtils.GetAttributeValue(item, "Remains", string.Empty);
				if (!string.IsNullOrEmpty(attributeValue2))
				{
					craftingRecipe.RemainsValue = DecodeResult(attributeValue2);
					craftingRecipe.RemainsCount = XmlUtils.GetAttributeValue<int>(item, "RemainsCount");
				}
				craftingRecipe.RequiredHeatLevel = XmlUtils.GetAttributeValue<float>(item, "RequiredHeatLevel");
				craftingRecipe.RequiredPlayerLevel = XmlUtils.GetAttributeValue(item, "RequiredPlayerLevel", 1f);
				craftingRecipe.Description = desc;
				craftingRecipe.Message = XmlUtils.GetAttributeValue<string>(item, "Message", null);
				if (craftingRecipe.ResultCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.ResultValue)].GetMaxStacking(craftingRecipe.ResultValue))
				{
					throw new InvalidOperationException($"In recipe for \"{attributeValue}\" ResultCount is larger than max stacking of result block.");
				}
				if (craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue))
				{
					throw new InvalidOperationException($"In Recipe for \"{attributeValue2}\" RemainsCount is larger than max stacking of remains block.");
				}
				var dictionary = new Dictionary<char, string>();
				foreach (XAttribute item2 in from a in item.Attributes()
											 where a.Name.LocalName.Length == 1 && char.IsLower(a.Name.LocalName[0])
											 select a)
				{
					DecodeIngredient(item2.Value, out string craftingId, out int? data);
					if (BlocksManager.FindBlocksByCraftingId(craftingId).Length == 0)
					{
						throw new InvalidOperationException($"Block with craftingId \"{item2.Value}\" not found.");
					}
					if (data.HasValue && (data.Value < 0 || data.Value > 262143))
					{
						throw new InvalidOperationException($"Data in recipe ingredient \"{item2.Value}\" must be between 0 and 0x3FFFF.");
					}
					dictionary.Add(item2.Name.LocalName[0], item2.Value);
				}
				string[] array = item.Value.Trim().Split(new string[] { "\n" }, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					int num = array[i].IndexOf('"');
					int num2 = array[i].LastIndexOf('"');
					if (num < 0 || num2 < 0 || num2 <= num)
					{
						throw new InvalidOperationException("Invalid recipe line.");
					}
					string text = array[i].Substring(num + 1, num2 - num - 1);
					for (int j = 0; j < text.Length; j++)
					{
						char c = text[j];
						if (char.IsLower(c))
						{
							string text2 = dictionary[c];
							craftingRecipe.Ingredients[j + i * 4] = text2;
						}
					}
				}

				m_recipes.Add(craftingRecipe);
				/*
				Block[] blocks = BlocksManager.Blocks;
				foreach (Block block in blocks)
				{
					m_recipes.AddRange(block.GetProceduralCraftingRecipes());
				}
				*/
				m_recipes.Sort(delegate (CraftingRecipe r1, CraftingRecipe r2)
				{
					int y = r1.Ingredients.Count((string s) => !string.IsNullOrEmpty(s));
					int x = r2.Ingredients.Count((string s) => !string.IsNullOrEmpty(s));
					return Comparer<int>.Default.Compare(x, y);
				});
			}
		}

		public static CraftingRecipe FindMatchingRecipe(SubsystemTerrain terrain, string[] ingredients, float heatLevel, float playerLevel)
		{
			CraftingRecipe craftingRecipe = null;
            Block[] blocks = BlocksManager.Blocks;
            for (int i = 0; i < blocks.Length; i++)
            {
                CraftingRecipe adHocCraftingRecipe = blocks[i].GetAdHocCraftingRecipe(terrain, ingredients, heatLevel, playerLevel);
                if (adHocCraftingRecipe != null && MatchRecipe(adHocCraftingRecipe.Ingredients, ingredients))
                {
                    craftingRecipe = adHocCraftingRecipe;
                    break;
                }
            }
            if (craftingRecipe != null)
			{
				if (heatLevel < craftingRecipe.RequiredHeatLevel)
				{
					craftingRecipe = null;
				}

			}
			
           
            
			if (craftingRecipe == null)
			{
				foreach (CraftingRecipe recipe in Recipes)
				{
					if (MatchRecipe(recipe.Ingredients, ingredients))
					{
						craftingRecipe = recipe;
						break;
					}
				}
			}


			/*     if (heatLevel < craftingRecipe.RequiredHeatLevel)
				 {
					 craftingRecipe = ((!(heatLevel > 0f)) ? new CraftingRecipe
					 {
						 Message = "使用熔炉来熔炼这个物品"
					 } : new CraftingRecipe
					 {
						 Message = "需要更高温度来熔炼这个物品，可通过寻找更好的燃料解决"
					 });
				 }
				 else if (playerLevel < craftingRecipe.RequiredPlayerLevel)
				 {
					 craftingRecipe = ((!(craftingRecipe.RequiredHeatLevel > 0f)) ? new CraftingRecipe
					 {
						 Message = String.Format("你需要达到等级{0}才能制作这个物品", craftingRecipe.RequiredPlayerLevel)
					 } : new CraftingRecipe
					 {
						 Message = String.Format("你需要达到等级{0}才能熔炼这个物品", craftingRecipe.RequiredPlayerLevel)
					 });
				 }
				 */
			return craftingRecipe;
		}

		public static bool MatchRecipe(string[] requiredIngredients, string[] actualIngredient)
		{
			if (requiredIngredients.Length <= 9 || actualIngredient.Length <= 9)
			{
				return BaseMatchRecipe(requiredIngredients, actualIngredient);
			}
			string[] array = new string[16];
			for (int i = 0; i < 2; i++)
			{
				for (int j = -4; j <= 4; j++)
				{
					for (int k = -4; k <= 4; k++)
					{
						bool flip = (i != 0) ? true : false;
						if (!TransformRecipe(array, requiredIngredients, k, j, flip))
						{
							continue;
						}
						bool flag = true;
						for (int l = 0; l < 4 * 4; l++)
						{
							if (l == actualIngredient.Length || !CompareIngredients(array[l], actualIngredient[l]))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool BaseMatchRecipe(string[] requiredIngredients, string[] actualIngredients)
		{
			string[] array = new string[9];
			for (int i = 0; i < 2; i++)
			{
				for (int j = -3; j <= 3; j++)
				{
					for (int k = -3; k <= 3; k++)
					{
						bool flip = (i != 0) ? true : false;
						if (!TransformRecipe(array, requiredIngredients, k, j, flip))
						{
							continue;
						}
						bool flag = true;
						for (int l = 0; l < 9; l++)
						{
							if (l == actualIngredients.Length || !CompareIngredients(array[l], actualIngredients[l]))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static int DecodeResult(string result)
		{
			string[] array = result.Split((new char[1] { ':' }));
			Block block = BlocksManager.FindBlockByTypeName(array[0], true);
			return Terrain.MakeBlockValue(data: (array.Length >= 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0, contents: block.BlockIndex, light: 0);
		}

		public static void DecodeIngredient(string ingredient, out string craftingId, out int? data)
		{
			string[] array = ingredient.Split((new char[1] { ':' }));
			craftingId = array[0];
			data = ((array.Length >= 2) ? new int?(int.Parse(array[1], CultureInfo.InvariantCulture)) : null);
		}

		public static bool TransformRecipe(string[] transformedIngredients, string[] ingredients, int shiftX, int shiftY, bool flip)
		{
			for (int i = 0; i < 16; i++)
			{
				transformedIngredients[i] = null;
			}
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					int num = (flip ? (4 - k - 1) : k) + shiftX;
					int num2 = j + shiftY;
					string text = ingredients[k + j * 4];
					if (num >= 0 && num2 >= 0 && num < 4 && num2 < 4)
					{
						transformedIngredients[num + num2 * 4] = text;
					}
					else if (!string.IsNullOrEmpty(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		private static bool CompareIngredients(string requiredIngredient, string actualIngredient)
		{
			if (requiredIngredient == null)
			{
				return actualIngredient == null;
			}
			if (actualIngredient == null)
			{
				return requiredIngredient == null;
			}
			DecodeIngredient(requiredIngredient, out string craftingId, out int? data);
			DecodeIngredient(actualIngredient, out string craftingId2, out int? data2);
			if (!data2.HasValue)
			{
				throw new InvalidOperationException("Actual ingredient data not specified.");
			}
			if (craftingId == craftingId2)
			{
				if (!data.HasValue)
				{
					return true;
				}
				return data.Value == data2.Value;
			}
			return false;
		}
	}
	public class XSmeltingRecipeWidget : CanvasWidget//显示配方书中的熔炉配方
	{
		public LabelWidget m_nameWidget;

		public LabelWidget m_descriptionWidget;

		public GridPanelWidget m_gridWidget;

		public FireWidget m_fireWidget;

		public CraftingRecipeSlotWidget m_resultWidget;

		public CraftingRecipe m_recipe;

		public string m_nameSuffix;

		public bool m_dirty = true;

		public string NameSuffix
		{
			get
			{
				return m_nameSuffix;
			}
			set
			{
				if (value != m_nameSuffix)
				{
					m_nameSuffix = value;
					m_dirty = true;
				}
			}
		}

		public CraftingRecipe Recipe
		{
			get
			{
				return m_recipe;
			}
			set
			{
				if (value != m_recipe)
				{
					m_recipe = value;
					m_dirty = true;
				}
			}
		}

		public XSmeltingRecipeWidget()
		{
			XElement node = ContentManager.Get<XElement>("Widgets/XSmeltingRecipe");
			LoadContents(this, node);
			m_nameWidget = Children.Find<LabelWidget>("SmeltingRecipeWidget.Name");
			m_descriptionWidget = Children.Find<LabelWidget>("SmeltingRecipeWidget.Description");
			m_gridWidget = Children.Find<GridPanelWidget>("SmeltingRecipeWidget.Ingredients");
			m_fireWidget = Children.Find<FireWidget>("SmeltingRecipeWidget.Fire");
			m_resultWidget = Children.Find<CraftingRecipeSlotWidget>("SmeltingRecipeWidget.Result");
			for (int i = 0; i < m_gridWidget.RowsCount; i++)
			{
				for (int j = 0; j < m_gridWidget.ColumnsCount; j++)
				{
					var widget = new CraftingRecipeSlotWidget();
					widget.Size = new Vector2(40f, 40f);
					m_gridWidget.Children.Add(widget);
					m_gridWidget.SetWidgetCell(widget, new Point2(j, i));
				}
			}
			m_resultWidget.Size = new Vector2(50f, 50f);
		}

		public override void MeasureOverride(Vector2 parentAvailableSize)
		{
			if (m_dirty)
			{
				UpdateWidgets();
			}
			base.MeasureOverride(parentAvailableSize);
		}

		public void UpdateWidgets()
		{
			m_dirty = false;
			if (m_recipe != null)
			{
				Block block = BlocksManager.Blocks[Terrain.ExtractContents(m_recipe.ResultValue)];
				m_nameWidget.Text = block.GetDisplayName(null, m_recipe.ResultValue) + ((!string.IsNullOrEmpty(NameSuffix)) ? NameSuffix : string.Empty);
				m_descriptionWidget.Text = m_recipe.Description;
				m_nameWidget.IsVisible = true;
				m_descriptionWidget.IsVisible = true;
				foreach (CraftingRecipeSlotWidget child in m_gridWidget.Children)
				{
					Point2 widgetCell = m_gridWidget.GetWidgetCell(child);
					child.SetIngredient(m_recipe.Ingredients[widgetCell.X + widgetCell.Y * 4]);
				}
				m_resultWidget.SetResult(m_recipe.ResultValue, m_recipe.ResultCount);
				m_fireWidget.ParticlesPerSecond = 40f;
			}
			else
			{
				foreach (CraftingRecipeSlotWidget child2 in m_gridWidget.Children)
				{
					child2.SetIngredient(null);
				}
				m_resultWidget.SetResult(0, 0);
				m_fireWidget.ParticlesPerSecond = 0f;
			}
		}
	}
	#region 合成展示槽
	public class XCraftingRecipeSlotWidget : CanvasWidget
	{
		public BlockIconWidget m_blockIconWidget;

		public LabelWidget m_labelWidget;

		public string m_ingredient;

		public int m_resultValue;

		public int m_resultCount;

		public XCraftingRecipeSlotWidget()
		{
			XElement node = ContentManager.Get<XElement>("Widgets/CraftingRecipeSlot");
			LoadContents(this, node);
			m_blockIconWidget = Children.Find<BlockIconWidget>("CraftingRecipeSlotWidget.Icon");
			m_labelWidget = Children.Find<LabelWidget>("CraftingRecipeSlotWidget.Count");
		}

		public void SetIngredient(string ingredient)
		{
			m_ingredient = ingredient;
			m_resultValue = 0;
			m_resultCount = 0;
		}

		public void SetResult(int value, int count)
		{
			m_resultValue = value;
			m_resultCount = count;
			m_ingredient = null;
		}

		public override void MeasureOverride(Vector2 parentAvailableSize)
		{
			m_blockIconWidget.IsVisible = false;
			m_labelWidget.IsVisible = false;
			if (!string.IsNullOrEmpty(m_ingredient))
			{
				XCraftingRecipesManager.DecodeIngredient(m_ingredient, out string craftingId, out int? data);
				Block[] array = BlocksManager.FindBlocksByCraftingId(craftingId);
				if (array.Length != 0)
				{
					Block block = array[(int)(1.0 * Time.RealTime) % array.Length];
					if (block != null)
					{
						m_blockIconWidget.Value = Terrain.MakeBlockValue(block.BlockIndex, 0, data.HasValue ? data.Value : 4);
						m_blockIconWidget.Light = 15;
						m_blockIconWidget.IsVisible = true;
					}
				}
			}
			else if (m_resultValue != 0)
			{
				m_blockIconWidget.Value = m_resultValue;
				m_blockIconWidget.Light = 15;
				m_labelWidget.Text = m_resultCount.ToString();
				m_blockIconWidget.IsVisible = true;
				m_labelWidget.IsVisible = true;
			}
			base.MeasureOverride(parentAvailableSize);
		}
	}
	#endregion
	public class XCraftingRecipeWidget : CanvasWidget
	{
		public LabelWidget m_nameWidget;

		public LabelWidget m_descriptionWidget;

		public GridPanelWidget m_gridWidget;

		public XCraftingRecipeSlotWidget m_resultWidget;

		public CraftingRecipe m_recipe;

		public string m_nameSuffix;

		public bool m_dirty = true;

		public string NameSuffix
		{
			get
			{
				return m_nameSuffix;
			}
			set
			{
				if (value != m_nameSuffix)
				{
					m_nameSuffix = value;
					m_dirty = true;
				}
			}
		}

		public CraftingRecipe Recipe
		{
			get
			{
				return m_recipe;
			}
			set
			{
				if (value != m_recipe)
				{
					m_recipe = value;
					m_dirty = true;
				}
			}
		}

		public XCraftingRecipeWidget()
		{
			XElement node = ContentManager.Get<XElement>("Widgets/XCraftingRecipe");
			LoadContents(this, node);
			m_nameWidget = Children.Find<LabelWidget>("CraftingRecipeWidget.Name");
			m_descriptionWidget = Children.Find<LabelWidget>("CraftingRecipeWidget.Description");
			m_gridWidget = Children.Find<GridPanelWidget>("CraftingRecipeWidget.Ingredients");
			m_resultWidget = Children.Find<XCraftingRecipeSlotWidget>("CraftingRecipeWidget.Result");
			for (int i = 0; i < m_gridWidget.RowsCount; i++)
			{
				for (int j = 0; j < m_gridWidget.ColumnsCount; j++)
				{
					var widget = new XCraftingRecipeSlotWidget();
					m_gridWidget.Children.Add(widget);
					widget.Size = new Vector2(30f, 30f);
					m_gridWidget.SetWidgetCell(widget, new Point2(j, i));
				}
			}
			m_resultWidget.Size = new Vector2(30f, 30f);
		}

		public override void MeasureOverride(Vector2 parentAvailableSize)
		{
			if (m_dirty)
			{
				UpdateWidgets();
			}
			base.MeasureOverride(parentAvailableSize);
		}

		public void UpdateWidgets()
		{
			m_dirty = false;
			if (m_recipe != null)
			{
				Block block = BlocksManager.Blocks[Terrain.ExtractContents(m_recipe.ResultValue)];
				m_nameWidget.Text = block.GetDisplayName(null, m_recipe.ResultValue) + ((!string.IsNullOrEmpty(NameSuffix)) ? NameSuffix : string.Empty);
				m_descriptionWidget.Text = m_recipe.Description;
				m_nameWidget.IsVisible = true;
				m_descriptionWidget.IsVisible = true;
				foreach (XCraftingRecipeSlotWidget child in m_gridWidget.Children)
				{
					Point2 widgetCell = m_gridWidget.GetWidgetCell(child);
					child.SetIngredient(m_recipe.Ingredients[widgetCell.X + widgetCell.Y * 4]);
				}
				m_resultWidget.SetResult(m_recipe.ResultValue, m_recipe.ResultCount);
			}
			else
			{
				foreach (XCraftingRecipeSlotWidget child2 in m_gridWidget.Children)
				{
					child2.SetIngredient(null);
				}
				m_resultWidget.SetResult(0, 0);
			}
		}
	}
	public class XFurnaceBlock : SixFaceBlock
	{
		public const int Index = 983; //275

		public XFurnaceBlock()
			: base("Textures/Blocks/XFurnace", Color.White)
		{
		}
	}

	public class TFurnaceBlock : SixFaceBlock
	{
		public const int Index = 982;//276

		public TFurnaceBlock()
			: base("Textures/Blocks/TFurnace", Color.White)
		{
		}
	}

	public class SubsystemFlameFurnaceBlockBehavior : SubsystemBlockBehavior
	{
		public SubsystemParticles m_subsystemParticles;

		public Dictionary<Point3, FireParticleSystem> m_particleSystemsByCell = new Dictionary<Point3, FireParticleSystem>();

		public override int[] HandledBlocks => new int[2]
		{
			983,//275
			982//276
		};

		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(oldValue) != 983 && Terrain.ExtractContents(oldValue) != 982)
			{
				DatabaseObject databaseObject = SubsystemTerrain.Project.GameDatabase.Database.FindDatabaseObject("FlameFurnace", SubsystemTerrain.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
				var valuesDictionary = new ValuesDictionary();
				valuesDictionary.PopulateFromDatabaseObject(databaseObject);
				valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", new Point3(x, y, z));
				Entity entity = SubsystemTerrain.Project.CreateEntity(valuesDictionary);
				SubsystemTerrain.Project.AddEntity(entity);
			}
			if (Terrain.ExtractContents(value) == 982)
			{
				AddFire(value, x, y, z);
			}
		}

		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			if (Terrain.ExtractContents(newValue) != 983 && Terrain.ExtractContents(newValue) != 982)
			{
				ComponentBlockEntity blockEntity = SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true).GetBlockEntity(x, y, z);
				if (blockEntity != null)
				{
					Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
					foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
					{
						item.DropAllItems(position);
					}
					SubsystemTerrain.Project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
				}
			}
			if (Terrain.ExtractContents(value) == 982)
			{
				RemoveFire(x, y, z);
			}
		}

		public override void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded)
		{
			if (Terrain.ExtractContents(value) == 982)
			{
				AddFire(value, x, y, z);
			}
		}

		public override void OnChunkDiscarding(TerrainChunk chunk)
		{
			var list = new List<Point3>();
			foreach (Point3 key in m_particleSystemsByCell.Keys)
			{
				if (key.X >= chunk.Origin.X && key.X < chunk.Origin.X + 16 && key.Z >= chunk.Origin.Y && key.Z < chunk.Origin.Y + 16)
				{
					list.Add(key);
				}
			}
			foreach (Point3 item in list)
			{
				RemoveFire(item.X, item.Y, item.Z);
			}
		}

		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{
			ComponentBlockEntity blockEntity = SubsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true).GetBlockEntity(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
			if (blockEntity != null && componentMiner.ComponentPlayer != null)
			{
				ComponentFlameFurnace componentFlameFurnace = blockEntity.Entity.FindComponent<ComponentFlameFurnace>(throwOnError: true);
				componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new FlameFurnaceWidget(componentMiner.Inventory, componentFlameFurnace);
				AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
				return true;
			}
			return false;
		}

		public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
		{
			base.OnNeighborBlockChanged(x, y, z, neighborX, neighborY, neighborZ);
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			m_subsystemParticles = Project.FindSubsystem<SubsystemParticles>(throwOnError: true);
		}

		public void AddFire(int value, int x, int y, int z)
		{
			var v = new Vector3(0.5f, 0.2f, 0.5f);
			float size = 0.15f;
			var fireParticleSystem = new FireParticleSystem(new Vector3(x, y, z) + v, size, 16f);
			m_subsystemParticles.AddParticleSystem(fireParticleSystem);
			m_particleSystemsByCell[new Point3(x, y, z)] = fireParticleSystem;
		}

		public void RemoveFire(int x, int y, int z)
		{
			var key = new Point3(x, y, z);
			FireParticleSystem particleSystem = m_particleSystemsByCell[key];
			m_subsystemParticles.RemoveParticleSystem(particleSystem);
			m_particleSystemsByCell.Remove(key);
		}
	}

	public class FlameFurnaceWidget : CanvasWidget //熔炉界面
	{
		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_flamefurnaceGrid;//放原料的4x4格子

		public InventorySlotWidget m_fuelSlot;

		public InventorySlotWidget m_resultSlot;

		public InventorySlotWidget m_remainsSlot;

		public FireWidget m_fire;

		public ValueBarWidget m_progress;

		public ComponentFlameFurnace m_componentFlameFurnace;

		public FlameFurnaceWidget(IInventory inventory, ComponentFlameFurnace componentFlameFurnace)
		{
			m_componentFlameFurnace = componentFlameFurnace;
			XElement node = ContentManager.Get<XElement>("Widgets/FlameFurnaceWidget");
			LoadContents(this, node);
			m_inventoryGrid = Children.Find<GridPanelWidget>("InventoryGrid");
			m_flamefurnaceGrid = Children.Find<GridPanelWidget>("FlameFurnaceGrid");
			m_fire = Children.Find<FireWidget>("Fire");
			m_progress = Children.Find<ValueBarWidget>("Progress");
			m_resultSlot = Children.Find<InventorySlotWidget>("ResultSlot");
			m_remainsSlot = Children.Find<InventorySlotWidget>("RemainsSlot");
			m_fuelSlot = Children.Find<InventorySlotWidget>("FuelSlot");
			
		
			int num = 10;
			for (int i = 0; i < m_inventoryGrid.RowsCount; i++)
			{
				for (int j = 0; j < m_inventoryGrid.ColumnsCount; j++)
				{
					var inventorySlotWidget = new InventorySlotWidget();
					inventorySlotWidget.AssignInventorySlot(inventory, num++);
					m_inventoryGrid.Children.Add(inventorySlotWidget);
					m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(j, i));
				}
			}
			num = 0;
			for (int k = 0; k < m_flamefurnaceGrid.RowsCount; k++)
			{
				for (int l = 0; l < m_flamefurnaceGrid.ColumnsCount; l++)
				{
					var inventorySlotWidget2 = new InventorySlotWidget { Size = new Vector2 (40,40)};
					inventorySlotWidget2.AssignInventorySlot(componentFlameFurnace, num++);
					m_flamefurnaceGrid.Children.Add(inventorySlotWidget2);
					m_flamefurnaceGrid.SetWidgetCell(inventorySlotWidget2, new Point2(l, k));
				}
			}
			m_fuelSlot.AssignInventorySlot(componentFlameFurnace, componentFlameFurnace.FuelSlotIndex);
			m_resultSlot.AssignInventorySlot(componentFlameFurnace, componentFlameFurnace.ResultSlotIndex);
			m_remainsSlot.AssignInventorySlot(componentFlameFurnace, componentFlameFurnace.RemainsSlotIndex);
		}

		public override void Update()
		{
			m_fire.ParticlesPerSecond = ((m_componentFlameFurnace.HeatLevel > 0f) ? 24 : 0);
			m_progress.Value = m_componentFlameFurnace.SmeltingProgress;
			if (!m_componentFlameFurnace.IsAddedToProject)
			{
				ParentWidget.Children.Remove(this);
			}
		}
	}

	public class ComponentFlameFurnace : ComponentInventoryBase, IUpdateable
	{
		private SubsystemTerrain m_subsystemTerrain;

		private SubsystemExplosions m_subsystemExplosions;

		private ComponentBlockEntity m_componentBlockEntity;

		private int m_furnaceSize;

		private string[] m_matchedIngredients = new string[16];

		private float m_fireTimeRemaining;

		private float m_heatLevel;

		private bool m_updateSmeltingRecipe;

		private CraftingRecipe m_smeltingRecipe;

		private float m_smeltingProgress;

		public int RemainsSlotIndex => SlotsCount - 1;

		public int ResultSlotIndex => SlotsCount - 2;

		public int FuelSlotIndex => SlotsCount - 3;

		public float HeatLevel => m_heatLevel;

		public float SmeltingProgress => m_smeltingProgress;

		public UpdateOrder UpdateOrder => UpdateOrder.Default;

		public override int GetSlotCapacity(int slotIndex, int value)
		{
			if (slotIndex == FuelSlotIndex)
			{
				if (BlocksManager.Blocks[Terrain.ExtractContents(value)].FuelHeatLevel > 0f)
				{
					return base.GetSlotCapacity(slotIndex, value);
				}
				return 0;
			}
			return base.GetSlotCapacity(slotIndex, value);
		}

		public override void AddSlotItems(int slotIndex, int value, int count)
		{
			base.AddSlotItems(slotIndex, value, count);
			m_updateSmeltingRecipe = true;
		}

		public override int RemoveSlotItems(int slotIndex, int count)
		{
			m_updateSmeltingRecipe = true;
			return base.RemoveSlotItems(slotIndex, count);
		}

		public void Update(float dt)
		{
			Point3 coordinates = m_componentBlockEntity.Coordinates;
			if (m_heatLevel >0)//如果热值是11
			{
				m_fireTimeRemaining = MathUtils.Max(0f, m_fireTimeRemaining - dt);
				if (m_fireTimeRemaining == 0f)
				{
					m_heatLevel = 0f;
				}
			}
			if (m_updateSmeltingRecipe)
			{
				m_updateSmeltingRecipe = false;
				float heatLevel = 0f;
				if (m_heatLevel > 0f)
				{
					heatLevel = m_heatLevel;
				}
				else
				{
					Slot slot = m_slots[FuelSlotIndex];
					if (slot.Count > 0)
					{
						int num = Terrain.ExtractContents(slot.Value);
						heatLevel = BlocksManager.Blocks[num].FuelHeatLevel;
					}
				}
				CraftingRecipe craftingRecipe = FindSmeltingRecipe(heatLevel);
				if (craftingRecipe != m_smeltingRecipe)
				{
					m_smeltingRecipe = craftingRecipe;
					m_smeltingProgress = 0f;
				}
			}
			if (m_smeltingRecipe == null)
			{
				m_heatLevel = 0f;
				m_fireTimeRemaining = 0f;
			}
			if (m_smeltingRecipe != null && m_fireTimeRemaining <= 0f)
			{
				Slot slot2 = m_slots[FuelSlotIndex];
				if (slot2.Count > 0)
				{
					int num2 = Terrain.ExtractContents(slot2.Value);
					Block block = BlocksManager.Blocks[num2];
					if (block.GetExplosionPressure(slot2.Value) > 0f)
					{
						slot2.Count = 0;
						m_subsystemExplosions.TryExplodeBlock(coordinates.X, coordinates.Y, coordinates.Z, slot2.Value);
					}
					else if (block.FuelHeatLevel > 0f)
					{
						slot2.Count--;
						m_fireTimeRemaining = block.FuelFireDuration;
						m_heatLevel = block.FuelHeatLevel;
					}
				}
			}
			if (m_fireTimeRemaining <= 0f)
			{
				m_smeltingRecipe = null;
				m_smeltingProgress = 0f;
			}
			if (m_smeltingRecipe != null)
			{
				m_smeltingProgress = MathUtils.Min(m_smeltingProgress + 0.15f * dt, 1f);
				if (m_smeltingProgress >= 1f)
				{
					for (int i = 0; i < 16; i++)
					{
						if (m_slots[i].Count > 0)
						{
							m_slots[i].Count--;
						}
					}
					m_slots[ResultSlotIndex].Value = m_smeltingRecipe.ResultValue;
					m_slots[ResultSlotIndex].Count += m_smeltingRecipe.ResultCount;
					if (m_smeltingRecipe.RemainsValue != 0 && m_smeltingRecipe.RemainsCount > 0)
					{
						m_slots[RemainsSlotIndex].Value = m_smeltingRecipe.RemainsValue;
						m_slots[RemainsSlotIndex].Count += m_smeltingRecipe.RemainsCount;
					}
					m_smeltingRecipe = null;
					m_smeltingProgress = 0f;
					m_updateSmeltingRecipe = true;
				}
			}
			TerrainChunk chunkAtCell = m_subsystemTerrain.Terrain.GetChunkAtCell(coordinates.X, coordinates.Z);
			if (chunkAtCell != null && chunkAtCell.State == TerrainChunkState.Valid)
			{
				int cellValue = m_subsystemTerrain.Terrain.GetCellValue(coordinates.X, coordinates.Y, coordinates.Z);
				m_subsystemTerrain.ChangeCell(coordinates.X, coordinates.Y, coordinates.Z, Terrain.ReplaceContents(cellValue, (m_heatLevel > 0f) ? 982 : 983));
			}
		}

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_subsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
			m_subsystemExplosions = base.Project.FindSubsystem<SubsystemExplosions>(true);
			m_componentBlockEntity = base.Entity.FindComponent<ComponentBlockEntity>(true);
			m_furnaceSize = SlotsCount - 3;
			if (m_furnaceSize < 1 || m_furnaceSize > 16)
			{
				throw new InvalidOperationException("Invalid furnace size.");
			}
			m_fireTimeRemaining = valuesDictionary.GetValue<float>("FireTimeRemaining");
			m_heatLevel = valuesDictionary.GetValue<float>("HeatLevel");
			m_updateSmeltingRecipe = true;
		}

		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			base.Save(valuesDictionary, entityToIdMap);
			valuesDictionary.SetValue("FireTimeRemaining", m_fireTimeRemaining);
			valuesDictionary.SetValue("HeatLevel", m_heatLevel);
		}

		private CraftingRecipe FindSmeltingRecipe(float heatLevel)
		{
            if (heatLevel > 0f)
            {
                for (int i = 0; i < this.m_furnaceSize; i++)
                {
                    int slotValue = this.GetSlotValue(i);
                    int num = Terrain.ExtractContents(slotValue);
                    int num2 = Terrain.ExtractData(slotValue);
                    if (this.GetSlotCount(i) > 0)
                    {
                        Block block = BlocksManager.Blocks[num];
                        this.m_matchedIngredients[i] = block.GetCraftingId(slotValue) + ":" + num2.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.m_matchedIngredients[i] = null;
                    }
                }
                ComponentPlayer componentPlayer = base.FindInteractingPlayer();
                float playerLevel = (componentPlayer != null) ? componentPlayer.PlayerData.Level : 1f;
                CraftingRecipe craftingRecipe = XCraftingRecipesManager.FindMatchingRecipe(this.m_subsystemTerrain, this.m_matchedIngredients, heatLevel, playerLevel);
                if (craftingRecipe != null && craftingRecipe.ResultValue != 0)
                {
                    if (craftingRecipe.RequiredHeatLevel <= 0f)
                    {
                        craftingRecipe = null;
                    }
                    if (craftingRecipe != null)
                    {
                        ComponentInventoryBase.Slot slot = this.m_slots[this.ResultSlotIndex];
                        int num3 = Terrain.ExtractContents(craftingRecipe.ResultValue);
                        if (slot.Count != 0 && (craftingRecipe.ResultValue != slot.Value || craftingRecipe.ResultCount + slot.Count > BlocksManager.Blocks[num3].GetMaxStacking(craftingRecipe.ResultValue)))
                        {
                            craftingRecipe = null;
                        }
                    }
                    if (craftingRecipe != null && craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > 0)
                    {
                        if (this.m_slots[this.RemainsSlotIndex].Count == 0 || this.m_slots[this.RemainsSlotIndex].Value == craftingRecipe.RemainsValue)
                        {
                            if (BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].GetMaxStacking(craftingRecipe.RemainsValue) - this.m_slots[this.RemainsSlotIndex].Count < craftingRecipe.RemainsCount)
                            {
                                craftingRecipe = null;
                            }
                        }
                        else
                        {
                            craftingRecipe = null;
                        }
                    }
                }
                if (craftingRecipe != null && !string.IsNullOrEmpty(craftingRecipe.Message) && componentPlayer != null)
                {
                    componentPlayer.ComponentGui.DisplaySmallMessage(craftingRecipe.Message, Color.White, true, true);
                }
                return craftingRecipe;
            }
            return null;
            
        }
	}//炼制台

    /*if (heatLevel >0f)//如果热值是11
			{
				for (int i = 0; i < 16; i++)
				{
					int slotValue = GetSlotValue(i);
					int num = Terrain.ExtractContents(slotValue);
					int num2 = Terrain.ExtractData(slotValue);
					if (GetSlotCount(i) > 0)
					{
						Block block = BlocksManager.Blocks[num];
						m_matchedIngredients[i] = block.CraftingId + ":" + num2.ToString(CultureInfo.InvariantCulture);
					}
					else
					{
						m_matchedIngredients[i] = null;
					}
				}
				CraftingRecipe craftingRecipe = null;
				ComponentPlayer componentPlayer = FindInteractingPlayer();
				float playerLevel = componentPlayer?.PlayerData.Level ?? 1f;
				craftingRecipe = XCraftingRecipesManager.FindMatchingRecipe(m_subsystemTerrain, m_matchedIngredients, heatLevel, playerLevel);
				if (craftingRecipe != null)
				{
					if (craftingRecipe.RequiredHeatLevel >0f)
					{
						craftingRecipe = null;
					}
					if (craftingRecipe != null)
					{
						Slot slot = m_slots[ResultSlotIndex];
						int num3 = Terrain.ExtractContents(craftingRecipe.ResultValue);
						if (slot.Count != 0 && (craftingRecipe.ResultValue != slot.Value || craftingRecipe.ResultCount + slot.Count > BlocksManager.Blocks[num3].MaxStacking))
						{
							craftingRecipe = null;
						}
					}
					if (craftingRecipe != null && craftingRecipe.RemainsValue != 0 && craftingRecipe.RemainsCount > 0)
					{
						if (m_slots[RemainsSlotIndex].Count == 0 || m_slots[RemainsSlotIndex].Value == craftingRecipe.RemainsValue)
						{
							if (BlocksManager.Blocks[Terrain.ExtractContents(craftingRecipe.RemainsValue)].MaxStacking - m_slots[RemainsSlotIndex].Count < craftingRecipe.RemainsCount)
							{
								craftingRecipe = null;
							}
						}
						else
						{
							craftingRecipe = null;
						}
					}
				}
				return craftingRecipe;
			}
			return null;*/

    public abstract class SixFaceBlock : Block
	{
		public Texture2D m_texture;

		public string m_texturename;

		public Color m_color;

		public SixFaceBlock(string texturename, Color color)
		{
			m_texturename = texturename;
			m_color = color;
		}

		public override void Initialize()
		{
			m_texture = ContentManager.Get<Texture2D>(m_texturename);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
			generator.GenerateCubeVertices(this, value, x, y, z, m_color, geometry.GetGeometry(m_texture).OpaqueSubsetsByFace);
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, m_color, m_color, environmentData, m_texture);
		}

		public override int GetTextureSlotCount(int value)
		{
			return 3;
		}

		public override int GetFaceTextureSlot(int face, int value)
		{
			switch (face)
			{
				case 0: return 0;
				case 1: return 0;
				case 2: return 0;
				case 3: return 0;
				case 4: return 1;
				case 5: return 1;
			}
			return 0;
		}
	}


	
	
	#endregion
}//魔法工艺
