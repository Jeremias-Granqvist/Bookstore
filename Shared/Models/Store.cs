using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Model;

public class Store : ModelBase
{
    private int _storeID;
    public int StoreID
    {
        get { return _storeID; }
        set
        {
            _storeID = value;

        }
    }

    private string _storeName;
    [Column("Store Name")]
    public string StoreName
    {
        get { return _storeName; }
        set
        {
            _storeName = value;
            RaisePropertyChanged();
        }
    }

    private string _adress;
    public string Adress
    {
        get { return _adress; }
        set
        {
            _adress = value;
            RaisePropertyChanged();
        }
    }

    private int _zipCode;
    [Column("Zip code")]
    public int ZipCode
    {
        get { return _zipCode; }
        set
        {
            _zipCode = value;
            RaisePropertyChanged();
        }
    }

    private string _city;
    public string City
    {
        get { return _city; }
        set
        {
            _city = value;
            RaisePropertyChanged();
        }
    }
}

