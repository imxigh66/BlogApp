using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class SortingStrategyFactory
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public SortingStrategyFactory(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public IPostSortingStrategy CreateStrategy(string strategyName)
        {
            return strategyName.ToLower() switch
            {
                "newest" => new NewestStrategy(),
                "popularity" => new PopularityStrategy(),
                "interests" => new UserInterestStrategy(userActivityRepository: _userActivityRepository),
                _ => new NewestStrategy() // по умолчанию сортировка по новизне
            };
        }
    }
}
