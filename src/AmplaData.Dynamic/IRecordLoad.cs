using System;

namespace AmplaData.Dynamic
{
    public interface IRecordLoad
    {
        /// <summary>
        /// Adds the column definition
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="dataType">Type of the data.</param>
        void AddColumn(string field, Type dataType);

        /// <summary>
        ///     Set the value using the Invariant value
        /// </summary>
        /// <param name="field"></param>
        /// <param name="invariantValue"></param>
        void SetValue(string field, string invariantValue);
    }
}