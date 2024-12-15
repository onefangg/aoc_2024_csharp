// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day15.txt"));
int splitAt = -1;
for (int i = 0; i < data.Length; i++)
{
    if (data[i] == string.Empty)
    {
        splitAt = i;
        break;
    }
}

var grid = data[..splitAt].Select(x => x.ToCharArray()).ToArray();
var directions = data[(splitAt + 1)..].SelectMany(x => x).ToArray();


var currRobotPosition = FindRobotPosition(grid);
foreach (var dire in directions)
{
    if (dire == '<')
    {
        var newPos = ShiftRobotPosition(grid, (0,-1), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == '^')
    {
        var newPos = ShiftRobotPosition(grid, (-1,0), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == '>')
    {
        var newPos = ShiftRobotPosition(grid, (0,1), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == 'v')
    {
        var newPos = ShiftRobotPosition(grid, (1,0), currRobotPosition);
        currRobotPosition = newPos;
    }

}
VisualiseRobots(grid);
Console.WriteLine($"Part 1: {CalculateBoxCoordinations(grid)}");

void VisualiseRobots(char[][] gridData)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            Console.Write(gridData[i][j]);
        }
        Console.WriteLine();
    }
}

long CalculateBoxCoordinations(char[][] gridData)
{
    long res = 0;

    for (int i = 1; i < gridData.Length - 1; i++)
    {
        for (int j = 1; j < gridData[j].Length-1; j++)
        {
            if (gridData[i][j] == 'O' || gridData[i][j] == '[')
            {
                res += 100 * i + j;
            }
        }
    }

    return res;
}

(int, int) ShiftRobotPosition(char[][] gridData, (int, int) directionOffset, (int, int) robotPosition)
{
    var (x, y) = robotPosition;
    var (dx, dy) = directionOffset; 
    var (shiftedX, shiftedY) = (x+dx, y+dy);
    if (gridData[shiftedX][shiftedY] == '#')
    {
        return (x, y);
    } else if (gridData[shiftedX][shiftedY] == '.')
    {
        gridData[x][y] = '.';
        gridData[shiftedX][shiftedY] = '@';
        return (shiftedX, shiftedY);
    } else if (gridData[shiftedX][shiftedY] == 'O')
    {
        var (nextFreeX, nextFreeY) = (shiftedX + dx, shiftedY + dy);
        while (true)
        {
            if (gridData[nextFreeX][nextFreeY] == '.') break;
            else if (gridData[nextFreeX][nextFreeY] == '#')
            {
                nextFreeX = shiftedX;
                nextFreeY = shiftedY;
                break;
            }
            nextFreeX += dx;
            nextFreeY += dy;
        }

        if (shiftedX == nextFreeX && shiftedY == nextFreeY)
        {
            return (x, y);
        }
        gridData[x][y] = '.';
        gridData[shiftedX][shiftedY] = '@';
        gridData[nextFreeX][nextFreeY] = 'O';
        return (shiftedX, shiftedY);
    }
    throw new Exception("I don't think this will ever happen");

}

(int, int) FindRobotPosition(char[][] gridData)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (gridData[i][j] == '@')
            {
                return (i, j);
            }
        }
    }
    return (-1, -1);
}








