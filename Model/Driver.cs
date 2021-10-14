using System;

namespace Model
{
    public class Driver : IParticipant
    {
        public Driver(string name, int points, IEquipment equipment, IParticipant.TeamColors teamColor)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
        }

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public IParticipant.TeamColors TeamColor { get; set; }
    }
}
