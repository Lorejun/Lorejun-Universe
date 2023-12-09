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

using Engine.Audio;
using Test1;
using System.Text.RegularExpressions;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
using static Game.TerrainContentsGenerator21;

namespace Game
{
    #region 老任务系统
    public class Task1//成就类型
    {
        public string Name;//名称

        public Action<object[]> Task1Action;//当完成时发生的事件

        public bool CompletionStatus;//是否完成

        public string TextureName;//展示完成成就时所显示的图片

        public string Text;//查看时的内容

        public string Text2;//点开成就内的描述
    }

    public class ComponentTask1 : Component, IUpdateable//注册新的成就组件
    {
        public UpdateOrder UpdateOrder => UpdateOrder.Default;//获取当前更新更新事件频率

        public bool flag2;//声明一个bool类型的值用于辅作作用

        public double lasttime;//偏移量转至最左端所需要持续的时间

        public CanvasWidget Task1Widget;

        public SubsystemTime subsystemTime;

        public SubsystemTask1 subsystemTask1;
        public SubsystemAudio m_subsystemAudio;

        public void DisplayTask1Message(string name, object[] items)//如果成就被完成，调用此方法来通知，items为基于奖励事件所必要要用到的变量
        {
            if (subsystemTask1.UseTask1(name, items, out string name2))//应用当前的成就，这是一个二级调用。首先先用use来看看成就是否被完成，而这个use本质上返回的是成就类里的完成bool值。
            {
                //如果成就还没有被完成，则执行以下代码，完成成就。完成成就的bool值变更在use方法内。
                Task1Widget.IsVisible = true;//展示成就显示界面
                Task1Widget.Margin = new Vector2(-Task1Widget.Size.X, 0);//将偏移量转至最右转
                flag2 = true;
                Task1Widget.Children.Find<RectangleWidget>("Task1.Icon").Subtexture = new Subtexture(ContentManager.Get<Texture2D>(name2), Vector2.Zero, Vector2.One);//给当前显示成就界面，赋予所需要显示的图片
                Task1Widget.Children.Find<LabelWidget>("Task1.Label").Text = "恭喜你完成了成就-" + name;//给予当前显示成就界面所有的文字描述
                m_subsystemAudio.PlaySound("Audio/achieve", 1f, 0f, 0f, 0.1f);

            }
        }

        public void Update(float dt)//当执行更新事件时
        {
            if (Task1Widget.IsVisible)//当提示成就界面显示时
            {
                if (Task1Widget.Margin.X != 0 && flag2)//如果偏移量不为0，
                {
                    Task1Widget.Margin += new Vector2(10f, 0);//持续将该界面的偏移量往界面左边驶去
                }
                else if (subsystemTime.GameTime - lasttime > 4 && !flag2 && Task1Widget.Margin.X != -Task1Widget.Size.X)//如果当前已达到至最左端时，且该状态已经持续了4秒时
                {
                    Task1Widget.Margin -= new Vector2(10f, 0);//持续将该界面往右边驱使
                }
                if (Task1Widget.Margin.X == 0 && flag2)//当偏移量已达到至最左端时
                {
                    lasttime = subsystemTime.GameTime;//将该当前显示时间赋值为现在的游戏时间
                    flag2 = false;
                }
                else if (Task1Widget.Margin.X == -Task1Widget.Size.X)//如果往右偏移，偏移量已达到最大值时
                {
                    flag2 = true;
                    Task1Widget.IsVisible = false;//将当前界面隐藏
                }
            }
            else
            {
                Task1Widget.Margin = new Vector2(-Task1Widget.Size.X, 0);//使当前界面的偏移量为最右端
            }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>();
            subsystemTask1 = Project.FindSubsystem<SubsystemTask1>(throwOnError: true);
            subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            Task1Widget = Entity.FindComponent<ComponentPlayer>(throwOnError: true).GuiWidget.Children.Find<CanvasWidget>("Task1");//从GameWidget.Xml文件中获取提示成就界面
        }
    }
    

    

    public class Task1Widget : CanvasWidget
    {
        public SubsystemTask1 m_subsystemTask1;

        public ListPanelWidget m_itemsList;//成就列表

