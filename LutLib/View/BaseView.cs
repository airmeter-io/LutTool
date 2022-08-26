using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LutLib.View
{
    public class BaseView : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string pPropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pPropertyName));
    }
}
