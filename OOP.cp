Инкапсуляция. Пример
Практика «Предприятие»
using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public readonly Guid Guid1;

        public Guid Guid => Guid1;
        public Enterprise(Guid guid)
        {
            Guid1 = guid;
        }

        public string Name { get; set; }

        string inn;

        public string Inn
        {
            get => inn;
            set
            {
                if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                inn = value;
            }
        }

        public DateTime EstablishDate { get; set; }


        public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == Guid1))
                amount += t.Amount;
            return amount;
        }
    }
}

Практика «Веса»
using System;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private double[] arr;
        private int start;
        private int length;

        public int Start
        {
            get => start;
            set
            {
                if (value < 0 || value > arr.Length)
                    throw new ArgumentException();

                start = value;
            }
        }

        public int Length
        {
            get => length;
            set
            {
                if (value < 0 || value > arr.Length)
                    throw new ArgumentException();

                length = value;
            }
        }

        public Indexer(double[] arr, int start, int length)
        {
            if (start + length > arr.Length)
                throw new ArgumentException();

            this.arr = arr;
            Start = start;
            Length = length;
        }

        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= length)
                    throw new IndexOutOfRangeException();

                index += start;
                return arr[index];
            }

            set
            {
                if (index < 0 || index >= length)
                    throw new IndexOutOfRangeException();

                index += start;
                arr[index] = value;
            }
        }
    }
}

Наследование и полиморфизм. Пример
Практика «Структура данных»
using System;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Name { get; set; }//имя
        public MessageType Type { get; set; }//тип
        public MessageTopic Topic { get; set; }//тема

        public Category(string name, MessageType messageType, MessageTopic messageTopic)
        {
            Name = name;
            Type = messageType;
            Topic = messageTopic;
        }

        public int CompareTo(object obj)//реализация интерфейса IComparable
        {
            if (!(obj is Category Compcategory))
            {
                return 0;//если сравниваем с Null Object
            }
            int nameComp = string.Compare(Name, Compcategory.Name);
            if (nameComp == 0)
            {
                int typeComp = Type.CompareTo(Compcategory.Type);
                if (typeComp == 0)
                {
                    int topicComp = Topic.CompareTo(Compcategory.Topic);
                    if (topicComp == 0)
                        return 0;
                    else return topicComp;
                }
                else return typeComp;
            }
            else return nameComp;
        }
        //переопределение HashCode 
        public override int GetHashCode()
        {
            unchecked
            {
                if (Name == null)//если сравниваем с Null Name
                    return base.GetHashCode();
                int hash = 31;
                hash = (hash * 11) + Name.GetHashCode();
                hash = (hash * 11) + Topic.GetHashCode();
                hash = (hash * 11) + Type.GetHashCode();
                return hash;
            }
        }
        //переопределение Equals
        public override bool Equals(object obj)
        {
            if (!(obj is Category Compcategory))
                return false;
            return Name == Compcategory.Name
                && Type == Compcategory.Type
                && Topic == Compcategory.Topic;
        }
        //реализация всех операторов сравнения
        public static bool operator >(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) > 0;
        }

        public static bool operator <(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) < 0;
        }

        public static bool operator >=(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) >= 0;
        }

        public static bool operator <=(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) <= 0;
        }
        //переопределение ToString
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", Name, Type, Topic);
        }
    }
}

Практика «HoMM»
namespace Inheritance.MapObjects
{
    //Создаём интерфейсы
    interface IOwner
    {
        int Owner { get; set; }
    }

    interface IArmy
    {
        Army Army { get; set; }
    }

    interface ITreasure
    {
        Treasure Treasure { get; set; }
    }
    //подключаем к классам необходимые интерфейсы
    public class Dwelling : IOwner
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwner, IArmy, ITreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmy, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            //Здесь и далее используйте следующий сокращенный синтаксис преобразования типа
            if ((mapObject is IOwner dwellingOwner) && !(mapObject is IArmy) && !(mapObject is ITreasure))
            {
                //Он более короткий и позволяет не производить множественных преобразований таких, как
                //((Dwelling)mapObject).Owner = player.Id;

                //а сразу обращаться к объекту, если он является каким-то классом.
                dwellingOwner.Owner = player.Id;

                return;
            }

            //Перед выполнение задания потренируйтесь и замените неправильное использование is ниже
            if ((mapObject is IOwner mineOwner) && (mapObject is IArmy mineArmy) && (mapObject is ITreasure mineTreasure))
            {
                if (player.CanBeat(mineArmy.Army))
                {
                    mineOwner.Owner = player.Id;
                    player.Consume(mineTreasure.Treasure);
                }
                else player.Die();
                return;
            }

            if (!(mapObject is IOwner) && (mapObject is IArmy creepsArmy) && (mapObject is ITreasure creepsTreasure))
            {//проверяем поддержку интерфейсов (в случае с Creeps нам необходимы IArmy и ITreasure и не нужен IOwner)
                if (player.CanBeat(creepsArmy.Army))//приводим к типу интерфейса
                    player.Consume(creepsTreasure.Treasure);
                else
                    player.Die();
                return;
            }

            if (!(mapObject is IOwner) && !(mapObject is IArmy) && (mapObject is ITreasure pileTreasure))
            {
                player.Consume(pileTreasure.Treasure);
                return;
            }

            if (!(mapObject is IOwner) && (mapObject is IArmy wolvesArmy) && !(mapObject is ITreasure))
            {
                if (!player.CanBeat(wolvesArmy.Army))
                    player.Die();
            }
        }
    }
}
