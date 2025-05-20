using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class ProfanityFilter : BaseCommentHandler
    {
        private readonly string[] _badWords = { "блин", "черт", "дурак" }; // Замените на реальный список

        public override string Handle(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                return base.Handle(comment);

            string filteredComment = comment;

            // Заменяем нецензурные слова на звездочки
            foreach (var word in _badWords)
            {
                // Используем регулярное выражение для поиска целых слов
                string pattern = $@"\b{word}\b";
                string replacement = new string('*', word.Length);
                filteredComment = Regex.Replace(filteredComment, pattern, replacement, RegexOptions.IgnoreCase);
            }

            // Передаем отфильтрованный комментарий следующему в цепочке
            return base.Handle(filteredComment);
        }
    }
}
