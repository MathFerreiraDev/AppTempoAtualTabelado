using AppTempoAtualTabelado.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTempoAtualTabelado.Helpers
{
    public class SQLiteDatabase
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabase(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<CondicoesClima>().Wait();
        }

        public Task<int> Insert(CondicoesClima de)
        {
            return _conn.InsertAsync(de);
        }

      

        public Task<List<CondicoesClima>> SelectAll()
        {

            return _conn.Table<CondicoesClima>().ToListAsync();
        }

      

       
    }
}
