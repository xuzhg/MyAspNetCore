using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;
using NewQueryOptionIn8.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace NewQueryOptionIn8.Extensions
{
    public class ProductSaleSearchBinder : QueryBinder, ISearchBinder
    {
        private static readonly Dictionary<BinaryOperatorKind, ExpressionType> BinaryOperatorMapping = new Dictionary<BinaryOperatorKind, ExpressionType>
        {
            { BinaryOperatorKind.And, ExpressionType.AndAlso },
            { BinaryOperatorKind.Or, ExpressionType.OrElse },
        };

        internal static readonly MethodInfo StringEqualsMethodInfo = typeof(string).GetMethod("Equals",
            new[]
            {
                typeof(string),
                typeof(string),
                typeof(StringComparison)
            });

        internal static readonly MethodInfo IntParse = typeof(int).GetMethod("Parse", new[] { typeof(string) });

        private static ISet<string> _categories = new HashSet<string>
        {
            "food", "office", "music"
        };

        private static ISet<string> _colors = new HashSet<string>
        {
            "white", "red", "yellow", "blue", "brown"
        };

        public Expression BindSearch(SearchClause searchClause, QueryBinderContext context)
        {
            SearchTermNode node = searchClause.Expression as SearchTermNode;
            if (node != null)
            {
                // Lambda expression methods:
                if (_categories.Contains(node.Text))
                {
                    // Be noted: ToLowerInvariant is not enabled/supported in EF Core.
                    // Switch to use "ToLower()" should work in EF Core.
                    Expression<Func<Product, bool>> exp = p => p.Category.ToString().ToLowerInvariant() == node.Text.ToLowerInvariant();
                    return exp;
                }
                else if (_colors.Contains(node.Text))
                {
                    Expression<Func<Product, bool>> exp = p => p.Color.ToString().ToLowerInvariant() == node.Text.ToLowerInvariant();
                    return exp;
                }
                else if (context.ElementClrType == typeof(Sale))
                {
                    try
                    {
                        int year = int.Parse(node.Text);
                        Expression<Func<Sale, bool>> exp = s => s.SaleDate.Year == year;
                        return exp;
                    }
                    catch
                    {
                        return Expression.Constant(false, typeof(bool));
                    }
                }

                throw new InvalidOperationException("Unknown search on product or sale!");
            }
            else
            {
                // Linq expression tree methods:
                Expression exp = BindSingleValueNode(searchClause.Expression, context);

                LambdaExpression lambdaExp = Expression.Lambda(exp, context.CurrentParameter);

                return lambdaExp;
            }
        }

        public override Expression BindSingleValueNode(SingleValueNode node, QueryBinderContext context)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.BinaryOperator:
                    return BindBinaryOperatorNode(node as BinaryOperatorNode, context);

                case QueryNodeKind.SearchTerm:
                    return BindSearchTerm(node as SearchTermNode, context);

                case QueryNodeKind.UnaryOperator:
                    return BindUnaryOperatorNode(node as UnaryOperatorNode, context);
            }

            return null;
        }

        public override Expression BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode, QueryBinderContext context)
        {
            Expression left = Bind(binaryOperatorNode.Left, context);

            Expression right = Bind(binaryOperatorNode.Right, context);

            if (BinaryOperatorMapping.TryGetValue(binaryOperatorNode.OperatorKind, out ExpressionType binaryExpressionType))
            {
                return Expression.MakeBinary(binaryExpressionType, left, right);
            }

            throw new NotImplementedException($"Binary operator '{binaryOperatorNode.OperatorKind}' is not supported!");
        }

        public Expression BindSearchTerm(SearchTermNode node, QueryBinderContext context)
        {
            // Source is from context;
            Expression source = context.CurrentParameter;

            string text = node.Text.ToLowerInvariant();
            if (_categories.Contains(text))
            {
                // $it.Category
                Expression categoryProperty = Expression.Property(source, "Category");

                // $it.Category.ToString()
                Expression categoryPropertyString = Expression.Call(categoryProperty, "ToString", typeArguments: null, arguments: null);

                // string.Equals($it.Category.ToString(), text, StringComparison.OrdinalIgnoreCase);
                return Expression.Call(null, StringEqualsMethodInfo,
                    categoryPropertyString, Expression.Constant(text, typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison)));
            }
            else if (_colors.Contains(text))
            {
                // $it.Color
                Expression colorProperty = Expression.Property(source, "Color");

                // $it.Color.ToString()
                Expression colorPropertyString = Expression.Call(colorProperty, "ToString", typeArguments: null, arguments: null);

                // string.Equals($it.Color.ToString(), text, StringComparison.OrdinalIgnoreCase);
                return Expression.Call(null, StringEqualsMethodInfo,
                    colorPropertyString, Expression.Constant(text, typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison)));
            }
            else if (context.ElementClrType == typeof(Sale))
            {
                int year;
                try
                {
                    year = int.Parse(text);
                }
                catch
                {
                    throw new InvalidOperationException($"Cannot convert '{text}' to int!");
                }

                // $it.SaleDate
                Expression saleDateProperty = Expression.Property(source, "SaleDate");

                // $it.SaleDate.Year
                Expression yearProperty = Expression.Property(saleDateProperty, "Year");

                return Expression.MakeBinary(ExpressionType.Equal, yearProperty, Expression.Constant(year, typeof(int)));
            }
            else
            {
                return Expression.Constant(false, typeof(bool));
            }
        }
    }
}
