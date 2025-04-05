using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        private readonly Dictionary<char, Action<IVirtualMachine>> commands = new();

        public string Instructions { get; } = string.Empty;
        public int InstructionPointer { get; set; }  
        public byte[] Memory { get; } = Array.Empty<byte>();
        public int MemoryPointer { get; set; }  

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            commands[symbol] = execute;
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                var instruction = Instructions[InstructionPointer];

                if (commands.ContainsKey(instruction))
                {
                    commands[instruction](this);
                }

                InstructionPointer++;
            }
        }
    }
}