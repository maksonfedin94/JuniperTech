using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnAppearing()
        {

        }

        public virtual void OnDisappearing()
        {

        }
    }
}
