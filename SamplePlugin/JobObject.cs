using Dalamud.Game.ClientState.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin
{
    public struct JobObject
    {
        public float CommonCoefficient;
        public int DPS;
        public Class JobClass;
        public Dictionary<int, float> BuffDic;
        public JobObject (int dps, Dictionary<int, float> buffDic, float coefficient1,Class _class)
        {
            CommonCoefficient = coefficient1;
            DPS = dps;
            BuffDic = buffDic;
            JobClass = _class;
        }
    }
}
