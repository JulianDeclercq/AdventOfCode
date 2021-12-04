namespace AdventOfCode2021.days;
using Board = List<Day4.BoardEntry>; 

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

    public class BoardEntry
    {
        public BoardEntry(int value, Point point)
        {
            Value = value;
            Position = point;
        }

        public int Value;
        public Point Position;
        public bool Marked = false;
    }

    private List<Board> ParseBoards(string[] lines)
    {
        var boards = new List<Board>();
        Board board = new(); 

        // skip first 2 lines
        for (var i = 2; i < lines.Length; ++i)
        {
            // add the current board to the list and start a new board if an empty line is found
            if (string.IsNullOrEmpty(lines[i]))
            {
                // copy the contents of board before clearing it
                boards.Add(board.ToList()); 
                board.Clear();
                continue;
            }
            
            // add the numbers to the board
            var row = (i - 2) % 6;
            var numbers = lines[i].Split(' ').Where(x => !string.IsNullOrEmpty(x));
            board.AddRange(numbers.Select((x, col) => new BoardEntry(int.Parse(x), new Point(col, row))));
        }
        
        // add the last board
        boards.Add(board);
        return boards;
    }
    

    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var boards = ParseBoards(lines);
        
        // game loop, pull the numbers
        var numbers = lines[0].Split(',').Select(int.Parse);
        foreach (var number in numbers)
        {
            
        }
    }
}