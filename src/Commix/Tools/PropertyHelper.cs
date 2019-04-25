using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Commix.Tools
{
    internal static class PropertyHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression) body = ((LambdaExpression) body).Body;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo) ((MemberExpression) body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}