using ImpoDoc.Common;
using System;

namespace ImpoDoc.ViewModel
{
    public class BaseViewModel : PropertyChangedNotification
    {
        protected readonly Predicate<object> CanExecute = delegate (object arg) { return true; };
    }
}
