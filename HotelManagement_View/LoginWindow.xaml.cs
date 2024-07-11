using HotelManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using Microsoft.Extensions.Configuration;

namespace HotelManagement_View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string pw = pwbPw.Password;
            var cus = FuminiHotelManagementContext.INSTANCE.Customers.FirstOrDefault(c => c.EmailAddress == email && c.Password == pw);
            var context = new FuminiHotelManagementContext();
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if (email == null || pw ==null)
            {
                MessageBox.Show("Email or password is invalid");
                return;
            }
            else
            {
                if (cus != null)
                {
                    int id = cus.CustomerId;
                    CustomerWindow customerWindow = new CustomerWindow(id);
                    this.Hide();
                    customerWindow.Show();
                }
                else if (email == config["AdminAccount:Email"] && pw == config["AdminAccount:Password"])
                {
                    MainWindow mainWindow = new MainWindow();
                    this.Hide();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Email or password is invalid");
                    return;
                }
            }
            
            
        }
    }
}
