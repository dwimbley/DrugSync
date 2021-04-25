using System.Collections.Generic;
using System.IO;
using System.Linq;
using DrugSync.System;

namespace DrungSync.Console
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();

            var drug1 = new Drug();
            drug1.Name = "Rittalin";
            drug1.IsAnchor = true;
            drug1.DaysSupply = 30;
            drug1.FillBeforeDays = 3;
            drug1.PlanYears = 1;
            drug1.AllowableDaysDifference = 4;
            drug1.FillDate = new DateTime(2021, 5, 2);

            var drug1a = new Drug();
            drug1a.Name = "Adderall";
            drug1a.IsAnchor = true;
            drug1a.DaysSupply = 30;
            drug1a.FillBeforeDays = 3;
            drug1a.PlanYears = 1;
            drug1a.AllowableDaysDifference = 4;
            drug1a.FillDate = new DateTime(2021, 5, 21);


            var drug2 = new Drug();
            drug2.Name = "Oxycotin";
            drug2.IsAnchor = false;
            drug2.DaysSupply = 30;
            drug2.FillBeforeDays = 3;
            drug2.PlanYears = 1;
            drug2.AllowableDaysDifference = 3;
            drug2.FillDate = new DateTime(2021, 5, 25);

            var drug3 = new Drug();
            drug3.Name = "Vicadin";
            drug3.IsAnchor = false;
            drug3.DaysSupply = 30;
            drug3.FillBeforeDays = 3;
            drug3.PlanYears = 1;
            drug3.AllowableDaysDifference = 4;
            drug3.FillDate = new DateTime(2021, 4, 29);

            var drug4 = new Drug();
            drug4.Name = "Tramdaol";
            drug4.IsAnchor = false;
            drug4.DaysSupply = 20;
            drug4.FillBeforeDays = 3;
            drug4.PlanYears = 1;
            drug4.AllowableDaysDifference = 3;
            drug4.FillDate = new DateTime(2021, 6, 5);

            var allDrugs = new List<Drug>();
            allDrugs.Add(drug1);
            allDrugs.Add(drug2);
            allDrugs.Add(drug3);
            allDrugs.Add(drug4);
            allDrugs.Add(drug1a);
            
            var syncer = new DrugSyncer();
            var output = syncer.Sync(allDrugs);

            var fuzzyMatches = output.Where(m => !m.IsAnchor && m.IsFuzzyMatchToAnchor);
            var matches = output.Where(m => !m.IsAnchor && m.IsMatchToAnchor);

            var dateWithMostMatches = matches.Select(m => new { m.FillDate }).GroupBy(m=>m.FillDate).Select(m=> new
            {
                FillDate = m.Key,
                Count = m.Count()
            }).OrderByDescending(m=>m.Count).FirstOrDefault();

            var dateWithMostFuzzyMatches = fuzzyMatches.Select(m => new { m.FillDate }).GroupBy(m=>m.FillDate).Select(m=> new
            {
                FillDate = m.Key,
                Count = m.Count()
            }).OrderByDescending(m=>m.Count).FirstOrDefault();


            var drugsWithMatchDate = matches.Where(m => m.FillDate == dateWithMostMatches.FillDate).Count();
            var drugsWithFuzzyMatchDate = fuzzyMatches.Where(m => m.FillDate == dateWithMostFuzzyMatches.FillDate).Count();

            Console.WriteLine("Date with most matches {0}", dateWithMostMatches.FillDate);
            Console.WriteLine("Date with most fuzzy matches {0}", dateWithMostFuzzyMatches.FillDate);

            Console.WriteLine("drugsWithMatchDate Count :: {0}", drugsWithMatchDate);
            Console.WriteLine("drugsWithFuzzyMatchDate Count :: {0}", drugsWithFuzzyMatchDate);
            




            Console.WriteLine("------------------------");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("-----        Anchor Drugs");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("------------------------");

            foreach (var item in output.Where(m => m.IsAnchor))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Anchor Drug");
                Console.WriteLine("Drug Name :: {0}", item.Name);
                Console.WriteLine("Fill Date :: {0}", item.FillDate);
                Console.WriteLine("*******************************");
                Console.WriteLine(Environment.NewLine);
            }

            Console.WriteLine("------------------------");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("-----        Matched Drugs");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("------------------------");


            foreach (var item in output.Where(m => !m.IsAnchor && m.IsMatchToAnchor))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("NEW DRUG");
                Console.WriteLine("Drug Name :: {0}", item.Name);
                Console.WriteLine("Fill Date :: {0}", item.FillDate);
                Console.WriteLine("Is match :: {0}", item.IsMatchToAnchor);
                Console.WriteLine("Is fuzzy match :: {0}", item.IsFuzzyMatchToAnchor);
                Console.WriteLine("Anchor Drug name :: {0}", item.AnchorName);
                Console.WriteLine("Days difference :: {0}", item.DaysDifference);
                Console.WriteLine("Compared Date :: {0}", item.ComparedDate);
                Console.WriteLine("*******************************");
                Console.WriteLine(Environment.NewLine);
            }

            Console.WriteLine("------------------------");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("-----        Fuzzy Match Drugs");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("------------------------");



            foreach (var item in output.Where(m => !m.IsAnchor && m.IsFuzzyMatchToAnchor))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("NEW DRUG");
                Console.WriteLine("Drug Name :: {0}", item.Name);
                Console.WriteLine("Fill Date :: {0}", item.FillDate);
                Console.WriteLine("Is match :: {0}", item.IsMatchToAnchor);
                Console.WriteLine("Is fuzzy match :: {0}", item.IsFuzzyMatchToAnchor);
                Console.WriteLine("Anchor Drug name :: {0}", item.AnchorName);
                Console.WriteLine("Days difference :: {0}", item.DaysDifference);
                Console.WriteLine("Compared Date :: {0}", item.ComparedDate);
                Console.WriteLine("*******************************");
                Console.WriteLine(Environment.NewLine);
            }


            Console.WriteLine("------------------------");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("-----        Calculated Drugs");
            Console.WriteLine("-----");
            Console.WriteLine("-----");
            Console.WriteLine("------------------------");




            foreach (var item in output.Where(m => !m.IsAnchor && Math.Abs(m.DaysDifference) <= 7))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("NEW DRUG");
                Console.WriteLine("Drug Name :: {0}", item.Name);
                Console.WriteLine("Fill Date :: {0}", item.FillDate);
                Console.WriteLine("Is match :: {0}", item.IsMatchToAnchor);
                Console.WriteLine("Is fuzzy match :: {0}", item.IsFuzzyMatchToAnchor);
                Console.WriteLine("Anchor Drug name :: {0}", item.AnchorName);
                Console.WriteLine("Days difference :: {0}", item.DaysDifference);
                Console.WriteLine("Compared Date :: {0}", item.ComparedDate);
                Console.WriteLine("*******************************");
                Console.WriteLine(Environment.NewLine);
            }

            Console.WriteLine();
        }
    }
}
