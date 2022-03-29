using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiademCalculator
{
    public struct DiademResourceInfo
    {
        public readonly uint Id;
        public readonly int Set;
        public readonly int ScripsReward;
        public readonly int PointsReward;

        public DiademResourceInfo(uint id, int set, int scripsReward, int pointsReward)
        {
            Id = id;
            Set = set;
            ScripsReward = scripsReward;
            PointsReward = pointsReward;
        }
    }
}
