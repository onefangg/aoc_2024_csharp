// See https://aka.ms/new-console-template for more information


var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day23.txt"));
var network = data.Select(x=> x.Split('-')).ToList();

var networkMap = new Dictionary<string, HashSet<string>>();

foreach (var node in network)
{
    var (partOne, partTwo) = (node[0], node[1]);
    if (!networkMap.TryAdd(partOne, [partTwo])) 
    {
        networkMap[partOne].Add(partTwo);
    }
    if (!networkMap.TryAdd(partTwo, [partOne])) 
    {
        networkMap[partTwo].Add(partOne);
    }
}


var connectedToThree = new List<string[]>();
foreach (var (node, neighbours) in networkMap)
{
    foreach (var n in neighbours)
    {
        if (!networkMap.ContainsKey(n))
        {
            continue;
        }
        networkMap.TryGetValue(n, out HashSet<string> indirectNeighbours);
        var sameNeighbours = indirectNeighbours.Intersect(neighbours).ToArray();
        if (sameNeighbours.Count() >= 1)
        {

            foreach (var n2 in sameNeighbours)
            {
                string[] constructMap = [node, n, n2];
                constructMap = constructMap.OrderBy(x => x).ToArray();
                if (!connectedToThree.Any(x => x.SequenceEqual(constructMap)))
                {
                    connectedToThree.Add(constructMap);    
                }    
            }
            
            
        }
    }
}

// var distinctThrees = connectedToThree
//     .DistinctBy(x => new HashSet<(string,string,string)>([(x.Item1, x.Item2, x.Item3)])).ToArray();
//
//
// var anyStartsWithT = 
//     distinctThrees.Where(x => new List<string>([x.Item1, x.Item2, x.Item3])
//         .Any(x => x.StartsWith("t"))).ToArray();

Console.WriteLine($"{connectedToThree.Count(x => x.Any(y => y.StartsWith("t")))}");


