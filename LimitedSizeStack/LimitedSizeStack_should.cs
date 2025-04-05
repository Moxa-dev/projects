using System;
using NUnit.Framework;

namespace LimitedSizeStack;

[TestFixture]
public class LimitedSizeStack_should
{
	[Test]
	public void PopAfterPush()
	{
		var stack = new LimitedSizeStack<int>(20);
		stack.Push(0);
		stack.Push(1);
		stack.Push(2);
		Assert.That(stack.Pop(), Is.EqualTo(2));
		Assert.That(stack.Pop(), Is.EqualTo(1));
		Assert.That(stack.Pop(), Is.EqualTo(0));
	}

	[Test]
	public void BeEmpty_AfterCreation()
	{
		var stack = new LimitedSizeStack<int>(20);
		Assert.That(stack.Count, Is.EqualTo(0));
	}
        
	[Test]
	public void PushAfterPop()
	{
		var stack = new LimitedSizeStack<int>(20);
		stack.Push(0);
		stack.Push(1);
		stack.Pop();
		stack.Push(2);
		Assert.That(stack.Pop(), Is.EqualTo(2));
		Assert.That(stack.Pop(), Is.EqualTo(0));
	}

	[Test]
	public void PushAfterEmptyStack()
	{
		var stack = new LimitedSizeStack<int>(20);
		stack.Push(0);
		stack.Push(1);
		stack.Pop();
		stack.Pop();
		stack.Push(2);
		Assert.That(stack.Pop(), Is.EqualTo(2));
	}

	[Test]
	public void Count_StackSize()
	{
		var stack = new LimitedSizeStack<int>(20);
		stack.Push(0);
		stack.Push(10);
		stack.Push(20);
		Assert.That(stack.Count, Is.EqualTo(3));
	}

	[Test]
	public void ForgetFirstElement_AfterPushBeyondLimit()
	{
		var stack = new LimitedSizeStack<int>(2);
		stack.Push(0);
		stack.Push(10);
		stack.Push(20);
		Assert.That(stack.Count, Is.EqualTo(2));
		Assert.That(stack.Pop(), Is.EqualTo(20));
	}

	[Test]
	public void ForgetFirstElement_AfterPushBeyondLimit_Twice()
	{
		var stack = new LimitedSizeStack<int>(2);
		stack.Push(0);
		stack.Push(1);
		stack.Push(2);
		stack.Pop();
		stack.Pop(); // empty!
		stack.Push(0);
		stack.Push(10);
		stack.Push(20);
		Assert.That(stack.Count, Is.EqualTo(2));
		Assert.That(stack.Pop(), Is.EqualTo(20));
		Assert.That(stack.Pop(), Is.EqualTo(10));
	}

	[Test]
	public void PopPushAfterLimitReached()
	{
		var stack = new LimitedSizeStack<int>(2);
		stack.Push(0);
		stack.Push(1);
		stack.Push(2);
		stack.Pop();
		stack.Push(3);
		Assert.That(stack.Pop(), Is.EqualTo(3));
		Assert.That(stack.Pop(), Is.EqualTo(1));
		Assert.That(stack.Count, Is.EqualTo(0));
	}

	[Test]
	public void WorkCorrectlyWhenLimitZero()
	{
		var stack = new LimitedSizeStack<int>(0);
		stack.Push(1);
		stack.Push(2);
		stack.Push(3);
		Assert.That(stack.Count, Is.EqualTo(0));
	}

	[Test]
	[Description("Стек не должен ссылаться на элементы, которые уже удалены из него")]
	public void StackDontKeepAllElements()
	{
		var counter = new Counter();
		var stack = new LimitedSizeStack<FinalizableClass>(30);
		for (var i = 0; i < 100; ++i)
			stack.Push(new FinalizableClass(counter));
		// Явный вызов сборщика мусора. В обычных программах так делать не нужно почти никогда. 
		// Но в этом тесте нам нужно убедиться, что на вытесненные из стека элементы больше не осталось ссылок,
		// Для этого мы вызываем сборщик мусора и проверяем, у скольки объектов сборщик мусора вызвал деструктор
		GC.Collect();
		GC.WaitForPendingFinalizers();
		Assert.That(counter.Value, Is.EqualTo(70));
		stack.Push(new FinalizableClass(counter)); // Чтобы объект стека не собрался сборщиком мусора раньше времени
	}
}