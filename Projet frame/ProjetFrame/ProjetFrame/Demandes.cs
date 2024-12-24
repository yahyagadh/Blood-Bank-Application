using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetFrame
{
    public partial class Demandes : Form
    {
        private BindingSource HopitalEtCliniqueBindingSource = new BindingSource();
        public Demandes()
        {
            InitializeComponent();
        }

        private void Demandes_Load(object sender, EventArgs e)
        {
            DisplayDemandes();
        }
       private void DisplayDemandes()
        {
            try
            {
                string Query = "SELECT * FROM HopitalEtClinique";
                using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                {
                    cnx.Open();
                    SqlDataAdapter sqlData = new SqlDataAdapter(Query, cnx);
                    using (sqlData)
                    {
                        DataTable DemandesTable = new DataTable();
                        sqlData.Fill(DemandesTable);
                        // Associer DataTable au BindingSource
                        HopitalEtCliniqueBindingSource.DataSource = DemandesTable;
                        //afficher les données de la table dans la DataGrid dgv1
                        ADGV1.DataSource = DemandesTable;
                        ADGV1.AutoGenerateColumns = false;
                    } // fin du using sqlData
                } // fin du using cnx
            } // fin du try
            catch (Exception ex)
            {
                // Gérer l'exception ici
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Reetour a la first page
            First_page FirstPage = new First_page();
            FirstPage.Show();
            this.Hide();
        }

        private void ADGV1_SortStringChanged(object sender, EventArgs e)
        {
            this.HopitalEtCliniqueBindingSource.Sort = this.ADGV1.SortString;

        }

        private void ADGV1_FilterStringChanged(object sender, EventArgs e)
        {
            this.HopitalEtCliniqueBindingSource.Filter = this.ADGV1.FilterString;

        }
        private void HopitalEtCliniqueBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            MessageBox.Show(HopitalEtCliniqueBindingSource.List.Count.ToString());
        }

        private void ADGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            // Passer à la page de Menu
            Menu MenuPage = new Menu();
            MenuPage.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Passer à la page de Menu
            Centre CentrePage = new Centre();
            CentrePage.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (ADGV1.SelectedRows.Count > 0)
            {
                // Récupérer l'ID  sélectionné dans le DataGridView
                int idDemande = Convert.ToInt32(ADGV1.SelectedRows[0].Cells["ID"].Value);

                // Confirmation de la suppression
                DialogResult result = MessageBox.Show("Voulez-vous vraiment réfuser cette demande ?", "Confirmation de supression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Supprimer le donneur de la base de données
                    using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                    {
                        //supprimer de la table de donneur
                        string query = "DELETE FROM HopitalEtClinique WHERE ID = @ID";

                        using (SqlCommand cmd = new SqlCommand(query, cnx))
                        {
                            cmd.Parameters.AddWithValue("@ID", idDemande);

                            cnx.Open();
                            cmd.ExecuteNonQuery();
                            /* cnx.Close();*/
                        }
                        cnx.Close();

                    }


                    // Supprimer la ligne sélectionnée du DataGridView
                    ADGV1.Rows.RemoveAt(ADGV1.SelectedRows[0].Index);

                    MessageBox.Show("Ligne supprimée avec succès !", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une à supprimer.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //Button Accepter
        private void button3_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (ADGV1.SelectedRows.Count > 0)
            {
                // Récupérer les informations de la demande sélectionnée dans le DataGridView
                int idDemande = Convert.ToInt32(ADGV1.SelectedRows[0].Cells["ID"].Value);
                string groupeSanguinDemande = ADGV1.SelectedRows[0].Cells["GroupSanguinRequis"].Value.ToString();
                int quantiteDemande = Convert.ToInt32(ADGV1.SelectedRows[0].Cells["QuantiteRequis"].Value);

                // Connexion à la base de données
                using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                {
                    cnx.Open();

                    // Vérifier s'il y a une quantité de sang équivalente dans la table DonneurDeSang
                    string query = "SELECT COUNT(*) FROM DonneurDeSang WHERE GroupeSanguin = @GroupeSanguin AND QuantiteSang >= @QuantiteSang";
                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.Parameters.AddWithValue("@GroupeSanguin", groupeSanguinDemande);
                        cmd.Parameters.AddWithValue("@QuantiteSang", quantiteDemande);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Supprimer la quantité donnée de la table DonneurDeSang
                            query = "DELETE FROM DonneurDeSang WHERE GroupeSanguin = @GroupeSanguin AND QuantiteSang >= @QuantiteSang";
                            using (SqlCommand deleteCmd = new SqlCommand(query, cnx))
                            {
                                deleteCmd.Parameters.AddWithValue("@GroupeSanguin", groupeSanguinDemande);
                                deleteCmd.Parameters.AddWithValue("@QuantiteSang", quantiteDemande);
                                deleteCmd.ExecuteNonQuery();
                            }

                            // Supprimer la demande de la table HopitalEtClinique
                            query = "DELETE FROM HopitalEtClinique WHERE ID = @ID";
                            using (SqlCommand deleteCmd = new SqlCommand(query, cnx))
                            {
                                deleteCmd.Parameters.AddWithValue("@ID", idDemande);
                                deleteCmd.ExecuteNonQuery();
                            }

                            // Supprimer la ligne sélectionnée du DataGridView
                            ADGV1.Rows.RemoveAt(ADGV1.SelectedRows[0].Index);

                            MessageBox.Show("Demande acceptée !", "Acceptation réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("La quantité de sang demandée n'est pas disponible.", "Quantité insuffisante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une demande à accepter.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
