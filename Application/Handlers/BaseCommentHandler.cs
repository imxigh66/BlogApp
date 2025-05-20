using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public abstract class BaseCommentHandler : ICommentHandler
    {
        protected ICommentHandler _nextHandler;

        public ICommentHandler SetNext(ICommentHandler handler)
        {
            _nextHandler = handler;
            return handler; // Возвращаем handler для создания цепочки
        }

        // Шаблонный метод для обработки
        public virtual string Handle(string comment)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(comment);
            }

            return comment; // Если нет следующего обработчика, просто возвращаем комментарий
        }
    }
}
