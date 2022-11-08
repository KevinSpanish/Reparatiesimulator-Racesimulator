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
        private static int _nextDirection = _direction; 
        private static bool _isCorner = false;
        private static readonly int _SectionSize = 100;
        private static int _positionX = 100;
        private static int _positionY = 0;
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
                    switch (_direction)
                    {
                        case 0: // UP
                            ParticipantReturn = _red0;
                            break;
                        case 1: // RIGHT
                            ParticipantReturn = _red1;
                            break;
                        case 2: // DOWN
                            ParticipantReturn = _red2;
                            break;
                        case 3: // LEFT
                            ParticipantReturn = _red3;
                            break;
                    }
                    break;
                case IParticipant.TeamColors.Green:
                    switch (_direction)
                    {
                        case 0: // UP
                            ParticipantReturn = _green0;
                            break;
                        case 1: // RIGHT
                            ParticipantReturn = _green1;
                            break;
                        case 2: // DOWN
                            ParticipantReturn = _green2;
                            break;
                        case 3: // LEFT
                            ParticipantReturn = _green3;
                            break;
                    }
                    break;
                case IParticipant.TeamColors.Yellow:
                    switch (_direction)
                    {
                        case 0: // UP
                            ParticipantReturn = _yellow0;
                            break;
                        case 1: // RIGHT
                            ParticipantReturn = _yellow1;
                            break;
                        case 2: // DOWN
                            ParticipantReturn = _yellow2;
                            break;
                        case 3: // LEFT
                            ParticipantReturn = _yellow3;
                            break;
                    }
                    break;
                case IParticipant.TeamColors.Grey:
                    switch (_direction)
                    {
                        case 0: // UP
                            ParticipantReturn = _grey0;
                            break;
                        case 1: // RIGHT
                            ParticipantReturn = _grey1;
                            break;
                        case 2: // DOWN
                            ParticipantReturn = _grey2;
                            break;
                        case 3: // LEFT
                            ParticipantReturn = _grey3;
                            break;
                    }
                    break;
                case IParticipant.TeamColors.Blue:
                    switch (_direction)
                    {
                        case 0: // UP
                            ParticipantReturn = _blue0;
                            break;
                        case 1: // RIGHT
                            ParticipantReturn = _blue1;
                            break;
                        case 2: // DOWN
                            ParticipantReturn = _blue2;
                            break;
                        case 3: // LEFT
                            ParticipantReturn = _blue3;
                            break;
                    }
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

        private const string _startHor = @".\Assets\Track\StartHorizontal.png";
        private const string _startVer = @".\Assets\Track\StartVertical.png";
        private const string _finishHor = @".\Assets\Track\FinishHorizontal.png";
        private const string _finishVer = @".\Assets\Track\FinishVertical.png";
        private const string _straightHor = @".\Assets\Track\StraightHorizontal.png";
        private const string _straightVer = @".\Assets\Track\StraightVertical.png";
        private const string _corner0 = @".\Assets\Track\Corner0.png";
        private const string _corner1 = @".\Assets\Track\Corner1.png";
        private const string _corner2 = @".\Assets\Track\Corner2.png";
        private const string _corner3 = @".\Assets\Track\Corner3.png";

        private const string _red0 = @".\Assets\Drivers\Red-0.png";
        private const string _red0_turn = @".\Assets\Drivers\Red-0-Corner.png";
        private const string _red1 = @".\Assets\Drivers\Red-1.png";
        private const string _red1_turn = @".\Assets\Drivers\Red-1-Corner.png";
        private const string _red2 = @".\Assets\Drivers\Red-2.png";
        private const string _red2_turn = @".\Assets\Drivers\Red-2-Corner.png";
        private const string _red3 = @".\Assets\Drivers\Red-3.png";
        private const string _red3_turn = @".\Assets\Drivers\Red-3-Corner.png";

        private const string _green0 = @".\Assets\Drivers\Green-0.png";
        private const string _green0_turn = @".\Assets\Drivers\Green-0-Corner.png";
        private const string _green1 = @".\Assets\Drivers\Green-1.png";
        private const string _green1_turn = @".\Assets\Drivers\Green-1-Corner.png";
        private const string _green2 = @".\Assets\Drivers\Green-2.png";
        private const string _green2_turn = @".\Assets\Drivers\Green-2-Corner.png";
        private const string _green3 = @".\Assets\Drivers\Green-3.png";
        private const string _green3_turn = @".\Assets\Drivers\Green-3-Corner.png";

        private const string _yellow0 = @".\Assets\Drivers\Yellow-0.png";
        private const string _yellow0_turn = @".\Assets\Drivers\Yellow-0-Corner.png";
        private const string _yellow1 = @".\Assets\Drivers\Yellow-1.png";
        private const string _yellow1_turn = @".\Assets\Drivers\Yellow-1-Corner.png";
        private const string _yellow2 = @".\Assets\Drivers\Yellow-2.png";
        private const string _yellow2_turn = @".\Assets\Drivers\Yellow-2-Corner.png";
        private const string _yellow3 = @".\Assets\Drivers\Yellow-3.png";
        private const string _yellow3_turn = @".\Assets\Drivers\Yellow-3-Corner.png";

        private const string _grey0 = @".\Assets\Drivers\Grey-0.png";
        private const string _grey0_turn = @".\Assets\Drivers\Grey-0-Corner.png";
        private const string _grey1 = @".\Assets\Drivers\Grey-1.png";
        private const string _grey1_turn = @".\Assets\Drivers\Grey-1-Corner.png";
        private const string _grey2 = @".\Assets\Drivers\Grey-2.png";
        private const string _grey2_turn = @".\Assets\Drivers\Grey-2-Corner.png";
        private const string _grey3 = @".\Assets\Drivers\Grey-3.png";
        private const string _grey3_turn = @".\Assets\Drivers\Grey-3-Corner.png";

        private const string _blue0 = @".\Assets\Drivers\Blue-0.png";
        private const string _blue0_turn = @".\Assets\Drivers\Blue-0-Corner.png";
        private const string _blue1 = @".\Assets\Drivers\Blue-1.png";
        private const string _blue1_turn = @".\Assets\Drivers\Blue-1-Corner.png";
        private const string _blue2 = @".\Assets\Drivers\Blue-2.png";
        private const string _blue2_turn = @".\Assets\Drivers\Blue-2-Corner.png";
        private const string _blue3 = @".\Assets\Drivers\Blue-3.png";
        private const string _blue3_turn = @".\Assets\Drivers\Blue-3-Corner.png";

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
