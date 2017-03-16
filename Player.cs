namespace BunniesSecondTry
{
    public class Player
    {
        public bool IsEaten { get; set; }

        public bool HasWon { get; set; }

        public Point Place { get; set; }

        public Player(Point place)
        {
            this.Place = place;
        }
    }
}
