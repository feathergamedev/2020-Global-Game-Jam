using NUnit.Framework;
using Repair.Infrastructures.Events;

namespace Repair.Tests
{
    public class EventTest
    {
        [SetUp]
        public void TestSetUp()
        {
            EventEmitter.Reset();
        }

        [Test]
        public void TestEmitNull()
        {
            var success = false;

            EventEmitter.Add("test", Action1);
            EventEmitter.Emit("test");
            Assert.IsTrue(success);

            void Action1(object val)
            {
                success = true;
            }
        }

        [Test]
        public void TestEmitInt()
        {
            var value = 0;

            EventEmitter.Add("test", Action1);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value);

            void Action1(object val)
            {
                value = (int)val;
            }
        }

        [Test]
        public void TestEmitMultiListener()
        {
            var value1 = 0;
            var value2 = 0;

            EventEmitter.Add("test", Action1);
            EventEmitter.Add("test", Action2);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 1, actual: value2);

            void Action1(object val)
            {
                value1 = (int)val;
            }

            void Action2(object val)
            {
                value2 = (int)val;
            }
        }

        [Test]
        public void TestEmitRemoveListener()
        {
            var value1 = 0;
            var value2 = 0;

            EventEmitter.Add("test", Action1);
            EventEmitter.Add("test", Action2);
            EventEmitter.Remove("test", Action2);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 0, actual: value2);

            void Action1(object val)
            {
                value1 = (int)val;
            }

            void Action2(object val)
            {
                value2 = (int)val;
            }
        }

        [Test]
        public void TestEmitOrder()
        {
            var value = 0;
            var value1 = 0;
            var value2 = 0;

            EventEmitter.Add("test", Action1);
            EventEmitter.Add("test", Action2);
            EventEmitter.Emit("test");
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 2, actual: value2);

            void Action1(object val)
            {
                value1 = ++value;
            }

            void Action2(object val)
            {
                value2 = ++value;
            }
        }
    }
}
