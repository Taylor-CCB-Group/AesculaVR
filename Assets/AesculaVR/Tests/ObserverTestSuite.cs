using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ObserverTestSuite
    {

        private class ConcreateObservableObject : ObservableObject
        {

        }

        private class ConcreateObserver : IObserver
        {
            private int count = 0;
            public int Count { get { return count; } }

            public void Notify(object Sender, EventArgs args)
            {
                count++;
            }
        }

        #region ObservableObject Tests

        [Test]
        public void ObservableObject_CountIncrease()
        {
            // Use the Assert class to test conditions
            ConcreateObserver         observer = new ConcreateObserver();
            ConcreateObservableObject observable = new ConcreateObservableObject();

            //count should start at 0.
            Assert.That(((IObservable)observable).Count() == 0, "The initial count should be 0");

            observable.AddObserver(observer);
            Assert.That(((IObservable)observable).Count() == 1, "The Count should be 1, after we add a single object");
            
            ConcreateObserver observer2 = new ConcreateObserver();
            ConcreateObserver observer3 = new ConcreateObserver();

            observable.AddObserver(observer2);
            observable.AddObserver(observer3);

            Assert.That(((IObservable)observable).Count() == 3, "The Count should be 3, after we have added 3 objects");
        }

        [Test]
        public void ObservableObject_CountDecrease()
        {
            // Use the Assert class to test conditions
            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObserver observer2 = new ConcreateObserver();
            ConcreateObserver observer3 = new ConcreateObserver();

            ConcreateObservableObject observable = new ConcreateObservableObject();

            //count should start at 0.
            Assert.That(((IObservable)observable).Count() == 0, "The initial count should be 0");

            observable.AddObserver(observer);
            observable.AddObserver(observer2);
            observable.AddObserver(observer3);
            Assert.That(((IObservable)observable).Count() == 3, "The Count should be 3, after we have added 3 objects");
          
            observable.RemoveObserver(observer3);
            Assert.That(((IObservable)observable).Count() == 2, "The Count should be 2, after we have added 3 objects and removed 1");

            observable.RemoveObserver(observer2);
            observable.RemoveObserver(observer);

            Assert.That(((IObservable)observable).Count() == 0, "The Count should be 0, after we have added 3 objects and removed 3");
                                                    
        }

        [Test]
        public void ObservableObject_Notify()
        {
            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObservableObject observable = new ConcreateObservableObject();

            observable.AddObserver(observer);
            Assert.That(observer.Count == 0, "The observer count should start at 0");

            observable.NotifyObservers();
            Assert.That(observer.Count == 1, "The observer count should be 1");

            observable.NotifyObservers();
            Assert.That(observer.Count == 2, "The observer count should be 2");

            observable.NotifyObservers();
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 5, "The observer count should be 5");
        }

        [Test]
        public void ObservableObject_NotifyStopsWhenRemoved()
        {
            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObservableObject observable = new ConcreateObservableObject();

            observable.AddObserver(observer);

            observable.NotifyObservers();
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 3, "The observer count should be 3");

            observable.RemoveObserver(observer);
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 3, "The observer count should still be at 3");
        }

        [Test]
        public void ObservableObject_AddDuplicates()
        {
            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObservableObject observable = new ConcreateObservableObject();

            Assert.That(observable.AddObserver(observer) , "Add observer should return true when there's no duplicates");
            Assert.That(!observable.AddObserver(observer), "Add observer should return false when there's a duplicate");

        }

        [Test]
        public void ObservableObject_RemoveNonexistant()
        {
            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObservableObject observable = new ConcreateObservableObject();

            Assert.That(!observable.RemoveObserver(observer), "remove observer should return false if the target is not watching");

            observable.AddObserver(observer);
            Assert.That(observable.RemoveObserver(observer), "remove observer should return true if the target is  watching");

        }

        #endregion

        #region ObservableComponent_Tests

        [Test]
        public void ObservableComponent_CountIncrease()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();
            ConcreateObserver observer = new ConcreateObserver();

            Assert.That(((IObservable)observable).Count() == 0, "The initial count should be 0");

            observable.AddObserver(observer);
            Assert.That(((IObservable)observable).Count() == 1, "The Count should be 1, after we add a single object");

            ConcreateObserver observer2 = new ConcreateObserver();
            ConcreateObserver observer3 = new ConcreateObserver();

            observable.AddObserver(observer2);
            observable.AddObserver(observer3);

            Assert.That(((IObservable)observable).Count() == 3, "The Count should be 3, after we have added 3 objects");

            GameObject.Destroy(obj);
        }

        [Test]
        public void ObservableComponent_CountDecrease()
        {
            // Use the Assert class to test conditions
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();

            ConcreateObserver observer = new ConcreateObserver();
            ConcreateObserver observer2 = new ConcreateObserver();
            ConcreateObserver observer3 = new ConcreateObserver();

            //count should start at 0.
            Assert.That(((IObservable)observable).Count() == 0, "The initial count should be 0");

            observable.AddObserver(observer);
            observable.AddObserver(observer2);
            observable.AddObserver(observer3);
            Assert.That(((IObservable)observable).Count() == 3, "The Count should be 3, after we have added 3 objects");

            observable.RemoveObserver(observer3);
            Assert.That(((IObservable)observable).Count() == 2, "The Count should be 2, after we have added 3 objects and removed 1");

            observable.RemoveObserver(observer2);
            observable.RemoveObserver(observer);

            Assert.That(((IObservable)observable).Count() == 0, "The Count should be 0, after we have added 3 objects and removed 3");

            GameObject.Destroy(obj);
        }

        [Test]
        public void ObservableComponent_Notify()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();
            ConcreateObserver observer = new ConcreateObserver();

            observable.AddObserver(observer);
            Assert.That(observer.Count == 0, "The observer count should start at 0");

            observable.NotifyObservers();
            Assert.That(observer.Count == 1, "The observer count should be 1");

            observable.NotifyObservers();
            Assert.That(observer.Count == 2, "The observer count should be 2");

            observable.NotifyObservers();
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 5, "The observer count should be 5");

            GameObject.Destroy(obj);
        }

        [Test]
        public void ObservableComponent_NotifyStopsWhenRemoved()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();
            ConcreateObserver observer = new ConcreateObserver();

            observable.AddObserver(observer);

            observable.NotifyObservers();
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 3, "The observer count should be 3");

            observable.RemoveObserver(observer);
            observable.NotifyObservers();
            observable.NotifyObservers();
            Assert.That(observer.Count == 3, "The observer count should still be at 3");

            GameObject.Destroy(obj);
        }

        [Test]
        public void ObservableComponent_AddDuplicates()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();
            ConcreateObserver observer = new ConcreateObserver();

            Assert.That(observable.AddObserver(observer), "Add observer should return true when there's no duplicates");
            Assert.That(!observable.AddObserver(observer), "Add observer should return false when there's a duplicate");

            GameObject.Destroy(obj);
        }

        [Test]
        public void ObservableComponent_RemoveNonexistant()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            TestObservable observable = obj.AddComponent<TestObservable>();
            ConcreateObserver observer = new ConcreateObserver();

            Assert.That(!observable.RemoveObserver(observer), "remove observer should return false if the target is not watching");

            observable.AddObserver(observer);
            Assert.That(observable.RemoveObserver(observer), "remove observer should return true if the target is  watching");

            GameObject.Destroy(obj);
        }
        #endregion
    }
}
