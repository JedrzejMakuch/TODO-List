using LINQtoCSV;
using Marten.Linq.Parsing;
using TODO_List;


var QuestList = new List<Quest>();

Console.WriteLine(@"Witaj w TODO Liscie zadan.
Komendy:
show - pokaz liste,
add nazwa_zadania - dodaj zadanie,
start id_zadania - zmien status zadania na 'w trakcie',
complete id_zadania - zmien status zadania na 'ukonczony',
export nazwa_pliku - eksportuj plik w formacie .csv,
import nazwa_pliku - importuj plik w formacie .csv,
exit - zamyka program.");
Console.WriteLine();
  var Id = 0;
  while (true)
  {
    var command = Console.ReadLine().ToLower();
    if (command == "show")
    {
        Console.WriteLine("Lista zadan:");
        if (QuestList.Count == 0)
            Console.WriteLine("Lista zadan jest aktualnie pusta.");
        
        foreach (var quest in QuestList)
        {
            if (quest.Status == "Zakonczony")
                Console.WriteLine(@"Zadanie: Nazwa: {0}, Data utworzenia: {1}, Status: {2}, Data ukończenia: {3}", quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status, quest.DateOfEnd.Value.ToShortDateString());
            else
                Console.WriteLine(@"Zadanie: Nazwa: {0}, Data utworzenia: {1}, Status: {2}", quest.Name, quest.DateOfStart.ToShortDateString(), quest.Status);
        }
    }
    else if (command.Contains("add"))
    {
        var commandArr = command.Split(" ");
        QuestList.Add(new Quest { Id = Id++, DateOfStart = DateTime.Now, Name = commandArr[1], Status = "Nowy" });
        Console.WriteLine("Zadanie zostalo dodane.");
    }
    else if (command.Contains("start"))
    {
        var commandArr = command.Split(" ");
        bool containsItem = QuestList.Any(x => x.Id == (Convert.ToInt32(commandArr[1])-1));
        if (!containsItem)
        {
            Console.WriteLine(new Exception("Niewlasciwe Id zadania."));
            continue;
        }
        if(QuestList.ElementAt(Convert.ToInt32(commandArr[1]) - 1).Status == "Zakonczony")
        {
            Console.WriteLine(new Exception("Zadanie zostalo zakonczone wczesniej."));
        } else
        {
            QuestList.ElementAt(Convert.ToInt32(commandArr[1]) - 1).Status = "W trakcie";
            Console.WriteLine("Zadanie zostalo rozpoczete.");
        }
        
    }
    else if (command.Contains("complete"))
    {
        var commandArr = command.Split(" ");
        bool containsItem = QuestList.Any(x => x.Id == (Convert.ToInt32(commandArr[1]) - 1));
        if (!containsItem || string.IsNullOrEmpty(commandArr[1]))
        {
            Console.WriteLine(new Exception("Niewlasciwe Id zadania."));
            continue;
        }
        if (QuestList.ElementAt(Convert.ToInt32(commandArr[1]) - 1).Status == "Nowy")
        {
            Console.WriteLine(new Exception("Zadanie, nie zostalo jeszcze rozpoczete."));
        } else
        {
            QuestList.ElementAt(Convert.ToInt32(commandArr[1]) - 1).Status = "Zakonczony";
            QuestList.ElementAt(Convert.ToInt32(commandArr[1]) - 1).DateOfEnd = DateTime.Now;
            Console.WriteLine("Zadanie zostalo zakonczone.");
        }

    }else if(command.Contains("export"))
    {
        var commandArr = command.Split(" ");
        if (string.IsNullOrEmpty(commandArr[1]))
        {
            Console.WriteLine(new Exception("Niewlasciwa nazwa pliku zadania."));
            continue;
        }
        var csvDescription = new CsvFileDescription
        {
            FirstLineHasColumnNames = true,
        };

        var csvContext = new CsvContext();
        csvContext.Write(QuestList, commandArr[1] + ".csv", csvDescription);
        Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal exportowany", commandArr[1]);
    } else if(command.Contains("import")) 
    {
        var commandArr = command.Split(" ");
        if (!File.Exists(commandArr[1] + ".csv"))
        {
            Console.WriteLine(new Exception("Niewlasciwa nazwa pliku"));
            continue;
        }

        var csvDescription = new CsvFileDescription
        {
            FirstLineHasColumnNames = true,
        };

        var csvContext = new CsvContext();
        var importwany = csvContext.Read<Quest>(commandArr[1] + ".csv", csvDescription);
        QuestList.AddRange(importwany);
        Console.WriteLine("Plik w formacie csv, o nazwie '{0}' zostal importowany", commandArr[1]);
    }
    else if(command == "exit")
    {
        Console.WriteLine("Milego dnia.");
        Environment.Exit(0);
    }
    else
        Console.WriteLine(new Exception("Niewlasciwa komenda."));
    
  }



     