using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrumNotes.Util;
using NUnit.Framework;

namespace DrumNotes.Tests
{
    [TestFixture]
    public class TempoTests
    {
        [Test]
        public void Tempo_ShouldBe_60()
        {
            var tempoUtil = new TempoUtility();

            var dateA = DateTime.Now;
            var dateB = dateA.AddSeconds(1);

            var t = tempoUtil.CalculateTampo(dateA, dateB).ToString();
            Assert.IsTrue(t=="60");
        }

        [Test]
        public void Tempo_ShouldBe_30()
        {
            var tempoUtil = new TempoUtility();

            var dateA = DateTime.Now;
            var dateB = dateA.AddSeconds(2);

            var t = tempoUtil.CalculateTampo(dateA, dateB).ToString();
            Assert.IsTrue(t == "30");
        }

        [Test]
        public void Tempo_ShouldBe_120()
        {
            var tempoUtil = new TempoUtility();

            var dateA = DateTime.Now;
            var dateB = dateA.AddSeconds(0.5);

            var t = tempoUtil.CalculateTampo(dateA, dateB).ToString();
            Assert.IsTrue(t == "120");
        }

        [Test]
        public void Reverse_Works()
        {
            var tempoUtil = new TempoUtility();
            var ms = tempoUtil.CalculateMs(120);
            Assert.IsTrue(ms==500);
        }
    }
}
