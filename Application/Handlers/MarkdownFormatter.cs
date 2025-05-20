using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class MarkdownFormatter : BaseCommentHandler
    {
        public override string Handle(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                return base.Handle(comment);

            // Преобразование Markdown в HTML
            string formatted = comment;

            // Заголовки
            formatted = Regex.Replace(formatted, @"^# (.+)$", "<h1>$1</h1>", RegexOptions.Multiline);
            formatted = Regex.Replace(formatted, @"^## (.+)$", "<h2>$1</h2>", RegexOptions.Multiline);

            // Выделение
            formatted = Regex.Replace(formatted, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            formatted = Regex.Replace(formatted, @"\*(.+?)\*", "<em>$1</em>");

            // Ссылки
            formatted = Regex.Replace(formatted, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");

            // Списки (упрощенная версия)
            formatted = Regex.Replace(formatted, @"^- (.+)$", "<li>$1</li>", RegexOptions.Multiline);

            // Передаем форматированный комментарий следующему обработчику
            return base.Handle(formatted);
        }
    }
}
