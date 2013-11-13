using System;
using System.Collections.Generic;
using System.Text;

namespace AmplaData.Data.Binding.ModelData.Validation
{
    /// <summary>
    ///     Validation Messages class for storing messages
    /// </summary>
    public class ValidationMessages
    {
        private readonly List<string> messages = new List<string>();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return messages.Count; }
        }

        /// <summary>
        /// Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Add(string message)
        {
            messages.Add(message);
        }

        /// <summary>
        /// Throws a new InvalidOperationException with the messages.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Throw()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} validation messages:", messages.Count);
            foreach (string message in messages)
            {
                builder.AppendLine(message);
            }

            throw new InvalidOperationException(builder.ToString());
        }
    }
}