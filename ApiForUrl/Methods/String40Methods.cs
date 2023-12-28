

namespace ApiForUrl.Methods;

internal static class String40Methods
{
    public static bool IsNullOrWhiteSpace(string value)
    {
        if (value == null)
        {
            return true;
        }

        for (int i = 0; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static string ReverseString(string str)
    {
        char[] array = str.ToCharArray();
        char[] array2 = new char[array.Length];
        int num = 0;
        int num2 = str.Length - 1;
        while (num < str.Length)
        {
            array2[num] = array[num2];
            num++;
            num2--;
        }

        return new string(array2);
    }

    public static bool IsAllDigit(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsDigit(str[i]))
            {
                return false;
            }
        }

        return true;
    }
}
