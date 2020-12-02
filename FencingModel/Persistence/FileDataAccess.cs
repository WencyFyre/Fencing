using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FencingGame.Persistence
{
    public class FileDataAccess<T> : IFencingDataAccess<T> where T : IGameSave
    {
        public async Task<T> LoadAsync(string path) => JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));


        public Task SaveAsync(string path, T table) => File.WriteAllTextAsync(Path.ChangeExtension(path, table.Extension), JsonConvert.SerializeObject(table));

    }
}
