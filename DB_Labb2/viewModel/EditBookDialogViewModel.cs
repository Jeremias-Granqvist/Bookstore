using Shared.Command;
using Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Bookstore.Service.Interfaces;
using Shared.Statics;

namespace Bookstore.viewModel
{
    public class EditBookDialogViewModel : ModelBase, ICloseWindows
    {
        private IBookService _bookService;
        public EditBookDialogViewModel(IBookService bookService, MainViewModel mainViewModel)
        {
            _bookService = bookService;
            Books = mainViewModel.Books;
            Authors = mainViewModel.Authors;


            CancelButtonCommand = new DelegateCommand(OnCancelClick);
            SaveButtonCommand = new DelegateCommand(OnSaveClick);
        }




        public ICommand SaveButtonCommand { get; }
        public ICommand CancelButtonCommand { get; }

        public Book LoadBookForEditing(Book book)
        {
            _bookService.EditBookAsync(book);
            SelectedAuthor.Clear();
            foreach (var author in book.Authors)
            {
                SelectedAuthor.Add(author);
            }
            return book;
        }
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }


        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;

                if (_selectedBook != null)
                {

                    SelectedYear = _selectedBook.ReleaseDate.Year;
                    SelectedMonth = _selectedBook.ReleaseDate.Month;
                    SelectedDay = _selectedBook.ReleaseDate.Day;
                    
                    _selectedBook = LoadBookForEditing(SelectedBook);

                    _originalISBN = _selectedBook.ISBN13;
                    _originalTitle = _selectedBook.Title;
                    _originalLanguage = _selectedBook.Language;
                    _originalPrice = _selectedBook.Price;
                    _originialReleaseDate = _selectedBook.ReleaseDate;
                    _originalAuthor = _selectedBook.Authors;

                }
                RaisePropertyChanged();
            }
        }




        private Author _selectedAuthorForBook;
        public Author SelectedAuthorForBook
        {
            get { return _selectedAuthorForBook; }
            set
            {
                _selectedAuthorForBook = value;

                if (_selectedAuthor.Count == 0)
                {
                    _selectedAuthor.Add(SelectedAuthorForBook);
                }
                RaisePropertyChanged();
            }
        }

        private ICollection<Author> _selectedAuthor = new List<Author>();

        public ICollection<Author> SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
            }
        }

        private ICollection<Author> _originalAuthor;
        public ICollection<Author> OriginalAuthor
        {
            get { return _originalAuthor; }
            set
            {
                _originalAuthor = value;
                RaisePropertyChanged();
            }
        }


        private long _originalISBN;
        public long OriginalISBN
        {
            get { return _originalISBN; }
            set
            {
                _originalISBN = value;
                RaisePropertyChanged();
            }
        }

        private string _originalTitle;
        public string OriginalTitle
        {
            get { return _originalTitle; }
            set
            {
                _originalTitle = value;
                RaisePropertyChanged();
            }
        }

        private int _originalPrice;
        public int OriginalPrice
        {
            get { return _originalPrice; }
            set
            {
                _originalPrice = value;
                RaisePropertyChanged();
            }
        }

        private string _originalLanguage;
        public string OriginalLanguage
        {
            get { return _originalLanguage; }
            set
            {
                _originalLanguage = value;
                RaisePropertyChanged();
            }
        }
        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _originialReleaseDate;
        public DateOnly OriginalReleaseDate
        {
            get { return _originialReleaseDate; }
            set
            {
                _originialReleaseDate = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _selectedReleaseDate;
        public DateOnly SelectedReleaseDate
        {
            get { return _selectedReleaseDate; }
            set
            {
                _selectedReleaseDate = value;
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
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        private void OnSaveClick(object obj)
        {
            if (IsSelected)
            {
                string messageBoxText = string.Format(StaticConstants.DELETE_WARNING, SelectedBook.Title);
                if (StaticMethods.ShowYesNoWarning(messageBoxText))
                {
                    DeleteBookFromDB(SelectedBook);
                }
            }
            if (!IsSelected)
            {
                if (SelectedBook == null)
                {
                    MessageBox.Show(StaticConstants.PICK_BOOK_EDIT);
                    return;
                }
                if (StaticMethods.IsValidISBN13(SelectedBook.ISBN13))
                {
                    MessageBox.Show(StaticConstants.INVALID_ISBN);
                    return;

                }
                else if (SelectedBook.Title == string.Empty || SelectedBook.Language == string.Empty || SelectedBook.Price == 0)
                {
                    StaticMethods.ShowError();
                }
                else
                {
                UpdateBookInformation(SelectedBook);
                Close?.Invoke();
                }
            }
        }

        private void UpdateBookInformation(Book book)
        {
            SelectedBook.ReleaseDate = StaticMethods.DateCreeator(_selectedYear, Month, _selectedDay);             
            SelectedBook.Authors = SelectedAuthor;
            _bookService.EditBookAsync(SelectedBook);
        }
        private void DeleteBookFromDB(Book book)
        {
            var bookToRemove = Books.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
            _bookService.DeleteBookAsync(bookToRemove);
        }
        private void OnCancelClick(object obj)
        {
            if (_selectedBook != null)
            {
                _selectedBook.ISBN13 = _originalISBN;
                _selectedBook.Title = _originalTitle;
                _selectedBook.Language = _originalLanguage;
                _selectedBook.Price = _originalPrice;
                _selectedReleaseDate = _originialReleaseDate;
                _selectedAuthor = _originalAuthor;
            }
            Close?.Invoke();
        }

        public Action Close { get; set; }
    }
}
