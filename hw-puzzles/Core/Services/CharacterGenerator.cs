#pragma warning disable CS1572, CS1573, CS1591
using System.Linq;
using hwpuzzles.Core.Utils;

namespace hwpuzzles.Core.Services
{
    public class CharacterGenerator : ICharacterGenerator
    {
        static readonly string[] UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => c.ToString()).ToArray();

        public string Generate(params object[] options)
        {
            var idx = UpperCaseLetters.Length.Random();
            var rc = UpperCaseLetters[idx];
            return rc;
        }
    }
}
