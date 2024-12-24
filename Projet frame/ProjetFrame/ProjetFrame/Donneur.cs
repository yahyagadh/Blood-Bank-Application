using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.ComTypes;

namespace ProjetFrame
{
    public partial class Donneur : Form
    {
        public Donneur()
        {
            InitializeComponent();
        }
        //bsh me tokoodsh a chaque fois taaml f connexion
        String connexionString = ConfigurationManager.ConnectionStrings["CnxSqlServer"].ConnectionString;

        private void Donneur_Load(object sender, EventArgs e)
        {
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Les controles de saisies

            // Vérification du l'ID 
            if (string.IsNullOrWhiteSpace(idbox.Text))
            {
                MessageBox.Show("Veuillez saisir votre ID.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du nom
            if (string.IsNullOrWhiteSpace(nombox.Text))
            {
                MessageBox.Show("Veuillez saisir votre nom.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du Prénom
            if (string.IsNullOrWhiteSpace(prenombox.Text))
            {
                MessageBox.Show("Veuillez saisir votre prénom.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //verifier que le nom et prenom se compsent seulement avec des lettres
            if (!EstUniquementAlphabetique(nombox.Text) || !EstUniquementAlphabetique(prenombox.Text))
            {
                MessageBox.Show("Le nom et le prénom doivent contenir uniquement des lettres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du l'adresse 
            if (string.IsNullOrWhiteSpace(adressebox.Text))
            {
                MessageBox.Show("Veuillez saisir votre adresse.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du numéro
            if (string.IsNullOrWhiteSpace(numerobox.Text) || numerobox.Text.Length != 8 || !numerobox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro doit être composé exactement de 8 chiffres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification des dates de naissance et de don
            DateTime dateNaissance = datedenaissancetimepicker.Value;
            DateTime dateDon = datedontimepicker.Value;
            if (dateNaissance >= DateTime.Today || dateDon > DateTime.Today)
            {
                MessageBox.Show("La date de naissance et la date de don doivent etre logiques.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification du groupe sanguin
            if (GroupesangBox.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un groupe sanguin.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérification de la quantité
            if (!float.TryParse(quantitebox.Text, out float quantite) || quantite < 1)
            {
                MessageBox.Show("La quantité doit être supérieur ou égal à 1.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Vérification des antécédents médicaux
            if (AMcombobox.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner au moins un antécédent médical.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Vérification des antécédents médicaux spécifiques
            string selectedAntecedents = AMcombobox.SelectedItem.ToString();
            if (selectedAntecedents.Contains("Avoir un tatouage récent ") || selectedAntecedents.Contains("Avoir une anémie sévère") ||
                selectedAntecedents.Contains("Avoir une maladie infectieuse") || selectedAntecedents.Contains("Avoir des maladies cardiaques")
                || selectedAntecedents.Contains("Avoir reçu une transfusion sanguine récente"))
            {
                MessageBox.Show("Le donneur ne peut pas faire de don en raison de ses antécédents médicaux.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si toutes les vérifications sont passées, ajouter le donneur à la base de données
            AjouterDonneurDeSang();
            //ajouter aussi ces dons dans table de stocks
            AjouterStock();
            //afficher les données dans le datagridview
            ChargerDataGridView();

        }
        //fonction AjouterDonneurDeSang a la base de donneé
        private void AjouterDonneurDeSang()
        {
            using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
            {
                // Requête SQL pour insérer les données dans la base de données
                string query = "INSERT INTO DonneurDeSang (Nom, Prenom, DateNaissance, DateDon, Adresse, Numero, GroupeSanguin, QuantiteSang, AntecedentsMedicaux) " +
                               "VALUES (@Nom, @Prenom, @DateNaissance, @DateDon, @Adresse, @Numero, @GroupeSanguin, @QuantiteSang, @AntecedentsMedicaux)";

                // Création de la commande SQL
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    // Attribution des valeurs des contrôles aux paramètres de la commande
                    cmd.Parameters.AddWithValue("@Nom", nombox.Text);
                    cmd.Parameters.AddWithValue("@Prenom", prenombox.Text);
                    cmd.Parameters.AddWithValue("@DateNaissance", datedenaissancetimepicker.Value);
                    cmd.Parameters.AddWithValue("@DateDon", datedontimepicker.Value);
                    cmd.Parameters.AddWithValue("@Adresse", adressebox.Text);
                    cmd.Parameters.AddWithValue("@Numero", numerobox.Text);
                    cmd.Parameters.AddWithValue("@GroupeSanguin", GroupesangBox.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@QuantiteSang", float.Parse(quantitebox.Text)); // Assurez-vous que la quantité est au format numérique
                    cmd.Parameters.AddWithValue("@AntecedentsMedicaux", AMcombobox.SelectedItem.ToString());

                    // Ouverture de la connexion
                    cnx.Open();

                    // Exécution de la commande
                    cmd.ExecuteNonQuery();

                    // Fermeture de la connexion
                    cnx.Close();
                }
            }

            // Affichage d'un message de succès
            MessageBox.Show("Donneur de sang ajouté avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //ajouter les dons dans la table de stock
        private void AjouterStock()
        {
            using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
            {
                // Requête SQL pour insérer les données dans la base de données
                string query1 = "INSERT INTO StockDeSang (GroupeSang, QuantiteSang, DateDon,DateExpiration) " +
                               "VALUES (@GroupeSanguin,@QuantiteSang,@DateDon,DATEADD(day, 10, @DateDon) )";

                // Création de la commande SQL
                using (SqlCommand cmd = new SqlCommand(query1, cnx))
                {
                    // Attribution des valeurs des contrôles aux paramètres de la commande
                    cmd.Parameters.AddWithValue("@DateDon", datedontimepicker.Value);
                    cmd.Parameters.AddWithValue("@GroupeSanguin", GroupesangBox.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@QuantiteSang", float.Parse(quantitebox.Text)); // Assurez-vous que la quantité est au format numérique
                    

                    // Ouverture de la connexion
                    cnx.Open();

                    // Exécution de la commande
                    cmd.ExecuteNonQuery();

                    // Fermeture de la connexion
                    cnx.Close();
                }
            }
        }

        //fonction d'afficher les données dans la dataGridview
        private void ChargerDataGridView()
        {
            dataGridView1.Rows.Clear();

            using (SqlConnection cnx = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DonneurDeSang", cnx);
                SqlDataReader reader = cmd.ExecuteReader();

                // Lire les données et les ajouter au DataGridView
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["ID"],
                        reader["Nom"],
                        reader["Prenom"],
                        reader["DateNaissance"],
                        reader["DateDon"],
                        reader["Adresse"],
                        reader["Numero"],
                        reader["GroupeSanguin"],
                        reader["QuantiteSang"],
                        reader["AntecedentsMedicaux"]
                    );
                }

                reader.Close();
            }
        }
        //fonction de controle de saisie des lettres 
        public bool EstUniquementAlphabetique(string str)
        {
            // Utilisation d'une expression régulière pour vérifier si la chaîne contient uniquement des lettres
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }

        private void quantitebox_TextChanged(object sender, EventArgs e)
        {

        }

        private void nombox_TextChanged(object sender, EventArgs e)
        {

        }
        //button supprimer
        private void button2_Click(object sender, EventArgs e)
        {

            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérer l'ID du donneur sélectionné dans le DataGridView
                int idDonneur = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                // Confirmation de la suppression
                DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce donneur ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

                        //supprimer de la table des stocks

                        using (SqlConnection cnx2 = new SqlConnection("Data Source=DESKTOP-2TJJJJ2\\SQLEXPRESS;Initial Catalog=ProjetFrame;Integrated Security=True"))
                        {
                            string query1 = "DELETE FROM StockDeSang WHERE ID = @ID";

                            using (SqlCommand cmd = new SqlCommand(query1, cnx2))
                            {
                                cmd.Parameters.AddWithValue("@ID", idDonneur);

                                cnx2.Open();
                                cmd.ExecuteNonQuery();
                            }
                            // Fermer la deuxième connexion après avoir terminé l'opération sur StockDeSang
                            cnx2.Close();
                        }

                    }


                    // Supprimer la ligne sélectionnée du DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    MessageBox.Show("Donneur supprimé avec succès !", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un donneur à supprimer.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Passer à la page de Menu
            Menu MenuPage = new Menu();
            MenuPage.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Reetour a la first page
            First_page FirstPage = new First_page();
            FirstPage.Show();
            this.Hide();
        }
    }
}