        public Task1Widget(SubsystemTask1 subsystemTask1)
        {
            m_subsystemTask1 = subsystemTask1;
            XElement node = ContentManager.Get<XElement>("Widgets/Task1/Task1Widget");
            LoadContents(this, node);
            m_itemsList = Children.Find<ListPanelWidget>("Task1List");
            m_itemsList.ItemWidgetFactory = delegate (object item)//当成就列表时，每一个成就所展示的像
            {
                var Task1 = (Task1)item;
                XElement node2 = ContentManager.Get<XElement>("Widgets/Task1/Task1Item");//获取展示单独一个成像时的Widget
                var obj = (ContainerWidget)LoadWidget(this, node2, null);//加载当前界面(获取xml中Widget至加载至该类中)
                string CompletionStatus = Task1.CompletionStatus ? "己完成" : "未完成";//提示判断
                obj.Children.Find<RectangleWidget>("Task1Item.Icon").Subtexture = new Subtexture(ContentManager.Get<Texture2D>(Task1.TextureName), Vector2.Zero, Vector2.One);//展示当前项的图片
                obj.Children.Find<LabelWidget>("Task1Item.Name").Text = Task1.Name;//展示该项的名称
                obj.Children.Find<LabelWidget>("Task1Item.Text").Text = " " + Task1.Text + " 完成情况：" + CompletionStatus;//展示该项的内容
                return obj;
            };
            m_itemsList.ItemClicked += delegate (object item)//当该项被选中时
            {
                var Task12 = (Task1)item;
            };
            foreach (Task1 Task13 in subsystemTask1.m_Task1)//遍历所有的成就
            {
                m_itemsList.AddItem(Task13);
            }
        }

        public override void Update()
        {
            if (Children.Find<ButtonWidget>("ClearButton").IsClicked)//当所有成就清除按钮按下时
            {
                foreach (Task1 Task14 in m_subsystemTask1.m_Task1)
                {
                    Task14.CompletionStatus = false;//将所有的成就清除
                }
            }
            Children.Find<ButtonWidget>("SeeButton").IsVisible = m_itemsList.SelectedItem != null;//判断是否。成就列表中有项被选择
            if (Children.Find<ButtonWidget>("SeeButton").IsClicked)//当查看被点击时
            {
                var Task14 = (Task1)m_itemsList.SelectedItem;//通过获得该被选择的项，来获得成就文件
                string CompletionStatus2 = Task14.CompletionStatus ? "己完成" : "未完成";//成就完成判断
                DialogsManager.ShowDialog(null, new MessageDialog(Task14.Name + "  完成情况：" + CompletionStatus2,  Task14.Text2, LanguageControl.Ok, null, new Vector2(620f, 420f), null));//展示内容
            }
        }
    }

    public class SubsystemTask1 : Subsystem//成就子系统
    {
        public List<Task1> m_Task1 = new List<Task1>();//声明所有成就类
        public ComponentTest1 m_componentTest1;
        public ComponentPlayer m_componentPlayer;
        public SubsystemAudio m_subsystemAudio;
        public SubsystemPlayers m_subsystemPlayer;
        public override void Load(ValuesDictionary valuesDictionary)//当加载世界时
        {

            InitializeTask1();//初始化所有成就
            foreach (Task1 item in m_Task1)//遍历所有的成就
            {
                item.CompletionStatus = valuesDictionary.GetValue(item.Name, false);//将存档xml中所有的存储的成就进度，赋值给当前所有的成就
            }
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemPlayer = Project.FindSubsystem<SubsystemPlayers>(true);
        }

        public override void Save(ValuesDictionary valuesDictionary)//当保存世界时，(退出世界加载/每120秒加载一次)
        {
            foreach (Task1 item in m_Task1)//遍历所有成就
            {
                valuesDictionary.SetValue(item.Name, item.CompletionStatus);//存储当前所有成就的进度
            }
        }

        public override void OnEntityAdded(Entity entity)
        {
            ComponentPlayer componentPlayer = entity.FindComponent<ComponentPlayer>();
            if (componentPlayer != null)
            {
                m_componentPlayer = componentPlayer;
                m_componentTest1 = m_componentPlayer.Entity.FindComponent<ComponentTest1>(true);
            }
        }

        public void InitializeTask1()
        {
            m_Task1.Add(new Task1()//添加一个成就
            {
                Name = "极寒中的冰块",//名称
                TextureName = "Textures/Task1/ice",//展示时所取的图片路径名称
                Text = "拾取一块冰",//成就内容
                Task1Action = delegate (object[] item)//当完成成就时所发生的事件
                {
                    var a = (ComponentGui)item[0];//通过object获取ComponentGui
                    a.DisplaySmallMessage("多么漂亮的一个冰块", Color.White, true, true);//给予提示
                    if (m_componentPlayer != null)
                    {
                        m_componentTest1.m_mplevel += 5;
                    }

                },
                Text2 = "冰块在现代都市似乎是唾手可得的东西，但是对于流落到这片陌生大陆艰难求生的你，这似乎是一种象征——你战胜了严寒的环境。 （完成奖励进度值加5）"
            });
            m_Task1.Add(new Task1()//上同
            {
                Name = "神灵的庇护",
                TextureName = "Textures/Task1/Cross",
                Text = "手持神圣十字架，触发重生",
                Task1Action = delegate (object[] item)
                {
                    var a = (ComponentGui)item[0];
                    a.DisplaySmallMessage("感佑神灵庇护", Color.White, true, true);
                },
                Text2 = ""
            });
        }

