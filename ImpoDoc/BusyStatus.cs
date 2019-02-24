using System;

namespace ImpoDoc
{
    public class BusyStatus
    {
        private static bool _isBusy;
        private static string _content;

        /// <summary>
        /// A static property which you'd like to bind to
        /// </summary>
        public static bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;

                // Raise a change event
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

                // Raise a change event
                OnContentChanged(EventArgs.Empty);
            }
        }

        // Declare a static event representing changes to your static property
        public static event EventHandler IsBusyChanged;
        public static event EventHandler ContentChanged;


        // Raise the change event through this static method
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
            // Set up an empty event handler
            IsBusyChanged += (sender, e) => { };
            ContentChanged += (sender, e) => { };
        }

        BusyStatus()
        {
        }
    }
}
