using System;

namespace QA.Validation.Xaml.Tests.Model
{
    public class Person
    {
        public int? Age { get; set; }
        public string Name { get; set; }
        public string DuplicateName { get; set; }
        public string Passport { get; set; }
        public DateTime? Date { get; set; }
    }
}
