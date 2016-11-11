using System;
using System.Text.RegularExpressions;

namespace MER.Chula.Domain
{
    public struct NameVersionPair : IEquatable<NameVersionPair>
    {
        private static Regex nameRex = new Regex(@"^\p{L}+$");
        private static Regex reprRex = new Regex(@"^(\p{L}+)-(\d+)$");
        public static readonly NameVersionPair Null = new NameVersionPair();
        private readonly string name;
        private readonly uint version;

        public NameVersionPair(string name, uint version)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!nameRex.IsMatch(name))
                throw new ArgumentException("Argument must only contain letters.", nameof(name));

            this.name = name;
            this.version = version;
        }

        public static NameVersionPair Parse(string repr)
        {
            var match = reprRex.Match(repr);

            if (match.Success)
                return new NameVersionPair(match.Groups[1].Value, uint.Parse(match.Groups[2].Value));
            else
                throw new FormatException();
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

        public override string ToString()
        {
            if (this.name == null)
                return "[NULL]";

            return string.Format("{0}-{1}", this.name, this.version);
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
