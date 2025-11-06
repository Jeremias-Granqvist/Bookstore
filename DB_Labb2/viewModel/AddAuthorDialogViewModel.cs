using Bookstore.Service;
using Bookstore.Service.Interfaces;
using Shared.Command;
using Shared.Model;
using Shared.Statics;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.viewModel;

public class AddAuthorDialogViewModel : ModelBase, ICloseWindows
{

    private IAuthorService _authorService;
    public AddAuthorDialogViewModel(IAuthorService authorService)
    {
        _authorService = authorService;
        AddAuthorCommand = new DelegateCommand(OnAddAuthor);
        CancelButtonCommand = new DelegateCommand(OnCancelClick);
    }


    public ICommand CancelButtonCommand { get; }
    public ICommand AddAuthorCommand { get; }
    public Action Close { get; set; }

    private void OnCancelClick(object obj)
    {
        Close?.Invoke();
    }


    private string _firstname;
    public string Firstname
    {
        get => _firstname;
        set
        {
            _firstname = value;
            RaisePropertyChanged();
        }
    }

    private string _lastname;
    public string Lastname
    {
        get => _lastname;
        set
        {
            _lastname = value;
            RaisePropertyChanged();
        }
    }

    private int _year;
    public int Year
    {
        get => _year;
        set
        {
            _year = value;
            RaisePropertyChanged();
        }
    }

    private Months _month;
    public Months Month
    {
        get => _month;
        set
        {
            _month = value;
            RaisePropertyChanged();
            DayComboBoxItemsSource = StaticMethods.UpdateDaysInMonth(_year, _month);
            RaisePropertyChanged();
            RaisePropertyChanged("DayComboBoxItemsSource");
        }
    }

    private int _day;
    public int Day
    {
        get => _day;
        set
        {
            _day = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<int> _dayComboBoxItemsSource;
    public ObservableCollection<int> DayComboBoxItemsSource
    {
        get => _dayComboBoxItemsSource;
        set
        {
            _dayComboBoxItemsSource = value;
            RaisePropertyChanged();
        }
    }


    private void OnAddAuthor(object obj)
    {
        if (string.IsNullOrWhiteSpace(Firstname) || string.IsNullOrWhiteSpace(Lastname) || new [] { Year, (int)Month, Day }.Any(val => val <= 0))
        {
            MessageBox.Show("Please fill out all fields.");
            return;
        }
        var selectedDate = StaticMethods.DateCreeator(Year, Month, Day);
        
        Author newAuthor = new Author
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Birthdate = selectedDate
        };

        _authorService.AddAuthorAsync(newAuthor);
        Close?.Invoke();
    }





}
