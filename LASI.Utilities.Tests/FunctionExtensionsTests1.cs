﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using LASI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace LASI.Utilities.Tests
{
    using static FunctionExtensions;
    [TestClass()]
    public class FunctionExtensionsTests1
    {
        [TestMethod()]
        public void ComposeTest()
        {
            Func<double, double> f = x => x * x;
            Func<double, double> g = x => x - 1;
            var target = f.Compose(g);
            var value = 5D;
            var temp = g(value);
            var expected = f(temp);
            var actual = target(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ComposeTest1()
        {
            bool fCalled = false;
            bool gCalled = false;
            Func<double, double> f = x =>
            {
                Assert.IsTrue(gCalled);
                fCalled = true;
                return x * x;
            };
            Func<double, double> g = x =>
            {
                Assert.IsFalse(fCalled);
                gCalled = true;
                return x - 1;
            };
            var target = f.Compose(g)(5);
            Assert.IsTrue(fCalled);
            Assert.IsTrue(gCalled);
        }

        [TestMethod()]
        public void ComposeTest2()
        {
            bool fCalled = false;
            bool gCalled = false;
            Func<double, double> f;
            Func<double, double> g;
            string failure = $"{nameof(f)} threw";
            f = x =>
            {
                fCalled = true;
                return 5;
            };
            g = x =>
            {
                gCalled = true;
                throw new Exception(failure);
            };
            var target = f.Compose(g);
            try
            {
                target(5);
            }
            catch (Exception e) when (e.Message == failure)
            {
            }
            Assert.IsFalse(fCalled);
            Assert.IsTrue(gCalled);
        }

        [TestMethod()]
        public void ComposeTest4()
        {
            ComposeTest1Helper<GenericParameterHelper, GenericParameterHelper, GenericParameterHelper>();
        }
        [TestMethod()]
        public void ComposeTest5()
        {
            ComposeTest1Helper<object, object, object>();
            ComposeTest1Helper<string, string, object>();
            ComposeTest1Helper<string, string, object>();

            ComposeTest1Helper<object, object, int>();
            ComposeTest1Helper<string, int, object>();
            ComposeTest1Helper<int, string, object>();
            ComposeTest1Helper<byte, int, decimal>();
            ComposeTest1Helper<Complex, int, int>();
            ComposeTest1Helper<BigInteger, int, int>();
            ComposeTest1Helper<int?, int?, int?>();
            ComposeTest1Helper<Complex?, Complex?, Complex?>();
            ComposeTest1Helper<BigInteger?, BigInteger?, BigInteger?>();
        }
        /// <summary>
        ///A test for Compose
        /// </summary>
        public void ComposeTest1Helper<R, U, T>()
        {
            Func<R, T> f = r => default(T);
            Func<U, R> g = u => default(R);
            Func<U, T> expected = u => default(T);
            Func<U, T> actual;
            var y = f.Compose(g);
            actual = FunctionExtensions.Compose(f, g);
            Assert.AreEqual(expected(default(U)), default(T));

        }

        [TestMethod()]
        public void AndThenTest4()
        {
            bool a1Called = false;
            bool a2Called = false;

            Action<string> a1 = s =>
            {
                Assert.IsFalse(a2Called);
                a1Called = true;
                Logger.Log(s);
            };
            Action<string> a2 = s =>
            {
                Assert.IsTrue(a1Called);
                a2Called = true;
                Logger.Log(s.ToUpper());
            };
            var target = a1.AndThen(a2);
            target("hello");
            Assert.IsTrue(a1Called);
            Assert.IsTrue(a2Called);
        }

        [TestMethod()]
        public void AndThenTest5()
        {
            bool a1Called = false;
            bool a2Called = false;

            Action<string> a1 = s =>
            {
                Assert.IsFalse(a2Called);
                a1Called = true;
                Logger.Log(s);
            };
            Action a2 = () =>
            {
                Assert.IsTrue(a1Called);
                a2Called = true;
                Logger.Log($"called {nameof(a2)}");
            };
            var target = a1.AndThen(a2);
            target("hello");
            Assert.IsTrue(a1Called);
            Assert.IsTrue(a2Called);
        }

        [TestMethod()]
        public void AndThenTest6()
        {
            bool a1Called = false;
            bool a2Called = false;

            Action a1 = () =>
            {
                Assert.IsFalse(a2Called);
                a1Called = true;
                Logger.Log($"called {nameof(a1)}");
            };
            Action a2 = () =>
            {
                Assert.IsTrue(a1Called);
                a2Called = true;
                Logger.Log($"called {nameof(a2)}");
            };
            var target = a1.AndThen(a2);
            target();
            Assert.IsTrue(a1Called);
            Assert.IsTrue(a2Called);
        }

        [TestMethod()]
        public void AndThenTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AndThenTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AndThenTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AndThenTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest4()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest5()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest6()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest7()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest8()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest9()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest10()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest11()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest12()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CurryTest13()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest4()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest5()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest6()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest7()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest8()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest9()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest10()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest11()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ApplyTest12()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IdentityTest()
        {
            IdentityTestHelper<object>();
            IdentityTestHelper<int>();
            IdentityTestHelper<int?>();
        }
        /// <summary>
        ///A test for Identity
        /// </summary>
        public void IdentityTestHelper<T>() where T : new()
        {
            T target = new T();
            T expected = target;
            T actual = Identity(target);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void WithTimerTest()
        {
            int synthetizedWaitInMs = 0;
            Complex result = default(Complex);
            Func<Complex> compute = () =>
            {
                System.Threading.Thread.Sleep(synthetizedWaitInMs);
                return result;
            };
            System.Diagnostics.Stopwatch sw;

            var computeWithTimer = compute.WithTimer(out sw);
            Assert.IsFalse(sw.IsRunning);
            computeWithTimer();
            Assert.IsFalse(sw.IsRunning);
        }

        [TestMethod()]
        public void WithTimerTest1()
        {
            int synthetizedWaitInMs = 0;
            Func<Complex, Complex> compute = data =>
            {
                System.Threading.Thread.Sleep(synthetizedWaitInMs);
                return new Complex(data.Imaginary, data.Real);
            };
            System.Diagnostics.Stopwatch sw;

            var computeWithTimer = compute.WithTimer(out sw);
            Assert.IsFalse(sw.IsRunning);
            computeWithTimer(new Complex(2, 2));
            Assert.IsFalse(sw.IsRunning);
        }

        [TestMethod()]
        public void WithTimerTest2()
        {
            int synthetizedWaitInMs = 0;
            Complex result = default(Complex);
            Action compute = () =>
            {
                System.Threading.Thread.Sleep(synthetizedWaitInMs);
                result = new Complex(1, -1);
            };
            System.Diagnostics.Stopwatch sw;

            var computeWithTimer = compute.WithTimer(out sw);
            Assert.IsFalse(sw.IsRunning);
            computeWithTimer();
            Assert.IsFalse(sw.IsRunning);
        }
    }
}