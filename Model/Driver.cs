using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public Driver(string name, int points, IEquipment equipment, IParticipant.TeamColors teamColor, int brokenCount)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
            BrokenCount = brokenCount;
        }
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public IParticipant.TeamColors TeamColor { get; set; }

        public bool Finished { get; set; }
        public int BrokenCount { get; set; }
    }
}
