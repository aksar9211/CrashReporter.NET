using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace CrashReporterTest
{
  using System.Collections.Generic;

  public partial class FormMain : Form
    {
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

      public CustomerService(string connStr)
      {
        _connStr = connStr;
      }

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
