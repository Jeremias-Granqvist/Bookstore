using Bookstore.Dialogs;
using Bookstore.Service;
using Bookstore.Service.Interfaces;
using Bookstore;
using Microsoft.EntityFrameworkCore;
using Shared.Command;
using Shared.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Bookstore.viewModel;

public class MainViewModel : ModelBase, ICloseWindows
{
    private IEventDispatcher _eventDispatcher;

    private IAuthorService _authorService;
    private IBookService _bookService;
    private IInventoryService _inventoryService;
    private IStoreService _storeService;

    public MainViewModel(
        IEventDispatcher eventDispatcher, 
        IAuthorService authorService, 
        IBookService bookService, 
        IInventoryService inventoryService,
        IStoreService storeService)
    {
        _authorService = authorService;
        _eventDispatcher = eventDispatcher;
        _bookService = bookService;
        _inventoryService = inventoryService;
        _storeService = storeService;

        _authors = new ObservableCollection<Author>();
        _books = new ObservableCollection<Book>();

        AttachEvents();
        SetCommands();
        SetDataGrids();
    }

    private void SetCommands()
    {
        PressCancelButtonCommand = new DelegateCommand(OnCancelButtonPress);
        AddAuthorCommand = new DelegateCommand(OnAddAuthorClick);
        AlterAuthorCommand = new DelegateCommand(OnAlterAuthorClick);
        AddBookCommand = new DelegateCommand(OnAddBookClick);
        EditBookCommand = new DelegateCommand(OnEditBookClick);
        PressAddToInventoryCommand = new DelegateCommand(OnAddToInventoryClick);
        PressRemoveFromInventoryCommand = new DelegateCommand(OnRemoveFromInventoryClick);
        PressBookSearchCommand = new DelegateCommand(OnBookSearchClick);
        PressAuthorSearchCommand = new DelegateCommand(OnAuthorSearchClick);
        PressResetCommand = new DelegateCommand(OnResetFilterClick);
    }

    private void AttachEvents()
    {
        _eventDispatcher.EntityAddedEvent += OnEntityAdded;
        _eventDispatcher.EntityUpdatedEvent += OnEntityEdited;
        _eventDispatcher.EntityRemovedEvent += OnEntityDeleted;
        _eventDispatcher.EntityListEvent += OnEntityFetch;

    }

    //startup settings and commands
    public ICommand PressResetCommand { get; set; }
    public ICommand PressBookSearchCommand { get; set; }
    public ICommand PressAuthorSearchCommand { get; set; }
    public ICommand PressAddToInventoryCommand { get; set; }
    public ICommand PressRemoveFromInventoryCommand { get; set; }
    public ICommand PressCancelButtonCommand { get; set; }
    public ICommand AddAuthorCommand { get; set; }
    public ICommand AlterAuthorCommand { get; set; }
    public ICommand AddBookCommand { get; set; }
    public ICommand EditBookCommand { get; set; }
    public Action Close { get; set; }

    private ObservableCollection<Inventory> _inventories;
    public ObservableCollection<Inventory> Inventories
    {
        get { return _inventories; }
        set { _inventories = value; }
    }

