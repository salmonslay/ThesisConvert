namespace ThesisConvert;

public class Html
{
    private string _html = "";

    public void AddText(string author, string text)
    {
        if (text != "Image")
            _html += $"<p><b>{author}</b>: {Escape(text)}</p>";
        else
            // highlight images to make them easier to find (& add)
            _html += $"<p style='background-color: salmon;'><b>{author}</b>: {Escape(text)}</p>";
    }

    public void AddHeader(string text)
    {
        _html += $"<h1>{Escape(text)}</h1>";
    }

    public string GetHtml()
    {
        return _html;
    }

    public static string Escape(string s)
    {
        return s.Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }
}