

namespace LearningManagementSystem.Domain.Validators
{
    public static class ValidatorHelper
    {
        public static bool OnlyCharacters(string property)
        {
            return property.All(char.IsLetter);
        }

        public static string OnlyCharactersError { get; } = "'{PropertyName}' should contains only characters";
    }
}
