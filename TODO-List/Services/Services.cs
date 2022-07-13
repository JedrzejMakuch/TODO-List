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

        public CsvContext GetCsvContext()
        {
            var csvContext = new CsvContext();
            return csvContext;
        }

        public CsvFileDescription GetCsvFileDescription()
        {
            var fileDescription = new CsvFileDescription()
            {
                FirstLineHasColumnNames = true,
            };
            return fileDescription;
        }

        public void ShowList(List<Quest> QuestList)
        {
            Console.WriteLine("Lista zadan:");
            if (QuestList.Count == 0)
                Console.WriteLine("Lista jest aktualnie pusta");

            foreach (var quest in QuestList)
            {
                if (quest.Status == StatusFinish())
                    Console.WriteLine(@"Zadanie {0}: Nazwa: {1}, Data utworzenia: {2}, Status: {3}, Data ukończenia: {4}", quest.Id, quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status, quest.DateOfEnd.Value.ToShortDateString());
                else
                    Console.WriteLine(@"Zadanie {0}: Nazwa: {1}, Data utworzenia: {2}, Status: {3}", quest.Id, quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status);
            }
        }

        public void AddNewTask(string commandWithSplit, List<Quest> QuestList)
        {
            if (!string.IsNullOrWhiteSpace(commandWithSplit))
            {
                var New = new Quest
                {
                    Id = QuestList.Count + 1,
                    Name = commandWithSplit,
                    DateOfStart = DateTime.Now,
                    Status = StatusNew()
                };

                QuestList.Add(New);
                Console.WriteLine("Zadanie {0} zostalo dodane.", New.Id);
            } else
                Error();
            
        }

        public void Start(List<Quest> QuestList, string[] commandArrWithSplit)
        {
            var Id = int.TryParse(commandArrWithSplit[1], out int result);
            if (Id && !string.IsNullOrEmpty(commandArrWithSplit[1]) && result <= QuestList.Count && result > 0)
            {
                foreach (var quest in QuestList)
                {
                    if (result == quest.Id)
                    {
                        if (quest.Status == StatusInProgres())
                        {
                            Console.WriteLine("Zadanie juz jest rozpoczete.");
                        }else
                        {
                                quest.Status = StatusInProgres();
                                Console.WriteLine("Zadanie {0}, zostalo rozpoczete.", quest.Id);
                        } 
                    }
                }
            }
            else
                Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", commandArrWithSplit[1]);
        }

        public void Complete(List<Quest> QuestList, string[] commandArrWithSplit)
        {
            var Id = int.TryParse(commandArrWithSplit[1], out int result);
            if (Id && !string.IsNullOrEmpty(commandArrWithSplit[1]) && result <= QuestList.Count && result > 0)
            {
                foreach (var quest in QuestList)
                {
                    if (result == quest.Id)
                    {
                        if (quest.Status == StatusFinish())
                        {
                            Console.WriteLine("Zadanie juz zostalo zakonczone.");
                        } else if (quest.Status == StatusNew())
                        {
                            Console.WriteLine("Nie mozna zakonczyc nie rozpoczetego zadania.");
                        } else
                        {
                            quest.Status = StatusFinish();
                            Console.WriteLine("Zadanie {0}, zostalo zakonczone.", quest.Id);
                        }
                    }
                }
            }
            else
                Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", commandArrWithSplit[1]);
        }

        public void Export(List<Quest> QuestList, string[] commandArrWithSplit, CsvFileDescription csvDescription, CsvContext csvContext)
        {
            csvContext.Write(QuestList, commandArrWithSplit[1] + ".csv", csvDescription);
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal exportowany", commandArrWithSplit[1]);
        }

        public void Import(List<Quest> QuestList, string[] commandArrWithSplit, CsvFileDescription csvDescription, CsvContext csvContext)
        {
            var importwany = csvContext.Read<Quest>(commandArrWithSplit[1] + ".csv", csvDescription);
            QuestList.AddRange(importwany);
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal importowany", commandArrWithSplit[1]);
        }

        public void Error()
        {
            Console.WriteLine("Niewlasciwa komeda lub niewlasciwe dzialanie.");
        }

        public void Exit()
        {
            Console.WriteLine("Milego dnia.");
            Environment.Exit(0);
        }

        public string ExtractCommand(string command)
        {
            var input = command.Split(' ');
            return input[0];
        }
        public string StatusInProgres()
        {
            return "W trakcie";
        }
        public string StatusFinish()
        {
            return "Zakonczony";
        }
        public string StatusNew()
        {
            return "Nowy";
        }
    }
}
