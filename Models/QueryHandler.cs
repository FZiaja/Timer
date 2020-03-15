using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Timer.Models
{
	static internal class QueryHandler
	{
		public static void MsgBoxTest()
		{
			MessageBox.Show("It worked!!");
		}

		public static void DBExecuteNonQuery(string stringSQL)
		{
			// App.config stores configuration data
			// System.Data.SqlClient provides classes
			// for accessing a SQL Server DB

			// connectionString defines the DB name, and
			// other parameters for connecting to the DB

			// Configurationmanager provides access to
			// config data in App.config
			string provider = ConfigurationManager.AppSettings["provider"];

			string connectionString = ConfigurationManager.AppSettings["connectionString"];

			// DbProviderFactories generates an 
			// instance of a DbProviderFactory
			DbProviderFactory factory = DbProviderFactories.GetFactory(provider);

			// The DBConnection represents the DB connection
			using (DbConnection connection =
				factory.CreateConnection())
			{
				// Check if a connection was made
				if (connection == null)
				{
					MessageBox.Show("Connection Error");
					return;
				}

				// The DB data needed to open the correct DB
				connection.ConnectionString = connectionString;

				// Open the DB connection
				connection.Open();

				// Insert into table
				DbCommand command = factory.CreateCommand();

				if (command == null)
				{
					MessageBox.Show("Command Error");
					return;
				}

				command.Connection = connection;

				command.CommandText = stringSQL;

				command.ExecuteNonQuery();
			}
		}

		public static DataTable DBExecuteQuery(string stringSQL)
		{
			// App.config stores configuration data
			// System.Data.SqlClient provides classes
			// for accessing a SQL Server DB

			// connectionString defines the DB name, and
			// other parameters for connecting to the DB

			// Configurationmanager provides access to
			// config data in App.config
			string provider = ConfigurationManager.AppSettings["provider"];

			string connectionString = ConfigurationManager.AppSettings["connectionString"];

			// DbProviderFactories generates an 
			// instance of a DbProviderFactory
			DbProviderFactory factory = DbProviderFactories.GetFactory(provider);

			// The DBConnection represents the DB connection
			using (DbConnection connection =
				factory.CreateConnection())
			{
				// Check if a connection was made
				if (connection == null)
				{
					Console.WriteLine("Connection Error");
					Console.ReadLine();
					return null;
				}

				// The DB data needed to open the correct DB
				connection.ConnectionString = connectionString;

				// Open the DB connection
				connection.Open();

				// Allows you to pass queries to the DB
				DbCommand command = factory.CreateCommand();

				if (command == null)
				{
					Console.WriteLine("Command Error");
					Console.ReadLine();
					return null;
				}

				// Set the DB connection for commands
				command.Connection = connection;

				// The query you want to issue
				command.CommandText = stringSQL;

				// DbDataReader reads the row results
				// from the query
				DbDataReader reader = command.ExecuteReader();

				DataTable table = new DataTable("table");

				table.Load(reader);

				return table;
			}
		}
	}
}
