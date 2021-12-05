namespace AdventOfCode2021.days;

public class Day4
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X;
        public int Y;
        
        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }

    public class Board
    {
        public class Entry
        {
            public Entry(int value, Point position)
            {
                Value = value;
                Position = position;
            }

            public readonly int Value;
            public Point Position;
            public bool Marked = false;
        }

        public int Index = -1; // for debugging purposes
        public int Score = 0; // score after this card wins
        public List<Entry> Entries = new();

        public void Clear() => Entries.Clear();
        public void AddRange(IEnumerable<Entry> range) => Entries.AddRange(range);
        public static Board Copy(Board toCopy) => new() { Entries = toCopy.Entries.ToList() };
        public bool HasWon() => Score != 0;

        // check all entries for given value and mark the entry with corresponding value
        public bool TryMark(int value, out Entry entry)
        {
            foreach (var e in Entries)
            {
                if (e.Value != value) 
                    continue;
                
                e.Marked = true;
                entry = e;
                
                Console.WriteLine($"Board {Index}: Marked entry on {e.Position} ({e.Value})");
                return true;
            }

            entry = default;
            return false;
        }

        // check if this board wins
        public bool CheckBingo(Entry lastMarkedEntry)
        {
            // check all columns
            if (Entries.Where(e => e.Position.X == lastMarkedEntry.Position.X).All(e => e.Marked) ||
                Entries.Where(e => e.Position.Y == lastMarkedEntry.Position.Y).All(e => e.Marked))
            {
                // calculate the score
                Score = lastMarkedEntry.Value * Entries.Where(e => !e.Marked).Sum(e => e.Value);
                return true;
            }
            return false;
        }
    }

    private List<Board> ParseBoards(string[] lines)
    {
        var boards = new List<Board>();
        Board board = new(); 

        // skip first 2 lines, to where the boards start
        for (var i = 2; i < lines.Length; ++i)
        {
            // add the current board to the list and start a new board if an empty line is found
            if (string.IsNullOrEmpty(lines[i]))
            {
                // copy the contents of board before clearing it
                boards.Add(Board.Copy(board));
                board.Clear();
                continue;
            }
            
            // add the numbers to the board
            var row = (i - 2) % 6;
            var numbers = lines[i].Split(' ').Where(x => !string.IsNullOrEmpty(x));
            board.AddRange(numbers.Select((x, col) => new Board.Entry(int.Parse(x), new Point(col, row))));
        }
        
        // add the last board
        boards.Add(board);
        return boards;
    }
    

    public void Part1()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");
        var boards = ParseBoards(lines);
        
        // for debugging purposes
        for (var i = 0; i < boards.Count; ++i)
            boards[i].Index = i;

        foreach(var board in boards)
            Console.WriteLine($"board {board.Index}");
        
        // game loop, pull the numbers
        var numbers = lines[0].Split(',').Select(int.Parse);
        foreach (var number in numbers)
        {
            Console.WriteLine($"Pulling number {number}");
            foreach (var board in boards)
            {
                // skip boards that have already won
                if (board.HasWon())
                    continue;
                
                // try to mark the number on the board, if successful check for bingo
                if (board.TryMark(number, out var entry) && board.CheckBingo(entry))
                {
                    Console.WriteLine($"Day 4 part 1: {board.Score}");
                    return;
                }
            }
        }
    }
    
    public void Part2()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");
        var boards = ParseBoards(lines);
        
        // for debugging purposes
        for (var i = 0; i < boards.Count; ++i)
            boards[i].Index = i;

        foreach(var board in boards)
            Console.WriteLine($"board {board.Index}");
        
        // game loop, pull the numbers
        Board? lastWon = null;
        var numbers = lines[0].Split(',').Select(int.Parse);
        foreach (var number in numbers)
        {
            Console.WriteLine($"Pulling number {number}");
            foreach (var board in boards)
            {
                // skip boards that have already won
                if (board.HasWon())
                    continue;
                
                // try to mark the number on the board, if successful check for bingo
                if (board.TryMark(number, out var entry) && board.CheckBingo(entry))
                    lastWon = board;
            }
        }
        Console.WriteLine($"Day 4 part 2: {lastWon!.Score}");
    }
}