using AppTempoAtualTabelado.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTempoAtualTabelado.Helpers
{
    class SQLiteDatabase
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabase(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<DadosEndereco>().Wait();
        }

        public Task<int> Insert(DadosEndereco de)
        {
            return _conn.InsertAsync(de);
        }

      

        public Task<List<DadosEndereco>> SelectAll()
        {

            return _conn.Table<DadosEndereco>().ToListAsync();
        }

      

       
    }
}
