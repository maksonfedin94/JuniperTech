using System;
using System.Threading.Tasks;

namespace Notes.Services
{
    public interface IDialogService
    {
        Task ShowBaseDialog(string title, string msg, string buttonCopy);
    }
}
