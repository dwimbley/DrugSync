using System;
using System.Collections.Generic;
using System.Linq;

namespace DrugSync.System
{
    public class DrugSyncer
    {
        public List<Drug> Sync(List<Drug> drugs)
        {
            var calculatedDrugs = new List<Drug>();

            foreach (var item in drugs)
            {
                var endingDate = item.FillDate.AddYears(item.PlanYears);

                var calculatedFillDate = item.FillDate;

                while (calculatedFillDate < endingDate)
                {
                    calculatedFillDate = item.IsAnchor ? calculatedFillDate.AddDays(item.DaysSupply) : calculatedFillDate.AddDays(30).AddDays(item.FillBeforeDays * -1);

                    var model = new Drug();
                    model.IsAnchor = item.IsAnchor;
                    model.DaysSupply = item.DaysSupply;
                    model.FillBeforeDays = item.FillBeforeDays;
                    model.AllowableDaysDifference = item.AllowableDaysDifference;
                    model.Name = item.Name;
                    model.FillDate = calculatedFillDate;
                    calculatedDrugs.Add(model);
                }
            }

            var anchorDrugs = calculatedDrugs.Where(m => m.IsAnchor);
            var allOtherDrugs = calculatedDrugs.Where(m => !m.IsAnchor).OrderBy(m=>m.Name).ThenBy(m=>m.AnchorName);

            Console.WriteLine("anchorDrugs.Count :: {0}", anchorDrugs.Count());

            var combinedDrugs = new List<Drug>();

            foreach (var drug in allOtherDrugs)
            {
                foreach (var item in anchorDrugs)
                {
                    var achorDate = item.FillDate;
                    var daysDifference = (drug.FillDate - achorDate).Days;

                    if (Math.Abs(daysDifference) > 7)
                    {
                        continue;
                    }

                    var isExactMatch = drug.FillDate == achorDate;
                    var isFuzzyMatch = Math.Abs(daysDifference) <= drug.AllowableDaysDifference;

                    var matchedDrug = new Drug();
                    matchedDrug.Name = drug.Name;
                    matchedDrug.FillDate = drug.FillDate;
                    matchedDrug.IsAnchor = drug.IsAnchor;
                    matchedDrug.DaysSupply = drug.DaysSupply;
                    matchedDrug.FillBeforeDays = drug.FillBeforeDays;
                    matchedDrug.AllowableDaysDifference = drug.AllowableDaysDifference;
                    matchedDrug.Name = drug.Name;


                   matchedDrug.IsMatchToAnchor = isExactMatch;
                   matchedDrug.IsFuzzyMatchToAnchor = isExactMatch ? false : isFuzzyMatch;
                   matchedDrug.AnchorName = item.Name;
                   matchedDrug.DaysDifference = daysDifference;
                    matchedDrug.ComparedDate = achorDate;

                    combinedDrugs.Add(matchedDrug);
                }
            }

            combinedDrugs.AddRange(anchorDrugs);

            return combinedDrugs;
        }
    
    }



    public class Drug
    {
        public string Name { get; set; }
        public bool IsAnchor { get; set; }
        public DateTime FillDate { get; set; }
        public int DaysSupply { get; set; }
        public int FillBeforeDays { get; set; }
        public int PlanYears { get; set; }
        public int AllowableDaysDifference { get; set; }


        public DateTime ComparedDate { get; set; }
        public bool IsMatchToAnchor { get; set; }
        public bool IsFuzzyMatchToAnchor { get; set; }
        public string AnchorName { get; set; }
        public int DaysDifference { get; set; }

    }
}
