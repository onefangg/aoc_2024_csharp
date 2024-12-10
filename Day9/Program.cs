// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day9.txt"))[0].ToCharArray().Select(x =>x.ToString()).ToArray();
var diskMap = new List<string>();
var currRunningNumber = 0;
var shouldFlipToFreeMemory = false;

for (int i = 0; i < data.Length; i++)
{
    var repeatN = Int32.Parse(data[i]);
    var addMemory = currRunningNumber.ToString();
    for (int n = 0; n < repeatN; n++)
    {
        if (shouldFlipToFreeMemory)
        {
            diskMap.Add(".");
        }
        else
        {
            diskMap.Add(addMemory);
        }

        
    }

    if (!shouldFlipToFreeMemory) currRunningNumber++;
    shouldFlipToFreeMemory = !shouldFlipToFreeMemory;
}



int FindNonFreeMemoryIndexFromTheBack(List<string> inputDiskMap, int startFromindex)
{
    for (int i = startFromindex; i > 0; i--)
    {
        if (inputDiskMap[i] != ".") return i;
    }
    //DUD
    return -1;
}

var updateDiskMap = new List<string>();
var end = FindNonFreeMemoryIndexFromTheBack(diskMap, diskMap.Count()-1);
var shiftedIndices = new List<int>();

for (int idx = 0; idx < diskMap.Count; idx++)
{
    if (shiftedIndices.Contains(idx) || idx > end)
    {
        updateDiskMap.Add(".");
        continue;
    }

    if (diskMap[idx] != ".")
    {
        updateDiskMap.Add(diskMap[idx]);
    }
    else
    {
        updateDiskMap.Add(diskMap[end]);
        shiftedIndices.Add(end);
        end  = FindNonFreeMemoryIndexFromTheBack(diskMap, end - 1);
    }
}

var res = (double) 0;
var diskMapJoin = string.Join("", updateDiskMap);

for (int i = 0; i < updateDiskMap.Count(); i++)
{
    if (updateDiskMap[i] == ".")
    {
        break;
        
    }
    res += (double)(i * Int64.Parse(updateDiskMap[i].ToString()));
}



// Console.WriteLine($"{string.Join("", updateDiskMap)}");
Console.WriteLine($"{res.ToString("F0")}");

