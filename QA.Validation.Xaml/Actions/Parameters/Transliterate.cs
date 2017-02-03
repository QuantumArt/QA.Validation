using System.Collections.Generic;
using System.Linq;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Транслитирирование текстового значения
    /// </summary>
    public class Transliterate : TextActionParameter
    {
        private Dictionary<string, string> DefaultMappings;
        public Dictionary<string, string> Mappings { get; set; }

        public Transliterate()
        {
            Mappings = new Dictionary<string, string>();
            DefaultMappings = new Dictionary<string, string>() 
            {
                // расширенные правила
                {"ья","ia"}, {"ий","iy"}, {"ый","iy"},
                // стандартная таблица
                {"а","a"},  {"А","A"},
                {"б","b"},  {"Б","B"},
                {"в","v"},  {"В","V"},
                {"г","g"},  {"Г","G"},
                {"д","d"},  {"Д","D"},
                {"е","e"},  {"Е","E"},
                {"ё","yo"},  {"Ё","Yo"},
                {"ж","zh"},  {"Ж","Zh"},
                {"з","z"},  {"З","Z"},
                {"и","i"},  {"И","I"},
                {"й","y"},  {"Й","Y"},
                {"к","k"},  {"К","K"},
                {"л","l"},  {"Л","L"},
                {"м","m"},  {"М","M"},
                {"н","n"},  {"Н","N"},
                {"о","o"},  {"О","O"},
                {"п","p"},  {"П","P"},
                {"р","r"},  {"Р","R"},
                {"с","s"},  {"С","S"},
                {"т","t"},  {"Т","T"},
                {"у","u"},  {"У","U"},
                {"ф","f"},  {"Ф","F"},
                {"х","kh"},  {"Х","Kh"},
                {"ц","ts"},  {"Ц","Ts"},
                {"ч","ch"},  {"Ч","Ch"},
                {"ш","sh"},  {"Ш","Sh"},
                {"щ","shch"},  {"Щ","Shch"},
                {"ъ","'"},  {"Ъ","'"},
                {"ы","y"},  {"Ы","Y"},
                {"ь","y"},  {"Ь","'"},
                {"э","e"},  {"Э","e"},
                {"ю","yu"},  {"Ю","Yu"},
                {"я","ya"},  {"Я","Ya"},
            };
        }

        protected override object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, string receivedValue)
        {
            if (receivedValue == null)
                return null;

            foreach (var mapping in DefaultMappings
                .Select(x => Mappings.ContainsKey(x.Key) ? Mappings.First(y => y.Key == x.Key) : x))
            {
                receivedValue = receivedValue.Replace(mapping.Key, mapping.Value);
            }

            return receivedValue;
        }
    }
}
