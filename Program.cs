using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;

namespace mysql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=localhost;user=root;database=testdb;port=3306;password=root";
            MySqlConnection conn = new MySqlConnection(connStr);
            /*try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "CREATE TABLE Pers (PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            conn.Close();
            Console.WriteLine("Done.");*/

            string path = "http://192.168.88.191:80/2.5m_diia.gov.ua_2022.txt";
            //string readText = File.ReadAllText("C:\\Users\\andri\\source\\repos\\mysql\\example.txt");
            string[] lines = File.ReadAllLines("C:\\Users\\andri\\source\\repos\\mysql\\example.txt");
            string text = "";
            for (int i = 0; lines.Length > i; i++)
            {
                text += lines[i] + "|";
            }
            string[] words = text.Split('|');
            Console.WriteLine(words);
            int itcnt=0;
            for(int i  = 0; words[i]!="e"; i++)
            {
                itcnt++;
            }
            Console.WriteLine(itcnt);
            int itlen = words.Length/itcnt;
            Console.WriteLine(itlen);
            string sql = "CREATE TABLE test1 (";
            for (int i = 0; i < itcnt; i++)
            {
                sql += words[i]+ " varchar(255)";
                if (i < itcnt-1)
                {
                    sql += ",";
                }
            }
            sql += ");";
            Console.WriteLine(sql);
            /*conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();*/

            sql = "INSERT INTO Customers (";
            for (int i = 0; i < itcnt; i++)
            {
                sql += words[i];
                if (i < itcnt - 1)
                {
                    sql += ", ";
                }
            }
            sql += ")VALUES (";
            for (int i = 1; i < itlen; i++)
            {
                string code = "(";
                for (int o = 0; o < itcnt; o++)
                {
                    string value = words[itcnt * i + 1 + o];
                    if (value == "")
                    {
                        value="none";
                    }
                    //Console.Write(words[itcnt*i + o] + "|");
                    code += $"'{value}'";
                    if (o < itcnt-1)
                    {
                        code += ", ";
                    }
                }
                code += ");";
                Console.WriteLine(code);
            }
            //conn.Close();
        }
    }
}
