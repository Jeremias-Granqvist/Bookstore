using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.SimulatingBarCode
{
    public class BarcodeScan
    {
        public string Barcode { get; set; } = string.Empty;
    }

    public class BarcodeSections
    {
        public List<BarcodeScan> AllBarcodes { get; set; } = new();
        public List<BarcodeScan> NewArrivals { get; set; } = new();
        public List<BarcodeScan> Bestsellers { get; set; } = new();
        public List<BarcodeScan> BrokenBarcode { get; set; } = new();



    }
}
