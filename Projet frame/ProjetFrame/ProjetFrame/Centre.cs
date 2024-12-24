using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetFrame
{
    public partial class Centre : Form
    {
        public Centre()
        {
            InitializeComponent();
        }

        private void Centre_Load(object sender, EventArgs e)
        {
            DisplayStock();
            VerifierNiveauxStock();
            VerifierExpirationSang();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Passer à la page de login
            Login LoginPage = new Login();
            LoginPage.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Passer à la page de demandes
            Demandes DemandesPage = new Demandes();
            DemandesPage.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Passer à la page de Hopital
            Hôpital HopitalPage = new Hôpital();
            HopitalPage.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Passer à la page de centre
            Donneur DonneurPage = new Donneur();
            DonneurPage.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Passer à la first page
            First_page FirstPage = new First_page();
            FirstPage.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            //Reetour a la first page
            First_page FirstPage  = new First_page();
            FirstPage.Show();
            this.Hide();
        }
        
        private void DisplayStock()
        {
            try
            {
                //date d'expiration aprés 10jours de date de don
                string Query = "SELECT ID,GroupeSanguin,QuantiteSang,DateDon,DATEADD(day, 10, DateDon) AS DateExpiration FROM DonneurDeSang";
                using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                {
                    cnx.Open();
                    SqlDataAdapter sqlData = new SqlDataAdapter(Query, cnx);
                    using (sqlData)
                    {
                        DataTable DemandesTable = new DataTable();
                        sqlData.Fill(DemandesTable);
                        // Associer DataTable au BindingSource
                        //afficher les données de la table dans la DataGrid dgv1
                        dataGridView1.DataSource = DemandesTable;
                        dataGridView1.AutoGenerateColumns = false;
                    } 
                } 
            } 
            catch (Exception ex)
            {
                // Gérer l'exception ici
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }
        //fonction d'alerte pour signaler les niveaux des stocksbas 
        private void VerifierNiveauxStock()
        {
            // Parcourir la DataGridView des stocks
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["GroupeSanguin"].Value != null)
                {

                    // Vérifier le niveau de stock bas (par exemple, seuil à 10 unités)
                    int seuilStock = 5;
                    int stockDisponible = Convert.ToInt32(row.Cells["QuantiteSang"].Value);
                    if (stockDisponible < seuilStock)
                    {
                        // Afficher une alerte sur le niveau de stock bas
                        MessageBox.Show("Attention ! Niveau de stock bas pour le groupe sanguin " + row.Cells["GroupeSanguin"].Value.ToString(), "Alerte de stock bas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        //fonction d'alerte pour signaler les unités de sang arrivent a expiration
        private void VerifierExpirationSang()
        {
            // Date actuelle
            DateTime dateSysteme = DateTime.Now;

            // Parcourir la DataGridView des stocks
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Vérifier si la date d'expiration est proche
                DateTime dateExpiration = Convert.ToDateTime(row.Cells["DateExpiration"].Value);
                TimeSpan difference = dateExpiration - dateSysteme;
                int joursRestants = difference.Days;

                // Vérifier si la date d'expiration 
                if (joursRestants <= 2 && joursRestants >= 0)
                {
                    // Afficher une alerte sur l'expiration imminente
                    MessageBox.Show("Attention ! L'unité de sang expirera dans " + joursRestants + " jours.", "Alerte d'expiration imminente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Vous pouvez ajouter d'autres actions ici, comme retirer l'unité de sang du stock, etc.
                }
            }
        }
    

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void SortDataGridViewByDateExpiration()
        {
            // Vérifier si la DataGridView contient des lignes
            if (dataGridView1.Rows.Count > 0)
            {
                // Tri par la colonne DateExpiration (index de colonne 4) dans l'ordre croissant
                dataGridView1.Sort(dataGridView1.Columns["DateExpiration"], ListSortDirection.Ascending);
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            // Passer à la page de Menu
            Menu MenuPage = new Menu();
            MenuPage.Show();
            this.Hide();
        }

        private void GroupesangBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //button rechercher
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Vérifier si un élément est sélectionné dans le ComboBox
                if (GroupesanguinBox.SelectedItem != null)
                {
                    // Construire la requête SQL en fonction de la sélection dans le ComboBox
                    string groupeSanguinSelectionne = GroupesanguinBox.SelectedItem.ToString();
                    string Query = "SELECT ID, GroupeSanguin, QuantiteSang, DateDon, DATEADD(day, 10, DateDon) AS DateExpiration FROM DonneurDeSang WHERE GroupeSanguin = @GroupeSanguin";

                    // Vérifier si la quantité est spécifiée
                    if (!string.IsNullOrEmpty(quantitebox.Text))
                    {
                        // Ajouter la condition pour la quantité dans la requête SQL
                        Query += " AND QuantiteSang >= @QuantiteSang";
                    }

                    // Connexion à la base de données
                    using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                    {
                        cnx.Open();

                        // Créer un adaptateur de données
                        using (SqlDataAdapter sqlData = new SqlDataAdapter(Query, cnx))
                        {
                            // Ajouter les paramètres pour le groupe sanguin sélectionné et la quantité (si spécifiée)
                            sqlData.SelectCommand.Parameters.AddWithValue("@GroupeSanguin", groupeSanguinSelectionne);
                            if (!string.IsNullOrEmpty(quantitebox.Text))
                            {
                                sqlData.SelectCommand.Parameters.AddWithValue("@QuantiteSang", Convert.ToInt32(quantitebox.Text));
                            }

                            // Remplir les données dans un DataTable
                            DataTable DemandesTable = new DataTable();
                            sqlData.Fill(DemandesTable);

                            // Lier le DataTable à la DataGridView
                            dataGridView1.DataSource = DemandesTable;
                            dataGridView1.AutoGenerateColumns = false;
                        }
                    }
                }
                else
                {
                    // Afficher un message si aucun groupe sanguin n'est sélectionné
                    MessageBox.Show("Veuillez sélectionner un groupe sanguin dans la liste.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions
                MessageBox.Show("Une erreur s'est produite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //button Refresh
        private void button2_Click(object sender, EventArgs e)
        {
                try
                {
                    // Connexion à la base de données
                    using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                    {
                        cnx.Open();

                        // Requête SQL pour sélectionner toutes les données
                        string query = "SELECT ID, GroupeSanguin, QuantiteSang, DateDon, DATEADD(day, 10, DateDon) AS DateExpiration FROM DonneurDeSang";

                        // Créer un adaptateur de données
                        using (SqlDataAdapter sqlData = new SqlDataAdapter(query, cnx))
                        {
                            // Remplir les données dans un DataTable
                            DataTable DemandesTable = new DataTable();
                            sqlData.Fill(DemandesTable);

                            // Lier le DataTable à la DataGridView
                            dataGridView1.DataSource = DemandesTable;
                            dataGridView1.AutoGenerateColumns = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gérer l'exception ici
                    MessageBox.Show("Une erreur s'est produite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        //button supprimer

        private void button3_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérer l'ID du donneur sélectionné dans le DataGridView
                int idDonneur = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                // Confirmation de la suppression
                DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet ligne ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Supprimer le donneur de la base de données
                    using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                    {
                        //supprimer de la table de donneur
                        string query = "DELETE FROM DonneurDeSang WHERE ID = @ID";

                        using (SqlCommand cmd = new SqlCommand(query, cnx))
                        {
                            cmd.Parameters.AddWithValue("@ID", idDonneur);

                            cnx.Open();
                            cmd.ExecuteNonQuery();
                            /* cnx.Close();*/
                        }
                        cnx.Close();

                    }


                    // Supprimer la ligne sélectionnée du DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    MessageBox.Show("Ligne supprimée avec succès !", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une à supprimer.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //button Trier
        private void button4_Click(object sender, EventArgs e)
        {
            SortDataGridViewByDateExpiration();

        }

        private void label9_Click(object sender, EventArgs e)
        {
        }
        //fin
    }
}
