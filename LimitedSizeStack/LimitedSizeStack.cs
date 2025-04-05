using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly long _maxStackSize;
    private readonly LinkedList<T> _stack;

    public LimitedSizeStack(long limit)
    {
        _maxStackSize = limit;
        _stack = new LinkedList<T>();
    }

    public long Count => _stack.Count;

    public void Push(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
            
        _stack.AddFirst(item);

        if (_stack.Count > _maxStackSize)
        {
            _stack.RemoveLast();
        }
    }

    public T Pop()
    {
        if (_stack.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        var topItem = _stack.First!.Value;
        _stack.RemoveFirst();
        return topItem;
    }

    public void PrintStack()
    {
        foreach (var item in _stack)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}