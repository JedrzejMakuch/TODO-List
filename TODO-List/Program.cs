using LINQtoCSV;
using Marten.Linq.Parsing;
using TODO_List;
using TODO_List.Service;

var QuestList = new List<Quest>();

Console.WriteLine(@"Witaj w TODO Liscie zadan.
Komendy:
show - pokaz liste,
add nazwa_zadania - dodaj zadanie,
start id_zadania - zmien status zadania na 'w trakcie',
complete id_zadania - zmien status zadania na 'ukonczony',
export nazwa_pliku bez rozszerzenia .csv - eksportuj plik w formacie .csv,
import nazwa_pliku bez rozszerzenia .csv- importuj plik w formacie .csv,
exit - zamyka program.");
Console.WriteLine();

while (true)
{
    var command = Console.ReadLine().ToLower();
    var commandArrWithSplit = command.Split(' ');
    var service = new Service();
    var shortCommand = service.ExtractCommand(command);

    switch (shortCommand)
    {
        case "show":
            service.ShowList(QuestList);
            break;

        case "add":
            service.AddNewTask(commandArrWithSplit[1], QuestList);
            break;

        case "start":
            service.Start(QuestList, commandArrWithSplit);
            break;

        case "complete":
            service.Complete(QuestList, commandArrWithSplit);
            break;

        case "export":
            if (string.IsNullOrEmpty(commandArrWithSplit[1]))
            {
                service.Error();
                continue;
            }
            service.Export(QuestList, commandArrWithSplit, service.GetCsvFileDescription(), service.GetCsvContext());
            break;

        case "import":
            if (!File.Exists(commandArrWithSplit[1] + ".csv"))
            {
                service.Error();
                continue;
            }
            service.Import(QuestList, commandArrWithSplit, service.GetCsvFileDescription(), service.GetCsvContext());
            break;

        case "exit":
            service.Exit();
            break;

        default:
            service.Error();
            break;
    }
}

