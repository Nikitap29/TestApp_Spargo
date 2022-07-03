using MySql.Data.MySqlClient;

namespace TestApp_Spargo
{
    internal class DBConnect
    {
        /// <summary>
        /// Выполнение запроса
        /// </summary>
        public static List<string[]> Request(string sql, int cols, out int status)
        {
            // строка подключения к БД
            var connStr = "server=localhost;user=root;database=spargo;password=4321;";
            var res = new List<string[]>();
            // создаём объект для подключения к БД
            var conn = new MySqlConnection(connStr);
            // устанавливаем соединение с БД
            conn.Open();
            // объект для выполнения SQL-запроса
            var command = new MySqlCommand(sql, conn);
            try
            {
                // объект для чтения ответа сервера
                var reader = command.ExecuteReader();
                // читаем результат
                while (reader.Read())
                {
                    var row = new string[cols];
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[i] = reader[i].ToString();
                    res.Add(row);
                }
                reader.Close(); // закрываем reader
                                // закрываем соединение с БД
                conn.Close();
                status = 0;
                return res;
            }
            catch
            {
                status = -1;
                return res;
            }
        }
    }
}
