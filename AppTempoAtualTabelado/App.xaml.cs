using AppTempoAtualTabelado.Helpers;

namespace AppTempoAtualTabelado
{
    public partial class App : Application
    {

        public static SQLiteDatabase _db;

        public static SQLiteDatabase Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_locais.db3");

                    _db = new SQLiteDatabase(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
