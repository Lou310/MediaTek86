using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MediaTek86.controleur;
using MediaTek86.modele;

namespace MediaTek86.vue
{
    public partial class FrmGestion : Form
    {
        private Controle controle;
        private DataGridView dgvPersonnel;
        private Button btnAjouter;
        private Button btnModifier;
        private Button btnSupprimer;
        private Button btnAbsences;
        private Label lblTitre;

        public FrmGestion(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
            RemplirListe();
        }

        /// <summary>
        /// charge la liste du personnel dans le datagridview
        /// </summary>
        private void RemplirListe()
        {
            List<Personnel> lesPersonnels = controle.GetLesPersonnels();
            dgvPersonnel.DataSource = lesPersonnels;
            
            // cacher les colonnes id
            if (dgvPersonnel.Columns["Idpersonnel"] != null) dgvPersonnel.Columns["Idpersonnel"].Visible = false;
            if (dgvPersonnel.Columns["Idservice"] != null) dgvPersonnel.Columns["Idservice"].Visible = false;
            
            // renommer les en-têtes
            if (dgvPersonnel.Columns["Nom"] != null) dgvPersonnel.Columns["Nom"].HeaderText = "Nom";
            if (dgvPersonnel.Columns["Prenom"] != null) dgvPersonnel.Columns["Prenom"].HeaderText = "Prénom";
            if (dgvPersonnel.Columns["Tel"] != null) dgvPersonnel.Columns["Tel"].HeaderText = "Téléphone";
            if (dgvPersonnel.Columns["Mail"] != null) dgvPersonnel.Columns["Mail"].HeaderText = "Mail";
            if (dgvPersonnel.Columns["Service"] != null) dgvPersonnel.Columns["Service"].HeaderText = "Service";

            dgvPersonnel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void InitializeComponent()
        {
            this.dgvPersonnel = new DataGridView();
            this.btnAjouter = new Button();
            this.btnModifier = new Button();
            this.btnSupprimer = new Button();
            this.btnAbsences = new Button();
            this.lblTitre = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnel)).BeginInit();
            this.SuspendLayout();

            // titre
            this.lblTitre.Text = "Gestion du Personnel";
            this.lblTitre.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            this.lblTitre.Location = new Point(12, 12);
            this.lblTitre.AutoSize = true;

            // datagridview
            this.dgvPersonnel.Location = new Point(12, 45);
            this.dgvPersonnel.Size = new Size(760, 310);
            this.dgvPersonnel.ReadOnly = true;
            this.dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonnel.MultiSelect = false;
            this.dgvPersonnel.AllowUserToAddRows = false;

            // bouton ajouter
            this.btnAjouter.Text = "Ajouter";
            this.btnAjouter.Location = new Point(12, 370);
            this.btnAjouter.Size = new Size(120, 35);
            this.btnAjouter.Click += new EventHandler(this.btnAjouter_Click);

            // bouton modifier
            this.btnModifier.Text = "Modifier";
            this.btnModifier.Location = new Point(142, 370);
            this.btnModifier.Size = new Size(120, 35);
            this.btnModifier.Click += new EventHandler(this.btnModifier_Click);

            // bouton supprimer
            this.btnSupprimer.Text = "Supprimer";
            this.btnSupprimer.Location = new Point(272, 370);
            this.btnSupprimer.Size = new Size(120, 35);
            this.btnSupprimer.Click += new EventHandler(this.btnSupprimer_Click);

            // bouton absences
            this.btnAbsences.Text = "Gérer les absences";
            this.btnAbsences.Location = new Point(612, 370);
            this.btnAbsences.Size = new Size(160, 35);
            this.btnAbsences.Click += new EventHandler(this.btnAbsences_Click);

            // form
            this.ClientSize = new Size(784, 421);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.dgvPersonnel);
            this.Controls.Add(this.btnAjouter);
            this.Controls.Add(this.btnModifier);
            this.Controls.Add(this.btnSupprimer);
            this.Controls.Add(this.btnAbsences);
            this.Text = "MediaTek86 - Liste du Personnel";
            this.StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// rafraichir la liste apres ajout/modif/suppression
        /// </summary>
        public void RefreshDonnees()
        {
            RemplirListe();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            FrmPersonnel frm = new FrmPersonnel(controle, this);
            frm.ShowDialog();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvPersonnel.SelectedRows.Count > 0)
            {
                Personnel p = (Personnel)dgvPersonnel.SelectedRows[0].DataBoundItem;
                FrmPersonnel frm = new FrmPersonnel(controle, this, p);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un personnel.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvPersonnel.SelectedRows.Count > 0)
            {
                Personnel p = (Personnel)dgvPersonnel.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer " + p.Prenom + " " + p.Nom + " ?\nSes absences seront aussi supprimées.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    controle.DelPersonnel(p);
                    RefreshDonnees();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un personnel.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAbsences_Click(object sender, EventArgs e)
        {
            if (dgvPersonnel.SelectedRows.Count > 0)
            {
                Personnel p = (Personnel)dgvPersonnel.SelectedRows[0].DataBoundItem;
                FrmAbsence frm = new FrmAbsence(controle, p);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un personnel.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
