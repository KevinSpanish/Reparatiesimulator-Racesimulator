using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track : Section
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sectionTypes)
        {
            Name = name;
            Sections = ArrToLinked(sectionTypes);
        }

        private LinkedList<Section> ArrToLinked(Section.SectionTypes[] SectionTypes)
        {
            var newlist = new LinkedList<Section>();

            foreach (var x in SectionTypes)
            {
                var section = new Section() { SectionType = x };

                newlist.AddLast(section);
            }

            return newlist;
        }
    }
}
