using Baseline.ImTools;
using DocumentFormat.OpenXml.Office.CustomUI;
using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_List.Service
{
    public class Service 
    {
        public List<Quest> QuestList { get; set; }
        public Service()
        {
            QuestList = new List<Quest>();
        }

        public static void ShowList(List<Quest> QuestList)
        {
            Console.WriteLine("Lista zadan:");
            if (QuestList.Count == 0)
                Error();

            foreach (var quest in QuestList)
            {
                if (quest.Status == "Zakonczony")
                    Console.WriteLine(@"Zadanie: Nazwa: {0}, Data utworzenia: {1}, Status: {2}, Data ukończenia: {3}", quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status, quest.DateOfEnd.Value.ToShortDateString());
                else
                    Console.WriteLine(@"Zadanie: Nazwa: {0}, Data utworzenia: {1}, Status: {2}", quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status);
            }
        }
        public static int AddNewTask(List<Quest> QuestList, int Id, string[] commandArrWithSplit)
        {
            QuestList.Add(new Quest { Id = Id++, DateOfStart = DateTime.Now, Name = commandArrWithSplit[1], Status = "Nowy" });
            Console.WriteLine("Zadanie zostalo dodane.");
            return Id;
        }
        public static void Start(List<Quest> QuestList, string[] commandArrWithSplit)
        {
            QuestList.ElementAt(Convert.ToInt32(commandArrWithSplit[1]) - 1).Status = "W trakcie";
            Console.WriteLine("Zadanie zostalo rozpoczete.");
        }
        public static void Complete(List<Quest> QuestList, string[] commandArrWithSplit)
        {
            QuestList.ElementAt(Convert.ToInt32(commandArrWithSplit[1]) - 1).Status = "Zakonczony";
            QuestList.ElementAt(Convert.ToInt32(commandArrWithSplit[1]) - 1).DateOfEnd = DateTime.Now;
            Console.WriteLine("Zadanie zostalo zakonczone.");
        }
        public static void Export(List<Quest> QuestList, string[] commandArrWithSplit, CsvFileDescription csvDescription, CsvContext csvContext)
        {
            csvContext.Write(QuestList, commandArrWithSplit[1] + ".csv", csvDescription);
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal exportowany", commandArrWithSplit[1]);
        }

        public static void Import(List<Quest> QuestList, string[] commandArrWithSplit, CsvFileDescription csvDescription, CsvContext csvContext)
        {
            var importwany = csvContext.Read<Quest>(commandArrWithSplit[1] + ".csv", csvDescription);
            QuestList.AddRange(importwany);
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal importowany", commandArrWithSplit[1]);
        }

        public static void Error()
        {
            Console.WriteLine(new Exception("Niewslasciwa komeda lub niewlasciwe dzialanie."));
        }
        public static void Exit()
        {
            Console.WriteLine("Milego dnia.");
            Environment.Exit(0);
        }

    }

}
