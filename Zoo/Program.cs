using System;

namespace ZooOOp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.StartZoo();
        }
    }

    public interface ICloneable
    {
        Animal Clone();
    }

    public abstract class Animal : ICloneable
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

        protected void GenerateRandomGender()
        {
            string[] genders = new string[] { "Самец", "Самка" };
            int genderCode = _random.Next(genders.Length);
            _gender = genders[genderCode];
        }

        protected void GenerateRandomAge(int maxAge) => _age = _random.Next(0, maxAge);

        public abstract Animal Clone();
    }

    public class Lion : Animal
    {
        public Lion() : base("Лев", "Рычит", 30)
        {
        }

        public override Animal Clone()
        {
            return new Lion();
        }
    }

    public class Tiger : Animal
    {
        public Tiger() : base("Тигр", "Мурлыкает", 20)
        {
        }

        public override Animal Clone()
        {
            return new Tiger();
        }
    }

    public class Bear : Animal
    {
        public Bear() : base("Медведь", "Ревет", 50)
        {
        }

        public override Animal Clone()
        {
            return new Bear();
        }
    }

    public class Monkey : Animal
    {
        public Monkey() : base("Обезьяна", "Кричит", 25)
        {
        }

        public override Animal Clone()
        {
            return new Monkey();
        }
    }

    public class Aviary
    {
        private List<ICloneable> _animals = new List<ICloneable>();

        public void AddAnimal(ICloneable animal)
        {
            _animals.Add(animal);
        }

        public void ShowAnimals()
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                int animalNumber = i + 1;

                Console.Write($"{animalNumber}) ");

                Animal animal = _animals[i].Clone() as Animal;

                animal.Show();
            }
        }
    }

    public class Zoo
    {
        private Dictionary<string, Aviary> _aviaries = new Dictionary<string, Aviary>();
        private Dictionary<string, Func<Animal>> animalTypes = new Dictionary<string, Func<Animal>>
    {
        { "Лев", () => new Lion() },
        { "Тигр", () => new Tiger() },
        { "Медведь", () => new Bear() },
        { "Обезьяна", () => new Monkey() }
    };

        private Random _random = new Random();

        public void FillAviaries()
        {
            int maxCapacityAviary = 5;

            foreach (string type in GetAnimalTypes())
            {
                Aviary aviary = new Aviary();

                for (int i = 0; i < _random.Next(1, maxCapacityAviary + 1); i++)
                {
                    Animal animal = CreateAnimalInstance(type);

                    aviary.AddAnimal(animal);
                }

                _aviaries.Add(type, aviary);
            }
        }

        public void ShowAviary(string animalType)
        {
            if (_aviaries.ContainsKey(animalType))
            {
                Console.Clear();
                Console.WriteLine($"Вы находитесь у вольера с {animalType}ами:");

                _aviaries[animalType].ShowAnimals();

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Такого вольера не существует.");
            }
        }

        private List<string> GetAnimalTypes()
        {
            return new List<string> { "Лев", "Тигр", "Медведь", "Обезьяна" };
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
        private enum Command
        {
            ShowAviaryLion = 1,
            ShowAviaryTiger,
            ShowAviaryBear,
            ShowAviaryMonkey,
            ExitProgram
        }

        private bool _isWorking = true;

        private Zoo _zoo = new Zoo();

        public void StartZoo()
        {
            _zoo.FillAviaries();

            while (_isWorking)
            {
                Console.Clear();
                Console.WriteLine($"{(int)Command.ShowAviaryLion} - Подойти к вольеру со Львами\n" +
                    $"{(int)Command.ShowAviaryTiger} - Подойти к вольеру с Тиграми\n" +
                    $"{(int)Command.ShowAviaryBear} - Подойти к вольеру с Медведями\n" +
                    $"{(int)Command.ShowAviaryMonkey} - Подойти к вольеру с Обезьянами\n" +
                    $"{(int)Command.ExitProgram} - Покинуть зоопарк");

                string userInput = Console.ReadLine();

                if (Enum.TryParse(userInput, out Command command))
                {
                    HandleCommand(command);
                }
                else
                {
                    Console.WriteLine("Некорректный ввод");
                }
            }
        }

        private void HandleCommand(Command command)
        {
            switch (command)
            {
                case Command.ShowAviaryLion:
                    _zoo.ShowAviary("Лев");
                    break;
                case Command.ShowAviaryTiger:
                    _zoo.ShowAviary("Тигр");
                    break;
                case Command.ShowAviaryBear:
                    _zoo.ShowAviary("Медведь");
                    break;
                case Command.ShowAviaryMonkey:
                    _zoo.ShowAviary("Обезьяна");
                    break;
                case Command.ExitProgram:
                    _isWorking = false;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод");
                    break;
            }
        }
    }
}