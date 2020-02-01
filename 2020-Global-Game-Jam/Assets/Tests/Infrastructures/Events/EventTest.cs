using System;
using NUnit.Framework;
using Repair.Infrastructures;

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
            Action<object> action = (obj) =>
            {
                success = true;
            };

            EventEmitter.Add("test", action);
            EventEmitter.Emit("test");
            Assert.IsTrue(success);
        }

        [Test]
        public void TestEmitInt()
        {
            var value = 0;
            Action<object> action = (val) =>
            {
                value = (int) val;
            };

            EventEmitter.Add("test", action);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value);
        }

        [Test]
        public void TestEmitMultiListener()
        {
            var value1 = 0;
            var value2 = 0;
            Action<object> action1 = (val) =>
            {
                value1 = (int)val;
            };

            Action<object> action2 = (val) =>
            {
                value2 = (int)val;
            };

            EventEmitter.Add("test", action1);
            EventEmitter.Add("test", action2);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 1, actual: value2);
        }

        [Test]
        public void TestEmitRemoveListener()
        {
            var value1 = 0;
            var value2 = 0;
            Action<object> action1 = (val) =>
            {
                value1 = (int)val;
            };

            Action<object> action2 = (val) =>
            {
                value2 = (int)val;
            };

            EventEmitter.Add("test", action1);
            EventEmitter.Add("test", action2);
            EventEmitter.Remove("test", action2);
            EventEmitter.Emit("test", 1);
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 0, actual: value2);
        }

        [Test]
        public void TestEmitOrder()
        {
            var value = 0;
            var value1 = 0;
            var value2 = 0;
            Action<object> action1 = (val) =>
            {
                value1 = ++value;
            };

            Action<object> action2 = (val) =>
            {
                value2 = ++value;
            };

            EventEmitter.Add("test", action1);
            EventEmitter.Add("test", action2);
            EventEmitter.Emit("test");
            Assert.AreEqual(expected: 1, actual: value1);
            Assert.AreEqual(expected: 2, actual: value2);
        }
    }
}
