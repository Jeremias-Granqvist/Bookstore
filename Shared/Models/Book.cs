using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Model;
public class Book : ModelBase
{
    private long _isbn13;
    public long ISBN13 
    {
        get => _isbn13; 
        set
        {
            _isbn13 = value;
            RaisePropertyChanged();
        } 
    }
    private string _title;
    public string? Title 
    { 
        get => _title; 
        set
        {
            _title = value;
            RaisePropertyChanged();
        } 
    }
    private int _price;
    public int Price 
    { 
        get => _price;
        set 
        {
            _price = value;
            RaisePropertyChanged();
        }
    }
    private string? _language;
    public string? Language 
    { 
        get => _language; 
        set
        {
            _language = value;
            RaisePropertyChanged();
        } 
    }

    private DateOnly _releaseDate;
    [Column("Release Date")]
    public DateOnly ReleaseDate 
    { get => _releaseDate; 
        set
        {
            _releaseDate = value;
            RaisePropertyChanged();
        } 
    }

    public ICollection<Author> Authors { get; set; } = new List<Author>();
    [JsonIgnore]
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<Inventory> inventories { get; set; } = new List<Inventory>();
}
