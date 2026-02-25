using System;

namespace MediaTek86.modele
{
    /// <summary>
    /// represente un membre du personnel
    /// </summary>
    public class Personnel
    {
        public int Idpersonnel { get; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public int Idservice { get; set; }
        public string Service { get; set; }

        public Personnel(int idpersonnel, string nom, string prenom, string tel, string mail, int idservice, string service)
        {
            this.Idpersonnel = idpersonnel;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Tel = tel;
            this.Mail = mail;
            this.Idservice = idservice;
            this.Service = service;
        }
    }
}
