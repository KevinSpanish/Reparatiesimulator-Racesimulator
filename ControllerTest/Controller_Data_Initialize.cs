using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    class Controller_Data_Initialize
    {
        [Test]
        public void Initialize_Contains_Something()
        {
            Data.Initialize();
            Assert.That(Data.Competition.Participants.Count, Is.GreaterThan(0));
            Assert.That(Data.Competition.Tracks.Count, Is.GreaterThan(0));
        }
    }
}
