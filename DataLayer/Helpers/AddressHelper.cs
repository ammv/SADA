using System.Linq;

namespace DataLayer.Helpers
{
    public static class AddressHelper
    {
        public static Address GetOrCreateAddress(string country, string region, string city, string district, string street, string buildingAddress, string corpus, int floor, string flat, string postalIndex)
        {
            Address address;
            using (var ctx = new SADAEntities())
            {
                Country countryFind = ctx.Country.FirstOrDefault(c => c.Name == country) ?? new Country { Name = country };

                Region regionFind = ctx.Region.FirstOrDefault(r => r.Name == region && r.CountryID == countryFind.ID) ?? new Region { Name = region };

                City cityFind = ctx.City.FirstOrDefault(c => c.Name == city && c.RegionID == regionFind.ID) ?? new City { Name = city };

                District districtFind = ctx.District.FirstOrDefault(d => d.Name == district && d.City.ID == cityFind.ID) ?? new District { Name = district };

                Street streetFind = ctx.Street.FirstOrDefault(s => s.Name == street && s.District.ID == districtFind.ID) ?? new Street { Name = street };

                address = ctx.Address.FirstOrDefault(a => a.Street.ID == streetFind.ID &&
                a.Floor == floor &&
                a.Flat == flat &&
                a.Corpus == corpus &&
                a.BuildingNumber == buildingAddress) ??
                CreateAddress(countryFind, regionFind, cityFind, districtFind, streetFind, buildingAddress, corpus, floor, flat, postalIndex);
            }

            return address;

            /// Китай, Чжунь-Шунь, Пхеньян, переулок Чуй-буй, Чжахнь-Мань, 10С, 1, 23, 5335323
        }

        private static Address CreateAddress(Country country, Region region, City city, District district, Street street, string buildingAddress, string corpus, int floor, string flat, string postalIndex)
        {
            Address address;

            using (var ctx = new SADAEntities())
            {
                country = country.ID == 0 ? ctx.Country.Add(country) : country;
                if (region.ID == 0)
                {
                    region.Country = country;
                    region = ctx.Region.Add(region);
                }
                if (city.ID == 0)
                {
                    city.Region = region;
                    city = ctx.City.Add(city);
                }
                if (district.ID == 0)
                {
                    district.City = city;
                    district = ctx.District.Add(district);
                }
                if (street.ID == 0)
                {
                    street.District = district;
                    street = ctx.Street.Add(street);
                }

                address = ctx.Address.Add(new Address { Street = street, BuildingNumber = buildingAddress, Floor = floor, Flat = flat, PostalIndex = postalIndex, Corpus = corpus});
                ctx.SaveChanges();
            }

            return address;
        }
    }
}