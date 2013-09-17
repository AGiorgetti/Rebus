﻿using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Rebus.Bus;
using Shouldly;
using System.Linq;

namespace Rebus.Tests.Unit
{
    [TestFixture]
    public class TestHeaderContext : FixtureBase
    {
        RebusBus.HeaderContext c;

        protected override void DoSetUp()
        {
            c = new RebusBus.HeaderContext();
        }

        [Test]
        public void AssociatesHeadersWithObjects()
        {
            // arrange
            var firstObject = new object();
            var secondObject = new object();

            // act
            c.AttachHeader(firstObject, "first-header1", "first-value");
            c.AttachHeader(firstObject, "first-header2", "first-value");
            c.AttachHeader(secondObject, "second-header", "first-value");

            // assert
            c.headers.Single(s => s.Item1.Target == firstObject).ShouldContainKeyAndValue("first-header1", "first-value");
            c.headers.Single(s => s.Item1.Target == firstObject).ShouldContainKeyAndValue("first-header2", "first-value");
            c.headers.Single(s => s.Item1.Target == secondObject).ShouldContainKeyAndValue("second-header", "first-value");
        }

        [Test]
        public void UsesWeakReferencesToKeepAssociation()
        {
            // arrange
            var someObject = new object();
            c.AttachHeader(someObject, "header1", "value1");
            c.AttachHeader(someObject, "header2", "value2");

            // just check that the dictionary is there
            c.headers.Count.ShouldBe(1);

            // act
            someObject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            c.Tick();

            // assert
            c.headers.Count.ShouldBe(0);
        }

        [Test]
        public void CleansUpPeriodically()
        {
            // arrange
            var someObject = new object();
            c.AttachHeader(someObject, "header1", "value1");

            // act
            someObject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(2000);

            // assert
            c.headers.Count.ShouldBe(0);
        }
    }

    public static class AssertionExtensions
    {
        public static void ShouldContainKeyAndValue(this Tuple<WeakReference, Dictionary<string, object>> tuple, string key, string value)
        {
            var dictionary = tuple.Item2;

            dictionary.ShouldContainKeyAndValue(key, value);
        }
    }
}