using System.Threading.Tasks;

namespace Notes.Services
{
    public class DialogService : IDialogService
    {
        public DialogService()
        {
        }

        public Task ShowBaseDialog(string title, string msg, string buttonCopy)
        {
            return App.Current.MainPage.DisplayAlert(title, msg, buttonCopy);
        }
    }
}
