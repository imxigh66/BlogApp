using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class SpamFilter : BaseCommentHandler
    {
        private readonly string[] _spamKeywords = { "купить", "скидка", "реклама", "заработок", "казино", "выигрыш" };
        private readonly Regex _urlPattern = new Regex(@"https?://\S+", RegexOptions.Compiled);

        public override string Handle(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                return base.Handle(comment);

            // Проверка на большое количество URL-ов
            var urlMatches = _urlPattern.Matches(comment);
            if (urlMatches.Count > 2)
            {
                throw new InvalidOperationException("Комментарий содержит слишком много ссылок и выглядит как спам");
            }

            // Проверка на спам-ключевые слова
            int spamWordsCount = 0;
            foreach (var keyword in _spamKeywords)
            {
                if (comment.ToLower().Contains(keyword.ToLower()))
                {
                    spamWordsCount++;
                }
            }

            if (spamWordsCount >= 2)
            {
                throw new InvalidOperationException("Комментарий содержит подозрительные ключевые слова и выглядит как спам");
            }

            // Если проверки пройдены, передаем комментарий следующему обработчику
            return base.Handle(comment);
        }
    }
}
