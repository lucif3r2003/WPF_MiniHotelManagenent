using HotelManagementLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HotelManagement_View
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private int customerId;
        public CustomerWindow(int id)
        {
            InitializeComponent();
            loadHistory(id);
            loadProfile(id);
        }

        private void loadHistory(int id)
        {
            var history = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(x=>x.Customer).Where(x=>x.CustomerId == id).ToList();
            lvHistory.ItemsSource = history;
            lvHistory.Items.Refresh();
        }

        private void loadProfile(int id)
        {
            var cus = FuminiHotelManagementContext.INSTANCE.Customers.FirstOrDefault(x=>x.CustomerId.Equals(id));
            txtName.Text = cus.CustomerFullName.ToString();
            txtMail.Text = cus.EmailAddress.ToString();
            txtPhone.Text = cus.Telephone.ToString();
            dpkDob.SelectedDate = cus.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue);
            pwbPw.Password = cus.Password;
            tblId.Text = cus.CustomerId.ToString();
        }
        private void lvHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lvHistory.SelectedItem != null)
            {
                BookingReservation bk = (BookingReservation)lvHistory.SelectedItem;
                int id = bk.BookingReservationId;
                BookingDetailWindow wd = new BookingDetailWindow(id);
                wd.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNumeric(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone, please try again");
                return;
            }
            else
            {
                int id = Convert.ToInt32(tblId.Text);
                var cus = FuminiHotelManagementContext.INSTANCE.Customers.FirstOrDefault(x => x.CustomerId==id);
                if (cus != null)
                {
                    cus.CustomerFullName = txtName.Text;
                    cus.EmailAddress = txtMail.Text;
                    cus.Telephone = txtPhone.Text;
                    cus.Password = pwbPw.Password;
                    cus.CustomerBirthday = DateOnly.FromDateTime(dpkDob.SelectedDate.Value);
                    FuminiHotelManagementContext.INSTANCE.SaveChanges();
                    MessageBox.Show("Update profile successfully");
                    loadProfile(id);
                }
                else
                {
                    MessageBox.Show($"Customer not found for ID: {customerId}");
                    Debug.WriteLine($"Customer not found for ID: {customerId}");
                }
            }
        }
        private bool IsNumeric(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}
