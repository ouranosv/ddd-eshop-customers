using System;
using System.Text.RegularExpressions;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Email : ValueObject<Email>
    {
        public string Value { get; private set; }

        public Email(string email)
        {
            if (email == null)
                throw new ArgumentNullException("email", "Email cannot be null");

            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("Not valid email", "email");
            }

            Value = email;
        }
    }
}
