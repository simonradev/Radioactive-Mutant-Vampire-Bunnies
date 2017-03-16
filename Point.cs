namespace BunniesSecondTry
{
    public class Point
    {
        public int Row { get; set; }

        public int Col { get; set; }

        public Point(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public static bool PointIsValid(char[,] matrix, Point point)
        {
            bool isValid = true;

            if (point.Row < 0 ||
                point.Row >= matrix.GetLength(0) ||
                point.Col < 0 ||
                point.Col >= matrix.GetLength(1))
            {
                isValid = false;
            }

            return isValid;
        }

        public Point ToPoint()
        {
            return new Point(this.Row, this.Col);
        }
    }
}
