namespace Domain;

public static class DomainUtils
{
    public static object ApplyOperator(string op, object a, object b)
    {
        switch (op)
        {
            case "=":
                return b;
            case "+":
                return a switch
                {
                    int aInt when b is int bInt => aInt + bInt,
                    double aDouble when b is double bDouble => aDouble + bDouble,
                    string aString when b is string bString => aString + bString,
                    _ => throw new ArgumentException("Wrong types for plus operator"),
                };
            case "-":
                return a switch
                {
                    int aInt when b is int bInt => aInt - bInt,
                    double aDouble when b is double bDouble => aDouble - bDouble,
                    _ => throw new ArgumentException("Wrong types for minus operator"),
                };
            default:
                throw new ArgumentException("Wrong operator");
        }
    }
}
