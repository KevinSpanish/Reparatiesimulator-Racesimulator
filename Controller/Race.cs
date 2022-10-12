using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
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
            //_timer.Elapsed += BreakRandomly;

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
                participant.Equipment.Quality = _random.Next(20, 50);
                participant.Equipment.Performance = _random.Next(1, 3);
                participant.Equipment.Speed = _random.Next(5, 15);
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

        public void BreakRandomly(object source, ElapsedEventArgs e)
        {
            var r = new Random();

            if (Participants == null) return;
            var i = r.Next(1, Participants.Count);
            if (Participants.Count < i || i < 1) return;
            i -= 1;
            if (Participants[i] == null) return;
            if (Participants[i]!.Finished) return;
            if (!Participants[i]!.Equipment.IsBroken)
            {
                Participants[i]!.Equipment
                    .IsBroken = true;
            }
            else
            {
                Participants[i]!.Equipment.IsBroken = false;
                Participants[i]!.Equipment.Performance -= r.Next(0, 3);
                Participants[i]!.Equipment.Quality -= r.Next(0, 4);
            }
        }

        public void BreakRandomly(IParticipant participant)
        {
            //if (!participant.Equipment.IsBroken && _random.Next(0, 1000) < 40)
            //{
            //    if (!participant.Equipment.IsBroken)
            //    {
            //        participant.Equipment.IsBroken = true;
            //        participant.Name = "!" + participant.Name;
            //        participant.Equipment.Performance = 1;
            //    }
            //    else
            //    {
            //        participant.Equipment.IsBroken = false;
            //        participant.Name = participant.Name.Trim('!');
            //    }
            //}

            int chance = participant.Equipment.IsBroken ? 10 : 1;
            IEquipment equipment = participant.Equipment;
            int random = _random.Next(0, 100 - (equipment.Quality));
            if (random <= chance)
            {
                if (!equipment.IsBroken)
                {
                    if (equipment.Speed > 20)
                        equipment.Speed -= 10;
                    equipment.Performance = 1;
                }
                
                equipment.IsBroken = !equipment.IsBroken;
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
                participant.Finished = true;
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
                    var equipment = data.Left.Equipment;
                    BreakRandomly(data.Left);

                    if (!equipment.IsBroken && !HasFinished(data.Left))
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
                        //if (data.Left != null)
                        //{
                        //    driverChanged = true;
                        //    data.Left = null;
                        //}
                    }

                }

                if (data.Right != null)
                {
                    var equipment = data.Right.Equipment;
                    BreakRandomly(data.Right);

                    if (!equipment.IsBroken && !HasFinished(data.Right))
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
                        //if (data.Right != null) {
                        //    driverChanged = true;
                        //    data.Right = null;
                        //}
                    }

                }

                sections = sections.Previous;
            }

            return driverChanged;
        }
    }
}
