// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day20.txt"));
var gridData = data.Select(r => r.ToCharArray()).ToArray();
(int,int)[] directions = [(-1, 0), (0, -1), (1, 0), (0, 1)];
(int, int) start = (0,0);
(int, int) end  = (0,0);
int height = gridData.Length;
int width = gridData[0].Length;
bool[][] visited = new bool[gridData.Length][];

for (int i = 0; i < gridData.Length; i++)
{
    var visitedRow = new bool[gridData[i].Length];
    for (int j = 0; j < gridData[i].Length; j++)
    {
        if (gridData[i][j] == 'S')
        {
            start = (i, j);
        } else if (gridData[i][j] == 'E')
        {
            end = (i, j);
        }
        visitedRow[j]= false;
    }
    visited[i] = visitedRow;
}

var orderedPath = new List<(int, int)>();
var shortcuts = new HashSet<((int, int), (int, int) , (int, int))>();
var q = new Queue<(int,int)>([start]);

while (q.Count > 0)
{
    var (r, c) = q.Dequeue();
    visited[r][c] = true;
    foreach (var d in directions)
    {
        var (dr, dc) = d;
        var rr = r + dr;
        var cc = c + dc;
        if (visited[rr][cc] == false && gridData[rr][cc] != '#')
        {
            q.Enqueue((rr, cc));
            orderedPath.Add((rr, cc));
        } else if (gridData[rr][cc] == '#')
        {
            var drr = rr + dr;
            var dcc = cc + dc;
            if (drr >= 0 && drr < height && dcc >= 0 && dcc < width && gridData[drr][dcc] != '#' && !visited[drr][dcc])
            {
                shortcuts.Add(((r,c),(rr,cc), (drr,dcc)));
            }
        } 
    }
}

var savingsLookup = GetCheatSavings(orderedPath, shortcuts);

var res= savingsLookup.Where(x => x.Key >= 100).Sum(x => x.Value);
Console.WriteLine(res);

// foreach (var (key, val) in savingsLookup)
// {
//     Console.WriteLine($"{key}: {val}");
// }

// Console.WriteLine(orderedPath.Count);
// Console.WriteLine(shortcuts.Count);


Dictionary<int, int> GetCheatSavings(List<(int, int)> path, HashSet<((int, int), (int, int) , (int, int))> cheats)
{
    var counts = new Dictionary<int, int>();

    foreach (var c in cheats)
    {
        var (anchorStart, cut, anchorEnd) = c;
        var firstStep = path.FindIndex(x => x == anchorStart) ;
        var reenterStep = path.FindIndex(x => x == anchorEnd);
        var savings = reenterStep - firstStep - 2;
        if (!counts.TryAdd(savings, 1))
        {
            counts[savings]++;
        }
    }
    return counts;
}