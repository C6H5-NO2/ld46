using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    class PathSearch
    {
        public PathSearch(Vector3 src, Vector3 dst, GridManager.MapObj unmoveable = GridManager.MapObj.Air)
        {
            map = GridManager.Instance.map;
            start = new Vector2Int((int)src.x, (int)src.z);
            end = new Vector2Int((int)dst.x, (int)dst.z);
            this.unmoveable = unmoveable;
        }

        private GridManager.MapObj[,] map;
        private Vector2Int start;
        private Vector2Int end;
        private GridManager.MapObj unmoveable;

        private int CalcG(Point p)
        {
            int g = p.parent?.G ?? 0;
            return g + 1;
        }

        private int CalcH(Point p)
        {
            //  Manhattan distance
            return Math.Abs(p.pos.x - end.x) + Math.Abs(p.pos.y - end.y);
        }

        private class Point
        {
            public Point parent = null;
            public Vector2Int pos;
            public int F = 0;
            public int G = 0;
        }

        private List<Point> mOpenList = new List<Point>();
        private List<Point> mCloseList = new List<Point>();

        private int FindMinInOpenList()
        {
            // Find min cost.
            int min = Int32.MaxValue;
            int index = 0;
            for (int i = 0; i < mOpenList.Count; ++i)
            {
                if (mOpenList[i].F < min)
                {
                    min = mOpenList[i].F;
                    index = i;
                }
            }

            return index;
        }

        private int FindInOpenList(Vector2Int pos)
        {
            for (int i = 0; i < mOpenList.Count; ++i)
            {
                if (mOpenList[i].pos == pos)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsInCloseList(Vector2Int pos)
        {
            for (int i = 0; i < mCloseList.Count; ++i)
            {
                if (mCloseList[i].pos == pos)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Vector2Int> Solve()
        {
            if (start == end)
            {
                return null;
            }

            int w = map.GetLength(0);
            int h = map.GetLength(1);

            Point startPint = new Point();
            startPint.pos = start;
            mOpenList.Add(startPint);

            bool found = false;
            while (!found && mOpenList.Count != 0)
            {
                // Find min cost.
                int index = FindMinInOpenList();

                Point curr = mOpenList[index];
                mOpenList.RemoveAt(index);

                mCloseList.Add(curr);

                // Check reachable points
                Vector2Int[] surround = new Vector2Int[4];
                surround[0] = new Vector2Int(curr.pos.x, curr.pos.y - 1); // Up
                surround[1] = new Vector2Int(curr.pos.x + 1, curr.pos.y); // Right
                surround[2] = new Vector2Int(curr.pos.x, curr.pos.y + 1); // Down
                surround[3] = new Vector2Int(curr.pos.x - 1, curr.pos.y); // Left

                foreach (Vector2Int next in surround)
                {
                    // Out of range
                    if (0 > next.x || next.x >= w || 0 > next.y || next.y >= h)
                        continue;

                    // Can not reach
                    if ((map[next.x, next.y] & unmoveable) != GridManager.MapObj.Null)
                        continue;

                    // Ignore points in close list
                    if (IsInCloseList(next))
                        continue;

                    // If not in open list
                    int indexInOpen = FindInOpenList(next);
                    if (indexInOpen == -1)
                    {
                        Point nextPoint = new Point();
                        nextPoint.parent = curr;
                        nextPoint.pos = next;

                        nextPoint.G = CalcG(nextPoint);
                        nextPoint.F = nextPoint.G + CalcH(nextPoint);

                        mOpenList.Add(nextPoint);

                        // Reach the end
                        if (next == end)
                        {
                            found = true;
                            break;
                        }
                    }
                    else
                    {
                        Point nextPoint = mOpenList[indexInOpen];
                        int tempG = curr.G + 1;
                        if (tempG < nextPoint.G)
                        {
                            nextPoint.parent = curr;
                            nextPoint.G = tempG;
                            nextPoint.F = tempG + CalcH(nextPoint);
                        }
                    }
                }
            }

            List<Vector2Int> path = null;
            if (found)
            {
                // The last point we add into openlist is the end.
                path = new List<Vector2Int>();
                Point point = mOpenList.Last();
                while (point != null)
                {
                    path.Add(point.pos);
                    point = point.parent;
                }

                path.Reverse();
            }

            mOpenList.Clear();
            mCloseList.Clear();
            return path;
        }

        public Vector2Int NextGrid()
        {
            var path = Solve();
            if (path != null && path.Count > 1)
            {
                return path[1];
            }

            return start;
        }
    }
