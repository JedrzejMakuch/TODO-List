using LINQtoCSV;

namespace TODO_List.Service
{
    public class Service
    {

        private List<Quest> QuestList { get; set; }

        public Service()
        {
            QuestList = new List<Quest>();
        }

        private CsvContext GetCsvContext()
        {
            var csvContext = new CsvContext();
            return csvContext;
        }

        private CsvFileDescription GetCsvFileDescription()
        {
            var fileDescription = new CsvFileDescription()
            {
                FirstLineHasColumnNames = true,
            };
            return fileDescription;
        }

        public void ShowList()
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

        public void AddNewTask(string[] splitted)
        {
            if (splitted.Length >= 2)
            {
                var New = new Quest
                {
                    Id = QuestList.Count + 1,
                    Name = splitted[1],
                    DateOfStart = DateTime.Now,
                    Status = StatusNew()
                };

                QuestList.Add(New);
                Console.WriteLine("Zadanie {0} zostalo dodane.", New.Id);
            }
            else { Error(); }
        }

        public void Start(string[] commandArrWithSplit, string command)
        {
            if (command.Length == 5)
            { Error(); }
            else
            {
                var Id = int.TryParse(commandArrWithSplit[1], out int result);
                if (Id && !string.IsNullOrEmpty(commandArrWithSplit[1]) && result <= QuestList.Count && result > 0)
                {
                    var quest = QuestList.Single(q => q.Id == result);
                    if (quest.Status == StatusInProgres())
                    {
                        Console.WriteLine("Zadanie juz jest rozpoeczete.");
                    }
                    else
                    {
                        quest.Status = StatusInProgres();
                        quest.DateOfEnd = DateTime.Now;
                        Console.WriteLine("Zadanie {0}, zostalo rozpoczete.", quest.Id);
                    }
                }
                else
                    Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", commandArrWithSplit[1]);
            }
        }

        public void Complete(string[] commandArrWithSplit, string command)
        {
            if (command.Length == 8)
            {
                Error();
            }
            else
            {
                var Id = int.TryParse(commandArrWithSplit[1], out int result);
                if (Id && !string.IsNullOrEmpty(commandArrWithSplit[1]) && result <= QuestList.Count && result > 0)
                {
                    var quest = QuestList.Single(q => q.Id == result);
                    if (quest.Status == StatusFinish())
                    {
                        Console.WriteLine("Zadanie juz zostalo zakonczone.");
                    }
                    else if (quest.Status == StatusNew())
                    {
                        Console.WriteLine("Nie mozna zakonczyc nie rozpoczetego zadania.");
                    }
                    else
                    {
                        quest.Status = StatusFinish();
                        Console.WriteLine("Zadanie {0}, zostalo zakonczone.", quest.Id);
                    }
                }
                else
                    Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", commandArrWithSplit[1]);
            }
        }

        public void Export(string[] commandArrWithSplit)
        {
            var csvContext = GetCsvContext();
            var csvDescription = GetCsvFileDescription();
            csvContext.Write(QuestList, commandArrWithSplit[1] + ".csv", csvDescription);
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal exportowany", commandArrWithSplit[1]);
        }

        public void Import(string[] commandArrWithSplit)
        {
            var csvContext = GetCsvContext();
            var csvDescription = GetCsvFileDescription();
            var importwany = csvContext.Read<Quest>(commandArrWithSplit[1] + ".csv", csvDescription);
            foreach (var quest in importwany)
            {
                if (quest.Status == StatusFinish())
                {
                    var New = new Quest
                    {
                        Id = QuestList.Count + 1,
                        Name = quest.Name,
                        DateOfStart = quest.DateOfStart,
                        Status = quest.Status,
                        DateOfEnd = quest.DateOfEnd,
                    };
                    QuestList.Add(New);
                }
                else
                {
                    var New = new Quest
                    {
                        Id = QuestList.Count + 1,
                        Name = quest.Name,
                        DateOfStart = quest.DateOfStart,
                        Status = quest.Status,
                    };
                    QuestList.Add(New);
                }
            }
            Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal importowany", commandArrWithSplit[1]);
        }

        public static void Error()
        {
            Console.WriteLine("Niewlasciwa komeda lub niewlasciwe dzialanie.");
        }

        public static void Exit()
        {
            Console.WriteLine("Milego dnia.");
            Environment.Exit(0);
        }

        public static string[] SplitCommand(string command)
        {
            var splitted = command.Split(' ');
            splitted = splitted.Where(s => string.IsNullOrWhiteSpace(s) == false).ToArray();

            return splitted.Any()
                ? splitted
                : new string[] { string.Empty };
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
