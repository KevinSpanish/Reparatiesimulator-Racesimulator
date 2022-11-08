using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.Section;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Track_GetNextSectionShould
    {
        private Track _track;
        [SetUp]
        public void SetUp()
        {
            _track = new Track("UnitTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish });
        }

    }
}
