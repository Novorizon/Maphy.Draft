using Microsoft.CodeAnalysis;
using Unity.Entities.SourceGen.SystemGeneratorCommon;

namespace Unity.Entities.SourceGen.SystemAPIQueryBuilder
{
   public static class SystemAPIQueryBuilderErrors
    {
        private const string ErrorTitle = "SystemAPIQueryBuilderError";

        public static void SGQB001(SystemDescription systemDescription, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGQB001),
                ErrorTitle,
                "`SystemAPI.QueryBuilder().WithOptions()` should only be called once per query. Subsequent calls will override previous options, rather than adding to them. " +
                "Use the bitwise OR operator '|' to combine multiple options.",
                errorLocation);
        }
    }
}
