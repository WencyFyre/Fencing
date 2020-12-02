using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FencingGame.Persistence
{
    public interface IFencingDataAccess<T> where T : IGameSave
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<T> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String path, T table);
    }
}
