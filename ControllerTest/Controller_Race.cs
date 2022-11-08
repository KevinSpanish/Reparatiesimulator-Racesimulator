using Controller;
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
    public class Controller_Race
    {
        private Race testRace;

        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            testRace = new Race(Data.CurrentRace.Track, Data.CurrentRace.Participants, 2);
        }

        [Test]
        public void GetSectionData_ReturnsData()
        {
            Section section = Data.CurrentRace.Track.Sections.First.Value;
            SectionData sectionData = testRace.GetSectionData(section);
            Assert.NotNull(sectionData);
        }

        [Test]
        public void InitializeStartPositions_ParticipantsOnStartGrid()
        {
            int count = 0;
            foreach (Section section in testRace.Track.Sections)
            {
                SectionData sectionData = testRace.GetSectionData(section);

                if (section.SectionType == SectionTypes.StartGrid && sectionData.Right != null)
                {
                    count++;
                }
                if (section.SectionType == SectionTypes.StartGrid && sectionData.Left != null)
                {
                    count++;
                }
            }

            Assert.AreEqual(count, testRace.Participants.Count);
        }
    }
}
