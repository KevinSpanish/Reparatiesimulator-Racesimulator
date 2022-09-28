﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;

        private Dictionary<Section, SectionData> _positions;

        private Timer _timer;

        public event EventHandler DriversChanged;
        public event EventHandler NextRace;

        private int _roundsAmount = 2;
        private Dictionary<IParticipant, int> _rounds;
        private int _finished;

        public Race(Track track, List<IParticipant> participants, int roundsAmount)
        {
            Track = track;
            Participants = participants;

            _positions = new Dictionary<Section, SectionData>();
            _random = new Random(DateTime.Now.Millisecond);
            _roundsAmount = roundsAmount;
            _rounds = new Dictionary<IParticipant, int>(); // ['participant' => amount]

            AddParticipantsPositions(Track, Participants);

            RandomizeEquipment();

            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;

            Start();
        }

        public SectionData GetSectionData(Section section)
        {

            if (!_positions.ContainsKey(section))
            {
                SectionData value = new SectionData();
                _positions.Add(section, value);
            }

            return _positions[section];
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
                //participant.Equipment.Speed = _random.Next(5, 15);
            }
        }

        public void AddParticipantsPositions(Track track, List<IParticipant> participants)
        {
            int counter = 0;
            int numParticipants = participants.Count;

            Stack<Section> startGrids = new Stack<Section>();

            foreach (Section section in Track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid || section.SectionType == SectionTypes.Finish)
                {
                    startGrids.Push(section);
                }
            }

            while (startGrids.Count != 0)
            {
                Section section = startGrids.Pop();

                if (counter + 2 <= numParticipants)
                {
                    // Place two participants on a section.
                    GetSectionData(section).Left = participants[counter];
                    GetSectionData(section).Right = participants[counter + 1];
                    counter += 2;
                }
                else if (counter + 1 <= numParticipants)
                {
                    // Place one participant on a section.
                    GetSectionData(section).Left = participants[counter];
                    counter++;
                }
            }
        }

        public void Start()
        {
            _timer.Start();
            StartTime = DateTime.Now;
        }

        public void CleanUp()
        {
            DriversChanged = null;
            Console.Clear();
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (_finished == Participants.Count)
            {
                _timer.Enabled = false;

                CleanUp();

                NextRace?.Invoke(this, new EventArgs());

            } else
            {
                var driverChanged = MoveParticipants();
                if (driverChanged)
                {
                    _timer.Enabled = false;
                    DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
                    _timer.Enabled = true;
                }
            }
 
        }

        public void IncreaseParticipantRounds(IParticipant participant)
        {
            if (!_rounds.ContainsKey(participant))
            {
                _rounds.Add(participant, -1);
            }

            var rounds = _rounds[participant];
            rounds++;
            _rounds[participant] = rounds;

            if (rounds >= _roundsAmount)
            {
                _finished++;
            }

        }

        public bool HasFinished(IParticipant participant)
        {
            if (!_rounds.ContainsKey(participant))
            {
                _rounds.Add(participant, -1);
            }

            var rounds = _rounds[participant];
            if (rounds >= _roundsAmount)
            {
                return true;
            }

            return false;
        }


        public void MoveParticipantsDepr()
        {
            SectionData nextSection = _positions.First().Value;

            foreach (var section in _positions.Reverse())
            {
                SectionData currentSection = section.Value;

                if (currentSection.Left != null)
                {
                    nextSection.Left = currentSection.Left;
                    currentSection.Left = null;
                }

                if (currentSection.Right != null)
                {
                    nextSection.Right = currentSection.Right;
                    currentSection.Right = null;
                }

                nextSection = currentSection;
            }


        }

        public bool MoveParticipants()
        {
            bool driverChanged = false;
            var sections = Track.Sections.Last;

            while (sections != null)
            {
                SectionData data = GetSectionData(sections.Value);
                if (data.Left != null)
                {

                    if (!HasFinished(data.Left))
                    {
                        driverChanged = true;
                        SectionData nextData = GetSectionData((sections.Next ?? sections.List.First).Value);

                        if (Section.Length > data.DistanceLeft)
                        {
                            data.DistanceLeft += data.Left.Equipment.Speed * data.Left.Equipment.Performance;
                        }

                        if (nextData.Left == null)
                        {
                            nextData.Left = data.Left;
                            nextData.DistanceLeft = data.DistanceLeft - Section.Length;
                            data.Left = null;

                            if (sections.Value.SectionType == SectionTypes.Finish)
                            {
                                IncreaseParticipantRounds(nextData.Left);
                            }

                            if (HasFinished(nextData.Left))
                            {
                                nextData.Left = null;
                            }
                        }

                        //else if (nextData.Right == null)
                        //{
                        //    nextData.Right = data.Left;
                        //    data.Left = null;
                        //}
                        //else
                        //{
                        //    data.DistanceLeft = 50;
                        //}
                    } else
                    {
                        if (data.Left != null)
                        {
                            driverChanged = true;
                            data.Left = null;
                        }
                    }

                }

                if (data.Right != null)
                {
                    if (!HasFinished(data.Right))
                    {
                        driverChanged = true;
                        SectionData nextData = GetSectionData((sections.Next ?? sections.List.First).Value);

                        if (Section.Length > data.DistanceRight)
                        {
                            data.DistanceRight += data.Right.Equipment.Speed * data.Right.Equipment.Performance;
                        }

                        if (nextData.Right == null)
                        {
                            nextData.Right = data.Right;
                            nextData.DistanceRight = data.DistanceRight - Section.Length;
                            data.Right = null;

                            if (sections.Value.SectionType == SectionTypes.Finish)
                            {
                                IncreaseParticipantRounds(nextData.Right);
                            }

                            if (HasFinished(nextData.Right))
                            {
                                nextData.Right = null;
                            }
                        }

                        //else if (nextData.Left == null)
                        //{
                        //    nextData.Left = data.Right;
                        //    data.Right = null;
                        //}
                        //else
                        //{
                        //    data.DistanceRight = 50;
                        //}
                    }
                    else
                    {
                        if (data.Right != null) {
                            driverChanged = true;
                            data.Right = null;
                        }
                    }

                }

                sections = sections.Previous;
            }

            return driverChanged;
        }
    }
}
