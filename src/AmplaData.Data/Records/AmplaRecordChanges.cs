using System;

namespace AmplaData.Data.Records
{
    public class AmplaRecordChanges : IComparable<AmplaRecordChanges>
    {
        public string Operation { get; set; }
        public string User { get; set; }
        public DateTime VersionDateTime { get; set; }
        public AmplaAuditField[] Changes { get; set; }
        public string Display { get; set; }

        public int CompareTo(AmplaRecordChanges other)
        {
            int compare = VersionDateTime.CompareTo(other.VersionDateTime);
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(User, other.User);
            }
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(Operation, other.Operation);
            }
            return compare;
        }
    }
}