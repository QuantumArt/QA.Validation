using System;
using System.Linq;
using System.Text.RegularExpressions;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Условие соответствия регилярному выражению.
    /// </summary>
    [ContentProperty("Expression")]
    public class MatchesForEachLine : Matches
    {
        /// <summary>
        /// Текст регулярного выражения
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Опции регулярного выражения
        /// </summary>
        public RegexOptions RegexOptions { get; set; }

        public bool Trim { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);
            bool isValid = true;
            if (value1 is string)
            {
                Regex regex = new Regex(Expression, RegexOptions);

                foreach (var line in ((string)value1)
                    .Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Trim ? x.Trim() : x)
                    .ToArray())
                {
                    bool isLineValid = regex.IsMatch(line);
                    isValid &= isLineValid;

                    if (AddDiagnosticsSummary && !isLineValid)
                    {
                        context.Messages.Add(string.Format("Diagnostics from {0} problem with line: {1}", this, line));
                    }
                }

                return isValid;
            }

            throw GetNotSupportedException(value1);
        }
    }
}
