using System.Globalization;
using CsvHelper;
using HtmlAgilityPack;

namespace Scraper
{
    class Program
    {
        const string URL = "https://participant.facilitymanagerplus.com/";
        const string AvailableStudiesSubNavXPath = ".//a[@id='ctl00_StudiesHLink']/following-sibling::ul/li/a";
        const string ProjectDetailsXPath = ".//div[@class='project-detail']";
        const string ProjectDollarsXPath = ".//i[@class='fa fa-dollar']";
        const string ProjectAgeXPath = ".//i[@class='fa fa-birthday-cake']/parent::node()";
        const string ProjectGenderXPath = ".//i[@class='fa fa-user']/parent::node()";
        const string ProjectTypeXPath = ".//i[@class='fa fa-comment']/parent::node()";

        static readonly HtmlWeb Web = new();
        static void Main()
        {
            HtmlDocument doc;

            try
            {
                doc = Web.Load(URL + "AvailableStudies.aspx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var subnav = doc.DocumentNode.SelectNodes(AvailableStudiesSubNavXPath);
            var studies = new List<Study>();

            foreach (var anchor in subnav)
            {
                string studyLocationURL = anchor.GetAttributeValue("href", string.Empty);

                if (studyLocationURL.Equals(string.Empty)) continue;

                HtmlDocument studyLocationDoc;
                try
                {
                    studyLocationDoc = Web.Load(URL + studyLocationURL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                var details = studyLocationDoc.DocumentNode.SelectNodes(ProjectDetailsXPath);

                foreach (var detail in details)
                {

                    
                    var study = new Study
                    {
                        Title = detail.Element("h3")?.InnerText?.Trim()?? string.Empty,
                        Location = anchor.InnerText,
                        Dollars = detail.SelectNodes(ProjectDollarsXPath)?.Count ?? 0,
                        Age = detail.SelectNodes(ProjectAgeXPath)?[0].InnerText?.Remove(0,6) ?? string.Empty,
                        Gender = detail.SelectNodes(ProjectGenderXPath)?[0].InnerText?.Remove(0,9) ?? string.Empty,
                        Type = detail.SelectNodes(ProjectTypeXPath)?[0].InnerText?.Remove(0,13) ?? string.Empty,
                        Description = detail.Element("p")?.InnerText?.Trim().Replace("\n", "").Replace("\r", "") ?? string.Empty,
                        Details = detail.Element("a")?.GetAttributeValue("href", string.Empty) ?? string.Empty
                    };

                    studies.Add(study);

                    Console.WriteLine($"Location: {study.Location}, Title: {study.Title}, Dollars: {study.Dollars}, Age: {study.Age}, Gender: {study.Gender}, Type: {study.Type}, Details: {study.Details}, Desc: {study.Description[..20]}...");
                }

                System.Threading.Thread.Sleep(1000);
            }

            using var writer = new StreamWriter("studies.csv");

            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(studies);
        }
    }

    class Study
    {
        public string? Title {get;set;}
        public int Dollars {get;set;}
        public string? Location {get;set;}
        public string? Age {get;set;}
        public string? Gender {get;set;}
        public string? Type {get;set;}
        public string? Description {get;set;}
        public string? Details {get;set;}
    }
}

