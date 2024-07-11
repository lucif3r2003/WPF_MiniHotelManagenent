﻿using System;
using System.Collections.Generic;

namespace HotelManagementLibrary.Models;

public partial class BookingReservation
{
    public int BookingReservationId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public int CustomerId { get; set; }

    public byte? BookingStatus { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Customer Customer { get; set; } = null!;
    public string StatusDisplay
    {
        get
        {
            return BookingStatus == 0 ? "Check out" : "Check in";
        }

    }
}
