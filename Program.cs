using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace mysql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=localhost;user=root;database=testdb;port=3306;password=root";
            MySqlConnection conn = new MySqlConnection(connStr);
            string TableName = "NameOfTable";
            string path = "Path to text file";
            string[] lines = File.ReadAllLines(path);
            string[] columns = lines[0].Split('|');
            int itcnt= columns.Length;
            Console.WriteLine("Number of columns"+itcnt);
            int itlen = lines.Length;
            Console.WriteLine("Number of lines: "+itlen);


            string sql = $"CREATE TABLE {TableName}(";
            for (int i = 0; i < itcnt; i++)
            {
                sql += columns[i]+ " varchar(255)";
                if (i < itcnt-1)
                {
                    sql += ",";
                }
            }
            sql += ");";
            Console.WriteLine("Creating table: "+sql);
            conn.Open();
            MySqlCommand create_table = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = create_table.ExecuteReader();
            rdr.Close();
            conn.Close();

            sql = $"INSERT INTO {TableName} (";
            for (int i = 0; i < itcnt; i++)
            {
                sql += columns[i];
                if (i < itcnt - 1)
                {
                    sql += ", ";
                }
            }
            sql += ")VALUES (";
            for (int i = 1; i < itlen; i++)
            {
                string[] words = lines[i].Split('|');
                string code = sql;
                for (int o = 0; o < itcnt; o++)
                {
                    string value = words[o];
                    for (int e=0; value.Length>e;e++)
                    {
                        if (value[e]=='\'')
                        {
                            value = value.Replace('\'', '`');
                        }
                    }
                    if (value == "")
                    {
                        value="none";
                    }
                    code += $"'{value}'";
                    if (o < itcnt-1)
                    {
                        code += ", ";
                    }
                }
                code += ");";
                Console.WriteLine("Adding a new line: "+code);
                conn.Open();
                MySqlCommand add_data = new MySqlCommand(code, conn);
                MySqlDataReader rdrr = add_data.ExecuteReader();
                rdrr.Close();
                conn.Close();
            }
            
            Console.WriteLine("Done!");
        }
    }
}
