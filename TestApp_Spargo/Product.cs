namespace TestApp_Spargo
{
    public class Product
    {
        /// <summary>
        /// Выборка товаров
        /// </summary>
        /// <param name="sql">Sql запрос</param>
        /// <returns>Статус и список с данными по товарам</returns>
        public static (int, List<string[]> ) Select(string sql = "")
        {
            if (sql == "") sql = "SELECT * FROM product;";
            var data = DBConnect.Request(sql, 2, out int res);
            return (res, data);
        }

        /// <summary>
        /// Добавить товар
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <returns>Статус выполнения</returns>
        public static int Add(string name)
        {
            string sql = "INSERT INTO product (name) VALUES ('" + name + "');";
            DBConnect.Request(sql, 0, out int res);
            return res;
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Статус выполнения</returns>
        public static int Delete(int id)
        {
            var res = Check(id);
            if (res == 0)
            {
                List<string[]> batch;
                (res, batch) = Batch.Select("SELECT id FROM batch WHERE product = '" + id + "';");
                if (res == 0)
                {
                    foreach (string[] row in batch)
                    {
                        res = Batch.Delete(int.Parse(row[0]));
                        if (res != 0) return res;
                    }
                    string sql = "DELETE FROM product WHERE(id = '" + id.ToString() + "');";
                    DBConnect.Request(sql, 0, out res);
                }
            }
            return res;
        }

        /// <summary>
        /// Проверка наличия товара в базе
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Статус выполнения</returns>
        public static int Check(int id)
        {
            var (res, data) = Select("SELECT * FROM product WHERE id = " + id.ToString());
            if (res == 0)
            {
                if (data.Count == 0) return -2;
                else return 0;
            }
            return res;
        }
    }
}