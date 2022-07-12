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
        [CsvColumn(Name= "Data utworzenia", FieldIndex =2, OutputFormat ="dd-MM-yyyy")]
        public DateTime DateOfStart { get; set; }
        [CsvColumn(Name = "Data zakonczenia", FieldIndex = 4, OutputFormat = "dd-MM-yyyy")]
        public DateTime? DateOfEnd { get; set; }
        [CsvColumn(Name = "Nazwa", FieldIndex = 1)]
        public string Name { get; set; }
        [CsvColumn(FieldIndex = 0)]
        public int Id { get; set; }
        [CsvColumn(Name = "Status", FieldIndex = 3)]
        public string Status { get; set; }  
    }
}
