using System.Linq;
using Game;
using Engine.Serialization;
using System.Xml.Linq;
using Engine;
using System;
using Engine.Media;
using Random = Game.Random;
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

/*namespace Game 
{
    #region 音乐
    public static class FCMusicManager
	{
		public static FCMusicManager.Mix CurrentMix
		{
			get
			{
				return FCMusicManager.m_currentMix;
			}
			set
			{
				if (value != FCMusicManager.m_currentMix)
				{
					FCMusicManager.m_currentMix = value;
					FCMusicManager.m_nextSongTime = 0.0;
				}
			}
		}

		public static bool IsPlaying
		{
			get
			{
				return FCMusicManager.m_sound != null && FCMusicManager.m_sound.State > SoundState.Stopped;
			}
		}

		public static float Volume
		{
			get
			{
				return SettingsManager.MusicVolume * 0.6f;
			}
		}

		public static void Update()
		{
			if (FCMusicManager.m_fadeSound != null)
			{
				FCMusicManager.m_fadeSound.Volume = MathUtils.Min(FCMusicManager.m_fadeSound.Volume - 0.33f * FCMusicManager.Volume * Time.FrameDuration, FCMusicManager.Volume);
				if (FCMusicManager.m_fadeSound.Volume <= 0f)
				{
					FCMusicManager.m_fadeSound.Dispose();
					FCMusicManager.m_fadeSound = null;
				}
			}
			if (FCMusicManager.m_sound != null && Time.FrameStartTime >= FCMusicManager.m_fadeStartTime)
			{
				FCMusicManager.m_sound.Volume = MathUtils.Min(FCMusicManager.m_sound.Volume + 0.33f * FCMusicManager.Volume * Time.FrameDuration, FCMusicManager.Volume);
			}
			if (FCMusicManager.m_currentMix == FCMusicManager.Mix.None || FCMusicManager.Volume == 0f)
			{
				FCMusicManager.StopMusic();
				return;
			}
			if (FCMusicManager.m_currentMix == FCMusicManager.Mix.Menu && (Time.FrameStartTime >= FCMusicManager.m_nextSongTime || !FCMusicManager.IsPlaying))
			{
				float startPercentage = FCMusicManager.IsPlaying ? FCMusicManager.m_random.Float(0f, 0.75f) : 0f;
				string ContentMusicPath = string.Empty;
				ModsManager.HookAction("MenuPlayMusic", delegate (ModLoader loader)
				{
					loader.MenuPlayMusic(out ContentMusicPath);
					return false;
				});
				if (!string.IsNullOrEmpty(ContentMusicPath))
				{
					FCMusicManager.PlayMusic(ContentMusicPath, startPercentage);
					FCMusicManager.m_nextSongTime = Time.FrameStartTime + (double)FCMusicManager.m_random.Float(40f, 60f);
					return;
				}
				switch (FCMusicManager.m_random.Int(0, 5))
				{
					case 0:
						MusicManager.PlayMusic("FCMusic/OP", startPercentage);
						break;
					case 1:
						MusicManager.PlayMusic("FCMusic/Lord", startPercentage);
						break;
					case 2:
						MusicManager.PlayMusic("FCMusic/Dreamlibrary", startPercentage);
						break;
					case 3:
						MusicManager.PlayMusic("FCMusic/cat", startPercentage);
						break;
					case 4:
						MusicManager.PlayMusic("FCMusic/Snowkingdom", startPercentage);
						break;
					case 5:
						MusicManager.PlayMusic("FCMusic/OP", startPercentage);
						break;
				}
				FCMusicManager.m_nextSongTime = Time.FrameStartTime + (double)FCMusicManager.m_random.Float(40f, 60f);
			}
		}

		public static void Initialize()
		{
		}

		public static void PlayMusic(string name, float startPercentage)
		{
			if (string.IsNullOrEmpty(name))
			{
				FCMusicManager.StopMusic();
				return;
			}
			try
			{
				FCMusicManager.StopMusic();
				FCMusicManager.m_fadeStartTime = Time.FrameStartTime + 2.0;
				float volume = (FCMusicManager.m_fadeSound != null) ? 0f : FCMusicManager.Volume;
				StreamingSource streamingSource = ContentManager.Get<StreamingSource>(name, ".ogg").Duplicate();
				streamingSource.Position = (long)(MathUtils.Saturate(startPercentage) * (float)(streamingSource.BytesCount / (long)streamingSource.ChannelsCount / 2L)) / 16L * 16L;
				FCMusicManager.m_sound = new StreamingSound(streamingSource, volume, 1f, 0f, false, true, 1f);
				FCMusicManager.m_sound.Play();
			}
			catch
			{
				Log.Warning("Error playing music \"{0}\".", new object[]
				{
					name
				});
			}
		}

		public static void StopMusic()
		{
			if (FCMusicManager.m_sound != null)
			{
				if (FCMusicManager.m_fadeSound != null)
				{
					FCMusicManager.m_fadeSound.Dispose();
				}
				FCMusicManager.m_sound.Stop();
				FCMusicManager.m_fadeSound = FCMusicManager.m_sound;
				FCMusicManager.m_sound = null;
			}
		}

		public const float m_fadeSpeed = 0.33f;

		public const float m_fadeWait = 2f;

		public static StreamingSound m_fadeSound;

		public static StreamingSound m_sound;

		public static double m_fadeStartTime;

		public static FCMusicManager.Mix m_currentMix;

		public static double m_nextSongTime;

		public static Random m_random = new Random();

		public enum Mix
		{
			None,
			Menu
		}
	}
    #endregion
}*/

namespace Test1 //界面测试
{
	public class Test1ButtonsPanelWidget : CanvasWidget
	{
		public bool YHLZ = false;
		public ComponentTest1 m_componentTest1;
		public BevelledButtonWidget m_button1;
		public BevelledButtonWidget m_button2;
		public BevelledButtonWidget m_button3;
		public BevelledButtonWidget m_button10;
		public Test1ButtonsPanelWidget(ComponentTest1 componentTest1)
		{
			m_componentTest1 = componentTest1;
			XElement node = ContentManager.Get<XElement>("Test1/Widgets/Test1ButtonsPanelWidget");
			LoadContents(this, node);
			m_button1 = Children.Find<BevelledButtonWidget>("Button1");//通过Name属性获取子界面，若xml中不存在对应的Name则会出错
			m_button2 = Children.Find<BevelledButtonWidget>("Button2");
			m_button3 = Children.Find<BevelledButtonWidget>("Button3");
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
			if(m_button3.IsChecked)
            {
				if(YHLZ==false)
                {
					YHLZ = true;
					m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("樱花粒子已经开启！", Color.Pink, true, true);//显示通知
				}
				else
                {
					YHLZ=false;
					m_componentTest1.m_componentPlayer.ComponentGui.DisplaySmallMessage("樱花粒子已经关闭！", Color.Pink, true, true);//显示通知
				}
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
	public class ComponentTest1 : Component, IUpdateable
	{//此组件用于添加打开界面的按钮，可将该组件全部功能合并至任意一个仅注册在玩家身上的组件
		bool isbuffing = false;
		
		List<Component> m_RDlist = new List<Component>();	
		public SubsystemTerrain m_systemTerrain;
		public NewModLoaderShengcheng Shengcheng1;
		public FCSubsystemTown m_subsystemtown;
		public FCSubsystemTownChunk m_subsystemtownchunk;
		public ComponentPlayer m_componentPlayer;
		public bool ischunkloding = false;//用来保证区块强制加载只执行一次
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
			FontScale = 0.7f,//字体大小
			IsHitTestVisible = false,//是否能点击，为true的话可能会挡住重叠在它下方的界面，出现点不到下方界面的情况
			HorizontalAlignment = WidgetAlignment.Near,//居右对齐(由于界面对齐以左上角为原点，因此水平方向的Near就是居左，Far就是居右，Center居中)
			VerticalAlignment = WidgetAlignment.Center,//居下对齐(由于界面对齐以左上角为原点，因此垂直方向的Near就是居上，Far就是居下，Center居中)
			DropShadow = true//是否投影阴影(为true时字体会在右下方重叠显示一片黑色阴影，看着更立体、更醒目)
		};

		public bool DeveloperModeOn;//开发者模式是否开启
		

		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			m_systemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
			
			m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);

			componentNightsight = Entity.FindComponent<ComponentNightsight>(true);
			componentSpeedUP = Entity.FindComponent<ComponentSpeedUP>(true);
			componentHealBuffA = Entity.FindComponent<ComponentHealBuffA>(true);
			componentAttackUP = Entity.FindComponent<ComponentAttackUP>(true);
			m_componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("MoreContents", true).Children.Add(m_test1Button);//将按钮添加至玩家右上角省略号内
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Add(m_test1Display);//将文字界面添加至屏幕
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Add(Buffscreen);//将buff界面添加至屏幕
			DeveloperModeOn = valuesDictionary.GetValue("DeveloperModeOn", false);//从project.xml中获取储存的值，若获取失败则采用false
			Shengcheng1 = new NewModLoaderShengcheng();
		}
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			base.Save(valuesDictionary, entityToIdMap);
			valuesDictionary.SetValue("DeveloperModeOn", DeveloperModeOn);//将该变量值储存到project.xml，防止重进存档后变回默认值
		}

		public override void OnEntityRemoved()
		{
			base.OnEntityRemoved();
			m_componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("MoreContents", true).Children.Remove(m_test1Button);//当玩家实体被移除后，也要移除按钮，否则玩家复活后会出现两个按钮
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Remove(m_test1Display);//移除文字界面，原因同上
			m_componentPlayer.GameWidget.Children.Find<CanvasWidget>("Gui", true).Children.Remove(Buffscreen);//移除文字界面，原因同上
		}
		public static int GetTemperatureAdjustmentAtHeight(int y)
		{//该静态方法从原版复制过来，用于计算高度变化带来的温度修正
			return (int)MathUtils.Round((y > 64) ? (-0.0008f * MathUtils.Sqr(y - 64)) : (0.1f * (64 - y)));
		}
		public bool isvillageLoading = false;
		public int num_village4 = 0;
		public ComponentHealBuffA componentHealBuffA;
		public ComponentAttackUP componentAttackUP;
		public ComponentSpeedUP componentSpeedUP;
        public ComponentNightsight componentNightsight;
        public bool buffscreenIsVisible = true;
		public async void Update(float dt)
		{
			
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
				m_componentPlayer.ComponentGui.ModalPanelWidget = new Test1ButtonsPanelWidget(this);//打开界面
			}
            #endregion
			if(buffscreenIsVisible == true)//时刻开启
			{
                Buffscreen.IsVisible = true;
                string buffinfo = "状态栏";
                string txt2 = string.Format("当前生命{0}",(int)m_componentPlayer.ComponentHealth.AttackResilience);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt2;
                string txt3 = string.Format("当前攻击力{0}", (int)m_componentPlayer.ComponentMiner.AttackPower);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt3;
                string txt4 = string.Format("当前速度{0}", (int)m_componentPlayer.ComponentLocomotion.WalkSpeed);//传入变量组成第一段文字
                buffinfo = buffinfo + "\n" + txt4;
                if (componentHealBuffA.m_HealDuration > 0 || componentAttackUP.m_ATKDuration > 0 || componentSpeedUP.m_SpeedDuration > 0||componentNightsight.m_NightseeDuration>0)//是否处于buff的前置判断
                {
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

                }
                Buffscreen.Text = buffinfo;
            }
           
           



			#region 村庄区块
			List<NewModLoaderShengcheng.RoadPoint> listRD = NewModLoaderShengcheng.listRD;
			List<NewModLoaderShengcheng.BuildPoint> listBD = NewModLoaderShengcheng.listBD;
			
