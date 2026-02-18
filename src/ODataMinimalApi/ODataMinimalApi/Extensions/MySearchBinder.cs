using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;
using ODataMinimalApi.Models;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace ODataMinimalApi.Extensions
{
    public class MySearchBinder : QueryBinder, ISearchBinder
    {
        public Expression BindSearch(SearchClause searchClause, QueryBinderContext context)
        {
            SearchTermNode searchTermNode = searchClause.Expression as SearchTermNode;
            if (searchTermNode != null)
            {
                string searchTerm = searchTermNode.Text;

                Expression<Func<Customer, bool>> exp = p => p.Name.Contains(searchTerm.ToLowerInvariant());
                return exp;

                //// Here we only support searching on Name and Email for demo purpose.
                //return Expression.OrElse(
                //    Expression.Call(
                //        Expression.Property(context.CurrentParameter, "Name"),
                //        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                //        Expression.Constant(searchTerm)),
                //    Expression.Call(
                //        Expression.Property(Expression.Property(context.CurrentParameter, "Info"), "Email"),
                //        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                //        Expression.Constant(searchTerm)));
            }

            return null;
        }
    }
}
