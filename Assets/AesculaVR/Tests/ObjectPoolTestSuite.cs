using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ObjectPoolTestSuite
    {

        private class TestObjectPool : ObjectPool
        {

        }

        private class TestPoolable : IPoolable
        {
            private bool inPool = false;
            public bool InPool { get { return inPool; } } 


            public void OnPoppedFromPool()
            {
                inPool = false;
            }

            public void OnPushedToPool()
            {
                inPool = true;
            }
        }

        [Test]
        public void PushTest()
        {
            TestPoolable testPoolable = new TestPoolable();
            TestObjectPool testObjectPool = new TestObjectPool();

            testObjectPool.Push(testPoolable);

            Assert.IsTrue(testPoolable.InPool);
        }

        [Test]
        public void PopTest()
        {
            TestPoolable testPoolable = new TestPoolable();
            TestObjectPool testObjectPool = new TestObjectPool();

            testObjectPool.Push(testPoolable);
            testObjectPool.Pop();

            Assert.IsTrue(!testPoolable.InPool);
        }

        [Test]
        public void MultiplePushPopTest()
        {
            TestPoolable testPoolable = new TestPoolable();
            TestPoolable testPoolable2 = new TestPoolable();
            TestPoolable testPoolable3 = new TestPoolable();
            TestObjectPool testObjectPool = new TestObjectPool();

            testObjectPool.Push(testPoolable);
            testObjectPool.Push(testPoolable2);
            testObjectPool.Push(testPoolable3);

            Assert.IsTrue(testPoolable .InPool);
            Assert.IsTrue(testPoolable2.InPool);
            Assert.IsTrue(testPoolable3.InPool);

            testObjectPool.Pop();
            testObjectPool.Pop();
            testObjectPool.Pop();

            Assert.IsTrue(!testPoolable .InPool);
            Assert.IsTrue(!testPoolable2.InPool);
            Assert.IsTrue(!testPoolable3.InPool);
        }


        [Test]
        public void CountTest()
        {
            TestPoolable testPoolable = new TestPoolable();
            TestPoolable testPoolable2 = new TestPoolable();
            TestPoolable testPoolable3 = new TestPoolable();
            TestObjectPool testObjectPool = new TestObjectPool();

            Assert.IsTrue(testObjectPool.Count() == 0);
            testObjectPool.Push(testPoolable);
            Assert.IsTrue(testObjectPool.Count() == 1);
            testObjectPool.Push(testPoolable2);
            Assert.IsTrue(testObjectPool.Count() == 2);
            testObjectPool.Pop();
            Assert.IsTrue(testObjectPool.Count() == 1);
            testObjectPool.Push(testPoolable3);
            Assert.IsTrue(testObjectPool.Count() == 2);
            testObjectPool.Pop();
            Assert.IsTrue(testObjectPool.Count() == 1);
            testObjectPool.Pop();
            Assert.IsTrue(testObjectPool.Count() == 0);

        }

    }
}
