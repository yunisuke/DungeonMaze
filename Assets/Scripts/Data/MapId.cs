using System;

namespace Data
{
    [Serializable]
    public class MapId
    {
        public string FileName;

        public MapId(string name)
        {
            FileName = name;
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as MapId;
            if (other == null) return false;

            return this.FileName == other.FileName;
        }
    }
}
