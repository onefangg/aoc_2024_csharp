// See https://aka.ms/new-console-template for more information

// actual input
var inputString = @"
Register A: 33024962
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,1,5,0,3,4,2,5,5,3,0";

// var inputString = @"
// Register A: 0
// Register B: 2024
// Register C: 43690
//
// Program: 4,0";


var parsingInput = inputString.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
var (parsingRegisters, parsingProgram) = (parsingInput[..3], parsingInput[3]);

var registers = new Dictionary<char, int>();
foreach (var register in parsingRegisters)
{
    var splitRegister = register.Split(": ");
    var registerName = splitRegister[0];
    registers[registerName[^1]] = Int32.Parse(splitRegister[1]);
}

var program = parsingProgram.Split(": ")[^1].Split(",").ToArray();

int instructionPointer = 0;

while (instructionPointer < program.Length)
{
    var opCode = int.Parse(program[instructionPointer]);
    var operand = GetValueFromOperand(char.Parse(program[instructionPointer + 1]), registers);
    if (opCode == 0)
    {
        var numerator = registers['A'];
        var denominator = Math.Pow(2, operand);
        registers['A'] = (int)Math.Truncate(numerator / denominator);
    }
    else if (opCode == 1)
    {
        registers['B'] ^= int.Parse(program[instructionPointer + 1]);
    }
    else if (opCode == 2)
    {
        registers['B'] = operand % 8;
    }
    else if (opCode == 3)
    {
        if (registers['A'] != 0)
        {
            instructionPointer = int.Parse(program[instructionPointer + 1]);
            continue;
        }
    }
    else if (opCode == 4)
    {
        registers['B'] ^= registers['C'];
    }
    else if (opCode == 5)
    {
        Console.Write($"{operand % 8},");
    }
    else if (opCode == 6)
    {
        var numerator = registers['A'];
        var denominator = Math.Pow(2, operand);
        registers['B'] = (int)Math.Truncate(numerator / denominator);
    }
    else if (opCode == 7)
    {
        var numerator = registers['A'];
        var denominator = Math.Pow(2, operand);
        registers['C'] = (int)Math.Truncate(numerator / denominator);
    }

    instructionPointer += 2;
}


int GetValueFromOperand(char operand, Dictionary<char, int> registers)
{
    switch (operand)
    {
        case '0':
        case '1':
        case '2':
        case '3':
            return Int32.Parse(operand.ToString());
        case '4': return registers['A'];
        case '5': return registers['B'];
        case '6': return registers['C'];
    }

    // shoulnd't happen
    return -1;
}
