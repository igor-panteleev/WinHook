using System;

namespace WinHook.Models
{
    public static class EventProxy<T>
    {
        public static event EventHandler<T> EventCaptured;

        private static void OnEventCaptured(object sender, T e)
        {
            if (EventCaptured != null)
            {
                EventCaptured(sender, e);
            }
        }

        public static void CaptureEvent(object sender, T e)
        {
            OnEventCaptured(sender, e);
        }
    }
}
