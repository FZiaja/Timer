using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.Common;
using System.Configuration;
using System.Windows.Threading;
using Timer.Models;
using System.Data;

namespace Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
	
	// Needs to be refactored to follow the MVVM design pattern.
    public partial class MainWindow : Window
    {
		private int time;
		private DispatcherTimer timer;

		public MainWindow()
        {
            InitializeComponent();
			LoadTimers();
        }

        private void New_Timer_Button_Click(object sender, RoutedEventArgs e)
        {
            NewTimerWindow newTimerWindow = new NewTimerWindow();
            newTimerWindow.Show();
        }

        private void LoadTimers()
        {
			string stringSQL = "Select * From Timers";

			DataTable table = QueryHandler.DBExecuteQuery(stringSQL);

			// Add a StackPanel
			StackPanel timerStackPanel = new StackPanel { Orientation = Orientation.Vertical };

			for (int i = 0; i < table.Rows.Count; i++)
			{
				TimerModel timer = new TimerModel(table.Rows[i][0].ToString(), int.Parse(table.Rows[i][1].ToString()));
				// MessageBox.Show(timer.ToString());

				StackPanel buttonStackPanel = new StackPanel { Orientation = Orientation.Horizontal };

				Button button = new Button();
				button.Click += new RoutedEventHandler(this.Timer_Button_Click);
				button.Content = timer.ToString();
				buttonStackPanel.Children.Add(button);

				Button deleteButton = new Button();
				deleteButton.Click += new RoutedEventHandler(this.Delete_Button_Click);
				deleteButton.Content = "Delete";
				buttonStackPanel.Children.Add(deleteButton);

				timerStackPanel.Children.Add(buttonStackPanel);
			}

			mainGrid.Children.Add(timerStackPanel);
		}

		private void Delete_Button_Click(object sender, RoutedEventArgs e)
		{
			StackPanel stackPanel = VisualTreeHelper.GetParent(sender as Button) as StackPanel;
			Button button = stackPanel.Children[0] as Button;
			int endPos = button.Content.ToString().LastIndexOf(' ') - 2;
			string timerName = button.Content.ToString().Substring(0, endPos);

			StackPanel parent = stackPanel.Parent as StackPanel;
			//parent.Children.Remove(stackPanel);
			mainGrid.Children.Remove(parent);

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
					this.Close();
					return;
				}

				// The DB data needed to open the correct DB
				connection.ConnectionString = connectionString;

				// Open the DB connection
				connection.Open();

				// Allows you to pass queries to the DB
				DbCommand deleteCommand = factory.CreateCommand();

				if (deleteCommand == null)
				{
					MessageBox.Show("Command Error");
					this.Close();
					return;
				}

				// Set the DB connection for commands
				deleteCommand.Connection = connection;

				// The query you want to issue
				deleteCommand.CommandText = string.Format("DELETE FROM Timers WHERE TimerName='{0}'", timerName);

				//"INSERT INTO Products (ProdId, Product, Price, Code) Values (6, 'Vegan Cheerios', 2.1, 'VCHE')";

				// Execute the qcommand
				deleteCommand.ExecuteNonQuery();
			}
			LoadTimers();

			//MessageBox.Show(button.Content.ToString().Substring(0, endPos));
		}

		private void Timer_Button_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			int startPos = button.Content.ToString().LastIndexOf(' ') + 1;
			string[] timerParts = button.Content.ToString().Substring(startPos).Split(':');

			int hours = int.Parse(timerParts[0]);
			int minutes = int.Parse(timerParts[1]);
			int seconds = int.Parse(timerParts[2]);

			string timeString = $"{hours}:" + (minutes < 10 ? $"0" : $"") + $"{minutes}:" + (seconds < 10 ? $"0" : $"") + $"{seconds}";

			lblTime.Content = timeString;

			timer = new DispatcherTimer();
			time = (3600 * hours) + (60 * minutes) + seconds;
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Tick += Timer_Tick;
			timer.Start();
			prButton.IsEnabled = true;

			//MessageBox.Show(timerParts[0] + "---" + timerParts[1] + "---" + timerParts[2] + "===" + string.Format("{0}", sum));
		}

		void Timer_Tick(object sender, EventArgs e)
		{
			if (time > 0)
			{
				time--;
				int hours = time / 3600;
				int minutes = (time % 3600) / 60;
				int seconds = time % 60;

				string timeString = $"{hours}:" + (minutes < 10 ? $"0" : $"") + $"{minutes}:" + (seconds < 10 ? $"0" : $"") + $"{seconds}";

				lblTime.Content = timeString;
			} 
			else
			{
				lblTime.Content = "RRRIINNGGG!!!";
				timer.Tick -= Timer_Tick;
				prButton.IsEnabled = false;
			}
		}

		void Main_Window_Activated(object sender, EventArgs e)
		{
			LoadTimers();
		}

		private void prButton_Click(object sender, RoutedEventArgs e)
		{
			if(string.Equals(prButton.Content, "Pause"))
			{
				prButton.Content = "Resume";
				timer.Tick -= Timer_Tick;

			}
			else if (string.Equals(prButton.Content, "Resume"))
			{
				prButton.Content = "Pause";
				timer.Tick += Timer_Tick;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			TimerModel timer = new TimerModel("Timer 1", 3601);
			TimerModel timer2 = new TimerModel("Timer 2", 3, 15, 5);

			MessageBox.Show(timer.ToString());
			MessageBox.Show(timer2.ToString());
		}
	}
}
