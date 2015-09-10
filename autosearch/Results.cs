using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autosearch
{
    public partial class Results : Form
    {

        public Results()
        {
            InitializeComponent();
        }

        private void Results_Load(object sender, EventArgs e)
        {
        }

        public void setSearch(String kz) {
            this.txtSearch.Text = kz;
            try
            {
                browser.Navigate("http://google.com/search?q=" + kz);
            }
            catch (Exception e) { }
        }
    }
}
