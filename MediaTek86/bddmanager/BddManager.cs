using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MediaTek86.bddmanager
{
    /// <summary>
    /// classe singleton pour gerer la connexion MySQL
    /// </summary>
    public class BddManager
    {
        private static BddManager instance = null;
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;

        /// <summary>
        /// constructeur privé (singleton)
        /// </summary>
        private BddManager(string stringConnect)
        {
            try
            {
                connection = new MySqlConnection(stringConnect);
                connection.Open();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Impossible de se connecter à la BDD :\n" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// retourne l'instance unique
        /// </summary>
        public static BddManager GetInstance(string stringConnect)
        {
            if (instance == null)
            {
                instance = new BddManager(stringConnect);
            }
            return instance;
        }

        /// <summary>
        /// execute un SELECT
        /// </summary>
        public void ReqSelect(string stringQuery)
        {
            try
            {
                command = new MySqlCommand(stringQuery, connection);
                command.Prepare();
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// execute un INSERT, UPDATE ou DELETE
        /// </summary>
        public void ReqUpdate(string stringQuery)
        {
            try
            {
                command = new MySqlCommand(stringQuery, connection);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// lire la ligne suivante du resultat
        /// </summary>
        public bool Read()
        {
            if (reader == null) return false;
            try
            {
                return reader.Read();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// recuperer la valeur d'un champ
        /// </summary>
        public object Field(string stringField)
        {
            if (reader == null) return null;
            try
            {
                return reader[stringField];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// fermer le reader
        /// </summary>
        public void Close()
        {
            if (reader != null)
            {
                try { reader.Close(); }
                catch { }
            }
        }
    }
}
