// See https://aka.ms/new-console-template for more information

var inputString = @"
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0";


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


int opCode = -1;
bool didJump = false;

for (int i = 0; i < program.Length; i++)
{
     if (i % 2 == 0) 
     {
         // instruction
         opCode = int.Parse(program[i]);
     }
     else
     {
         if (didJump)
         {
             didJump = false;
             continue;
         }
         
         var operand = int.Parse(program[i]);
         if (opCode == 0)
         {
             var numerator = registers['A'];
             var denominator = Math.Pow(2, operand);
             registers['A'] = (int) Math.Truncate(numerator / denominator);
         }
         else if (opCode == 1)
         {
             registers['B'] = Convert.ToByte(registers['B']) ^ Convert.ToByte(operand);
         }
         else if (opCode == 2)
         {
             registers['B'] = operand % 8;
         }
         else if (opCode == 3)
         {
             if (registers['A'] != 0)
             {
                 registers['A'] = operand;
                 didJump = true;
             } 
         }
         else if (opCode == 4)
         {
             registers['C'] = Convert.ToByte(registers['B']) ^ Convert.ToByte(registers['C']);
         }
         else if (opCode == 5)
         {
             Console.Write($"{operand % 5},");
         }
         else if (opCode == 6)
         {
             var numerator = registers['A'];
             var denominator = Math.Pow(2, operand);
             registers['A'] = (int) Math.Truncate(numerator / denominator);
         }
         else if (opCode == 7) {}
         
     }
    
}

Console.WriteLine(program[0]);