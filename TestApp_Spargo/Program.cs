using System.Text;
using TestApp_Spargo;

int cmd; //команда меню
while (true)
{
    Console.Clear();
    //вывод меню
    Console.WriteLine("Выберете действие:");
    Console.WriteLine();
    Console.WriteLine("11 - Создать товар");
    Console.WriteLine("12 - Удалить товар");
    Console.WriteLine();
    Console.WriteLine("21 - Создать аптеку");
    Console.WriteLine("22 - Удалить аптеку");
    Console.WriteLine();
    Console.WriteLine("31 - Создать склад");
    Console.WriteLine("32 - Удалить склад");
    Console.WriteLine();
    Console.WriteLine("41 - Создать партию");
    Console.WriteLine("42 - Удалить партию");
    Console.WriteLine();
    Console.WriteLine("5 - Вывести на экран весь список товаров и его количество в выбранной аптеке");
    Console.WriteLine();
    Console.WriteLine("6 - Выход из программы");
    Console.WriteLine();
    cmd = InputNum("");
    int res = 0;
    List<string[]> rows;
    if (cmd == 6) break; 
    switch (cmd)
    {
        case 11:
            res = Product.Add(InputString("Введите наименование товара:"));
            if (res == 0) Console.WriteLine("Товар успешно добавлен!");
            break;
        case 12:
            (res, rows) = Product.Select();
            if (res == 0)
            {
                Console.WriteLine("Список товаров:");
                OutputRows(rows);
                res = Product.Delete(InputNum("Введите id товара, который необходимо удалить:"));
                if (res == 0) Console.WriteLine("Товар успешно удалён!");
            }
            break;
        case 21:
            res = Pharmacy.Add(InputString("Введите наименование аптеки:"),
                InputString("Введите адрес аптеки:"),
                InputString("Введите телефон аптеки:"));
            if (res == 0) Console.WriteLine("Аптека успешно добавлена!");
            break;
        case 22:
            (res, rows) = Pharmacy.Select();
            if (res == 0)
            {
                Console.WriteLine("Список аптек:");
                OutputRows(rows);
                res = Pharmacy.Delete(InputNum("Введите id аптеки, который необходимо удалить:"));
                if (res == 0) Console.WriteLine("Аптека успешно удалена!");
            }
            break;
        case 31:
            (res, rows) = Pharmacy.Select();
            if (res == 0)
            {
                Console.WriteLine("Список аптек:");
                OutputRows(rows);
                res = Storage.Add(InputString("Введите наименование склада:"),
                InputNum("Введите id аптеки:"));
                if (res == 0) Console.WriteLine("Склад успешно добавлен!");
            }
            break;
        case 32:
            (res, rows) = Storage.Select();
            if (res == 0)
            {
                Console.WriteLine("Список складов:");
                OutputRows(rows);
                res = Storage.Delete(InputNum("Введите id склада, который необходимо удалить:"));
                if (res == 0) Console.WriteLine("Склад успешно удален!");
            }
            break;
        case 41:
            (res, rows) = Storage.Select();
            if (res == 0)
            {
                Console.WriteLine("Список складов:");
                OutputRows(rows);
                (res, rows) = Product.Select();
                if (res == 0)
                {
                    Console.WriteLine("Список товаров:");
                    OutputRows(rows);
                    res = Batch.Add(InputNum("Введите id склада:"),
                    InputNum("Введите id товара:"),
                    InputNum("Введите количество товаров в партии:"));
                    if (res == 0) Console.WriteLine("Партия успешно добавлена!");
                }
            }
            break;
        case 42: 
            (res, rows) = Batch.Select();
            if (res == 0)
            {
                Console.WriteLine("Список партий:");
                OutputRows(rows);
                res = Batch.Delete(InputNum("Введите id партии, которую необходимо удалить:"));
                if (res == 0) Console.WriteLine("Партия успешно удалена!");
            }
            break;
        case 5:
            (res, rows) = Product.Select();
            if (res == 0)
            {
                Console.WriteLine("Список товаров:");
                OutputRows(rows);
                int id = InputNum("Введите id товара, наличие которого необходимо проверить:");
                string sql = "SELECT ph.id, ph.name, SUM(b.count) " +
                    "FROM pharmacy AS ph, product AS p, batch AS b, storage AS s " +
                    "WHERE ph.id = s.pharmacy AND s.id = b.storage AND p.id = b.product AND p.id = " + id.ToString() +
                    " GROUP BY 1, 2 ";
                (res, rows) = Storage.Select(sql);
                if (res == 0) OutputRows(rows);
            }
            break;
        default:
            Console.WriteLine("Неизвестная команда");
            break;
    }
    switch (res)
    {
        case -1:
            Console.WriteLine("Ошибка доступа к БД!");
            break;
        case -2:
            Console.WriteLine("Объект для удаления не найден в БД!");
            break;
    }
    Console.WriteLine("Нажмите Enter");
    Console.ReadLine();
}

//вывод строк на экран
void OutputRows(List<string[]> rows)
{
    var res = new StringBuilder();
    foreach (string[] row in rows)
    {
        foreach (string s in row)
        {
            res.Append($"{s}\t");
        }
        res.AppendLine();
    }
    Console.WriteLine(res);
}

//ввод строки
string InputString(string ques)
{
    Console.WriteLine(ques);
    return Console.ReadLine();
}

//ввод числа
int InputNum(string ques)
{
    int res = 0;
    Console.WriteLine(ques);
    bool f = false; //проверка на корректный ввод
    while (!f)
    {
        f = int.TryParse(Console.ReadLine(), out res);
        if (!f) Console.WriteLine("Некорректный ввод!");
    }
    return res;
}