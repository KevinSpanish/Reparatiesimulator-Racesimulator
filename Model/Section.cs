using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Section
    {
        public SectionTypes SectionType { get; set; }

        public enum SectionTypes
        {
            Straight,
            LeftCorner,
            RightCorner,
            StartGrid,
            Finish,
        }
    }
}