        public Task1 GetTask1(string name)//通过成就名称获取所有成就中的一个成就
        {
            foreach (var item in m_Task1)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            throw new InvalidOperationException("获取成就失败.");//若无法获取则报错
        }

        public bool UseTask1(string name, object[] items, out string name2)
        {
            Task1 item = GetTask1(name);//获取成就对象,并赋值给item，item传参到成就委托函数实现一些方法
            if (!item.CompletionStatus)//如果当前成就没有被完成
            {
                item.CompletionStatus = true;//使当前成就完成
                item.Task1Action(items);//执行当前成就要触发的奖励事件
                name2 = item.TextureName;//返回当前成就的图片获取路径名称
                return true;//返回使用成功
            }
            name2 = null;//因为已经被完成了，不返回当前成就图片的获取路径名称
            return false;//返回使用失败
        }
    }
    #endregion

    #region 任务系统
    public enum TaskType
    {
        Main, //主线
        Other//支线
    }

    public class Task
    {
        public string Name;  //任务名称

        public TaskType TaskType;  //任务类型

        public int Index;   //索引

        public string Text; //描述

        public string RewardsText; //奖励描述

        public Action<ComponentPlayer, SubsystemWorldSystem,SubsystemPickables> Rewards;//奖励事件

        public bool Conditions = false;//任务完成情况
    }

    public class TaskWidget : CanvasWidget
    {
        public ListPanelWidget m_kindsList;//定义了一个列表面板部件（ListPanelWidget）m_kindsList，用来显示任务类型。

        public ListPanelWidget m_itemsList;//定义了一个列表面板部件 m_itemsList，用来显示任务项。

        public ComponentTask m_componentTask;//定义了一个 ComponentTask 类型的变量 m_componentTask，它持有当前组件的任务逻辑。

        public BevelledRectangleWidget m_displaytaskWidget;//定义了一个斜面矩形部件（BevelledRectangleWidget）m_displaytaskWidget，用于显示任务详情。

        public BevelledRectangleWidget m_displayrewardstaskWidget;//定义了一个斜面矩形部件 m_displayrewardstaskWidget，用于显示任务奖励详情。

        public bool IsMove = false; //定义了一个布尔变量 IsMove，用来控制动画状态。

        public float m_displaytaskWidgetValue = 5f;//定义了一个浮点数 m_displaytaskWidgetValue，用来控制 m_displaytaskWidget 动画速度。

        public float m_displayrewardstaskWidgetValue = 10f;//定义了一个浮点数 m_displayrewardstaskWidgetValue，用来控制 m_displayrewardstaskWidget 动画速度。

