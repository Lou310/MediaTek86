using System;
using System.Collections.Generic;
using MediaTek86.dal;
using MediaTek86.modele;
using MediaTek86.vue;

namespace MediaTek86.controleur
{
    /// <summary>
    /// Controleur principal de l'appli (MVC)
    /// </summary>
    public class Controle
    {
        private FrmAuthentification frmAuth;
        private FrmGestion frmGestion;
        
        public Controle()
        {
            frmAuth = new FrmAuthentification(this);
            frmAuth.ShowDialog();
        }

        /// <summary>
        /// verifie le login et le mot de passe
        /// </summary>
        public bool ControleAuthentification(string login, string pwd)
        {
            if (Access.GetInstance().ControleAuthentification(login, pwd))
            {
                frmAuth.Hide();
                frmGestion = new FrmGestion(this);
                frmGestion.ShowDialog();
                return true;
            }
            return false;
        }

        // -- gestion du personnel --
        
        public List<Personnel> GetLesPersonnels()
        {
            return Access.GetInstance().GetLesPersonnels();
        }

        public List<Service> GetLesServices()
        {
            return Access.GetInstance().GetLesServices();
        }

        public void AddPersonnel(Personnel personnel)
        {
            Access.GetInstance().AddPersonnel(personnel);
        }

        public void UpdatePersonnel(Personnel personnel)
        {
            Access.GetInstance().UpdatePersonnel(personnel);
        }

        public void DelPersonnel(Personnel personnel)
        {
            Access.GetInstance().DelPersonnel(personnel);
        }

        // --- gestion des absences ---

        public List<Absence> GetLesAbsences(Personnel personnel)
        {
            return Access.GetInstance().GetLesAbsences(personnel);
        }

        public List<Motif> GetLesMotifs()
        {
            return Access.GetInstance().GetLesMotifs();
        }

        public void AddAbsence(Absence absence)
        {
            Access.GetInstance().AddAbsence(absence);
        }

        public void UpdateAbsence(Absence ancienne, Absence nouvelle)
        {
            Access.GetInstance().UpdateAbsence(ancienne, nouvelle);
        }

        public void DelAbsence(Absence absence)
        {
            Access.GetInstance().DelAbsence(absence);
        }
    }
}
