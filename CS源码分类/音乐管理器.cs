using Engine.Audio;
using Engine.Media;
using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;
using Test1;

namespace Game
{
    
        public class FCMusicManager : Subsystem, IUpdateable
        {
            public SubsystemWeather m_subsystemWeather;
            public SubsystemPlayers m_subsystemPlayer;
            public ComponentPlayer m_componentPlayer;
            public ComponentTest1 m_componentTest1;
            public SubsystemNaturallyBuildings m_subsystemNaturallyBuildings;
            public SubsystemWorldDemo m_subsystemWorldDemo;
            public bool flag_surface = false;
            public bool flag_cave = false;
            public bool flag_space = false;
            public bool flag_defualt = false;
            public bool flag_Blood = false;
            public bool flag_Sen = false;
            public UpdateOrder UpdateOrder
            {
                get
                {
                    return UpdateOrder.Input;
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

            public static bool IsPlaying
            {
                get
                {
                    return m_sound != null && m_sound.State > SoundState.Stopped;
                }
            }

            public static float Volume
            {
                get
                {
                    return SettingsManager.MusicVolume * 0.6f;
                }
            }
            public Point3 point;
            private SubsystemGameInfo m_subsystemGameInfo;
            public void Update(float dt)
            {

                if (m_componentPlayer != null)
                {
                    point = Terrain.ToCell(m_componentPlayer.ComponentBody.Position);//玩家所在方块坐标
                }

                if (point.Y > 600f)
                {
                    m_subsystemWeather.m_precipitationEndTime = m_subsystemGameInfo.TotalElapsedGameTime;

                }

                //用来判断位置变换。
                if (m_sound != null)
                {
                    Point2 pointcoords = ((int)point.X / 16, (int)point.Z / 16);
                    WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界 //273秒

                    if (m_componentTest1 != null)//Sen为音乐总判断
                    {
                        if (m_componentTest1.m_sen <= 20)
                        {
                            if (flag_Sen == false)
                            {
                                StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(10f, 15f);//3分半-4分钟
                            }
                        }
                        else if (worldType == WorldType.Default)//如果是主世界（地球）
                        {
                            if (m_componentTest1 != null)
                            {
                                if (m_componentTest1.Areaname == "血泪之池")
                                {
                                    if (flag_Blood == false)
                                    {
                                        StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                        m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(10f, 15f);//3分半-4分钟
                                    }
                                }
                                else
                                {
                                    if (point.Y <= 54)
                                    {
                                        if (flag_cave == false)
                                        {
                                            StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(20f, 30f);//3分半-4分钟     
                                        }
                                        //洞穴音乐



                                    }
                                    else if (point.Y > 54 && point.Y <= 256)
                                    {
                                        if (flag_surface == false)
                                        {
                                            StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(20f, 30f);//3分半-4分钟     
                                        }
                                        //地表

                                    }
                                    else if (point.Y > 1000)
                                    {
                                        if (flag_space == false)
                                        {
                                            StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(20f, 30f);//3分半-4分钟     
                                        }
                                        //宇宙

                                    }
                                    else
                                    {
                                        if (flag_defualt == false)
                                        {
                                            StopMusic();//如果位置变换但当前音乐不是洞穴音乐，开始变换。
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(20f, 30f);//3分半-4分钟      
                                        }
                                        //默认地表

                                    }
                                }
                            }



                        }
                    }


                }
                // 检查是否有音乐正在淡出
                if (m_fadeSound != null)
                {
                    //// 减少淡出音乐的音量
                    m_fadeSound.Volume = MathUtils.Min(m_fadeSound.Volume - 0.1f * Volume * Time.FrameDuration, Volume);
                    // // 如果淡出音乐的音量已降至0或以下，则释放音乐资源并将其置为null
                    if (m_fadeSound.Volume <= 0f)
                    {
                        m_fadeSound.Dispose();
                        m_fadeSound = null;
                    }
                }
                //// 检查当前是否有音乐正在播放，并且是否到达淡入音乐的时间
                if (m_sound != null && Time.FrameStartTime >= m_fadeStartTime)
                {
                    //增加音乐的音量进行淡入,直到音量和设置里的音量相同
                    m_sound.Volume = MathUtils.Min(m_sound.Volume + 0.33f * Volume * Time.FrameDuration, Volume);
                }
                //// 检查当前是否没有音乐混合播放或音量为0
                if (Volume == 0f)
                {
                    StopMusic();
                    return;
                }

                //// 检查当前音乐混合类型是否为菜单，并且是否到了播放下一首歌的时间并且没有音乐正在播放,这里是设置音乐播放的核心，前面是对音乐淡入淡出的控制
                if (Time.FrameStartTime >= m_nextSongTime)
                {
                    if (IsPlaying == true)
                    {
                        StopMusic();

                    }
                    if (IsPlaying == false)
                    {
                        float startPercentage = 0f;//播放开始的片段锁定在开头，即完整播放。


                        WorldType worldType = m_subsystemWorldDemo.worldType;//获取当前所在世界
                        if (m_componentTest1 != null)//Sen为音乐总判断
                        {
                            if (m_componentTest1.m_sen <= 20)
                            {
                                PlayMusic("BackgroundMusic/LowSen", startPercentage);//洞穴音乐
                                flag_Sen = true;
                                m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(300f, 310f);//3分半-4分钟
                                Log.Information(string.Format("低sen音乐已经播放！"));
                            }
                            else if (worldType == WorldType.Default)//如果是主世界（地球）
                            {
                                if (m_componentTest1 != null)
                                {
                                    if (m_componentTest1.Areaname == "血泪之池")
                                    {
                                        PlayMusic("BackgroundMusic/Blood", startPercentage);//洞穴音乐
                                        flag_Blood = true;
                                        m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(120f, 130f);//3分半-4分钟
                                        Log.Information(string.Format("血泪音乐已经播放！"));
                                    }
                                    else
                                    {
                                        if (point.Y <= 54)
                                        {
                                            PlayMusic("BackgroundMusic/CaveMusic", startPercentage);//洞穴音乐
                                            flag_cave = true;
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(212f, 240f);//3分半-4分钟
                                            Log.Information(string.Format("地球洞穴音乐已经播放！"));
                                            // 洞穴音乐


                                        }
                                        else if (point.Y > 54 && point.Y <= 256)
                                        {
                                            PlayMusic("BackgroundMusic/SurfaceMusic", startPercentage);//地表
                                            flag_surface = true;
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(212f, 240f);//3分半-4分钟
                                            Log.Information(string.Format("地球地表音乐已经播放！"));

                                        }
                                        else if (point.Y > 1000)
                                        {
                                            PlayMusic("BackgroundMusic/SpaceMusic", startPercentage);//宇宙
                                            flag_space = true;
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(212f, 240f);//3分半-4分钟
                                            Log.Information(string.Format("地球太空音乐已经播放！"));
                                        }
                                        else
                                        {

                                            PlayMusic("BackgroundMusic/SurfaceMusic", startPercentage);//默认地表
                                            flag_defualt = true;
                                            m_nextSongTime = Time.FrameStartTime + (double)m_random.Float(212f, 240f);//3分半-4分钟
                                            Log.Information(string.Format("地球地表音乐已经播放！"));
                                        }
                                    }
                                }



                            }
                        }

                    }
                    //float startPercentage = IsPlaying ? m_random.Float(0f, 0.75f) : 0f;


                    // 如果ContentMusicPath为空，使用随机数选择要播放的音乐
                    /*switch (m_random.Int(0, 5))
                    {
                        case 0:
                            PlayMusic("BackgroundMusic/NativeAmericanFluteSpirit", startPercentage);
                            break;
                        case 1:
                            PlayMusic("BackgroundMusic/AloneForever", startPercentage);
                            break;
                        case 2:
                            PlayMusic("BackgroundMusic/NativeAmerican", startPercentage);
                            break;
                        case 3:
                            PlayMusic("BackgroundMusic/NativeAmericanHeart", startPercentage);
                            break;
                        case 4:
                            PlayMusic("BackgroundMusic/NativeAmericanPeaceFlute", startPercentage);
                            break;
                        case 5:
                            PlayMusic("BackgroundMusic/NativeIndianChant", startPercentage);
                            break;
                    }*/


                }
            }

            public override void Load(ValuesDictionary valuesDictionary)
            {
                m_subsystemWeather = Project.FindSubsystem<SubsystemWeather>(true);
                m_subsystemPlayer = Project.FindSubsystem<SubsystemPlayers>();
                m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
                m_subsystemNaturallyBuildings = Project.FindSubsystem<SubsystemNaturallyBuildings>(true);
                m_subsystemWorldDemo = Project.FindSubsystem<SubsystemWorldDemo>(true);
                base.Load(valuesDictionary);
            }

            public override void Save(ValuesDictionary valuesDictionary)
            {
                base.Save(valuesDictionary);
            }

            public void PlayMusic(string name, float startPercentage)
            {
                if (string.IsNullOrEmpty(name))
                {
                    StopMusic();
                    return;
                }
                try
                {
                    StopMusic();
                    m_fadeStartTime = Time.FrameStartTime + 2.0;
                    float volume = (m_fadeSound != null) ? 0f : Volume;
                    StreamingSource streamingSource = ContentManager.Get<StreamingSource>(name, ".ogg").Duplicate();
                    streamingSource.Position = (long)(MathUtils.Saturate(startPercentage) * (float)(streamingSource.BytesCount / (long)streamingSource.ChannelsCount / 2L)) / 16L * 16L;
                    m_sound = new StreamingSound(streamingSource, volume, 1f, 0f, false, true, 1f);
                    m_sound.Play();
                }
                catch
                {
                    Log.Warning("Error playing music \"{0}\".", new object[]
                    {
                    name
                    });
                }
            }

            public void StopMusic()
            {

                if (m_sound != null)
                {
                    if (m_fadeSound != null)
                    {
                        m_fadeSound.Dispose();
                    }
                    m_sound.Stop();
                    if (flag_cave == true)
                    {
                        flag_cave = false;
                    }
                    if (flag_surface == true)
                    {
                        flag_surface = false;
                    }
                    if (flag_space == true)
                    {
                        flag_space = false;
                    }
                    if (flag_defualt == true)
                    {
                        flag_defualt = false;
                    }
                    if (flag_Blood == true)
                    {
                        flag_Blood = false;
                    }
                    if (flag_Sen == true)
                    {
                        flag_Sen = false;

                    }

                    m_fadeSound = m_sound;
                    m_sound.Stop();
                    m_sound.Dispose(); // 确保你处理了音乐。
                    m_sound = null;    // 将m_sound设置为null。

                }
            }

            public const float m_fadeSpeed = 0.33f;

            public const float m_fadeWait = 2f;

            public static StreamingSound m_fadeSound;

            public static StreamingSound m_sound;

            public static double m_fadeStartTime;



            public static double m_nextSongTime;

            public static Random m_random = new Random();


        }




}//音乐系统