        public TaskWidget(ComponentTask componentTask)
        {
            m_componentTask = componentTask;
            XElement node = ContentManager.Get<XElement>("Widgets/Task1/TaskWidget");
            LoadContents(this, node);
            m_kindsList = Children.Find<ListPanelWidget>("KindList");//在任务部件的子部件中找到并赋值 m_kindsList。，任务类型
            m_itemsList = Children.Find<ListPanelWidget>("TaskList");//在任务部件的子部件中找到并赋值 m_itemsList。任务选项
            m_displaytaskWidget = Children.Find<BevelledRectangleWidget>("DisplayTask.BevelledRectangleWidget");//找到显示任务详情的斜面矩形部件 m_displaytaskWidget。
            m_displayrewardstaskWidget = Children.Find<BevelledRectangleWidget>("GetRewards.BevelledRectangleWidget");//找到显示任务奖励的斜面矩形部件 m_displayrewardstaskWidget。
            m_kindsList.ItemWidgetFactory = delegate (object item)//设置任务类型列表的部件工厂，它为每个任务类型创建一个部件。
            {
                var canvasWidget = new CanvasWidget();//创建一个新的画布部件。
                var textLabel = new LabelWidget();//创建一个新的文本标签部件。
                var bevelledRectangleWidget = new BevelledRectangleWidget();//创建一个新的斜面矩形部件。
                bevelledRectangleWidget.BevelSize = -1;//设置斜面矩形部件的倒角尺寸。
                textLabel.Text = ModItemManger.GetName("TaskType." + ((TaskType)item).ToString());//设置文本标签的文本，通过 ModItemManger.GetName 翻译任务类型的名称。
                textLabel.HorizontalAlignment = WidgetAlignment.Center;
                textLabel.VerticalAlignment = WidgetAlignment.Center;
                canvasWidget.Children.Add(bevelledRectangleWidget);
                canvasWidget.Children.Add(textLabel);
                //将斜面矩形部件 (bevelledRectangleWidget) 和文本标签部件 (textLabel) 添加到 canvasWidget 的子部件列表中。
                //这两个部件是在 ItemWidgetFactory 委托中创建的，用于显示任务类型列表中的每个任务类型项。
                return canvasWidget;
            };
            m_kindsList.ItemClicked += delegate (object item)//为 m_kindsList（任务类型列表）添加一个匿名委托，当任务类型列表中的项被点击时触发该委托。//当
           //点击主线任务的时候，先清空，然后再添加，不然会导致一直点一直增加任务的情况
            {
                var typename = (TaskType)item;//将点击的任务类型项转换为 TaskType 枚举值。
                m_itemsList.ClearItems();//清空 m_itemsList（任务项列表），准备为新选中的任务类型添加任务项。
                if (typename == TaskType.Other)//如果被点击的任务类型是 Other。
                {
                    foreach (var task in componentTask.m_othertasks)//遍历 componentTask.m_othertasks 字典中的所有任务。
                    {
                        m_itemsList.AddItem(task.Value);//将遍历到的每个任务添加到 m_itemsList（任务项列表）中。
                    }
                }
                else if (typename == TaskType.Main)//如果被点击的任务类型是 Main。
                {
                    m_itemsList.AddItem(SubsystemTask.GetTask(componentTask.MainTaskIndex));//调用 SubsystemTask.GetTask 方法获取当前主线任务，并将其添加到 m_itemsList（任务项列表）中。

                    for (int i = SubsystemTask.m_alltasks.Count-1; i >= 0; i--)
                    {
                        Task task = SubsystemTask.m_alltasks[i];
                        if (task.TaskType == TaskType.Main&&task.Index< componentTask.MainTaskIndex)
                        {
                            task.Conditions=true;
                            m_itemsList.AddItem(SubsystemTask.GetTask(task.Index));//调用 SubsystemTask.GetTask 方法获取当前主线任务，并将其添加到 m_itemsList（任务项列表）中。

                        }

                    }

                }
            };
            m_itemsList.ItemWidgetFactory = delegate (object item)//为 m_itemsList（任务项列表）设置 ItemWidgetFactory 委托，该委托生成显示特定任务名称的用户界面部件。
            {
                var textLabel = new LabelWidget();
                textLabel.Text = ((Task)item).Name;
                return textLabel;
            };
            m_itemsList.ItemClicked += delegate (object item)//为 m_itemsList（任务项列表）添加一个匿名委托，当任务项被点击时触发该委托。
            {
                IsMove = true;
            };
            m_kindsList.AddItem(TaskType.Main);//向 m_kindsList（任务类型列表）添加一个表示主线任务的项。
            m_kindsList.AddItem(TaskType.Other);//向 m_kindsList（任务类型列表）添加一个表示其他任务的项。
            m_itemsList.SelectionColor = ModItemManger.MainUiColor;
        }

