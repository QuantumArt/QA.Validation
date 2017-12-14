using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Validation.Xaml.Tests.Util
{
    public static class ValidatorConstants
    {
        public const string Basic_Object = "QA.Validation.Xaml.Tests.Examples.Basic_Object.xaml";
        public const string Advanced_Object = "QA.Validation.Xaml.Tests.Examples.Advanced_Object.xaml";
        public const string Advanced_Dictionary = "QA.Validation.Xaml.Tests.Examples.Advanced_Dictionary.xaml";
        public const string ArrayType_Dictionary = "QA.Validation.Xaml.Tests.Examples.ArrayType_Dictionary.xaml";
        public const string Advanced_ArrayType_Dictionary = "QA.Validation.Xaml.Tests.Examples.Advanced_ArrayType_Dictionary.xaml";
        public const string Basic_Dictionary = "QA.Validation.Xaml.Tests.Examples.Basic_Dictionary.xaml";
        public const string Alias_Dictionary = "QA.Validation.Xaml.Tests.Examples.Alias_Dictionary.xaml";
        public const string Alias_Advanced = "QA.Validation.Xaml.Tests.Examples.Alias_Advanced.xaml";

        // Составные валидаторы

        public const string Validator_Part1 = "QA.Validation.Xaml.Tests.Examples.Validator_Part1.xaml";
        public const string Validator_Part2 = "QA.Validation.Xaml.Tests.Examples.Validator_Part2.xaml";
        public const string Validator_Part3 = "QA.Validation.Xaml.Tests.Examples.Validator_Part3.xaml";

        public const string ValueProvider = "QA.Validation.Xaml.Tests.Examples.ValueProvider.xaml";

        // Expressions
        public const string Basic_Expressions = "QA.Validation.Xaml.Tests.Examples.Basic_Expressions.xaml";
        public const string Basic_Lambdas = "QA.Validation.Xaml.Tests.Examples.Basic_Lambdas.xaml";

        // Воспроизведение багов
        public static class Issues
        {
            /// <summary>
            /// Отсутствует приведение типов при проверке Equals
            /// </summary>
            public const string Equals_Converter_Boolean = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Equals_Converter_Boolean.xaml";
            public const string Equals_Converter_Int32 = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Equals_Converter_Int32.xaml";
            public const string GreaterThan = "QA.Validation.Xaml.Tests.Issues.Issue_0001.GreaterThan.xaml";
            public const string Length_MinLength = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Length_MinLength.xaml";
            public const string Length_Lists = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Length_Lists.xaml";
            public const string GreaterThan_Two_Properties = "QA.Validation.Xaml.Tests.Issues.Issue_0001.GreaterThan_Two_Properties.xaml";

            //Length_Lists_MinLength
            public const string Length_Lists_MinLength = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Length_Lists_MinLength.xaml";
            public const string Length_Lists_MaxLength = "QA.Validation.Xaml.Tests.Issues.Issue_0001.Length_Lists_MaxLength.xaml";

            public const string Uses_DynamicResource = "QA.Validation.Xaml.Tests.Issues.Issue_0002.Uses_DynamicResource.xaml";
            public const string MatchesForEachLine = "QA.Validation.Xaml.Tests.Issues.Issue_0002.MatchesForEachLine.xaml";
            public const string MatchesForEachLineMeta = "QA.Validation.Xaml.Tests.Issues.Issue_0002.MatchesForEachLineMeta.xaml";
            public const string Issue_40666 = "QA.Validation.Xaml.Tests.Issues.Issue_40666.xaml";
        }

        public static class ValueArguments
        {
            public const string Example_000 = "QA.Validation.Xaml.Tests.ValueArguments.Example_000.xaml";
            public const string Example_001 = "QA.Validation.Xaml.Tests.ValueArguments.Example_001.xaml";
            public const string Example_002 = "QA.Validation.Xaml.Tests.ValueArguments.Example_002.xaml";
            public const string Example_003 = "QA.Validation.Xaml.Tests.ValueArguments.Example_003.xaml";            
        }
    }
}
