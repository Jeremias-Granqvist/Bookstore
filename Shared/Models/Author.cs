using System.Text.Json.Serialization;

namespace Shared.Model;

public class Author : ModelBase
{
    //PK
    public int AuthorID { get; set; }
    
    private string _firstName;
    public required string Firstname 
    { 
        get => _firstName; 
        set 
        { 
            _firstName = value; 
            RaisePropertyChanged(); 
        } 
    }
    private string _lastName;
    public required string Lastname 
    { 
        get => _lastName; 
        set
        {
            _lastName = value;
            RaisePropertyChanged();
        } 
    }
    private DateOnly _birthDate;
    public DateOnly Birthdate 
    {
        get => _birthDate;
        set 
        {
            _birthDate = value;
            RaisePropertyChanged();
        } 
    }

    public override bool Equals(object obj)
    {
        return obj is Author author && AuthorID == author.AuthorID;
    }

    public override int GetHashCode()
    {
        return AuthorID.GetHashCode();
    }
    public string FullName => $"{Firstname} {Lastname}";
    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

}

