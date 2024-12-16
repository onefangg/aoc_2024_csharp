// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day16_sample.txt"));
var gridData = data.Select(x => x.ToCharArray()).ToArray();

var reindeerStartPos = FindPos(gridData, 'S');
var targetEndPos = FindPos(gridData, 'E');
var minScore = int.MaxValue;

// (row, col, row_direction, col_direction)
var currPos = (reindeerStartPos.Item1, reindeerStartPos.Item2, 1, 0);
var enqueuePath = new Queue<(int, int, int, int)>([currPos]);
var branches = new Queue<(int, int, int, int)>();

while (currPos.Item1 != targetEndPos.Item1 && currPos.Item2 == targetEndPos.Item2)
{
    var getRelativeNeighbours = GetPossibleNeighbours(currPos, gridData);
    
    
    
    if (getRelativeNeighbours.Count > 1)
    {
        branches.Enqueue();
    }
    
    
}


List<(int, int, int, int)> GetPossibleNeighbours((int, int, int, int) currNode, char[][] grid)
{
    var neighbours = new List<(int, int, int, int)>();
    var (currRow, currCol, currDr, currDc) = currNode;

    var (lc, lr) = TurnLeft(currDr, currDc);
    var (rc, rr) = TurnRight(currDr, currDc);
    // move forward ( in current direction )

    (int, int, int, int)[] goToDirections = [
        (currRow+currDr, currCol+currDc, currDr, currDc), // go straight
        (currRow+currDr, currCol+currDc, lc, lr), // go left ( relative to current direction )
        (currRow+currDr, currCol+currDc, rc, rr), // go right ( relative to current direction )
    ];

    foreach (var neighbour in goToDirections)
    {
        var row = neighbour.Item1;
        var col = neighbour.Item2;
        if (grid[row][col] != '#')
        {
            neighbours.Add(neighbour);
        }
    }
    return neighbours;
}

(int, int) TurnLeft(int dr, int dc)
{
    if (dr == 0 && dc == 1)
    {
        return (-1, 0);
    } else if (dr == 0 && dc == -1)
    {
        return (1, 0);
    }else if (dr == 1 && dc == 0) // facing south -> east
    {
        return (0, 1);
    }else if (dr == -1 && dc == 0) // facing north -> west
    {
        return (0, -1);
    }

    throw new Exception();
}

(int, int) TurnRight(int dr, int dc)
{
    if (dr == 0 && dc == 1)
    {
        return (1, 0);
    } else if (dr == 0 && dc == -1)
    {
        return (-1, 0);
    }else if (dr == 1 && dc == 0) // facing south -> west
    {
        return (0, -1);
    }else if (dr == -1 && dc == 0) // facing north -> east
    {
        return (0, 1);
    }

    throw new Exception();
}

(int, int) FindPos(char[][] grid, char ele)
{
    for (int r = 0; r < grid.Length; r++)
    {
        for (int c = 0; c < grid[r].Length; c++)
        {
            if (grid[r][c] == ele) return (r, c);
        }
    }
    return (-1, -1);
}

struct Pathing
{
    public int Movements {get;set;}
    public int Turns {get;set;}
}