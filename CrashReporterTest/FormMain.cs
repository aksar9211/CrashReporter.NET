using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace CrashReporterTest
{
  using System.Collections.Generic;
  using System.Data.SqlClient;

  public partial class FormMain : Form
    {
        /// <summary>
        /// Initializes a new instance of the FormMain class and sets up the form's UI components.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }

        private void ButtonTestClick(object sender, EventArgs e)
        {
            ThrowException();
        }

        private void ButtonThreadException_Click(object sender, EventArgs e)
        {
            var thread = new Thread(ThrowException);
            thread.Start();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> and, if a specific file does not exist, throws a <see cref="FileNotFoundException"/> with the original exception as the inner exception.
        /// </summary>
        private void ThrowException()
        {
            try
            {
                throw new ArgumentException();
            }
            catch (ArgumentException argumentException)
            {
                const string path = "test.txt";

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException(
                        "File Not found when trying to write argument exception to the file", argumentException);
                }
            }
        }
    }

    public class CustomerService
    {
      private string _connStr;

      /// <summary>
      /// Initializes a new instance of the CustomerService class with the specified database connection string.
      /// </summary>
      /// <param name="connStr">The connection string used to connect to the database.</param>
      public CustomerService(string connStr)
      {
        _connStr = connStr;
      }

      /// <summary>
      /// Retrieves customer names from the database for the specified customer IDs, with optional filtering by prefix and case conversion.
      /// </summary>
      /// <param name="ids">A list of customer IDs to look up.</param>
      /// <param name="uppercase">If true, converts each customer name to uppercase.</param>
      /// <param name="filterPrefix">If provided, only includes names that start with this prefix.</param>
      /// <returns>A list of customer names matching the specified criteria.</returns>
      public List<string> GetCustomerNames(List<string> ids, bool uppercase, string filterPrefix = null)
      {
        var output = new List<string>();

        if (ids.Count > 0)
        {
          SqlConnection con = new SqlConnection(_connStr);
          con.Open();

          foreach (var cid in ids)
          {
            var cmd = new SqlCommand("SELECT Name FROM Customers WHERE Id = '" + cid + "'", con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
              var name = rdr[0].ToString();

              if (!string.IsNullOrEmpty(filterPrefix))
              {
                if (!name.StartsWith(filterPrefix)) continue;
              }

              if (uppercase)
                name = name.ToUpper();

              output.Add(name);
            }
            rdr.Close();
          }

          con.Close();
        }

        return output;
      }
    }

}
