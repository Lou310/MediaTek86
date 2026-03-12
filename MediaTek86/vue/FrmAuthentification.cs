using System;
using System.Drawing;
using System.Windows.Forms;
using MediaTek86.controleur;

namespace MediaTek86.vue
{
    public partial class FrmAuthentification : Form
    {
        private Controle controle;
        
        private TextBox txtLogin;
        private TextBox txtPwd;
        private Button btnConnexion;
        private Label lblLogin;
        private Label lblPwd;
        private Label lblTitre;

        public FrmAuthentification(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        private void InitializeComponent()
        {
            this.txtLogin = new TextBox();
            this.txtPwd = new TextBox();
            this.btnConnexion = new Button();
            this.lblLogin = new Label();
            this.lblPwd = new Label();
            this.lblTitre = new Label();
            this.SuspendLayout();

            // titre
            this.lblTitre.Text = "MediaTek86 - Connexion";
            this.lblTitre.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            this.lblTitre.Location = new Point(50, 15);
            this.lblTitre.AutoSize = true;

            // login
            this.lblLogin.Text = "Identifiant :";
            this.lblLogin.Location = new Point(40, 55);
            this.lblLogin.AutoSize = true;
            
            this.txtLogin.Location = new Point(40, 75);
            this.txtLogin.Size = new Size(200, 23);

            // mot de passe
            this.lblPwd.Text = "Mot de passe :";
            this.lblPwd.Location = new Point(40, 110);
            this.lblPwd.AutoSize = true;
            
            this.txtPwd.Location = new Point(40, 130);
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new Size(200, 23);

            // bouton connexion
            this.btnConnexion.Text = "Se connecter";
            this.btnConnexion.Location = new Point(40, 170);
            this.btnConnexion.Size = new Size(200, 30);
            this.btnConnexion.Click += new EventHandler(this.btnConnexion_Click);

            // form
            this.ClientSize = new Size(284, 230);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.btnConnexion);
            this.Text = "MediaTek86 - Authentification";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == "" || txtPwd.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!controle.ControleAuthentification(txtLogin.Text, txtPwd.Text))
            {
                MessageBox.Show("Identifiant ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd.Text = "";
                txtPwd.Focus();
            }
        }
    }
}
