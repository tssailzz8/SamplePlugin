using Dalamud.Game.ClientState.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin
{
    public class JobData
    {
        public static readonly Dictionary<int, float> 通用数据 = new Dictionary<int, float>()
        {
            { 0x2c,0.375f},//黑头
            { 0x2b,0.25f},
            {62,0.5f },//伤害降低
            {215,0.5f },
            {696,0.5f },
            {1090,0.5f },
            {2092,0.5f },
            {2404,0.5f },
            {2522,0.5f },
            {2911,0.5f },
        };
        public static readonly Dictionary<int, float> 骑士数据 = new Dictionary<int, float>()
        {
            { 1368,1.153717f}//安魂
        };
        public static readonly Dictionary<int, float> 武僧数据 = new Dictionary<int, float>()
        {
            { 0x49d,1.407104f}//红莲
        };
        public static readonly Dictionary<int, float> 战士数据 = new Dictionary<int, float>()
        {
            { 1177,2.026167239f}//狂魂
        };
        public static readonly Dictionary<int, float> 龙骑数据 = new Dictionary<int, float>()
        {
            { 1864,1.415f}
        };
        public static readonly Dictionary<int, float> 诗人数据 = new Dictionary<int, float>()
        {
            { 125,1.26f}
        };
        public static readonly Dictionary<int, float> 白魔数据 = new Dictionary<int, float>()
        {
            { 157,1.117f}//神速
        };
        public static readonly Dictionary<int, float> 黑魔数据 = new Dictionary<int, float>()
        {
            { 738,1.2f}//黑魔纹
        };
        public static readonly Dictionary<int, float> 召唤数据 = new Dictionary<int, float>()
        {
            { 1868,1.23f}//凤凰
        };
        public static readonly Dictionary<int, float> 机工数据 = new Dictionary<int, float>()
        {
            { 0x35d,1.468f}//野火
        };
        public static readonly Dictionary<int, float> 忍者数据 = new Dictionary<int, float>()
        {
            { 0x27e,1.8f} //背刺
        };
        public static readonly Dictionary<int, float> 黑骑数据 = new Dictionary<int, float>()
        {
            { 0x7b4,1.89f}//血乱
           
        };
        public static readonly Dictionary<int, float> 武士数据 = new Dictionary<int, float>()
        {
            { 1233,1.506f}//明镜

        };
        public static readonly Dictionary<int, float> 赤魔数据 = new Dictionary<int, float>()
        {
            { 1971,1.3725f}//无情

        };
        public static readonly Dictionary<int, float> 枪刃数据 = new Dictionary<int, float>()
        {
            { 1971,1.3725f}

        };
        public static readonly Dictionary<int, float> 舞者数据 = new Dictionary<int, float>()
        {
            { 1825,1.637f}//进攻之探戈

        };
        public static readonly Dictionary<int, float> 钐镰客数据 = new Dictionary<int, float>()
        {
            { 2599,1.2f}

        };
        public static readonly Dictionary<int, float> 奶妈数据 = new Dictionary<int, float>()
        {
            { 1,1}

        };
        public static readonly List<JobObject> data = new List<JobObject>()
        {
            { new JobObject(8559,黑魔数据,0.98f,Class.黑魔) },
            { new JobObject(8539,武士数据,0.898f,Class.武士) },
            { new JobObject(8209,武僧数据,0.883684487f, Class.武僧) },
            { new JobObject(8135,钐镰客数据,0.933f,Class.钐镰客) },
            { new JobObject(8081,龙骑数据,0.88f,Class.龙骑) },
            { new JobObject(7862,忍者数据,0.71f,Class.忍者) },
            { new JobObject(7761,机工数据,0.933f,Class.机工) },
            { new JobObject(7697,召唤数据,0.94f,Class.召唤) },
            { new JobObject(7628,赤魔数据,0.9411f,Class.赤魔) },
            { new JobObject(6866,诗人数据,0.913f,Class.诗人) },
            { new JobObject(6669,舞者数据,0.8725f,Class.舞者) },
            { new JobObject(5604,黑骑数据,0.85f, Class.黑骑) },
            { new JobObject(5561,枪刃数据,0.824f,Class.枪刃) },
            { new JobObject(5363,战士数据,0.842128117f,Class.战士) },
            { new JobObject(5361,骑士数据,0.96167f,Class.骑士) },
            { new JobObject(4764,奶妈数据,1,Class.贤者) },
            { new JobObject(4552,白魔数据,0.98f,Class.白魔) },
            { new JobObject(3839,奶妈数据,1,Class.占星) },
            { new JobObject(4441,奶妈数据,1,Class.学者) },
        };
         
    }
}
