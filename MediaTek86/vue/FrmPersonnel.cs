using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MediaTek86.controleur;
using MediaTek86.modele;

namespace MediaTek86.vue
{
    public partial class FrmPersonnel : Form
    {
        private Controle controle;
        private FrmGestion parent;
        private Personnel personnelModif = null;
        
        private TextBox txtNom;
        private TextBox txtPrenom;
        private TextBox txtTel;
        private TextBox txtMail;
        private ComboBox cbxService;
        private Button btnEnregistrer;
        private Button btnAnnuler;
        private Label lblNom, lblPrenom, lblTel, lblMail, lblService;

        /// <summary>
        /// constructeur pour ajouter
        /// </summary>
        public FrmPersonnel(Controle controle, FrmGestion parent)
        {
            InitializeComponent();
            this.controle = controle;
            this.parent = parent;
            RemplirServices();
        }

        /// <summary>
        /// constructeur pour modifier
        /// </summary>
        public FrmPersonnel(Controle controle, FrmGestion parent, Personnel p)
        {
            InitializeComponent();
            this.controle = controle;
            this.parent = parent;
            this.personnelModif = p;
            RemplirServices();
            RemplirChamps();
        }

        private void RemplirServices()
        {
            List<Service> services = controle.GetLesServices();
            cbxService.DataSource = services;
        }

        /// <summary>
        /// pre-remplit les champs pour la modif
        /// </summary>
        private void RemplirChamps()
        {
            if (personnelModif != null)
            {
                txtNom.Text = personnelModif.Nom;
                txtPrenom.Text = personnelModif.Prenom;
                txtTel.Text = personnelModif.Tel;
                txtMail.Text = personnelModif.Mail;
                
                // sélectionner le bon service dans la combobox
                foreach (Service s in cbxService.Items)
                {
                    if (s.Idservice == personnelModif.Idservice)
                    {
                        cbxService.SelectedItem = s;
                        break;
                    }
                }
                
                this.Text = "MediaTek86 - Modifier un personnel";
                btnEnregistrer.Text = "Modifier";
            }
        }

        private void InitializeComponent()
        {
            this.txtNom = new TextBox();
            this.txtPrenom = new TextBox();
            this.txtTel = new TextBox();
            this.txtMail = new TextBox();
            this.cbxService = new ComboBox();
            this.btnEnregistrer = new Button();
            this.btnAnnuler = new Button();
            this.lblNom = new Label();
            this.lblPrenom = new Label();
            this.lblTel = new Label();
            this.lblMail = new Label();
            this.lblService = new Label();
            this.SuspendLayout();

            // nom
            this.lblNom.Text = "Nom :";
            this.lblNom.Location = new Point(30, 30);
            this.lblNom.AutoSize = true;
            this.txtNom.Location = new Point(130, 27);
            this.txtNom.Size = new Size(200, 23);

            // prenom
            this.lblPrenom.Text = "Prénom :";
            this.lblPrenom.Location = new Point(30, 70);
            this.lblPrenom.AutoSize = true;
            this.txtPrenom.Location = new Point(130, 67);
            this.txtPrenom.Size = new Size(200, 23);

            // telephone
            this.lblTel.Text = "Téléphone :";
            this.lblTel.Location = new Point(30, 110);
            this.lblTel.AutoSize = true;
            this.txtTel.Location = new Point(130, 107);
            this.txtTel.Size = new Size(200, 23);

            // mail
            this.lblMail.Text = "Mail :";
            this.lblMail.Location = new Point(30, 150);
            this.lblMail.AutoSize = true;
            this.txtMail.Location = new Point(130, 147);
            this.txtMail.Size = new Size(200, 23);

            // service
            this.lblService.Text = "Service :";
            this.lblService.Location = new Point(30, 190);
            this.lblService.AutoSize = true;
            this.cbxService.Location = new Point(130, 187);
            this.cbxService.Size = new Size(200, 23);
            this.cbxService.DropDownStyle = ComboBoxStyle.DropDownList;

            // boutons
            this.btnEnregistrer.Text = "Ajouter";
            this.btnEnregistrer.Location = new Point(130, 240);
            this.btnEnregistrer.Size = new Size(95, 30);
            this.btnEnregistrer.Click += new EventHandler(this.btnEnregistrer_Click);

            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Location = new Point(235, 240);
            this.btnAnnuler.Size = new Size(95, 30);
            this.btnAnnuler.Click += new EventHandler(this.btnAnnuler_Click);

            // form
            this.ClientSize = new Size(380, 300);
            this.Controls.Add(this.lblNom); this.Controls.Add(this.txtNom);
            this.Controls.Add(this.lblPrenom); this.Controls.Add(this.txtPrenom);
            this.Controls.Add(this.lblTel); this.Controls.Add(this.txtTel);
            this.Controls.Add(this.lblMail); this.Controls.Add(this.txtMail);
            this.Controls.Add(this.lblService); this.Controls.Add(this.cbxService);
            this.Controls.Add(this.btnEnregistrer);
            this.Controls.Add(this.btnAnnuler);
            this.Text = "MediaTek86 - Ajouter un personnel";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            // vérifier que tous les champs sont remplis
            if (txtNom.Text == "" || txtPrenom.Text == "" || txtTel.Text == "" || txtMail.Text == "" || cbxService.SelectedItem == null)
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Service serviceChoisi = (Service)cbxService.SelectedItem;

            if (personnelModif == null)
            {
                // ajout
                Personnel p = new Personnel(0, txtNom.Text, txtPrenom.Text, txtTel.Text, txtMail.Text, serviceChoisi.Idservice, serviceChoisi.Nom);
                controle.AddPersonnel(p);
            }
            else
            {
                // modification
                DialogResult confirm = MessageBox.Show("Confirmer la modification ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;
                
                Personnel p = new Personnel(personnelModif.Idpersonnel, txtNom.Text, txtPrenom.Text, txtTel.Text, txtMail.Text, serviceChoisi.Idservice, serviceChoisi.Nom);
                controle.UpdatePersonnel(p);
            }

            parent.RefreshDonnees();
            this.Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
