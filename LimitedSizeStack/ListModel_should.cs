using System.Collections.Generic;
using NUnit.Framework;

namespace LimitedSizeStack;

[TestFixture]
public class ListModel_Should
{
	[Test]
	public void AddItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		Assert.That(model.Items, Is.EqualTo(new List<string>{"a", "bb", "ccc"}));
	}

	[Test]
	public void RemoveFromTheEnd()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(2);
		Assert.That(model.Items, Is.EqualTo(new List<string> { "a", "bb" }));
	}

	[Test]
	public void RemoveFromTheBeginning()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		Assert.That(model.Items, Is.EqualTo(new List<string> { "bb", "ccc" }));
	}

	[Test]
	public void RemoveFromTheMiddle()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(1);
		Assert.That(model.Items, Is.EqualTo(new List<string> { "a", "ccc" }));
		model.Undo();
		Assert.That(model.Items, Is.EqualTo(new List<string> { "a", "bb", "ccc" }));
	}

	[Test]
	public void RemoveAndUndoAllItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		model.RemoveItem(0);
		model.RemoveItem(0);
		Assert.That(model.Items, Is.EqualTo(new List<string>()));
		model.Undo();
		model.Undo();
		model.Undo();
		Assert.That(model.Items, Is.EqualTo(new List<string> { "a", "bb", "ccc" }));
	}

	[Test]
	public void UndoAddOperations()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		Assert.That(model.CanUndo(), Is.EqualTo(true));
		model.Undo();
		Assert.That(model.Items.Count, Is.EqualTo(0));
	}

	[Test]
	public void NotUndo_WhenEverythingIsUndone()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		Assert.That(model.CanUndo(), Is.EqualTo(false));
	}

	[Test]
	public void Add_AfterUndo()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		model.AddItem("qq");
		Assert.That(model.Items, Is.EqualTo(new List<string> { "qq" }));
	}

	[Test]
	public void Undo_AfterRemove()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		Assert.That(model.Items, Is.EqualTo(new List<string> { "a", "bb" }));
	}

	[Test]
	public void NotUndo_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(2);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		model.Undo();
		Assert.That(model.CanUndo(), Is.EqualTo(false));
		Assert.That(model.Items, Is.EqualTo(new List<string> {"a"}));
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(1);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("ccc");
		Assert.AreEqual(true, model.CanUndo());
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsZero()
	{
		var model = new ListModel<string>(0);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		Assert.AreEqual(false, model.CanUndo());
	}
}