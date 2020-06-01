using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kata.Models
{
    public class Trips
    {
        public String driver;
        public int distance;
        public int mph;

        public Trips(String name, int dist, int milesph)
        {
            driver = name;
            distance = dist;
            mph = milesph;
        }
    }
}