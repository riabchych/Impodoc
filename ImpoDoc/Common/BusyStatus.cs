using System;

namespace ImpoDoc.Common
{
    public class BusyStatus
    {
        private static bool _isBusy;
        private static string _content;

        public static bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnIsBusyChanged(EventArgs.Empty);
            }
        }

        public static string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
                OnContentChanged(EventArgs.Empty);
            }
        }

        public static event EventHandler IsBusyChanged;
        public static event EventHandler ContentChanged;

        protected static void OnIsBusyChanged(EventArgs e)
        {
            IsBusyChanged?.Invoke(null, e);
        }

        protected static void OnContentChanged(EventArgs e)
        {
            ContentChanged?.Invoke(null, e);
        }

        static BusyStatus()
        {
            IsBusyChanged += (sender, e) => { };
            ContentChanged += (sender, e) => { };
        }

        BusyStatus()
        {
        }
    }
}
