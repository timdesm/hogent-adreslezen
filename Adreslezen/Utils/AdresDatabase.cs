using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class AdresDatabase
    {

        public static void addAdres(Adres adres)
        {
            MySqlConnection con = Program.database.connection;

            using var cmd = Program.database.CommandExecutor("SELECT * FROM adres WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", adres.ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            
            if(!rdr.Read())
            {
                con.Close();
                
                using var cmd2 = Program.database.CommandExecutor("INSERT INTO adres (id, straatnaamID, huisnummer, appnummer, busnummer, huisnummerlabel, adreslocatieID) VALUES (@id, @straatnaamID, @huisnummer, @appnummer, @busnummer, @huisnummerlabel, @adreslocatieID)");
                cmd2.Parameters.AddWithValue("@id", adres.ID);
                cmd2.Parameters.AddWithValue("@straatnaamID", adres.Straatnaam.ID);
                cmd2.Parameters.AddWithValue("@huisnummer", adres.Number);
                cmd2.Parameters.AddWithValue("@appnummer", adres.AppNumber);
                cmd2.Parameters.AddWithValue("@busnummer", adres.BusNumber);
                cmd2.Parameters.AddWithValue("@huisnummerlabel", adres.NumberLabel);
                cmd2.Parameters.AddWithValue("@adreslocatieID", adres.Locatie.ID);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            con.Close();
        }

        public static Adres getAdres(int id)
        {
            DatabaseUtil database = new DatabaseUtil(Program.mysql_host, Program.mysql_user, Program.mysql_pass, Program.mysql_data);
            MySqlConnection con = database.connection;

            using var cmd = database.CommandExecutor("SELECT * FROM adres WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                int streetID = rdr.GetInt32("straatnaamID");
                String number = rdr.GetString("huisnummer");
                String appNumber = rdr.GetString("appnummer");
                String busNumber = rdr.GetString("busnummer");
                String numberLabel = rdr.GetString("huisnummerlabel");
                int locationID = rdr.GetInt32("adreslocatieID");

                Straatnaam straatnaam = Program.streets[streetID];
                AdresLocatie location = Program.locations[locationID];

                Adres adres = new Adres(id, straatnaam, appNumber, busNumber, number, number, straatnaam.Gemeente, 0, location.X, location.Y);
                return adres;
            }
            return null;
        }

        public static void saveCities(Dictionary<int, Gemeente> cities)
        {
            DatabaseUtil database = new DatabaseUtil(Program.mysql_host, Program.mysql_user, Program.mysql_pass, Program.mysql_data);
            foreach (Gemeente city in cities.Values)
            {
                addCity(database, city);
                Program.savedCities.Add(city.NIScode);
            }
        }

        public static void saveStreets(Dictionary<int, Straatnaam> streets)
        {
            DatabaseUtil database = new DatabaseUtil(Program.mysql_host, Program.mysql_user, Program.mysql_pass, Program.mysql_data);
            foreach (Straatnaam street in streets.Values)
            {
                addStreet(database, street);
                Program.savedStreets.Add(street.ID);
            }
        }

        public static void saveLocation(Dictionary<int, AdresLocatie> locations)
        {
            DatabaseUtil database = new DatabaseUtil(Program.mysql_host, Program.mysql_user, Program.mysql_pass, Program.mysql_data);
            foreach (AdresLocatie location in locations.Values)
            {
                addLocation(database, location);
                Program.savedLocaties.Add(location.ID);
            }
        }

        public static void addCity(DatabaseUtil util, Gemeente city)
        {
            MySqlConnection con = util.connection;

            using var cmd = util.CommandExecutor("SELECT * FROM gemeente WHERE NIScode = @niscode");
            cmd.Parameters.AddWithValue("@niscode", city.NIScode);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            if (!rdr.Read())
            {
                con.Close();

                using var cmd2 = util.CommandExecutor("INSERT INTO gemeente (NIScode, gemeentenaam) VALUES (@niscode, @gemeentenaam)");
                cmd2.Parameters.AddWithValue("@niscode", city.NIScode);
                cmd2.Parameters.AddWithValue("@gemeentenaam", city.Gemeentenaam);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            con.Close();
        }

        public static void addStreet(DatabaseUtil util, Straatnaam straatnaam)
        {
            MySqlConnection con = util.connection;

            using var cmd = util.CommandExecutor("SELECT * FROM straatnaam WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", straatnaam.ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            if (!rdr.Read())
            {
                con.Close();

                using var cmd2 = util.CommandExecutor("INSERT INTO straatnaam (id, straatnaam, NIScode) VALUES (@id, @straatnaam, @niscode)");
                cmd2.Parameters.AddWithValue("@id", straatnaam.ID);
                cmd2.Parameters.AddWithValue("@straatnaam", straatnaam.Streetname);
                cmd2.Parameters.AddWithValue("@niscode", straatnaam.Gemeente.NIScode);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            con.Close();
        }

        public static void addLocation(DatabaseUtil util,  AdresLocatie location)
        {
            MySqlConnection con = util.connection;

            using var cmd = util.CommandExecutor("SELECT * FROM adreslocatie WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", location.ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            if (!rdr.Read())
            {
                con.Close();

                using var cmd2 = util.CommandExecutor("INSERT INTO adreslocatie (id, x, y) VALUES (@id, @x, @y)");
                cmd2.Parameters.AddWithValue("@id", location.ID);
                cmd2.Parameters.AddWithValue("@x", location.X);
                cmd2.Parameters.AddWithValue("@y", location.Y);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            con.Close();
        }
    }
}
