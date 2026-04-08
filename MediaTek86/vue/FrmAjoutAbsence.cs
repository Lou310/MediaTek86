using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MediaTek86.controleur;
using MediaTek86.modele;

namespace MediaTek86.vue
{
    public partial class FrmAjoutAbsence : Form
    {
        private Controle controle;
        private FrmAbsence parent;
        private Personnel personnel;
        private Absence absenceModif = null;

        private DateTimePicker dtpDebut;
        private DateTimePicker dtpFin;
        private ComboBox cbxMotif;
        private Button btnEnregistrer;
        private Button btnAnnuler;
        private Label lblDebut, lblFin, lblMotif;

        /// <summary>
        /// constructeur pour ajout
        /// </summary>
        public FrmAjoutAbsence(Controle controle, FrmAbsence parent, Personnel personnel)
        {
            InitializeComponent();
            this.controle = controle;
            this.parent = parent;
            this.personnel = personnel;
            RemplirMotifs();
        }

        /// <summary>
        /// constructeur pour modification
        /// </summary>
        public FrmAjoutAbsence(Controle controle, FrmAbsence parent, Personnel personnel, Absence absence)
        {
            InitializeComponent();
            this.controle = controle;
            this.parent = parent;
            this.personnel = personnel;
            this.absenceModif = absence;
            RemplirMotifs();
            RemplirChamps();
        }

        private void RemplirMotifs()
        {
            List<Motif> motifs = controle.GetLesMotifs();
            cbxMotif.DataSource = motifs;
        }

        private void RemplirChamps()
        {
            if (absenceModif != null)
            {
                dtpDebut.Value = absenceModif.Datedebut;
                dtpFin.Value = absenceModif.Datefin;

                // sélectionner le bon motif
                foreach (Motif m in cbxMotif.Items)
                {
                    if (m.Idmotif == absenceModif.Idmotif)
                    {
                        cbxMotif.SelectedItem = m;
                        break;
                    }
                }

                this.Text = "MediaTek86 - Modifier une absence";
                btnEnregistrer.Text = "Modifier";
            }
        }

        private void InitializeComponent()
        {
            this.dtpDebut = new DateTimePicker();
            this.dtpFin = new DateTimePicker();
            this.cbxMotif = new ComboBox();
            this.btnEnregistrer = new Button();
            this.btnAnnuler = new Button();
            this.lblDebut = new Label();
            this.lblFin = new Label();
            this.lblMotif = new Label();
            this.SuspendLayout();

            // date debut
            this.lblDebut.Text = "Date début :";
            this.lblDebut.Location = new Point(30, 30);
            this.lblDebut.AutoSize = true;
            this.dtpDebut.Location = new Point(130, 27);
            this.dtpDebut.Format = DateTimePickerFormat.Short;
            this.dtpDebut.Size = new Size(150, 23);

            // date fin
            this.lblFin.Text = "Date fin :";
            this.lblFin.Location = new Point(30, 70);
            this.lblFin.AutoSize = true;
            this.dtpFin.Location = new Point(130, 67);
            this.dtpFin.Format = DateTimePickerFormat.Short;
            this.dtpFin.Size = new Size(150, 23);

            // motif
            this.lblMotif.Text = "Motif :";
            this.lblMotif.Location = new Point(30, 110);
            this.lblMotif.AutoSize = true;
            this.cbxMotif.Location = new Point(130, 107);
            this.cbxMotif.Size = new Size(150, 23);
            this.cbxMotif.DropDownStyle = ComboBoxStyle.DropDownList;

            // boutons
            this.btnEnregistrer.Text = "Enregistrer";
            this.btnEnregistrer.Location = new Point(80, 160);
            this.btnEnregistrer.Size = new Size(95, 30);
            this.btnEnregistrer.Click += new EventHandler(this.btnEnregistrer_Click);

            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Location = new Point(185, 160);
            this.btnAnnuler.Size = new Size(95, 30);
            this.btnAnnuler.Click += new EventHandler(this.btnAnnuler_Click);

            // form
            this.ClientSize = new Size(330, 220);
            this.Controls.Add(this.lblDebut); this.Controls.Add(this.dtpDebut);
            this.Controls.Add(this.lblFin); this.Controls.Add(this.dtpFin);
            this.Controls.Add(this.lblMotif); this.Controls.Add(this.cbxMotif);
            this.Controls.Add(this.btnEnregistrer);
            this.Controls.Add(this.btnAnnuler);
            this.Text = "MediaTek86 - Ajouter une absence";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            DateTime debut = dtpDebut.Value.Date;
            DateTime fin = dtpFin.Value.Date;

            // vérifier que le motif est sélectionné
            if (cbxMotif.SelectedItem == null)
            {
                MessageBox.Show("Veuillez choisir un motif.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // vérifier les dates
            if (fin < debut)
            {
                MessageBox.Show("La date de fin doit être après la date de début.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Motif motifChoisi = (Motif)cbxMotif.SelectedItem;

            // vérifier qu'il n'y a pas de chevauchement avec une autre absence
            List<Absence> absences = controle.GetLesAbsences(personnel);
            foreach (Absence abs in absences)
            {
                // en cas de modif, on ignore l'absence qu'on est en train de modifier
                if (absenceModif != null && abs.Datedebut == absenceModif.Datedebut)
                {
                    continue;
                }

                // test de chevauchement : debut <= finExistante ET fin >= debutExistante
                if (debut <= abs.Datefin.Date && fin >= abs.Datedebut.Date)
                {
                    MessageBox.Show("Il y a déjà une absence sur cette période (" + abs.Datedebut.ToShortDateString() + " - " + abs.Datefin.ToShortDateString() + ").", "Chevauchement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (absenceModif == null)
            {
                // ajout
                Absence nouvelleAbsence = new Absence(personnel.Idpersonnel, debut, fin, motifChoisi.Idmotif, motifChoisi.Libelle);
                controle.AddAbsence(nouvelleAbsence);
            }
            else
            {
                // modification avec confirmation
                DialogResult confirm = MessageBox.Show("Confirmer la modification ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                Absence nouvelleAbsence = new Absence(personnel.Idpersonnel, debut, fin, motifChoisi.Idmotif, motifChoisi.Libelle);
                controle.UpdateAbsence(absenceModif, nouvelleAbsence);
            }

            parent.RemplirListeAbsences();
            this.Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
