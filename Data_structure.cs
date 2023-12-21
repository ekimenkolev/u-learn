// Вставьте сюда финальное содержимое файла Category.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public Category(string name, MessageType type, MessageTopic top)
        {
            Name = name;
            Type = type;
            Top = top;
        }

        public string Name { get; private set; }
        public MessageType Type { get; private set; }
        public MessageTopic Top { get; private set; }
        public int CompareTo(object obj)
        {
            try
            {
                var cat = obj as Category;
                
                if (String.Compare(Name, cat.Name) < 0) return -1;
                else if (String.Compare(Name, cat.Name) > 0) return 1;

                if (Type < cat.Type) return -1;
                else if (Type > cat.Type) return 1;

                if (Top < cat.Top) return -1;
                else if (Top > cat.Top) return 1;

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Category cat
                && String.Compare(Name, cat.Name) == 0
                && Type.Equals(cat.Type)&& Top.Equals(cat.Top);
        }
        public override int GetHashCode()
        {
                if (Name == null)
                    return base.GetHashCode();
                int hash = 31;
                hash = (hash * 45) + Name.GetHashCode();
                hash = (hash * 13) + Top.GetHashCode();
                hash = (hash * 10) + Type.GetHashCode();
                return hash;
		}

        public override string ToString()
        {
			return string.Format("{0}.{1}.{2}", Name, Type, Top);
        }

        public static bool operator <(Category cat1, Category cat2) => 
		cat1.CompareTo(cat2) < 0;
        public static bool operator <=(Category cat1, Category cat2) => 
		cat1.CompareTo(cat2) <= 0;
        public static bool operator >(Category cat1, Category cat2) => 
		cat1.CompareTo(cat2) > 0;
        public static bool operator >=(Category cat1, Category cat2) => 
		cat1.CompareTo(cat2) >= 0;
    }
}

