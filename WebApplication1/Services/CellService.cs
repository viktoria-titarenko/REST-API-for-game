using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1
{
    public class CellService : ICellService
    {
        public CellService(MyDbContext myDbContext, IConnectService connectService)
        {
            dbContext = myDbContext;
            _connectService = connectService;
        }

        private MyDbContext dbContext;
        private IConnectService _connectService;

        /// <summary>
        /// Данный метод проверяет корректность отправленный в запросе данных, таких как KeyGame, KeyPlayer, View.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> IsKeyCorrectAsync(Cell cell, CancellationToken cancellationToken)

        {
            var keys = await _connectService.GetCountKeyGameAsync(cell.KeyGame, cancellationToken);

            if ((cell.KeyGame == keys[0].KeyGame && cell.KeyPlayer == keys[0].KeyPlayer && cell.View == keys[0].View) ||
                (cell.KeyGame == keys[1].KeyGame && cell.KeyPlayer == keys[1].KeyPlayer && cell.View == keys[1].View))
                return await ProcessAsync(cell, cancellationToken);
            else
            {
                if (cell.KeyPlayer != keys[0].KeyPlayer || cell.KeyPlayer != keys[1].KeyPlayer)
                {
                    throw new Exception("Uncorrect key player!");
                }
                else if (cell.KeyGame != keys[0].KeyGame || cell.KeyGame != keys[1].KeyGame)
                {
                    throw new Exception("Uncorrect key game!");
                }
                else
                {
                    throw new Exception("Uncorrect type view");
                }
            }
        }


        /// <summary>
        /// Данный метод реализует процесс игры.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public async Task<string> ProcessAsync(Cell cell, CancellationToken cancellationToken)
        {
            if (await GetWinnerAsync(cell.KeyGame, cancellationToken))
            {
                return "Game is over! Please start new game!";
            }

            if (await KeyExistsAsync(cell.Value, cell.KeyGame, cancellationToken))
            {
                throw new BadHttpRequestException("Cell is not empty!");
            }


            if (cell.KeyPlayer == await GetLastMoveAsync(cell.KeyGame, cancellationToken))
            {
                throw new BadHttpRequestException("Now is not your turn!");
            }

            await AddCellAsync(cell.Value, cell.KeyGame, cell.KeyPlayer, cancellationToken);
            var player1 = await GetPositionsPlayerAsync(cell.KeyPlayer, cancellationToken);

            int[] won1 = {1, 2, 3};
            int[] won2 = {1, 5, 9};
            int[] won3 = {1, 4, 7};
            int[] won4 = {2, 5, 8};
            int[] won5 = {3, 6, 9};
            int[] won6 = {3, 5, 7};
            int[] won7 = {4, 5, 6};
            int[] won8 = {7, 8, 9};
            if (won1.All(e => player1.Contains(e))
                || won2.All(e => player1.Contains(e))
                || won3.All(e => player1.Contains(e))
                || won4.All(e => player1.Contains(e))
                || won5.All(e => player1.Contains(e))
                || won6.All(e => player1.Contains(e))
                || won7.All(e => player1.Contains(e))
                || won8.All(e => player1.Contains(e)))
            {
                ViewType winner = cell.View;
                string win = "winner:" + winner;
                AddWinnerAsync(cell.KeyGame, winner, cancellationToken);
                return win;
            }
            else
            {
                var draw = await GetPositionAllAsync(cell.KeyGame, cancellationToken);
                if (draw.Count == 9)
                {
                    return "You have a draw! ";
                }
            }

            return "Keep going!";
        }


        /// <summary>
        /// Данный метод предназначен для проверки свободна ли ячейка.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="keyGame"></param>
        /// <returns></returns>
        /// 
        public async Task<bool> KeyExistsAsync(int position, string keyGame, CancellationToken cancellationToken)
        {
            var b = await dbContext.ProcessGame.Where(k => k.Position == position)
                .AnyAsync(k => k.KeyGame == keyGame, cancellationToken);

            return b;
        }

        /// <summary>
        /// Данный метод доьавляет в базу данных ход игрока.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="keyGame"></param>
        /// <param name="keyPlayer"></param>
        public async Task AddCellAsync(int position, string keyGame, string keyPlayer,
            CancellationToken cancellationToken)
        {
            dbContext.ProcessGame.Add(new ProcessGame()
            {
                KeyPlayer = keyPlayer, KeyGame = keyGame, Position = position, Id = Guid.NewGuid(),
                createdAt = DateTime.Now
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Данный метод получает все занятые ячейки игрком.
        /// </summary>
        /// <param name="keyPlayer"></param>
        /// <returns></returns>
        public async Task<List<int>> GetPositionsPlayerAsync(string keyPlayer, CancellationToken cancellationToken)
        {
            return await dbContext.ProcessGame.Where(k => k.KeyPlayer == keyPlayer).Select(k => k.Position)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Данный метод получает все ячейки данной игры, для определения "Ничьи".
        /// </summary>
        /// <param name="keyGame"></param>
        /// <returns></returns>
        public async Task<List<int>> GetPositionAllAsync(string keyGame, CancellationToken cancellationToken)
        {
            return await dbContext.ProcessGame.Where(k => k.KeyGame == keyGame).Select(k => k.Position)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Данный метод получает вид игрока, который сделал последний ход в игре.
        /// </summary>
        /// <param name="keyGame"></param>
        /// <returns></returns>
        public async Task<string> GetLastMoveAsync(string keyGame, CancellationToken cancellationToken)
        {
            if (await dbContext.ProcessGame
                    .AnyAsync(e => e.KeyGame == keyGame, cancellationToken))
            {
                var lastAddedItem = await dbContext.ProcessGame
                    .Where(item => item.KeyGame == keyGame)
                    .OrderByDescending(item => item.createdAt)
                    .FirstOrDefaultAsync(cancellationToken);
                return lastAddedItem.KeyPlayer;
            }
            else return "";
        }

        /// <summary>
        /// Данный метод добавляет в таблицу данные о победителе игры.
        /// </summary>
        /// <param name="keyGame"></param>
        /// <param name="win"></param>
        public async Task AddWinnerAsync(string keyGame, ViewType win, CancellationToken cancellationToken)
        {
            dbContext.Winners.Add(new Winners()
                {KeyGame = keyGame, Winner = win});
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Данный метод проверяет, есть ли в игре победитель(окончена ли игра).
        /// </summary>
        /// <param name="keyGame"></param>
        /// <returns></returns>
        public async Task<bool> GetWinnerAsync(string keyGame, CancellationToken cancellationToken)
        {
            bool result = await dbContext.Winners
                .AnyAsync(e => e.KeyGame == keyGame, cancellationToken);
            return result;
        }
    }
}