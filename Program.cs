using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace mysql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=localhost;user=root;database=testdb;port=3306;password=root";//MySql connection data
            MySqlConnection conn = new MySqlConnection(connStr);
            string TableName = "NameOfTable";//Table name
            string path = "Path to text file";//Text table path
            string[] lines = File.ReadAllLines(path);//Devides text into lines
            string[] columns = lines[0].Split('|');//Split 0 line into column names

            //Table size
            int itcnt= columns.Length;
            Console.WriteLine("Number of columns"+itcnt);
            int itlen = lines.Length;
            Console.WriteLine("Number of lines: "+itlen);

            //Creating a sql command for the table
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

            //Creating a table
            conn.Open();
            MySqlCommand create_table = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = create_table.ExecuteReader();
            rdr.Close();
            conn.Close();


            //Creating a sql command to insert data
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
                        //Checks for bad characters and replaces them
                        if (value[e]=='\'')
                        {
                            value = value.Replace('\'', '`');
                        }
                        else if (value[e]=='\\')
                        {
                            value = value.Replace('\\', '/');
                        }
                    }
                    //Change emptiness to nothing
                    if (value == "")
                    {
                        value="none";
                    }
                    //Adds element to the code
                    code += $"'{value}'";
                    //puts a comma after element
                    if (o < itcnt-1)
                    {
                        code += ", ";
                    }
                }
                code += ");";
                Console.WriteLine("Adding a new line: " + code);

                //Insert line to table
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
