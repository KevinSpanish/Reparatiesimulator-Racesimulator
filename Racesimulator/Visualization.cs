﻿using Model;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Racesimulator
{
    public class Visualization
    {
        #region graphics

        private static string[] _finishHorizontal =
            {
            "----",
            " 1|#",
            "2|# ",
            "----"
        };

        private static string[] _finishHorizontalInv =
            {
            "----",
            "#|1 ",
            " #|2",
            "----"
        };

        private static string[] _finishVertical =
            {
            "|##|",
            "|- |",
            "|1 |",
            "| 2|"
        };

        private static string[] _finishVerticalInv =
            {
            "|  |",
            "|2 |",
            "| 1|",
            "|##|"
        };

        private static string[] _startHorizontal =
            {
            "----",
            "  1|",
            " 2| ",
            "----"
        };

        private static string[] _startHorizontalInv =
            {
            "----",
            "|2  ",
            " |1 ",
            "----"
        };

        private static string[] _startVertical =
            {
            "|- |",
            "|1-|",
            "| 2|",
            "|  |"
        };

        private static string[] _startVerticalInv =
            {
            "|  |",
            "| 1|",
            "|2-|",
            "|- |"
        };

        private static string[] _straightHorizontal =
            {
            "----",
            "  1 ",
            " 2  ",
            "----"
        };

        private static string[] _straightHorizontalInv =
            {
            "----",
            "  2 ",
            " 1  ",
            "----"
        };

        private static string[] _straightVertical =
            {
            "|  |",
            "|1 |",
            "| 2|",
            "|  |"
        };

        private static string[] _straightVerticalInv =
            {
            "|  |",
            "|2 |",
            "| 1|",
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
            "  1|",
            " 2 |",
            "|  |"
        };

        private static string[] _corner3 =
            {
            "|  |",
            " 2 |",
            "  1|",
            "---|"
        };

        #endregion

        public static void Initialize()
        {
            Console.WriteLine($"Track: {Race.CurrentRace.Track.Name}");
            Console.WriteLine($"{Race.CurrentRace.Track.Rounds} rondjes");

            Race.CurrentRace.DriversChanged += OnDriversChanged;
            Race.CurrentRace.RaceEnded += OnNextRace;

            Track track = Race.CurrentRace.Track;
            DrawTrack(track);
        }

        public static void DrawTrack(Track track)
        {
            int direction = 1; // 0 = UP, 1 = RIGHT, 2 = DOWN, 3 = LEFT

            int verticalPosition = 5;    //Add small margin,
            int horizontalPosition = 10; // otherwhise everything is so stuck to the edge

            //TODO: Sections are not rotated correctly, a straight horizontal section places the participants wrongly 
            //      when the section is placed. This is probably because it uses the same section graphic as a normal horizontal piece.
            //      Can potentially be fixed by making a section graphic for every rotation (or just an inverted) and switch to this
            //      depening on the direction.

            for (int i = 0; i < track.Sections.Count; i++)
            {
                Section section = track.Sections.ElementAt(i);
                string sectionType = section.SectionType.ToString();
                SectionData sectiondata = Race.CurrentRace.GetSectionData(section);

                switch (sectionType)
                {
                    case "StartGrid":

                        for (int j = 0; j < 4; j++)
                        {
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

                        //TODO: Un-spaget this.
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

                        //TODO: Un-spaget this.
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
                        else
                        {
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

            Console.SetCursorPosition(0, 0);
        }

        public static string PlaceParticipant(string segment, SectionData sectionData)
        {

            if (segment.Contains("1") && sectionData.Left != null)
            {
                if (sectionData.Left.Equipment.IsBroken)
                {
                    segment = segment.Replace("1", "!");
                }
                else
                {
                    segment = segment.Replace("1", sectionData.Left.Name.Substring(0, 1));
                }
            }
            else
            {
                segment = segment.Replace("1", " ");
            }

            if (segment.Contains("2") && sectionData.Right != null)
            {
                if (sectionData.Right.Equipment.IsBroken)
                {
                    segment = segment.Replace("2", "!");
                }
                else
                {
                    segment = segment.Replace("2", sectionData.Right.Name.Substring(0, 1));
                }
            }
            else
            {
                segment = segment.Replace("2", " ");
            }

            return segment;
        }
        public static void OnDriversChanged(object model, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }

        public static void OnNextRace(object model)
        {
            Console.Clear();

            Race.CurrentRace.CleanUp();

            Race.NextRace();

            Console.Clear();
            Initialize();
        }
    }
}