			if (listRD.Count !=0&&isvillageLoading==false)//村庄代码区块
			{
				isvillageLoading = true;
				if (FCSubsystemTown.Village_start.Count !=0)
				{
					if((FCSubsystemTown.Village_start.Count-num_village4)>0)//如果村庄生成有变化，实时提醒。
                    {
						num_village4++;
						string txt = $"检测到村庄已经生成，玩家可前往探索！坐标为：{FCSubsystemTown.Village_start[num_village4-1]}";
						m_componentPlayer.ComponentGui.DisplaySmallMessage(txt, Color.White, true, true);//显示通知
					}
					
					
				}
				if (ischunkloding == false)
				{
					for (int i = 0; i < listRD.Count; i++)
					{

					  Shengcheng1.AddChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);//加载区块
					  await Task.Delay(100);

					}
					ischunkloding = true;
				}

				for (int i = 0; i < 8; i++)
				{
					if (listRD.Count == 0)
					{
						break;
					}
					TerrainChunk chunk_v = m_systemTerrain.Terrain.GetChunkAtCell(listRD[i].Position.X, listRD[i].Position.Z);
					int visiblerange = SettingsManager.VisibilityRange;
					
					if (chunk_v == null)//如果区块为空，则跳过该路径点的生成。
					{
						continue;
					}
					else 
                    {
						Point3 point = Terrain.ToCell(m_componentPlayer.ComponentBody.Position);//玩家所在方块坐标
						int chunkX = chunk_v.Origin.X;
						int chunkY = chunk_v.Origin.Y;
						int chunkX1 = chunk_v.Coords.X;
						int chunkY1 = chunk_v.Coords.Y;
						double distance1 = Math.Sqrt(((Math.Abs(listRD[i].Position.X - point.X)) * (Math.Abs(listRD[i].Position.X - point.X))) + ((Math.Abs(listRD[i].Position.Z - point.Z)) * (Math.Abs(listRD[i].Position.Z - point.Z))));
						int distance_int = (int)distance1;//计算路径点目标位置与玩家真实距离。
						double distance2 =  Math.Sqrt(((Math.Abs(chunkX - point.X)) * (Math.Abs(chunkX - point.X))) + ((Math.Abs(chunkY - point.Z)) * (Math.Abs(chunkY - point.Z))));
						int distance_int2 = (int)distance2;//计算目标真实区块与玩家真实距离。
						
						Point2 point_t1 = (chunkX1, chunkY1);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

						bool ischunkload = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_t1);//检测存档保存的坐标
						bool ischunkload2 =NewModLoaderShengcheng.Dic_Chunk_Village3.ContainsKey(point_t1);//检测游戏进行时候的坐标

						if (ischunkload == false && ischunkload2 == false)
                        {
							await Task.Delay(250);
							await Shengcheng1.FCGenerateVillage(chunk_v, m_systemTerrain);
						}
							

						    /*if (distance_int < (visiblerange / 3 * 2) && distance_int2 < (visiblerange / 3 * 2)-3)//如果距离小于三分之
							{


								//Shengcheng1.AddChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//加载区块
								
								//Shengcheng1.RemoveChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//卸载区块

							}*/
						
						
							/*if (distance_int < visiblerange-32  && distance_int2 < visiblerange -32)//如果距离小于三分之
							{


								//Shengcheng1.AddChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//加载区块
								await Shengcheng1.FCGenerateVillage(chunk_v, m_systemTerrain);
								//Shengcheng1.RemoveChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//卸载区块

							}
						
						
							if (distance_int < (visiblerange / 3 * 2) && distance_int2 < 128)//如果距离小于三分之
							{


								//Shengcheng1.AddChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//加载区块
								await Shengcheng1.FCGenerateVillage(chunk_v, m_systemTerrain);
								//Shengcheng1.RemoveChunks007(chunk_v.Coords.X, chunk_v.Coords.Y);//卸载区块

							}*/
						
						
					       
					}

                    
				}
				if(listRD.Count != 0)//如果它依然没生成完毕，则重置一次
                {
					isvillageLoading = false;
                }
				if (listRD.Count == 0)
				{
					/*for (int i = 0; i < listRD.Count; i++)
                    {
						Shengcheng1.RemoveChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);
                    }*/
					for (int i = 0; i < listRD.Count; i++)
					{


					   Shengcheng1.RemoveChunks007(listRD[i].chunkCoords.X, listRD[i].chunkCoords.Y);


					}
					listRD.Clear();
					listBD.Clear();
					isvillageLoading = false;
					ischunkloding = false;
					
				}
			}
            #endregion
        }
    }


}
namespace Game
{
    #region 新生命组件
    public class FCComponentHealth : ComponentHealth, IUpdateable
    {
        
       

        public new void Update(float dt)
        {
            this.AttackResilience = this.m_attackResilience * this.AttackResilienceFactor;
            this.FallResilience = this.m_fallResilience * this.FallResilienceFactor;
            this.FireResilienceFactor = this.m_fireResilience * this.FireResilienceFactor;
            Vector3 position = this.m_componentCreature.ComponentBody.Position;
            if (this.Health > 0f && this.Health < 1f)
            {
                float num = 0f;
                if (this.m_componentPlayer != null)
                {
                    if (this.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Harmless)
                    {
                        num = 0.016666668f;
                    }
                    else if (this.m_componentPlayer.ComponentSleep.SleepFactor == 1f && this.m_componentPlayer.ComponentVitalStats.Food > 0f)
                    {
                        num = 0.0016666667f;
                    }
                    else if (this.m_componentPlayer.ComponentVitalStats.Food > 0.5f)
                    {
                        num = 0.0011111111f;
                    }
                }
                else
                {
                    num = 0.0011111111f;
                }
                this.Heal(this.m_subsystemGameInfo.TotalElapsedGameTimeDelta * num);
            }
            if (this.BreathingMode == BreathingMode.Air)
            {
                int cellContents = this.m_subsystemTerrain.Terrain.GetCellContents(Terrain.ToCell(position.X), Terrain.ToCell(this.m_componentCreature.ComponentCreatureModel.EyePosition.Y), Terrain.ToCell(position.Z));
                this.Air = ((BlocksManager.Blocks[cellContents] is FluidBlock || position.Y > 259f) ? MathUtils.Saturate(this.Air - dt / this.AirCapacity) : 1f);
            }
            else if (this.BreathingMode == BreathingMode.Water)
            {
                this.Air = ((this.m_componentCreature.ComponentBody.ImmersionFactor > 0.25f) ? 1f : MathUtils.Saturate(this.Air - dt / this.AirCapacity));
            }
            if (this.m_componentCreature.ComponentBody.ImmersionFactor > 0f && this.m_componentCreature.ComponentBody.ImmersionFluidBlock is MagmaBlock)
            {
                this.Injure(2f * this.m_componentCreature.ComponentBody.ImmersionFactor * dt, null, false, LanguageControl.Get(base.GetType().Name, 1));
                float num2 = 1.1f + 0.1f * (float)MathUtils.Sin(12.0 * this.m_subsystemTime.GameTime);
                this.m_redScreenFactor = MathUtils.Max(this.m_redScreenFactor, num2 * 1.5f * this.m_componentCreature.ComponentBody.ImmersionFactor);
            }
            float num3 = MathUtils.Abs(this.m_componentCreature.ComponentBody.CollisionVelocityChange.Y);
            if (!this.m_wasStanding && num3 > this.FallResilience)
            {
                float num4 = MathUtils.Sqr(MathUtils.Max(num3 - this.FallResilience, 0f)) / 15f;
                if (this.m_componentPlayer != null)
                {
                    num4 /= this.m_componentPlayer.ComponentLevel.ResilienceFactor;
                }
                this.Injure(num4, null, false, LanguageControl.Get(base.GetType().Name, 2));
            }
            this.m_wasStanding = (this.m_componentCreature.ComponentBody.StandingOnValue != null || this.m_componentCreature.ComponentBody.StandingOnBody != null);
           
            bool flag = this.m_subsystemTime.PeriodicGameTimeEvent(1.0, 0.0);
            if (flag && this.Air == 0f)
            {
                float num5 = 0.12f;
                if (this.m_componentPlayer != null)
                {
                    num5 /= this.m_componentPlayer.ComponentLevel.ResilienceFactor;
                }
                this.Injure(num5, null, false, LanguageControl.Get(base.GetType().Name, 7));
            }
            if (flag && (this.m_componentOnFire.IsOnFire || this.m_componentOnFire.TouchesFire))
            {
                float num6 = 1f / this.FireResilience;
                if (this.m_componentPlayer != null)
                {
                    num6 /= this.m_componentPlayer.ComponentLevel.ResilienceFactor;
                }
                this.Injure(num6, this.m_componentOnFire.Attacker, false, LanguageControl.Get(base.GetType().Name, 5));
            }
            if (flag && this.CanStrand && this.m_componentCreature.ComponentBody.ImmersionFactor < 0.25f)
            {
                int? standingOnValue = this.m_componentCreature.ComponentBody.StandingOnValue;
                int num7 = 0;
                if (!(standingOnValue.GetValueOrDefault() == num7 & standingOnValue != null) || this.m_componentCreature.ComponentBody.StandingOnBody != null)
                {
                    this.Injure(0.05f, null, false, LanguageControl.Get(base.GetType().Name, 6));
                }
            }
            this.HealthChange = this.Health - this.m_lastHealth;
            this.m_lastHealth = this.Health;
            if (this.m_redScreenFactor > 0.01f)
            {
                this.m_redScreenFactor *= MathUtils.Pow(0.2f, dt);
            }
            else
            {
                this.m_redScreenFactor = 0f;
            }
            if (this.HealthChange < 0f)
            {
                this.m_componentCreature.ComponentCreatureSounds.PlayPainSound();
                this.m_redScreenFactor += -4f * this.HealthChange;
                ComponentPlayer componentPlayer2 = this.m_componentPlayer;
                if (componentPlayer2 != null)
                {
                    componentPlayer2.ComponentGui.HealthBarWidget.Flash(MathUtils.Clamp((int)((0f - this.HealthChange) * 30f), 0, 10));
                }
            }
            if (this.m_componentPlayer != null)
            {
                this.m_componentPlayer.ComponentScreenOverlays.RedoutFactor = MathUtils.Max(this.m_componentPlayer.ComponentScreenOverlays.RedoutFactor, this.m_redScreenFactor);
            }
            if (this.m_componentPlayer != null)
            {
                this.m_componentPlayer.ComponentGui.HealthBarWidget.Value = this.Health;
            }
            if (this.Health == 0f && this.HealthChange < 0f)
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
                    Vector3 position2 = this.m_componentCreature.ComponentBody.Position + new Vector3(0f, this.m_componentCreature.ComponentBody.StanceBoxSize.Y / 2f, 0f);
                    float x = this.m_componentCreature.ComponentBody.StanceBoxSize.X;
                    this.m_subsystemParticles.AddParticleSystem(new KillParticleSystem(this.m_subsystemTerrain, position2, x));
                    Vector3 position3 = (this.m_componentCreature.ComponentBody.BoundingBox.Min + this.m_componentCreature.ComponentBody.BoundingBox.Max) / 2f;
                    foreach (IInventory inventory in base.Entity.FindComponents<IInventory>())
                    {
                        inventory.DropAllItems(position3);
                    }
                    this.DeathTime = new double?(this.m_subsystemGameInfo.TotalElapsedGameTime);
                }
            }
            if (this.Health <= 0f && this.CorpseDuration > 0f)
            {
                double? num8 = this.m_subsystemGameInfo.TotalElapsedGameTime - this.DeathTime;
                double num9 = (double)this.CorpseDuration;
                if (num8.GetValueOrDefault() > num9 & num8 != null)
                {
                    this.m_componentCreature.ComponentSpawn.Despawn();
                }
            }
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

    #region 1.0版本物品
    public class Dangao : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/dangao", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 962;

