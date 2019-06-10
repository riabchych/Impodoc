using ImpoDoc.Commands;
using ImpoDoc.Entities;

namespace ImpoDoc.ViewModel
{
    public class IncomingDocDetailsViewModel : ItemDetailsViewModel<IncomingDocument>
    {
        private RelayCommand<object> selectFileCommand;
        public RelayCommand<object> SelectFileCommand
        {
            get
            {
                return selectFileCommand ??
                  (selectFileCommand = new RelayCommand<object>(obj => ActiveItem.Attachment.Create(), delegate (object arg) { return true; }));
            }
        }

        private RelayCommand<object> openFileCommand;
        public RelayCommand<object> OpenFileCommand
        {
            get
            {
                return openFileCommand ??
                  (openFileCommand = new RelayCommand<object>(obj => ActiveItem.Attachment.CreateAndOpenFile(), CheckAttachment));
            }
        }

        private RelayCommand<object> printFileComand;
        public RelayCommand<object> PrintFileComand
        {
            get
            {
                return printFileComand ??
                  (printFileComand = new RelayCommand<object>(obj => ActiveItem.Attachment.CreateAndPrintFile(), CheckAttachment));
            }
        }

        private bool CheckAttachment(object arg)
        {
            return !(ActiveItem.Attachment.Content == null || ActiveItem.Attachment.Filename == null || ActiveItem.Attachment.Path == null);
        }

    }
}