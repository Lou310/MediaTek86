using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MediaTek86.controleur;
using MediaTek86.modele;

namespace MediaTek86.vue
{
    public partial class FrmAbsence : Form
    {
        private Controle controle;
        private Personnel personnel;

        private DataGridView dgvAbsences;
        private Button btnAjouter;
        private Button btnModifier;
        private Button btnSupprimer;
        private Label lblTitre;

        public FrmAbsence(Controle controle, Personnel personnel)
        {
            InitializeComponent();
            this.controle = controle;
            this.personnel = personnel;
            lblTitre.Text = "Absences de " + personnel.Prenom + " " + personnel.Nom;
            RemplirListeAbsences();
        }

        /// <summary>
        /// remplit le datagridview avec les absences du personnel
        /// </summary>
        public void RemplirListeAbsences()
        {
            List<Absence> lesAbsences = controle.GetLesAbsences(personnel);
            dgvAbsences.DataSource = lesAbsences;

            // cacher les colonnes techniques
            if (dgvAbsences.Columns["Idpersonnel"] != null) dgvAbsences.Columns["Idpersonnel"].Visible = false;
            if (dgvAbsences.Columns["Idmotif"] != null) dgvAbsences.Columns["Idmotif"].Visible = false;
            
            // renommer les en-têtes
            if (dgvAbsences.Columns["Datedebut"] != null) dgvAbsences.Columns["Datedebut"].HeaderText = "Date début";
            if (dgvAbsences.Columns["Datefin"] != null) dgvAbsences.Columns["Datefin"].HeaderText = "Date fin";
            if (dgvAbsences.Columns["Motif"] != null) dgvAbsences.Columns["Motif"].HeaderText = "Motif";

            dgvAbsences.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void InitializeComponent()
        {
            this.dgvAbsences = new DataGridView();
            this.btnAjouter = new Button();
            this.btnModifier = new Button();
            this.btnSupprimer = new Button();
            this.lblTitre = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbsences)).BeginInit();
            this.SuspendLayout();

            // titre
            this.lblTitre.Text = "Absences";
            this.lblTitre.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblTitre.Location = new Point(12, 12);
            this.lblTitre.AutoSize = true;

            // datagridview
            this.dgvAbsences.Location = new Point(12, 45);
            this.dgvAbsences.Size = new Size(560, 220);
            this.dgvAbsences.ReadOnly = true;
            this.dgvAbsences.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAbsences.MultiSelect = false;
            this.dgvAbsences.AllowUserToAddRows = false;

            // bouton ajouter
            this.btnAjouter.Text = "Ajouter";
            this.btnAjouter.Location = new Point(12, 275);
            this.btnAjouter.Size = new Size(120, 35);
            this.btnAjouter.Click += new EventHandler(this.btnAjouter_Click);

            // bouton modifier
            this.btnModifier.Text = "Modifier";
            this.btnModifier.Location = new Point(142, 275);
            this.btnModifier.Size = new Size(120, 35);
            this.btnModifier.Click += new EventHandler(this.btnModifier_Click);

            // bouton supprimer
            this.btnSupprimer.Text = "Supprimer";
            this.btnSupprimer.Location = new Point(272, 275);
            this.btnSupprimer.Size = new Size(120, 35);
            this.btnSupprimer.Click += new EventHandler(this.btnSupprimer_Click);

            // form
            this.ClientSize = new Size(584, 325);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.dgvAbsences);
            this.Controls.Add(this.btnAjouter);
            this.Controls.Add(this.btnModifier);
            this.Controls.Add(this.btnSupprimer);
            this.Text = "MediaTek86 - Absences";
            this.StartPosition = FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbsences)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            FrmAjoutAbsence frm = new FrmAjoutAbsence(controle, this, personnel);
            frm.ShowDialog();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvAbsences.SelectedRows.Count > 0)
            {
                Absence a = (Absence)dgvAbsences.SelectedRows[0].DataBoundItem;
                FrmAjoutAbsence frm = new FrmAjoutAbsence(controle, this, personnel, a);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une absence.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvAbsences.SelectedRows.Count > 0)
            {
                Absence a = (Absence)dgvAbsences.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show("Supprimer cette absence ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    controle.DelAbsence(a);
                    RemplirListeAbsences();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une absence.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
