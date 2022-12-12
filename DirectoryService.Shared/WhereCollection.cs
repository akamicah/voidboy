namespace DirectoryService.Shared;

public class WhereCollection
{
    private readonly Dictionary<string, List<object>> _conditions;

    public WhereCollection()
    {
        _conditions = new Dictionary<string, List<object>>();
    }

    public void Add(string column, object value)
    {
        if (_conditions.ContainsKey(column.ToLower()))
        {
            if(!_conditions[column.ToLower()].Contains(value))
                _conditions[column.ToLower()].Add(value);    
        }
        else
        {
            _conditions.Add(column.ToLower(), new List<object>()
            {
                value
            });
        }
    }

    public Dictionary<string, List<object>> ToDictionary()
    {
        return _conditions;
    }
}