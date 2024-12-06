// See https://aka.ms/new-console-template for more information


var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day6_sample.txt"));
var grid = data.Select(row => row.ToCharArray()).ToArray();

(int, int) GetCoordinates(char[][] gridData, char ele)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (ele == gridData[i][j]) return (i, j);
        }
    }
    // cannot find
    return (-1, -1);
} 
var guardStartPos = GetCoordinates(grid, '^');
var guardFacingDirection = Direction.Up;

var magicList = new List<(int, int)> ();
var width = grid[0].Length;
(int, int) GetNewPositionAfterTravelling(char[][] gridData, (int, int) startPos, Direction travelDirection)
{
    (int, int) newPos = (-1, -1);
    var (rowIdx, colIdx) = startPos;

    
    switch (travelDirection)
    {
        case Direction.Up:
        {
            newPos = (Enumerable.Range(0, rowIdx).Reverse().FirstOrDefault(row => gridData[row][colIdx] == '#')+1, colIdx);
            Array.ForEach(Enumerable.Range(rowIdx, rowIdx - newPos.Item1).ToArray(), i => magicList.Add((i, colIdx)));
            break;
        }
        case Direction.Down:
        {
            newPos = (Enumerable.Range(rowIdx+1, (gridData.Length-rowIdx-1)).FirstOrDefault(row => gridData[row][colIdx] == '#')-1, colIdx);
            Array.ForEach(Enumerable.Range(rowIdx, newPos.Item1 - rowIdx).ToArray(), i => magicList.Add((i, colIdx)));
            break;
        }
        case Direction.Left:
        {
            newPos = (rowIdx, 1 + Enumerable.Range(0, colIdx).Reverse().FirstOrDefault(col => gridData[rowIdx][col] == '#'));
            Array.ForEach(Enumerable.Range(colIdx, colIdx - newPos.Item2).ToArray(), i => magicList.Add((rowIdx, i)));
            break;
        }
        case Direction.Right:
        {
            newPos = (rowIdx, -1 + Enumerable.Range(colIdx+1, (width-colIdx-1)).FirstOrDefault(col => gridData[rowIdx][col] == '#'));
            Array.ForEach(Enumerable.Range(colIdx, newPos.Item2 - colIdx).ToArray(), i => magicList.Add((rowIdx, i)));
            break;
        }
        
    };
    return newPos;
}

var res = 0;

var starPos = guardStartPos;
var starDir = guardFacingDirection;
while (true) {
    try
    {
        var (startPosRow, startPosCol) = starPos;
        var (newPosRow, newPosCol) = GetNewPositionAfterTravelling(grid, starPos, starDir);

        var distanceTravelled = Math.Max(Math.Abs(newPosRow - startPosRow), Math.Abs(newPosCol - startPosCol));
        Console.WriteLine($"distance travelled {distanceTravelled}");
        res += distanceTravelled;
        starPos = (newPosRow, newPosCol);
        if (starDir == Direction.Up) starDir = Direction.Right;
        else if (starDir == Direction.Right) starDir = Direction.Down;
        else if (starDir == Direction.Down) starDir = Direction.Left;
        else if (starDir == Direction.Left) starDir = Direction.Up;
    }
    catch
    {
        var (startPosRow, startPosCol) = starPos;
        if (starDir == Direction.Up)
        {
            
            magicList.AddRange(Enumerable.Range(0, startPosRow+1).Select(x => (x, startPosCol)));
        }
        else if (starDir == Direction.Right)
        {
            magicList.AddRange(Enumerable.Range(startPosCol, width-startPosCol+1).Select(x => (startPosRow, x)));
        }
        else if (starDir == Direction.Down)
        {
            magicList.AddRange(Enumerable.Range(startPosCol, width-startPosCol+1).Select(x => (x, startPosCol)));
        }
        else if (starDir == Direction.Left)
        {
            
        }
        
        break;
    }
}

Console.WriteLine(res);




public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}