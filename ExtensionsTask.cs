// Вставьте сюда финальное содержимое файла ExtensionsTask.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		public static double Median(this IEnumerable<double> items)
		{
		    var mass= items.OrderBy(x => x).ToArray();
			try
			{
				if (mass.Length % 2 == 1)
					return mass[mass.Length / 2];
                else
					return (mass[mass.Length / 2 - 1] +
							mass[mass.Length / 2]) / 2;
			}

			catch 
			{ 
					throw new InvalidOperationException();
			}
		}

		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var i1 = default(T);
			var isFirst = true;
			foreach (var i in items)
			{
				if (isFirst)
				{
					i1 = i;
					isFirst = false;
					continue;
				}
				
				yield return Tuple.Create<T, T>(i1, i);
				i1 = i;
			}
		}
	}
}

