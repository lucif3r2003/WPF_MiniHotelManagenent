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
    /// Interaction logic for BookingDetailWindow.xaml
    /// </summary>
    public partial class BookingDetailWindow : Window
    {
        public BookingDetailWindow( int id)
        {
            InitializeComponent();
            loadDetail(id);
        }
        private void loadDetail(int id)
        {
            var booking = FuminiHotelManagementContext.INSTANCE.BookingDetails.Where(b=>b.BookingReservationId == id).ToList();
            dgvBookingDetail.ItemsSource = booking;
            dgvBookingDetail.Items.Refresh();
        }
    }
}
