using HotelManagementLibrary.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for RoomDetailWindow.xaml
    /// </summary>
    public partial class RoomDetailWindow : Window
    {
        private RoomInformation _room;
        public RoomDetailWindow(RoomInformation room)
        {
            InitializeComponent();
            _room = room;
            loadData();
        }

       
        private void loadData()
        {
            
            if(_room != null)//View Detail, Update, Delete
            {
                txtId.Text = _room.RoomId.ToString();
                txtName.Text = _room.RoomNumber.ToString();
                var roomType = FuminiHotelManagementContext.INSTANCE.RoomTypes.ToList();
                cbbType.ItemsSource = roomType;
                cbbType.DisplayMemberPath = "RoomTypeName";
                cbbType.SelectedValuePath = "RoomTypeId";
                cbbType.Text = _room.RoomType.RoomTypeName.ToString();
                txtDescription.Text = _room.RoomDetailDescription;
                txtPrice.Text  = _room.RoomPricePerDay.ToString();
                txtCapacity.Text = _room.RoomMaxCapacity.ToString();
                RadioButton rdbAvailable = new RadioButton()
                {
                    Content = "Available",
                    Tag = 0,
                    GroupName = "Status",
                    IsChecked = _room.RoomStatus == 0
                };
                //rdbAvailable.Checked += (s, e) => Status((int)rdbAvailable.Tag);
                RadioButton rdbOccupied = new RadioButton()
                {
                    Content = "Occupied",
                    Tag = 1,
                    GroupName = "Status",
                    IsChecked = _room.RoomStatus == 1
                };
                //rdbOccupied.Checked += (s, e) => Status((int)rdbOccupied.Tag);
                spStatus.Children.Add(rdbAvailable);
                spStatus.Children.Add(rdbOccupied);

                Button btnSave = new Button()
                {
                    Content = "Save",
                    Width = 100,
                    Margin = new Thickness(50,0,50,0)
                };
                btnSave.Click += btnSave_Click;
                spUpdate.Children.Add(btnSave);
                Button btnDelete = new Button()
                {
                    Content = "Delete",
                    Width = 100,
                    Margin = new Thickness(50, 0, 50, 0)
                };
                btnDelete.Click += btnDelete_Click;
                spUpdate.Children.Add(btnDelete);
            }
            else
            {
                txtName.IsReadOnly = false;
                var roomType = FuminiHotelManagementContext.INSTANCE.RoomTypes.ToList();
                cbbType.ItemsSource = roomType;
                cbbType.DisplayMemberPath = "RoomTypeName";
                cbbType.SelectedValuePath = "RoomTypeId";
                RadioButton rdbAvailable = new RadioButton()
                {
                    Content = "Available",
                    Tag = 0,
                    GroupName = "Status",
                    
                };
                RadioButton rdbOccupied = new RadioButton()
                {
                    Content = "Occupied",
                    Tag = 1,
                    GroupName = "Status",
                    
                };
                spStatus.Children.Add(rdbAvailable);
                spStatus.Children.Add(rdbOccupied);
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
            
            

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(x => x.RoomType).Any(r => r.RoomNumber == txtName.Text))
            {
                MessageBox.Show("Room number is excist");
                return;
            }
            if (!IsNumeric(txtPrice.Text))
            {
                MessageBox.Show("Price is invalid");
                return;
            }
            if (!IsNumeric(txtCapacity.Text))
            {
                MessageBox.Show("Cappacity is invalid");
                return;
            }
            RoomInformation room = new RoomInformation();
            room.RoomNumber = txtName.Text;
            room.RoomTypeId = (int)cbbType.SelectedValue;
            room.RoomDetailDescription = txtDescription.Text;
            room.RoomPricePerDay = decimal.Parse(txtPrice.Text);
            room.RoomMaxCapacity = int.Parse(txtCapacity.Text);
            var status = spStatus.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            if (status != null)
            {
                room.RoomStatus = Convert.ToByte(status.Tag);
            }
            else
            {
                MessageBox.Show("Please choose status");
                return;
            }
            try
            {
                FuminiHotelManagementContext.INSTANCE.Add(room);
                FuminiHotelManagementContext.INSTANCE.SaveChanges();
                MessageBox.Show($" Roomtype :{room.RoomTypeId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Inner exception: {ex.InnerException.Message}\n\n{ex.InnerException.StackTrace}");
                }
            }

            
            this.Close();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var room = FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(x => x.RoomType).FirstOrDefault(r => r.RoomId == _room.RoomId);
            if(room != null)
            {
                if (!IsNumeric(txtPrice.Text))
                {
                    MessageBox.Show("Price is invalid");
                    return;
                }
                if (!IsNumeric(txtCapacity.Text))
                {
                    MessageBox.Show("Cappacity is invalid");
                    return;
                }
                room.RoomTypeId = (int)cbbType.SelectedValue;
                room.RoomDetailDescription = txtDescription.Text;
                room.RoomPricePerDay = int.Parse(txtPrice.Text);
                room.RoomMaxCapacity = int.Parse(txtCapacity.Text);
                var status = spStatus.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
                if (status != null)
                {
                    room.RoomStatus = Convert.ToByte(status.Tag);
                }
                
                
            }            
            FuminiHotelManagementContext.INSTANCE.SaveChanges();
            MessageBox.Show("Update sucessfully");
            this.Close();
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this room?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                FuminiHotelManagementContext.INSTANCE.Remove(_room);
                FuminiHotelManagementContext.INSTANCE.SaveChanges();
                MessageBox.Show("Room deleted successfully.");
            }
            else
            {
                MessageBox.Show("Deletion Cancel");
            }
            this.Close();
        }
        private bool IsNumeric(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}
