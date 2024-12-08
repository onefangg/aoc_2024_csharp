// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day8_sample.txt"));
var gridData = data.Select(x => x.ToCharArray()).ToArray();
var antennasCoords = new List<(int, int)>();
var antennasLookup = new Dictionary<char, List<(int, int)>>();
var width = gridData[0].Length;
var height = gridData.Length;


// find the coords of all the antennas
for (var row = 0; row < gridData.Length; row++)
{
    for (var col = 0; col < gridData[row].Length; col++)
    {
        var element = gridData[row][col];
        if (element != '.')
        {
            antennasCoords.Add((row, col));
            if (!antennasLookup.ContainsKey(element))
            {
                antennasLookup[element] = [(row, col)];
            } 
            antennasLookup[element].Add((row, col));
        }
    }
}


var antinodes = new HashSet<(int, int)>();
while (antennasCoords.Count > 0)
{
    var (currRow, currCol) = antennasCoords[0];
    var currentAntenna = gridData[currRow][currCol];
    antennasCoords.RemoveAt(0);
    var antinodesSpawn = antennasLookup[currentAntenna]
        .SelectMany(coord =>
        {
            var (row, col) = coord;
            if (row == currRow && col == currCol) return Array.Empty<(int, int)>();
            var offsetRow = row - currRow;
            var offsetCol = col - currCol;
            return [(row + offsetRow, col + offsetCol), (currRow + -offsetRow, currCol + -offsetCol)];
        }).Where(coord => (coord.Item1 >= 0 && coord.Item1 < width) && (coord.Item2 >= 0 && coord.Item2 < height))
        .ToArray();
    
    Array.ForEach(antinodesSpawn, antinode =>
    {
        Console.WriteLine($"row {antinode.Item1} - col {antinode.Item2}");
        antinodes.Add(antinode);
    });
}

Console.WriteLine(antinodes.Count);