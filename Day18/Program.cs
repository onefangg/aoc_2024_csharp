var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day18.txt"));

var (startX, startY) = (0, 0);
// testing sample
// var (endX, endY) = (6, 6);
// var n = 12;

var (endX, endY) = (70, 70);
var nIterations = 1024;

var fallingBytes = data.Select(x =>x.Split(",").Select(int.Parse).ToArray()).ToArray();
var gridData = CreateGridAfterN(fallingBytes, endX, endY, nIterations);
var dist = CreatDistanceGrid(endX, endY);
var visited = CreateVisitedGrid(endX, endY);
dist[startX][startY] = 0;
visited[startX][startY] = true;

var q = new Queue<(int, int)>([(startX, startY)]);
while (q.Count > 0)
{
    var curr = q.Dequeue();
    
    (int, int)[] directions = [(-1, 0), (0, -1), (1,0), (0, 1)];
    var currDist = dist[curr.Item1][curr.Item2];
    if (curr.Item1 == endX && curr.Item2 == endY)
    {
        break;
    }
    
    foreach (var dir in directions)
    {
        try
        {
            var nextX = curr.Item1 + dir.Item1;
            var nextY = curr.Item2 + dir.Item2;

            if (!visited[nextX][nextY] && gridData[nextX][nextY] == '.')
            {
                visited[nextX][nextY] = true;
                dist[nextX][nextY] = currDist + 1;
                q.Enqueue((nextX, nextY));
            }
        }
        catch
        {
            continue;
        }

    }
}

// for (int i = 0; i < gridData.Length; i++)
// {
//     for (int j = 0; j < gridData.Length; j++)
//     {
//         Console.Write(gridData[i][j] + " ");
//     }
//     Console.WriteLine();
// }

// for (int i = 0; i < dist.Length; i++)
// {
//     for (int j = 0; j < dist.Length; j++)
//     {
//         Console.Write(dist[i][j] + " ");
//     }
//     Console.WriteLine();
// }

Console.WriteLine($"Part 1: {dist[endX][endY]}");

bool[][] CreateVisitedGrid(int widthInclusive, int heightInclusive)
{
    var grid = new bool[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new bool[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = false;
        }
    }
    return grid;
}

int[][] CreatDistanceGrid(int widthInclusive, int heightInclusive)
{
    var grid = new int[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new int[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = Int32.MaxValue;
        }
    }
    return grid;
}

char[][] CreateGridAfterN(int[][] falling, int widthInclusive, int heightInclusive, int n)
{
    var grid = new char[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new char[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = '.';
        }
    }
    var takeForFalling = falling.Take(n).ToArray();

    foreach (var row in takeForFalling)
    {
        var (x, y) = (row[0], row[1]);
        grid[x][y] = '#';
    }
    return grid;
}


