using System;
using System.Data;
using System.Data.SqlClient;

namespace QA.Validation.Xaml.RuleConditions
{
    public abstract class SqlCondition: PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            if (!OnValidating(context))
            {
                return false;
            }

            if (context.ServiceProvider == null)
                throw new InvalidOperationException("Service provider is not specified. Contact developers for futher information.");

            var connection = context.ServiceProvider.GetService(typeof(SqlConnection)) as SqlConnection;

            if (connection == null)
                throw new InvalidOperationException("SqlConnection is not specified. Contact developers for futher information.");

            bool forceClose = false;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    forceClose = true;
                    connection.Open();
                }
                else if (connection.State != ConnectionState.Open)
                {
                    throw new InvalidOperationException(string.Format("The connection is in invalid state (expected Open|Close, but actual state is {0})", connection.State));
                }

                // производим обращение к БД
                return Validate(connection, context);
            }
            finally
            {
                if (forceClose)
                    connection.Close();
            }
        }

        /// <summary>
        /// Метод, вызывающийся беред открытием подключения к БД.
        /// Здесь необходимо произвести проверки валидности правила.
        /// </summary>
        /// <param name="context">контекст валидации</param>
        /// <returns>результат проверок</returns>
        protected abstract bool OnValidating(ValidationConditionContext context);

        /// <summary>
        /// Метод, вызывающийся для произведения валидации. 
        /// При этом передается открытое подключение к БД.
        /// </summary>
        /// <param name="connection">подключение к БД</param>
        /// <param name="context">контекст валидации</param>
        /// <returns>результат валидации</returns>
        protected abstract bool Validate(SqlConnection connection, ValidationConditionContext context);
    }
}
