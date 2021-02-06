using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTour
{
    class City
    {
        private string name = "N/A";
        private string country = "N/A";
        private int occurrence = 0;

        public string getName()
        {
            return this.name;
        }

        public void resetOccurrence()
        {
            this.occurrence = 0;
        }

        public string getCountry()
        {
            return this.country;
        }

        public int GetOccurrence()
        {
            return this.occurrence;
        }

        public void increaseOccurrence()
        {
            this.occurrence++;
        }

        public City(string name, string country)
        {
            this.name = name;
            this.country = country;
            this.occurrence = 0;
        }

        public City(string name, string country, string occurrence)
        {
            this.name = name;
            this.country = country;
            if(!occurrence.Equals(String.Empty))
            {
                try
                {
                    this.occurrence = Convert.ToInt32(occurrence);
                }
                catch
                {
                    this.occurrence = 0;
                }
            }
            else
            {
                this.occurrence = 0;
            }
        }
    }
}
