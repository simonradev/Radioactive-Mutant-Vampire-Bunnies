namespace BunniesSecondTry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class VampireBunniesEntryPoint
    {
        public static Player player;
        public static List<Point> currBunnies;
        public static List<Point> newBunnies;
        public static char[,] matrix;

        public static void Main()
        {
            int[] matrixInfo = Console.ReadLine()
                                .Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

            int rows = matrixInfo[0];
            int cols = matrixInfo[1];
            
            matrix = new char[rows, cols];
            currBunnies = new List<Point>();
            player = null;
            for (int currRow = 0; currRow < rows; currRow++)
            {
                string fieldLayer = Console.ReadLine();

                for (int currCell = 0; currCell < fieldLayer.Length; currCell++)
                {
                    char symbol = fieldLayer[currCell];

                    matrix[currRow, currCell] = symbol;

                    if (symbol == 'P')
                    {
                        player = new Player(new Point(currRow, currCell));
                    }
                    else if (symbol == 'B')
                    {
                        currBunnies.Add(new Point(currRow, currCell));
                    }
                }
            }

            string moves = Console.ReadLine();
            for (int currSymbol = 0; currSymbol < moves.Length; currSymbol++)
            {
                char move = moves[currSymbol];

                switch (move)
                {
                    case 'L':
                        MoveThePlayerToTheLeftOrRightAndCheckTheConditions(-1);
                        break;

                    case 'R':
                        MoveThePlayerToTheLeftOrRightAndCheckTheConditions(+1);
                        break;

                    case 'U':
                        MoveThePlayerUpOrDownAndCheckTheConditions(-1);
                        break;

                    case 'D':
                        MoveThePlayerUpOrDownAndCheckTheConditions(+1);
                        break;

                    default:
                        break;
                }

                SpreadTheBunniesAndCheckTheConditions();

                if (player.IsEaten || player.HasWon)
                {
                    break;
                }
            }

            PrintMatrix();
        }

        private static void SpreadTheBunniesAndCheckTheConditions()
        {
            newBunnies = new List<Point>();
            foreach (Point bunny in currBunnies)
            {
                SpreadTheBunnyToTheLeftAndRightAndCheckTheConditions(bunny, -1);
                SpreadTheBunnyToTheLeftAndRightAndCheckTheConditions(bunny, +1);
                SpreadTheBunnyUpAndDownAndCheckTheConditions(bunny, -1);
                SpreadTheBunnyUpAndDownAndCheckTheConditions(bunny, +1);
            }

            currBunnies = newBunnies.ToList();
        }

        private static void SpreadTheBunnyUpAndDownAndCheckTheConditions(Point bunny, int newRow)
        {
            Point newBunny = new Point(bunny.Row + newRow, bunny.Col);

            PlaceTheBunnyAndCheckTheConditions(newBunny);
        }

        private static void SpreadTheBunnyToTheLeftAndRightAndCheckTheConditions(Point bunny, int newCol)
        {
            Point newBunny = new Point(bunny.Row, bunny.Col + newCol);

            PlaceTheBunnyAndCheckTheConditions(newBunny);
        }

        private static void PlaceTheBunnyAndCheckTheConditions(Point newBunny)
        {
            if (!Point.PointIsValid(matrix, newBunny))
            {
                return;
            }
            else if (matrix[newBunny.Row, newBunny.Col] == '.')
            {
                matrix[newBunny.Row, newBunny.Col] = 'B';

                newBunnies.Add(newBunny);
            }
            else if (matrix[newBunny.Row, newBunny.Col] == 'P')
            {
                player.IsEaten = true;

                matrix[newBunny.Row, newBunny.Col] = 'B';

                newBunnies.Add(newBunny);
            }
        }

        private static void MoveThePlayerUpOrDownAndCheckTheConditions(int newRow)
        {
            Point newPlace = new Point(player.Place.Row + newRow, player.Place.Col);

            ReplaceThePlayerAndCheckTheConditions(newPlace);
        }

        private static void MoveThePlayerToTheLeftOrRightAndCheckTheConditions(int newCol)
        {
            Point newPlace = new Point(player.Place.Row, player.Place.Col + newCol);

            ReplaceThePlayerAndCheckTheConditions(newPlace);
        }

        private static void ReplaceThePlayerAndCheckTheConditions(Point newPlace)
        {
            matrix[player.Place.Row, player.Place.Col] = '.';

            if (!Point.PointIsValid(matrix, newPlace))
            {
                player.HasWon = true;
            }
            else if (matrix[newPlace.Row, newPlace.Col] == 'B')
            {
                player.IsEaten = true;

                player.Place = newPlace.ToPoint();
            }
            else if (matrix[newPlace.Row, newPlace.Col] == '.')
            {
                matrix[newPlace.Row, newPlace.Col] = 'P';

                player.Place = newPlace.ToPoint();
            }
        }

        public static void PrintMatrix()
        {
            StringBuilder result = new StringBuilder();
            for (int currRow = 0; currRow < matrix.GetLength(0); currRow++)
            {
                for (int currCol = 0; currCol < matrix.GetLength(1); currCol++)
                {
                    result.Append(matrix[currRow, currCol]);
                }

                result.Append("\r\n");
            }

            string playerStatus = player.HasWon ? "won" : "dead";
            result.Append($"{playerStatus}: {player.Place.Row} {player.Place.Col}");
            Console.WriteLine(result.ToString());
        }
    }
}
