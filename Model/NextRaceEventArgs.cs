namespace Model
{
    public class NextRaceEventArgs : EventArgs
    {
        public Track Track { get; set; }
        public NextRaceEventArgs(Track track)
        {
            Track = track;
        }
    }
}