		private Texture2D m_texture;
	}

	public class Beer : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/beer", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 901;

		private Texture2D m_texture;
	}

	public class Boliping : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/boliping", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 902;

		private Texture2D m_texture;
	}

	public class Cong : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/cong", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 903;

		private Texture2D m_texture;
	}

	public class Guazi : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/guazi", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 904;

		private Texture2D m_texture;
	}

	public class Hamburger : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/hamburger", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 905;

		private Texture2D m_texture;
	}

	public class Huangmengji : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/huangmengji", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 906;

		private Texture2D m_texture;
	}

	public class Jiangyou : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/jiangyou", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 907;

		private Texture2D m_texture;
	}

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

		public const int Index = 908;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}

	

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

		public const int Index = 910;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}

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

		public const int Index = 911;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}

	public class Mianbaopian : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/mianbaopian", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 912;

		private Texture2D m_texture;
	}

	public class Qingtuan : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/qingtuan", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 913;

		private Texture2D m_texture;
	}

	public class Qingzhengyu : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/qingzhengyu", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 914;

		private Texture2D m_texture;
	}

	public class Rawguazi : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/rawguazi", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 915;

		private Texture2D m_texture;
	}

	public class Rawjikuai : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/rawjikuai", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 916;

		private Texture2D m_texture;
	}

	public class Rawxiandan : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/rawxiandan", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 917;

		private Texture2D m_texture;
	}

	public class Readymeat : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/readymeat", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 918;

		private Texture2D m_texture;
	}

	public class Salt : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/salt", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 919;

		private Texture2D m_texture;
	}

	public class Shousi : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/shousi", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 920;

		private Texture2D m_texture;
	}

	public class Shuxiandan : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/shuxiandan", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 921;

		private Texture2D m_texture;
	}

	public class Xianbing : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/xianbing", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 922;

		private Texture2D m_texture;
	}

	public class Xiaosurou : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/xiaosurou", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 923;

		private Texture2D m_texture;
	}

	public class Yanchicken : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/yanchicken", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 924;

		private Texture2D m_texture;
	}

	public class Yanfish : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/yanfish", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 925;

		private Texture2D m_texture;
	}


	public class Yanrou : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/yanrou", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 926;

		private Texture2D m_texture;
	}


	public class Youping : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/youping", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 927;

		private Texture2D m_texture;
	}

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

		public const int Index = 928;

		public BlockMesh m_standaloneBlockMesh = new BlockMesh();
	}

	public class Zhaji : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/zhaji", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 929;

		private Texture2D m_texture;

	}

	public class Xiguapian : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/xiguapian", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 930;

		private Texture2D m_texture;

	}
    #endregion 

    #region  1.06以后的食物，西瓜汁-橘子
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

	}

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

	}

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

	}

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



	public class Juzi : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 950;

		private Texture2D m_texture;
	}

	public class Pingguo : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 951;

		private Texture2D m_texture;
	}

	public class Cocobean : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 952;

		private Texture2D m_texture;
	}

	public class Cocofeng : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 957;

		private Texture2D m_texture;
	}

	public class Chocolate : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 959;

		private Texture2D m_texture;
	}

	public class RawChocolate : FlatBlock
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
			this.m_texture = ContentManager.Get<Texture2D>("Textures/amod/rawqiaokeli", null);
		}

		public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
		{
		}

		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 960;

		private Texture2D m_texture;
	}


	public class YHBeer : FlatBlock
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
			BlocksManager.DrawFlatBlock(primitivesRenderer, value, size * 1f, ref matrix, this.m_texture, Color.White, true, environmentData);
		}

		public const int Index = 968;

		private Texture2D m_texture;
	}

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
	}

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
	}
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

    public class ShuipingBlock : FCTwoDBlock
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

		public ShuipingBlock()
		   : base("Textures/amod/shuiping")
		{
		}

		public const int Index = 965;
	}

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


		public class FCYHGrassBlock : Block
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
		}

		public class FCRDGrassBlock : Block
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
		}
	}



	#endregion
	#endregion

	#region 物品子系统
	public class ChocolateSystem : SubsystemBlockBehavior
	{
		
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
			int F987ID = Terrain.ExtractContents(componentMiner.ActiveBlockValue);
			if (componentPlayer != null)
			{
				if(componentMiner.ActiveBlockValue==959)
                {
					componentPlayer.ComponentVitalStats.Food = 1f;
					componentPlayer.ComponentVitalStats.Stamina +=0.7f;//耐力
					
					componentPlayer.ComponentGui.DisplaySmallMessage("吃了巧克力,你不再感到饥饿。你感觉浑身充满了力量！（耐力和饱食度大量恢复！）", Color.White, true, true);
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
					
					
					buffManager.AddBuff(1, 2f, 1.5f);
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

					componentPlayer.ComponentGui.DisplaySmallMessage("喝下樱花酒，你的攻击力和速度略微提升！", Color.White, true, true);
					AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
					componentMiner.RemoveActiveTool(1);
                }//樱花酒
				else if(F987ID == 987)
				{
					if(Terrain.ExtractData(componentMiner.ActiveBlockValue) ==6)
					{
						buffManager.AddBuff(9, 20f);
                        componentPlayer.ComponentGui.DisplaySmallMessage("喝下夜视药水，你终于摆脱了无尽的黑夜！", Color.Purple, true, true);
                        AudioManager.PlaySound("Audio/Creatures/Drink/drink1", 1f, 0f, 0f);
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
			
		}

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
			0.7f,
			0.7f,
			0.7f,
			0.6f,
			0.5f,
			0.5f,
			1f,
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
			if (num < 0 || num >= FCDVFoodBlock.m_displayNames.Length)
			{
				return string.Empty;
			}
			return FCDVFoodBlock.m_displayNames[num];
		}

		private static string[] m_displayNames = new string[]
		{
            "实体灵能",
            "基础的闪烁西瓜",
            "基础的闪烁苹果",
            "基础的闪烁橘子",
            "基础的闪烁可可豆",
            "基础的闪烁可可粉",
            "夜视药水I",
			"酵素粉enzyme powder",
			"硅板silicon plate",
			"精制淀粉refined starch",
			"偏导芯片bias chip",
			"齿轮gear",
			"钢棒steel rod",
			"酵母菌yeast",
			"活塞universal piston",
			"钢锭steelIngot",
			"碳粉",
			"铁粉",
			"磁化铁棒",
			"基础马达",


		};

		private static string[] m_textureNames = new string[]
		{
			"Textures/FCDVFood/tongban",
			"Textures/FCDVFood/tieban",
			"Textures/FCDVFood/gangban",
			"Textures/FCDVFood/tongxianquan",
			"Textures/FCDVFood/gui",
			"Textures/FCDVFood/xianluban",
			"Textures/FCDVFood/yeshi",
			"Textures/FCDVFood/jiaosufeng",
			"Textures/FCDVFood/guiban",
			"Textures/FCDVFood/DVfeng",
			"Textures/FCDVFood/PDU",
			"Textures/FCDVFood/chilun",//齿轮
			"Textures/FCDVFood/gangbang",
			"Textures/FCDVFood/jiaomujun",
			"Textures/FCDVFood/huosai",//活塞
			"Textures/FCDVFood/gangding",
			"Textures/FCDVFood/tanfeng",
			"Textures/FCDVFood/tiefeng",
			"Textures/FCDVFood/cihuatiebang",
			"Textures/FCDVFood/mada",

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
			gangban,//3基础的闪烁苹果
			Tongxianquan,//4.基础的闪烁橘子
			SI,//5.基础的闪烁可可豆
			Xianluban,//6基础的闪烁可可粉
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
    #endregion
    #region buff体系
    public  class BuffManager
    {
        public ComponentPlayer componentPlayer;

        public BuffManager(ComponentPlayer player)
        {
            componentPlayer = player;
        }
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
                    float MaxDuration = 20f;//提供统一接口参数
					float MaxRate = 3f;
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

							if(componentHealBuffA.m_HealRate> MaxRate)//倍率至多为3
							{
								componentHealBuffA.m_HealRate = MaxRate;
							}
							else if (componentHealBuffA.HealingRate< MaxRate)
							{
                                componentHealBuffA.m_HealRate = 1.1f * componentHealBuffA.m_HealRate;
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
					float MaxDurationATK= 20f;    
					float MaxATK= 15f;
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
                    float MaxDurationSpeed = 20f;
                    float MaxSpeed = 3f;
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
                    float MaxDurationNightsee = 20f;
                   
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
                                componentNightsight .m_NightseeDuration += 10f;//加10秒
                                if (componentNightsight .m_NightseeDuration > MaxDurationNightsee)
                                {
                                    componentNightsight.m_NightseeDuration = MaxDurationNightsee;
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
		public const int blind = 4;
		public const int AttackDown = 5;
		public const int SpeedDown = 6;
		public const int Nofire = 7;
		public const int onfire = 8;
        public const int Nightsight = 9;


    }
	#region buff区域

	public class ComponentHealBuffA : Component, IUpdateable
	{
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
			if(IsActive == true)
            {
				if (m_HealDuration > 0 && HealingRate > 0)//如果持续时间大于0，说明处于生命恢复状态
				{
					
					m_componentPlayer.ComponentHealth.Heal((1f / m_componentPlayer.ComponentHealth.AttackResilience) * HealingRate*dt);//生命恢复=1x恢复速率
					m_HealDuration = m_HealDuration - dt;
					if (m_HealDuration <= 0)
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
			m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);
			
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
					
					m_ATKDuration = m_ATKDuration - dt;
					if (m_ATKDuration <= 0)
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
			m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);

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

		public float m_AttackRate;//保存攻击加成速率
		public float m_ATKDuration;//buff持续时间，需要在存档读取。
		public Random m_random = new Random();


	}
	public class ComponentSpeedUP : Component, IUpdateable
	{
		public bool isadded = false;
		public float Speed_first;
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
				isadded = false;
				m_componentPlayer.ComponentLocomotion.WalkSpeed= Speed_first;
				Speed_first = 4f;
			}

		}

		public void Update(float dt)
		{
			if (IsActive == true)
			{
				if (m_SpeedDuration > 0 && SpeedRate > 0)//如果持续时间大于0，说明处于生命恢复状态
				{
					if (isadded == false)
					{
						Speed_first = m_componentPlayer.ComponentLocomotion.WalkSpeed;
						m_componentPlayer.ComponentLocomotion.WalkSpeed = m_componentPlayer.ComponentLocomotion.WalkSpeed * (1f + SpeedRate / 100) + 1;
						isadded = true;
					}

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
			m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);

			m_SpeedDuration = valuesDictionary.GetValue<float>("SpeedDuration", 0f);
			m_SpeedRate = valuesDictionary.GetValue<float>("SpeedRate", 1f);

		}
		public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
		{
			valuesDictionary.SetValue<float>("SpeedDuration", m_SpeedDuration);
			valuesDictionary.SetValue<float>("SpeedRate", m_SpeedRate);
		}
		public SubsystemGameInfo m_subsystemGameInfo;

		public SubsystemTerrain m_subsystemTerrain;

		public SubsystemTime m_subsystemTime;

		public SubsystemAudio m_subsystemAudio;

		public SubsystemParticles m_subsystemParticles;
		public ComponentPlayer m_componentPlayer;

		public float m_SpeedRate;//保存速率加成
		public float m_SpeedDuration;//buff持续时间，需要在存档读取。
		public Random m_random = new Random();


	}
    public class ComponentBlind : Component, IUpdateable
    {
        public bool isadded = false;
        public float Speed_first;
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
                isadded = false;
                m_componentPlayer.ComponentLocomotion.WalkSpeed = Speed_first;
                Speed_first = 4f;
            }

        }

        public void Update(float dt)
        {
            if (IsActive == true)
            {
                if (m_SpeedDuration > 0 && SpeedRate > 0)//如果持续时间大于0，说明处于生命恢复状态
                {
                    if (isadded == false)
                    {
                        Speed_first = m_componentPlayer.ComponentLocomotion.WalkSpeed;
                        m_componentPlayer.ComponentLocomotion.WalkSpeed = m_componentPlayer.ComponentLocomotion.WalkSpeed * (1f + SpeedRate / 100) + 1;
                        isadded = true;
                    }

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
            m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>(true);

            m_SpeedDuration = valuesDictionary.GetValue<float>("SpeedDuration", 0f);
            m_SpeedRate = valuesDictionary.GetValue<float>("SpeedRate", 1f);

        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue<float>("SpeedDuration", m_SpeedDuration);
            valuesDictionary.SetValue<float>("SpeedRate", m_SpeedRate);
        }
        public SubsystemGameInfo m_subsystemGameInfo;

        public SubsystemTerrain m_subsystemTerrain;

        public SubsystemTime m_subsystemTime;

        public SubsystemAudio m_subsystemAudio;

        public SubsystemParticles m_subsystemParticles;
        public ComponentPlayer m_componentPlayer;

        public float m_SpeedRate;//保存速率加成
        public float m_SpeedDuration;//buff持续时间，需要在存档读取。
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
			ValuesDictionary dicChunkVillage = valuesDictionary.GetValue<ValuesDictionary>("Dic_Chunk_Village",null);
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
				Dic_Chunk_Village.Add((0,0),0);
			}
			
		}

		public override void Save(ValuesDictionary valuesDictionary)
		{
			if(m_modloader.tg_num == 0)
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
		public Test1.ComponentTest1 m_component;

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
		public FCSubsystemTownChunk m_subsystemtownchunk ;
		public SubsystemSky m_subsystemSky;
        public bool IsNightVisionActive { get; set; } 
        public SubsystemPlayers m_subsystemPlayers;
        //public  Dictionary<Point2, int> Dic_Chunk_Village ;  //村庄区块


        public override void __ModInitialize()
		{
			ModsManager.RegisterHook("OnTerrainContentsGenerated", this);
			ModsManager.RegisterHook("OnProjectLoaded", this);
			ModsManager.RegisterHook("AttackBody", this);
			ModsManager.RegisterHook("InitializeCreatureTypes", this);
			ModsManager.RegisterHook("ToFreeChunks", this);
			ModsManager.RegisterHook("ToAllocateChunks", this);
			ModsManager.RegisterHook("CalculateLighting", this);
		}
		public override void OnProjectLoaded(Project project)
		{

			m_subsystemGameInfo = project.FindSubsystem<SubsystemGameInfo>();
			m_seed = m_subsystemGameInfo.WorldSeed;
			m_subsystemTerrain = project.FindSubsystem<SubsystemTerrain>();
			m_worldSettings = m_subsystemGameInfo.WorldSettings;
			m_subsystemParticles = project.FindSubsystem<SubsystemParticles>();
			m_subsystemtown = project.FindSubsystem<FCSubsystemTown>();
			m_subsystemtownchunk = project.FindSubsystem<FCSubsystemTownChunk>();
			m_subsystemPlayers = project.FindSubsystem<SubsystemPlayers>();
			
			TGExtras = true;
			TGCavesAndPockets = true;
		}
		public override void OnTerrainContentsGenerated(TerrainChunk chunk)
		{
			生成西瓜(chunk);//生成西瓜测试
			生成向日葵(chunk);
			generateMineralLikeCoal(chunk, 934, 3, 0, 200);
			//GenerateFCTrees(chunk);
			//GenerateFCCaves(chunk);//洞穴
			GenerateFCPockets(chunk);//空腔地形生成，岩浆空腔
			FCGenerateSurface(chunk);
			//FCGenerateVillage(chunk);
			

		}
		/// <summary>
		/// </summary>
		/// 

		#region 区块强制加载
		public List<Point2> m_terrainChunks007 = new List<Point2>();
		public override void ToFreeChunks(TerrainUpdater terrainUpdater, TerrainChunk chunk, out bool KeepWorking)
		{
			KeepWorking = (m_terrainChunks007.Contains(chunk.Coords));
		}

        public override void CalculateLighting(ref float brightness)
        {

			/*if(IsNightVisionActive ==false)
			{

				
            }
			else
			{
                brightness = 5f;
            }
            /*ComponentPlayer componentPlayer;
            if (m_subsystemPlayers.ComponentPlayers.Count > 0)
            {
				foreach(ComponentPlayer player in m_subsystemPlayers.ComponentPlayers)
				{
                    componentPlayer = player;
                    var componentNightsight = componentPlayer.Entity.FindComponent<ComponentNightsight>();
					if(componentNightsight.m_NightseeDuration>0)
					{
						brightness = 5f;
					}
					else
					{
						brightness = SettingsManager.Brightness;
					}

                }
               
            }
            else
            {
                componentPlayer = null;
            }*/
            
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
					int temperature = m_subsystemTerrain.Terrain.GetTemperature(point.X, point.Z);
					int humidity = m_subsystemTerrain.Terrain.GetHumidity(point.X, point.Z);
					int num = Terrain.ExtractContents(m_subsystemTerrain.Terrain.GetCellValueFast(point.X, point.Y - 1, point.Z));
					if ((num != 3 && num != 67 && num != 4 && num != 66 && num != 2 && num != 7) || temperature <= 2 || humidity <2)
					{
						return 0f;
					}
					return 0.85f;
				},
				SpawnFunction = ((SubsystemCreatureSpawn.CreatureType creatureType, Point3 point) => spawn.SpawnCreatures(creatureType, "Cave_Spider", point, 1).Count)//3是生成数量
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

			Game.Random random = new Game.Random(17);
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
								if(treeType == FCTreeType.Yinghua)
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
		public void GenerateFCCaves(TerrainChunk chunk)
		{
			if (!this.TGCavesAndPockets)
			{
				return;
			}
			List<NewModLoaderShengcheng.CavePoint> list = new List<NewModLoaderShengcheng.CavePoint>();//创建一个名为list的空列表，用于存储洞穴点的信息。
			int x = chunk.Coords.X;//获取chunk对象的Coords属性的X成员，并将其赋值给整型变量x。
			int y = chunk.Coords.Y;//获取chunk对象的Coords属性的Y成员，并将其赋值给整型变量y。
			for (int i = x - 2; i <= x + 2; i++)//执行一个循环，循环变量i从x-2开始，一直遍历到x+2。
			{
				for (int j = y - 2; j <= y + 2; j++)//在外层循环内部，执行一个嵌套循环，循环变量j从y-2开始，一直遍历到y+2。
				{
					if(list.Count >100)
                    {
						list.Clear();
					}//清空列表list，准备存储新的洞穴点的信息。
					Random random = new Random(this.m_seed + i + 9973 * j); //创建一个名为random的Random对象，使用一个种子值来初始化。
					int num = i * 16 + random.Int(0, 15);//根据当前循环变量i计算出一个x坐标，乘以16得到具体的地块内的x坐标。
														 //根据当前嵌套循环变量j计算出一个y坐标，乘以16得到具体的地块内的y坐标。
														 //然后使用随机数生成器random生成一个0到15之间的随机整数，加上前面计算的y坐标，得到num2。然后使用随机数生成器random生成一个0到15之间的随机整数，加上前面计算的y坐标，得到num2。

					int num2 = j * 16 + random.Int(0, 15);
					float probability = 1f;
					//使用随机数生成器random调用Bool方法，传入probability作为参数。
					//这个方法根据给定的概率返回一个随机的布尔值
					//如果返回的布尔值为true，表示概率为probability的情况发生。
					if (random.Bool(probability))
					{
						//调用当前对象的CalculateHeight方法，传入num和num2作为参数，获取一个高度值。
						//将高度值转换为整型，并将其赋值给变量num3。
						int num3 = (int)this.CalculateHeight((float)num, (float)num2);

						//调用当前对象的CalculateHeight方法，传入num+3和num2作为参数，获取一个高度值。
						//将高度值转换为整型，并将其赋值给变量num4。
						int num4 = (int)this.CalculateHeight((float)(num + 3), (float)num2);

						//调用当前对象的CalculateHeight方法，传入num和num2+3作为参数，获取一个高度值。
						//将高度值转换为整型，并将其赋值给变量num5。
						int num5 = (int)this.CalculateHeight((float)num, (float)(num2 + 3));

						//创建一个名为position的Vector3对象，它的坐标由num、num3-1和num2组成。
						//这个位置表示洞穴点的起始位置，y坐标减1表示在地面下方。
						Vector3 position = new Vector3((float)num, (float)(num3 - 1), (float)num2);

						//创建一个名为v的Vector3对象，它的坐标由3、num4-num3和0组成。
						//这个向量表示洞穴点在x方向上的延伸。
						Vector3 v = new Vector3(3f, (float)(num4 - num3), 0f);

						//创建一个名为v2的Vector3对象，它的坐标由0、num5-num3和3组成。
						//这个向量表示洞穴点在z方向上的延伸。
						Vector3 v2 = new Vector3(0f, (float)(num5 - num3), 3f);

						//创建一个名为vector的Vector3对象，它是v和v2的叉积向量的单位向量形式。
						//这个向量表示洞穴点的方向。
						Vector3 vector = Vector3.Normalize(Vector3.Cross(v, v2));


						if (vector.Y > -0.6f)//检查vector的Y坐标是否大于-0.6。
						{
							//如果满足条件，说明洞穴点的方向与地表接近垂直，可以被加入到生成的洞穴列表中。

							//创建一个新的CavePoint对象，设置它的Position属性为position，Direction属性为vector，BrushType属性为0，Length属性为一个在80到240之间的随机整数。
							//将这个对象添加到列表list中。
							list.Add(new NewModLoaderShengcheng.CavePoint
							{
								Position = position,//洞穴起始位置
								Direction = vector,//洞穴方向
								BrushType = 0,
								Length = random.Int(80, 240)
							});
										
						}
						else
                        {
							if(position.Y <64)
                            {
								list.Add(new NewModLoaderShengcheng.CavePoint
								{
									Position = position,//洞穴起始位置
									Direction = vector,//洞穴方向
									BrushType = 0,
									Length = random.Int(80, 240)
								});
							}
                        }

						//根据当前循环变量i计算出一个x坐标，乘以16得到具体的地块内的x坐标。
						//然后加上8，得到num6。
						int num6 = i * 16 + 8;
						int num7 = j * 16 + 8;////根据当前嵌套循环变量j计算出一个y坐标，乘以16得到具体的地块内的y坐标。然后加上8，得到num7。
						int k = 0;

						while (k < list.Count)//执行一个循环，循环条件是k小于列表list的元素个数。
						{
							//获取列表list中第k个元素，并将其赋值给变量cavePoint。
							NewModLoaderShengcheng.CavePoint cavePoint = list[k];
							//根据cavePoint的BrushType属性从caveBrushesByType列表中获取对应的笔刷列表，将其赋值给list2。
							List<TerrainBrush> list2 = NewModLoaderShengcheng.m_fccaveBrushesByType[cavePoint.BrushType];

							//从list2中随机选择一个笔刷，然后调用该笔刷的PaintFastAvoidWater方法来在指定的地块中绘制洞穴。
							//PaintFastAvoidWater方法接收chunk对象和cavePoint的Position属性的各个坐标值（转换为整型）作为参数。

							list2[random.Int(0, list2.Count - 1)].PaintFastAvoidWater(chunk, Terrain.ToCell(cavePoint.Position.X), Terrain.ToCell(cavePoint.Position.Y), Terrain.ToCell(cavePoint.Position.Z));
							
							
							//将cavePoint的Position属性增加2倍的cavePoint的Direction属性。
							//这样就更新了洞穴点的位置。
							cavePoint.Position += 2f * cavePoint.Direction;
							cavePoint.StepsTaken += 2;//将cavePoint的StepsTaken属性增加2。这表示洞穴点已经前进了两个步骤。
							float num8 = cavePoint.Position.X - (float)num6;//计算cavePoint的Position属性的X坐标与num6之间的差值，赋值给num8。
							float num9 = cavePoint.Position.Z - (float)num7;//计算cavePoint的Position属性的Z坐标与num7之间的差值，赋值给num9。

							if (random.Bool(0.5f))//使用随机数生成器random调用Bool方法，传入概率0.5f作为参数。/这个0.5表示有百分之50的概率改变洞穴方向，取反操作。
							{
								//使用随机数生成器random调用Vector3方法，生成一个随机的三维向量。
								//然后对该向量进行单位化，得到一个单位向量，并将其赋值给vector2。
								Vector3 vector2 = Vector3.Normalize(random.Vector3(1f));

								//检查num8是否小于-25.5，并且vector2的X坐标是否小于0，或者num8是否大于25.5，并且vector2的X坐标是否大于0。
								//如果满足其中一种情况，将vector2的X坐标取相反数。
								if ((num8 < -25.5f && vector2.X < 0f) || (num8 > 25.5f && vector2.X > 0f))
								{
									vector2.X = 0f - vector2.X;
								}

								//检查num9是否小于-25.5，并且vector2的Z坐标是否小于0，或者num9是否大于25.5，并且vector2的Z坐标是否大于0。
								//如果满足其中一种情况，将vector2的Z坐标取相反数。
								if ((num9 < -25.5f && vector2.Z < 0f) || (num9 > 25.5f && vector2.Z > 0f))
								{
									vector2.Z = 0f - vector2.Z;
								}


								//检查cavePoint的Direction属性的Y坐标是否小于-0.5，并且vector2的Y坐标是否小于-10，或者cavePoint的Direction属性的Y坐标是否大于0.1，并且vector2的Y坐标是否大于0。
								//如果满足其中一种情况，将vector2的Y坐标取相反数。
								if ((cavePoint.Direction.Y < -0.5f && vector2.Y < -10f) || (cavePoint.Direction.Y > 0.1f && vector2.Y > 0f))
								{
									vector2.Y = 0f - vector2.Y;
								}
								//将cavePoint的Direction属性加上0.5倍的vector2，然后对结果进行单位化。
								//这样更新了洞穴点的方向。
								cavePoint.Direction = Vector3.Normalize(cavePoint.Direction + 0.5f * vector2);
							}

							//第二套判断
							//检查cavePoint的StepsTaken属性是否大于20，且随机数生成器random返回的布尔值为true，概率为0.06。
							if (cavePoint.StepsTaken > 20 && random.Bool(0.06f))
							{
								//在满足条件的情况下，使用随机数生成器random调用Vector3方法，生成一个随机的三维向量。
								//然后将这个向量乘以(1, 0.33, 1)，得到一个新的向量，然后对其进行单位化。
								//这个向量作为新的洞穴点的方向。
								cavePoint.Direction = Vector3.Normalize(random.Vector3(1f) * new Vector3(1f, 0.33f, 1f));
							}

							//检查cavePoint的StepsTaken属性是否大于20，且随机数生成器random返回的布尔值为true，概率为0.05。
							if (cavePoint.StepsTaken > 20 && random.Bool(0.5f))
							{
								//将cavePoint的Direction属性的Y坐标设为0。
								cavePoint.Direction.Y = 0f;
								//将cavePoint的BrushType属性增加2，并将结果与caveBrushesByType列表的元素个数-1取较小值。
								//这个操作用于更新洞穴点的笔刷类型。
								cavePoint.BrushType = MathUtils.Min(cavePoint.BrushType + 2, NewModLoaderShengcheng.m_fccaveBrushesByType.Count - 1);
							}

							//检查cavePoint的StepsTaken属性是否大于30，且随机数生成器random返回的布尔值为true，概率为0.03。
							if (cavePoint.StepsTaken > 20 && random.Bool(0.03f))
							{
								//将cavePoint的Direction属性的X坐标设为0。 //洞穴垂直向下
								cavePoint.Direction.X = 0f;
								cavePoint.Direction.Y = -1f;
								cavePoint.Direction.Z = 0f;
							}
							//检查cavePoint的StepsTaken属性是否大于30，且cavePoint的Position属性的Y坐标是否小于30，且随机数生成器random返回的布尔值为true，概率为0.02。
							if (cavePoint.StepsTaken > 20 && cavePoint.Position.Y < 30f && random.Bool(0.02f))
							{
								cavePoint.Direction.X = 0f;
								cavePoint.Direction.Y = 1f; //洞穴垂直向上
								cavePoint.Direction.Z = 0f;
							}

							//使用随机数生成器random调用Bool方法，传入概率0.33f作为参数。
							//如果返回的布尔值为true，表示概率为0.33的情况发生。

							if (random.Bool(0.33f))//修改点1
							{
								//在满足条件的情况下，通过随机数生成器random生成一个0到0.999之间的随机浮点数。
								//将这个浮点数的7次方乘以TerrainContentsGenerator23.m_caveBrushesByType.Count，然后取整数部分，赋值给cavePoint的BrushType属性。
								//这个操作基于随机数生成的概率，选择了一个新的洞穴笔刷的类型。
								cavePoint.BrushType = (int)(MathUtils.Pow(random.Float(0f, 0.999f), 7f) * (float)NewModLoaderShengcheng.m_fccaveBrushesByType.Count);
							}

							//检查随机数生成器random返回的布尔值为true，概率为0.06，且列表list的元素个数小于12，
							//且cavePoint的StepsTaken属性大于20，且cavePoint的Position属性的Y坐标小于58。
							if (random.Bool(0.76f) || list.Count < 30 || cavePoint.StepsTaken > 20 || cavePoint.Position.Y < 58f)//修改点
							{
								//在满足条件的情况下，创建一个新的CavePoint对象，设置它的Position属性为cavePoint的Position属性，
								//Direction属性为一个通过随机数生成器random生成的向量进行单位化得到的向量，
								//BrushType属性为一个根据随机数生成的概率得到的整数，
								//Length属性为一个在40到180之间的随机整数。
								//添加到列表
								list.Add(new NewModLoaderShengcheng.CavePoint
								{
									Position = cavePoint.Position,
									Direction = Vector3.Normalize(random.Vector3(1f, 1f) * new Vector3(1f, 0.33f, 1f)),
									BrushType = (int)(MathUtils.Pow(random.Float(0f, 0.999f), 7f) * (float)NewModLoaderShengcheng.m_fccaveBrushesByType.Count),
									Length = random.Int(40, 180)
								});
							}
							//检查cavePoint的StepsTaken属性是否大于等于cavePoint的Length属性，
							//或者num8的绝对值是否大于34，或者num9的绝对值是否大于34，
							//或者cavePoint的Position属性的Y坐标是否小于5或大于246。
							if (cavePoint.StepsTaken >= cavePoint.Length  || cavePoint.Position.Y < 5f || cavePoint.Position.Y > 246f)
							{
								
								//如果满足其中一种情况，则将k的值增加1，进入下一个洞穴点的处理。
								k++;
							}
							//否则，如果cavePoint的StepsTaken属性能够被20整除，即取模等于0。
							else if (cavePoint.StepsTaken % 20 == 0)
							{
								//调用CalculateHeight方法，传入cavePoint的Position属性的X和Z坐标作为参数，获取一个高度值。
								//如果cavePoint的Position属性的Y坐标大于num10加上1。
								//将k的值增加1，进入下一个洞穴点的处理。
								float num10 = this.CalculateHeight(cavePoint.Position.X, cavePoint.Position.Z);
								if (cavePoint.Position.Y > num10 + 1f)
								{
									
									k++;
									//整个循环的目的是对洞穴点列表中的每个点进行处理，包括更新位置、方向，绘制洞穴，
									//并根据一些条件判断是否继续处理当前洞穴点。当满足结束条件时，进入下一个洞穴点的处理。
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
				new NumberWeightPair(0,	80),   // 空气的权重为
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
					if(num81 - 15 >0)
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
						int num86 = random.Int(6, max);//随机生成盒子的尺寸，范围为2到max之间。
						int num87 = random.Int(11, max2) -   num86;//随机生成盒子的数量，范围为8到max2之间，减去2倍盒子的尺寸。
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
			ComponentPlayer m_componentPlayer= new ComponentPlayer();
			FCSubsystemTown fCSubsystemTown  = new FCSubsystemTown();
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
			Vector3 player_position =m_subsystemTerrain.TerrainContentsGenerator.FindCoarseSpawnPosition();//获取玩家出生点
			
			Point2 player_positon2 = ((int)player_position.X, (int)player_position.Z);//转化成point2坐标
			Point2 point_village_T = (x1, y1);//获取当前区块的绝对坐标，用于和区块字典进行比对，判断是否为村庄区块，如果是村庄区块则不进行起点生成。

			Terrain terrain = this.m_subsystemTerrain.Terrain;
			int middle_y = terrain.CalculateTopmostCellHeight(middle_x, middle_z);//获取当前区块中心点的高度
			Random random = new Random(this.m_seed + chunk.Coords.X + 101 * chunk.Coords.Y);
			
			
			List<int> height = new List<int>(); // 创建一个空的 List<int>,储存区块高度

			bool isvillage_chunk = FCSubsystemTownChunk.Dic_Chunk_Village.ContainsKey(point_village_T);
			bool ischunkload2 = Dic_Chunk_Village3.ContainsKey(point_village_T);

			if (listRD.Count == 0 && isvillage_chunk == false&& ischunkload2 == false);//如果路径点为空，且不为村庄区块，则执行遍历区块，村庄符合第一条件。
			{
				int num_villageRange=0;//用来检测村庄之间的距离，计数。


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

				if(num_villageRange<Village_start.Count)
                {
					for (int i1 = 0; i1 < Village_start.Count; i1++)
					{
						int point_x = Village_start[i1].X;
						int point_z = Village_start[i1].Z;

						double distance1 = Math.Sqrt(((Math.Abs(middle_x - point_x)) * (Math.Abs(middle_x - point_x))) + ((Math.Abs(middle_z - point_z)) * (Math.Abs(middle_z - point_z))));
						if (distance1 > 1000)
						{
							num_villageRange++;
						}

					}
				}
				
				if (height1 < 5 && middle_y > 64 && middle_y < 84 && Village_start.Count == 0&&random.Bool(0.73f))//当地图还没有村庄的时候，先找到第一个可生成点生成村庄。
                {


					double distance1 = Math.Sqrt(((Math.Abs(middle_x - player_positon2.X)) * (Math.Abs(middle_x - player_positon2.X))) + ((Math.Abs(middle_z - player_positon2.Y)) * (Math.Abs(middle_z - player_positon2.Y))));
					
					if(distance1 > 500)//大于出生点300距离
                    {
						flag_v = true;
					}
					
                }
				if (height1 < 5 && middle_y > 64 && middle_y < 84 &&ischunkloding2 == false&& num_villageRange == Village_start.Count && num_villageRange != 0 && Village_start.Count != 0 && random.Bool(0.73f))//如果高度差小于5，且中心点位于高度66-86之间,距离每一个村庄点都大于300。生成村庄。
				{
					flag_v = true;
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
							int num6 = 8 ;//定义一个整数6，这代表要生成的内容。

							int num8 = (k + 1 < 255) ? chunk.GetCellContentsFast(i, k + 1, j) : 0;
							int numfc1 = (k + 1 < 255) ? chunk.GetCellContentsFast(i, k - 1, j) : 0;
							//获取当前网格点上方单元格的内容。
							//下面是核心判断生成语句

							if (num4 == 8)//如果是草方块
							{
								

								

								if (num8 != 233 && num8 != 232 && num8 != 18 && numfc1 == 2  && humidity >= 5 && temperature >= 3 && temperature <= 10)
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

							
							int num12 = TerrainChunk.CalculateCellIndex(i, k + 1, j);//计算当前网格点上方单元格的索引。
							Block block1 = BlocksManager.Blocks[Terrain.ExtractContents(chunk.GetCellValueFast(num12))];
							if(isvillage_chunk == false&& tg_num == 0)//如果不是村庄区块则生成。
                            {
								if (Terrain.ExtractContents(chunk.GetCellValueFast(num12)) == 0||block1.IsTransparent==true)//如果当前单元格不为空方块。
								{

									int value1 = Terrain.ReplaceContents(0, num6);//将当前网格点的方块内容替换为新的方块内容。
									chunk.SetCellValueFast(num12 - 1, value1);


								}
							}
							
							//上面是生成地表群系泥土。

							
							if (num6 != 8)//如果生成了群系土，说明群系生成成功，接下来判断是否开始生成村庄。
                            {
								
								if( flag_v == true &&flag_v2 ==true)//如果当前区块生成村庄起点。高度差合适！
                                { 
									if(listRD.Count == 0)//如果当前的列表是空的。说明村庄还没有起点。则生成起点。
                                    {
										listRD.Add(new NewModLoaderShengcheng.RoadPoint
										{
											Position= (middle_x, middle_y, middle_z),
											BrushType = 0,
											TG = false,//代表该路径点是否已经生成
											TFirst = true,//判断该路径点是否为村庄起点。
											Tturn =false,//判断该路径点是否为转折点。
											is_Vice = false,
											chunkCoords = (chunk.Coords.X,chunk.Coords.Y),

										});
										Point3 point_start = (middle_x, middle_y ,middle_z);
										Village_start.Add(point_start);//记录村庄起点。
										
										
										listRD.Add(new NewModLoaderShengcheng.RoadPoint
										{
											Position = (middle_x, middle_y, middle_z + 16),
											BrushType = 0,
											TG = false,//代表该路径点是否已经生成
											TFirst = true,//判断该路径点是否为村庄起点。
											Tturn = false,//判断该路径点是否为转折点。
											is_Vice = true,
											chunkCoords = (chunk.Coords.X, chunk.Coords.Y+1),

										});
										listBD.Add(new NewModLoaderShengcheng.BuildPoint//同时生成建筑点
										{
											Position = (middle_x, middle_y, middle_z +16),
											BrushType = 0,
											TG = false,//代表该路径点是否已经生成
											

										});

										Log.Information(string.Format("村庄起点的坐标是：{0}, {1}, {2}", middle_x, middle_y, middle_z));
										
										for (int a = 1; a < 5; a++)
										{
											if(a == 2)//如果生成到第三个路径点，则第三个为转折点
                                            {
												listRD.Add(new NewModLoaderShengcheng.RoadPoint
												{
													Position = (listRD[a].Position.X + 16, middle_y, listRD[ a ].Position.Z),
													BrushType = 2,//拐弯
													TG = false,//代表该路径点是否已经生成
													TFirst = false,//判断该路径点是否为村庄起点。
													Tturn = true,//判断该路径点是否为转折点。
													chunkCoords = (chunk.Coords.X+2, chunk.Coords.Y),

												});
												Log.Information(string.Format("村庄转折点的坐标是：{0}, {1}, {2}", listRD[a ].Position.X + 16, middle_y, listRD[a ].Position.Z));
												
											}
                                            else
                                            {
												if(a >2)//拐弯后
                                                {
													if(a == 3)
                                                    {
														listRD.Add(new NewModLoaderShengcheng.RoadPoint
														{
															Position = (listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16),
															BrushType = 1,//z轴路径
															TG = false,//代表该路径点是否已经生成
															TFirst = false,//判断该路径点是否为村庄起点。
															Tturn = false,//判断该路径点是否为转折点。
															is_Vice = false,
															chunkCoords = (chunk.Coords.X+2, chunk.Coords.Y+1),

														});
														Log.Information(string.Format("村庄z轴路径的坐标是：{0}, {1}, {2}", listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16));
													}
													
													if(a == 4)
                                                    {
														listRD.Add(new NewModLoaderShengcheng.RoadPoint
														{
															Position = (listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16),
															BrushType = 1,//z轴路径
															TG = false,//代表该路径点是否已经生成
															TFirst = false,//判断该路径点是否为村庄起点。
															Tturn = false,//判断该路径点是否为转折点。
															is_Vice = false,
															chunkCoords = (chunk.Coords.X+2, chunk.Coords.Y+2),

														});
														Log.Information(string.Format("村庄z轴路径的坐标是：{0}, {1}, {2}", listRD[a + 1].Position.X, middle_y, listRD[a + 1].Position.Z + 16));
														listRD.Add(new NewModLoaderShengcheng.RoadPoint
														{
															Position = (listRD[6].Position.X - 16, middle_y, listRD[6].Position.Z ),
															BrushType = 1,//z轴路径
															TG = false,//代表该路径点是否已经生成
															TFirst = false,//判断该路径点是否为村庄起点。
															Tturn = false,//判断该路径点是否为转折点。
															is_Vice = true,
															chunkCoords = (chunk.Coords.X+1, chunk.Coords.Y+2),

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
														is_Vice =false,
														chunkCoords = (chunk.Coords.X+1, chunk.Coords.Y),
													});
													Log.Information(string.Format("村庄主路的坐标是：{0}, {1}, {2}", listRD[0].Position.X + 16, middle_y, listRD[0].Position.Z));
													listRD.Add(new NewModLoaderShengcheng.RoadPoint//主路，x轴方向
													{
														Position = (listRD[2].Position.X , middle_y, listRD[2].Position.Z + 16),
														BrushType = 0,//x轴路径
														TG = false,//代表该路径点是否已经生成
														TFirst = false,//判断该路径点是否为村庄起点。
														Tturn = false,//判断该路径点是否为转折点。
														is_Vice = true,
														chunkCoords = (chunk.Coords.X+1, chunk.Coords.Y+1),
													});
													listBD.Add(new NewModLoaderShengcheng.BuildPoint//同时生成建筑点右侧
													{
														Position = (listRD[a - 1].Position.X + 16, middle_y, listRD[a - 1].Position.Z+16),
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
								
								if(listRD.Count != 0)
                                {
									if (isvillage_chunk == true) ;
									{
										flag_tree = false;

									}
								}

								
									
								
								if(flag_tree == true)
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
		#region 注释掉的代码，请忽略
		//树生成,生成群系土之后执行。


		/*int lx1 = num; // x原始坐标
			int ly1 = k - 1; // y原始坐标
			int lz1 = num2; // z原始坐标

			int lx = num + random.Int(2, 6); // x坐标 待生成
			int ly = k - 1; // y坐标
			int lz = num2 + random.Int(2, 6); // z坐标

			int cx = num + random.Int(2, 6); // x坐标 待生成
			int cy = k - 1; // y坐标
			int cz = num2 + random.Int(2, 6); // z坐标


			if (terrain.GetCellContentsFast(lx, ly, lz) != 0 && terrain.GetCellContentsFast(lx, ly, lz) == 8 && terrain.GetCellContentsFast(lx, ly + 1, lz) != 18 && terrain.GetCellContentsFast(lx, ly + 1, lz) == 0)
			{



				// 生成樱花树
				// 获取樱花树的地形刷子
				ReadOnlyList<TerrainBrush> cherryBlossomBrushes = FCPlantManager.GetTreeBrushes(FCTreeType.Yinghua);
				TerrainBrush terrainBrush = cherryBlossomBrushes[random.Int(cherryBlossomBrushes.Count)];
				// 应用樱花树的地形刷子到地形上

				terrainBrush.PaintFast(chunk, cx, cy, cz);
				chunk.AddBrushPaint(cx, cy, cz, terrainBrush);



			}
			/*else if (!BlocksManager.Blocks[terrain.GetCellContentsFast(cx + 1, cy, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx - 1, cy, cz)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, cy, cz + 1)].IsCollidable && !BlocksManager.Blocks[terrain.GetCellContentsFast(cx, cy, cz - 1)].IsCollidable)
			{
				cx = lx1;
				cz = lz1;
			}*/

		/*if (Yh_Tree == true) //生成樱花树。
		{

		}

		int lx1 = num; // x原始坐标
			int ly1 = k - 1; // y原始坐标
			int lz1 = num2; // z原始坐标

			int lx = num + random.Int(1, 3); // x坐标 待生成
			int ly = k - 1; // y坐标
			int lz = num2 + random.Int(1, 3); // z坐标

			int cx = num + random.Int(1, 3); // x坐标 待生成
			int cy = k - 1; // y坐标
			int cz = num2 + random.Int(1, 3); // z坐标
			FCTreeType[] treeTypes = new FCTreeType[] { FCTreeType.Apple, FCTreeType.Orange, FCTreeType.Coco };
			FCTreeType selectedTreeType = treeTypes[random.Int(treeTypes.Length)];



			if (terrain.GetCellContentsFast(lx, ly, lz) != 0 && terrain.GetCellContentsFast(lx, ly, lz) == 8 && terrain.GetCellContentsFast(lx, ly + 1, lz) != 18 && terrain.GetCellContentsFast(lx, ly + 1, lz) == 0)
			{
				if (random.Bool(0.1f))
				{
					selectedTreeType = FCTreeType.Lorejun;
				}
				ReadOnlyList<TerrainBrush> treeBrushes = FCPlantManager.GetTreeBrushes(selectedTreeType);
				TerrainBrush terrainBrush = treeBrushes[random.Int(treeBrushes.Count)];
				terrainBrush.PaintFast(chunk, cx, cy, cz);
				chunk.AddBrushPaint(cz, cy, cz, terrainBrush);

			}
		 */
		#endregion
		#endregion

		#region 村庄生成子方法

		
		public bool ischunkloding2 = false;//用来保证村庄生成的全过程独立性
		public int Bg_num = 0;//计算副建筑区块的生成个数，到3清零。//建筑列表暂时未使用过。
		public int tg_num = 0;//用来计算村庄的生成个数，到5清零。
		/*public void FCGenerateVillageBuild1(TerrainChunk chunk, SubsystemTerrain m_systemTerrain) //未使用。
        {
			int origin = chunk.Origin.X;
			int origin2 = chunk.Origin.Y;
			int x1 = chunk.Coords.X;//获取区块的绝对坐标。
			int y1 = chunk.Coords.Y;
			int middle_x = origin + 8;
			int middle_z = origin2 + 8;
			Terrain terrain = m_systemTerrain.Terrain;

			int middle_y = terrain.CalculateTopmostCellHeight(middle_x, middle_z);//获取当前区块中心点的高度
			Random random = new Random(this.m_seed + chunk.Coords.X + 555 * chunk.Coords.Y);
			if (listBD.Count != 0)//如果有路径点。
			{
				for (int i = 0; i < 3; i++)
				{
					TerrainChunk chunk_v = terrain.GetChunkAtCell(listBD[i].Position.X, listBD[i].Position.Z);
					if (chunk_v == null || listBD[i].TG == true)//如果区块为空，则跳过该路径点的生成。
					{
						continue;
					}
					else if (chunk == chunk_v) //如果不为空，则准备生成路径
					{
						if (listBD[i].TG == false && listBD[i].BrushType == 0)
						{
							List<int> height_T = new List<int>(); // 创建一个空的 List<int>,储存区块高度
							int x_T = chunk_v.Origin.X;//获取该路径点区块的原始坐标。
							int z_T = chunk_v.Origin.Y;
							int x_T_coords = chunk_v.Coords.X;
							int z_T_coords = chunk_v.Coords.Y;
							Point2 point_t1 = (x_T_coords, z_T_coords);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

							bool ischunkload = Dic_Chunk_Village.ContainsKey(point_t1);
							if (ischunkload == false)
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
								if (i == 0)//如果是副区块第一个，则生成养殖间。
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
											int y1t = y + max1 - 4;
											int z1t = z_T + z;
											terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
										}
									}
								}
								else
								{
									if (random.Bool(0.1f))
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
												int y1t = y + max1 - 4;
												int z1t = z_T + z;
												terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
											}
										}
									}
									else
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
												int y1t = y + max1 - 4;
												int z1t = z_T + z;
												terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
											}
										}
									}


								}
								Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
								Dic_Chunk_Village.Add(pointRD1, chunk_v);
								listBD[i].TG = true;
								Bg_num++;



								m_systemTerrain.ChangeCell(x_T, 3, z_T, 0);
							}

						}//x轴主路
						if (listBD[i].TG == false && listBD[i].BrushType == 1)
						{
							List<int> height_T = new List<int>(); // 创建一个空的 List<int>,储存区块高度
							int x_T = chunk_v.Origin.X;//获取该路径点区块的原始坐标。
							int z_T = chunk_v.Origin.Y;
							int x_T_coords = chunk_v.Coords.X;
							int z_T_coords = chunk_v.Coords.Y;
							Point2 point_t1 = (x_T_coords, z_T_coords);//如果可以生成，则先获取绝对坐标，比较区块字典，如果发现是已生成区块，则不生成。

							bool ischunkload = Dic_Chunk_Village.ContainsKey(point_t1);
							if (ischunkload == false)
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
										int x1t = x + x_T;
										int y1t = y + max1 - 4;
										int z1t = z_T + z;
										terrain.SetCellValueFast(x1t, y1t, z1t, block_house);
									}
								}




								Point2 pointRD1 = (chunk_v.Coords.X, chunk_v.Coords.Y);
								Dic_Chunk_Village.Add(pointRD1, chunk_v);
								listBD[i].TG = true;
								Bg_num++;



								m_systemTerrain.ChangeCell(x_T, 3, z_T, 0);
							}
						}
					}
				}

			}
		}*/
		public async Task FCGenerateVillage(TerrainChunk chunk ,SubsystemTerrain m_systemTerrain)
        {
			ischunkloding2 = true;
			SubsystemBlockEntities entityScan = new SubsystemBlockEntities();//实体检测
			SubsystemCreatureSpawn spawn1 = new SubsystemCreatureSpawn();
			FCSubsystemTownChunk m_subsystemtownchunk= new FCSubsystemTownChunk();
			int origin = chunk.Origin.X;
			int origin2 = chunk.Origin.Y;
			int x1 = chunk.Coords.X;//获取区块的绝对坐标。
			int y1 = chunk.Coords.Y;
			int middle_x = origin + 8;
			int middle_z = origin2 + 8;
			Terrain terrain = m_systemTerrain.Terrain;
			
			int middle_y = terrain.CalculateTopmostCellHeight(middle_x, middle_z);//获取当前区块中心点的高度
			Random random = new Random(this.m_seed + chunk.Coords.X + 555 * chunk.Coords.Y);
			
			
			
			if (listRD.Count != 0)//如果路径点等于5，说明已经路径点已经完毕，执行生成。
			{
				await Task.Delay(100);
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

						if (x1 == chunkX1&& y1 == chunkY1)
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
										if (Dic_Chunk_Village3.ContainsKey(pointRD1)==false)
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
																ComponentBlockEntity result= entityScan.GetBlockEntity(x1t, y1t, z1t);
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
																}
																
															}
															if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
															{
																ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
																if (result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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
										else if (i==0)//如果不是主路第二条
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
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
																}

																
															}
															if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
															{
																ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);

															    if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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
														m_systemTerrain.ChangeCell(x1t, y1t, z1t, block_house);
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
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
																}
																
															}
															if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
															{
																ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
																}
																
															}
															if (Terrain.ExtractContents(block_house) == 64)//如果是熔炉
															{
																ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Furnace", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
																}
																
															}
															if (Terrain.ExtractContents(block_house) == 45)//如果等于箱子，补充实体
															{
																ComponentBlockEntity result = entityScan.GetBlockEntity(x1t, y1t, z1t);
																if(result == null)
                                                                {
																	DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
																	ValuesDictionary valuesDictionary = new ValuesDictionary();
																	valuesDictionary.PopulateFromDatabaseObject(databaseObject);
																	valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
																	Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
																	m_systemTerrain.Project.AddEntity(entity);
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





									m_systemTerrain.ChangeCell(x_T, 3, z_T, 0);
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
													if(result == null)
                                                    {
														DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
														ValuesDictionary valuesDictionary = new ValuesDictionary();
														valuesDictionary.PopulateFromDatabaseObject(databaseObject);
														valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
														Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
														m_systemTerrain.Project.AddEntity(entity);
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
									m_systemTerrain.ChangeCell(x_T, 3, z_T, 0);
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
														if(result == null)
                                                        {
															DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
															ValuesDictionary valuesDictionary = new ValuesDictionary();
															valuesDictionary.PopulateFromDatabaseObject(databaseObject);
															valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
															Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
															m_systemTerrain.Project.AddEntity(entity);
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
														if(result == null)
                                                        {
															DatabaseObject databaseObject = m_systemTerrain.Project.GameDatabase.Database.FindDatabaseObject("Chest", m_systemTerrain.Project.GameDatabase.EntityTemplateType, true);
															ValuesDictionary valuesDictionary = new ValuesDictionary();
															valuesDictionary.PopulateFromDatabaseObject(databaseObject);
															valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue<Point3>("Coordinates", new Point3(x1t, y1t, z1t));
															Entity entity = m_systemTerrain.Project.CreateEntity(valuesDictionary);
															m_systemTerrain.Project.AddEntity(entity);
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

									m_systemTerrain.ChangeCell(x_T, 3, z_T, 0);


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
								Entity entity1 = DatabaseManager.CreateEntity(m_systemTerrain.Project, "FCVillager", true);
								entity1.FindComponent<ComponentBody>(true).Position = (chunk_v.Origin.X, true_height, chunk_v.Origin.Y);
								entity1.FindComponent<ComponentBody>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, random.Float(0f, 6.2831855f));
								entity1.FindComponent<ComponentCreature>(true).ConstantSpawn = false;
								
								m_systemTerrain.Project.AddEntity(entity1);
							}
							catch
                            {
								Log.Error("Generation villager failed.");
								
							}
							
						}

						
					}

					
				}
				
				
				Log.Information(string.Format("生成完毕的区块总数：{0}", tg_num));



				if(tg_num==8)
                {
					
					
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
		private async Task GenerateVillageAsync(TerrainChunk chunk,Terrain terrain)
		{
			await Task.Run(() =>
			{
				


			});
		}
			#endregion
		}



    #endregion

    #region 区块（植物）
    #region 草皮方块子系统
    public class FCSubsystemGrassBlockBehavior : SubsystemPollableBlockBehavior, IUpdateable
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
	}
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
					19,
					20,
					24,
					25,
					28,
					99,
					131,
					244,
					132,
					174,
					204,
					
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

						if (num2 != 8 && num2 != 2 && num2 != 7 && num2 != 168 && num2 != 984 && num2 != 985 && num2 != 986)//草地，泥土，沙子，土壤
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
			if (num2 != 8 && num2 != 2 && num2 != 168 && num2 != 984 && num2 != 985 && num2 != 986)
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

    #region 子系统天空
	public class FCSubsystemSky:SubsystemSky, IDrawable, IUpdateable
	{

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
                }
                if (this.ViewUnderWaterDepth > 0f)
                {
                    int seasonalHumidity = this.m_subsystemTerrain.Terrain.GetSeasonalHumidity(x, z);
                    int temperature = this.m_subsystemTerrain.Terrain.GetSeasonalTemperature(x, z) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
                    Color c = BlockColorsMap.WaterColorsMap.Lookup(temperature, seasonalHumidity);
                    float num = MathUtils.Lerp(1f, 0.5f, (float)seasonalHumidity / 15f);
                    float num2 = MathUtils.Lerp(1f, 0.2f, MathUtils.Saturate(0.075f * (this.ViewUnderWaterDepth - 2f)));
                    float num3 = MathUtils.Lerp(0.33f, 1f, this.SkyLightIntensity);
                    this.m_viewFogRange.X = 0f;
                    this.m_viewFogRange.Y = MathUtils.Lerp(4f, 10f, num * num2 * num3);
                    this.m_viewFogColor = Color.MultiplyColorOnly(c, 0.66f * num2 * num3);
                    this.VisibilityRangeYMultiplier = 1f;
                    this.m_viewIsSkyVisible = false;
                }
                else if (this.ViewUnderMagmaDepth > 0f)
                {
                    this.m_viewFogRange.X = 0f;
                    this.m_viewFogRange.Y = 0.1f;
                    this.m_viewFogColor = new Color(255, 80, 0);
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
                if (!this.DrawSkyEnabled || !this.m_viewIsSkyVisible || SettingsManager.SkyRenderingMode == SkyRenderingMode.Disabled)
                {
                    FlatBatch2D flatBatch2D = this.m_primitivesRenderer2d.FlatBatch(-1, DepthStencilState.None, RasterizerState.CullNoneScissor, BlendState.Opaque);
                    int count = flatBatch2D.TriangleVertices.Count;
                    ModsManager.HookAction("ViewFogColor", delegate (ModLoader modLoader)
                    {
                        modLoader.ViewFogColor(this.ViewUnderWaterDepth, this.ViewUnderMagmaDepth, ref this.m_viewFogColor);
                        return false;
                    });
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
                        this.DrawStars(camera);
                        this.DrawSunAndMoon(camera);
                        this.DrawClouds(camera);
                        this.DrawEarth(camera);

                    }
                    
                    ModsManager.HookAction("SkyDrawExtra", delegate (ModLoader loader)
                    {
                        loader.SkyDrawExtra(this, camera);
                        return false;
                    });
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
        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            this.m_EarthTexture = ContentManager.Get<Texture2D>("Textures/Earth");
        }
        public Texture2D m_EarthTexture;
        public new void DrawSunAndMoon(Camera camera)
        {
            float f = (camera.ViewPosition.Y > 1000f) ? 0f : this.m_subsystemWeather.GlobalPrecipitationIntensity;
            float timeOfDay = this.m_subsystemTimeOfDay.TimeOfDay;
            float f2 = MathUtils.Max(SubsystemSky.CalculateDawnGlowIntensity(timeOfDay), SubsystemSky.CalculateDuskGlowIntensity(timeOfDay));
            float num = 2f * timeOfDay * 3.1415927f;
            float angle = num + 3.1415927f;
            float num2 = MathUtils.Lerp(90f, 160f, f2);
            float num3 = MathUtils.Lerp(60f, 80f, f2);
            Color color = Color.Lerp(new Color(255, 255, 255), new Color(255, 255, 160), f2);
            Color color2 = Color.White;
            color2 *= 1f - base.SkyLightIntensity;
            color *= MathUtils.Lerp(1f, 0f, f);
            color2 *= MathUtils.Lerp(1f, 0f, f);
            Color color3 = color * 0.6f * MathUtils.Lerp(1f, 0f, f);
            Color color4 = color * 0.2f * MathUtils.Lerp(1f, 0f, f);
            TexturedBatch3D batch = this.m_primitivesRenderer3d.TexturedBatch(this.m_glowTexture, false, 0, DepthStencilState.DepthRead, null, BlendState.Additive, null);
            TexturedBatch3D batch2 = this.m_primitivesRenderer3d.TexturedBatch(this.m_sunTexture, false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
            TexturedBatch3D batch3 = this.m_primitivesRenderer3d.TexturedBatch(this.m_moonTextures[base.MoonPhase], false, 1, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, null);
            base.QueueCelestialBody(batch, camera.ViewPosition, color3, 900f, 3.5f * num2, num);
            base.QueueCelestialBody(batch, camera.ViewPosition, color4, 900f, 3.5f * num3, angle);
            base.QueueCelestialBody(batch2, camera.ViewPosition, color, 900f, num2, num);
            base.QueueCelestialBody(batch3, camera.ViewPosition, color2, 900f, num3, angle);
        }
        public new void DrawClouds(Camera camera)
        {
            if (SettingsManager.SkyRenderingMode == SkyRenderingMode.NoClouds || camera.ViewPosition.Y > 1000f)
            {
                return;
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
            TexturedBatch3D texturedBatch3D = this.m_primitivesRenderer3d.TexturedBatch(this.m_cloudsTexture, false, 2, DepthStencilState.DepthRead, null, BlendState.AlphaBlend, SamplerState.LinearWrap);
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
    #endregion


    #region 樱花粒子效果
    public class BlossomParticleSystem : ParticleSystem<BlossomParticleSystem.Particle>
	{
		
		public class Particle : Game.Particle
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
				Particle obj = Particles[i];
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
				Particle particle = Particles[i];
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
	public class FCSubsystemPlantataBlockBehavior : SubsystemBlockBehavior, IUpdateable
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
			AddPlantData(value, x, y, z);
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
			AddPlantData(value, x, y, z);

			

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
	}
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
				this.GrowYHCarpet(value, x, y, z, pollPass);
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

			else if ((Terrain.ExtractContents(cellValue1) != 0)&& (Terrain.ExtractContents(cellValue2) == 0)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
				base.SubsystemTerrain.ChangeCell(x, y - 13, z, carpetValue, true);
			}
			else if ((Terrain.ExtractContents(cellValue3) != 0) && (Terrain.ExtractContents(cellValue1) == 0)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
				base.SubsystemTerrain.ChangeCell(x, y - 14, z, carpetValue, true);
			}
			else if ((Terrain.ExtractContents(cellValue2) != 0) && (Terrain.ExtractContents(cellValue4) == 0)) // 检查樱花树叶下方块是否为空
			{
				// 生成樱花地毯
				int carpetValue = Terrain.MakeBlockValue(963); // 方块ID为963
				base.SubsystemTerrain.ChangeCell(x, y - 12, z, carpetValue, true);
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
				foreach (KeyValuePair<Point3, SubsystemPlantBlockBehavior.Replacement> keyValuePair in this.m_toReplace)
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
			"铜板Copper plate",
			"铁板iron plate",
			"钢板steel plate",
			"铜线圈copper coil",
			"硅silicon",
			"线路板circuit board",
			"集成芯片integrated chip",
			"酵素粉enzyme powder",
			"硅板silicon plate",
			"精制淀粉refined starch",
			"偏导芯片bias chip",
			"齿轮gear",
			"钢棒steel rod",
			"酵母菌yeast",
			"活塞universal piston",
			"钢锭steelIngot",
			"碳粉",
			"铁粉",
			"磁化铁棒",
			"基础马达",


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
				this.m_smeltingProgress = MathUtils.Min(this.m_smeltingProgress + 0.01f * dt, 1f);
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
				CraftingRecipe craftingRecipe = CraftingRecipesManager.FindMatchingRecipe(this.m_subsystemTerrain, this.m_matchedIngredients, heatLevel, playerLevel);
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

			if (this.m_heatLevel == 900f)//如果熔炉的热量级别=1000，开始烧

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
				CraftingRecipe craftingRecipe = this.FindFCGLSmeltingRecipe(heatLevel);    //查找匹配heatLevel的熔炼配方（FindSmeltingRecipe方法），并将其设置为当前的熔炼配方（m_smeltingRecipe）。
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
				this.m_smeltingProgress = MathUtils.Min(this.m_smeltingProgress + 0.2f * dt, 1f);
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
				this.m_subsystemTerrain.ChangeCell(coordinates.X, coordinates.Y, coordinates.Z, Terrain.ReplaceContents(cellValue, (this.m_heatLevel > 0f) ? 976 : 975), true);
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

		public virtual CraftingRecipe FindFCGLSmeltingRecipe(float heatLevel)
		{
			if (heatLevel == 900f)  //首先，检查热量级别是否大于0。如果不大于0，则直接返回null，表示没有匹配的熔炼配方。
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
				CraftingRecipe craftingRecipe = CraftingRecipesManager.FindMatchingRecipe(this.m_subsystemTerrain, this.m_matchedIngredients, heatLevel, playerLevel);
				if (craftingRecipe != null && craftingRecipe.ResultValue != 0) //如果找到了匹配的熔炼配方，并且配方的结果物品不为0，继续执行以下判断：
				{
					if (craftingRecipe.RequiredHeatLevel != 900f) //1.如果配方要求的热量级别小于等于0，则将熔炼配方设置为null，表示没有匹配的熔炼配方。
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
					InventorySlotWidget inventorySlotWidget2 = new InventorySlotWidget();
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
			CraftingRecipe craftingRecipe = FCCraftingRecipesManager.FindMatchingRecipe(base.Project.FindSubsystem<SubsystemTerrain>(true), this.m_matchedIngredients, 10f, playerLevel);
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
	#region 樱花酒液体测试
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
			if (base.SubsystemTime.PeriodicGameTimeEvent(0.25, 0.0))
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
					91,
					93,
					110,
					245,
					251,
					252,
					129,
					128,
					966//樱花酒桶 979液体
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

					if (block is WaterBlock && FluidBlock.GetLevel(data) == 0) //如果方块是水方块且水的高度为0，则执行以下逻辑。
					{
						int value = Terrain.ReplaceContents(activeBlockValue, 91);  //将当前活动的方块值替换为水桶（方块索引为91），存储在value变量中。
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, inventory.GetSlotCount(inventory.ActiveSlotIndex)); //移除玩家物品栏中当前激活槽位的物品，数量为槽位中的物品数量。
						
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)//如果物品栏中当前激活槽位的物品数量为0，则执行以下逻辑。
						{
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value, 1);//向物品栏的当前激活槽位添加水桶物品，数量为1。
						}
						base.SubsystemTerrain.DestroyCell(0, cellFace.X, cellFace.Y, cellFace.Z, 0, false, false); //销毁交互的水方块。
						return true;
					}
					if (block is MagmaBlock && FluidBlock.GetLevel(data) == 0)   //如果是岩浆
					{
						int value2 = Terrain.ReplaceContents(activeBlockValue, 93); 
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, inventory.GetSlotCount(inventory.ActiveSlotIndex));
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
						{
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value2, 1);
						}
						base.SubsystemTerrain.DestroyCell(0, cellFace.X, cellFace.Y, cellFace.Z, 0, false, false);
						return true;
					}

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
				}
				else if (obj is BodyRaycastResult) ////如果射线检测结果是与实体交互（BodyRaycastResult类型）。 这里是指挤牛奶的行为
				{
					////获取射线检测结果的物体实体组件ComponentUdder，存储在componentUdder变量中。
					ComponentUdder componentUdder = ((BodyRaycastResult)obj).ComponentBody.Entity.FindComponent<ComponentUdder>(); 
					if (componentUdder != null && componentUdder.Milk(componentMiner)) //如果componentUdder不为null且调用Milk方法成功挤奶。
					{
						int value3 = Terrain.ReplaceContents(activeBlockValue, 110);
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, inventory.GetSlotCount(inventory.ActiveSlotIndex));
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
						{
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value3, 1);
						}
						this.m_subsystemAudio.PlaySound("Audio/Milked", 1f, 0f, ray.Position, 2f, true);
					}
					return true;
				}
			}
			if (num == 91) //如果是水桶
			{
				TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Interaction, true, true, true);
				if (terrainRaycastResult != null && componentMiner.Place(terrainRaycastResult.Value, Terrain.MakeBlockValue(18)))
				{
					inventory.RemoveSlotItems(inventory.ActiveSlotIndex, 1);
					if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
					{
						int value4 = Terrain.ReplaceContents(activeBlockValue, 90);
						inventory.AddSlotItems(inventory.ActiveSlotIndex, value4, 1);
					}
					return true;
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
			if (num == 93)
			{
				TerrainRaycastResult? terrainRaycastResult2 = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Interaction, true, true, true);
				if (terrainRaycastResult2 != null)
				{
					if (componentMiner.Place(terrainRaycastResult2.Value, Terrain.MakeBlockValue(92)))
					{
						inventory.RemoveSlotItems(inventory.ActiveSlotIndex, 1);
						if (inventory.GetSlotCount(inventory.ActiveSlotIndex) == 0)
						{
							int value5 = Terrain.ReplaceContents(activeBlockValue, 90);
							inventory.AddSlotItems(inventory.ActiveSlotIndex, value5, 1);
						}
					}
					return true;
				}
			}
			if (num <= 129)
			{
				if (num != 110)
				{
					if (num - 128 > 1)
					{
						return false;
					}
					TerrainRaycastResult? terrainRaycastResult3 = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging, true, true, true);
					if (terrainRaycastResult3 != null)
					{
						CellFace cellFace2 = terrainRaycastResult3.Value.CellFace;
						int cellValue2 = base.SubsystemTerrain.Terrain.GetCellValue(cellFace2.X, cellFace2.Y, cellFace2.Z);
						int num3 = Terrain.ExtractContents(cellValue2);
						Block block2 = BlocksManager.Blocks[num3];
						if (block2 is IPaintableBlock)
						{
							Vector3 normal = CellFace.FaceToVector3(terrainRaycastResult3.Value.CellFace.Face);
							Vector3 position = terrainRaycastResult3.Value.HitPoint(0f);
							int? num4 = (num == 128) ? null : new int?(PaintBucketBlock.GetColor(Terrain.ExtractData(activeBlockValue)));
							Color color = (num4 != null) ? SubsystemPalette.GetColor(base.SubsystemTerrain, num4) : new Color(128, 128, 128, 128);
							int value6 = ((IPaintableBlock)block2).Paint(base.SubsystemTerrain, cellValue2, num4);
							base.SubsystemTerrain.ChangeCell(cellFace2.X, cellFace2.Y, cellFace2.Z, value6, true);
							componentMiner.DamageActiveTool(1);
							this.m_subsystemAudio.PlayRandomSound("Audio/Paint", 0.4f, this.m_random.Float(-0.1f, 0.1f), componentMiner.ComponentCreature.ComponentBody.Position, 2f, true);
							this.m_subsystemParticles.AddParticleSystem(new PaintParticleSystem(base.SubsystemTerrain, position, normal, color));
						}
						return true;
					}
					return false;
				}
			}
			else if (num != 245)
			{
				if (num - 251 > 1)
				{
					return false;
				}
				return true;
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

	#region 合成表
	public class ComponentGameSystem1 : ComponentInventoryBase, IUpdateable
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
	}


	#endregion
	#region 合成表
	public class FCRecipeWidget : CanvasWidget
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
    #endregion
}










namespace Game
{
	#region 附魔台炼药锅
	#region ModLoader
	public class ModModLoader : ModLoader
	{
		public override void OnLoadingFinished(List<System.Action> actions)
		{
			actions.Add(delegate {
				XCraftingRecipesManager.Initialize();
			});
		}
	}
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
	public class XSmeltingRecipeWidget : CanvasWidget
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
					widget.Size = new Vector2(50f, 50f);
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

	public class FlameFurnaceWidget : CanvasWidget
	{
		public GridPanelWidget m_inventoryGrid;

		public GridPanelWidget m_flamefurnaceGrid;

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
					var inventorySlotWidget2 = new InventorySlotWidget();
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
			if (m_heatLevel == 11f)
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
				m_smeltingProgress = MathUtils.Min(m_smeltingProgress + 0.25f * dt, 1f);
				if (m_smeltingProgress >= 1f)
				{
					for (int i = 0; i < 8; i++)
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
			if (heatLevel == 11f)
			{
				for (int i = 0; i < 8; i++)
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
					if (craftingRecipe.RequiredHeatLevel != 11f)
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
			return null;
		}
	}

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
}
