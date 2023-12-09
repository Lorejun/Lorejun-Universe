using Engine.Graphics;
using Engine.Media;
using Engine;
using Game;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TemplatesDatabase;
using Test1;

namespace Game
{
    public class Achievements//成就类型
    {
        public string Name;//名称

        public Action<object[]> AchievementsAction;//当完成时发生的事件

        public bool CompletionStatus;//是否完成

        public string TextureName;//展示完成成就时所显示的图片

        public string Text;//查看时的内容

        public string Text2;//点开成就内的描述
    }

    public class ComponentAchievements : Component, IUpdateable//注册新的成就组件
    {
        public UpdateOrder UpdateOrder => UpdateOrder.Default;//获取当前更新更新事件频率

        public bool flag2;//声明一个bool类型的值用于辅作作用

        public double lasttime;//偏移量转至最左端所需要持续的时间

        public CanvasWidget AchievementsWidget;

        public SubsystemTime subsystemTime;

        public SubsystemAchievements subsystemAchievements;
        public SubsystemAudio m_subsystemAudio;

        public void DisplayAchievementsMessage(string name, object[] items)//如果成就被完成，调用此方法来通知，items为基于奖励事件所必要要用到的变量
        {
            if (subsystemAchievements.UseAchievements(name, items, out string name2))//应用当前的成就，这是一个二级调用。首先先用use来看看成就是否被完成，而这个use本质上返回的是成就类里的完成bool值。
            {
                //如果成就还没有被完成，则执行以下代码，完成成就。完成成就的bool值变更在use方法内。
                AchievementsWidget.IsVisible = true;//展示成就显示界面
                AchievementsWidget.Margin = new Vector2(-AchievementsWidget.Size.X, 0);//将偏移量转至最右转
                flag2 = true;
                AchievementsWidget.Children.Find<RectangleWidget>("Achievements.Icon").Subtexture = new Subtexture(ContentManager.Get<Texture2D>(name2), Vector2.Zero, Vector2.One);//给当前显示成就界面，赋予所需要显示的图片
                AchievementsWidget.Children.Find<LabelWidget>("Achievements.Label").Text = "恭喜你完成了成就-" + name;//给予当前显示成就界面所有的文字描述
                m_subsystemAudio.PlaySound("Audio/achieve", 1f, 0f, 0f, 0.1f);
                
            }
        }

