using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AmplaData.Data.Binding.Mapping;

namespace AmplaData.Data.Records
{
    /// <summary>
    /// Represents an Ampla Record from the GetData web-service
    /// Fields are stored in an internal data table
    /// </summary>
    public class AmplaRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaRecord"/> class.
        /// </summary>
        /// <param name="recordId">The record unique identifier.</param>
        public AmplaRecord(int recordId)
        {
            Id = recordId;
            dataStore = new DataTable("Record");
            AddColumn("Location", typeof (string));
            dataStore.Rows.Add();
        }

        /// <summary>
        /// The Record Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The type of model
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location
        {
            get { return (string) GetValue("Location"); }
            set { SetValue("Location", value); }
        }

        /// <summary>
        /// The Ampla Module
        /// </summary>
        public string Module { get; set; }

        private readonly DataTable dataStore;

        private readonly List<string> mappedProperties = new List<string>();

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="dataType">Type of the data.</param>
        public void AddColumn(string fieldName, Type dataType)
        {
            DataColumn column = dataStore.Columns[fieldName];
            if (column == null)
            {
                dataStore.Columns.Add(new DataColumn(fieldName, dataType));
            }
            else
            {
                if (column.DataType != dataType)
                {
                    string message =
                        string.Format(
                            "The column: {0} already exists with DataType: {1}.  Unable to change DataType to {2}",
                            fieldName, column.DataType, dataType);
                    throw new ArgumentException(message);
                }
            }
        }

        /// <summary>
        /// Gets the field names.
        /// </summary>
        /// <returns></returns>
        public string[] GetFieldNames()
        {
            return (from DataColumn column in dataStore.Columns select column.ColumnName).ToArray();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string field, string value)
        {
            dataStore.Rows[0][field] = value;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public object GetValue(string field)
        {
            object value = dataStore.Rows[0][field];
            return value == DBNull.Value ? null : value;
        }

        public bool IsMapped(string field)
        {
            return mappedProperties.Contains(field);
        }

        /// <summary>
        ///     Set the Mapped properties
        /// </summary>
        /// <param name="getFieldMappings"></param>
        public void SetMappedProperties(IEnumerable<FieldMapping> getFieldMappings)
        {
            foreach (var mapping in getFieldMappings)
            {
                //if (mapping.CanWrite)
                {
                    mappedProperties.Add(mapping.Name);
                }
            }
        }

        public T GetValueOrDefault<T>(string fieldName, T defaultValue)
        {
            object value = null;
            if (dataStore.Columns.Contains(fieldName))
            {
                value = GetValue(fieldName);
            }
            if (value == null)
            {
                value = defaultValue;
            }
            return (T) Convert.ChangeType(value, typeof (T));
        }
    }
}