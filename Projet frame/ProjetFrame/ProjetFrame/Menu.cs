using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetFrame
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Passer à la page Donneur
            Donneur DonneurPage = new Donneur();
            DonneurPage.Show();
            this.Hide();
        }

        private void gunaPictureBox2_Click(object sender, EventArgs e)
        {
            // Passer à la page de centre
            Centre CentrePage = new Centre();
            CentrePage.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Passer à la page Hopital
            Hôpital HôpitalPage = new Hôpital();
            HôpitalPage.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Reetour a la first page
            First_page FirstPage = new First_page();
            FirstPage.Show();
            this.Hide();
        }
    }
}
