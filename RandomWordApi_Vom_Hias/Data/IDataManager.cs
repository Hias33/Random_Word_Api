using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWordApi_Vom_Hias.Data.Interfaces
{
    public interface IDataManager
    {
        Task<Word> GetWords(string pTable, int pNumber, int minLength, int maxLength);
    }
}
