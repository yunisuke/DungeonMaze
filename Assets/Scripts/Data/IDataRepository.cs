using System.Collections.Generic;

namespace Data
{
    public interface IDataRepository
    {
        public void Save(List<ClearData> clearData);
        public List<ClearData> Load();
        public void Delete();
    }
}
