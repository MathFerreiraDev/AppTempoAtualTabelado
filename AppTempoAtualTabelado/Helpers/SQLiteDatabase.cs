using AppTempoAtualTabelado.Model;
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

        public Task<List<DadosEndereco>> Update(DadosEndereco de)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(sql, de.Descricao, de.Quantidade, de.Preco, de.Id);
        }

        public Task<List<DadosEndereco>> SelectAll()
        {

            return _conn.Table<DadosEndereco>().ToListAsync();
        }

        public Task<int> Delete(int id)
        {

            return _conn.Table<DadosEndereco>().DeleteAsync(i => i.Id == id);
        }

       
    }
}