        public override void Update()
        {
            if (m_itemsList.SelectedItem != null)//这行代码检查 m_itemsList（任务项列表）中是否有被选中的项。如果有，则继续执行代码块内的逻辑。
            {
                var task = (Task)m_itemsList.SelectedItem;//将选中的项转换为 Task 类型，并将其赋值给一个局部变量 task。这样就可以访问这个任务的各种属性。
                if(task.Conditions==true)
                {
                    Children.Find<LabelWidget>("DisplayTaskNameLabel").Text = task.Name+"（已完成）"; //在任务界面的子部件中查找 DisplayTaskNameLabel，这是一个显示任务名称的 LabelWidget，然后设置其文本为任务的名称。
                }
                else
                {
                    Children.Find<LabelWidget>("DisplayTaskNameLabel").Text = task.Name ; //在任务界面的子部件中查找 DisplayTaskNameLabel，这是一个显示任务名称的 LabelWidget，然后设置其文本为任务的名称。
                }

                Children.Find<LabelWidget>("DisplayTaskLabel").Text = task.Text;
                //同样地，查找并设置显示任务描述的 LabelWidget 的文本。

                Children.Find<LabelWidget>("DisplayTaskRewardsLabel").Text = task.RewardsText;
                //查找并设置显示任务奖励内容的 LabelWidget 的文本。

                if (Children.Find<ButtonWidget>("RewardsButton").IsClicked)//检查名为 RewardsButton 的按钮是否被点击。如果被点击，那么将执行奖励逻辑。
                {
                    //检查任务是否已经满足条件。对于其他任务，task.Conditions 表明任务是否已经完成。对于主线任务，m_componentTask.MainTaskConditions 表明主线任务是否已经完成。
                    if (task.Conditions || (task.TaskType == TaskType.Main && m_componentTask.MainTaskConditions&&task.Index==m_componentTask.MainTaskIndex))
                    {
                        //如果任务完成，则调用 task.Rewards 委托，传递 ComponentPlayer 和 SubsystemWorldSystem 实例。这个委托通常负责颁发任务奖励给玩家。
                        task.Rewards(m_componentTask.Entity.FindComponent<ComponentPlayer>(), m_componentTask.Project.FindSubsystem<SubsystemWorldSystem>(), m_componentTask.Project.FindSubsystem<SubsystemPickables>());
                        m_componentTask.RemoveTask(task);
                        if(task.Index==0)
                        {
                            string txt1 = task.Name;

                            m_componentTask.Entity.FindComponent<ComponentPlayer>().ComponentGui.DisplayLargeMessage("这里究竟是哪里？不管了，我得先生存下去……", "任务：" + txt1 + "已完成", 5f, 1f);
                        }
                        else if (task.Index==1)
                        {
                            string txt1 = task.Name;

                            m_componentTask.Entity.FindComponent<ComponentPlayer>().ComponentGui.DisplayLargeMessage("真是一个奇怪的人……", "任务：" + txt1 + "已完成", 3f, 1f);
                        }
                        else if(task.Index==2)
                        {
                            string txt1 = task.Name;

                            m_componentTask.Entity.FindComponent<ComponentPlayer>().ComponentGui.DisplayLargeMessage("真是宏伟的血池！它到底是哪里来的？令人毛骨悚然", "任务：" + txt1 + "已完成", 5f, 1f);
                        }
                        else if( task.Index==3)
                        {
                            string txt1 = task.Name;

                            m_componentTask.Entity.FindComponent<ComponentPlayer>().ComponentGui.DisplayLargeMessage("鸟人？可爱的女生？还是……", "任务：" + txt1 + "已完成", 5f, 1f);
                        }
                       
                        //m_itemsList.RemoveItem(task);

                    }
                    else
                    {
                        m_componentTask.Entity.FindComponent<ComponentGui>().DisplaySmallMessage(ModItemManger.GetName("TaskDisplay"), Color.White, true, true);
                    }
                }
            }
            else
            {
                IsMove = false;
            }
            if (IsMove)
            {
                if (m_displaytaskWidget.Size.X != 300f)
                {
                    m_displaytaskWidget.Size = new Vector2(m_displaytaskWidget.Size.X + m_displaytaskWidgetValue, 382f);
                }
                if (m_displaytaskWidget.Size.X == 300f && m_displayrewardstaskWidget.Size.X != 300f)
                {
                    m_displayrewardstaskWidget.Size = new Vector2(m_displayrewardstaskWidget.Size.X + m_displayrewardstaskWidgetValue, 382f);
                }
                if (m_displaytaskWidget.Size.X == 300f && m_displayrewardstaskWidget.Size.X == 300f)
                {
                    IsMove = false;
                    m_displayrewardstaskWidget.IsVisible = false;
                    m_displaytaskWidget.IsVisible = false;
                    Children.Find<CanvasWidget>("DisplayTask").IsVisible = true;
                    Children.Find<CanvasWidget>("GetRewards").IsVisible = true;
                }
            }
            else
            {
                if (m_itemsList.SelectedItem == null)
                {
                    m_displayrewardstaskWidget.IsVisible = true;
                    m_displaytaskWidget.IsVisible = true;
                    if (m_displaytaskWidget.Size.X != 0f && m_displayrewardstaskWidget.Size.X == 0f)
                    {
                        m_displaytaskWidget.Size = new Vector2(m_displaytaskWidget.Size.X - m_displaytaskWidgetValue, 382f);
                    }
                    if (m_displayrewardstaskWidget.Size.X != 0f)
                    {
                        m_displayrewardstaskWidget.Size = new Vector2(m_displayrewardstaskWidget.Size.X - m_displayrewardstaskWidgetValue, 382f);
                    }
                    Children.Find<CanvasWidget>("DisplayTask").IsVisible = false;
                    Children.Find<CanvasWidget>("GetRewards").IsVisible = false;
                }
            }
        }
    }
    public class SubsystemTask : Subsystem
    {
        public static List<Task> m_alltasks = new List<Task>();
        //m_alltasks 是一个静态的 List，用于存储所有的 Task 对象。这个列表是全局静态的，意味着所有 SubsystemTask 实例共享这个任务列表。
        public SubsystemWorldSystem m_subsystemWorldSystem;

