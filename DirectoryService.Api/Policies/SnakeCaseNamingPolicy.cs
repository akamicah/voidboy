using System.Text.Json;

namespace DirectoryService.Api.Policies;

/// <summary>
/// Unless explicitly defined in the DTO/Model, Pascal fields will be
/// converted to snake_case.
/// </summary>
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        static IEnumerable<char> ToSnakeCase(CharEnumerator e)
        {
            if (!e.MoveNext()) yield break;
            yield return char.ToLower(e.Current);
            while (e.MoveNext())
            {
                if (char.IsUpper(e.Current))
                {
                    yield return '_';
                    yield return char.ToLower(e.Current);
                }
                else
                {
                    yield return e.Current;
                }
            }
        }

        return new string(ToSnakeCase(name.GetEnumerator()).ToArray());
    }
}
