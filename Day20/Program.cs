// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day20_sample.txt"));
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

bool[][] p2Visited = (bool[][])visited.Clone();

var orderedPath = new List<(int, int)>();
var p1Shortcuts = new HashSet<((int, int),  (int, int))>();
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
                p1Shortcuts.Add(((r,c),(drr,dcc)));
            }
        } 
    }
}
var p1SavingsLookup = GetCheatSavings(orderedPath, p1Shortcuts);
var res= p1SavingsLookup.Where(x => x.Key >= 100).Sum(x => x.Value);


Console.WriteLine(res);
var p2Shortcuts = new HashSet<((int, int), (int, int))>();
var legitWalkable = orderedPath.ToHashSet();
orderedPath.Insert(0, start);
foreach (var anchorStart in orderedPath.Take(1))
{
    var anchorAllEndpoints = Bls(anchorStart, 20);
    var allEmptyEndpoints = anchorAllEndpoints.Where(n => gridData[n.Item1][n.Item2]!='#').ToArray();
    foreach (var endpoint in allEmptyEndpoints)
    {
        p2Shortcuts.Add((anchorStart, endpoint));
    }
}


var p2SavingsLookup = GetCheatSavingsForPart2(orderedPath, p2Shortcuts);

foreach (var (key,val) in p2SavingsLookup.Where(x=>x.Key>=50).OrderByDescending(x=>x.Key))
{
    Console.WriteLine($"{key}: {val}");
}
Console.WriteLine($"{p2SavingsLookup.Count}");
HashSet<(int, int)> Bls((int,int) curr, int limit)
{
    var newSpace = new HashSet<(int, int)>();
    var q = new SortedSet<(int, int, int)>([(curr.Item1, curr.Item2, 0)]);
    while (q.Count > 0)
    {
        var (r, c, l) = q.Min();
        q.Remove((r, c, l));
        
        if (l >= limit) continue;
        foreach (var (dr, dc) in directions)
        {
            var (rr, cc) = (r + dr, c + dc);
            if (rr >= 0 && rr < height && cc >= 0 && cc < width)
            {
                if (newSpace.Add((rr, cc)))
                {
                    q.Add((rr,cc,l+1));
                }
            }
        }
    }
    newSpace.Remove((curr.Item1, curr.Item2));
    
    return newSpace;
}
Dictionary<int, int> GetCheatSavings(List<(int, int)> path, HashSet<((int, int), (int, int))> cheats)
{
    var counts = new Dictionary<int, int>();

    foreach (var c in cheats)
    {
        var (anchorStart,  anchorEnd) = c;
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


Dictionary<int, int> GetCheatSavingsForPart2(List<(int, int)> path, HashSet<((int, int), (int, int))> cheats)
{
    var counts = new Dictionary<int, int>();

    foreach (var ch in cheats)
    {
        var (anchorStart,  anchorEnd) = ch;
        (int, int,int) resume = (-1, -1,-1);
        var q = new Queue<(int, int, int)>([(anchorStart.Item1, anchorStart.Item2, 0)]);
        var visitedInQ = new HashSet<(int, int)>();
        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            var currNode = (curr.Item1, curr.Item2);
            var (r, c) = currNode;
            if (!visitedInQ.Add(currNode))
            {
                continue;
            };
            if (currNode == anchorEnd)
            {
                resume = curr;
                break;
            }

            foreach (var d in directions)
            {
                var (dr, dc) = d;
                var (rr, cc) = (dr + r, dc + c);
                if (rr >= 0 && rr < height && cc >= 0 && cc < width)
                {
                    // var walls = gridData[rr][cc] == '#' ? curr.Item3 + 1: curr.Item3;
                    // q.Enqueue((rr, cc,walls));
                    q.Enqueue((rr, cc,curr.Item3+1));
                }
            }
        }
        var firstStep = path.FindIndex(x => x == anchorStart) ;
        var reenterStep = path.FindIndex(x => x == anchorEnd);
        var savings = reenterStep - firstStep - resume.Item3;
        if (!counts.TryAdd(savings, 1))
        {
            counts[savings]++;
        }
    }
    return counts;
} 