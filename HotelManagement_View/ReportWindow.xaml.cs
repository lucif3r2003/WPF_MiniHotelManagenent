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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            loadReport();
        }

        private void loadReport()
        {
            var rp = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(b=>b.Customer).ToList();
            lvReport.ItemsSource = rp;
            lvReport.Items.Refresh();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(dpkStart.SelectedDate == null ||  dpkEnd.SelectedDate == null)
            {
                var r = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(b => b.Customer).ToList();
                lvReport.ItemsSource = r;
                lvReport.Items.Refresh();
            }
            else
            {
                DateOnly startDate = DateOnly.FromDateTime(dpkStart.SelectedDate.Value);
                DateOnly endDate = DateOnly.FromDateTime(dpkEnd.SelectedDate.Value);
                if (startDate >= endDate)
                {
                    MessageBox.Show("Please input invalid date");
                    return;
                }
                if (startDate >= DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("Please input invalid date");
                    return;
                }
                var rp = FuminiHotelManagementContext.INSTANCE.BookingReservations.Include(b => b.Customer).Where(x => x.BookingDate <= endDate && x.BookingDate >= startDate).ToList();
                lvReport.ItemsSource = rp;
                lvReport.Items.Refresh();
            }
            
          
        }
    }
}
