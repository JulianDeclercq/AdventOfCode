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
            public readonly Point Position;
            public bool Marked = false;
        }

        public int Score = 0; // score after this card wins
        private List<Entry> _entries = new();

        public void Clear() => _entries.Clear();
        public void AddRange(IEnumerable<Entry> range) => _entries.AddRange(range);
        public static Board Copy(Board toCopy) => new() { _entries = toCopy._entries.ToList() };
        public bool Bingo() => Score != 0;

        // check all entries for given value and mark the entry with corresponding value
        public bool TryMark(int value, out Entry entry)
        {
            foreach (var e in _entries)
            {
                if (e.Value != value) 
                    continue;
                
                e.Marked = true;
                entry = e;
                
                return true;
            }

            entry = default;
            return false;
        }

        // check if this board wins
        public bool CheckBingo(Entry lastMarkedEntry)
        {
            // check all columns and all rows
            if (_entries.Where(e => e.Position.X == lastMarkedEntry.Position.X).All(e => e.Marked) ||
                _entries.Where(e => e.Position.Y == lastMarkedEntry.Position.Y).All(e => e.Marked))
            {
                // calculate the score
                Score = lastMarkedEntry.Value * _entries.Where(e => !e.Marked).Sum(e => e.Value);
                return true;
            }
            return false;
        }
    }

    private static List<Board> ParseBoards(IReadOnlyList<string> lines)
    {
        var boards = new List<Board>();
        Board board = new(); 

        // skip first 2 lines, to where the boards start
        for (var i = 2; i < lines.Count; ++i)
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
    

    public static void Part1()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");
        var boards = ParseBoards(lines);
        
        // game loop, pull the numbers
        var numbers = lines[0].Split(',').Select(int.Parse);
        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                // skip boards that have already won
                if (board.Bingo())
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
    
    public static void Part2()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");
        var boards = ParseBoards(lines);
        
        // game loop, pull the numbers
        Board? lastWon = null;
        var numbers = lines[0].Split(',').Select(int.Parse);
        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                // skip boards that have already won
                if (board.Bingo())
                    continue;
                
                // try to mark the number on the board, if successful check for bingo
                if (board.TryMark(number, out var entry) && board.CheckBingo(entry))
                    lastWon = board;
            }
        }
        Console.WriteLine($"Day 4 part 2: {lastWon!.Score}");
    }
}