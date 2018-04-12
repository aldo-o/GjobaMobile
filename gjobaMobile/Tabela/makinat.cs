using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace gjobaMobile.Tabela
{
    public class makinat
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string sqMakina { get; set; }
        public string sqTarga { get; set; }
        public string sqShasia { get; set; }
        public makinat()
        {

        }
        public makinat(string makina, string targa, string shasi)
        {
            sqMakina = makina;
            sqTarga = targa;
            sqShasia = shasi;
        }
    }
}
