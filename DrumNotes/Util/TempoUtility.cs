using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumNotes.Util
{
    public class TempoUtility
    {
        public double CalculateTampo(DateTime previous, DateTime now)
        {
            var diff = now - previous;
            const double oneSixith = (double)((double) 1/(double) 60);
            var ms = (double)diff.Ticks/(double)10000;
            
            var tempo = (1000/ms)/oneSixith;
            return tempo;
        }

        public int CalculateMs(int tempo)
        {
            return (int)Math.Round((double)1000/(double)(tempo/(double)60));
        }
    }
}
