using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class Day4
    {       
        public static void Run()
        {
            var lines = File.ReadLines("data/day4.txt");
            var drawnNumbersStr = lines.First();            
            var drawnNumbers = NumberList.Instance;
            var boards = new List<BingoBoard>();

            for(int i = 1; i < lines.Count(); i++)
            {
                if (lines.ElementAt(i) == "")
                    continue;
                boards.Add(new BingoBoard(lines.Skip(i).Take(5), drawnNumbers));
                i += 4;
            }

            Console.WriteLine($"Loaded {boards.Count()} boards...");


            List<BingoBoard> boardsWithoutBingo = new List<BingoBoard>();
            foreach(var nbr in drawnNumbersStr.Split(',',StringSplitOptions.RemoveEmptyEntries))
            {
             
                Console.WriteLine($"Drawing number {nbr}\n");
                drawnNumbers.AddLast(int.Parse(nbr));
                //foreach (var board in boards)
                //    Console.WriteLine(board);

                if (boards.All(b => b.HasBingo()))
                {
                    Console.WriteLine("All boards have a bingo! Calculating score of the last one");
                    var scores = boardsWithoutBingo.Select(b => b.CalculateScore()).ToList();
                    foreach (var score in scores)
                        Console.WriteLine($"Score: {score}");
                    return;

                }
                else if (boards.Any(b => b.HasBingo()))
                {
                    Console.WriteLine("We have at least one bingo! Calculating score(s)...");
                    var scores = boards.Where(b => b.HasBingo()).Select(b => b.CalculateScore()).ToList();
                    foreach (var score in scores)
                        Console.WriteLine($"Score: {score}");
                }

                boardsWithoutBingo = boards.Where(b => !b.HasBingo()).ToList();
                Console.WriteLine($"Boards with no bingo remaining: {boardsWithoutBingo.Count()}\nPress any key to draw another...");
                Console.ReadKey();

            }

        }

        
    }


    public sealed class NumberList : LinkedList<int>
    {
        
        private static NumberList instance = null;

        private NumberList()
        {
            
        }

       

        public static NumberList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NumberList();
                }
                return instance;
            }
        }
    }
    public class BingoBoard
    {
        public NumberList DrawnNumbers { get; private set; }
        readonly int boardDimension;
        readonly BingoCell[,] cells;


        public BingoBoard(IEnumerable<string> boardRows, NumberList drawnNumbers)
        {
            DrawnNumbers = drawnNumbers;
            boardDimension = boardRows.Count();
            cells = new BingoCell[boardDimension, boardDimension];

            for (int row = 0; row < boardDimension; row++)
            {
                var currentRow = boardRows.ElementAt(row).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < boardDimension; col++)
                {
                    cells[row, col] = new BingoCell(this, currentRow[col]);
                }
            }

        }

        public IEnumerable<List<BingoCell>> GetRows()
        {
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                var resultList = new List<BingoCell>();
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    resultList.Add(cells[row, col]);
                }
                yield return resultList;
            }
        }

        public IEnumerable<List<BingoCell>> GetColumns()
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                var resultList = new List<BingoCell>();
                for (int row = 0; row < cells.GetLength(0); row++)
                {
                    resultList.Add(cells[row, col]);
                }
                yield return resultList;
            }
        }

        public Boolean HasBingo()
        {
            var rowsWithBingo = GetRows()
                .Any(row => row.All(cell => cell.isDrawn()));

            var colsWithBingo = GetColumns()
                .Any(col => col.All(cell => cell.isDrawn()));

            return rowsWithBingo || colsWithBingo;
        }

        public int CalculateScore(int? lastNumberDrawn = null)
        {
            var numberDrawn = lastNumberDrawn.GetValueOrDefault(DrawnNumbers.Last());

            var unmarked = cells.Cast<BingoCell>().Where(c => !c.isDrawn()).Select(c => c.Value).Sum();

            return unmarked * numberDrawn;
        }

        public int BoardDimension => boardDimension;

        public class BingoCell
        {
            private readonly int _value;
            private readonly BingoBoard owner;

            internal BingoCell(BingoBoard owner, string value)
            {
                _value = Int32.Parse(value);
                this.owner = owner;
            }

            public int Value => _value;

            public bool isDrawn()
            {
                return owner.DrawnNumbers.Contains(_value);
            }
            public override string ToString()
            {
                if (isDrawn())
                    return $"{_value}*";
                return $"{_value}";
            }

        }

        public override string ToString()
        {
            var result = new StringBuilder();
            if (HasBingo())
                result.AppendLine(" !!! Board has bingo !!!");
            foreach(var row in GetRows())
            {
                result.AppendLine($"{row[0]} {row[1]} {row[2]} {row[3]} {row[4]}");
            }
            return result.ToString();
        }
    }
    
}
