using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientConsoleApp.Models
{
    public class CampModel
    {
        public string Name { get; set; }
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; }
        public int Length { get; set; } = 1;
        public string Venue { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
        public ICollection<TalkModel> Talks { get; set; }

        public override bool Equals(object obj)
        {
            CampModel extObj = obj as CampModel;

            return extObj == null ?
                false : (extObj.Name.Equals(this.Name) && extObj.Moniker.Equals(this.Moniker));
        }

        public override int GetHashCode()
        {
            return this.Moniker.GetHashCode();
        }
    }
}
