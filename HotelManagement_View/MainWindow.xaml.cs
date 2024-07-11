using HotelManagementLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement_View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadFromDB();
            loadCustomer();
            loadBooking();
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------   
        //---------------------------------------------ROOM_MANAGEMENT_HERE------------------------------------------------------------------------------------------------------------------

        private void loadFromDB()
        {
            //Load all room
            var listRoom = FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(x => x.RoomType).ToList();
            dgvRoom.ItemsSource = listRoom;
            dgvRoom.Items.Refresh();
            //End load all room
           //Filter
           //Radio button
            var rdbAvailable = new RadioButton()
            {
                Content = "Available",
                Tag = 0,
                GroupName = "Status"
            };
            spStatus.Children.Add(rdbAvailable);
            rdbAvailable.Checked += (s, e) => FilterRoom();

            var rdbOccupied = new RadioButton()
            {
                Content = "Occupied",
                Tag = 1,
                GroupName = "Status"
            };
            spStatus.Children.Add(rdbOccupied);
            rdbOccupied.Checked += (s, e) => FilterRoom();
            //end radio button
            //combobox
            var roomType = FuminiHotelManagementContext.INSTANCE.RoomTypes.ToList();
            cbbTypeRoom.ItemsSource = roomType;
            cbbTypeRoom.DisplayMemberPath = "RoomTypeName";
            cbbTypeRoom.SelectedValuePath = "RoomTypeId";
            //end combobox
        }

        private void btnViewRoom_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int roomId = (int)btn.Tag;
                var room = FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(x => x.RoomType).FirstOrDefault(y => y.RoomId == roomId);
                RoomDetailWindow roomDetailWindow = new RoomDetailWindow(room);
                roomDetailWindow.ShowDialog();

            }

        }
        private void FilterRoom()
        {   
            //get status(ko hieu lam)
            RadioButton rdb = spStatus.Children.OfType<RadioButton>().FirstOrDefault(r=>r.IsChecked==true);
            int? status = rdb !=null ? (int?)rdb.Tag : null ;
            //get roomtypeid
            int? roomTypeId = cbbTypeRoom.SelectedValue as int?;
            var query = FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(r => r.RoomType).AsQueryable();
            //filter
            if (status.HasValue)
            {
                query =query.Where(y=>y.RoomStatus == status.Value);
            }
            if(roomTypeId.HasValue)
            {
                query =query.Where(y=>y.RoomTypeId == roomTypeId.Value);
            }
            var filterRoom = query.ToList();
            dgvRoom.ItemsSource = filterRoom;
            dgvRoom.Items.Refresh();
        }

        private void cbbTypeRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterRoom();
        }

        private void btnclearFilter_Click(object sender, RoutedEventArgs e)
        {   
            //Clear Combobox selection
            cbbTypeRoom.SelectedIndex = -1;

            // Clear RadioButton selection
            foreach (var radioButton in spStatus.Children.OfType<RadioButton>())
            {
                radioButton.IsChecked = false;
            }
            //load from db
            var listRoom = FuminiHotelManagementContext.INSTANCE.RoomInformations.Include(x => x.RoomType).ToList();
            dgvRoom.ItemsSource = listRoom;
            dgvRoom.Items.Refresh();
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomInformation room = null;
            RoomDetailWindow roomDetailWindow = new RoomDetailWindow(room);
            roomDetailWindow.ShowDialog();
        }





 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------   
 //---------------------------------------------CUSTOMER_MANAGEMENT_HERE------------------------------------------------------------------------------------------------------------------

        private void loadCustomer()
        {
            dgvCustomer.ItemsSource = null;
            var listCustomer = FuminiHotelManagementContext.INSTANCE.Customers.Include(x => x.BookingReservations).ToList();
            dgvCustomer.ItemsSource = listCustomer;
            dgvCustomer.Items.Refresh();
            var rdbActive = new RadioButton()
            {
                Content = "Active",
                Tag = 1,
            };
            var rdbDeactive = new RadioButton()
            {
                Content = "Deactive",
                Tag = 0,
            };
            rdbActive.Checked += (s, e) => FilterCustomer();
            rdbDeactive.Checked += (s, e) => FilterCustomer();
            spCusStatus.Children.Add(rdbActive);
            spCusStatus.Children.Add(rdbDeactive);
        }

        private void FilterCustomer()
        {
            RadioButton rdb = spCusStatus.Children.OfType<RadioButton>().FirstOrDefault(r=> r.IsChecked==true);
            var status = Convert.ToByte(rdb.Tag);
            if (status != null)
            {
                var filterCustomer = FuminiHotelManagementContext.INSTANCE.Customers.Where(c => c.CustomerStatus == status).ToList();
                dgvCustomer.ItemsSource = filterCustomer;
                dgvCustomer.Items.Refresh();
            }
        }

        private void btnclearFilter1_Click(object sender, RoutedEventArgs e)
        {
            foreach(var rdb in spCusStatus.Children.OfType<RadioButton>())
            {
                rdb.IsChecked = false;
            }
            var listCus = FuminiHotelManagementContext.INSTANCE.Customers.ToList();
            dgvCustomer.ItemsSource = listCus;
            dgvCustomer.Items.Refresh();
        }

        private void btnViewCusDetail_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if(btn != null)
            {
                int? cusId = (int)btn.Tag;
                var customer = FuminiHotelManagementContext.INSTANCE.Customers.FirstOrDefault(x=>x.CustomerId==cusId);
                CustomerDetailWindow cdw = new CustomerDetailWindow(customer);
                cdw.ShowDialog();
            }

        }

        private void btnAddCusomer_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            CustomerDetailWindow customerDetail = new CustomerDetailWindow(customer);
            customerDetail.ShowDialog();
        }

    
        
        
        
        
 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------   
 //---------------------------------------------BOOKING_MANAGEMENT_HERE------------------------------------------------------------------------------------------------------------------
        

        private void loadBooking()
        {
            var context = new FuminiHotelManagementContext();
            var listBooking = context.BookingReservations.Include(x=>x.Customer).ToList();
            dgvBooking.ItemsSource = listBooking;
            dgvBooking.Items.Refresh();
            RadioButton rdbCheckIn = new RadioButton()
            {
                Content= "Check in",
                Tag = 1
            };
            RadioButton rdbCheckOut = new RadioButton()
            {
                Content = "Check out",
                Tag = 0
            };
            rdbCheckIn.Checked += (s,e) => FilterBooking();
            rdbCheckOut.Checked += (s, e) => FilterBooking();
            spStatusBooking.Children.Add(rdbCheckIn);
            spStatusBooking.Children.Add(rdbCheckOut);
        }

        private void FilterBooking()
        {
            var status = spStatusBooking.Children.OfType<RadioButton>().FirstOrDefault(r=>r.IsChecked ==true);
            byte s = Convert.ToByte(status.Tag);
            if (s !=null)
            {
                var filterBooking = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(b=>b.Customer).Where(x=>x.BookingStatus == s).ToList();
                dgvBooking.ItemsSource= filterBooking;
                dgvBooking.Items.Refresh();
            }
        }

        private void btnViewBooking_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = (int)btn.Tag;
            if (id != null)
            {
                BookingDetailWindow wd = new BookingDetailWindow(id);
                wd.ShowDialog();
            }
        }

        private void btnclearFilter2_Click(object sender, RoutedEventArgs e)
        {
            foreach (var rdb in spStatusBooking.Children.OfType<RadioButton>())
            {
                rdb.IsChecked = false;

            }
            var listBook = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(x => x.Customer).ToList();
            dgvBooking.ItemsSource = listBook;
            dgvBooking.Items.Refresh();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
        }
    }
}