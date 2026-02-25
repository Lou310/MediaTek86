using System;

namespace MediaTek86.modele
{
    /// <summary>
    /// represente un motif d'absence
    /// </summary>
    public class Motif
    {
        public int Idmotif { get; }
        public string Libelle { get; }

        public Motif(int idmotif, string libelle)
        {
            this.Idmotif = idmotif;
            this.Libelle = libelle;
        }

        /// <summary>
        /// pour l'affichage dans la combobox
        /// </summary>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
