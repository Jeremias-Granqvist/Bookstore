using Shared.Command;
using Shared.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Bookstore.Service.Interfaces;
using Shared.Statics;

namespace Bookstore.viewModel
{
    public class EditAuthorDialogViewModel : ModelBase, ICloseWindows
    {
        private IAuthorService _authorService;
        public EditAuthorDialogViewModel(IAuthorService authorService, MainViewModel mainViewModel)
        {
            Authors = mainViewModel.Authors;
            _authorService = authorService;
            EditAuthorCommand = new DelegateCommand(OnEditAuthor);
            CancelButtonCommand = new DelegateCommand(OnCancelClick);
        }
        public ICommand EditAuthorCommand { get; }
        public ICommand CancelButtonCommand { get; }
        public Action Close { get; set; }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();

                if (_selectedAuthor != null)
                {
                    SelectedYear = _selectedAuthor.Birthdate.Year;
                    SelectedMonth = _selectedAuthor.Birthdate.Month;
                    SelectedDay = _selectedAuthor.Birthdate.Day;

                    OriginalFirstName = _selectedAuthor.Firstname;
                    OriginalLastName = _selectedAuthor.Lastname;
                    OriginalBirthDate = _selectedAuthor.Birthdate;
                }
            }
        }

        private string _originalFirstName;

        public string OriginalFirstName
        {
            get { return _originalFirstName; }
            set { _originalFirstName = value; }
        }

        private string _originalLastName;

        public string OriginalLastName
        {
            get { return _originalLastName; }
            set { _originalLastName = value; }
        }
        private DateOnly _originalBirthDate;

        public DateOnly OriginalBirthDate
        {
            get { return _originalBirthDate; }
            set
            {
                _originalBirthDate = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _birthDate;

        public DateOnly BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                Month = (Months)value;
                RaisePropertyChanged();
            }
        }

        private Months _month;

        public Months Month
        {
            get { return _month; }
            set
            {
                _month = value;
                DayComboBoxItemsSource = StaticMethods.UpdateDaysInMonth(_selectedYear, _month);
                RaisePropertyChanged();
            }
        }

        private int _selectedDay;

        public int SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                _selectedDay = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<int> _dayComboBoxItemsSource;

        public ObservableCollection<int> DayComboBoxItemsSource
        {
            get { return _dayComboBoxItemsSource; }
            set
            {
                _dayComboBoxItemsSource = value;
                RaisePropertyChanged();
            }
        }

        private bool _isSelected;
        public bool IsDeleteSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        private void OnEditAuthor(object obj)
        {
            if (IsDeleteSelected)
            {
                string messageBoxText = string.Format(StaticConstants.DELETE_WARNING, SelectedAuthor.FullName);
                    if (StaticMethods.ShowYesNoWarning(messageBoxText)) DeleteAuthorFromDB(SelectedAuthor);
                else IsDeleteSelected = !IsDeleteSelected;
            }
            if (!IsDeleteSelected)
            {
                if (SelectedAuthor == null)
                {
                    MessageBox.Show(StaticConstants.PICK_AUTH_EDIT);
                    return;
                }
                if (SelectedAuthor.Firstname == string.Empty || SelectedAuthor.Lastname == string.Empty || new[] { SelectedYear, (int)SelectedMonth, SelectedDay }.Any(val => val == 0))
                {
                    MessageBox.Show(StaticConstants.FILL_ALL_FIELDS);
                    return;
                }
                else
                {
                UpdateAuthorInformation(SelectedAuthor);

                Close?.Invoke();
                }
            }

            
        }

        private void UpdateAuthorInformation(Author author)
        {
            SelectedAuthor.Birthdate = StaticMethods.DateCreeator(SelectedYear, Month, SelectedDay);
            _authorService.EditAuthorAsync(SelectedAuthor);
        }

        private void DeleteAuthorFromDB(Author author)
        {
            var authorToRemove = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
            _authorService.DeleteAuthorAsync(authorToRemove);
        }

        private void OnCancelClick(object obj)
        {
            if (_selectedAuthor != null)
            {
                _selectedAuthor.Firstname = _originalFirstName;
                _selectedAuthor.Lastname = _originalLastName;
                _selectedAuthor.Birthdate = _originalBirthDate;
            }
            Close?.Invoke();
        }
    }
}
