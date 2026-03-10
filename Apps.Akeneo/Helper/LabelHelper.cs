namespace Apps.Akeneo.Helper;

public static class LabelHelper
{
    public static Dictionary<string, string> GenerateLabelsBody(List<string> locales, List<string> values)
    {
        var labelsDict = new Dictionary<string, string>();

        for (int i = 0; i < locales.Count; i++)
        {
            string locale = locales[i];
            string value = values[i];
            labelsDict.Add(locale, value);
        }

        return labelsDict;
    }
}
