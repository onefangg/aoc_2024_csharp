// See https://aka.ms/new-console-template for more information

// actual input
// var inputString = @"
// Register A: 33024962
// Register B: 0
// Register C: 0
//
// Program: 2,4,1,3,7,5,1,5,0,3,4,2,5,5,3,0";

var inputString = @"
Register A: 117440
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0";

var parsingInput = inputString.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
var (parsingRegisters, parsingProgram) = (parsingInput[..3], parsingInput[3]);
var registers = parsingRegisters.Select(x =>
{
    var splitRegister = x.Split(": ");
    return long.Parse(splitRegister[1]);
}).ToArray();

var inputProgramString = parsingProgram.Split(": ")[^1];
var outputProgramString = RunProgram(registers[0],registers[1], registers[2], inputProgramString);
Console.WriteLine($"{outputProgramString}");

string RunProgram(long a, long b, long c, string programStr)
{
    var output = new List<char>();
    var program = programStr.Split(",").ToArray();
    
    int instructionPointer = 0;
    while (instructionPointer < program.Length)
    {
        var opCode = int.Parse(program[instructionPointer]);
        var operand = GetValueFromOperand(char.Parse(program[instructionPointer + 1]), a, b,c);
        if (opCode == 0) a = (long)Math.Truncate(a / Math.Pow(2, operand));
        else if (opCode == 1) b^= long.Parse(program[instructionPointer + 1]);
        else if (opCode == 2) b = operand % 8;
        else if (opCode == 3 && a != 0)
        {
            instructionPointer = int.Parse(program[instructionPointer + 1]);
            continue;
        }
        else if (opCode == 4) b ^= c;
        else if (opCode == 5) output.Add(char.Parse((operand % 8).ToString()));
        else if (opCode == 6) b = (long)Math.Truncate(a / Math.Pow(2, operand));
        else if (opCode == 7) c = (long)Math.Truncate(a / Math.Pow(2, operand));
        instructionPointer += 2;
    }
    return String.Join(",", output);
}

long GetValueFromOperand(char operand, long a, long b, long c)
{
    switch (operand)
    {
        case '0':
        case '1':
        case '2':
        case '3':
            return long.Parse(operand.ToString());
        case '4': return a;
        case '5': return b;
        case '6': return c;
    }
    return -1;
}