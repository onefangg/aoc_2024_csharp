// See https://aka.ms/new-console-template for more information

var data  = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day24.txt"));
var splitAtIdx = Array.FindIndex(data, string.IsNullOrEmpty);
var wires = data[..splitAtIdx];
var gates = data[(splitAtIdx + 1)..];

var lookup = new Dictionary<string, bool>();
foreach (var w in wires)
{
    var (bitName, bit) = (w.Split(": ")[0], w.Split(": ")[1]);
    lookup[bitName] = !bit.Equals("0");
}

var q = new Queue<string>(gates);

while (q.Count > 0)
{
    var g = q.Dequeue();
    var parse = g.Split(" ");
    var op = parse[1];
    var (a, b) = (parse[0], parse[2]);
    if (!(lookup.ContainsKey(a) && lookup.ContainsKey(b)))
    {
        q.Enqueue(g);
        continue;
    }
    
    var c = parse[^1];

    switch (op)
    {
        case "AND":
            lookup[c] = lookup[a] && lookup[b];
            break;
        case "OR":
            lookup[c] = lookup[a] || lookup[b];
            break;
        case "XOR":
            lookup[c] = lookup[a] ^ lookup[b];
            break;
    }
}

Console.WriteLine($"{Convert.ToInt64(string.Join("",
    lookup.Where(x=>x.Key.StartsWith("z"))
        .OrderByDescending(x=>x.Key)
        .Select(x=> Convert.ToInt32(x.Value))), 2)}");




