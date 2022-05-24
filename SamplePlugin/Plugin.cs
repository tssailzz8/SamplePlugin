using Dalamud.Game;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Hooking;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SamplePlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sample Plugin";

        private const string commandName = "/faka";
        public DateTime DoSomeThing;
        private static readonly Dictionary<Type, JobGaugeBase> JobGaugeCache = new();
        private CommandManager CommandManager { get; init; }
        public readonly HashSet<ushort> ZeroBuff = new HashSet<ushort>
        {
            1882,//太阳神之衡
            1883,//世界树之干
            1884,//放浪神之箭
            1885,//战争神之枪
            1886,//河流神之瓶
            1887,//建筑神之塔
        };
        private Configuration Configuration { get; init; }
        public IntPtr actionManagerInPtr;
        private PluginUI PluginUi { get; init; }
        public unsafe delegate byte UseActionDelegate(ActionManager* actionManager, uint actionType, uint actionID, long targetObjectID, uint param, uint useType, int pvp, bool* isGroundTarget);
        public static Hook<UseActionDelegate> UseActionHook;
        private unsafe delegate byte DoActionLocationDelegate(long a1, uint actionType, uint actionId, long a4/* 0xE0000000 */, Vector3* a5, uint a6/* 0 */);
        private DoActionLocationDelegate _doActionLocationFunc;
        private IntPtr player;

        private float playerR
        {
            get
            {
                return Marshal.PtrToStructure<float>(this.player);
            }
            set
            {
                Marshal.StructureToPtr<float>(value, this.player, true);
            }
        }
        public unsafe Plugin(
             DalamudPluginInterface pluginInterface,
            CommandManager commandManager)
        {
            DalamudApi.Initialize(this, pluginInterface);
            this.CommandManager = commandManager;
            
            this.Configuration = DalamudApi.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(DalamudApi.PluginInterface);
            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(DalamudApi.PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = DalamudApi.PluginInterface.UiBuilder.LoadImage(imagePath);
            this.PluginUi = new PluginUI(this.Configuration, goatImage);
            DalamudApi.Condition.ConditionChange += Condition_ConditionChange;
            //this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            //{
            //    HelpMessage = "A useful message to display in /xlhelp"
            //});
            ZoneInfoHandler.Init(DalamudApi.DataManager);
            actionManagerInPtr = (IntPtr)ActionManager.Instance();
            UseActionHook = new Hook<UseActionDelegate>((IntPtr)ActionManager.fpUseAction, UseActionDetour);
            UseActionHook.Enable();
            DalamudApi.Framework.Update += OnFrameworkUpdate;
            _doActionLocationFunc = Marshal.GetDelegateForFunctionPointer<DoActionLocationDelegate>((IntPtr)ActionManager.fpUseActionLocation);
            DalamudApi.PluginInterface.UiBuilder.Draw += DrawUI;
            DalamudApi.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        private unsafe void OnFrameworkUpdate(Framework framework)
        {
            if (DateTime.Now.ToString()==DoSomeThing.ToString()&&Configuration.AutoChou)
            {
                重抽();
            }
        }
        public unsafe void 重抽()
        {
            var gauge = GetJobGauge<ASTGauge>();
            switch (gauge.DrawnCard)
            {
                case CardType.BALANCE:
                case CardType.BOLE:
                    if (gauge.ContainsSeal(SealType.SUN))
                    {
                        _doActionLocationFunc((long)actionManagerInPtr, 1, 3593, 0xe0000000, default, 0);
                    }
                    break;
                case CardType.ARROW:
                case CardType.EWER:
                    if (gauge.ContainsSeal(SealType.MOON))
                    {
                        _doActionLocationFunc((long)actionManagerInPtr, 1, 3593, 0xe0000000, default, 0);
                    }
                    break;
                case CardType.SPEAR:
                case CardType.SPIRE:
                    if (gauge.ContainsSeal(SealType.CELESTIAL))
                    {
                        _doActionLocationFunc((long)actionManagerInPtr, 1, 3593, 0xe0000000, default, 0);
                    }
                    break;
                default:
                    break;
            }
        }
        private unsafe byte UseActionDetour(ActionManager* actionManager, uint actionType, uint actionID, long targetObjectID, uint param, uint useType, int pvp, bool* isGroundTarget)
        {
            try
            {
                if(Configuration.AutoFaKa && actionID == 17055)
            {
                    var wantPlayer = DOCompute();
                    uint wantObject;
                    if (wantPlayer is null)
                    {
                        wantObject = DalamudApi.ClientState.LocalPlayer.ObjectId;
                    }
                    else
                    {
                        wantObject = wantPlayer.GameObject.ObjectId;
                    }
                    return UseActionHook.Original(actionManager, actionType, actionID, wantObject, param, 1, pvp, isGroundTarget);
                }
                if (Configuration.AutoFaKa && actionID == 3590)
                {
                    DoSomeThing = DateTime.Now.AddSeconds(0.1f);
                    return UseActionHook.Original(actionManager, actionType, actionID, targetObjectID, param, 1, pvp, isGroundTarget);
                }
            else
                {
                    return UseActionHook.Original(actionManager, actionType, actionID, targetObjectID, param, useType, pvp, isGroundTarget);
                }
            }
            catch (Exception)
            {
                return 0;

            }

            
        }

        private void Condition_ConditionChange(Dalamud.Game.ClientState.Conditions.ConditionFlag flag, bool value)
        {

        }
        public Vector3 MiddlePos()
        {
            var wantVcetor = new Vector3();
            if (DalamudApi.ClientState.LocalPlayer is null)
            {
                return default;
            }
            var 位置 = DalamudApi.ClientState.TerritoryType;
            var mapInfo = ZoneInfoHandler.GetMapInfoFromTerritoryTypeID(位置);
			for (int i = 0; i < mapInfo.Length; i++)
			{
                var abc = mapInfo[i].GetMapCoordinates(new Vector2(0.5f, 0.5f) * 2048.0f);
                wantVcetor=new Vector3(abc.X, DalamudApi.ClientState.LocalPlayer.Position.Y, abc.Y);
                var distance = Vector3.Distance(wantVcetor, DalamudApi.ClientState.LocalPlayer.Position);
				if (distance<20)
				{
                    return wantVcetor;

                }
            }
            return default;
        }
        public bool FindBuff(ushort effectID)
        {
            if (DalamudApi.ObjectTable is null)
            {
                return false;
            }

            foreach (var status in DalamudApi.ClientState.LocalPlayer.StatusList)
            {

                if (status.StatusId == effectID)
                {
                    
                    return true;
                }
            }

            return false;
        }
        public void Dispose()
        {
            DalamudApi.Dispose();
            UseActionHook.Disable();
            this.PluginUi.Dispose();
            DalamudApi.Framework.Update -= OnFrameworkUpdate;
            DalamudApi.Condition.ConditionChange -= Condition_ConditionChange;
            this.CommandManager.RemoveHandler(commandName);
        }
        [Command("/faka")]
        [HelpMessage("autoSendCard")]
        private unsafe void OnCommand(string command, string args)
        {
            string[] array = args.Split(new char[]
    {
                    ' '
    });
            var midddle = MiddlePos();
            switch (array[0])  
            {
                case "dixing":
                    this.player = DalamudApi.ClientState.LocalPlayer.Address + 176;
                    var roation = DalamudApi.ClientState.LocalPlayer.Rotation;
                    
                    var distance = Vector3.Distance(midddle,DalamudApi.ClientState.LocalPlayer.Position);
                    _doActionLocationFunc((long)actionManagerInPtr, 1, 2262, 0xe0000000, &midddle, 0);
                    break;
                case "liyi":
                   
					if (FindBuff(2709))
					{
						UseActionHook.Original(ActionManager.Instance(), 1, 25862, 0xe0000000, 0, 0, 0, (bool*)0);

					}
					else
					{
                        _doActionLocationFunc((long)actionManagerInPtr, 1, 25862, 0xe0000000, &midddle, 0);
                    }
					break;
                case "chong":
                    重抽();
                    break;
                default:
                  var wantPlayer=DOCompute();
                   
                    PluginLog.Log($"发卡给{wantPlayer.Name.ToString()}:{wantPlayer.ObjectId:X4}");
                break;
            }
        }
        //计算小队成员dps最大人员
        public PartyMember DOCompute()
        {
            var want=new List<(PartyMember, float)>();
            float wantParm;
            //foreach (var item in JobData.data)
            //{
            //    var a = (uint)item.Key;
            //    var b = DalamudApi.ClientState.LocalPlayer;
            //    if (b.ClassJob.Id == (uint)item.Key)
            //    {
            //        float wantParm1=0f;
            //        var hasBuff = FindBuff((ushort)item.Value.BuffID, b);
            //        if (hasBuff)
            //        {
            //            wantParm1 = item.Value.BurstCoefficient;
            //        }

            //        else
            //        {
            //            wantParm1 = item.Value.CommonCoefficient;
            //        }
            //        wantParm = item.Value.DPS * wantParm1;
            //        //want.Add((p, wantParm));
            //        PluginLog.Log($"计算过程:{b.Name}DPS为{wantParm}={item.Value.DPS}X{wantParm1}");
            //    }
            //}
            if (DalamudApi.PartyList.Length<1)
            {
                return null;
            }
            foreach (var p in DalamudApi.PartyList)
            {
                var distance = Vector3.Distance(p.Position,DalamudApi.ClientState.LocalPlayer.Position);
                if (distance<30)
                {
                    break;
                }
                foreach (var item in JobData.data)
                {

                    if (p.ClassJob.Id==(uint)item.JobClass)
                    {
                        
                        var gauge = GetJobGauge<ASTGauge>();
                        var role = ClassRole.GetRole(item.JobClass);

                        float wantParm1 = item.CommonCoefficient;
                        foreach (var data in item.BuffDic)
                        {
                            if (FindBuff((ushort)data.Key, (BattleChara)p.GameObject))
                            {
                                if (wantParm1<1)
                                {
                                    wantParm1 = 1f;
                                }
                                wantParm1 = wantParm1* data.Value;
                            }
                        }
                        //var hasBuff = FindBuff((ushort)item.BuffID, (BattleChara)p.GameObject);
                        foreach (var down in JobData.通用数据)
                        {
                            if (FindBuff((ushort)down.Key, (BattleChara)p.GameObject, false))
                            {
                                wantParm1 = wantParm1 * down.Value;
                            }
                        }
                        //if (FindBuff(0x2c, (BattleChara)p.GameObject))
                        //{
                        //    wantParm1 = wantParm1 * 0.375f;
                        //}
                        //if (FindBuff(0x2b, (BattleChara)p.GameObject))
                        //{
                        //    wantParm1 = wantParm1 * 0.25f;
                        //}
                        //else
                        //{
                        //    wantParm1 = item.CommonCoefficient;
                        //}
                        switch (gauge.DrawnCard)
                        {
                            case CardType.SPEAR:
                            case CardType.BALANCE:
                            case CardType.ARROW:
                                if (role == Role.近)
                                {
                                    wantParm1 = wantParm1 * 1.06f;
                                }
                                else
                                {
                                    wantParm1 = wantParm1 * 1.03f;
                                }
                                break;
                            case CardType.SPIRE:
                            case CardType.BOLE:
                            case CardType.EWER:
                                if (role == Role.近)
                                {
                                    wantParm1 = wantParm1 * 1.03f;
                                }
                                else
                                {
                                    wantParm1 = wantParm1 * 1.06f;
                                }
                                break;

                            default:
                                break;
                        }
                        if (p.CurrentHP <= 1)
                        {
                            wantParm1 = wantParm1*0f;
                        }
                        foreach (var buff in ZeroBuff)
                        {
                            if (FindBuff(buff, (BattleChara)p.GameObject,false))
                            {
                                wantParm1 = wantParm1 * 0f;
                            }
                        }
                        wantParm =item.DPS * wantParm1;
                        want.Add((p,wantParm));
                        PluginLog.Log($"计算过程:{p.Name}：DPS为{wantParm}={item.DPS}X{wantParm1}");
                    }
                }
            }
            var abc = want.OrderBy(i=>i.Item2).ToList();
            return abc.Last().Item1;
        }
        protected static T GetJobGauge<T>() where T : JobGaugeBase
        {
            if (!JobGaugeCache.TryGetValue(typeof(T), out var gauge))
                gauge = JobGaugeCache[typeof(T)] = DalamudApi.JobGauges.Get<T>();

            return (T)gauge;
        }
        public  bool FindBuff(ushort effectID, BattleChara p,bool judge=true)
        {
            if (DalamudApi.ObjectTable is null)
            {
                return false;
            }

            if (p is not null)
            {
                var objects = DalamudApi.ObjectTable?.Where(i => i.ObjectKind == ObjectKind.Player||i.ObjectKind==ObjectKind.BattleNpc);
                foreach (var actor1 in objects)
                {
                    var actor = (BattleChara)actor1;
                    foreach (var status in actor.StatusList)
                    {
                        if (judge)
                        {
                            if (status.StatusId == effectID && status.SourceID == p.ObjectId)
                            {
                                return true;
                            }
                        }
                        if (judge==false)
                        {
                            if (status.StatusId == effectID &&actor1.ObjectId==p.ObjectId)
                            {
                                return true;
                            }
                        }
                        
                    }
                }
            }

            return false;
        }
        private void DrawUI()
        {
            this.PluginUi.Draw();
        }

        private void DrawConfigUI()
        {
            this.PluginUi.SettingsVisible = true;
        }
    }
}
