using System;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Интерфейс валидатора объектов
    /// </summary>
    public interface IObjectValidator
    {
        /// <summary>
        /// Валидировать поля объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="result">результаты валидации</param>
        void Validate(Object obj, ValidationContext result);
    }
}
