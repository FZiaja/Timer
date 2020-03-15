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
using System.Windows.Shapes;

using System.Data.Common;
using System.Configuration;
using Timer.Models;

namespace Timer
{
    /// <summary>
    /// Interaction logic for NewTimerWindow.xaml
    /// </summary>
    public partial class NewTimerWindow : Window
    {
        public NewTimerWindow()
        {
            InitializeComponent();

            List<int> hoursData = new List<int>();

            for (int i = 0; i < 24; i++)
            {
                hoursData.Add(i);
            }

            hoursComboBox.ItemsSource = hoursData;

            List<int> msData = new List<int>();

            for (int i = 0; i < 60; i++)
            {
                msData.Add(i);
            }

            minutesComboBox.ItemsSource = msData;
            secondsComboBox.ItemsSource = msData;

            hoursComboBox.SelectedItem = 0;
            minutesComboBox.SelectedItem = 0;
            secondsComboBox.SelectedItem = 0;
        }

        private void createTimerButton_Click(object sender, RoutedEventArgs e)
        {
			TimerModel newTimer = new TimerModel(timerName.Text, int.Parse(hoursComboBox.SelectedItem.ToString()), int.Parse(minutesComboBox.SelectedItem.ToString()), int.Parse(secondsComboBox.SelectedItem.ToString()));
			string SQL = string.Format("INSERT INTO Timers (TimerName, Seconds) VALUES ('{0}', {1})", newTimer.Name, newTimer.TotalSeconds);

			QueryHandler.DBExecuteNonQuery(SQL);
			this.Close();
		}
	}
}
