using System.ComponentModel;

namespace CD_in.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region IDataErrorInfo

        public string Error => string.Empty;

        public virtual string this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }

        protected bool IsPropertysValid(params string[] names)
        {
            return names?.All(x => string.IsNullOrEmpty(this[x])) ?? true;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
