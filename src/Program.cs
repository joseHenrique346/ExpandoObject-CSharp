using System.Dynamic;

#region ExpandoObject

#region Criação Expando Principal

dynamic expando = new ExpandoObject();
bool success = false;

while (!success)
{
    Console.WriteLine("Vamos criar um objeto? \n");
    Console.Write("Digite o nome de seu objeto: ");
    string objectName = Console.ReadLine();
    if (objectName != null)
    {
        expando.Name = objectName;
        success = true;
    }
}

#endregion

#region Criação Expando Adicionais

success = false;
List<ExpandoObject> additionalExpandoObjects = new List<ExpandoObject>();

while (!success)
{
    Console.WriteLine($"Gostaria de criar mais um objeto antes de criar as propriedades de {expando.Name}? (S/N)");
    string result = Console.ReadLine();
    if (result.ToLower() == "n" | result.ToLowerInvariant() == "não")
        success = true;

    if (result.ToLower() == "s" | result.ToLower() == "sim")
    {
        Console.Clear();
        Console.Write("Digite o nome de seu objeto: ");
        string objectName = Console.ReadLine();
        if (objectName != null)
        {
            dynamic additionalExpando = new ExpandoObject();
            additionalExpando.Name = objectName;
            additionalExpandoObjects.Add(additionalExpando);
        }
    }
}

#endregion

#region Criação Propriedades Expando Adicionais

success = false;
Console.Clear();

foreach (dynamic expandoObject in additionalExpandoObjects)
{
    #region Escolha quantidade de propriedades adicionais

    int propertiesQuantity = ChoosePropertyQuantity(expandoObject);

    success = false;

    #endregion

    if (propertiesQuantity > 1)
    {
        for (int i = 0; i < propertiesQuantity; i++)
        {
            CreateProperty(expandoObject, additionalExpandoObjects);
            Console.Clear();
            success = false;
        }
    }
}

#region ChoosePropertyQuantity

static int ChoosePropertyQuantity(dynamic expandoObject)
{
    int propertiesQuantity = default;
    bool success = false;
    while (!success)
    {
        Console.WriteLine($"Quantas propriedades gostaria de adicionar para {expandoObject.Name}?");
        Console.WriteLine("Informe de 1 a 10: ");
        propertiesQuantity = int.TryParse(Console.ReadLine(), out int result) ? result : default;

        success = true;

        if (propertiesQuantity == null || propertiesQuantity <= 0 || propertiesQuantity > 10)
        {
            Console.Clear();
            Console.WriteLine("Informe um valor válido");
            success = false;
        }
    }
    return propertiesQuantity;
}

#endregion

#region CreateProperty

static void CreateProperty(dynamic expandoObject, List<ExpandoObject> additionalExpandoObjects)
{
    bool success = false;
    while (!success)
    {
        Console.Write($"Digite o tipo de sua propriedade para {expandoObject.Name}: ");
        string typeName = Console.ReadLine();
        Type type = AliasHelper.GetAliasType(typeName);
        if (type != null)
        {
            Console.Write($"Digite o nome de sua propriedade para {expandoObject.Name}: ");
            string propertyName = Console.ReadLine();
            if (propertyName != null)
            {
                Console.Write($"Digite o valor de sua propriedade para {expandoObject.Name}: ");
                string propertyValue = Console.ReadLine();

                object typedValue = Convert.ChangeType(propertyValue, type);

                if (typedValue != null)
                {
                    var dict = (IDictionary<string, object>)expandoObject;
                    dict[propertyName] = typedValue;

                    Console.Clear();
                    success = true;
                }
            }
        }
        if (typeName == "ExpandoObject" | typeName == "ExpandoObject()")
        {
            dynamic expando = new ExpandoObject();

            Console.Write("Digite o nome de seu objeto: ");
            success = false;
            while (!success)
            {
                string objectName = Console.ReadLine();
                if (objectName != null)
                {
                    expando.Name = objectName;
                    success = true;
                }
            }

            var dict = (IDictionary<string, object>)expandoObject;
            dict["ForeignKey"] = expando.Name + "Id";
            dict["AdditionalExpandoObject"] = expando;

            success = false;

            int propertyQuantity = ChoosePropertyQuantity(expando);

            if (propertyQuantity > 1 | propertyQuantity < 11)
            for (int i = 0; i < propertyQuantity; i++)
            {
                CreateProperty(expando, additionalExpandoObjects);
                Console.Clear();
                success = true;
            }
        }
    }
    Console.Clear();
}

#endregion

#endregion

#region Print

foreach (dynamic expandoObject in additionalExpandoObjects)
{
    var expandoDict = (IDictionary<string, object>)expandoObject;
    foreach (var dict in expandoDict)
    {
        if (dict.Key == "AdditionalExpandoObject")
        {
            PrintAdditionalExpando(expandoObject, dict.Value);
            continue;
        }
        Console.WriteLine($"{expandoObject.Name}: {dict.Key} = {dict.Value}");
    }
}

void PrintAdditionalExpando(dynamic expandoObject, dynamic childExpandoObject)
{
    foreach (var dict in (IDictionary<string, object>)childExpandoObject)
    {
        if (dict.Key == "AdditionalExpandoObject")
        {
            PrintAdditionalExpando(childExpandoObject.Value, dict.Value);
            continue;
        }

        string formattedName = expandoObject.ForeignKey;
        formattedName = formattedName.Substring(0, formattedName.Length - 2);

        Console.WriteLine($"{formattedName}: {dict.Key} = {dict.Value}");
    }
}

#endregion

#endregion

#region AliasHelper

public static class AliasHelper
{
    private static readonly Dictionary<string, Type> AliasTypes = new Dictionary<string, Type>
    {
        { "string", typeof(string) },
        { "int", typeof(int) },
        { "bool", typeof(bool) },
        { "double", typeof(double) },
        { "decimal", typeof(decimal) },
        { "long", typeof(long) },
        { "short", typeof(short) },
        { "byte", typeof(byte) },
        { "char", typeof(char) },
        { "object", typeof(object) }
    };

    public static Type GetAliasType(string name)
    {
        if (AliasTypes.TryGetValue(name, out var type))
            return type;

        return Type.GetType(name);
    }
}

#endregion