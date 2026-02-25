using System;

namespace MediaTek86.modele
{
    /// <summary>
    /// represente un service (administratif, prêt, etc.)
    /// </summary>
    public class Service
    {
        public int Idservice { get; }
        public string Nom { get; }

        public Service(int idservice, string nom)
        {
            this.Idservice = idservice;
            this.Nom = nom;
        }

        /// <summary>
        /// pour afficher le nom dans la combobox
        /// </summary>
        public override string ToString()
        {
            return this.Nom;
        }
    }
}
