using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track : Section
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public int Rounds { get; set; }

        public Track(string name, SectionTypes[] sectionTypes)
        {
            Name = name;
            Sections = ArrToLinked(sectionTypes);
            Rounds = 2;
        }

        private LinkedList<Section> ArrToLinked(Section.SectionTypes[] SectionTypes)
        {
            var newlist = new LinkedList<Section>();

            foreach (var x in SectionTypes)
            {
                var section = new Section() { SectionType = x };

                newlist.AddLast(section);
            }

            // Add the first section AGAIN to fix a weird bug that I ain't gonna fix rn
            //newlist.AddLast(new Section() { SectionType = SectionTypes.First() });

            return newlist;
        }
    }
}
