using System;

namespace MediaTek86.modele
{
    /// <summary>
    /// represente le responsable (pour la connexion)
    /// </summary>
    public class Responsable
    {
        public string Login { get; }
        public string Pwd { get; }

        public Responsable(string login, string pwd)
        {
            this.Login = login;
            this.Pwd = pwd;
        }
    }
}
