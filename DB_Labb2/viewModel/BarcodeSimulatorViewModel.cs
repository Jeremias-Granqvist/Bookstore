using Bookstore.SimulatingBarCode;
using Grpc.Net.Client;
using Shared.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

public class BarcodeSimulatorViewModel : INotifyPropertyChanged
{
    private BarcodeSections _sections = new();
    private ObservableCollection<string> _currentBarcodes = new();
    private bool _isSectionsViewVisible;

    public ObservableCollection<string> CurrentBarcodes
    {
        get => _currentBarcodes;
        set { _currentBarcodes = value; OnPropertyChanged(); }
    }

    public bool IsSectionsViewVisible
    {
        get => _isSectionsViewVisible;
        set { _isSectionsViewVisible = value; OnPropertyChanged(); }
    }

    public ICommand SwapViewCommand { get; }
    public ICommand SectionCommand { get; }
    public ICommand RevertCommand { get; }
    public ICommand PurchaseCommand { get; }

    public BarcodeSimulatorViewModel()
    {
        LoadSections();

        SwapViewCommand = new DelegateCommand(_ => IsSectionsViewVisible = !IsSectionsViewVisible);
        SectionCommand = new DelegateCommand(param => LoadSection(param?.ToString() ?? ""));
        RevertCommand = new DelegateCommand(_ => CurrentBarcodes.Clear());
        PurchaseCommand = new DelegateCommand(async _ => await PurchaseCurrentBarcodes());
    }

    private void LoadSections()
    {
        var json = File.ReadAllText("barcodes.json");
        _sections = JsonSerializer.Deserialize<BarcodeSections>(json) ?? new BarcodeSections();
    }

    private void LoadSection(string sectionName)
    {
        CurrentBarcodes.Clear();
        List<BarcodeScan> section = sectionName switch
        {
            "AllBarcodes" => _sections.AllBarcodes,
            "NewArrivals" => _sections.NewArrivals,
            "Bestsellers" => _sections.Bestsellers,
            "BrokenBarcode" => _sections.BrokenBarcode,
            _ => new List<BarcodeScan>()
        };

        foreach (var scan in section)
        {
            CurrentBarcodes.Add(scan.Barcode);
        }
    }

    private async Task PurchaseCurrentBarcodes()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      //  var client = new BarcodeScannerServiceClient(channel);

        foreach (var barcode in CurrentBarcodes)
        {
       //     var response = await client.ScanBarcodeAsync(new BarcodeScanRequest { Barcode = barcode });
        //    if (response.Success)
            {
                // Reduce inventory
               // BookManager.DecreaseInventory(response.Book.Isbn13, 1);
            }
        }

        CurrentBarcodes.Clear();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
