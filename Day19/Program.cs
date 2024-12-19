// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day19.txt")).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

var availTowels = data[0].Split(", ").ToArray();
var desiredPatterns = data[1..];

var cnt = 0;
foreach (var design in desiredPatterns)
{
    var iterateThrough = new SortedSet<string>(Comparer<string>.Create((a,b)=>a.Length-b.Length));
    iterateThrough.Add(design);
    
    while (iterateThrough.Count > 0)
    {
        var matchAgainst = iterateThrough.Min()!;
        iterateThrough.Remove(matchAgainst);
        
        if (matchAgainst == "")
        {
            cnt++;
            break;
        }
        foreach (var towel in availTowels)
        {
            if (matchAgainst.StartsWith(towel))
            {
                iterateThrough.Add(matchAgainst[towel.Length..]);
            }
        }

    }
}
Console.WriteLine(cnt);
