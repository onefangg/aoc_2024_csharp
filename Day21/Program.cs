// See https://aka.ms/new-console-template for more information

string[] inputCodes = ["029A", "980A", "179A","456A", "379A"];

ButtonPress[] numPads = [
    new('7', 0, 0),
    new('8', 0, 1),
    new('9', 0, 2),
    new('4', 1, 0),
    new('5', 1, 1),
    new('6', 1, 2),
    new('1', 2, 0),
    new('2', 2, 1),
    new('3', 2, 2),
    new('0', 3, 1),
    new('A', 3, 2),
];

ButtonPress[] dirPads = [
    new('^', 0, 1),
    new('A', 0, 2),
    new('<', 1, 0),
    new('v', 1, 1),
    new('>', 1, 2),
];

Direction[] DIRECTIONS = [
    
    new ('v', 1,0),
    new ('<', 0,-1),
    new ('>', 0,1),
    new ('^', -1,0),
];

var shortestNumPads = GetShortestPathsForNum(numPads, new ButtonPress(' ', 3 ,0));
var shortestDirPads = GetShortestPathsForNum(dirPads, new ButtonPress(' ', 0 ,0));

// Console.WriteLine(shortestNumPads.Count);
// Console.WriteLine(shortestDirPads.Count);
var p1 = 0;
foreach (var code in inputCodes)
{
    // var digits = code[..^1];
    var digits = code;

    var startPress = 'A';
    var cnt = 0;
    for (int i =0; i<digits.Length; i++)
    {
        var d = digits[i];
        var numpadDist = shortestNumPads[(startPress, d)].MinBy(x=>x.Length)!;
        // var fullPath = i == 0 ? startPress + numpadDist : numpadDist;
        var fullPath =  numpadDist;
        // Console.WriteLine($"{fullPath}");
        var calc = FindKeyPresses(fullPath, 2);
        cnt += calc;
        startPress = d;
    }

    var numPart = Int32.Parse(digits[..^1]);
    var res = numPart * cnt;
    p1 += res;
    Console.WriteLine($"Sum total for {code}: {res}");
    Console.WriteLine($"Working for {code}: {cnt} * {numPart} ");
}
Console.WriteLine($"Part 1: {p1}");

int FindKeyPresses(string path, int depth = 1)
{
    
    if (depth == 1)
    {
        var cnt = 0;
        path = 'A' + path;
        // Console.WriteLine($"Found path {path} at depth {depth}");
        for (int i = 0; i < path.Length-1; i++)
        {
            // Console.WriteLine($"Found path from {path[i]} to {path[i+1]} at depth {depth}: {shortestDirPads[(path[i], path[i+1])].MinBy(x=>x.Length)!}");
            cnt += shortestDirPads[(path[i], path[i+1])].MinBy(x=>x.Length)!.Length;
        
        }
        return cnt;
    }
    
    var cnt2 = 0;
    // Console.WriteLine($"Found path {path} at depth {depth}");
    path = 'A' + path;
    for (int i = 0; i < path.Length-1; i++)
    {
        
        // Console.WriteLine($"Found path from {path[i]} to {path[i+1]} at depth {depth}: {shortestDirPads[(path[i], path[i+1])].MinBy(x=>x.Length)!}");
        cnt2 += FindKeyPresses(shortestDirPads[(path[i], path[i+1])].MinBy(x=>x.Length)!, depth -1);
    }
    return cnt2;
}


Dictionary<(char, char), List<string>> GetShortestPathsForNum(ButtonPress[] inputNumPad, ButtonPress escape)
{
    var lookup = new Dictionary<(char, char), List<string>>();
    
    var (escR, escC) = (escape.r, escape.c);

    for (int i = 0; i < inputNumPad.Length; i++)
    {
        for (int j = 0; j < inputNumPad.Length; j++)
        {
            var fromToEnd = (inputNumPad[i].e, inputNumPad[j].e);
            if (i==j && fromToEnd.Item1 == fromToEnd.Item2)
            {
                if (lookup.ContainsKey(fromToEnd))
                {
                    lookup[fromToEnd].Add("A");    
                }
                else
                {
                    lookup[fromToEnd] = ["A"];
                }

                continue;
            };
            var startPress = inputNumPad[i];
            var endPress = inputNumPad[j];
            var (er, ec) = (endPress.r, endPress.c);

            var q = new Queue<(ButtonPress, List<char>)>([(startPress, [])]);
            var visited = new HashSet<ButtonPress>([]);
            List<char> minPath = [];
            while (q.Count > 0)
            {
                var (curr, path) = q.Dequeue();
                if (!visited.Add(curr))
                {
                    continue;   
                }
                if (curr.r == er && curr.c == ec)
                {
                    path.Add('A');
                    minPath = path;
                    break;
                }

                foreach (var dir in DIRECTIONS)
                {
                    var (rr, cc) =  (curr.r + dir.dr, curr.c + dir.dc);
                    // ignore everything if ure trying to go over the empty space
                    if (rr == escR && cc == escC) continue;
                    try
                    {
                        var matching = inputNumPad.Single(x => x.r == rr && x.c == cc);
                        var amendPath = new List<char>(path) { dir.d };
                        q.Enqueue((matching, amendPath));

                    }
                    catch
                    {
                     // do nothing
                    }
                }
            }

            if (lookup.ContainsKey((startPress.e, endPress.e)))
            {
                lookup[(startPress.e, endPress.e)].Add(string.Join("", minPath));    
            }
            else
            {
                lookup[(startPress.e, endPress.e)] = [string.Join("", minPath)];
            }
            
        }
    }
    return lookup;
}

struct Direction(char d, int dr, int dc)
{
    public char d { get; set; } = d;
    public int dr { get; set; } = dr;
    public int dc { get; set; } = dc;
}

record struct ButtonPress(char e, int r, int c)
{
    public char e { get; set; } = e;
    public int r { get; set; } = r;
    public int c { get; set; } = c;
    public char prev { get; set; }
}
