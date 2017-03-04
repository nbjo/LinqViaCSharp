namespace LinqViaCSharp
{
    public static class BooleanExtensions
    {
        public static Boolean And(this Boolean a, Boolean b) => ChurchBoolean.And(a)(b);

        public static Boolean Or(this Boolean a, Boolean b) => ChurchBoolean.Or(a)(b);

        public static Boolean Not(this Boolean a) => ChurchBoolean.Not(a);

        public static Boolean Xor(this Boolean a, Boolean b) => ChurchBoolean.Xor(a)(b);
    }
}
