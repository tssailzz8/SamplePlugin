using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin
{
    using static Class;
    public enum Class : byte
    {
        None = 0,
        剑术师 = 1,
        格斗家 = 2,
        斧术师 = 3,
        枪术师 = 4,
        弓箭手 = 5,
        幻术师 = 6,
        咒术师 = 7,
        刻木匠 = 8,
        锻铁匠 = 9,
        铸甲匠 = 10,
        雕金匠 = 11,
        制革匠 = 12,
        裁衣匠 = 13,
        炼金术士 = 14,
        烹调师 = 15,
        采矿工 = 16,
        园艺工 = 17,
        捕鱼人 = 18,
        骑士 = 19,
        武僧 = 20,
        战士 = 21,
        龙骑 = 22,
        诗人 = 23,
        白魔 = 24,
        黑魔 = 25,
        秘术师 = 26,
        召唤 = 27,
        学者 = 28,
        双剑师 = 29,
        忍者 = 30,
        机工 = 31,
        黑骑 = 32,
        占星 = 33,
        武士 = 34,
        赤魔 = 35,
        青魔 = 36,
        枪刃 = 37,
        舞者 = 38,
        钐镰客 = 39,
        贤者 = 40,
    }

    public enum Role
    {
        None = 0,
        近 = 1,
        远 = 2,

    }

    public static class ClassRole
    {
        public static Role GetRole(this Class cls)
        {
            return cls switch {
                剑术师 or 骑士 or 斧术师 or 战士 or 黑骑 or 枪刃 or 枪术师 or 龙骑 or 格斗家 or 武僧 or 双剑师 or 忍者 or 武士 or 钐镰客 => Role.近,
                弓箭手 or 诗人 or 机工 or 舞者 or 咒术师 or 黑魔 or 秘术师 or 召唤 or 赤魔 or 青魔 or 学者 or 幻术师 or 白魔 or 占星 or 贤者 => Role.远,
                _ => Role.None
            };
        }
    }
}
