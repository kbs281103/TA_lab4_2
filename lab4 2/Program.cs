using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class PhoneDirectory
{
    private string surname;
    private string name;
    private string patronymic;
    private string address;
    private string phone;

    public string Surname
    {
        get { return surname; }
        set { surname = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Patronymic
    {
        get { return patronymic; }
        set { patronymic = value; }
    }

    public string Address
    {
        get { return address; }
        set { address = value; }
    }

    public string Phone
    {
        get { return phone; }
        set { phone = value; }
    }

    public PhoneDirectory() { }

    public PhoneDirectory(string surname, string name, string patronymic, string address, string phone)
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Address = address;
        Phone = phone;
    }

    public override string ToString()
    {
        return $"{Surname} {Name} {Patronymic}, Адреса: {Address}, Телефон: {Phone}";
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        List<PhoneDirectory> phoneDirectory = new List<PhoneDirectory>();
        string filePath = "phone_directory.txt";

        LoadData(filePath, phoneDirectory);

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Додати запис");
            Console.WriteLine("2. Редагувати запис");
            Console.WriteLine("3. Видалити запис");
            Console.WriteLine("4. Вивести всі записи");
            Console.WriteLine("5. Пошук запису за прізвищем");
            Console.WriteLine("6. Сортування за телефоном");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть опцію: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddRecord(phoneDirectory);
                    break;
                case "2":
                    EditRecord(phoneDirectory);
                    break;
                case "3":
                    DeleteRecord(phoneDirectory);
                    break;
                case "4":
                    DisplayAllRecords(phoneDirectory);
                    break;
                case "5":
                    SearchBySurname(phoneDirectory);
                    break;
                case "6":
                    SortByPhone(phoneDirectory);
                    break;
                case "0":
                    SaveData(filePath, phoneDirectory);
                    Console.WriteLine("Вихід з програми.");
                    return;
                default:
                    Console.WriteLine("Некоректна опція. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void AddRecord(List<PhoneDirectory> phoneDirectory)
    {
        Console.WriteLine("\nДодавання запису:");

        Console.Write("Прізвище: ");
        string surname = Console.ReadLine();

        Console.Write("Ім'я: ");
        string name = Console.ReadLine();

        Console.Write("По батькові: ");
        string patronymic = Console.ReadLine();

        Console.Write("Адреса: ");
        string address = Console.ReadLine();

        Console.Write("Телефон: ");
        string phone = Console.ReadLine();

        // Перевірка на коректність телефону
        if (!IsPhoneValid(phone))
        {
            Console.WriteLine("Некоректний телефон. Запис не додано.");
            return;
        }

        PhoneDirectory newRecord = new PhoneDirectory(surname, name, patronymic, address, phone);
        phoneDirectory.Add(newRecord);
        Console.WriteLine("Запис успішно додано.");
    }

    // Метод для перевірки коректності телефону
    static bool IsPhoneValid(string phone)
    {
        return phone.All(char.IsDigit);
    }

    static void EditRecord(List<PhoneDirectory> phoneDirectory)
    {
        Console.WriteLine("\nРедагування запису:");
        Console.Write("Введіть прізвище для пошуку запису: ");
        string surname = Console.ReadLine();

        var recordsToEdit = phoneDirectory.Where(r => r.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)).ToList();

        if (recordsToEdit.Count == 0)
        {
            Console.WriteLine("Записів з таким прізвищем не знайдено.");
            return;
        }

        Console.WriteLine("\nЗнайдені записи:");
        for (int i = 0; i < recordsToEdit.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {recordsToEdit[i]}");
        }

        Console.Write("\nОберіть номер запису для редагування: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > recordsToEdit.Count)
        {
            Console.WriteLine("Некоректний вибір номера запису.");
            return;
        }

        PhoneDirectory recordToEdit = recordsToEdit[index - 1];

        Console.Write($"Прізвище (поточне: {recordToEdit.Surname}): ");
        string newSurname = Console.ReadLine();
        if (!string.IsNullOrEmpty(newSurname))
        {
            recordToEdit.Surname = newSurname;
        }

        Console.Write($"Ім'я (поточне: {recordToEdit.Name}): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            recordToEdit.Name = newName;
        }

        Console.Write($"По батькові (поточне: {recordToEdit.Patronymic}): ");
        string newPatronymic = Console.ReadLine();
        if (!string.IsNullOrEmpty(newPatronymic))
        {
            recordToEdit.Patronymic = newPatronymic;
        }

        Console.Write($"Адреса (поточна: {recordToEdit.Address}): ");
        string newAddress = Console.ReadLine();
        if (!string.IsNullOrEmpty(newAddress))
        {
            recordToEdit.Address = newAddress;
        }

        Console.Write($"Телефон (поточний: {recordToEdit.Phone}): ");
        string newPhone = Console.ReadLine();
        if (!string.IsNullOrEmpty(newPhone))
        {
            if (IsPhoneValid(newPhone))
            {
                recordToEdit.Phone = newPhone;
                Console.WriteLine("Запис успішно відредаговано.");
            }
            else
            {
                Console.WriteLine("Некоректний телефон. Запис не відредаговано.");
            }
        }
    }

    static void DeleteRecord(List<PhoneDirectory> phoneDirectory)
    {
        Console.WriteLine("\nВидалення запису:");
        Console.Write("Введіть прізвище для пошуку запису: ");
        string surname = Console.ReadLine();

        var recordsToDelete = phoneDirectory.Where(r => r.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)).ToList();

        if (recordsToDelete.Count == 0)
        {
            Console.WriteLine("Записів з таким прізвищем не знайдено.");
            return;
        }

        Console.WriteLine("\nЗнайдені записи:");
        for (int i = 0; i < recordsToDelete.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {recordsToDelete[i]}");
        }

        Console.Write("\nОберіть номер запису для видалення: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > recordsToDelete.Count)
        {
            Console.WriteLine("Некоректний вибір номера запису.");
            return;
        }

        phoneDirectory.Remove(recordsToDelete[index - 1]);
        Console.WriteLine("Запис успішно видалено.");
    }

    static void DisplayAllRecords(List<PhoneDirectory> phoneDirectory)
    {
        Console.WriteLine("\nВсі записи:");

        if (phoneDirectory.Count == 0)
        {
            Console.WriteLine("Записи відсутні.");
            return;
        }

        foreach (var record in phoneDirectory)
        {
            Console.WriteLine(record);
        }
    }

    static void SearchBySurname(List<PhoneDirectory> phoneDirectory)
    {
        Console.WriteLine("\nПошук запису за прізвищем:");
        Console.Write("Введіть прізвище: ");
        string surname = Console.ReadLine();

        var foundRecords = phoneDirectory.Where(r => r.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundRecords.Count == 0)
        {
            Console.WriteLine("Записів з таким прізвищем не знайдено.");
        }
        else
        {
            Console.WriteLine("\nЗнайдені записи:");
            foreach (var record in foundRecords)
            {
                Console.WriteLine(record);
            }
        }
    }

    static void SortByPhone(List<PhoneDirectory> phoneDirectory)
    {
        phoneDirectory.Sort((x, y) => x.Phone.CompareTo(y.Phone));

        Console.WriteLine("\nЗаписи відсортовані за телефоном:");
        foreach (var record in phoneDirectory)
        {
            Console.WriteLine(record);
        }
    }

    static void LoadData(string filePath, List<PhoneDirectory> phoneDirectory)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не знайдено, створення нового.");
            return;
        }

        try
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 5)
                    {
                        PhoneDirectory record = new PhoneDirectory(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), parts[3].Trim(), parts[4].Trim());
                        phoneDirectory.Add(record);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка завантаження даних: {ex.Message}");
        }
    }

    static void SaveData(string filePath, List<PhoneDirectory> phoneDirectory)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var record in phoneDirectory)
                {
                    sw.WriteLine($"{record.Surname}, {record.Name}, {record.Patronymic}, {record.Address}, {record.Phone}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка збереження даних: {ex.Message}");
        }
    }
}
