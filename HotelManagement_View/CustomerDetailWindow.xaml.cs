using HotelManagementLibrary.Models;
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

namespace HotelManagement_View
{
    /// <summary>
    /// Interaction logic for CustomerDetailWindow.xaml
    /// </summary>
    public partial class CustomerDetailWindow : Window
    {
        private Customer _customer;
        public CustomerDetailWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            loadData();
        }
        private void loadData()
        {
            if (_customer == null)
            {
                RadioButton rdbActive = new RadioButton()
                {
                    Content = "Active",
                    Tag = 1
                };
                RadioButton rdbDeactive = new RadioButton()
                {
                    Content = "Deactive",
                    Tag = 0
                };
                spStatus.Children.Add(rdbActive);
                spStatus.Children.Add(rdbDeactive);
                Button btnAdd = new Button()
                {
                    Content = "Add",
                    Width = 100,
                    Margin = new Thickness(50, 0, 50, 0)
                };
                spUpdate.Children.Add(btnAdd);
                btnAdd.Click += BtnAdd_Click;
                Button btnCancel = new Button()
                {
                    Content = "Cancel",
                    Width = 100,
                    Margin = new Thickness(50, 0, 50, 0)
                };
                btnCancel.Click += BtnCancel_Click;
                spUpdate.Children.Add(btnCancel);
            }
            else
            {
                txtId.Text = _customer.CustomerId.ToString();
                txtName.Text = _customer.CustomerFullName;
                txtPhone.Text = _customer.Telephone;
                txtEmail.Text = _customer.EmailAddress.ToString();
                DateOnly dob = _customer.CustomerBirthday.Value;
                dpkDob.SelectedDate = dob.ToDateTime(TimeOnly.MinValue);
                RadioButton rdbActive = new RadioButton()
                {
                    Content = "Active",
                    Tag = 1
                };
                RadioButton rdbDeactive = new RadioButton()
                {
                    Content = "Deactive",
                    Tag =0
                };
                if(_customer.CustomerStatus != null)
                {
                    if(_customer.CustomerStatus == 1)
                    {
                        rdbActive.IsChecked = true;
                    }
                    else rdbDeactive.IsChecked = true;
                }
                spStatus.Children.Add(rdbActive);
                spStatus.Children.Add(rdbDeactive);
                Button btnSave = new Button()
                {
                    Content = "Save",
                    Width = 100,
                    Margin = new Thickness(50, 0, 50, 0)
                };
                btnSave.Click += BtnSave_Click;
                spUpdate.Children.Add(btnSave);
                Button btnDelete = new Button()
                {
                    Content = "Delete",
                    Width = 100,
                    Margin = new Thickness(50, 0, 50, 0)
                };
                btnDelete.Click += BtnDelete_Click;
                spUpdate.Children.Add(btnDelete);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = new MessageBoxResult();
            result = MessageBox.Show("Do you want to delete?", "Confirm", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                FuminiHotelManagementContext.INSTANCE.Remove(_customer);
                FuminiHotelManagementContext.INSTANCE.SaveChanges();
                MessageBox.Show("Delete customer successfully");              
            }
            else
            {
                MessageBox.Show("Deletion Cancel");
            }
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNumeric(txtPhone.Text))
            {
                MessageBox.Show("Phone number is invalid, please try another!");
                return;
            }
            _customer.CustomerFullName = txtName.Text;
            _customer.EmailAddress = txtEmail.Text;
            _customer.Telephone = txtPhone.Text;
            _customer.CustomerBirthday = DateOnly.FromDateTime(dpkDob.SelectedDate.Value);
            var status = spStatus.Children.OfType<RadioButton>().FirstOrDefault(r=> r.IsChecked ==true);
            if (status != null)
            {
                _customer.CustomerStatus = Convert.ToByte(status.Tag);
            }
            else
            {
                MessageBox.Show("Please choose status!");
                return;
            }
            FuminiHotelManagementContext.INSTANCE.SaveChanges();
            MessageBox.Show("Update infomation succesfully");
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var context = new FuminiHotelManagementContext();
            if(context.Customers.Any(c=>c.EmailAddress == txtEmail.Text))
            {
                MessageBox.Show("Email is already excist, please log in! ");
                return;     
            }
            if (!IsNumeric(txtPhone.Text))
            {
                MessageBox.Show("Phone number is invalid, please try another!");
                return;
            }
            if(context.Customers.Any(c=>c.Telephone == txtPhone.Text))
            {
                MessageBox.Show("Phone number is already excist, please try another!");
                return; 
            }
            var newCustomer = new Customer()
            {
                CustomerFullName = txtName.Text,
                CustomerBirthday = DateOnly.FromDateTime(dpkDob.SelectedDate.Value),
                EmailAddress = txtEmail.Text,
                Telephone = txtPhone.Text,
                Password="1"
            };
            var status = spStatus.Children.OfType<RadioButton>().FirstOrDefault(r=>r.IsChecked==true);
            if (status!=null)
            {
                newCustomer.CustomerStatus = Convert.ToByte(status.Tag);
            }
            else
            {
                MessageBox.Show("Please choose status ");
                return;
            }
            context.Customers.Add(newCustomer);
            context.SaveChanges();
            MessageBox.Show("Add new customer successfully!");
            this.Close();
        }
        private bool IsNumeric(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}
