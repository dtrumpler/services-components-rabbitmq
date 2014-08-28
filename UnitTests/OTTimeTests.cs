using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace OpenTable.Services.Components.Common.UnitTests.OTTimeTests
{
    [TestFixture]
    public class OTTimeMethodTests
    {
        private DateTime _dt1 = DateTime.Parse("2014-03-14 08:00");
        private DateTime _dt2 = DateTime.Parse("7/6/2014 2:00:00 PM");
        private DateTime _dt3 = DateTime.Parse("2013-12-31 23:31");

        private int _tzEST = 8;
        private int _tzHawaii = 2;
        private int _tzEastEurope = 17;


        [SetUp]
        public void SetUp()
        {
            SystemTime.SetUtc(_dt3);
        }

        [TearDown]
        public void TearDown()
        {
            SystemTime.Clear();
        }

        [Test]
        public void ConvertToDateTimeOffset_Yields_Expected_Result()
        {
            string expected = "3/14/2014 8:00:00 AM -04:00";
            string actual = OTTime.ConvertToDateTimeOffset(_dt1, _tzEST).ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertToUtc_Yields_Expected_Result()
        {
            string expected = "3/14/2014 6:00:00 PM";
            string actual = OTTime.ConvertToUtc(_dt1, _tzHawaii).ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertFromUtc_Yields_Expected_Result()
        {
            string expected = "3/14/2014 4:00:00 AM";
            string actual = OTTime.ConvertFromUtc(_dt1, _tzEST).ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void In_EST_ConvertFromUtc_composed_with_ConvertToUtc_Yields_Original_DateTime()
        {
            Assert.AreEqual(_dt1, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt1, _tzEST), _tzEST));
            Assert.AreEqual(_dt2, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt2, _tzEST), _tzEST));
            Assert.AreEqual(_dt3, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt3, _tzEST), _tzEST));
        }

        [Test]
        public void In_HI_ConvertFromUtc_composed_with_ConvertToUtc_Yields_Original_DateTime()
        {
            Assert.AreEqual(_dt1, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt1, _tzHawaii), _tzHawaii));
            Assert.AreEqual(_dt2, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt2, _tzHawaii), _tzHawaii));
            Assert.AreEqual(_dt3, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt3, _tzHawaii), _tzHawaii));
        }


        [Test]
        public void In_EastEurope_ConvertFromUtc_composed_with_ConvertToUtc_Yields_Original_DateTime()
        {
            Assert.AreEqual(_dt1, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt1, _tzEastEurope), _tzEastEurope));
            Assert.AreEqual(_dt2, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt2, _tzEastEurope), _tzEastEurope));
            Assert.AreEqual(_dt3, OTTime.ConvertFromUtc(OTTime.ConvertToUtc(_dt3, _tzEastEurope), _tzEastEurope));
        }

        [Test]
        public void GetLocalNow_Yields_The_Expected_Result()
        {
            var expected = SystemTime.UtcNow().AddHours(-5); // EST is -5 hours from UTC
            var actual = OTTime.GetLocalNow(_tzEST);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToIsoDateTimeString_NoSeconds_Yields_The_Expected_Result()
        {
            string expected = "2013-12-31T23:31-05:00";
            string actual = OTTime.ToIsoDateTimeString(_dt3, 8);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToIsoDateTimeString_WithSeconds_Yields_The_Expected_Result()
        {
            string expected = "2013-12-31T23:31:00-05:00";
            string actual = OTTime.ToIsoDateTimeString(_dt3, 8, true);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void All_Time_Zone_MS_Names_Are_Valid()
        {
            Assert.DoesNotThrow(() => Enumerable.Range(1, 38).Select(n => OTTime.GetTimeZoneInfo(n)).ToList());
        }
    }

    [TestFixture]
    public class when_testing_timezones_with_no_day_light_savings_times
    {
        private static readonly DateTime nonDslDate = DateTime.Parse("2014-02-21 20:00");

        private readonly Dictionary<int, string> _expected = new Dictionary<int, string>()
        {
            {2, "2014-02-21T20:00-10:00"},		//Hawaiian
            {3, "2014-02-21T20:00-09:00"},		//Alaska
            {4, "2014-02-21T20:00-08:00"},		//Pacific
            {5, "2014-02-21T20:00-07:00"},		//US Mountain Standard
            {6, "2014-02-21T20:00-07:00"},		//Mountain Standard
            {7, "2014-02-21T20:00-06:00"},		//Central Standard Time
            {8, "2014-02-21T20:00-05:00"},		//Eastern Standard Time
            {9, "2014-02-21T20:00-05:00"},		//US Eastern Standard Time
            {10, "2014-02-21T20:00-04:00"},		//Atlantic Standrad Time
            {15, "2014-02-21T20:00+00:00"},		//Greenwich Standard
            {16, "2014-02-21T20:00+01:00"},		//Central Europe Standard 
            {27, "2014-02-21T20:00+09:00"},		//Tokyo Standard Time
            {32, "2014-02-21T20:00-06:00"},		//Central Standard Mexico Time
            {33, "2014-02-21T20:00-04:00"},		//SA Western Standard Time
            {34, "2014-02-21T20:00-06:00"},		//Central america Standard Time
            {35, "2014-02-21T20:00-07:00"},		//Mountain Mexico Standard Time
            {36, "2014-02-21T20:00-08:00"},		//Pacific Mexico Standard Time
            {38, "2014-02-21T20:00-05:00"},		//S.A. Pacific Standard Time
        };

        [Test]
        public void it_should_validate_Dates()
        {
            foreach (KeyValuePair<int, string> entry in _expected)
            {
                string actual = OTTime.ToIsoDateTimeString(nonDslDate, entry.Key);
                Assert.IsTrue(entry.Value.Equals(actual));
            }
        }
    }

    [TestFixture]
    public class when_testing_timezones_with_day_light_savings_times
    {
        private static readonly DateTime dslDate = DateTime.Parse("2014-09-21 20:00");

        private readonly Dictionary<int, string> _expected = new Dictionary<int, string>()
        {
            {2, "2014-09-21T20:00-10:00"},		//Hawaiian (no DST)
            {3, "2014-09-21T20:00-08:00"},		//Alaska
            {4, "2014-09-21T20:00-07:00"},		//Pacific
            {5, "2014-09-21T20:00-07:00"},		//US Mountain Standard
            {6, "2014-09-21T20:00-06:00"},		//Mountain Standard
            {7, "2014-09-21T20:00-05:00"},		//Central Standard Time
            {8, "2014-09-21T20:00-04:00"},		//Eastern Standard Time
            {9, "2014-09-21T20:00-04:00"},		//US Eastern Standard Time
            {10, "2014-09-21T20:00-03:00"},		//Atlantic Standrad Time
            {15, "2014-09-21T20:00+01:00"},		//Greenwich Standard (no DST)
            {16, "2014-09-21T20:00+02:00"},		//Central Europe Standard 
            {32, "2014-09-21T20:00-05:00"},		//Central Standard Mexico Time
            {35, "2014-09-21T20:00-06:00"},		//Mountain Mexico Standard Time
            {36, "2014-09-21T20:00-07:00"},		//Pacific Mexico Standard Time
            {38, "2014-09-21T20:00-05:00"},		//S.A. Pacific Standard Time - mislabled in database
        };

        [Test]
        public void it_should_validate_Dates()
        {
            foreach (KeyValuePair<int, string> entry in _expected)
            {
                string actual = OTTime.ToIsoDateTimeString(dslDate, entry.Key);
                Assert.IsTrue(entry.Value.Equals(actual));
            }
        }
    }

}
