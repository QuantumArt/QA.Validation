using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Ошибки, связанные с валидатором
    /// </summary>
    [Serializable]
    public sealed class XamlValidatorException : Exception
    {
        /// <summary>
        /// Причина возникновения ошибки
        /// </summary>
        public ValidatorErrorReason Reason { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public XamlValidatorException() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        public XamlValidatorException(string message) : this(ValidatorErrorReason.General, message) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        public XamlValidatorException(ValidatorErrorReason reason, string message)
            : base(message)
        {
            Reason = reason;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public XamlValidatorException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        public XamlValidatorException(ValidatorErrorReason reason, string message, Exception inner)
            : base(message, inner)
        {
            Reason = reason;
        }

        /// <summary>
        /// When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ErrorReason", this.Reason, typeof(int));

            base.GetObjectData(info, context);
        }

        private XamlValidatorException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            this.Reason = (ValidatorErrorReason)info.GetInt32("ErrorReason");
        }

        #region Nested types

        /// <summary>
        /// Причина возникновения ошибки XamlValidatorException
        /// </summary>
        public enum ValidatorErrorReason
        {
            /// <summary>
            /// Причина ошибки не указана
            /// </summary>
            General = 0,

            /// <summary>
            /// Текст валидатора некорректен (ошибка разбора)
            /// </summary>
            ParsingError = 1,

            /// <summary>
            /// Текст валидатора корректен, но есть ошибки в настройках некоторых условий или правил
            /// </summary>
            ValidatorCompositionError = 2,

            /// <summary>
            /// Текст валидатора корректен, но есть ошибки в настройках самого валидатора (опсиания полей, уникальность, итд)
            /// </summary>
            ValidatorError = 3,

            /// <summary>
            /// Ошибка вызвана данными формы
            /// </summary>
            ProvidedDataError = 4
        }

        #endregion
    }
}
