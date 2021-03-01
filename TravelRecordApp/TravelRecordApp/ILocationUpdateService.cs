using System;
using System.Collections.Generic;
using System.Text;

namespace TravelRecordApp
{
    public interface ILocationUpdateService
    {
        void GetUserLocation();
        event EventHandler<ILocationEvenArgs> LocationChanged;
    }

    public interface ILocationEvenArgs
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }

}
