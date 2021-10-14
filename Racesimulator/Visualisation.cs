using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Racesimulator
{
    public class Visualisation
    {
        #region graphics

        private static string[] _finishHorizontal =
            {
            "----",
            " 1|#",
            "2 |#",
            "----"
        };

        private static string[] _finishVertical =
            {
            "|##|",
            "|--|",
            "|1 |",
            "| 2|"
        };

        private static string[] _startHorizontal =
            {
            "----",
            "  1|",
            " 2| ",
            "----"
        };

        private static string[] _startVertical =
            {
            "|- |",
            "|1-|",
            "| 2|",
            "|  |"
        };

        private static string[] _straightHorizontal =
            {
            "----",
            "  1 ",
            " 2  ",
            "----"
        };

        private static string[] _straightVertical =
            {
            "|  |",
            "|1 |",
            "| 2|",
            "|  |"
        };

        private static string[] _corner0 =
            {
            "|  |",
            "|1  ",
            "| 2 ",
            "----"
        };

        private static string[] _corner1 =
            {
            "----",
            "|1  ",
            "| 2 ",
            "|  |"
        };

        private static string[] _corner2 =
            {
            "----",
            " 1 |",
            "  2|",
            "|  |"
        };

        private static string[] _corner3 =
            {
            "|  |",
            " 1 |",
            "  2|",
            "---|"
        };

        #endregion

        public static void DrawTrack(Track track)
        {
            int direction = 1; // 0 = UP, 1 = RIGHT, 2 = DOWN, 3 = LEFT

            int verticalPosition = 4;    //Add small margin,
            int horizontalPosition = 10; // otherwhise everything is so stuck to the edge

            for (int i = 0; i < track.Sections.Count; i++)
            {
                Section section = track.Sections.ElementAt(i);
                string sectionType = section.SectionType.ToString();
                SectionData sectiondata = Data.CurrentRace.GetSectionData(section);

                switch (sectionType)
                {
                    case "StartGrid":

                        for (int j = 0; j < 4; j++) {
                            Console.SetCursorPosition(horizontalPosition, verticalPosition + j);

                            if (direction % 2 == 0)
                            {
                                Console.Write(PlaceParticipant(_startVertical[j], sectiondata));
                            }
                            else
                            {
                                Console.Write(PlaceParticipant(_startHorizontal[j], sectiondata));
                            }
                        }

                        horizontalPosition += 4;

                        break;
                    case "Finish":

                        for (int j = 0; j < 4; j++)
                        {
                            Console.SetCursorPosition(horizontalPosition, verticalPosition + j);

                            if (direction % 2 == 0)
                            {
                                Console.Write(PlaceParticipant(_finishVertical[j], sectiondata));
                            }
                            else
                            {
                                Console.Write(PlaceParticipant(_finishHorizontal[j], sectiondata));
                            }
                        }

                        horizontalPosition += 4;

                        break;
                    case "RightCorner":

                        for (int j = 0; j < 4; j++)
                        {
                            Console.SetCursorPosition(horizontalPosition, verticalPosition + j);

                            switch (direction)
                            {
                                case 0:
                                    Console.Write(PlaceParticipant(_corner1[j], sectiondata));
                                    break;
                                case 1:
                                    Console.Write(PlaceParticipant(_corner2[j], sectiondata));

                                    break;
                                case 2:
                                    Console.Write(PlaceParticipant(_corner3[j], sectiondata));

                                    break;
                                case 3:
                                    Console.Write(PlaceParticipant(_corner0[j], sectiondata));
                                    break;
                            }
                        }

                        if (direction == 0) { direction++; horizontalPosition += 4; }
                        else if (direction == 1) { direction++; verticalPosition += 4; }
                        else if (direction == 2) { direction++; horizontalPosition -= 4; }
                        else { direction = 0; verticalPosition -= 4; }

                        break;
                    case "LeftCorner":

                        for (int j = 0; j < 4; j++)
                        {
                            Console.SetCursorPosition(horizontalPosition, verticalPosition + j);

                            switch (direction)
                            {
                                case 0:
                                    Console.Write(PlaceParticipant(_corner2[j], sectiondata));

                                    break;
                                case 1:
                                    Console.Write(PlaceParticipant(_corner3[j], sectiondata));
                                    break;
                                case 2:
                                    Console.Write(PlaceParticipant(_corner0[j], sectiondata));
                                    break;
                                case 3:
                                    Console.Write(PlaceParticipant(_corner1[j], sectiondata));
                                    break;
                            }
                        }

                        if (direction == 0) { direction = 3; horizontalPosition -= 4; }
                        else if (direction == 1) { direction--; verticalPosition -= 4; }
                        else if (direction == 2) { direction--; horizontalPosition += 4; }
                        else { direction--; verticalPosition += 4; }

                        break;
                    default: //Straight 

                        if (direction % 2 != 0)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                Console.SetCursorPosition(horizontalPosition, verticalPosition + j);
                                Console.Write(PlaceParticipant(_straightHorizontal[j], sectiondata));
                            }

                            if (direction == 1) { horizontalPosition += 4; }
                            else { horizontalPosition -= 4; }

                        }
                        else { 
                            for (int j = 0; j < 4; j++)
                            {
                                Console.SetCursorPosition(horizontalPosition, verticalPosition + j);
                                Console.Write(PlaceParticipant(_straightVertical[j], sectiondata));
                            }


                            if (direction == 2) { verticalPosition += 4; }
                            else { verticalPosition -= 4; }
                        }

                        break;
                }
            }
        }

        private static string PlaceParticipant(String segment, SectionData sectiondata)
        {
            if (segment.Contains("1") && sectiondata.Left != null)
            {
                segment = segment.Replace("1", sectiondata.Left.Name.Substring(0, 1));
            }
            else
            {
                segment = segment.Replace("1", " ");
            }

            if (segment.Contains("2") && sectiondata.Right != null)
            {
                segment = segment.Replace("2", sectiondata.Right.Name.Substring(0, 1));
            }
            else
            {
                segment = segment.Replace("2", " ");
            }

            return segment;
        }
    }
}
