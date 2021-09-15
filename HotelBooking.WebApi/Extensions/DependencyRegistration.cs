using HotelBooking.Core;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.WebApi.Extensions
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IRepository<Room>, RoomRepository>()
                .AddScoped<IRepository<Customer>, CustomerRepository>()
                .AddScoped<IRepository<Booking>, BookingRepository>()
                .AddScoped<IBookingManager, BookingManager>()
                .AddTransient<IDbInitializer, DbInitializer>();
        }
    }
}
