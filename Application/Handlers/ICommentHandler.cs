using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public interface ICommentHandler
    {
        // Ссылка на следующий обработчик в цепочке
        ICommentHandler SetNext(ICommentHandler handler);

        // Метод для обработки комментария
        string Handle(string comment);
    }
}
