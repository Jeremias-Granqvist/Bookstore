
using Bookstore.Dialogs;
using Shared.Model;
using Bookstore.viewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Bookstore.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        Loaded += async (s, e) => await viewModel.LoadDataAsync();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadCloseWindows();
    }
    private void LoadCloseWindows()
    {
        if (DataContext is ICloseWindows vm)
        {
                vm.Close = new Action(this.Close); 
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}