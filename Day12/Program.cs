// See https://aka.ms/new-console-template for more information
var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day12.txt"));
var gridData = data.Select(r => r.ToCharArray()).ToArray();
var visited = new HashSet<(int,int)>();

var res = 0;
for (int r = 0; r < gridData.Length; r++)
{
    for (int c = 0; c < gridData[r].Length; c++)
    {
        var isVisted = visited.Add((r, c));
        if (!isVisted) continue;
        
        var currEle= gridData[r][c];
        Queue<(int, int)> getCurrEleNeighbours = new Queue<(int,int)>
        (
    [
                (r-1, c),
                (r+1, c),
                (r, c-1),
                (r, c+1)
            ]
        );
        var samePlot = new List<(int, int)>() { (r, c) };
        while (getCurrEleNeighbours.Count > 0)
        {
            var (popRow, popCol) = getCurrEleNeighbours.Dequeue();
            try
            {
                var getNeighbour = gridData[popRow][popCol];
                if (getNeighbour == currEle && !visited.Contains((popRow, popCol)))
                {
                    visited.Add((popRow, popCol));
                    samePlot.Add((popRow, popCol));
                    (int, int)[] popNeighbours = [
                        (popRow-1,popCol),
                        (popRow+1,popCol),
                        (popRow,popCol-1),
                        (popRow,popCol+1),
                    ];
                    Array.ForEach(popNeighbours.Where(x => x.Item1 >= 0 && x.Item2 >= 0 ).ToArray(), e =>
                    {
                        getCurrEleNeighbours.Enqueue(e);    
                    });
                }
            }
            catch
            {
                // ignored
            }
        }
        res += CalculatePrice(samePlot);
    }
}

Console.WriteLine(res);

int CalculatePrice(List<(int, int)> neighbours)
{
    int area = neighbours.Count;
    var perimeter = 0;
    foreach (var neighbour in neighbours)
    {
        var (r, c) = neighbour;
        (int, int)[] numberOfNeighbours = [
            (r - 1, c),
            (r + 1, c),
            (r, c + 1),
            (r, c- 1),
        ];
        var adjacentCount = neighbours.Intersect(numberOfNeighbours).Count();
        perimeter += 4 -adjacentCount; 
    }
    return area * perimeter;
}

