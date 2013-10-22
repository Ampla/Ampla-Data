using System;
using System.Collections.Generic;

namespace AmplaWeb.Data.Records
{
    public class AmplaAuditSession : IComparable<AmplaAuditSession>
    {
        public AmplaAuditSession(string user, DateTime editedTime)
        {
            User = user;
            EditedTime = editedTime;
            Fields = new List<AmplaAuditField>();
        }

        public string User { get; private set; }

        public DateTime EditedTime { get; set; }

        public IList<AmplaAuditField> Fields { get; private set; }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(AmplaAuditSession other)
        {
            int compare = EditedTime.CompareTo(other.EditedTime);
            if (compare == 0)
            {
                compare = -Fields.Count.CompareTo(other.Fields.Count);
            }
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(User, other.User);
            }
            return compare;
        }
    }
}