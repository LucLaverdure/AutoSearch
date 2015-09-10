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
        private String lastS = "";
        public Results()
        {
            InitializeComponent();
        }

        private void Results_Load(object sender, EventArgs e)
        {
        }

        public void setSearch(String kz) {
            this.txtSearch.Text = kz;
        }

        private void timNavigate_Tick(object sender, EventArgs e)
        {
            if (this.txtSearch.Text != lastS)
            {
                try
                {
                    browser.Navigate("http://google.com/search?q=" + this.txtSearch.Text);
                    lastS = this.txtSearch.Text;
                }
                catch (Exception e) { }
            }
        }
    }
}
