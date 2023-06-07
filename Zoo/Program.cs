using System;

public class Program
{
    static void Main(string[] args)
    {
        Menu menu = new Menu();
        menu.StartZoo();
    }
}

public abstract class Animal
{
    private string _gender;
    private int _age;
    private Random _random = new Random();

    public Animal(string type, string voice, int maxAge)
    {
        Type = type;
        Voice = voice;
        MaxAge = maxAge;
        GenerateRandomGender();
        GenerateRandomAge(maxAge);
    }

    public string Type { get; protected set; }
    public string Voice { get; protected set; }
    public int MaxAge { get; protected set; }

    public void Show() => Console.WriteLine($"Животное - {Type} : Гендер животного - {_gender} : Возраст животного - {_age} лет : Животное издает звук - {Voice}");

    public abstract Animal Clone();

    protected void GenerateRandomGender()
    {
        string[] genders = new string[] { "Самец", "Самка" };
        int genderCode = _random.Next(genders.Length);
        _gender = genders[genderCode];
    }

    protected void GenerateRandomAge(int maxAge) => _age = _random.Next(0, maxAge);
}

public class Lion : Animal
{
    public Lion() : base("Лев", "Рычит", 30)
    {
    }

    public override Animal Clone()
    {
        return (Animal)MemberwiseClone();
    }
}

public class Tiger : Animal
{
    public Tiger() : base("Тигр", "Мурлыкает", 20)
    {
    }

    public override Animal Clone()
    {
        return (Animal)MemberwiseClone();
    }
}

public class Bear : Animal
{
    public Bear() : base("Медведь", "Ревет", 50)
    {
    }

    public override Animal Clone()
    {
        return (Animal)MemberwiseClone();
    }
}

public class Monkey : Animal
{
    public Monkey() : base("Обезьяна", "Кричит", 25)
    {
    }

    public override Animal Clone()
    {
        return (Animal)MemberwiseClone();
    }
}

public class Aviary
{
    private List<Animal> _animals = new List<Animal>();

    public void AddAnimal(Animal animal)
    {
        _animals.Add(animal);
    }

    public void ShowAnimals()
    {
        for (int i = 0; i < _animals.Count; i++)
        {
            int animalNumber = i + 1;

            Console.Write($"{animalNumber}) ");

            Animal animal = _animals[i];

            animal.Show();
        }
    }
}

public class Zoo
{
    private List<Aviary> _aviaries = new List<Aviary>();
    private Dictionary<string, Func<Animal>> animalTypes = new Dictionary<string, Func<Animal>>();
    private Random _random = new Random();

    public Zoo()
    {
        animalTypes.Add("Лев", () => new Lion());
        animalTypes.Add("Тигр", () => new Tiger());
        animalTypes.Add("Медведь", () => new Bear());
        animalTypes.Add("Обезьяна", () => new Monkey());
    }

    public void FillAviaries()
    {
        int maxCapacityAviary = 5;
        int maxAnimal = _random.Next(1, maxCapacityAviary + 1);

        foreach (string type in GetAnimalTypes())
        {
            Aviary aviary = new Aviary();

            for (int i = 0; i < maxAnimal; i++)
            {
                Animal animal = CreateAnimalInstance(type);

                aviary.AddAnimal(animal);
            }

            _aviaries.Add(aviary);
        }
    }

    public void ShowAviary(int index)
    {
        if (index >= 0 && index < _aviaries.Count)
        {
            Console.Clear();
            Console.WriteLine($"Вы находитесь у вольера номер {index + 1}:");

            _aviaries[index].ShowAnimals();

            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Такого вольера не существует.");
        }
    }

    public int GetAviaryCount()
    {
        return _aviaries.Count;
    }

    private List<string> GetAnimalTypes()
    {
        List<string> types = new List<string>();

        foreach (string animalType in animalTypes.Keys)
        {
            types.Add(animalType);
        }

        return types;
    }

    private Animal CreateAnimalInstance(string animalType)
    {
        if (animalTypes.ContainsKey(animalType))
        {
            return animalTypes[animalType].Invoke();
        }
        else
            throw new ArgumentException("Недопустимый тип животного.");
    }
}

public class Menu
{
    private bool _isWorking = true;
    private Zoo _zoo = new Zoo();

    public void StartZoo()
    {
        _zoo.FillAviaries();

        while (_isWorking)
        {
            Console.Clear();
            Console.WriteLine($"Текущее количество вольеров: {_zoo.GetAviaryCount()}\n"+
               $"{(int)Command.ShowAviary} - Подойти к вольеру\n" +
               $"{(int)Command.ExitProgram} - Покинуть зоопарк");

            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int command))
            {
                HandleCommand(command);
            }
            else
            {
                Console.WriteLine("Некорректный ввод");
            }
        }
    }

    private void HandleCommand(int command)
    {
        if (command == (int)Command.ShowAviary)
        {
            Console.WriteLine("Введите номер вольера:");

            if (int.TryParse(Console.ReadLine(), out int aviaryIndex))
            {
                _zoo.ShowAviary(aviaryIndex - 1);
            }
            else
            {
                Console.WriteLine("Некорректный номер вольера");
            }
        }
        else if (command == (int)Command.ExitProgram)
        {
            _isWorking = false;
        }
        else
        {
            Console.WriteLine("Некорректный ввод");
        }
    }

    private enum Command
    {
        ShowAviary = 1,
        ExitProgram
    }
}