        public override void Load(ValuesDictionary valuesDictionary)
        {
            m_subsystemWorldSystem = Project.FindSubsystem<SubsystemWorldSystem>(throwOnError: true);
            if (m_alltasks.Count == 0)//这是一个条件判断，它检查任务列表是否为空。如果为空，就会初始化任务列表。
            {
                m_alltasks.Add(new Task()//在列表为空的情况下，这行代码往 m_alltasks 列表中添加一个新的 Task 实例。
                {
                    Name = "开通vip",
                    Text = "这个商店的vip看起来是非常有作用的，先开通试试吧",
                    RewardsText = "开通后给予奖励2000$",
                    TaskType = TaskType.Other,
                    Index = -1,
                    Rewards = delegate (ComponentPlayer player, SubsystemWorldSystem worldSystem,SubsystemPickables subsystemPickables)
                    {
                        player.Entity.FindComponent<ComponentPlayerSystem>().PlayerInfo.Money += 2000;
                        
                    },
                });
                m_alltasks.Add(new Task()
                {
                    Name = "初入岛屿",
                    Text = "……，你在迷茫中醒来。你不知道自己曾经发生了什么，只知道一醒来，你就在这片大陆上了。整个大陆上似乎非同一般，到处去看看吧。\n（随便行动100米，完成任务）",
                    RewardsText = "你的行动距离需要超过100。任务完成后，进度+10，实体灵能+10",
                    TaskType = TaskType.Main,
                    Index = 0,
                    Rewards = delegate (ComponentPlayer player, SubsystemWorldSystem worldSystem,SubsystemPickables subsystemPickables)
                    {
                        player.Entity.FindComponent<ComponentPlayerSystem>().PlayerInfo.Money += 200;
                        player.Entity.FindComponent<ComponentTest1>().m_mplevel += 10;
                        MoneyManager.AddItem(player.Entity.FindComponent<ComponentMiner>(), subsystemPickables, MoneyManager.CoinValue, 10);
                    },
                });
                m_alltasks.Add(new Task()
                {
                    Name = "召唤洛德",
                    Text = "你可以随时按右上角的设置面板召唤商人洛德，你可以与他闲聊，也可以和他交易,游戏的唯一货币为实体灵能，这个世界的人不能没有灵能！。记住，游戏里的剧情信息还是比较重要的，所以我建议你不要遗漏，沉浸地体验这个世界。\n（空手与洛德交互完成任务）",
                    RewardsText = "任务完成后，进度+10，实体灵能+20",
                    TaskType = TaskType.Main,
                    Index = 1,
                    Rewards = delegate (ComponentPlayer player, SubsystemWorldSystem worldSystem, SubsystemPickables subsystemPickables)
                    {
                        player.Entity.FindComponent<ComponentPlayerSystem>().PlayerInfo.Money += 200;
                        player.Entity.FindComponent<ComponentTest1>().m_mplevel += 10;
                        MoneyManager.AddItem(player.Entity.FindComponent<ComponentMiner>(), subsystemPickables, MoneyManager.CoinValue, 20);
                    },
                });
                m_alltasks.Add(new Task()
                {
                    Name = "前往血池",
                    Text = "经过一番发育之后，想必你已经做好了前往血池的准备。血泪之池的坐标为（1024，1024）。这段路途可不短！请务必做好充足的准备！\n（抵达血泪之池完成任务）",
                    RewardsText = "任务完成后，进度+30，实体灵能+100",
                    TaskType = TaskType.Main,
                    Index = 2,
                    Rewards = delegate (ComponentPlayer player, SubsystemWorldSystem worldSystem, SubsystemPickables subsystemPickables)
                    {
                        player.Entity.FindComponent<ComponentPlayerSystem>().PlayerInfo.Money += 200;
                        player.Entity.FindComponent<ComponentTest1>().m_mplevel += 30;
                        MoneyManager.AddItem(player.Entity.FindComponent<ComponentMiner>(), subsystemPickables, MoneyManager.CoinValue, 100);
                    },
                });
                m_alltasks.Add(new Task()
                {
                    Name = "渡渡来咯",
                    Text = "在日夜被猩红血气笼罩的血泪之池中，生活着一只渡渡鸟。她的名字叫愚鸟渡渡。一个乐观开朗的元气少女……\n（空手与渡渡交互完成任务）",
                    RewardsText = "任务完成后，进度+100，实体灵能+100",
                    TaskType = TaskType.Main,
                    Index = 3,
                    Rewards = delegate (ComponentPlayer player, SubsystemWorldSystem worldSystem, SubsystemPickables subsystemPickables)
                    {
                        player.Entity.FindComponent<ComponentPlayerSystem>().PlayerInfo.Money += 200;
                        player.Entity.FindComponent<ComponentTest1>().m_mplevel += 100;
                        MoneyManager.AddItem(player.Entity.FindComponent<ComponentMiner>(), subsystemPickables, MoneyManager.CoinValue, 100);
                    },
                });
            }
        }

