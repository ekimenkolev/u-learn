using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
using System.Linq;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            HashSet<Point> chests = new HashSet<Point>(state.Chests);//набор сундуков
            List<Point> result = new List<Point>();//итоговый лист
            DijkstraPathFinder pathFinder = new DijkstraPathFinder();//из прошлой практики
            var position = state.Position;//позиция

            if (state.Chests.Count < state.Goal)//если сундуков меньше, чем требуется
            {
                return new List<Point>();//возвращаем пустой список
            }

            return FindPath(state, chests, result, pathFinder, position);
        }

        private static List<Point> FindPath(State state, HashSet<Point> chests, List<Point> result,
                                             DijkstraPathFinder pathFinder, Point position)
        {
            for (int i = 0; i < state.Goal; i++)//пока не наберётся нужное кол-во сундуков
            {
                if (!chests.Any())//если сундуков нет
                    return new List<Point>();//возвращаем пустой список

                var pathToChest = pathFinder.GetPathsByDijkstra(state, position, chests).FirstOrDefault();
                if (pathToChest == null)//если невозможно найти путь
                    return new List<Point>();//возвращаем пустой список
                if (state.Energy < pathToChest.Cost)//если не хватает энергии
                    return new List<Point>();//возвращаем пустой список
                position = pathToChest.End;

                for (int j = 1; j < pathToChest.Path.Count; j++)//добавляем путь в итоговый лист
                    result.Add(pathToChest.Path[j]);

                chests.Remove(position);//удаляем сундук из списка сундуков
            }

            return result;
        }
    }
}
