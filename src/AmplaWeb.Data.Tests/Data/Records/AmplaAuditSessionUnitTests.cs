using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AmplaWeb.Data.Records
{
    [TestFixture]
    public class AmplaAuditSessionUnitTests : TestFixture
    {
        [Test]
        public void SortByTime()
        {
            AmplaAuditSession now = new AmplaAuditSession("User", DateTime.Now);
            AmplaAuditSession tomorrow = new AmplaAuditSession("User", DateTime.Today.AddDays(1));
            AmplaAuditSession nightBeforeLast = new AmplaAuditSession("User", DateTime.Today.AddDays(-1).AddHours(-1));

            List<AmplaAuditSession> list = new List<AmplaAuditSession> {tomorrow, nightBeforeLast, now};

            list.Sort();

            Assert.That(list[0], Is.EqualTo(nightBeforeLast));
            Assert.That(list[1], Is.EqualTo(now));
            Assert.That(list[2], Is.EqualTo(tomorrow));
        }

        [Test]
        public void SortByUser()
        {
            DateTime now = DateTime.Now;
            AmplaAuditSession one = new AmplaAuditSession("One", now);
            AmplaAuditSession two = new AmplaAuditSession("Two", now);
            AmplaAuditSession three = new AmplaAuditSession("Three", now);

            List<AmplaAuditSession> list = new List<AmplaAuditSession> { two, one, three };

            list.Sort();

            Assert.That(list[0], Is.EqualTo(one));
            Assert.That(list[1], Is.EqualTo(three));
            Assert.That(list[2], Is.EqualTo(two));
        }

        [Test]
        public void SortByNumberOfSessions()
        {
            DateTime now = DateTime.Now;
            AmplaAuditSession one = new AmplaAuditSession("One", now);
            AmplaAuditSession two = new AmplaAuditSession("Two", now);
            AmplaAuditSession three = new AmplaAuditSession("Three", now);

            one.Fields.Add(new AmplaAuditField());
            two.Fields.Add(new AmplaAuditField());
            two.Fields.Add(new AmplaAuditField());
            three.Fields.Add(new AmplaAuditField());
            three.Fields.Add(new AmplaAuditField());
            three.Fields.Add(new AmplaAuditField());


            List<AmplaAuditSession> list = new List<AmplaAuditSession> { two, one, three };

            list.Sort();

            Assert.That(list[0], Is.EqualTo(three));
            Assert.That(list[1], Is.EqualTo(two));
            Assert.That(list[2], Is.EqualTo(one));
        }


    }
}