        public static Task GetTask(string name, TaskType taskType)//这是一个静态方法，用于根据任务名称和类型检索特定的 Task 对象。
        {
            foreach (var task in m_alltasks)//这个循环遍历所有任务。
            {
                if (task.Name == name && task.TaskType == taskType)//在循环内部，检查每个任务的 Name 和 TaskType 是否与传入的参数匹配。如果匹配，则返回该任务。
                {
                    return task;
                }
            }
            return GetBaseTask();
        }

        public static Task GetBaseTask()//这是一个静态方法，创建并返回一个基础任务，这个任务可能是当没有其他任务匹配时的默认任务。
        {
            var task = new Task()
            {
                Name = "前面的章节以后再来探索吧！",
                Text = "恭喜你成功达成了本次版本所有的任务，您可以接取副线任务，或者继续游玩下去",
                RewardsText = "没有任何奖励可以领了",
                TaskType = TaskType.Main,
                Index = 0,
                Rewards = delegate
                {
                }
            };
            return task;
        }

        public static Task GetTask(int index)//这是另一个静态方法，它通过任务的索引来检索任务。
        {
            foreach (var task in m_alltasks)
            {
                if (task.Index == index)
                {
                    return task;
                }
            }
            return GetBaseTask();
        }

        public override void Save(ValuesDictionary valuesDictionary)
        {
        }
    }

    public class ComponentTask : Component, IUpdateable
    {
       
        public UpdateOrder UpdateOrder => UpdateOrder.Default;

        public ComponentPlayer m_componentPlayer;

        public ComponentPlayerSystem m_componentPlayerSystem;

        public Dictionary<string, Task> m_othertasks
        {
            get
            {
                return m_subsystemWorldSystem.GetPlayerInfo(m_componentPlayer.PlayerData.PlayerIndex).m_othertasks;
                //这个属性的 get 访问器通过 m_subsystemWorldSystem 获取当前玩家的信息，并返回该玩家的其他任务列表。
            }
        }

        public int MainTaskIndex;//声明了一个 int 类型的成员变量 MainTaskIndex，用来跟踪玩家当前主线任务的索引。

        public bool MainTaskConditions;//声明了一个 bool 类型的成员变量 MainTaskConditions，用来指示当前主线的任务是否已完成，这个只能用来标注当前的一个任务。

        public SubsystemWorldSystem m_subsystemWorldSystem;
        public Task tasknow;
        public bool showmessage=false;

        public void Update(float dt)
        {
           
            tasknow = SubsystemTask.GetTask(MainTaskIndex);
            if (m_componentPlayer.PlayerStats.DistanceTravelled >= 100 && MainTaskIndex == 0)
            {
               
                //在 Update 方法中，如果玩家旅行的距离超过了100，并且当前的主线任务索引为0，则表示这个主线任务已经完成。
                FinishedTask(null, TaskType.Main);//FinishedTask 方法用于标记一个任务为完成状态。
                
               
                   
                
                
            }
            else if (Lordtask1 == true && MainTaskIndex == 1)
            {
                
                //在 Update 方法中，如果玩家旅行的距离超过了100，并且当前的主线任务索引为0，则表示这个主线任务已经完成。
                FinishedTask(null, TaskType.Main);//FinishedTask 方法用于标记一个任务为完成状态。
                
                
                
                   
            }
            else if (BloodPool == true && MainTaskIndex == 2)
            {
                
                //在 Update 方法中，如果玩家旅行的距离超过了100，并且当前的主线任务索引为0，则表示这个主线任务已经完成。
                FinishedTask(null, TaskType.Main);//FinishedTask 方法用于标记一个任务为完成状态。
                
               
                
                    
            }
            else if (Dodo1 == true && MainTaskIndex == 3)
            {
                
                //在 Update 方法中，如果玩家旅行的距离超过了100，并且当前的主线任务索引为0，则表示这个主线任务已经完成。
                FinishedTask(null, TaskType.Main);//FinishedTask 方法用于标记一个任务为完成状态。
               
              
                
                    
            }
        }

        public void FinishedTask(string name, TaskType taskType)
        {
            //如果任务类型是 Other，则在 m_othertasks 字典中找到这个任务并将其 Conditions 设置为 true，否则将 MainTaskConditions 设置为 true。
            if (taskType == TaskType.Other) m_othertasks[name].Conditions = true;
            else MainTaskConditions = true;
        }

        public void AddTask(string name, bool conditions)//AddTask 方法用于向玩家的其他任务列表中添加新任务。
        {
            //这段代码从子系统中获取任务，设置任务的完成状态，并将其添加到玩家的其他任务列表中。
            var task = SubsystemTask.GetTask(name, TaskType.Other);
            task.Conditions = conditions;
            Log.Information("add" + name);
            m_othertasks.Add(name, task);
        }

