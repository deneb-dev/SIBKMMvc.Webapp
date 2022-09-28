using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using SIBKMMvc.Webapp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SIBKMMvc.Webapp.Controllers
{
    public class CountryController : Controller
    {
        SqlConnection sqlConnection;

        /*
         * Data Source -> Server
         * Initial Catalog -> Database
         * User ID -> username
         * Password -> password
         * Connect Timeout
         */
        string connectionString = "Data Source=LAPTOP-7FOH7BBK;Initial Catalog=DENEBNET;" +
            "User ID=sibkmnet;Password=1234567890;Connect Timeout=30;";

        

        /* GET ALL*/
        /* GET*/
        public IActionResult Index()
        {
            string query = "SELECT * FROM Country";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            List<Country> Countries = new List<Country>();

            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Country country = new Country();
                            country.Id = Convert.ToInt32(sqlDataReader[0]);
                            country.Name = sqlDataReader[1].ToString();
                            Countries.Add(country);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return View(Countries);
        }

        /* GET BY ID*/
        /* GET*/
        public IActionResult GetById(int id)
        {
            string query = "SELECT * FROM Country WHERE Id = @id";

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = id;

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add(sqlParameter);
            List<Country> Countries = new List<Country>();
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Country country = new Country();
                            country.Id = Convert.ToInt32(sqlDataReader[0]);
                            country.Name = sqlDataReader[1].ToString();
                            //Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(Countries);
        }


        /* CREATE*/
        /* GET*/
        public IActionResult Create()
        {
            return View();
        }
        /* POST*/
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = country.Name;

                sqlCommand.Parameters.Add(sqlParameter);
                List<Country> Countries = new List<Country>();

                try
                {
                    sqlCommand.CommandText = "INSERT INTO Country " +
                        "(Name) VALUES (@name)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");

        }


        /* Edit*/
        /* GET*/
        public IActionResult Update()
        {
            return View();

        }
        /* POST*/
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public IActionResult Update(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                SqlParameter sqlParameter1 = new SqlParameter();
                SqlParameter sqlParameter2 = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = country.Name;
                sqlParameter1.ParameterName = "@update";
                sqlParameter1.Value = country.Update;
                sqlParameter2.ParameterName = "@id";
                sqlParameter2.Value = country.Id;


                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.Parameters.Add(sqlParameter1);
                sqlCommand.Parameters.Add(sqlParameter2);
                List<Country> Countries = new List<Country>();

                try
                {
                    sqlCommand.CommandText = "UPDATE Country SET name = (@update)" + "WHERE Id = (@id)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");
        }

        /* Hapus*/
        /* GET*/

        public IActionResult Delete(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@id";
                sqlParameter.Value = country.Id;

                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    sqlCommand.CommandText = "DELETE Country " + "WHERE (Id) = (@id)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");
        }
    
}
}
