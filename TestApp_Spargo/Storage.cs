namespace TestApp_Spargo
{
    public class Storage
    {
        /// <summary>
        /// Выборка складов
        /// </summary>
        /// <param name="sql">Sql запрос</param>
        /// <returns>Статус и список с данными по складам</returns>
        public static (int, List<string[]>) Select(string sql = "")
        {
            if (sql == "") sql = "SELECT * FROM storage;";
            var data = DBConnect.Request(sql, 3, out int res);
            return (res, data);
        }

        /// <summary>
        /// Добавить склад
        /// </summary>
        /// <param name="name">Наименование склада</param>
        /// <param name="pharmacy">Идентификатор аптеки</param>
        /// <returns>Статус выполнения</returns>
        public static int Add(string name, int pharmacy)
        {
            string sql = "INSERT INTO storage (name, pharmacy) VALUES ('" + name + "', '" + pharmacy.ToString() + "');";
            DBConnect.Request(sql, 0, out int res);
            return res;
        }

        /// <summary>
        /// Удалить склад
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        /// <returns>Статус выполнения</returns>
        public static int Delete(int id)
        {
            var res = Check(id);
            if (res == 0)
            {
                List<string[]> batch;
                (res, batch) = Batch.Select("SELECT id FROM batch WHERE storage = '" + id + "';");
                if (res == 0)
                {
                    foreach (string[] row in batch)
                    {
                        res = Batch.Delete(int.Parse(row[0]));
                        if (res != 0) return res;
                    }
                    string sql = "DELETE FROM storage WHERE (id = '" + id.ToString() + "');";
                    DBConnect.Request(sql, 0, out res);
                }
            }
            return res;
        }

        /// <summary>
        /// Проверка наличия склада в базе
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        /// <returns>Статус выполнения</returns>
        public static int Check(int id)
        {
            var (res, data) = Select("SELECT * FROM storage WHERE id = " + id.ToString());
            if (res == 0)
            {
                if (data.Count == 0) return -2;
                else return 0;
            }
            return res;
        }
    }
}