using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FencingGame.Persistence
{
    public interface IGameSave
    {
        [JsonIgnore]
        public String Extension { get; }
    }
}
