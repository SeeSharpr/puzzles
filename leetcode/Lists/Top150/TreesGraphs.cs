using System.Runtime.CompilerServices;

namespace leetcode.Lists.Top150
{
    public class TreesGraphs
    {
        // 200. Number of Islands
        // Given an m x n 2D binary grid grid which represents a map of '1's(land) and '0's(water)| return the number of islands.
        // An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. You may assume all four edges of the grid are all surrounded by water.
        [Trait("Company", "Amazon")]
        [Theory]
        [InlineData("1,1,1,1,0|1,1,0,1,0|1,1,0,0,0|0,0,0,0,0", 1)]
        [InlineData("1,1,0,0,0|1,1,0,0,0|0,0,1,0,0|0,0,0,1,1", 3)]
        [InlineData("1,0,1,1,0,1,1", 3)]
        public void NumIslands(string input, int expected)
        {
            char[][] grid = input.ParseNestedArrayStringLC(char.Parse).Select(e => e.ToArray()).ToArray();

            static void NukeIsland(char[][] grid, int x, int y)
            {
                grid[y][x] = '0';

                if (x - 1 >= 0 && grid[y][x - 1] == '1') NukeIsland(grid, x - 1, y);
                if (x + 1 < grid[y].Length && grid[y][x + 1] == '1') NukeIsland(grid, x + 1, y);
                if (y - 1 >= 0 && grid[y - 1][x] == '1') NukeIsland(grid, x, y - 1);
                if (y + 1 < grid.Length && grid[y + 1][x] == '1') NukeIsland(grid, x, y + 1);
            }

            int actual = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '1')
                    {
                        actual++;
                        NukeIsland(grid, x, y);
                    }
                }
            }

            Assert.Equal(expected, actual);
        }
    }
}
