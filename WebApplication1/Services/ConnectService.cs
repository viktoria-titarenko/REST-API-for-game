using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;

namespace WebApplication1.Services

{
    public class Key
    {
        public string KeyPlayer { get; set; }
        public string KeyGame { get; set; }

        public ViewType ViewType { get; set; }
    }

    public class ConnectService : IConnectService
    {
        private MyDbContext dbContext;

        public ConnectService(MyDbContext myDbContext)
        {
            dbContext = myDbContext;
        }

        /// <summary>
        /// Данный метод осуществяет проверку на корректность вида игрока.
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Key> IsViewCorrectAsync(Connect connect, CancellationToken cancellationToken)
        {
            if (connect.Type != ViewType.Circle && connect.Type != ViewType.Сross)
                throw new Exception("Uncorrect type view");
            else return await GetKeyAsync(connect, cancellationToken);
        }

        /// <summary>
        /// Данный метод позволяет получит KeyGame, KeyPlayer.
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        public async Task<Key> GetKeyAsync(Connect connect, CancellationToken cancellationToken)

        {
            var keya = await GetCountKeyGameAsync(connect.KeyGame, cancellationToken);

            if ((connect.KeyGame == "") || (keya.Count >= 2))
            {
                string key2 = GetRandomKey(connect);
                string key = GetRandomKey(connect);
                Key keys = new Key
                {
                    KeyPlayer = key,
                    KeyGame = key2,
                    ViewType = ViewType.Сross,
                };

                await AddKeyAsync(key, key2, ViewType.Сross, cancellationToken);
                await GetCountKeyGameAsync(connect.KeyGame, cancellationToken);
                return (keys);
            }
            else
            {
                if (keya.Count < 2)
                {
                    string key2 = connect.KeyGame;
                    string key = GetRandomKey(connect);
                    Key keys = new Key
                    {
                        KeyPlayer = key,
                        KeyGame = key2,
                        ViewType = ViewType.Circle,
                    };

                    await AddKeyAsync(key, key2, ViewType.Circle, cancellationToken);
                    await GetCountKeyGameAsync(connect.KeyGame, cancellationToken);
                    return (keys);
                }
            }

            {
                return new Key();
            }
        }


        /// <summary>
        /// Данный метод генерирует рандомный ключ.
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        public string GetRandomKey(Connect connect)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var key = new char[32];

            for (int i = 0; i < key.Length; i++)
            {
                key[i] = chars[random.Next(chars.Length)];
            }

            string randomKey = new string(key);

            //_properties.Add(connect.Type, randomKey);

            return randomKey;
        }


        /// <summary>
        /// Данный метод добавляет в базу данных сгенерированные KeyGame, KeyPLayer, View.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key2"></param>
        /// <param name="viewType"></param>
        public async Task AddKeyAsync(string key, string key2, ViewType viewType, CancellationToken cancellationToken)
        {
            dbContext.Keys.Add(new Keys() {KeyPlayer = key, KeyGame = key2, View = viewType});
            await dbContext.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Данный метод проверят сколько игроков создано для конкретной игры.
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        public async Task<List<Keys>> GetCountKeyGameAsync(string connect, CancellationToken cancellationToken)
        {
            var items = await dbContext.Keys
                .Where(x => x.KeyGame == connect)
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}