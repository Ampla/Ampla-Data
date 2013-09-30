

using System.Collections.Generic;
using AmplaWeb.Data.Membership;

namespace AmplaWeb.Security.Membership
{
    public class AmplaRoleProvider : ReadOnlyRoleProvider
    {
        private readonly string[] roles;


        public AmplaRoleProvider()
        {
            roles = new List<string>
                {
                    "ViewRecord",
                    "AddRecord",
                    "EditRecord"
                }.ToArray();
        }


        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string[] GetRolesForUser(string username)
        {
            return roles;
        }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string[] GetAllRoles()
        {
            return roles;
        }
    }
}