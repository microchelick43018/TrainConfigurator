using System;
using System.Collections.Generic;

namespace TrainConfigurator
{
    class Program
    {
        static void ShowMenu()
        {
            Console.WriteLine("1 - Создать направление.");
            Console.WriteLine("2 - Продать билеты.");
            Console.WriteLine("3 - Сформировать поезд.");
            Console.WriteLine("4 - Отправить поезд.");
            Console.WriteLine("5 - Выход из программы.");
        }

        static void TapToCountinue()
        {
            Console.WriteLine("Нажмите на любую клавишу, чтобы продолжить...");
            Console.ReadKey();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            bool exit = false;
            int choice, choiceCounter = 1;
            Trip trip = new Trip();
            while (exit == false)
            {
                if (trip.Direction != "")
                {
                    trip.ShowInfo();
                }
                else
                {
                    Console.WriteLine("Поезд не создан");
                }
                ShowMenu();
                Console.WriteLine();
                choice = InputChecker.MakeChoice(5);
                if (choiceCounter != choice && choice != 5)
                {
                    Console.WriteLine($"Следующий пункт, который необходимо сделать {choiceCounter}.");
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            trip.EnterDirection();
                            break;
                        case 2:
                            trip.SellTickets();
                            break;
                        case 3:
                            trip.EnterTrainInfo();
                            trip.Train.SeatPassengers(trip.PassengersCount);
                            break;
                        case 4:
                            trip.SendTrain();
                            choiceCounter = 1;
                            break;
                        case 5:
                            exit = true;
                            break;
                        default:
                            break;
                    }
                    choiceCounter++;
                }   
                TapToCountinue(); 
            }
        }
    }

    class InputChecker
    {
        public static int ReadInt()
        {
            bool isCorrected = int.TryParse(Console.ReadLine(), out int choice);
            while (isCorrected == false)
            {
                Console.WriteLine("Неверный ввод. Повторите попытку: ");
                isCorrected = int.TryParse(Console.ReadLine(), out choice);
            }
            return choice;
        }

        public static int MakeChoice(int maxNumber = 5)
        {
            int choice = ReadInt();
            while (choice > maxNumber || choice < 1)
            {
                Console.Write("Неверный ввод. Повторите попытку: ");
                choice = ReadInt();
            }
            return choice;
        }

        public static int MakeChoice(int minNumber = 1, int maxNumber = 5)
        {
            int choice = ReadInt();
            while (choice > maxNumber || choice < minNumber)
            {
                Console.Write("Неверный ввод. Повторите попытку: ");
                choice = ReadInt();
            }
            return choice;
        }
    }

    class Trip
    {
        private Train _train;
        private string _direction;
        private int _passengersCount;

        public string Direction { get => _direction;  }
        public int PassengersCount { get => _passengersCount; }
        public Train Train { get => _train; }

        public Trip()
        {
            _direction = "";
            _passengersCount = 0;
            _train = new Train();
        }

        public void Clear()
        {
            Train.Clear();
            _direction = "";
            _passengersCount = 0;
        }

        public void EnterDirection()
        {
            Console.WriteLine("Введите точку отправления: ");
            string cityFrom = Console.ReadLine();
            Console.WriteLine("Введите точку прибытия: ");
            string cityWhere = Console.ReadLine();
            SetDirection(cityFrom, cityWhere);
        }

        public void EnterTrainInfo()
        {
            Console.WriteLine($"Введите количество вагонов (минимум {Train.MinCarriagesAdd}, максимум {Train.MaxCarriagesAdd})");
            int carriagesCount = InputChecker.MakeChoice(Train.MinCarriagesAdd, Train.MaxCarriagesAdd);
            Train.AddCarriages(carriagesCount);
            int missingSeatsCount = PassengersCount - Train.GetAmountCapacity();
            while (missingSeatsCount > 0)
            {
                Console.WriteLine($"Не хватает {missingSeatsCount} мест. Необходимо добавить ещё.");
                Console.WriteLine($"Введите количество вагонов (минимум 1, максимум {Train.MaxCarriagesAdd})");
                carriagesCount = InputChecker.MakeChoice(1, Train.MaxCarriagesAdd);
                Train.AddCarriages(carriagesCount);
                missingSeatsCount = PassengersCount - Train.GetAmountCapacity();
            }
        }

        public void SendTrain()
        {
            Console.WriteLine("Поезд успешно отправлен!");
            Clear();
        }

        public void SellTickets()
        {
            Random rand = new Random();
            _passengersCount = rand.Next(50, 150);
            Console.WriteLine($"Билеты успешно проданы. Количество пассажиров = {_passengersCount}");
        }

        public void SetDirection(string cityFrom, string cityWhere)
        {
            _direction = cityFrom + " - " + cityWhere;
        }

        public void ShowInfo()
        {
            if (Direction != "")
            {
                Console.WriteLine($"Напрвление: {Direction}");
            }
            else
            {
                Console.WriteLine("Направление не создано.");
                return;
            }
            if (PassengersCount != 0)
            {
                Console.WriteLine($"Количество пассажиров {PassengersCount}");
            }
            else
            {
                Console.WriteLine($"Билеты не куплены!");
                return;
            }
            Train.ShowInfo();
        }
    }

    class Train
    {
        private const int _minCarriagesAdd = 3;
        private const int _maxCarriagesAdd = 10;
        private List<Carriage> _carriages;

        public static int MinCarriagesAdd => _minCarriagesAdd;
        public static int MaxCarriagesAdd => _maxCarriagesAdd;

        public Train()
        {
            _carriages = new List<Carriage>();
        }

        public void AddCarriage(int capacity)
        {
            _carriages.Add(new Carriage(capacity, _carriages.Count + 1));
        }

        public void AddCarriages(int countToAdd)
        {
            int capacity;
            Console.WriteLine($"Вместимость вагона от {Carriage.MinSeatsNumber} до {Carriage.MaxSeatsNumber} мест.");
            for (int i = 0; i < countToAdd; i++)
            {
                Console.Write($"Введите количество мест {_carriages.Count + 1} - ого вагона: ");
                capacity = InputChecker.MakeChoice(Carriage.MinSeatsNumber, Carriage.MaxSeatsNumber);
                AddCarriage(capacity);
            }
        }

        public void Clear()
        {
            _carriages.Clear();
        }

        public int GetAmountCapacity()
        {
            int amountCapacity = 0;
            for (int i = 0; i < _carriages.Count; i++)
            {
                amountCapacity += _carriages[i].Capacity;
            }
            return amountCapacity;
        }

        public void SeatPassengers(int passengersCount)
        {
            int randomNumber;
            Random rand = new Random();
            for (int i = 0; i < passengersCount; i++)
            {
                randomNumber = rand.Next(1, _carriages.Count);
                bool result = _carriages[randomNumber].AddPassengers(1);
                while (result == false)
                {
                    randomNumber = (randomNumber + 1) % _carriages.Count;
                    result = _carriages[randomNumber].AddPassengers(1);
                }
            }
            Console.WriteLine("Пассажиры успешно распределены!");
        }

        public void ShowInfo()
        {
            if (_carriages.Count != 0)
            {
                Console.WriteLine($"Количество вагонов = {_carriages.Count}. Информация о вагонах:");
                Console.WriteLine("Номер\tВместимость\tКол-во свободных мест");
                for (int i = 0; i < _carriages.Count; i++)
                {
                    _carriages[i].ShowInfoAsTable();
                }
            }
            else
            {
                Console.WriteLine("Поезд не сформирован.");
            }
        }
    }

    class Carriage
    {
        private int _capacity;
        private int _freePlacesCount;
        private int _number;
        private const int _minSeatsNumber = 10;
        private const int _maxSeatsNumber = 20;

        public int Capacity { get => _capacity; }
        public int FreePlacesCount { get => _freePlacesCount; }
        public int Number { get => _number; }
        public static int MinSeatsNumber => _minSeatsNumber;
        public static int MaxSeatsNumber => _maxSeatsNumber;

        public Carriage(int capacity, int number)
        {
            _capacity = capacity;
            _freePlacesCount = _capacity;
            _number = number;
        }

        public bool AddPassengers(int count = 1)
        {
            if (_freePlacesCount == 0)
            {
                return false;
            }
            _freePlacesCount -= count;
            return true;
        }

        public void ShowInfoAsTable()
        {
            Console.WriteLine($"{_number}\t{_capacity}\t\t{_freePlacesCount}");
        }
    }
}
