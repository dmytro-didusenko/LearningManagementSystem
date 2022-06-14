using System.ComponentModel;
using AutoMapper;

namespace LearningManagementSystem.Domain.Extensions
{
    public static class Extensions
    {
        public static (int, int) ToTuple(this string data, string splitter)
        {
            var splitted = data.Split(splitter);
            if (!int.TryParse(splitted[0], out var l) || !int.TryParse(splitted[1], out var r))
            {
                throw new Exception("Cannot convert string to tuple<int, int>! Invalid data");
            }
            return (l, r);
        }

        public static IMappingExpression<TSource, TDestination> MapStringInTuple<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression, string splitter)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                if (property is string)
                {
                    expression.ForMember(property.Name, opt =>
                       opt.MapFrom(f => f.ToString().ToTuple(splitter)));
                }
            }
            return expression;
        }
    }
}
