using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TickTimer : Indicator
    {
        [Parameter("Public v1.12", DefaultValue = "https://ctrader.com/users/profile/64575", Group = "m0")]
        public string M_ { get; set; }
        
        [Parameter("Centered?", DefaultValue = true, Group = "Settings")]
        public bool CenterPos { get; set; }
        
        [Parameter("Number Color", DefaultValue = "FF808080", Group = "Settings")]
        public Color NumCol { get; set; }
        
        [Parameter("Timer Reset Color", DefaultValue = "FFFE0000", Group = "Settings")]
        public Color ResetCol { get; set; }
        
        private DateTime lastTickTime;
        private int elapsedSeconds;

        protected override void Initialize()
        {
            lastTickTime = Server.Time;
            Timer.Start(1);
            DisplayElapsedTime();
        }

        public override void Calculate(int index)
        {
            if (Bid != Bars.OpenPrices.LastValue)
            {
                lastTickTime = Server.Time;
                Timer.Stop();
                Timer.Start(1);
                //Couldn't get the audio notifications to work
                //Notifications.PlaySound("C:\\Windows\\Media\\tada.wav");
            }
        }

        protected override void OnTimer()
        {
            elapsedSeconds = (int)(Server.Time - lastTickTime).TotalSeconds;
            DisplayElapsedTime();
        }

        private void DisplayElapsedTime()
        {
            Chart.RemoveObject("timerLabel");
            {
                if (CenterPos)
                {
                    if (elapsedSeconds == 1)
                    {
                        Chart.DrawStaticText("timerLabel", " " + elapsedSeconds.ToString(), VerticalAlignment.Center, HorizontalAlignment.Center, ResetCol);
                    }
                    else
                    {
                        Chart.DrawStaticText("timerLabel", " " + elapsedSeconds.ToString(), VerticalAlignment.Center, HorizontalAlignment.Center, NumCol);
                    }
                }
                else
                {
                    if (elapsedSeconds == 1)
                    {
                        Chart.DrawStaticText("timerLabel", " " + elapsedSeconds.ToString(), VerticalAlignment.Bottom, HorizontalAlignment.Right, ResetCol);
                    }
                    else
                    {
                        Chart.DrawStaticText("timerLabel", " " + elapsedSeconds.ToString(), VerticalAlignment.Bottom, HorizontalAlignment.Right, NumCol);
                    }
                }
            }
        }
    }
}