        public void Update(float dt)//当执行更新事件时
        {
            if (AchievementsWidget.IsVisible)//当提示成就界面显示时
            {
                if (AchievementsWidget.Margin.X != 0 && flag2)//如果偏移量不为0，
                {
                    AchievementsWidget.Margin += new Vector2(10f, 0);//持续将该界面的偏移量往界面左边驶去
                }
                else if (subsystemTime.GameTime - lasttime > 4 && !flag2 && AchievementsWidget.Margin.X != -AchievementsWidget.Size.X)//如果当前已达到至最左端时，且该状态已经持续了4秒时
                {
                    AchievementsWidget.Margin -= new Vector2(10f, 0);//持续将该界面往右边驱使
                }
                if (AchievementsWidget.Margin.X == 0 && flag2)//当偏移量已达到至最左端时
                {
                    lasttime = subsystemTime.GameTime;//将该当前显示时间赋值为现在的游戏时间
                    flag2 = false;
                }
                else if (AchievementsWidget.Margin.X == -AchievementsWidget.Size.X)//如果往右偏移，偏移量已达到最大值时
                {
                    flag2 = true;
                    AchievementsWidget.IsVisible = false;//将当前界面隐藏
                }
            }
            else
            {
                AchievementsWidget.Margin = new Vector2(-AchievementsWidget.Size.X, 0);//使当前界面的偏移量为最右端
            }
        }

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>();
            subsystemAchievements = Project.FindSubsystem<SubsystemAchievements>(throwOnError: true);
            subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            AchievementsWidget = Entity.FindComponent<ComponentPlayer>(throwOnError: true).GuiWidget.Children.Find<CanvasWidget>("Achievements");//从GameWidget.Xml文件中获取提示成就界面
        }
    }
    public class ComponentXInventory : ComponentInventory
    {
        public override void AddSlotItems(int slotIndex, int value, int count)
        {
            if (value == 62)
            {
                Entity.FindComponent<ComponentAchievements>(throwOnError: true).DisplayAchievementsMessage("极寒中的冰块", new object[1] { Entity.FindComponent<ComponentGui>(throwOnError: true) });
            }

            base.AddSlotItems(slotIndex, value, count);
        }
    }

    public class FYJD模板Loader : ModLoader//注册新的ModLoader
    {
        public SubsystemAchievements subsystemAchievements;
        public override void __ModInitialize()
        {
            ModsManager.RegisterHook("ClothingWidgetOpen", this);//实例化ClothingWidgetOpen方法
            ModsManager.RegisterHook("OnProjectLoaded", this);//实例化OnProjectLoaded方法
        }

        public override void ClothingWidgetOpen(ComponentGui componentGui, ClothingWidget clothingWidget)//当玩家打开衣服界面时
        {
            StackPanelWidget stackPanelWidget = (StackPanelWidget)clothingWidget.Children.Find<StackPanelWidget>();//寻找在该ClothingWidget界面中出现的第1个StackPanelWidget
            var bevelledButtonWidget = new BevelledButtonWidget//声明一个新的按钮变量
            {
                Name = "AchievementsButton",//名称
                Font = ContentManager.Get<BitmapFont>("Fonts/Pericles"),//获取字体，可默认不获取
                Size = new Vector2(70f, 60f),//设置按钮大小
                IsEnabled = true
            };
            var rectangleWidget = new RectangleWidget()//声明一个图片界面
            {
                FillColor = new Color(255, 255, 255),//背景以内的图片颜色
                OutlineColor = new Color(0, 0, 0, 0),//边缘线的颜色
                Subtexture = new Subtexture(ContentManager.Get<Texture2D>("Textures/Ui/Achievements"), Vector2.Zero, Vector2.One)//获取展示的图片文件
            };
            bevelledButtonWidget.Children.Add(rectangleWidget);//在该按钮中添加图片界面
            stackPanelWidget.Children.Add(bevelledButtonWidget);//在该StackPanelWidget界面中添加你按钮界面
            clothingWidget.Update1 += delegate//在更新该ClothingWidget界面时的方法
            {
                clothingWidget.Update();//更新原来的方法
                if (bevelledButtonWidget.IsClicked)//当该按钮被轻触一次时
                {
                    componentGui.ModalPanelWidget = new AchievementsWidget(subsystemAchievements);//将当前界面转至AchievementsWidget
                }
            };
        }

        public override void OnProjectLoaded(Project project)
        {
            subsystemAchievements = project.FindSubsystem<SubsystemAchievements>(throwOnError: true);
        }
    }

    public class AchievementsWidget : CanvasWidget
    {
        public SubsystemAchievements m_subsystemAchievements;

        public ListPanelWidget m_itemsList;//成就列表

        public AchievementsWidget(SubsystemAchievements subsystemAchievements)
        {
            m_subsystemAchievements = subsystemAchievements;
            XElement node = ContentManager.Get<XElement>("Widgets/AchievementsWidget");
            LoadContents(this, node);
            m_itemsList = Children.Find<ListPanelWidget>("AchievementsList");
            m_itemsList.ItemWidgetFactory = delegate (object item)//当成就列表时，每一个成就所展示的像
            {
                var achievements = (Achievements)item;
                XElement node2 = ContentManager.Get<XElement>("Widgets/AchievementsItem");//获取展示单独一个成像时的Widget
                var obj = (ContainerWidget)LoadWidget(this, node2, null);//加载当前界面(获取xml中Widget至加载至该类中)
                string CompletionStatus = achievements.CompletionStatus ? "己完成" : "未完成";//提示判断
                obj.Children.Find<RectangleWidget>("AchievementsItem.Icon").Subtexture = new Subtexture(ContentManager.Get<Texture2D>(achievements.TextureName), Vector2.Zero, Vector2.One);//展示当前项的图片
                obj.Children.Find<LabelWidget>("AchievementsItem.Name").Text = achievements.Name;//展示该项的名称
                obj.Children.Find<LabelWidget>("AchievementsItem.Text").Text = " " + achievements.Text + " 完成情况：" + CompletionStatus;//展示该项的内容
                return obj;
            };
            m_itemsList.ItemClicked += delegate (object item)//当该项被选中时
            {
                var achievements2 = (Achievements)item;
            };
            foreach (Achievements achievements3 in subsystemAchievements.m_achievements)//遍历所有的成就
            {
                m_itemsList.AddItem(achievements3);
            }
        }

        public override void Update()
        {
            if (Children.Find<ButtonWidget>("ClearButton").IsClicked)//当所有成就清除按钮按下时
            {
                foreach (Achievements achievements4 in m_subsystemAchievements.m_achievements)
                {
                    achievements4.CompletionStatus = false;//将所有的成就清除
                }
            }
            Children.Find<ButtonWidget>("SeeButton").IsVisible = m_itemsList.SelectedItem != null;//判断是否。成就列表中有项被选择
            if (Children.Find<ButtonWidget>("SeeButton").IsClicked)//当查看被点击时
            {
                var achievements4 = (Achievements)m_itemsList.SelectedItem;//通过获得该被选择的项，来获得成就文件
                string CompletionStatus2 = achievements4.CompletionStatus ? "己完成" : "未完成";//成就完成判断
                DialogsManager.ShowDialog(null, new MessageDialog(achievements4.Name + "  完成情况：" + CompletionStatus2, achievements4.Text+achievements4.Text2, LanguageControl.Ok, null, new Vector2(620f, 420f), null));//展示内容
            }
        }
    }

    public class SubsystemAchievements : Subsystem//成就子系统
    {
        public List<Achievements> m_achievements = new List<Achievements>();//声明所有成就类
        public ComponentTest1 m_componentTest1;
        public ComponentPlayer m_componentPlayer;
        public SubsystemAudio m_subsystemAudio;
        public SubsystemPlayers m_subsystemPlayer;
        public override void Load(ValuesDictionary valuesDictionary)//当加载世界时
        {
            
            InitializeAchievements();//初始化所有成就
            foreach (Achievements item in m_achievements)//遍历所有的成就
            {
                item.CompletionStatus = valuesDictionary.GetValue(item.Name, false);//将存档xml中所有的存储的成就进度，赋值给当前所有的成就
            }
            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
            m_subsystemPlayer = Project.FindSubsystem<SubsystemPlayers>(true);
        }

        public override void Save(ValuesDictionary valuesDictionary)//当保存世界时，(退出世界加载/每120秒加载一次)
        {
            foreach (Achievements item in m_achievements)//遍历所有成就
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

        public void InitializeAchievements()
        {
            m_achievements.Add(new Achievements()//添加一个成就
            {
                Name = "极寒中的冰块",//名称
                TextureName = "Textures/Achievements/ice",//展示时所取的图片路径名称
                Text = "拾取一块冰",//成就内容
                AchievementsAction = delegate (object[] item)//当完成成就时所发生的事件
                {
                    var a = (ComponentGui)item[0];//通过object获取ComponentGui
                    a.DisplaySmallMessage("多么漂亮的一个冰块", Color.White, true, true);//给予提示
                    if (m_componentPlayer != null)
                    {
                        m_componentTest1.m_mplevel += 5;
                    }
                    
                },
                Text2 ="冰块在现代都市似乎是唾手可得的东西，但是对于流落到这片陌生大陆艰难求生的你，这似乎是一种象征——你战胜了严寒的环境。 （完成奖励进度值加5）"
            });
            m_achievements.Add(new Achievements()//上同
            {
                Name = "神灵的庇护",
                TextureName = "Textures/Achievements/Cross",
                Text = "手持神圣十字架，触发重生",
                AchievementsAction = delegate (object[] item)
                {
                    var a = (ComponentGui)item[0];
                    a.DisplaySmallMessage("感佑神灵庇护", Color.White, true, true);
                },
                Text2 = ""
            });
        }

        public Achievements GetAchievements(string name)//通过成就名称获取所有成就中的一个成就
        {
            foreach (var item in m_achievements)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            throw new InvalidOperationException("获取成就失败.");//若无法获取则报错
        }

        public bool UseAchievements(string name, object[] items, out string name2)
        {
            Achievements item = GetAchievements(name);//获取成就对象,并赋值给item，item传参到成就委托函数实现一些方法
            if (!item.CompletionStatus)//如果当前成就没有被完成
            {
                item.CompletionStatus = true;//使当前成就完成
                item.AchievementsAction(items);//执行当前成就要触发的奖励事件
                name2 = item.TextureName;//返回当前成就的图片获取路径名称
                return true;//返回使用成功
            }
            name2 = null;//因为已经被完成了，不返回当前成就图片的获取路径名称
            return false;//返回使用失败
        }
    }

   
}
