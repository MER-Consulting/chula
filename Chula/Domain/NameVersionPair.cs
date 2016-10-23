using System;

namespace MER.Chula.Domain
{
    public struct NameVersionPair : IEquatable<NameVersionPair>
    {
        public static readonly NameVersionPair Null = new NameVersionPair();
        private readonly string name;
        private readonly uint version;

        public NameVersionPair(string name, uint version)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.version = version;
        }

        public bool Equals(NameVersionPair other)
        {
            return this.name == null ? other.name == null : this.name == other.name && this.version == other.version;
        }

        public override bool Equals(object obj)
        {
            return obj is NameVersionPair && this.Equals((NameVersionPair)obj);
        }

        public override int GetHashCode()
        {
            return this.name == null ? 0 : this.name.GetHashCode() ^ this.version.GetHashCode();
        }

        public string Name
        {
            get
            {
                if (this.name == null)
                    throw new InvalidOperationException();

                return this.name;
            }
        }

        public uint Version
        {
            get
            {
                if (this.name == null)
                    throw new InvalidOperationException();

                return this.version;
            }
        }

        public static bool operator ==(NameVersionPair pair1, NameVersionPair pair2)
        {
            return pair1.Equals(pair2);
        }

        public static bool operator !=(NameVersionPair pair1, NameVersionPair pair2)
        {
            return !pair1.Equals(pair2);
        }
    }
}
