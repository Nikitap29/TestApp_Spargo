namespace TestApp_Spargo
{
    public class Pharmacy
    {
        /// <summary>
        /// Выборка аптек
        /// </summary>
        /// <param name="sql">Sql запрос</param>
        /// <returns>Статус и список с данными по аптекам</returns>
        public static (int, List<string[]>) Select(string sql = "")
        {
            if (sql == "") sql = "SELECT * FROM pharmacy;";
            var data = DBConnect.Request(sql, 4, out int res);
            return (res, data);
        }

        /// <summary>
        /// Добавить аптеку
        /// </summary>
        /// <param name="name">Название аптеки</param>
        /// <param name="address">Адрес аптеки</param>
        /// <param name="phone">Телефон аптеки</param>
        /// <returns>Статус выполнения</returns>
        public static int Add(string name, string address, string phone)
        {
            string sql = "INSERT INTO pharmacy (name, address, phone) VALUES ('" + name + "', '" + address + "', '" + phone + "');";
            DBConnect.Request(sql, 0, out int res);
            return res;
        }

        /// <summary>
        /// Удалить аптеку
        /// </summary>
        /// <param name="id">Идентификатор аптеки</param>
        /// <returns>Статус выполнения</returns>
        public static int Delete(int id)
        {
            var res = Check(id);
            if (res == 0)
            {
                List<string[]> storage;
                (res, storage) = Storage.Select("SELECT id FROM storage WHERE pharmacy = '" + id + "';");
                if (res == 0)
                {
                    foreach (string[] row in storage)
                    {
                        res = Storage.Delete(int.Parse(row[0]));
                        if (res != 0) return res;
                    }
                    string sql = "DELETE FROM pharmacy WHERE (id = '" + id.ToString() + "');";
                    DBConnect.Request(sql, 0, out res);
                }
            }
            return res;
        }

        /// <summary>
        /// Проверка наличия аптеки в базе
        /// </summary>
        /// <param name="id">Идентификатор аптеки</param>
        /// <returns>Статус выполнения</returns>
        public static int Check(int id)
        {
            var (res, data) = Select("SELECT * FROM pharmacy WHERE id = " + id.ToString());
            if (res == 0)
            {
                if (data.Count == 0) return -2;
                else return 0;
            }
            return res;
        }
    }
}