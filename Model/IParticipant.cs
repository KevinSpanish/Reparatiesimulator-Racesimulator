﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        bool Finished { get; set; }

        public int BrokenCount { get; set; }

        public enum TeamColors
        {
            Red,
            Green,
            Yellow,
            Grey,
            Blue
        }
    }
}
