var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day16_sample.txt"));
var gridData = data.Select(x => x.ToCharArray()).ToArray();

var distance = InitDistanceGrid(gridData);
var visited = InitVisitedGrid(gridData);
var (startX, startY) = FindPos(gridData, 'S');
var (endX, endY) = FindPos(gridData, 'E');
var linkVertices = new Dictionary<Vertex, List<Vertex>>();
var priorityQueue = new SortedSet<Vertex>();
var minCost = int.MaxValue;
priorityQueue.Add(new Vertex()
{
    Row = startX,
    Col = startY,
    Dr = 0,
    Dc = 1,
    Cost = 0
});
distance[startX][startY] = 0;

while (priorityQueue.Any())
{
    var curr = priorityQueue.Min()!;
    priorityQueue.Remove(curr);
    if (visited[curr.Row][curr.Col]) continue;
    visited[curr.Row][curr.Col] = true;
    
    // VisualiseGrid(gridData, visited);
    (int,int)[] directions = [
        (curr.Dr, curr.Dc),
        curr.TurnLeft(),
        curr.TurnRight(),
    ];

    if (gridData[curr.Row][curr.Col] == 'E')
    {
        minCost = distance[curr.Row][curr.Col];
        break;
    }
    
    for (var idx = 0; idx < directions.Length; idx++)
    {
        var (dr, dc) = directions[idx];
        var nextRow = curr.Row + dr;
        var nextCol = curr.Col + dc;
        if (gridData[nextRow][nextCol] != '#')
        {
            var newDist = curr.Cost + (idx == 0 ? 1 : 1001);
            var linkedChild = new Vertex()
            {
                Row = nextRow,
                Col = nextCol,
                Dr = dr,
                Dc = dc,
                Cost = newDist
            };
            if (linkVertices.ContainsKey(linkedChild))
            {
                linkVertices[linkedChild].Add(curr);    
            }
            else
            {
                linkVertices[linkedChild] = [curr];
            }
            if (newDist < distance[nextRow][nextCol])
            {
                distance[nextRow][nextCol] = newDist; 
                priorityQueue.Add(linkedChild);    
            }
        }

    }
}

for (var i = 0; i < distance.Length; i++)
{
    for (var j = 0; j < distance[i].Length; j++)
    {
        if (distance[i][j] == int.MaxValue)
        {
            Console.Write("....");
        }
        else
        {
            Console.Write(distance[i][j]);    
        }
        Console.Write(",");
    }
    Console.WriteLine();
}
Console.WriteLine($"Part 1: {minCost}");


var endNodes = linkVertices.Where(x => x.Key.Row == endX && x.Key.Col == endY)
    .Select(x=>x.Key).ToArray();
var invertQueue = new Queue<Vertex>(endNodes);
var paths = InitFalseyGrid(gridData);
while (invertQueue.Any())
{
    var curr = invertQueue.Dequeue();
    if (curr.Row == startX && curr.Col == startY)
    {
        continue;
    }
    if (paths[curr.Row][curr.Col])
    {
        continue;
    }
    paths[curr.Row][curr.Col] = true;
    
    // (int,int)[] directions = [
    //     (curr.Dr, curr.Dc),
    //     curr.TurnLeft(),
    //     curr.TurnRight(),
    // ];
    //
    // for (var i = 0; i < directions.Length; i++)
    // {
    //     var nextRow = curr.Row + directions[i].Item1;
    //     var nextCol = curr.Col + directions[i].Item2;
    //     if (distance[nextRow][nextCol] == curr.Cost - 1 || distance[nextRow][nextCol] == curr.Cost - 1001)
    //     {
    //         invertQueue.Enqueue(new Vertex()
    //         {
    //             Row = nextRow,
    //             Col = nextCol,
    //             Dr = directions[i].Item1,
    //             Dc = directions[i].Item2,
    //             Cost = distance[nextRow][nextCol]
    //         });
    //         distance[nextRow][nextCol] = int.MaxValue;
    //     }
    // }
    var linkedNodes = linkVertices.Where(x => x.Key == curr).SelectMany(x => x.Value).ToList();
    foreach (var node in linkedNodes)
    {
        if (node.Cost == curr.Cost - 1 || node.Cost == curr.Cost - 1001)
        {
            invertQueue.Enqueue(node);
        }
}
    
}

for (var i = 0; i < paths.Length; i++)
{
    for (var j = 0; j < paths[i].Length; j++)
    {
        if (paths[i][j])
        {
            Console.Write("1");
        }
        else
        {
            Console.Write("0");    
        }
        Console.Write(",");
    }
    Console.WriteLine();
}

Console.WriteLine($"Part 2: {paths.SelectMany(x => x.ToArray()).Count(x=>x)}");



bool[][] InitFalseyGrid(char[][] grid)
{
    var visited = new bool[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        var row = new bool[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            row[c] = false;
        }
        visited[r] = row;
    }
    return visited;
}
void VisualiseGrid(char[][] grid, bool[][] visited)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == '#')
            {
                Console.Write(grid[i][j]);
            } else if (visited[i][j])
            {
                Console.Write('O');
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
}

int[][] InitDistanceGrid(char[][] grid)
{
    var distance = new int[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        
        var row = new int[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            row[c] = int.MaxValue;
        }
        distance[r] = row;
    }
    return distance;
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

bool[][] InitVisitedGrid(char[][] grid)
{
    var visited = new bool[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        
        var row = new bool[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            if (grid[r][c] == '#')
            {
                row[c] = false;
            }
            else
            {
                row[c] = false;
            }
        }
        visited[r] = row;
    }
    return visited;
}

record Vertex : IComparable<Vertex>
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Dr { get; set; }
    public int Dc { get; set; }
    public int Cost { get; set; }

    public (int, int) TurnLeft()
    {
        return (-1 * Dc, Dr);
    }
    public (int, int) TurnRight()
    {
        return (Dc, Dr * -1);
    }

    public int CompareTo(Vertex other)
    {
        if (Cost == other.Cost)
        {
            if (Row == other.Row)
            {
                return Col.CompareTo(other.Col);
            }
            return Row.CompareTo(other.Row);
        }
        return Cost.CompareTo(other.Cost);
    }
    
}

