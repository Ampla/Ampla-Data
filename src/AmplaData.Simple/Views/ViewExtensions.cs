using System.Collections.Generic;
using System.Linq;
using AmplaData.AmplaData2008;

namespace AmplaData.Views
{
    public static class ViewExtensions
    {
        public static GetViewsAllowedOperation[] AllowAll(this GetViewsAllowedOperation[] existingOperations)
        {
            return existingOperations.Allow(AllOperations);
        }

        public static GetViewsAllowedOperation[] Allow(this GetViewsAllowedOperation[] existingOperations,
                                                       params ViewAllowedOperations[] operations)
        {
            List<ViewAllowedOperations> allowedOperations = new List<ViewAllowedOperations>(operations ?? new ViewAllowedOperations[0]);
            List<ViewAllowedOperations> existingPermissions = (from operation in existingOperations where operation.Allowed select operation.Operation).ToList();

            return AllOperations.Select(operation => new GetViewsAllowedOperation
            {
                Operation = operation,
                Allowed = existingPermissions.Contains(operation) || allowedOperations.Contains(operation)
            }).ToArray();
        }

        public static GetViewsAllowedOperation[] Disallow(this GetViewsAllowedOperation[] existingOperations,
                                                params ViewAllowedOperations[] operations)
        {
            List<ViewAllowedOperations> disallowedOperations = new List<ViewAllowedOperations>(operations ?? new ViewAllowedOperations[0]);
            List<ViewAllowedOperations> existingPermissions = (from operation in existingOperations where operation.Allowed select operation.Operation).ToList();

            return AllOperations.Select(operation => new GetViewsAllowedOperation
            {
                Operation = operation,
                Allowed = existingPermissions.Contains(operation) && !disallowedOperations.Contains(operation)
            }).ToArray();
        }

        public static readonly ViewAllowedOperations[] AllOperations = new[]
            {
                ViewAllowedOperations.AddRecord, 
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord, 
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.SplitRecord, 
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord
            };

    }
}