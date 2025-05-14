using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class BookShopManagerment : Form
    {
        public BookShopManagerment()
        {
            InitializeComponent();
        }
        int StartPoint = 0;
        private void time_tick_Tick(object sender, EventArgs e)
        {
            StartPoint += 1;
            ProgressBar1.Value = StartPoint;
            lblPercent.Text = StartPoint + "%";
            if (ProgressBar1.Value == 100)
            {
                ProgressBar1.Value = 0;
                time_tick.Stop();
                UserLogin Obj = new UserLogin();
                Obj.Show();
                this.Hide();
            }
        } 
    }
}
