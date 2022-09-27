using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {
        public IParticipant Left { set; get; }
        public int DistanceLeft { set; get; }
        public IParticipant Right { set; get; }
        public int DistanceRight { set; get; }

    }
}
