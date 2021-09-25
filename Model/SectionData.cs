using System;
using System.Collections.Generic;
using System.Text;

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
