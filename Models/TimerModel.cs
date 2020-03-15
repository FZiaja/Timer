using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timer.Models
{
    class TimerModel
    {
        private string name;
        private int seconds;
        public TimerModel(string name, int seconds)
        {
            this.name = name;
            this.seconds = seconds;
        }
        public TimerModel(string name, int hours, int minutes, int seconds)
        {
            this.name = name;
            this.seconds = (hours * 3600) + (minutes * 60) + seconds;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int TotalSeconds
        {
            get
            {
                return seconds;
            }
        }

        public int Hours
        {
            get { return seconds / 3600; }
        }
        public int Minutes
        {
            get { return ((seconds) % 3600) / 60; }
        }
        public int Seconds
        {
            get { return seconds % 60; }
        }

        public override string ToString()
        {
            return Name + " - " + Hours.ToString() + ":" + Minutes.ToString().PadLeft(2, '0') + ":" + Seconds.ToString().PadLeft(2, '0'); //string.Format("{0}:{1}:{2}", Hours, string.PadLeft(Minutes.ToString(), 2));
        }
    }
}
