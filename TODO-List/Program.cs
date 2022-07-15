﻿using TODO_List.Service;

var service = new Service();
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
    var splitted = Service.SplitCommand(command);

    switch (splitted[0])
    {
        case "show":
            service.ShowList();
            break;

        case "add":
            service.AddNewTask(splitted);
            break;

        case "start":
            service.Start(command);
            break;

        case "complete":
            service.Complete( command);
            break;

        case "export":
            if (command.Length == 6)
            {
                service.Error();
            }
            //else
            //{
            //    if (string.IsNullOrWhiteSpace(commandArrWithSplit[1]))
            //    {
            //        service.Error();
            //        continue;
            //    }
            //    service.Export(commandArrWithSplit);
            //}
            break;
            
        case "import":
            if (command.Length == 6)
            {
                service.Error();
            }
            //else
            //{
            //    if (!File.Exists(commandArrWithSplit[1] + ".csv"))
            //    {
            //        service.Error();
            //        continue;
            //    }
            //    service.Import(commandArrWithSplit);
            //}
            break;

        case "exit":
            Service.Exit();
            break;

        default:
            Service.Error();
            break;
    }
}