    private ObservableCollection<Author> _authors;
    public ObservableCollection<Author> Authors
    {
        get => _authors;
        set
        {
            _authors = value;
            FilteredAuthors = _authors;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Book> _books;
    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            _books = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Store> _stores;
    public ObservableCollection<Store> Stores
    {
        get { return _stores; }
        set
        {
            _stores = value;
            RaisePropertyChanged();
        }
    }
    public async Task LoadDataAsync()
    {
        AttachEvents();
        SetCommands();
        SetDataGrids();
    }
    private async Task SetDataGrids()
    {
        var authors = await _authorService.GetAuthorsAsync();
        Authors = new ObservableCollection<Author>(authors);
        //FilteredAuthors = Authors;
        var books = await _bookService.GetBooksAsync();
        Books = new ObservableCollection<Book>(books);
        //FilteredBooks = Books;
        var inventories = await _inventoryService.GetInventoriesAsync();
        Inventories = new ObservableCollection<Inventory>(inventories);

        var stores = await _storeService.GetStoresAsync();
        Stores = new ObservableCollection<Store>(stores);

        FilteredAuthors = Authors;
        FilteredBooks = Books;
    
    }
    //Search handling

    private string _authorSearchString;

    public string AuthorSearchString
    {
        get { return _authorSearchString; }
        set 
        { 
            _authorSearchString = value;
            RaisePropertyChanged();
        }
    }


    private ObservableCollection<Author> _filteredAuthors;
    public ObservableCollection<Author> FilteredAuthors
    {
        get { return _filteredAuthors; }
        set 
        { 
            _filteredAuthors = value;
            RaisePropertyChanged();
        }
    }

    private string _bookSearchString;

    public string BookSearchString
    {
        get { return _bookSearchString; }
        set 
        { 
            _bookSearchString = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Book> _filteredBooks;
    public ObservableCollection<Book> FilteredBooks
    {
        get { return _filteredBooks; }
        set 
        { 
            _filteredBooks = value;
            RaisePropertyChanged();
        }
    }
    private void OnBookSearchClick(object obj)
    {
        var filteredBook = string.IsNullOrWhiteSpace(BookSearchString)
                        ? Books
                        : new ObservableCollection<Book>(Books.Where(b => b.Title.ToLower().Contains(BookSearchString.ToLower())));
        FilteredBooks = filteredBook;
    }

    private void OnAuthorSearchClick(object obj)
    {
        var filteredAuthor = string.IsNullOrWhiteSpace(AuthorSearchString)
                        ? Authors
                        : new ObservableCollection<Author>(Authors.Where(b => b.FullName.ToLower().Contains(AuthorSearchString.ToLower())));
        FilteredAuthors = filteredAuthor;
    }

    private void OnResetFilterClick(object obj)
    {
        SetDataGrids();
    }
    //Inventory updating

    private Store _selectedStore;

    public Store SelectedStore
    {
        get { return _selectedStore; }
        set
        {
            _selectedStore = value;
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
            RaisePropertyChanged();
        }
    }

    private int _addToAmount;

    public int AddToAmount
    {
        get { return _addToAmount; }
        set
        {
            _addToAmount = value;
            RaisePropertyChanged();
        }
    }

    private void OnAddToInventoryClick(object obj)
    {
        bool? existsInList = Inventories.Any(i => i.InventoryISBN13 == SelectedBook.ISBN13 && i.StoreID == SelectedStore.StoreID);

        if (existsInList == true)
        {
            foreach (var inventory in Inventories)
            {
                if (inventory.InventoryISBN13 == SelectedBook.ISBN13 && inventory.StoreID == SelectedStore.StoreID)
                {
                    inventory.Amount = inventory.Amount + AddToAmount;
                    RaisePropertyChanged();
                    _inventoryService.EditInventoryAsync(inventory);
                }
            }
        }
        else
        {
            var newAdditionToInventory = new Inventory() 
            { 
                Amount = AddToAmount, 
                StoreID = SelectedStore.StoreID, 
                store = SelectedStore, 
                book = SelectedBook, 
                InventoryISBN13 = SelectedBook.ISBN13 
            };
            RaisePropertyChanged("Inventories");
        _inventoryService.AddInventoryAsync(newAdditionToInventory);
        }
    }

    private void OnRemoveFromInventoryClick(object obj)
    {
        bool existsInList = Inventories.Any(i => i.InventoryISBN13 == SelectedBook.ISBN13 && i.StoreID == SelectedStore.StoreID);

        if (existsInList)
        {
            foreach (var inventory in Inventories)
            {
                if (inventory.InventoryISBN13 == SelectedBook.ISBN13 && inventory.StoreID == SelectedStore.StoreID)
                {
                    inventory.Amount = inventory.Amount - AddToAmount;
                    if (inventory.Amount < 0)
                    {
                        inventory.Amount = 0;
                        _inventoryService.DeleteInventoryAsync(inventory);
                    }
                    else
                    {
                    _inventoryService.EditInventoryAsync(inventory);
                    }
                    RaisePropertyChanged("Inventories");
                        break;
                }
            }
        }
        else
        {
            string messageBoxText = $"This book does not exist in your inventory, doublecheck your storename and book title.";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }
    }


    //DIALOGBOX OPENING
    #region Dialogbox Openers
    private void OnAddAuthorClick(object obj)
    {
        var addAuthorModel = new AddAuthorDialogViewModel(_authorService);
        var addAuthorWindow = new AddAuthorDialog(addAuthorModel)
        {
            DataContext = addAuthorModel
        };

        if (addAuthorWindow.DataContext is ICloseWindows dialogviewmodel)
        {
            dialogviewmodel.Close = new Action(addAuthorWindow.Close);
        }
        addAuthorWindow.Show();
    }

    private void OnAlterAuthorClick(object obj)
    {
        var editAuthorModel = new EditAuthorDialogViewModel(_authorService, this);
        var editAuthorWindow = new EditAuthorDialog(editAuthorModel)
        {
            DataContext = editAuthorModel
        };

        if (editAuthorWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(editAuthorWindow.Close);
        }
        editAuthorWindow.Show();
    }

    private void OnAddBookClick(object obj)
    {
        var addBookModel = new AddBookDialogViewModel(_bookService, this);
        var addBookWindow = new AddBookDialog(addBookModel)
        {
            DataContext = addBookModel
        };
        if (addBookWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(addBookWindow.Close);
        }
        addBookWindow.Show();
    }

    private void OnEditBookClick(object obj)
    {
        var editBookModel = new EditBookDialogViewModel(_bookService, this);
        var editBookWindow = new EditBookDialog(editBookModel)
        {
            DataContext = editBookModel
        };
        if (editBookWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(editBookWindow.Close);
        }
        editBookWindow.Show();
    }
    #endregion
    //EVENT HANDLING
    #region Event Handling
    private void OnEntityAdded(object? sender, object entity)
    {
        if (entity is Author author)
        {
            App.Current.Dispatcher.Invoke(() =>
            Authors.Add(author));
            RaisePropertyChanged(nameof(Authors));
        }
        else if (entity is Book book)
        {
            Books.Add(book);
            RaisePropertyChanged(nameof(Books));
        }
        else if (entity is Inventory inventory)
        {
            Inventories.Add(inventory);
            RaisePropertyChanged(nameof(Inventories));
        }
    }
    private void OnEntityEdited(object? sender, object entity)
    {
        if (entity is Author author)
        {
            var authorInCollection = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
            if (authorInCollection != null)
            {
                authorInCollection.Firstname = author.Firstname;
                authorInCollection.Lastname = author.Lastname;
                authorInCollection.Birthdate = author.Birthdate;
            }
            RaisePropertyChanged(nameof(Author));
            RaisePropertyChanged(nameof(Authors));

        }
        else if (entity is Book book)
        {
            var bookInCollection = Books.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
            if (bookInCollection != null)
            {
                bookInCollection.ISBN13 = book.ISBN13;
                bookInCollection.Title = book.Title;
                bookInCollection.ReleaseDate = book.ReleaseDate;
                bookInCollection.Language = book.Language;
                bookInCollection.Price = book.Price;
                bookInCollection.Authors = book.Authors;
            }
            RaisePropertyChanged(nameof(Books));
            RaisePropertyChanged(nameof(Authors));
        }
        else if (entity is Inventory inventory)
        {
            var inventoryInCollection = Inventories.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
            if (inventoryInCollection != null)
            {
                inventoryInCollection.Amount = inventory.Amount;
            }
            if (inventoryInCollection == null)
            {
                Inventories.Add(inventory);
            }
            RaisePropertyChanged(nameof(Inventories));
        }
    }
    private void OnEntityDeleted(object? sender, object entity)
    {
        if (entity is Author author)
        {
            var authorToDelete = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
            if (authorToDelete != null)
            {
                Authors.Remove(authorToDelete);
            }
            RaisePropertyChanged(nameof(Authors));
        }
        else if (entity is Book book)
        {
            var bookToDelete = Books.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
            if (bookToDelete != null)
            {
                Books.Remove(bookToDelete);
                RaisePropertyChanged(nameof(Books));
            }
        }
        else if (entity is Inventory inventory)
        {
            var inventoryToDelete = Inventories.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
            if (inventoryToDelete != null)
            {
                Inventories.Remove(inventoryToDelete);
                RaisePropertyChanged(nameof(Inventories));
            }
        }
    }
    private void OnEntityFetch(object? sender, object entity)
    {
        RaisePropertyChanged(nameof(Inventories));
        RaisePropertyChanged(nameof(Authors));
        RaisePropertyChanged(nameof(Books));
        RaisePropertyChanged(nameof(Stores));
    }

    private void OnCancelButtonPress(object obj)
    {
        Close?.Invoke();
    }
    #endregion
}
interface ICloseWindows
{
    Action Close { get; set; }
}
