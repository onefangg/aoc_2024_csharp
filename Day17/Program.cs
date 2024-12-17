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
var inputRegisters = new Dictionary<char, long>();
foreach (var register in parsingRegisters)
{
    var splitRegister = register.Split(": ");
    var registerName = splitRegister[0];
    inputRegisters[registerName[^1]] = Int32.Parse(splitRegister[1]);
}
var inputProgramString = parsingProgram.Split(": ")[^1];
var outputProgramString = RunProgram(inputRegisters, inputProgramString);
Console.WriteLine($"{outputProgramString}");

string RunProgram(Dictionary<char, long> register, string programStr)
{
    var output = new List<char>();
    var program = programStr.Split(",").ToArray();
    
    int instructionPointer = 0;
    while (instructionPointer < program.Length)
    {
        var opCode = int.Parse(program[instructionPointer]);
        var operand = GetValueFromOperand(char.Parse(program[instructionPointer + 1]), register);
        if (opCode == 0)
        {
            var numerator = register['A'];
            var denominator = Math.Pow(2, operand);
            register['A'] = (long)Math.Truncate(numerator / denominator);
        }
        else if (opCode == 1)
        {
            register['B'] ^= long.Parse(program[instructionPointer + 1]);
        }
        else if (opCode == 2)
        {
            register['B'] = operand % 8;
        }
        else if (opCode == 3)
        {
            if (register['A'] != 0)
            {
                instructionPointer = int.Parse(program[instructionPointer + 1]);
                continue;
            }
        }
        else if (opCode == 4)
        {
            register['B'] ^= register['C'];
        }
        else if (opCode == 5)
        {
            output.Add(char.Parse((operand % 8).ToString()));
        }
        else if (opCode == 6)
        {
            var numerator = register['A'];
            var denominator = Math.Pow(2, operand);
            register['B'] = (long)Math.Truncate(numerator / denominator);
        }
        else if (opCode == 7)
        {
            var numerator = register['A'];
            var denominator = Math.Pow(2, operand);
            register['C'] = (long)Math.Truncate(numerator / denominator);
        }
        instructionPointer += 2;
    }
    return String.Join(",", output);
}

long GetValueFromOperand(char operand, Dictionary<char, long> registers)
{
    switch (operand)
    {
        case '0':
        case '1':
        case '2':
        case '3':
            return long.Parse(operand.ToString());
        case '4': return registers['A'];
        case '5': return registers['B'];
        case '6': return registers['C'];
    }
    // shoulnd't happen
    return -1;
}