using Guna.UI2.WinForms;
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
    public partial class Hôpital : Form
    {
        public Hôpital()
        {
            InitializeComponent();
        }

        private void Hôpital_Load(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        //--------------------button ajouter-------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            // Vérification du nom
            if (string.IsNullOrWhiteSpace(NomHbox.Text))
            {
                MessageBox.Show("Veuillez saisir le nom de l'hopital/clinique.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du l'adresse
            if (string.IsNullOrWhiteSpace(AdresseHbox.Text))
            {
                MessageBox.Show("Veuillez saisir l'adresse de l'hopital/clinique.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //check les checked boxes
            if (NombresCasesCochées() == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins un type de sang", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (quantités_selectionnés() != NombresCasesCochées())
            {
                MessageBox.Show("Veuillez saisir la quantité pour chaque type de sang sélectionné", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NombresComboBoxCorrespondantes() != NombresCasesCochées())
            {
                MessageBox.Show("Veuillez sélectionner un degré d'urgence pour chaque type de sang", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            /*string groupSanguin = "";
            float quantité;*/

            // Récupérer le contenu du CheckBox sélectionné pour le groupe sanguin
            if (ABpluscheck.Checked)
            {
                string groupeSanguin = "AB+";
                //-------------
                float quantité;
                if (float.TryParse(ABplusbox.Text, out quantité))
                {
                    string degreUrgence = ABpluscombobox.SelectedItem != null ? ABpluscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin AB+", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //---------------
                /*float quantité = ABplusbox.Value;
                string degreUrgence = ABpluscombobox.SelectedItem != null ? ABpluscombobox.SelectedItem.ToString() : "";
                InsererDonnees(groupeSanguin, quantité, degreUrgence);*/
            }
            else if (ABmoinscheck.Checked)
            {
                string groupeSanguin = "AB-";
                float quantité;
                if (float.TryParse(ABmoinsbox.Text, out quantité))
                {
                    string degreUrgence = ABmoinscombobox.SelectedItem != null ? ABmoinscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin AB-", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Apluscheck.Checked)
            {
                string groupeSanguin = "A+";
                float quantité;
                if (float.TryParse(Aplusbox.Text, out quantité))
                {
                    /*string degreUrgence = Apluscombobox.SelectedItem != null ? ABpluscombobox.SelectedItem.ToString() : "";*/
                    string degreUrgence = Apluscombobox.SelectedItem?.ToString() ?? "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin A+", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Amoinscheck.Checked)
            {
                string groupeSanguin = "A-";
                float quantité;
                if (float.TryParse(Amoinsbox.Text, out quantité))
                {
                    string degreUrgence = Amoinscombobox.SelectedItem != null ? Amoinscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin A-", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Bpluscheck.Checked)
            {
                string groupeSanguin = "B+";
                float quantité;
                if (float.TryParse(Bplusbox.Text, out quantité))
                {
                    string degreUrgence = Bpluscombobox.SelectedItem != null ? Bpluscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin B+", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Bmoinscheck.Checked)
            {
                string groupeSanguin = "B-";
                float quantité;
                if (float.TryParse(Bmoinsbox.Text, out quantité))
                {
                    string degreUrgence = Bmoinscombobox.SelectedItem != null ? Bmoinscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin B-", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Opluscheck.Checked)
            {
                string groupeSanguin = "O+";
                float quantité;
                if (float.TryParse(ABplusbox.Text, out quantité))
                {
                    string degreUrgence = ABpluscombobox.SelectedItem != null ? ABpluscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin O+", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Omoinscheck.Checked)
            {
                string groupeSanguin = "O-";
                float quantité;
                if (float.TryParse(ABplusbox.Text, out quantité))
                {
                    string degreUrgence = ABpluscombobox.SelectedItem != null ? ABpluscombobox.SelectedItem.ToString() : "";
                    InsererDonnees(groupeSanguin, quantité, degreUrgence);
                }
                else
                {
                    MessageBox.Show("Veuillez saisir une quantité valide pour le groupe sanguin O-", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ChargerDataGridView();

        }
        //----------------------LES FONCTIONS DU BUTTONS---------------------------------------------------------------------------
        private int NombresCasesCochées()
        {
            int nombreCasesCochées = 0;

            // Parcourir tous les contrôles sur le formulaire
            foreach (Control control in Controls)
            {
                // Vérifier si le contrôle est une case à cocher et si elle est cochée
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    nombreCasesCochées++;
                }
            }

            return nombreCasesCochées;
        }
        //-----------------2---------------------------------------
        private int NombresComboBoxCorrespondantes()
        {
            CheckBox[] checkBoxes = { ABpluscheck, ABmoinscheck,Apluscheck,Amoinscheck,Bpluscheck, Bmoinscheck, Opluscheck, Omoinscheck };
            ComboBox[] comboBoxes = { ABpluscombobox, ABmoinscombobox, Apluscombobox, Amoinscombobox,Bpluscombobox, Bmoinscombobox, Opluscombobox,Omoinscombobox };
            int nombreComboBoxCorrespondantes = 0;

            // Parcourir les listes
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                // Vérifier si la case à cocher est cochée et si la ListBox a au moins un élément sélectionné
                if (checkBoxes[i].Checked && comboBoxes[i].SelectedIndex != -1)
                {
                    nombreComboBoxCorrespondantes++;
                }
            }

            return nombreComboBoxCorrespondantes;
        }
        //------------------3-------------------------------------------
        private int quantités_selectionnés()
        {
            CheckBox[] checkBoxes = { ABpluscheck, ABmoinscheck, Apluscheck, Amoinscheck, Bpluscheck, Bmoinscheck, Opluscheck, Omoinscheck };
            Guna2TextBox[] TextBoxes = { ABplusbox, ABmoinsbox, Aplusbox, Amoinsbox, Bplusbox, Bmoinsbox, Oplusbox, Omoinsbox };

            int nombreBoxesremplis = 0;
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (checkBoxes[i].Checked)
                {
                    float valeur;
                    // Vérifier si la valeur de la TextBox peut être convertie en float et si elle est supérieure à zéro
                    if (float.TryParse(TextBoxes[i].Text, out valeur) && valeur > 1)
                    {
                        nombreBoxesremplis++;
                    }
                }
            }
            return nombreBoxesremplis ;
        }
        //-------------------4--------------------------------------------
        private int nombreBoxesremplisNonVides()
        {
            int nombreBoxesremplisNonVides = 0;

            // Parcourir tous les contrôles sur le formulaire
            foreach (Control control in Controls)
            {
                // Vérifier si le contrôle est un NumericUpDown et si sa valeur est supérieure à zéro
                if (control is NumericUpDown numericUpDown && numericUpDown.Value > 0)
                {
                    nombreBoxesremplisNonVides++;
                }
            }

            return nombreBoxesremplisNonVides;
        }
        private void InsererDonnees(string groupeSanguin, float quantité, string degreUrgence)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True";
                using (SqlConnection cnx = new SqlConnection(connectionString))
                {
                    cnx.Open();

                    // Construire la commande SQL d'insertion
                    string query = "INSERT INTO HopitalEtClinique (Nom, Adresse, GroupSanguinRequis, QuantiteRequis, DegreUrgence) VALUES (@Nom, @Adresse, @GroupSanguinRequis, @QuantiteRequis, @DegreUrgence)";
                    SqlCommand cmd = new SqlCommand(query, cnx);

                    // Définir les paramètres de la commande avec les valeurs des contrôles
                    cmd.Parameters.AddWithValue("@Nom", NomHbox.Text);
                    cmd.Parameters.AddWithValue("@Adresse", AdresseHbox.Text);
                    cmd.Parameters.AddWithValue("@GroupSanguinRequis", groupeSanguin);
                    cmd.Parameters.AddWithValue("@QuantiteRequis", quantité); // Remplacez ce nombre par la quantité sélectionnée
                    cmd.Parameters.AddWithValue("@DegreUrgence", degreUrgence); // Remplacez cette chaîne par la valeur appropriée du degré d'urgence

                    // Exécuter la commande
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Les données pour le groupe sanguin " + groupeSanguin + " ont été insérées avec succès dans la base de données ", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de l'insertion des données pour le groupe sanguin " + groupeSanguin + " : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //------------------------------------------END------------------------------------------------------------------------------------------------
        //------------------------DataGridView---------------------------------
        private void ChargerDataGridView()
        {

            dataGridView1.Rows.Clear();

            using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM HopitalEtClinique", cnx);
                SqlDataReader reader = cmd.ExecuteReader();

                // Lire les données et les ajouter au DataGridView
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["ID"],
                        reader["Nom"],
                        reader["Adresse"],
                        reader["GroupSanguinRequis"],
                        reader["QuantiteRequis"],
                        reader["DegreUrgence"]
                    );
                }

                reader.Close();
            }
        }
        //------------------Button supprimer-------------------------

        private void button2_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérer l'ID du l'hopital sélectionné dans le DataGridView
                int idHôpital = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                // Confirmation de la suppression
                DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet ligne ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Supprimer le donneur de la base de données
                    using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                    {
                        string query = "DELETE FROM HopitalEtClinique WHERE ID = @ID";

                        using (SqlCommand cmd = new SqlCommand(query, cnx))
                        {
                            cmd.Parameters.AddWithValue("@ID", idHôpital);

                            cnx.Open();
                            cmd.ExecuteNonQuery();
                            cnx.Close();
                        }
                    }

                    // Supprimer la ligne sélectionnée du DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    MessageBox.Show("Ligne supprimé avec succès !", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un hopital/clinique à supprimer.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Reetour a la first page
            First_page FirstPage = new First_page();
            FirstPage.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Passer à la page de Menu
            Menu MenuPage = new Menu();
            MenuPage.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
