using System;

namespace LinqViaCSharp
{
    public delegate Func<dynamic, dynamic> Boolean(dynamic @true);

    public static partial class ChurchBoolean
    {
        public static readonly Boolean True = @true => @false => @true;
        public static readonly Boolean False = @true => @false => @false;

        public static readonly Func<Boolean, Func<Boolean, Boolean>> And = a => b => (Boolean)a(b)(False);
        public static readonly Func<Boolean, Func<Boolean, Boolean>> Or = a => b => a(True)(b);
        public static readonly Func<Boolean, Boolean> Not = boolean => boolean(False)(True);
        public static readonly Func<Boolean, Func<Boolean, Boolean>> Xor = a => b => a(Not(b))(b);
    }
}
