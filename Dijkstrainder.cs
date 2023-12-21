// Вставьте сюда финальное содержимое файла DijkstraPathFinder.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    class DijkstraData
    {
        public Point Previous { get; set; }
        public int Price { get; set; }
    }
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            HashSet<Point> hashSets = new HashSet<Point>();
            var open = hashSets;
            open.Add(start);
            var visit = new HashSet<Point>();
            var track = new Dictionary<Point, DijkstraData>
            {
                [start] = new DijkstraData { Price = 0, Previous = new Point(-1, -1) }
            };
            var chests = targets.ToList();
            while (true)
            {
                Point toOpen = new Point(-1, -1);
                var best = int.MaxValue;
                foreach (var x in open)
                {
                    if (track[x].Price < best)
                    {
                        toOpen = x;
                        best = track[x].Price;
                    }
                }

                if (toOpen == new Point(-1,-1))
                    yield break;
                var sosedi = Host(toOpen, state);
                foreach (var sosed in sosedi)
                {
                    if (!state.InsideMap(sosed))
                        continue;
                    var nowPrice = track[toOpen].Price + state.CellCost[sosed.X, sosed.Y];
                    if (!track.ContainsKey(sosed) || nowPrice < track[sosed].Price)
                    {
                        track[sosed] = new DijkstraData { Price = nowPrice, Previous = toOpen };
                    }
                    if (!visit.Contains(sosed))
                    {
                        open.Add(sosed);
                    }
                }

                if (chests.Contains(toOpen))
                {
                    if (chests.Count == 0)
                        yield break;
                    var open1 = toOpen;
                    var y = new List<Point>();
                    while (open1 != new Point(-1, -1))
                    {
                        y.Add(open1);
                        open1 = track[open1].Previous;
                    }
                    y.Reverse();
                    var result = new PathWithCost(track[toOpen].Price, y.ToArray());
                    chests.Remove(open1);

                    yield return result;
                }
                open.Remove(toOpen);
                visit.Add(toOpen);
            }
        }
        private IEnumerable<Point> Host(Point xy, State state)
        {
            return new Point[]
            {
                new Point(xy.X, xy.Y+1),
                new Point(xy.X+1, xy.Y),
                new Point(xy.X, xy.Y-1),
                new Point(xy.X-1, xy.Y)
                }.Where(point => state.InsideMap(point) && !state.IsWallAt(point));
        }
    }
}

