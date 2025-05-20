using Application.Abstractions;
using Domain.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CommentHandlerFactory
    {
        private readonly ICommentRepository _repository;

        public CommentHandlerFactory(ICommentRepository repository)
        {
            _repository = repository;
        }

        public ICommentHandler CreateCommentProcessingChain(int articleId, string username)
        {
            // Создаем обработчики
            var spamFilter = new SpamFilter();
            var profanityFilter = new ProfanityFilter();
            var markdownFormatter = new MarkdownFormatter();
            var commentSaver = new CommentSaver(_repository, articleId, username);

            // Связываем их в цепочку
            spamFilter
                .SetNext(profanityFilter)
                .SetNext(markdownFormatter)
                .SetNext(commentSaver);

            // Возвращаем начало цепочки
            return spamFilter;
        }
    }
}
