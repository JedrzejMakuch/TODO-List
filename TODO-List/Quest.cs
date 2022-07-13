using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_List
{
    public class Quest
    {
        private const string DATE_FORMAT = "dd-MM-yyyy";

        [CsvColumn(Name= "Data utworzenia", FieldIndex = 2, OutputFormat = DATE_FORMAT)]
        public DateTime DateOfStart { get; set; }

        [CsvColumn(Name = "Data zakonczenia", FieldIndex = 4, OutputFormat = DATE_FORMAT)]
        public DateTime? DateOfEnd { get; set; }

        [CsvColumn(Name = "Nazwa", FieldIndex = 1)]
        public string Name { get; set; }

        [CsvColumn(FieldIndex = 0)]
        public int Id { get; set; }

        [CsvColumn(Name = "Status", FieldIndex = 3)]
        public string Status { get; set; }
    }
}
