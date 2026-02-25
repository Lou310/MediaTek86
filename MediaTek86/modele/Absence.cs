using System;

namespace MediaTek86.modele
{
    /// <summary>
    /// represente une absence
    /// </summary>
    public class Absence
    {
        public int Idpersonnel { get; }
        public DateTime Datedebut { get; set; }
        public DateTime Datefin { get; set; }
        public int Idmotif { get; set; }
        public string Motif { get; set; }

        public Absence(int idpersonnel, DateTime datedebut, DateTime datefin, int idmotif, string motif)
        {
            this.Idpersonnel = idpersonnel;
            this.Datedebut = datedebut;
            this.Datefin = datefin;
            this.Idmotif = idmotif;
            this.Motif = motif;
        }
    }
}
