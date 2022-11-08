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
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var track = new Track("Test", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            Assert.That(result, Is.EqualTo(track));
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("Test track", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track1 = new Track("Track A", new SectionTypes[] { });
            Track track2 = new Track("Track B", new SectionTypes[] { });

            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);

            _competition.NextTrack();
            Track result = _competition.NextTrack();
            Assert.AreEqual(track2, result);
        }
    }
}
