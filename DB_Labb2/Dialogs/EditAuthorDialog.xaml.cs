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
public partial class EditAuthorDialog : Window
{
    public EditAuthorDialog(EditAuthorDialogViewModel editAuthorModel)
    {
        InitializeComponent();
        this.DataContext = editAuthorModel;
        YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
    }
}
