using System.Linq;
using System.Windows.Forms;

namespace QLKS.Forms
{
    public partial class FormInvoice : Form
    {
        DbContext db = new DbContext(DbContext.ConnectionType.ConfigurationManager, "DefaultConnection");

        public FormInvoice()
        {
            InitializeComponent();
            dataView.AutoGenerateColumns = false;
            dataView.DataSource = ViewModels.InvoiceViewModel.GetInvoices(db).ToList();
        }
    }
}
