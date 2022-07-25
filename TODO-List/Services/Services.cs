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
                if (quest.Status == QuestStatus.Completed)
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
                    Status = QuestStatus.New
                };

                QuestList.Add(New);
                Console.WriteLine("Zadanie {0} zostalo dodane.", New.Id);
            }
            else { Error(); }
        }

        public void Start(string[] splitted)
        {
            if (splitted.Length < 2)
            { Error(); }
            else
            {
                var Id = int.TryParse(splitted[1], out int result);
                if (Id && result <= QuestList.Count && result > 0)
                {
                    var quest = QuestList.Single(q => q.Id == result);
                    if (quest.Status == QuestStatus.InProgres || quest.Status == QuestStatus.Completed)
                    {
                        Console.WriteLine("Zadanie juz jest rozpoeczete, lub zostalo juz zakonczone.");
                    }
                    else
                    {
                        quest.Status = QuestStatus.InProgres;
                        quest.DateOfEnd = DateTime.Now;
                        Console.WriteLine("Zadanie {0}, zostalo rozpoczete.", quest.Id);
                    }
                }
                else
                    Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", splitted[1]);
            }
        }

        public void Complete(string[] splitted)
        {
            if (splitted.Length < 2)
            {
                Error();
            }
            else
            {
                var Id = int.TryParse(splitted[1], out int result);
                if (Id && result <= QuestList.Count && result > 0)
                {
                    var quest = QuestList.Single(q => q.Id == result);
                    if (quest.Status == QuestStatus.Completed || quest.Status == QuestStatus.New)
                    {
                        Console.WriteLine("Zadanie juz zostalo zakonczone, lub nie zostalo rozpoczete.");
                    }
                    else
                    {
                        quest.Status = QuestStatus.Completed;
                        quest.DateOfEnd = DateTime.Now;
                        Console.WriteLine("Zadanie {0}, zostalo zakonczone.", quest.Id);
                    }
                }
                else
                    Console.WriteLine("Zadanie z podanym Id: {0}, nie istnieje.", splitted[1]);
            }
        }

        public void Export(string[] splitted)
        {
            if (splitted.Length < 2)
            {
                Error();
            }
            else
            {
                var csvContext = GetCsvContext();
                var csvDescription = GetCsvFileDescription();
                csvContext.Write(QuestList, splitted[1] + ".csv", csvDescription);
                Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal exportowany", splitted[1]);
            }
        }

        public void Import(string[] splitted)
        {
            if (splitted.Length < 2 || !File.Exists(splitted[1] + ".csv"))
            {
                Error();
            }
            else
            {
                try
                {

                    var csvContext = GetCsvContext();
                    var csvDescription = GetCsvFileDescription();
                    var imported = csvContext.Read<Quest>(splitted[1] + ".csv", csvDescription);
                    foreach (var quest in imported)
                    {

                        var New = new Quest
                        {
                            Id = QuestList.Count + 1,
                            Name = quest.Name,
                            DateOfStart = quest.DateOfStart,
                            Status = quest.Status,
                            DateOfEnd = quest.DateOfEnd.HasValue ? DateTime.Now : null,
                        };

                        QuestList.Add(New);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Nie mozna otworzyc pliku o nazwie {0}", splitted[1]);
                }
                    Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal importowany", splitted[1]);
            }


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
        private readonly Dictionary<QuestStatus, string> Dictonary = new()
        {
            { QuestStatus.New, "Nowy" },
            { QuestStatus.InProgres, "W Trakcie" },
            { QuestStatus.Completed, "Zakonczony" }
     };

    }
}
