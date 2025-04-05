using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var bracketPairs = FindMatchingBrackets(vm.Instructions);
            RegisterLoopCommands(vm, bracketPairs);
        }

        private static void RegisterLoopCommands(IVirtualMachine vm, Dictionary<int, int> bracketPairs)
        {
            RegisterOpenBracket(vm, bracketPairs);
            RegisterCloseBracket(vm, bracketPairs);
        }

        private static void RegisterOpenBracket(IVirtualMachine vm, Dictionary<int, int> bracketPairs)
        {
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                {
                    b.InstructionPointer = bracketPairs[b.InstructionPointer];
                }
            });
        }

        private static void RegisterCloseBracket(IVirtualMachine vm, Dictionary<int, int> bracketPairs)
        {
            vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = bracketPairs[b.InstructionPointer];
                }
            });
        }

        private static Dictionary<int, int> FindMatchingBrackets(string program)
        {
            var pairs = new Dictionary<int, int>();
            var stack = new Stack<int>();

            for (var i = 0; i < program.Length; i++)
            {
                var currentChar = program[i];
                if (currentChar == '[')
                {
                    stack.Push(i);
                }
                else if (currentChar == ']')
                {
                    if (stack.Count == 0)
                        throw new InvalidOperationException("Unmatched closing bracket");

                    var openPos = stack.Pop();
                    pairs[openPos] = i;
                    pairs[i] = openPos;
                }
            }

            if (stack.Count > 0)
                throw new InvalidOperationException("Unmatched opening bracket");

            return pairs;
        }
    }
}