namespace TestApp_Spargo
{
    public class Batch
    {
        /// <summary>
        /// Выборка партий
        /// </summary>
        /// <param name="sql">Sql запрос</param>
        /// <returns>Статус и список с данными по партиям</returns>
        public static (int, List<string[]>) Select(string sql = "")
        {
            if (sql == "") sql = "SELECT * FROM batch;";
            var data = DBConnect.Request(sql, 4, out int res);
            return (res, data);
        }

        /// <summary>
        /// Добавить партию
        /// </summary>
        /// <param name="storage">Идентификатор склада</param>
        /// <param name="product">Идентификатор продукта</param>
        /// <param name="count">Количество продуктов</param>
        /// <returns>Статус выполнения команды</returns>
        public static int Add(int storage, int product, int count)
        {
            string sql = "INSERT INTO batch (storage, product, count) VALUES (" + storage.ToString() + ", " + product.ToString() + ", " + count.ToString() + ");";
            DBConnect.Request(sql, 0, out int res);
            return res;
        }

        /// <summary>
        /// Удалить партию
        /// </summary>
        /// <param name="id">Идентификатор партии</param>
        /// <returns>Статус выполнения команды</returns>
        public static int Delete(int id)
        {
            var res = Check(id);
            if (res == 0)
            {
                string sql = "DELETE FROM batch WHERE (id = '" + id.ToString() + "');";
                DBConnect.Request(sql, 0, out res);
            }
            return res;
        }

        /// <summary>
        /// Проверка наличия партии в базе
        /// </summary>
        /// <param name="id">Идентификатор партии</param>
        /// <returns>Статус выполнения</returns>
        public static int Check(int id)
        {
            var (res, data) = Select("SELECT * FROM batch WHERE id = " + id.ToString());
            if (res == 0)
            {
                if (data.Count == 0) return -2;
                else return 0;
            }
            return res;
        }
    }
}