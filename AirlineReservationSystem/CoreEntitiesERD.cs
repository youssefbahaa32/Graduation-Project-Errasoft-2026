


/*
 * Core Entities
 * 
             ApplicationUser
                    │
                    ├────────────── Notification
                    │
                    ▼
                  Booking
                    │
                    ├──────────── Payment
                    │
                    ▼
             BookingPassenger
             ┌──────┼───────────────┐
             ▼      ▼               ▼
            Passenger FlightSeat    Ticket
                │         │
                │         ▼
                │       Flight
                │         ▲
                │         │
                │      Aircraft
                │         │
                │         ▼
                │        Seat
                │
                ▼
             Baggage
                │
                ▼
            LoyaltyAccount
                │
                ▼
            PointTransaction





Search Flight
      │
      ▼
Select Flight
      │
      ▼
Select Seat
      │
      ▼
Enter Passenger Information
      │
      ▼
Create Booking
      │
      ▼
Create BookingPassenger
      │
      ▼
Reserve FlightSeat
      │
      ▼
Create Payment
      │
 ┌────┴─────┐
 │          │
 ▼          ▼
Failed   Success
 │          │
 ▼          ▼
Cancel    Confirm Booking
Booking        │
 │             ▼
 ▼        Book FlightSeat
Release        │
Seat           ▼
          Generate Ticket
                │
                ▼
       Update Loyalty Points
                │
                ▼
        Send Notification
                │
                ▼
             End



Admin Login
      │
      ▼
Dashboard
      │
      ├──────── Airport CRUD
      │
      ├──────── Aircraft CRUD
      │
      ├──────── Seat Management
      │
      ├──────── Flight CRUD
      │          │
      │          ▼
      │    Generate FlightSeats
      │
      ├──────── Booking Management
      │
      ├──────── Passenger Management
      │
      ├──────── Payment Management
      │
      ├──────── Ticket Management
      │
      ├──────── Loyalty Management
      │
      ├──────── Notification Management
      │
      └──────── Reports & Dashboard

*/