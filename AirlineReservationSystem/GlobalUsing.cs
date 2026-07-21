// System

// Project Namespaces (عدّل حسب اسم مشروعك)
global using AirlineReservationSystem.Models.Enums;
global using AirlineReservationSystem.Models.Common;
global using AirlineReservationSystem.Models.Entities;
global using AirlineReservationSystem.Models.Identity;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using AirlineReservationSystem.Data;
global using AirlineReservationSystem.Repositories.Implementations;
global using AirlineReservationSystem.Repositories.Interfaces;
global using AirlineReservationSystem.Services.Interfaces;
global using AirlineReservationSystem.Services.Implementations;
global using Microsoft.Data.SqlClient;
global using AirlineReservationSystem.ViewModels.AirportVM;
global using System.Linq.Expressions;
global using AirlineReservationSystem.Queries;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using AirlineReservationSystem.ViewModels.Loyalty;
global using AirlineReservationSystem.ViewModels.LoyaltyAccountVM;
global using AirlineReservationSystem.ViewModels.IdentityVM;
global using System.Security.Claims;
global using AirlineReservationSystem.Mappings;

global using AirlineReservationSystem.ViewModels;
global using AirlineReservationSystem.ViewModels.Airport;
global using AirlineReservationSystem.ViewModels.Aircraft;
global using AirlineReservationSystem.ViewModels.Seat;
global using AirlineReservationSystem.ViewModels.Flight;
global using AirlineReservationSystem.ViewModels.BookingVM;
global using AirlineReservationSystem.ViewModels.PassengerVM;
global using AirlineReservationSystem.ViewModels.PaymentVM;
global using AutoMapper;
global using AirlineReservationSystem.Services.IServices;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Azure.Core;
global using System.Net.Sockets;

global using AirlineReservationSystem.Utility;

// ASP.NET Core MVC
global using Microsoft.AspNetCore.Mvc;
// Entity Framework Core
global using Microsoft.EntityFrameworkCore;
global using System;

global using System.Collections.Generic;
// Data Annotations
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;

//
global using System.Diagnostics;

global using System.Linq;
global using System.Reflection;
global using System.Threading.Tasks;

//
global using Mapster;
/*using System.ComponentModel.DataAnnotations.Schema;

 * using ECommerceStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
 */