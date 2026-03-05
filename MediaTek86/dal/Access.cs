using System;
using System.Collections.Generic;
using MediaTek86.modele;
using MediaTek86.bddmanager;

namespace MediaTek86.dal
{
    /// <summary>
    /// Classe d'acces aux données (couche DAL)
    /// fait le lien entre le controleur et la BDD via BddManager
    /// </summary>
    public class Access
    {
        /// <summary>
        /// chaine de connexion a la BDD (adapter si besoin)
        /// </summary>
        private static readonly string connectionString = "server=localhost;user id=root;password=;database=mediatek86;SslMode=none";
        private static Access instance = null;

        /// <summary>
        /// singleton
        /// </summary>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        private Access() { }

        /// <summary>
        /// verifie l'authentification du responsable
        /// </summary>
        public bool ControleAuthentification(string login, string pwd)
        {
            try
            {
                BddManager bdd = BddManager.GetInstance(connectionString);
                string req = "SELECT * FROM responsable WHERE login='" + login + "' AND pwd=SHA2('" + pwd + "', 256);";
                bdd.ReqSelect(req);
                bool result = bdd.Read();
                bdd.Close();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // ----------- PERSONNEL -----------

        /// <summary>
        /// recupere tous les personnels
        /// </summary>
        public List<Personnel> GetLesPersonnels()
        {
            List<Personnel> lesPersonnels = new List<Personnel>();
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "SELECT p.idpersonnel, p.nom, p.prenom, p.tel, p.mail, p.idservice, s.nom as nomservice ";
            req += "FROM personnel p JOIN service s ON p.idservice = s.idservice ORDER BY p.nom, p.prenom;";
            
            bdd.ReqSelect(req);
            while (bdd.Read())
            {
                Personnel p = new Personnel((int)bdd.Field("idpersonnel"),
                                            (string)bdd.Field("nom"),
                                            (string)bdd.Field("prenom"),
                                            (string)bdd.Field("tel"),
                                            (string)bdd.Field("mail"),
                                            (int)bdd.Field("idservice"),
                                            (string)bdd.Field("nomservice"));
                lesPersonnels.Add(p);
            }
            bdd.Close();
            return lesPersonnels;
        }

        /// <summary>
        /// ajoute un personnel dans la bdd
        /// </summary>
        public void AddPersonnel(Personnel personnel)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "INSERT INTO personnel (nom, prenom, tel, mail, idservice) ";
            req += "VALUES ('" + personnel.Nom + "', '" + personnel.Prenom + "', '" + personnel.Tel + "', '" + personnel.Mail + "', " + personnel.Idservice + ");";
            bdd.ReqUpdate(req);
        }

        /// <summary>
        /// modifie un personnel
        /// </summary>
        public void UpdatePersonnel(Personnel personnel)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "UPDATE personnel SET ";
            req += "nom = '" + personnel.Nom + "', ";
            req += "prenom = '" + personnel.Prenom + "', ";
            req += "tel = '" + personnel.Tel + "', ";
            req += "mail = '" + personnel.Mail + "', ";
            req += "idservice = " + personnel.Idservice + " ";
            req += "WHERE idpersonnel = " + personnel.Idpersonnel + ";";
            bdd.ReqUpdate(req);
        }

        /// <summary>
        /// supprime un personnel et ses absences
        /// </summary>
        public void DelPersonnel(Personnel personnel)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "DELETE FROM personnel WHERE idpersonnel = " + personnel.Idpersonnel + ";";
            bdd.ReqUpdate(req);
        }

        // ----------- SERVICES -----------

        /// <summary>
        /// recupere la liste des services
        /// </summary>
        public List<Service> GetLesServices()
        {
            List<Service> lesServices = new List<Service>();
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "SELECT * FROM service ORDER BY nom;";
            bdd.ReqSelect(req);
            while (bdd.Read())
            {
                Service s = new Service((int)bdd.Field("idservice"), (string)bdd.Field("nom"));
                lesServices.Add(s);
            }
            bdd.Close();
            return lesServices;
        }

        // ----------- ABSENCES -----------

        /// <summary>
        /// recupere les absences d'un personnel (triés par date decroissante)
        /// </summary>
        public List<Absence> GetLesAbsences(Personnel personnel)
        {
            List<Absence> lesAbsences = new List<Absence>();
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "SELECT a.idpersonnel, a.datedebut, a.datefin, a.idmotif, m.libelle ";
            req += "FROM absence a JOIN motif m ON a.idmotif = m.idmotif ";
            req += "WHERE a.idpersonnel = " + personnel.Idpersonnel + " ORDER BY a.datedebut DESC;";
            
            bdd.ReqSelect(req);
            while (bdd.Read())
            {
                Absence a = new Absence((int)bdd.Field("idpersonnel"),
                                        (DateTime)bdd.Field("datedebut"),
                                        (DateTime)bdd.Field("datefin"),
                                        (int)bdd.Field("idmotif"),
                                        (string)bdd.Field("libelle"));
                lesAbsences.Add(a);
            }
            bdd.Close();
            return lesAbsences;
        }

        /// <summary>
        /// ajoute une absence
        /// </summary>
        public void AddAbsence(Absence absence)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string dDebut = absence.Datedebut.ToString("yyyy-MM-dd HH:mm:ss");
            string dFin = absence.Datefin.ToString("yyyy-MM-dd HH:mm:ss");
            string req = "INSERT INTO absence (idpersonnel, datedebut, datefin, idmotif) ";
            req += "VALUES (" + absence.Idpersonnel + ", '" + dDebut + "', '" + dFin + "', " + absence.Idmotif + ");";
            bdd.ReqUpdate(req);
        }

        /// <summary>
        /// modifie une absence existante
        /// </summary>
        public void UpdateAbsence(Absence ancienne, Absence nouvelle)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string dDebutOld = ancienne.Datedebut.ToString("yyyy-MM-dd HH:mm:ss");
            string dDebutNew = nouvelle.Datedebut.ToString("yyyy-MM-dd HH:mm:ss");
            string dFinNew = nouvelle.Datefin.ToString("yyyy-MM-dd HH:mm:ss");

            string req = "UPDATE absence SET ";
            req += "datedebut = '" + dDebutNew + "', datefin = '" + dFinNew + "', idmotif = " + nouvelle.Idmotif + " ";
            req += "WHERE idpersonnel = " + ancienne.Idpersonnel + " AND datedebut = '" + dDebutOld + "';";
            bdd.ReqUpdate(req);
        }

        /// <summary>
        /// supprime une absence
        /// </summary>
        public void DelAbsence(Absence absence)
        {
            BddManager bdd = BddManager.GetInstance(connectionString);
            string dDebut = absence.Datedebut.ToString("yyyy-MM-dd HH:mm:ss");
            string req = "DELETE FROM absence WHERE idpersonnel = " + absence.Idpersonnel + " AND datedebut = '" + dDebut + "';";
            bdd.ReqUpdate(req);
        }

        // ----------- MOTIFS -----------

        /// <summary>
        /// recupere la liste des motifs d'absence
        /// </summary>
        public List<Motif> GetLesMotifs()
        {
            List<Motif> lesMotifs = new List<Motif>();
            BddManager bdd = BddManager.GetInstance(connectionString);
            string req = "SELECT * FROM motif ORDER BY libelle;";
            bdd.ReqSelect(req);
            while (bdd.Read())
            {
                Motif m = new Motif((int)bdd.Field("idmotif"), (string)bdd.Field("libelle"));
                lesMotifs.Add(m);
            }
            bdd.Close();
            return lesMotifs;
        }
    }
}
