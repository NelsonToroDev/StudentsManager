namespace StudentsManager.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class that adds a sort method by string expression to the <c>IEnumerable</c> class.
    /// </summary>
    public static class SortByExpressionExtension
    {
        /// <summary>
        /// Orders the <c>IEnumerable</c> using the sort expression.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="sortExpression">
        /// The sort expression.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// A ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> OrderByExpression<TEntity>(this IEnumerable<TEntity> source, string sortExpression) where TEntity : class
        {
            return OrderByExpression(source.AsQueryable(), sortExpression);
        }

        /// <summary>
        /// Orders the <c>IQueryable</c> using the sort expression.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="sortExpression">
        /// The sort expression.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// A ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> OrderByExpression<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            // Just to avoid use reflection (to get properties) in case that this collection does not have elements to sort.
            if (source.Any() == false || IsAnInvalidSortExpression(sortExpression))
            {
                return source as IOrderedQueryable<TEntity>;
            }

            string[] individualExpressionsOfOrder = sortExpression.Split(',');

            IOrderedQueryable<TEntity> result = null;

            for (int currentFieldIndex = 0; currentFieldIndex < individualExpressionsOfOrder.Length; currentFieldIndex++)
            {
                string[] expressionParts = individualExpressionsOfOrder[currentFieldIndex].Trim().Split(' ');

                if (expressionParts.Length < 2)
                {
                    throw new ArgumentException("The sort criteria should have the next format:PropertyName Asc");
                }

                // For example 'Id'.
                string orderByField = expressionParts[0];

                // For example 'ASC'.
                string orderDirection = expressionParts[1];

                bool descendingOrder = (expressionParts.Length == 2) && orderDirection.StartsWith("DESC", StringComparison.OrdinalIgnoreCase);

                if (descendingOrder)
                {
                    result = currentFieldIndex == 0 ? source.OrderByDescending(orderByField) : result.ThenByDescending(orderByField);
                }
                else
                {
                    result = currentFieldIndex == 0 ? source.OrderBy(orderByField) : result.ThenBy(orderByField);
                }
            }

            return result;
        }

        /// <summary>
        /// Orders <c>ascending</c> by the field name.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type
        /// </typeparam>
        /// <returns>
        /// The ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        /// Orders <c>descending</c> by the field name.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type
        /// </typeparam>
        /// <returns>
        /// The ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        /// Orders <c>ascending</c> keeping the last ordered.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type
        /// </typeparam>
        /// <returns>
        /// The ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        /// Orders <c>descending</c> keeping the last ordered.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type
        /// </typeparam>
        /// <returns>
        /// The ordered enumerable.
        /// </returns>
        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        /// Verifies if the string is  null, empty or filled with spaces.
        /// </summary>
        /// <param name="sortExpression">The sort expression</param>
        /// <returns>True is is an invalid expression.</returns>
        private static bool IsAnInvalidSortExpression(string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
            {
                return true;
            }

            return string.IsNullOrEmpty(sortExpression.Trim());
        }

        /// <summary>
        /// Generates a method call expression.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// The method call expression.
        /// </returns>
        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, string fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(
                                            typeof(Queryable), 
                                            methodName,
                                            new[] { type, selectorResultType },
                                            source.Expression, 
                                            Expression.Quote(selector));
            return resultExp;
        }

        /// <summary>
        /// Generates the selector.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <typeparam name="TEntity">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// The lambda expression.
        /// </returns>
        private static LambdaExpression GenerateSelector<TEntity>(string propertyName, out Type resultType) where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "Entity");

            // create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;

            if (propertyName.Contains('.'))
            {
                // support to be sorted for child fields.
                string[] childProperties = propertyName.Split('.');

                property = GetProperty(typeof(TEntity), childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = GetProperty(property.PropertyType, childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = GetProperty(typeof(TEntity), propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            resultType = property.PropertyType;
            
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        /// <summary>
        /// Gets the property according to the property name.
        /// </summary>
        /// <param name="ownerType">
        /// The owner type.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The property.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the property does not exist.
        /// </exception>
        private static PropertyInfo GetProperty(Type ownerType, string propertyName)
        {
            PropertyInfo property = ownerType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (property == null)
            {
                throw new ArgumentOutOfRangeException(propertyName, string.Format("The property '{0}' could not be found in '{1}' entity.", propertyName, ownerType.Name));
            }

            return property;
        }
    }
}
