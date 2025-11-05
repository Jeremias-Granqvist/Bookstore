using Bookstore.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bookstore.Dialogs;
public partial class EditBookDialog : Window
{
    public EditBookDialog(EditBookDialogViewModel editBookDialog)
    {
        InitializeComponent();
        this.DataContext = editBookDialog;
        YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
    }
    
    private void ISBNTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {

        e.Handled = !IsDigit(e.Text);
        
    }

    private bool IsDigit(string text)
    {
        return text.All(char.IsDigit);
    }
}
