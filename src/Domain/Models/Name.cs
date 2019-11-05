using System;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Name : ValueObject<Name>
    {
        public string First { get; private set; }
        public string Last { get; private set; }

        public Name(string first, string last)
        {
            if (string.IsNullOrWhiteSpace(first))
                throw new ArgumentNullException("first", "Firstname cannot be null or whitespace");

            if (string.IsNullOrWhiteSpace(last))
                throw new ArgumentNullException("last", "Lastname cannot be null or whitespace");

            First = first;
            Last = last;
        }
    }
}
