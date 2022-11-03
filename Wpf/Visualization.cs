using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using SectionTypes = Model.Section.SectionTypes;
using Section = Model.Section;
using Controller;

namespace Wpf
{
    public static class Visualization
    {
        private static int _direction = 1; // 0 = UP, 1 = RIGHT, 2 = DOWN, 3 = LEFT
        private static readonly int _SectionSize = 100;
        private static int _positionX = 100;
        private static int _positionY = 100;
        private static Race? _currentRace;

        public static void Initialize(Race? race)
        {
            _currentRace = race;
        }

        public static BitmapSource DrawTrack(Track track)
        {
            //TODO: Calculate max track size and use this to set the canvas sizes

            Bitmap canvas = Images.EmptyBitmap(1280, 720);
            Graphics g = Graphics.FromImage(canvas);
            
            foreach (var section in track.Sections)
            {
                var sectionBitmap = Images.Load(GetSectionBitmap(section.SectionType, _direction));
                g.DrawImage(sectionBitmap, _positionX, _positionY, _SectionSize, _SectionSize);


                PlaceParticipants(section, g);

                ChangeDirection(section.SectionType);
                ChangeCursorPosition();
            }

            return Images.CreateBitmapSourceFromGdiBitmap(canvas);
        }

        private static void PlaceParticipants(Section section, Graphics g)
        {
            var lefty = _currentRace?.GetSectionData(section)?.Left;
            var righty = _currentRace?.GetSectionData(section)?.Right;

            //TODO: Fix overtaking in Controller > Race > MoveParticipants();

            if (lefty != null)
            {
                PlaceParticipant(lefty, g, TrackPosition.Left);
            }

            if (righty != null)
            {
                PlaceParticipant(righty, g, TrackPosition.Right);
            }
        }

        private static void PlaceParticipant(IParticipant? participant, Graphics g, TrackPosition side)
        {
            var (x, y) = BeepBoopParticipantPosition(side);

            var participantBitmap = Images.Load(GetParticipantBitmap(participant));
            g.DrawImage(participantBitmap, _positionX + x, _positionY + y);
            
            if (participant != null && participant.Equipment.IsBroken)
            {
                var broken = Images.Load(_broken);
                g.DrawImage(broken, _positionX + x, _positionY + y);
            }
        }

        private static (int x, int y) BeepBoopParticipantPosition(TrackPosition side)
        {
            switch (_direction)
            {
                case 0:
                    switch (side)
                    {
                        case TrackPosition.Left:
                            return (20, 20);
                        case TrackPosition.Right:
                            return (50, 20);
                    }
                    break;
                case 1:
                    switch (side)
                    {
                        case TrackPosition.Left:
                            return (20, 20);
                        case TrackPosition.Right:
                            return (20, 50);
                    }
                    break;
                case 2:
                    switch (side)
                    {
                        case TrackPosition.Left:
                            return (50, 20);
                        case TrackPosition.Right:
                            return (20, 20);
                    }
                    break;
                case 3:
                    switch (side)
                    {
                        case TrackPosition.Left:
                            return (20, 50);
                        case TrackPosition.Right:
                            return (20, 20);
                    }
                    break;
            }

            return (20, 20);
        }

        public static void ChangeDirection(SectionTypes section)
        {
            if (section == SectionTypes.RightCorner)
            {
                switch (_direction)
                {
                    case 0: case 1: case 2: _direction++; break;
                    default: _direction = 0; break;
                }
            } 
            else if (section == SectionTypes.LeftCorner)
            {
                switch (_direction)
                {
                    case 0: _direction = 3; break;
                    default: _direction--; break;
                }
            }
        }

        public static void ChangeCursorPosition()
        {
            var distance = _SectionSize;

            switch (_direction)
            {
                case 0: _positionY -= distance; break;
                case 1: _positionX += distance; break;
                case 2: _positionY += distance; break;
                case 3: _positionX -= distance; break;
            }
        }

        private static string GetParticipantBitmap(IParticipant? participant)
        {
            var ParticipantReturn = "";

            switch (participant.TeamColor)
            {
                case IParticipant.TeamColors.Red:
                    ParticipantReturn = _red;
                    break;
                case IParticipant.TeamColors.Green:
                    ParticipantReturn = _green;
                    break;
                case IParticipant.TeamColors.Yellow:
                    ParticipantReturn = _yellow;
                    break;
                case IParticipant.TeamColors.Grey:
                    ParticipantReturn = _grey;
                    break;
                case IParticipant.TeamColors.Blue:
                    ParticipantReturn = _blue;
                    break;
            }

            return ParticipantReturn;
        }

        public static string GetSectionBitmap(SectionTypes section, int direction)
        {
            var SectionReturn = "";

            switch (section)
            {
                case SectionTypes.Straight:
                    SectionReturn = direction % 2 == 0 ? _straightVer : _straightHor;
                    break;
                case SectionTypes.Finish:
                    SectionReturn = direction % 2 == 0 ? _finishVer : _finishHor;
                    break;
                case SectionTypes.StartGrid:
                    SectionReturn = _startHor;
                    break;
                case SectionTypes.RightCorner:

                    switch (direction)
                    {
                        case 0:
                            SectionReturn = _corner1;
                            break;
                        case 1:
                            SectionReturn = _corner2;
                            break;
                        case 2:
                            SectionReturn = _corner3;
                            break;
                        case 3:
                            SectionReturn = _corner0;
                            break;
                    }

                    break;
                case SectionTypes.LeftCorner:

                    switch (direction)
                    {
                        case 0:
                            SectionReturn = _corner2;
                            break;
                        case 1:
                            SectionReturn = _corner3;
                            break;
                        case 2:
                            SectionReturn = _corner0;
                            break;
                        case 3:
                            SectionReturn = _corner1;
                            break;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("GetSection Switch was default.");
                    break;
            }
            
            return SectionReturn;

        }

        #region graphics

        private const string _startHor = @".\Assets\StartHorizontal.png";
        private const string _startVer = @".\Assets\StartVertical.png";
        private const string _finishHor = @".\Assets\FinishHorizontal.png";
        private const string _finishVer = @".\Assets\FinishVertical.png";
        private const string _straightHor = @".\Assets\StraightHorizontal.png";
        private const string _straightVer = @".\Assets\StraightVertical.png";
        private const string _corner0 = @".\Assets\Corner0.png";
        private const string _corner1 = @".\Assets\Corner1.png";
        private const string _corner2 = @".\Assets\Corner2.png";
        private const string _corner3 = @".\Assets\Corner3.png";

        private const string _red = @".\Assets\Mario.png";
        private const string _green = @".\Assets\Yoshi.png";
        private const string _yellow = @".\Assets\Bowser.png";
        private const string _grey = @".\Assets\Koopa.png";
        private const string _blue = @".\Assets\Toad.png";

        private const string _broken = @".\Assets\Biem.png";

        #endregion
        public enum TeamColors
        {
            Red,
            Green,
            Yellow,
            Grey,
            Blue
        }

        public enum TrackPosition
        {
            Left,
            Right
        }
    }
}
