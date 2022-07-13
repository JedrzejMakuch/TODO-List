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
export nazwa_pliku - eksportuj plik w formacie .csv,
import nazwa_pliku - importuj plik w formacie .csv,
exit - zamyka program.");
Console.WriteLine();
var Id = 0;

while (true)
{
    var command = Console.ReadLine().ToLower();

    var commandArrWithSplit = command.Split(' ');
    var csvDescription = new CsvFileDescription
    {
        FirstLineHasColumnNames = true,
    };
    var csvContext = new CsvContext();
    string statusFinish = "Zakonczony";
    string statusNew = "Nowy";
    string statusInProgres = "W trakcie";

    var service = new Service();

    //var shortCommand = ExtractCommand(command);
    //switch (shortCommand)
    //{
        
    //}

    if (command.StartsWith("show"))
    {
        service.ShowList(QuestList);
    }
    else if (command.StartsWith("add"))
    {
        Id = Service.AddNewTask(QuestList, Id, commandArrWithSplit);
    }
    else if (command.StartsWith("start"))
    {
        var commandInt = int.TryParse(commandArrWithSplit[1], out int result);
        bool containsItem = QuestList.Any(x => x.Id == result - 1);

        if (!containsItem || QuestList.ElementAt(result - 1).Status == statusFinish || QuestList.ElementAt(result - 1).Status == statusInProgres)
        {
            Service.Error();
            continue;
        }
        else
            Service.Start(QuestList, commandArrWithSplit);
    }
    else if (command.StartsWith("complete"))
    {
        var commandInt = int.TryParse(commandArrWithSplit[1], out int result);
        bool containsItem = QuestList.Any(x => x.Id == result - 1);

        if (!containsItem || string.IsNullOrEmpty(commandArrWithSplit[1]) || QuestList.ElementAt(result - 1).Status == statusNew || 
            QuestList.ElementAt(result - 1).Status == statusFinish)
        {
            Service.Error();
            continue;
        }
        else
            Service.Complete(QuestList, commandArrWithSplit);
    }
    else if (command.StartsWith("export"))
    {
        if (string.IsNullOrEmpty(commandArrWithSplit[1]))
        {
            Service.Error();
            continue;
        }
        Service.Export(QuestList, commandArrWithSplit, csvDescription, csvContext);
    }
    else if (command.StartsWith("import"))
    {
        if (!File.Exists(commandArrWithSplit[1] + ".csv"))
        {
            Service.Error();
            continue;
        }
        Service.Import(QuestList, commandArrWithSplit, csvDescription, csvContext);
    }
    else if (command == "exit")
    {
        Service.Exit();
    }
    else
        Service.Error();
}

string ExtractCommand(string input)
{
    var splitted = input.Split(' ');
    return splitted[0];
}
