namespace COTL_API.Saves;

internal class ApiData
{
    private string[] RandomEasteregg =
        { "Rawr~", "Hello world!", "Another day another unstable API! Hello!", "Proud bug maker!" };

    public string OwO => RandomEasteregg[UnityEngine.Random.Range(0, RandomEasteregg.Length)];

    public ObjectDictionary EnumData { get; } = new();
}