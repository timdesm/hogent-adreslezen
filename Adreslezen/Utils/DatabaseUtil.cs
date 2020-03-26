using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class DatabaseUtil
    {
        public MySqlConnection connection;

        public DatabaseUtil(String host, String user, String password, String database)
        {
            String cs = @"server=" + host + ";userid=" + user + ";password=" + password + ";database=" + database;
            this.connection = new MySqlConnection(cs);
        }

        public MySqlCommand CommandExecutor(String sql)
        {
            return new MySqlCommand(sql, this.connection);
        }
    }
}
