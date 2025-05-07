//intern:
using RandomWordApi_Vom_Hias.Data.Interfaces;

//extern:
using Microsoft.AspNetCore.Mvc;

namespace RandomWordApi_Vom_Hias.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        public WordController(IDataManager pDataManager)
        {
            _dataManager = pDataManager;
        }

        [HttpGet(Name = "GetWord")]
        public async Task<Word> GetWord(string lang, int number, int min, int max)
        {
            string table = "";
            switch (lang)
            {
                case "de":{
                        table = "german_db";
                        break;
                    }
                case "en":
                    {
                        table = "english_db";
                        break;
                    }
                case "it":
                    {
                        table = "italian_db";
                        break;
                    }
            }
            return await _dataManager.GetWords(table, number, min, max);
        }

        private readonly IDataManager _dataManager;
    }
}
