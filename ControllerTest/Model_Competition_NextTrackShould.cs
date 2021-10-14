using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Model.Section;

namespace ControllerTest
{
    [TestFixture]
    class Model_Competition_NextTrackShould
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
            var track = new Track("Test", new SectionTypes[] {});
            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("Test track", new SectionTypes[] {});
            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track1 = new Track("Test track 1", new SectionTypes[] {});
            Track track2 = new Track("Test track 2", new SectionTypes[] {});

            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);

            Assert.AreEqual(_competition.Tracks.Dequeue(), track1);
        }
    }
}
