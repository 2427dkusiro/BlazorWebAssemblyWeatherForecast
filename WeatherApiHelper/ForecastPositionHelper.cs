using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace WeatherApiHelper
{
    public static class ForecastPositionHelper
    {
        public static ForecastPositionIndex ParseForecastPositionsXml(Stream stream)
        {
            string str;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                str = streamReader.ReadToEnd();
            }

            var xDocument = XDocument.Parse(str);
            var groups = xDocument.Root.Elements()
                 .Select(pref =>
                 {
                     var cities = pref.Elements().Where(elem => elem.Name == "city")
                     .Select(city =>
                     {
                         string cityName = city.Attribute("title").Value;
                         int id = int.Parse(city.Attribute("id").Value);
                         return new ForecastPosition(cityName, id);
                     }).ToArray();
                     string prefName = pref.Attribute("title").Value;
                     var group = new ForecastPositionGroup(prefName, cities);
                     return group;
                 });

            var dict = groups.SelectMany(x => x.ForecastPositions).ToDictionary(x => x.CityName, x => x.Id);
            return new ForecastPositionIndex(dict, groups.ToArray());
        }
    }

    public record ForecastPositionIndex(Dictionary<string, int> PositionDictionary, IList<ForecastPositionGroup> PositionGroups);

    public record ForecastPosition(string CityName, int Id);

    public record ForecastPositionGroup(string GroupName, IList<ForecastPosition> ForecastPositions);
}
