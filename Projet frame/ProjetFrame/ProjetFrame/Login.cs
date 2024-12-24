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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            // Masquer le texte saisi dans le champ de mot de passe
            mdptextbox.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gunaImageCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //les controles de saisie
            if (codetextbox.Text == "centrededons" && mdptextbox.Text == "1234")
            {
                MessageBox.Show("Connexion réussie !");
                //passer a la page de Menu
                Menu MenuPage = new Menu();
                MenuPage.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Code ou mot de passe incorrect !");
            }


        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
