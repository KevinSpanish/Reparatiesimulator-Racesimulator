using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Track : Section
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sectionTypes)
        {
            
        }
    }
}
