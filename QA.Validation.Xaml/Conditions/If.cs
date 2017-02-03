using System;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Ветвление ЕСЛИ
    /// </summary>
    [ContentProperty("Condition")]
    public class If : CompositeCondition
    {
        [Obsolete("Use Then")]
        public ValidationCondition True
        {
            get { return Then; }
            set { Then = value; }
        }

        [Obsolete("Use Else")]
        public ValidationCondition False
        {
            get { return Else; }
            set { Else = value; }
        }

        /// <summary>
        /// Выполняется если выполнено условие
        /// </summary>
        public ValidationCondition Then { get; set; }

        /// <summary>
        /// Выполняется если не выполнено условие
        /// </summary>
        public ValidationCondition Else { get; set; }

        public bool DefaultValue { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            bool isValid = (Condition != null && Condition.Execute(context));
            if (isValid)
            {
                if (Then != null)
                {
                    Then.Execute(context);
                }
            }
            else
            {
                if (Else != null)
                {
                    Else.Execute(context);
                }
            }

            return DefaultValue == isValid;
        }
    }
}
