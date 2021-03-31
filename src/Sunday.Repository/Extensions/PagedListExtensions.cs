using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sunday.Repository.Extensions
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// PagedList
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageIndex">1为起始页</param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        public static async Task<Page<T>> ToPagedListAsync<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            int realIndex = pageIndex - 1;
            int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await query.Skip(realIndex * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken)
                                   .ConfigureAwait(false);
            return new Page<T>(items, pageIndex, pageSize, count);
        }

        public static Page<T> ToPagedList<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize)
        {
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            int realIndex = pageIndex - 1;
            int count = query.Count();
            var items = query.Skip(realIndex * pageSize)
                             .Take(pageSize)
                             .ToList();
            return new Page<T>(items, pageIndex, pageSize, count);
        }
    }
}