using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ActionTestSuite
    {

        private class TestAction : IAction
        {
            private TestObject testObject;
            private int value;

            public TestAction(TestObject testObject, int value)
            {
                this.testObject = testObject;
                this.value = value;
            }

            public string Description()
            {
                return "change value by " + value.ToString();
            }

            public void DoAction()
            {
                testObject.Modify(value);
            }

            public void UndoAction()
            {
                testObject.Modify(-value);
            }
        }

        private class TestObject
        {

            private int value;
            public int Value { get { return value; } }

            public TestObject(int value)
            {
                this.value = value;
            }

            public void Modify(int amount)
            {
                this.value += amount;
            }


        }

        private class TestObserver : IObserver
        {
            private int count;
            public int Count { get { return count; } }

            public void Notify(object Sender, EventArgs args)
            {
                count++;
            }

            public TestObserver()
            {
                this.count = 0;
            }
        }

        #region Counts

        [Test]
        public void TestPastActionsCount()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            Assert.That(actionManager.PastActionsCount == 0, "Nothing been done, count should be 0");
            actionManager.DoAction(action);
            Assert.That(actionManager.PastActionsCount == 1, "We have done a Single action, Count should be 1");
            actionManager.UndoAction();
            Assert.That(actionManager.PastActionsCount == 0, "We Undid the only action, count should be zero");
        }

        [Test]
        public void TestRedoActionsCount()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            Assert.That(actionManager.FutureActionsCount == 0, "Nothing been done, count should be 0");
            actionManager.DoAction(action);
            Assert.That(actionManager.FutureActionsCount == 0, "We have not undone a single action, count should be 0");
            actionManager.UndoAction();
            Assert.That(actionManager.FutureActionsCount == 1, "We have undone the only action, count should be 1");
        }
        #endregion

        #region Can Undo, Can Redo
        [Test]
        public void TestCanUndo ()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            Assert.That(!actionManager.CanUndo(), "We should not be able to undo, if nothing has been done.");
            actionManager.DoAction(action);
            Assert.That(actionManager.CanUndo() , "We should be able to undo if we have done something");
            actionManager.UndoAction();
            Assert.That(!actionManager.CanUndo() , "We should not be able to undo if we have done one thing and then we undid it");
            actionManager.RedoAction();
            Assert.That(actionManager.CanUndo(), "We should not be able to undo if we have do -> undo -> redo");

        }

        [Test]
        public void TestCanRedo()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            Assert.That(!actionManager.CanRedo(), "We should not be able to redo, if nothing has been done.");
            actionManager.DoAction(action);
            Assert.That(!actionManager.CanRedo(), "We should not be able to undo if we have not undone an action.");
            actionManager.UndoAction();
            Assert.That(actionManager.CanRedo(), "We should  be able to undo if we have undone an action.");
            actionManager.RedoAction();
            Assert.That(!actionManager.CanRedo(), "We should not be able to undo if do->undo->redo.");
        }
        #endregion

        #region Single Action
        //do action
        [Test]
        public void DoesDoActionDoTheAction()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action);

            Assert.AreEqual(testObject.Value, 10);
        }

        //do action (flag is true)
        [Test]
        public void DoesDoActionNotDoTheActionIfTheActionHasBeenDone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action,true);

            Assert.AreEqual(testObject.Value, 8);
        }

        //undo action
        [Test]
        public void CanTheActionBeUndone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action);
            actionManager.UndoAction();

            Assert.AreEqual(testObject.Value, 8);


        }

        //undo action (flag is true)
        [Test]
        public void CanTheActionBeUndoneIfTheActionWasDone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action, true);
            actionManager.UndoAction();

            Assert.AreEqual(testObject.Value, 6);
        }

        //do -> undo -> redo
        [Test]
        public void CanActionBeDoneThenUndoneThenRedone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action);

            actionManager.UndoAction();
            actionManager.RedoAction();

            Assert.AreEqual(testObject.Value, 10);
        }

        //do (flag is true) -> undo -> redo
        [Test]
        public void CanActionBeDoneThenUndoneThenRedoneIfTheActionWasDone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);

            actionManager.DoAction(action,true);
            actionManager.UndoAction();
            actionManager.RedoAction();

            Assert.AreEqual(testObject.Value, 8);
        }

        #endregion

        #region Multiple Actions


        // do(a) -> do (b)
        [Test]
        public void DoTwoActions()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.DoAction(actionB);

            Assert.AreEqual(testObject.Value,  15);
        }

        // do(a) -> do (b) -> undo
        [Test]
        public void DoTwoActionsUndoOne()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.DoAction(actionB);
            actionManager.UndoAction();

            Assert.AreEqual(testObject.Value, 10);
        }

        // do(a) -> do (b) -> undo -> undo
        [Test]
        public void DoTwoActionsUndoTwo()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.DoAction(actionB);

            actionManager.UndoAction();
            actionManager.UndoAction();

            Assert.AreEqual(testObject.Value, 8);
        }

        // do(a) -> do (b) -> undo -> undo -> redo
        [Test]
        public void DoTwoActionsUndoTwoRedoOne()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.DoAction(actionB);

            actionManager.UndoAction();
            actionManager.UndoAction();

            actionManager.RedoAction();

            Assert.AreEqual(testObject.Value, 10);
        }

        // do(a) -> do (b) -> undo -> undo -> redo -> redo
        [Test]
        public void DoTwoActionsUndoTwoRedoTwo()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.DoAction(actionB);

            actionManager.UndoAction();
            actionManager.UndoAction();

            actionManager.RedoAction();
            actionManager.RedoAction();

            Assert.AreEqual(testObject.Value, 15);
        }

        #endregion

        #region RedoStackClearing

        [Test]
        public void DoesRedoStackGetClearedWhenANewActionIsDone()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);

            TestAction actionA = new TestAction(testObject, 2);
            TestAction actionB = new TestAction(testObject, 5);

            actionManager.DoAction(actionA);
            actionManager.UndoAction();
            actionManager.DoAction(actionB);

            Assert.That(!actionManager.CanRedo());

        }

        #endregion

        #region Observer

        [Test]
        public void ObserverDoAction()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);
            TestObserver observer = new TestObserver();

            actionManager.AddObserver(observer);
            actionManager.DoAction(action);

            Assert.That(observer.Count == 1, "We should observered only 1 action");

        }

        [Test]
        public void ObserveUndo()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);
            TestObserver observer = new TestObserver();

            actionManager.DoAction(action);

            actionManager.AddObserver(observer);
            actionManager.UndoAction();
            Assert.That(observer.Count == 1, "We should observered only 1 action");
        }

        [Test]
        public void ObserverRedo()
        {
            ActionManager actionManager = new ActionManager();
            TestObject testObject = new TestObject(8);
            TestAction action = new TestAction(testObject, 2);
            TestObserver observer = new TestObserver();

            actionManager.DoAction(action);
            actionManager.UndoAction();

            actionManager.AddObserver(observer);
            actionManager.RedoAction();

            Assert.That(observer.Count == 1, "We should observered only 1 action");
        }

        

        #endregion

    }
}
