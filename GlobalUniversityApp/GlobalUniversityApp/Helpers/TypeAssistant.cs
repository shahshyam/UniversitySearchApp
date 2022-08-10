using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GlobalUniversityApp.Helpers
{
    public class TypeAssistant
    {
        public event EventHandler Idled = delegate { };
        public int WaitingMilliSeconds { get; set; }
        Timer waitingTimer;

        public TypeAssistant(int waitingMilliSeconds = 300)
        {
            WaitingMilliSeconds = waitingMilliSeconds;
            waitingTimer = new Timer(p =>
            {
                Idled(this, EventArgs.Empty);
            });
        }
        public void TextChanged()
        {
            waitingTimer.Change(WaitingMilliSeconds, Timeout.Infinite);
        }
    }
}