        //RemoveTask 方法用于从玩家的任务列表中移除一个任务。
        //在尝试块中，根据任务类型，从其他任务列表中移除任务或者递增主线任务索引并重置其完成状态。
        public void RemoveTask(Task task)
        {
            try
            {
                if (task.TaskType == TaskType.Other)
                {
                    m_othertasks.Remove(task.Name);
                }
                else
                {
                    MainTaskIndex++;
                    MainTaskConditions = false;
                }

            }
            catch (Exception e) { Log.Information(e.ToString()); }
        }


        public bool Lordtask1;//召唤洛德任务
        public bool BloodPool;//前往血泪池子
        public bool Dodo1;//与渡渡交互，渡渡任务1
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
            m_componentPlayerSystem = Entity.FindComponent<ComponentPlayerSystem>(true);
            m_subsystemWorldSystem = Project.FindSubsystem<SubsystemWorldSystem>(throwOnError: true);
            MainTaskIndex = valuesDictionary.GetValue("MainTaskIndex", 0);
            MainTaskConditions = valuesDictionary.GetValue("MainTaskConditions", false);

            //任务完成变量控制
            Lordtask1 = valuesDictionary.GetValue<bool>("Lordtask", false);
            BloodPool = valuesDictionary.GetValue<bool>("BloodPool", false);
            Dodo1 = valuesDictionary.GetValue<bool>("Dodo1", false);
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue("MainTaskIndex", MainTaskIndex);
            valuesDictionary.SetValue("MainTaskConditions", MainTaskConditions);
            valuesDictionary.SetValue<bool>("Lordtask1", Lordtask1);
            valuesDictionary.SetValue<bool>("BloodPool", BloodPool);
            valuesDictionary.SetValue<bool>("Dodo1", Dodo1);
        }
    }
    public static class ModItemManger
    {
        public static List<ItemData> m_itemDatas = new List<ItemData>();

        public static Color MainUiColor = new Color(255, 0, 0);

        public static bool CreaterMode;

        public static int MaxSellCount;

        public static void Initialize(ValuesDictionary valuesDictionary)
        {
            CreaterMode = valuesDictionary.GetValue("CreaterMode", false);
            MainUiColor = valuesDictionary.GetValue("MainUiColor", Color.Red);
            MaxSellCount = valuesDictionary.GetValue("MaxSellCount", 40);
            foreach (XElement item in ContentManager.Get<XElement>("ItemDatas").Elements())
            {
                m_itemDatas.Add(new ItemData()
                {
                    Index = XmlUtils.GetAttributeValue<string>(item, "DisplayName") == "Null" ? -1 : CraftingRecipesManager.DecodeResult(XmlUtils.GetAttributeValue<string>(item, "DisplayName")),
                    BuyMoney = XmlUtils.GetAttributeValue<int>(item, "BuyMoney"),
                    HitTime = XmlUtils.GetAttributeValue<double>(item, "HitTime"),
                    AimTime = XmlUtils.GetAttributeValue<float>(item, "AimTime"),
                    HitDistance = XmlUtils.GetAttributeValue<float>(item, "HitDistance"),
                    MoveScreenWhenAim = XmlUtils.GetAttributeValue<bool>(item, "MoveScreenWhenAim"),
                    SellMoney = XmlUtils.GetAttributeValue<int>(item, "SellMoney"),
                    Quality = XmlUtils.GetAttributeValue<float>(item, "Quality"),
                });
            }
        }

        public static ItemData GetItemData(int index)
        {
            foreach (var data in m_itemDatas)
            {
                if (data.Index == index)
                {
                    return data;
                }
            }
            return m_itemDatas[0];
        }

        public static string GetName(string name)
        {
            foreach (XElement item in ContentManager.Get<XElement>("GameDatas").Descendants("LanguageData"))
            {
                if (XmlUtils.GetAttributeValue<string>(item, "Name") == name)
                {
                    return XmlUtils.GetAttributeValue<string>(item, "ChineseText");
                }
            }
            return string.Empty;
        }

        public static void Load(ValuesDictionary valuesDictionary)
        {
        }

        public static void Save(ValuesDictionary valuesDictionary)
        {
            valuesDictionary.SetValue("CreaterMode", CreaterMode);
            valuesDictionary.SetValue("MainUiColor", MainUiColor);
            valuesDictionary.SetValue("MaxSellCount", MaxSellCount);
        }
    }
    public class ItemData
    {
        public int Index;

        public int BuyMoney;

        public int SellMoney;

        public double HitTime;

        public float AimTime;

        public bool MoveScreenWhenAim;

        public float HitDistance;

        public float Quality;
    }
    #endregion
}